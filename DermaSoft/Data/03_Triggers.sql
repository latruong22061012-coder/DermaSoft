USE DERMASOFT;
GO

PRINT '========== CREATING TRIGGERS =========='
GO

-- ============================================================
-- DROP CÁC TRIGGER CŨ
-- ============================================================
DROP TRIGGER IF EXISTS TRG_NhanVien_LogChuyenDoi;
DROP TRIGGER IF EXISTS TRG_NhanVien_LogChuyenDoi_KoaHuy;
DROP TRIGGER IF EXISTS TRG_NguoiDung_LogChuyenDoi;
DROP TRIGGER IF EXISTS TRG_NguoiDung_UpdateEmailVerificationAt;
DROP TRIGGER IF EXISTS TRG_NguoiDung_CheckEmailConfirmationNeeded;
DROP TRIGGER IF EXISTS TRG_NhapKho_CapNhatTon;
DROP TRIGGER IF EXISTS TRG_HoaDon_CapPhatDiem;
DROP TRIGGER IF EXISTS TRG_PhieuKham_TaoLogChuyenDoi;
DROP TRIGGER IF EXISTS TRG_HoaDon_TaoLogChuyenDoi;
GO

-- ============================================================
-- PHẦN 1: TRIGGERS CHO BẢNG NguoiDung (USERS)
-- ============================================================

-- ============================================================
-- TRIGGER 1: GHI LOG THAY ĐỔI BẢNG NguoiDung
-- Mục Đích: Ghi nhập audit log khi có update
-- ============================================================
PRINT 'Creating TRG_NguoiDung_LogChuyenDoi...'
GO
CREATE TRIGGER TRG_NguoiDung_LogChuyenDoi
ON NguoiDung AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        INSERT INTO NguoiDung_AuditLog (MaNguoiDung, HanhDongCu, NgayTao, DuLieuCu, DuLieuMoi)
        SELECT 
            i.MaNguoiDung,
            'UPDATE',
            GETDATE(),
            CONCAT('TenDangNhap: ', d.TenDangNhap, ' | Email: ', d.Email, ' | SoDienThoai: ', d.SoDienThoai),
            CONCAT('TenDangNhap: ', i.TenDangNhap, ' | Email: ', i.Email, ' | SoDienThoai: ', i.SoDienThoai)
        FROM inserted i
        INNER JOIN deleted d ON i.MaNguoiDung = d.MaNguoiDung;
    END TRY
    BEGIN CATCH
        PRINT 'Lỗi trong trigger TRG_NguoiDung_LogChuyenDoi: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================
-- TRIGGER 2: TỰ ĐỘNG CẬP NHẬT EMAIL VERIFICATION TIME
-- Mục Đích: Cập nhật lần xác thực email mới nhất
-- ============================================================
PRINT 'Creating TRG_NguoiDung_UpdateEmailVerificationAt...'
GO
CREATE TRIGGER TRG_NguoiDung_UpdateEmailVerificationAt
ON NguoiDung AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Nếu EmailVerifiedAt được set lần đầu, tự động cập nhật LastEmailVerificationAt
        UPDATE NguoiDung
        SET LastEmailVerificationAt = GETDATE()
        WHERE MaNguoiDung IN (SELECT MaNguoiDung FROM inserted)
        AND LastEmailVerificationAt IS NULL
        AND EmailVerifiedAt IS NOT NULL;
    END TRY
    BEGIN CATCH
        PRINT 'Lỗi trong trigger TRG_NguoiDung_UpdateEmailVerificationAt: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================
-- TRIGGER 3: KIỂM TRA EMAIL+CONFIRMATION (3 NĂM)
-- Mục Đích: Đánh dấu khi cần xác thực email mỗi 3 năm
-- ============================================================
PRINT 'Creating TRG_NguoiDung_CheckEmailConfirmationNeeded...'
GO
CREATE TRIGGER TRG_NguoiDung_CheckEmailConfirmationNeeded
ON NguoiDung AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Khi tạo user mới, tự động điền LastEmailVerificationAt
        UPDATE NguoiDung
        SET 
            LastEmailVerificationAt = GETDATE(),
            EmailConfirmationNeeded = 0
        WHERE MaNguoiDung IN (SELECT MaNguoiDung FROM inserted)
        AND LastEmailVerificationAt IS NULL;
    END TRY
    BEGIN CATCH
        PRINT 'Lỗi trong trigger TRG_NguoiDung_CheckEmailConfirmationNeeded: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================
-- PHẦN 2: TRIGGERS CHO KHO (WAREHOUSE)
-- ============================================================

-- ============================================================
-- TRIGGER 4: CẬP NHẬT TỒN KHO KHI NHẬP THUỐC
-- Mục Đích: Cập nhật số lượng tồn tự động
-- ============================================================
PRINT 'Creating TRG_NhapKho_CapNhatTon...'
GO
CREATE TRIGGER TRG_NhapKho_CapNhatTon
ON ChiTietNhapKho
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Thuoc
    SET SoLuongTon = Thuoc.SoLuongTon + i.SoLuong
    FROM Thuoc
    INNER JOIN inserted i ON Thuoc.MaThuoc = i.MaThuoc;
    
    IF @@ERROR <> 0
        ROLLBACK TRANSACTION;
END;
GO

-- ============================================================
-- PHẦN 3: TRIGGERS CHO TÀI CHÍNH (FINANCE)
-- ============================================================

-- ============================================================
-- TRIGGER 5: CẤP PHÁT ĐIỂM THƯỞNG KHI THANH TOÁN
-- Mục Đích: Tính điểm dựa trên tiền thanh toán (1 triệu = 1000 điểm)
-- ============================================================
PRINT 'Creating TRG_HoaDon_CapPhatDiem...'
GO
CREATE TRIGGER TRG_HoaDon_CapPhatDiem
ON HoaDon AFTER UPDATE
AS 
BEGIN
    SET NOCOUNT ON;
    
    UPDATE tvi
    SET DiemTichLuy = DiemTichLuy + CAST(ROUND(i.TienKhachTra / 1000, 0) AS INT)
    FROM ThanhVienInfo tvi
    INNER JOIN PhieuKham pk ON tvi.MaBenhNhan = pk.MaBenhNhan
    INNER JOIN inserted i ON pk.MaPhieuKham = i.MaPhieuKham
    WHERE i.TrangThai = 1
    AND i.MaHoaDon IN (SELECT inserted.MaHoaDon 
                       FROM inserted 
                       WHERE inserted.TrangThai = 1);
END;
GO

-- ============================================================
-- PHẦN 4: TRIGGERS CHO LỊCH SỬ (HISTORY)
-- ============================================================

-- ============================================================
-- TRIGGER 6: GHI NHẬP LOG CỰU DỮ LIỆU PHIẾU KHÁM
-- Mục Đích: Giữ lịch sử thay đổi khám phá
-- ============================================================
PRINT 'Creating TRG_PhieuKham_TaoLogChuyenDoi...'
GO
CREATE TRIGGER TRG_PhieuKham_TaoLogChuyenDoi
ON PhieuKham AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO PhieuKham_LichSu
    (MaPhieuKham, MaBenhNhan, MaNguoiDung, MaLichHen, NgayKham, 
     TrieuChung, ChanDoan, NgayTaiKham, TrangThai, ThoiGianBatDau, ThoiGianKetThuc)
    SELECT 
        d.MaPhieuKham,
        d.MaBenhNhan,
        d.MaNguoiDung,
        d.MaLichHen,
        d.NgayKham,
        d.TrieuChung,
        d.ChanDoan,
        d.NgayTaiKham,
        d.TrangThai,
        GETDATE(),
        GETDATE()
    FROM deleted d;
END;
GO

-- ============================================================
-- TRIGGER 7: GHI NHẬP LOG CỰU DỮ LIỆU HÓA ĐƠN
-- Mục Đích: Giữ lịch sử thay đổi thanh toán
-- ============================================================
PRINT 'Creating TRG_HoaDon_TaoLogChuyenDoi...'
GO
CREATE TRIGGER TRG_HoaDon_TaoLogChuyenDoi
ON HoaDon AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO HoaDon_LichSu
    (MaHoaDon, MaPhieuKham, TongTien, GiamGia, TienKhachTra, 
     PhuongThucThanhToan, NgayThanhToan, TrangThai, ThoiGianBatDau, ThoiGianKetThuc)
    SELECT 
        d.MaHoaDon,
        d.MaPhieuKham,
        d.TongTien,
        d.GiamGia,
        d.TienKhachTra,
        d.PhuongThucThanhToan,
        d.NgayThanhToan,
        d.TrangThai,
        GETDATE(),
        GETDATE()
    FROM deleted d;
END;
GO

PRINT '✓ Tất cả Triggers đã được tạo thành công!'
GO
