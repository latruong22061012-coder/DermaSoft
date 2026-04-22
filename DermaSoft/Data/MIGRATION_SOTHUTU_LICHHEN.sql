-- ═══════════════════════════════════════════════════════════════════════
-- 🔢 MIGRATION: Thêm Số Thứ Tự Tự Động cho Lịch Hẹn
-- ═══════════════════════════════════════════════════════════════════════
-- Chạy script này trên SSMS để:
--   1. Thêm cột SoThuTu vào bảng LichHen (nullable)
--   2. Tạo trigger tự động gán STT khi lễ tân xác nhận (TrangThai 0→1)
--   3. Cập nhật STT cho lịch hẹn đã xác nhận (backfill)
-- ═══════════════════════════════════════════════════════════════════════

SET NOCOUNT ON;

PRINT '════════════════════════════════════════════════════════════';
PRINT '  🔢 THÊM SỐ THỨ TỰ TỰ ĐỘNG - LỊCH HẸN';
PRINT '  Thời gian: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '════════════════════════════════════════════════════════════';
PRINT '';

-- ── BƯỚC 1: Thêm cột SoThuTu vào bảng LichHen ──
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('LichHen') AND name = 'SoThuTu')
BEGIN
    ALTER TABLE LichHen ADD SoThuTu INT NULL;
    PRINT '  ✅ Đã thêm cột SoThuTu vào bảng LichHen.';
END
ELSE
    PRINT '  ℹ️  Cột SoThuTu đã tồn tại.';

PRINT '';

-- ── BƯỚC 2: Backfill STT cho lịch hẹn đã xác nhận (TrangThai=1) ──
PRINT '--- Gán STT cho lịch hẹn đã xác nhận (theo ngày + bác sĩ + giờ) ---';

WITH CTE AS (
    SELECT 
        MaLichHen,
        ROW_NUMBER() OVER (
            PARTITION BY CAST(ThoiGianHen AS DATE), MaNguoiDung 
            ORDER BY ThoiGianHen ASC
        ) AS STT_Moi
    FROM LichHen
    WHERE TrangThai = 1 -- Đã xác nhận
      AND SoThuTu IS NULL
)
UPDATE LichHen
SET SoThuTu = CTE.STT_Moi
FROM LichHen lh
INNER JOIN CTE ON lh.MaLichHen = CTE.MaLichHen;

PRINT '  ✅ Đã gán STT cho ' + CAST(@@ROWCOUNT AS VARCHAR) + ' lịch hẹn.';
PRINT '';

-- ── BƯỚC 3: Tạo trigger tự động gán STT khi xác nhận ──
IF OBJECT_ID('TRG_LichHen_GanSTT', 'TR') IS NOT NULL
    DROP TRIGGER TRG_LichHen_GanSTT;
GO

PRINT 'Creating TRG_LichHen_GanSTT...';
GO
CREATE TRIGGER TRG_LichHen_GanSTT
ON LichHen
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Chỉ chạy khi TrangThai chuyển từ 0 (Chờ XN) → 1 (Đã XN)
    IF EXISTS (
        SELECT 1 FROM inserted i
        INNER JOIN deleted d ON i.MaLichHen = d.MaLichHen
        WHERE i.TrangThai = 1 AND d.TrangThai = 0
    )
    BEGIN
        -- Gán STT cho lịch hẹn vừa xác nhận
        -- STT = thứ tự theo ThoiGianHen trong cùng ngày + bác sĩ
        UPDATE lh
        SET SoThuTu = seq.STT
        FROM LichHen lh
        INNER JOIN inserted i ON lh.MaLichHen = i.MaLichHen
        INNER JOIN (
            SELECT 
                MaLichHen,
                ROW_NUMBER() OVER (
                    PARTITION BY CAST(ThoiGianHen AS DATE), MaNguoiDung 
                    ORDER BY ThoiGianHen ASC
                ) AS STT
            FROM LichHen
            WHERE TrangThai = 1 -- Chỉ đếm lịch đã xác nhận
              AND CAST(ThoiGianHen AS DATE) = (
                  SELECT CAST(ThoiGianHen AS DATE) FROM inserted WHERE MaLichHen = LichHen.MaLichHen
              )
              AND MaNguoiDung = (
                  SELECT MaNguoiDung FROM inserted WHERE MaLichHen = LichHen.MaLichHen
              )
        ) seq ON lh.MaLichHen = seq.MaLichHen
        WHERE i.TrangThai = 1;
    END
END;
GO

PRINT '  ✅ Đã tạo trigger TRG_LichHen_GanSTT.';
PRINT '';

-- ── BƯỚC 4: Kiểm tra kết quả ──
PRINT '════════════════════════════════════════════════════════════';
PRINT '  📊 KẾT QUẢ';
PRINT '════════════════════════════════════════════════════════════';

SELECT 
    CAST(ThoiGianHen AS DATE) AS Ngay,
    ISNULL(nd.HoTen, N'Chưa phân công') AS BacSi,
    COUNT(*) AS SoLichHen,
    SUM(CASE WHEN SoThuTu IS NOT NULL THEN 1 ELSE 0 END) AS DaCoSTT
FROM LichHen lh
LEFT JOIN NguoiDung nd ON lh.MaNguoiDung = nd.MaNguoiDung
WHERE TrangThai = 1
GROUP BY CAST(ThoiGianHen AS DATE), nd.HoTen
ORDER BY Ngay DESC;

PRINT '';
PRINT '  Hoàn thành: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '════════════════════════════════════════════════════════════';
