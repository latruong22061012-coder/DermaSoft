-- ============================================================
-- 05_CONSTRAINTS.SQL
-- TẤT CẢ CÁC RÀNG BUỘC & INDEXES CHO HỆ THỐNG
-- ============================================================
-- Ngày: 2026-03-23
-- Mục Đích: Thêm toàn bộ ràng buộc kiểm tra & index hiệu năng
-- ============================================================

USE DERMASOFT;
GO

PRINT '========== CREATING CONSTRAINTS & INDEXES =========='
GO

-- ============================================================
-- PHẦN 1: RÀNG BUỘC DỰA TRÊN THỜI GIAN
-- ============================================================
PRINT 'Adding Time-based constraints...'

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_ThoiGianCa')
    ALTER TABLE CaLamViec
    ADD CONSTRAINT CHK_ThoiGianCa 
    CHECK (GioKetThuc > GioBatDau);
GO

-- ============================================================
-- PHẦN 2: RÀNG BUỘC GIÁ/SỐ LƯỢNG
-- ============================================================
PRINT 'Adding Price/Quantity constraints...'

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_DichVu_DonGia')
    ALTER TABLE DichVu
    ADD CONSTRAINT CHK_DichVu_DonGia 
    CHECK (DonGia >= 0);
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_Thuoc_DonGia')
    ALTER TABLE Thuoc
    ADD CONSTRAINT CHK_Thuoc_DonGia 
    CHECK (DonGia >= 0);
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_Thuoc_SoLuongTon')
    ALTER TABLE Thuoc
    ADD CONSTRAINT CHK_Thuoc_SoLuongTon 
    CHECK (SoLuongTon >= 0);
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_ChiPhi_SoTien')
    ALTER TABLE ChiPhiHoatDong
    ADD CONSTRAINT CHK_ChiPhi_SoTien 
    CHECK (SoTien > 0);
GO

-- ============================================================
-- PHẦN 3: RÀNG BUỘC KHO & PHÂN BỔ
-- ============================================================
PRINT 'Adding Warehouse constraints...'

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_SoLuongConLai')
    ALTER TABLE ChiTietNhapKho
    ADD CONSTRAINT CHK_SoLuongConLai 
    CHECK (SoLuongConLai >= 0);
GO

-- ============================================================
-- PHẦN 4: RÀNG BUỘC TRẠNG THÁI
-- ============================================================
PRINT 'Adding State/Status constraints...'

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_TrangThaiLich')
    ALTER TABLE LichHen
    ADD CONSTRAINT CHK_TrangThaiLich 
    CHECK (TrangThai BETWEEN 0 AND 3);
GO
-- 0=Chờ xác nhận, 1=Đã xác nhận, 2=Hoàn thành, 3=Hủy

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_TrangThaiPhieuKham')
    ALTER TABLE PhieuKham
    ADD CONSTRAINT CHK_TrangThaiPhieuKham 
    CHECK (TrangThai BETWEEN 0 AND 4);
GO
-- 0=Mới, 1=Đang khám, 2=Hoàn thành, 3=Đã thanh toán, 4=Hủy

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_NgayTaiKham')
    ALTER TABLE PhieuKham
    ADD CONSTRAINT CHK_NgayTaiKham 
    CHECK (NgayTaiKham > NgayKham OR NgayTaiKham IS NULL);
GO

-- ============================================================
-- PHẦN 5: RÀNG BUỘC CHO BẢNG NguoiDung
-- ============================================================
PRINT 'Adding NguoiDung constraints...'

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_TrangThaiTK')
    ALTER TABLE NguoiDung
    ADD CONSTRAINT CHK_TrangThaiTK 
    CHECK (TrangThaiTK IN (0, 1));
GO
-- 0=Khóa, 1=Hoạt động

-- ============================================================
-- PHẦN 6: RÀNG BUỘC XÁC THỰC OTP
-- ============================================================
PRINT 'Adding OTP constraints...'

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_SoLanSai')
    ALTER TABLE XacThucOTP
    ADD CONSTRAINT CHK_SoLanSai 
    CHECK (SoLanSai BETWEEN 0 AND 3);
GO
-- 0-2 lần được phép, 3 = bị khóa

-- ============================================================
-- PHẦN 7: RÀNG BUỘC ĐÁNH GIÁ & PHẢN HỒI
-- ============================================================
PRINT 'Adding Feedback constraints...'

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_DiemDanh')
    ALTER TABLE DanhGia
    ADD CONSTRAINT CHK_DiemDanh 
    CHECK (DiemDanh BETWEEN 1 AND 5);
GO
-- 1-5 sao

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CHK_LoaiThongBao')
    ALTER TABLE LichHen_Notification
    ADD CONSTRAINT CHK_LoaiThongBao 
    CHECK (LoaiThongBao IN (1, 2, 3));
GO
-- 1=Email, 2=SMS, 3=Trong ứng dụng

-- ============================================================
-- PHẦN 8: INDEXES CHO HIỆU NĂNG - XOÁ MỀM (SOFT DELETE)
-- ============================================================
PRINT 'Creating Soft Delete indexes...'

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BenhNhan_IsDeleted')
    CREATE INDEX IX_BenhNhan_IsDeleted
    ON BenhNhan(IsDeleted)
    INCLUDE (MaBenhNhan, HoTen, SoDienThoai);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PhieuKham_IsDeleted')
    CREATE INDEX IX_PhieuKham_IsDeleted
    ON PhieuKham(IsDeleted)
    INCLUDE (MaPhieuKham, MaBenhNhan, NgayKham, TrangThai);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_HoaDon_IsDeleted')
    CREATE INDEX IX_HoaDon_IsDeleted
    ON HoaDon(IsDeleted)
    INCLUDE (MaHoaDon, MaPhieuKham, TongTien, TrangThai);
GO

-- ============================================================
-- PHẦN 9: INDEXES CHO BẢNG NguoiDung (OTP/EMAIL)
-- ============================================================
PRINT 'Creating NguoiDung indexes...'

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_NguoiDung_SoDienThoai')
    CREATE INDEX IDX_NguoiDung_SoDienThoai ON NguoiDung(SoDienThoai);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_NguoiDung_Email')
    CREATE INDEX IDX_NguoiDung_Email ON NguoiDung(Email);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_NguoiDung_EmailConfirmToken')
    CREATE INDEX IDX_NguoiDung_EmailConfirmToken ON NguoiDung(EmailConfirmToken);
GO

PRINT 'Creating NguoiDung Email Verification Token index...'

-- ============================================================
-- PHẦN 10: INDEXES CHO TÌMKIẾM & BÁOCÁO
-- ============================================================
PRINT 'Creating Search & Report indexes...'

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PhieuKham_NgayKham')
    CREATE INDEX IX_PhieuKham_NgayKham ON PhieuKham(NgayKham) 
    INCLUDE (MaBenhNhan, TrangThai);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_HoaDon_NgayThanhToan')
    CREATE INDEX IX_HoaDon_NgayThanhToan ON HoaDon(NgayThanhToan) 
    INCLUDE (TongTien, TrangThai);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BenhNhan_SoDienThoai')
    CREATE INDEX IX_BenhNhan_SoDienThoai ON BenhNhan(SoDienThoai);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChiTietNhapKho_HanSuDung')
    CREATE INDEX IX_ChiTietNhapKho_HanSuDung ON ChiTietNhapKho(HanSuDung) 
    INCLUDE (SoLuongConLai, MaThuoc);
GO

-- ============================================================
-- PHẦN 11: KIỂM CHỨNG TẤT CẢ RÀNG BUỘC
-- ============================================================
PRINT ''
PRINT '========== VERIFICATION =========='
GO

-- Kiểm chứng tất cả check constraints
SELECT
    t.name AS TableName,
    c.name AS ConstraintName,
    'CHECK' AS ConstraintType
FROM sys.tables t
INNER JOIN sys.check_constraints c ON t.object_id = c.parent_object_id
ORDER BY t.name;
GO

-- Kiểm chứng tất cả unique constraints
SELECT
    t.name AS TableName,
    c.name AS ConstraintName,
    'UNIQUE' AS ConstraintType
FROM sys.tables t
INNER JOIN sys.key_constraints c ON t.object_id = c.parent_object_id
WHERE c.type = 'UQ'
ORDER BY t.name;
GO

-- Kiểm chứng tất cả indexes
SELECT
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType
FROM sys.indexes i
WHERE OBJECTPROPERTY(i.object_id, 'IsUserTable') = 1
AND i.name IS NOT NULL
ORDER BY OBJECT_NAME(i.object_id);
GO

PRINT ''
PRINT '✓ Tất cả Constraints & Indexes đã được tạo thành công!'
GO
