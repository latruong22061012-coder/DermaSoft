USE DERMASOFT;
GO

-- ────────────────────────────────────────────────────────────
-- 1. THÊM CỘT AnhDaiDien VÀO BẢNG NguoiDung (Avatar)
-- ────────────────────────────────────────────────────────────
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

-- ────────────────────────────────────────────────────────────
-- 2. SỬA CỘT LichHen.MaNguoiDung → CHO PHÉP NULL
--    (Website đặt lịch không cần gán bác sĩ ngay)
-- ────────────────────────────────────────────────────────────

-- 2a. Xóa FK cũ nếu có (do FK không cho phép ALTER cột)
DECLARE @fkName NVARCHAR(200);
SELECT @fkName = fk.name
FROM sys.foreign_keys fk
JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
JOIN sys.columns c ON fkc.parent_column_id = c.column_id AND fkc.parent_object_id = c.object_id
WHERE fk.parent_object_id = OBJECT_ID('LichHen')
  AND c.name = 'MaNguoiDung';

IF @fkName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE LichHen DROP CONSTRAINT ' + @fkName);
    PRINT N'✓ Đã xóa FK cũ: ' + @fkName;
END
GO

-- 2b. Sửa cột MaNguoiDung cho phép NULL
ALTER TABLE LichHen ALTER COLUMN MaNguoiDung INT NULL;
PRINT N'✓ Đã sửa LichHen.MaNguoiDung → NULL';
GO

-- 2c. Thêm lại FK với ON DELETE SET NULL
IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys 
    WHERE parent_object_id = OBJECT_ID('LichHen') AND name = 'FK_LichHen_NguoiDung'
)
BEGIN
    ALTER TABLE LichHen
    ADD CONSTRAINT FK_LichHen_NguoiDung
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
    ON DELETE SET NULL;
    PRINT N'✓ Đã thêm FK mới FK_LichHen_NguoiDung (ON DELETE SET NULL)';
END
GO

-- ────────────────────────────────────────────────────────────
-- 3. THÊM CỘT SoDienThoaiKhach VÀO BẢNG LichHen
--    (Lưu SĐT khách khi đặt lịch qua website)
-- ────────────────────────────────────────────────────────────
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

-- ────────────────────────────────────────────────────────────
-- 4. CẬP NHẬT DỮ LIỆU HangThanhVien
--    (4 hạng: Đỏ 0đ, Bạc 300đ, Vàng 1000đ, Kim Cương 5000đ)
-- ────────────────────────────────────────────────────────────

-- Xóa dữ liệu cũ rồi insert lại (an toàn hơn)
-- CHỈ chạy nếu bảng HangThanhVien chưa có đúng 4 hạng
IF (SELECT COUNT(*) FROM HangThanhVien) <> 4
   OR NOT EXISTS (SELECT 1 FROM HangThanhVien WHERE TenHang = N'Thành Viên Đỏ')
BEGIN
    -- Kiểm tra xem ThanhVienInfo có FK đang tham chiếu không
    -- Nếu có, cập nhật thay vì xóa
    DELETE FROM HangThanhVien 
    WHERE MaHang NOT IN (SELECT DISTINCT MaHang FROM ThanhVienInfo);
    
    -- Đảm bảo có đủ 4 hạng
    IF NOT EXISTS (SELECT 1 FROM HangThanhVien WHERE TenHang = N'Thành Viên Đỏ')
        INSERT INTO HangThanhVien (TenHang, DiemToiThieu, MauHangHex) VALUES (N'Thành Viên Đỏ', 0, '#FF4C4C');
    
    IF NOT EXISTS (SELECT 1 FROM HangThanhVien WHERE TenHang = N'Thành Viên Bạc')
        INSERT INTO HangThanhVien (TenHang, DiemToiThieu, MauHangHex) VALUES (N'Thành Viên Bạc', 300, '#C0C0C0');
    
    IF NOT EXISTS (SELECT 1 FROM HangThanhVien WHERE TenHang = N'Thành Viên Vàng')
        INSERT INTO HangThanhVien (TenHang, DiemToiThieu, MauHangHex) VALUES (N'Thành Viên Vàng', 1000, '#FFD700');
    
    IF NOT EXISTS (SELECT 1 FROM HangThanhVien WHERE TenHang = N'Thành Viên Kim Cương')
        INSERT INTO HangThanhVien (TenHang, DiemToiThieu, MauHangHex) VALUES (N'Thành Viên Kim Cương', 5000, '#89CFF0');
    
    -- Cập nhật điểm & màu cho hạng đã tồn tại (phòng dữ liệu cũ sai)
    UPDATE HangThanhVien SET DiemToiThieu = 0,    MauHangHex = '#FF4C4C' WHERE TenHang = N'Thành Viên Đỏ';
    UPDATE HangThanhVien SET DiemToiThieu = 300,  MauHangHex = '#C0C0C0' WHERE TenHang = N'Thành Viên Bạc';
    UPDATE HangThanhVien SET DiemToiThieu = 1000, MauHangHex = '#FFD700' WHERE TenHang = N'Thành Viên Vàng';
    UPDATE HangThanhVien SET DiemToiThieu = 5000, MauHangHex = '#89CFF0' WHERE TenHang = N'Thành Viên Kim Cương';

    PRINT N'✓ Đã cập nhật HangThanhVien (4 hạng)';
END
ELSE
    PRINT N'⊘ HangThanhVien đã đúng 4 hạng — bỏ qua';
GO

-- ────────────────────────────────────────────────────────────
-- 5. TẠO STORED PROCEDURE SP_DatLichHen
--    (Đặt lịch hẹn từ website — upsert BenhNhan + tạo LichHen)
-- ────────────────────────────────────────────────────────────
IF OBJECT_ID('SP_DatLichHen', 'P') IS NOT NULL
    DROP PROCEDURE SP_DatLichHen;
GO

CREATE PROCEDURE SP_DatLichHen
    @HoTen       NVARCHAR(100),
    @SoDienThoai VARCHAR(15),
    @ThoiGianHen DATETIME,
    @GhiChu      NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @MaBenhNhan INT;

        -- Upsert BenhNhan theo SĐT
        SELECT @MaBenhNhan = MaBenhNhan
        FROM   BenhNhan
        WHERE  SoDienThoai = @SoDienThoai AND IsDeleted = 0;

        IF @MaBenhNhan IS NULL
        BEGIN
            INSERT INTO BenhNhan (HoTen, SoDienThoai)
            VALUES (@HoTen, @SoDienThoai);
            SET @MaBenhNhan = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            UPDATE BenhNhan SET HoTen = @HoTen WHERE MaBenhNhan = @MaBenhNhan;
        END;

        -- Kiểm tra trùng lịch cùng ngày (TrangThai 1=chờ, 2=xác nhận)
        IF EXISTS (
            SELECT 1 FROM LichHen
            WHERE  MaBenhNhan = @MaBenhNhan
              AND  TrangThai IN (1, 2)
              AND  CAST(ThoiGianHen AS DATE) = CAST(@ThoiGianHen AS DATE)
        )
        BEGIN
            RAISERROR(N'Bạn đã có lịch hẹn trong ngày này, vui lòng chọn ngày khác.', 16, 1);
            RETURN;
        END;

        -- Ghi lịch hẹn mới (MaNguoiDung = NULL → chờ lễ tân phân công)
        INSERT INTO LichHen (MaBenhNhan, MaNguoiDung, ThoiGianHen, GhiChu, TrangThai, SoDienThoaiKhach)
        VALUES (@MaBenhNhan, NULL, @ThoiGianHen, @GhiChu, 1, @SoDienThoai);

        DECLARE @MaLichHen INT = SCOPE_IDENTITY();

        COMMIT TRANSACTION;

        SELECT @MaLichHen AS MaLichHen, @MaBenhNhan AS MaBenhNhan;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        DECLARE @Err NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
    END CATCH
END;
GO

PRINT N'✓ Đã tạo SP_DatLichHen';
GO

-- ────────────────────────────────────────────────────────────
-- 6. CẬP NHẬT CONSTRAINT CHK_TrangThaiLich
--    Trạng thái LichHen: 0=Chờ, 1=Xác nhận, 2=Hoàn thành, 3=Hủy
-- ────────────────────────────────────────────────────────────
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_TrangThaiLich')
BEGIN
    ALTER TABLE LichHen DROP CONSTRAINT CHK_TrangThaiLich;
    PRINT N'✓ Đã xóa constraint CHK_TrangThaiLich cũ';
END
GO

ALTER TABLE LichHen
ADD CONSTRAINT CHK_TrangThaiLich 
CHECK (TrangThai BETWEEN 0 AND 3);
GO
-- 0 = Chờ xác nhận
-- 1 = Đã xác nhận
-- 2 = Hoàn thành
-- 3 = Hủy



-- ────────────────────────────────────────────────────────────
-- 7. THÊM CỘT KHUYẾN MÃI VÀO HangThanhVien
--    Ưu đãi giảm giá theo từng hạng thành viên
-- ────────────────────────────────────────────────────────────

-- 7a. Thêm cột PhanTramGiamDuocPham (% giảm giá dược phẩm)
IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('HangThanhVien') AND name = 'PhanTramGiamDuocPham'
)
BEGIN
    ALTER TABLE HangThanhVien ADD PhanTramGiamDuocPham DECIMAL(5,2) DEFAULT 0;
    PRINT N'✓ Đã thêm cột PhanTramGiamDuocPham';
END
ELSE
    PRINT N'⊘ Cột PhanTramGiamDuocPham đã tồn tại — bỏ qua';
GO

-- 7b. Thêm cột PhanTramGiamTongHD (% giảm giá tổng hóa đơn)
IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('HangThanhVien') AND name = 'PhanTramGiamTongHD'
)
BEGIN
    ALTER TABLE HangThanhVien ADD PhanTramGiamTongHD DECIMAL(5,2) DEFAULT 0;
    PRINT N'✓ Đã thêm cột PhanTramGiamTongHD';
END
ELSE
    PRINT N'⊘ Cột PhanTramGiamTongHD đã tồn tại — bỏ qua';
GO

-- 7c. Thêm cột GiamGiaCodinh (giảm giá cố định VNĐ)
IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('HangThanhVien') AND name = 'GiamGiaCodinh'
)
BEGIN
    ALTER TABLE HangThanhVien ADD GiamGiaCodinh INT DEFAULT 0;
    PRINT N'✓ Đã thêm cột GiamGiaCodinh';
END
ELSE
    PRINT N'⊘ Cột GiamGiaCodinh đã tồn tại — bỏ qua';
GO

-- 7d. Thêm cột GhiChuKhuyenMai (mô tả ưu đãi)
IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('HangThanhVien') AND name = 'GhiChuKhuyenMai'
)
BEGIN
    ALTER TABLE HangThanhVien ADD GhiChuKhuyenMai NVARCHAR(255) NULL;
    PRINT N'✓ Đã thêm cột GhiChuKhuyenMai';
END
ELSE
    PRINT N'⊘ Cột GhiChuKhuyenMai đã tồn tại — bỏ qua';
GO

-- 7e. Cập nhật dữ liệu khuyến mãi theo hạng
UPDATE HangThanhVien SET PhanTramGiamDuocPham = 0,  PhanTramGiamTongHD = 0,  GiamGiaCodinh = 100000, GhiChuKhuyenMai = N'Giảm 100.000đ cho hóa đơn đầu tiên'                              WHERE TenHang = N'Thành Viên Đỏ';
UPDATE HangThanhVien SET PhanTramGiamDuocPham = 5,  PhanTramGiamTongHD = 0,  GiamGiaCodinh = 0,      GhiChuKhuyenMai = N'Giảm 5% sản phẩm dược phẩm (không áp dụng dịch vụ chăm sóc da)'  WHERE TenHang = N'Thành Viên Bạc';
UPDATE HangThanhVien SET PhanTramGiamDuocPham = 10, PhanTramGiamTongHD = 0,  GiamGiaCodinh = 0,      GhiChuKhuyenMai = N'Giảm 10% sản phẩm dược phẩm (không áp dụng dịch vụ chăm sóc da)' WHERE TenHang = N'Thành Viên Vàng';
UPDATE HangThanhVien SET PhanTramGiamDuocPham = 0,  PhanTramGiamTongHD = 10, GiamGiaCodinh = 0,      GhiChuKhuyenMai = N'Giảm 10% tổng hóa đơn'                                          WHERE TenHang = N'Thành Viên Kim Cương';
PRINT N'✓ Đã cập nhật dữ liệu khuyến mãi 4 hạng';
GO



-- ============================================================
PRINT N''
PRINT N'========================================'
PRINT N'  MIGRATION HOÀN TẤT - 25/03/2026'
PRINT N'========================================'
PRINT N''
PRINT N'TÓM TẮT:'
PRINT N'  1. NguoiDung        + cột AnhDaiDien (avatar)'
PRINT N'  2. LichHen.MaNguoiDung → cho phép NULL'
PRINT N'  3. LichHen          + cột SoDienThoaiKhach'
PRINT N'  4. HangThanhVien    = 4 hạng (Đỏ/Bạc/Vàng/Kim Cương)'
PRINT N'  5. SP_DatLichHen    = Stored Procedure đặt lịch'
PRINT N'  6. CHK_TrangThaiLich = Constraint (0-3)'
PRINT N'  7. HangThanhVien    + cột khuyến mãi (PhanTramGiamDuocPham, PhanTramGiamTongHD, GiamGiaCodinh, GhiChuKhuyenMai)'
GO
