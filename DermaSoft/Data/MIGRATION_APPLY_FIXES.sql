-- ═══════════════════════════════════════════════════════════════
-- MIGRATION: ÁP DỤNG CÁC SỬA LỖI SQL LÊN DB HIỆN TẠI
-- ═══════════════════════════════════════════════════════════════
-- Ngày: 2026-04
-- Mục đích: Chạy file này TRÊN DB ĐANG CÓ DỮ LIỆU để cập nhật
--           các sửa lỗi mà không cần DROP/CREATE lại DB.
--
-- SAU KHI CHẠY FILE NÀY, tiếp tục chạy theo thứ tự:
--   1. 02_StoredProcedures.sql  (DROP IF EXISTS + CREATE — an toàn)
--   2. 03_Triggers.sql          (DROP IF EXISTS + CREATE — an toàn)
--   3. 04_Views.sql             (DROP IF EXISTS + CREATE — an toàn)
--
-- LƯU Ý: File 01_Tables_Data.sql và 05_Constraints.sql 
--         CHỈ dùng cho FRESH DEPLOY (DB mới hoàn toàn).
-- ═══════════════════════════════════════════════════════════════

USE DERMASOFT;
GO

PRINT N'========== BẮT ĐẦU MIGRATION =========='
GO

-- ─────────────────────────────────────────────────────────────
-- 1. SỬA LichHen.TrangThai DEFAULT 1 → 0
--    (Lịch hẹn mới phải ở trạng thái "Chờ xác nhận" = 0)
-- ─────────────────────────────────────────────────────────────

-- Tìm và xóa constraint DEFAULT cũ
DECLARE @dfName NVARCHAR(200);
SELECT @dfName = dc.name
FROM sys.default_constraints dc
JOIN sys.columns c ON dc.parent_column_id = c.column_id 
                   AND dc.parent_object_id = c.object_id
WHERE dc.parent_object_id = OBJECT_ID('LichHen')
  AND c.name = 'TrangThai';

IF @dfName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE LichHen DROP CONSTRAINT ' + @dfName);
    PRINT N'✓ Đã xóa constraint DEFAULT cũ: ' + @dfName;
END

-- Thêm constraint DEFAULT mới = 0
ALTER TABLE LichHen ADD CONSTRAINT DF_LichHen_TrangThai DEFAULT 0 FOR TrangThai;
PRINT N'✓ LichHen.TrangThai DEFAULT = 0 (Chờ xác nhận)';
GO

-- ─────────────────────────────────────────────────────────────
-- 2. THÊM CỘT IsDeleted VÀO BẢNG Thuoc (nếu chưa có)
-- ─────────────────────────────────────────────────────────────

IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('Thuoc') AND name = 'IsDeleted'
)
BEGIN
    ALTER TABLE Thuoc ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT N'✓ Đã thêm cột IsDeleted vào bảng Thuoc';
END
ELSE
    PRINT N'⊘ Cột IsDeleted đã tồn tại trong Thuoc — bỏ qua';
GO

-- ─────────────────────────────────────────────────────────────
-- 3. TẠO BẢNG CaiDatHeThong (nếu chưa có)
-- ─────────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'CaiDatHeThong')
BEGIN
    CREATE TABLE CaiDatHeThong (
        Khoa   VARCHAR(50)    PRIMARY KEY,
        GiaTri NVARCHAR(500)  NOT NULL,
        MoTa   NVARCHAR(200)  NULL
    );

    INSERT INTO CaiDatHeThong (Khoa, GiaTri, MoTa) VALUES
    ('NGUONG_THAP',       '10',         N'Ngưỡng tồn kho mức Thấp'),
    ('NGUONG_NGUY_HIEM',  '3',          N'Ngưỡng tồn kho mức Nguy hiểm'),
    ('MK_MAC_DINH',       'Temp@2026',  N'Mật khẩu mặc định khi tạo/reset nhân viên');

    PRINT N'✓ Đã tạo bảng CaiDatHeThong + seed data';
END
ELSE
    PRINT N'⊘ Bảng CaiDatHeThong đã tồn tại — bỏ qua';
GO

-- ─────────────────────────────────────────────────────────────
-- 4. CẬP NHẬT LichHen.MaNguoiDung → CHO PHÉP NULL + ON DELETE SET NULL
--    (Website đặt lịch không cần gán bác sĩ ngay)
-- ─────────────────────────────────────────────────────────────

-- 4a. Xóa FK cũ nếu có
DECLARE @fkName2 NVARCHAR(200);
SELECT @fkName2 = fk.name
FROM sys.foreign_keys fk
JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
JOIN sys.columns c ON fkc.parent_column_id = c.column_id 
                   AND fkc.parent_object_id = c.object_id
WHERE fk.parent_object_id = OBJECT_ID('LichHen')
  AND c.name = 'MaNguoiDung';

IF @fkName2 IS NOT NULL
BEGIN
    EXEC('ALTER TABLE LichHen DROP CONSTRAINT ' + @fkName2);
    PRINT N'✓ Đã xóa FK cũ: ' + @fkName2;
END
GO

-- 4b. Sửa cột cho phép NULL
ALTER TABLE LichHen ALTER COLUMN MaNguoiDung INT NULL;
PRINT N'✓ LichHen.MaNguoiDung → cho phép NULL';
GO

-- 4c. Thêm lại FK với ON DELETE SET NULL
IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys 
    WHERE parent_object_id = OBJECT_ID('LichHen') 
      AND name = 'FK_LichHen_NguoiDung'
)
BEGIN
    ALTER TABLE LichHen
    ADD CONSTRAINT FK_LichHen_NguoiDung
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
    ON DELETE SET NULL;
    PRINT N'✓ Đã thêm FK mới FK_LichHen_NguoiDung (ON DELETE SET NULL)';
END
GO

-- ─────────────────────────────────────────────────────────────
-- 5. THÊM CỘT SoDienThoaiKhach VÀO LichHen (nếu chưa có)
-- ─────────────────────────────────────────────────────────────

IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('LichHen') AND name = 'SoDienThoaiKhach'
)
BEGIN
    ALTER TABLE LichHen ADD SoDienThoaiKhach VARCHAR(15) NULL;
    PRINT N'✓ Đã thêm cột SoDienThoaiKhach vào LichHen';
END
ELSE
    PRINT N'⊘ Cột SoDienThoaiKhach đã tồn tại — bỏ qua';
GO

-- ─────────────────────────────────────────────────────────────
-- 6. THÊM CỘT AnhDaiDien VÀO NguoiDung (nếu chưa có)
-- ─────────────────────────────────────────────────────────────

IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('NguoiDung') AND name = 'AnhDaiDien'
)
BEGIN
    ALTER TABLE NguoiDung ADD AnhDaiDien NVARCHAR(255) NULL;
    PRINT N'✓ Đã thêm cột AnhDaiDien vào NguoiDung';
END
ELSE
    PRINT N'⊘ Cột AnhDaiDien đã tồn tại — bỏ qua';
GO

-- ─────────────────────────────────────────────────────────────
-- 7. CẬP NHẬT KHUYẾN MÃI HẠNG THÀNH VIÊN (nếu chưa có cột)
-- ─────────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HangThanhVien') AND name = 'PhanTramGiamDuocPham')
    ALTER TABLE HangThanhVien ADD PhanTramGiamDuocPham DECIMAL(5,2) DEFAULT 0;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HangThanhVien') AND name = 'PhanTramGiamTongHD')
    ALTER TABLE HangThanhVien ADD PhanTramGiamTongHD DECIMAL(5,2) DEFAULT 0;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HangThanhVien') AND name = 'GiamGiaCodinh')
    ALTER TABLE HangThanhVien ADD GiamGiaCodinh INT DEFAULT 0;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HangThanhVien') AND name = 'GhiChuKhuyenMai')
    ALTER TABLE HangThanhVien ADD GhiChuKhuyenMai NVARCHAR(255) NULL;
GO

UPDATE HangThanhVien SET PhanTramGiamDuocPham = 0,  PhanTramGiamTongHD = 0,  GiamGiaCodinh = 100000, GhiChuKhuyenMai = N'Giảm 100.000đ cho hóa đơn đầu tiên'                              WHERE TenHang = N'Thành Viên Đỏ';
UPDATE HangThanhVien SET PhanTramGiamDuocPham = 5,  PhanTramGiamTongHD = 0,  GiamGiaCodinh = 0,      GhiChuKhuyenMai = N'Giảm 5% sản phẩm dược phẩm (không áp dụng dịch vụ chăm sóc da)'  WHERE TenHang = N'Thành Viên Bạc';
UPDATE HangThanhVien SET PhanTramGiamDuocPham = 10, PhanTramGiamTongHD = 0,  GiamGiaCodinh = 0,      GhiChuKhuyenMai = N'Giảm 10% sản phẩm dược phẩm (không áp dụng dịch vụ chăm sóc da)' WHERE TenHang = N'Thành Viên Vàng';
UPDATE HangThanhVien SET PhanTramGiamDuocPham = 0,  PhanTramGiamTongHD = 10, GiamGiaCodinh = 0,      GhiChuKhuyenMai = N'Giảm 10% tổng hóa đơn'                                          WHERE TenHang = N'Thành Viên Kim Cương';
PRINT N'✓ Đã cập nhật khuyến mãi 4 hạng thành viên';
GO

-- ═══════════════════════════════════════════════════════════════
PRINT N''
PRINT N'========================================='
PRINT N'  MIGRATION HOÀN TẤT'
PRINT N'========================================='
PRINT N''
PRINT N'BƯỚC TIẾP THEO — Chạy theo thứ tự:'
PRINT N'  1. 02_StoredProcedures.sql'
PRINT N'  2. 03_Triggers.sql'
PRINT N'  3. 04_Views.sql'
PRINT N'  (05_Constraints.sql — tùy chọn, đã có IF NOT EXISTS)'
PRINT N''
GO
