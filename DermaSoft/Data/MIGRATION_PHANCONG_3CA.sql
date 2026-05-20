USE DERMASOFT;
GO

-- ═══════════════════════════════════════════════════════════════════════
-- 📅 MIGRATION: Cho phép phân tối đa 3 ca / ngày cho mỗi nhân viên
-- ═══════════════════════════════════════════════════════════════════════
-- Quy định nghiệp vụ mới:
--   • Mỗi ca (Sáng / Chiều / Tối) dài 4 tiếng.
--   • Mỗi nhân viên có thể được phân tối đa 3 ca / ngày (= 12 tiếng).
--   • KHÔNG được phân trùng ca đã phân (UNIQUE theo MaNguoiDung+Ngay+MaCa).
--   • Cập nhật SoGioChuanNgay trong CauHinhLuong: 8 → 12 để
--     khớp khung giờ chuẩn mới và logic tính tăng ca trong BangLuongForm.
-- ═══════════════════════════════════════════════════════════════════════

SET NOCOUNT ON;

PRINT '════════════════════════════════════════════════════════════';
PRINT '  📅 MIGRATION: TỐI ĐA 3 CA / NGÀY / NHÂN VIÊN';
PRINT '════════════════════════════════════════════════════════════';

-- ─────────────────────────────────────────────────────────────
-- 1. Dọn dữ liệu trùng (nếu tồn tại) trước khi tạo UNIQUE
--    Giữ lại bản ghi cũ nhất, xóa các bản ghi (MaNguoiDung, Ngay, MaCa)
--    trùng phát sinh nhiều lần (nếu có).
-- ─────────────────────────────────────────────────────────────
;WITH cte AS (
    SELECT MaPhanCong,
           ROW_NUMBER() OVER (
               PARTITION BY MaNguoiDung, NgayLamViec, MaCa
               ORDER BY MaPhanCong ASC
           ) AS rn
    FROM PhanCongCa
)
DELETE FROM cte WHERE rn > 1;
PRINT N'  ✅ Đã dọn dữ liệu trùng (MaNguoiDung + NgayLamViec + MaCa).';

-- ─────────────────────────────────────────────────────────────
-- 2. Bỏ UNIQUE cũ (nếu có) trên (MaNguoiDung, NgayLamViec)
--    vì ràng buộc cũ chỉ cho 1 ca / ngày.
-- ─────────────────────────────────────────────────────────────
DECLARE @cName SYSNAME;
SELECT TOP 1 @cName = i.name
FROM sys.indexes i
JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE i.object_id = OBJECT_ID('PhanCongCa')
  AND i.is_unique = 1
  AND c.name IN ('MaNguoiDung','NgayLamViec')
GROUP BY i.name, i.index_id
HAVING COUNT(*) = 2
   AND SUM(CASE WHEN c.name = 'MaCa' THEN 1 ELSE 0 END) = 0;

IF @cName IS NOT NULL
BEGIN
    DECLARE @sql NVARCHAR(MAX) = N'ALTER TABLE PhanCongCa DROP CONSTRAINT ' + QUOTENAME(@cName) + N';';
    BEGIN TRY
        EXEC sp_executesql @sql;
        PRINT N'  ✅ Đã bỏ UNIQUE cũ: ' + @cName;
    END TRY
    BEGIN CATCH
        SET @sql = N'DROP INDEX ' + QUOTENAME(@cName) + N' ON PhanCongCa;';
        EXEC sp_executesql @sql;
        PRINT N'  ✅ Đã drop INDEX cũ: ' + @cName;
    END CATCH
END
ELSE
    PRINT N'  ℹ️  Không tìm thấy UNIQUE cũ trên (MaNguoiDung, NgayLamViec).';

-- ─────────────────────────────────────────────────────────────
-- 3. Tạo UNIQUE mới (MaNguoiDung, NgayLamViec, MaCa)
--    Đảm bảo: NV không được phân cùng 1 ca 2 lần / ngày.
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'UQ_PhanCongCa_NV_Ngay_Ca' AND object_id = OBJECT_ID('PhanCongCa')
)
BEGIN
    ALTER TABLE PhanCongCa
        ADD CONSTRAINT UQ_PhanCongCa_NV_Ngay_Ca UNIQUE (MaNguoiDung, NgayLamViec, MaCa);
    PRINT N'  ✅ Đã tạo UNIQUE: UQ_PhanCongCa_NV_Ngay_Ca.';
END
ELSE
    PRINT N'  ℹ️  UNIQUE UQ_PhanCongCa_NV_Ngay_Ca đã tồn tại.';

-- ─────────────────────────────────────────────────────────────
-- 4. CHECK constraint: tối đa 3 ca / NV / ngày (qua trigger)
--    SQL Server không hỗ trợ CHECK đếm trên nhiều dòng → dùng TRIGGER.
-- ─────────────────────────────────────────────────────────────
IF OBJECT_ID('TRG_PhanCongCa_MaxCaTrongNgay', 'TR') IS NOT NULL
    DROP TRIGGER TRG_PhanCongCa_MaxCaTrongNgay;
GO

CREATE TRIGGER TRG_PhanCongCa_MaxCaTrongNgay
ON PhanCongCa
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM PhanCongCa p
        JOIN (
            SELECT MaNguoiDung, NgayLamViec FROM inserted
            UNION
            SELECT MaNguoiDung, NgayLamViec FROM deleted
        ) k ON p.MaNguoiDung = k.MaNguoiDung
           AND p.NgayLamViec = k.NgayLamViec
        GROUP BY p.MaNguoiDung, p.NgayLamViec
        HAVING COUNT(*) > 3
    )
    BEGIN
        RAISERROR (N'Mỗi nhân viên chỉ được phân tối đa 3 ca trong một ngày.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

PRINT N'  ✅ Đã tạo trigger TRG_PhanCongCa_MaxCaTrongNgay (tối đa 3 ca/ngày).';

-- ─────────────────────────────────────────────────────────────
-- 5. Cập nhật SoGioChuanNgay trong CauHinhLuong: 8 → 12
--    (3 ca × 4 tiếng = 12 giờ chuẩn / ngày)
-- ─────────────────────────────────────────────────────────────
UPDATE CauHinhLuong
SET SoGioChuanNgay = 12,
    SoCaChuanNgay  = 3
WHERE SoGioChuanNgay = 8;

PRINT CONCAT(N'  ✅ Đã cập nhật SoGioChuanNgay = 12 cho ', @@ROWCOUNT, N' bản ghi CauHinhLuong.');
GO

PRINT '════════════════════════════════════════════════════════════';
PRINT '  ✅ MIGRATION HOÀN TẤT';
PRINT '════════════════════════════════════════════════════════════';
GO
