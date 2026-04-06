USE DERMASOFT;
GO

-- ═══════════════════════════════════════
-- SCRIPT INSERT DỮ LIỆU TEST CHO BÁO CÁO DOANH THU
-- Chạy theo thứ tự: BenhNhan → PhieuKham → ChiTietDichVu → ChiTietDonThuoc → HoaDon
-- ═══════════════════════════════════════

PRINT '========== SEED TEST DATA =========='
GO

-- ═══════════════════════════════════════
-- BƯỚC 1: Thêm Bệnh Nhân (nếu chưa có)
-- ═══════════════════════════════════════
PRINT 'Bước 1: Thêm BenhNhan...'

IF NOT EXISTS (SELECT 1 FROM BenhNhan WHERE SoDienThoai = '0901000001')
    INSERT INTO BenhNhan (HoTen, NgaySinh, GioiTinh, SoDienThoai, TienSuBenhLy)
    VALUES (N'Trần Thị Mai', '1990-05-15', 0, '0901000001', N'Dị ứng da');

IF NOT EXISTS (SELECT 1 FROM BenhNhan WHERE SoDienThoai = '0901000002')
    INSERT INTO BenhNhan (HoTen, NgaySinh, GioiTinh, SoDienThoai, TienSuBenhLy)
    VALUES (N'Nguyễn Văn Hùng', '1985-08-20', 1, '0901000002', N'Viêm da cơ địa');

IF NOT EXISTS (SELECT 1 FROM BenhNhan WHERE SoDienThoai = '0901000003')
    INSERT INTO BenhNhan (HoTen, NgaySinh, GioiTinh, SoDienThoai, TienSuBenhLy)
    VALUES (N'Lê Hoàng Anh', '1995-03-10', 1, '0901000003', NULL);

IF NOT EXISTS (SELECT 1 FROM BenhNhan WHERE SoDienThoai = '0901000004')
    INSERT INTO BenhNhan (HoTen, NgaySinh, GioiTinh, SoDienThoai, TienSuBenhLy)
    VALUES (N'Phạm Thị Lan', '1988-12-01', 0, '0901000004', N'Nổi mề đay');

IF NOT EXISTS (SELECT 1 FROM BenhNhan WHERE SoDienThoai = '0901000005')
    INSERT INTO BenhNhan (HoTen, NgaySinh, GioiTinh, SoDienThoai, TienSuBenhLy)
    VALUES (N'Võ Minh Tuấn', '1992-07-25', 1, '0901000005', NULL);

PRINT 'Bước 1: Xong.'
GO

-- ═══════════════════════════════════════
-- BƯỚC 2: Thêm Phiếu Khám
-- ═══════════════════════════════════════
PRINT 'Bước 2: Thêm PhieuKham...'

DECLARE @BacSi INT = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = 2 AND IsDeleted = 0);
IF @BacSi IS NULL SET @BacSi = (SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE IsDeleted = 0);

DECLARE @BN1 INT = (SELECT TOP 1 MaBenhNhan FROM BenhNhan WHERE SoDienThoai = '0901000001');
DECLARE @BN2 INT = (SELECT TOP 1 MaBenhNhan FROM BenhNhan WHERE SoDienThoai = '0901000002');
DECLARE @BN3 INT = (SELECT TOP 1 MaBenhNhan FROM BenhNhan WHERE SoDienThoai = '0901000003');
DECLARE @BN4 INT = (SELECT TOP 1 MaBenhNhan FROM BenhNhan WHERE SoDienThoai = '0901000004');
DECLARE @BN5 INT = (SELECT TOP 1 MaBenhNhan FROM BenhNhan WHERE SoDienThoai = '0901000005');

INSERT INTO PhieuKham (MaBenhNhan, MaNguoiDung, NgayKham, TrieuChung, ChanDoan, TrangThai)
VALUES 
(@BN1, @BacSi, DATEADD(DAY, -6, GETDATE()), N'Ngứa da, nổi mẩn đỏ',   N'Viêm da tiếp xúc',  3),
(@BN2, @BacSi, DATEADD(DAY, -5, GETDATE()), N'Da khô, bong tróc',       N'Viêm da cơ địa',    3),
(@BN3, @BacSi, DATEADD(DAY, -4, GETDATE()), N'Mụn trứng cá',            N'Acne vulgaris',      3),
(@BN1, @BacSi, DATEADD(DAY, -3, GETDATE()), N'Tái khám',                 N'Viêm da ổn định',   3),
(@BN4, @BacSi, DATEADD(DAY, -2, GETDATE()), N'Nổi mề đay toàn thân',    N'Urticaria',          3),
(@BN5, @BacSi, DATEADD(DAY, -1, GETDATE()), N'Nám da, tàn nhang',        N'Melasma',            3),
(@BN2, @BacSi, GETDATE(),                    N'Tái khám da',              N'Da cải thiện',       3),
(@BN3, @BacSi, DATEADD(DAY, -5, GETDATE()), N'Mụn viêm',                 N'Acne nặng',          3),
(@BN4, @BacSi, DATEADD(DAY, -2, GETDATE()), N'Dị ứng mỹ phẩm',          N'Contact dermatitis', 3),
(@BN5, @BacSi, GETDATE(),                    N'Rụng tóc',                 N'Alopecia',           3);

PRINT 'Bước 2: Xong.'
GO

-- ═══════════════════════════════════════
-- BƯỚC 3: Thêm ChiTietDichVu
-- ═══════════════════════════════════════
PRINT 'Bước 3: Thêm ChiTietDichVu...'

DECLARE @DV1 INT = (SELECT TOP 1 MaDichVu FROM DichVu);
DECLARE @GiaDV DECIMAL(18,2) = (SELECT DonGia FROM DichVu WHERE MaDichVu = @DV1);

IF @DV1 IS NOT NULL
BEGIN
    DECLARE @PKTable TABLE (ID INT IDENTITY, MaPK INT);
    INSERT INTO @PKTable 
    SELECT TOP 10 MaPhieuKham FROM PhieuKham 
    WHERE MaPhieuKham NOT IN (SELECT MaPhieuKham FROM ChiTietDichVu WHERE MaDichVu = @DV1)
    ORDER BY MaPhieuKham DESC;

    DECLARE @idx INT = 1;
    DECLARE @maxIdx INT = (SELECT COUNT(*) FROM @PKTable);
    DECLARE @curPK INT;

    WHILE @idx <= @maxIdx
    BEGIN
        SET @curPK = (SELECT MaPK FROM @PKTable WHERE ID = @idx);

        INSERT INTO ChiTietDichVu (MaPhieuKham, MaDichVu, SoLuong, ThanhTien)
        VALUES (@curPK, @DV1, 1, @GiaDV);

        SET @idx = @idx + 1;
    END
END

PRINT 'Bước 3: Xong.'
GO

-- ═══════════════════════════════════════
-- BƯỚC 4: Thêm ChiTietDonThuoc
-- ═══════════════════════════════════════
PRINT 'Bước 4: Thêm ChiTietDonThuoc...'

DECLARE @Thuoc1 INT = (SELECT TOP 1 MaThuoc FROM Thuoc WHERE SoLuongTon > 0);

IF @Thuoc1 IS NOT NULL
BEGIN
    DECLARE @PKTable2 TABLE (ID INT IDENTITY, MaPK INT);
    INSERT INTO @PKTable2 
    SELECT TOP 10 MaPhieuKham FROM PhieuKham 
    WHERE MaPhieuKham NOT IN (SELECT MaPhieuKham FROM ChiTietDonThuoc WHERE MaThuoc = @Thuoc1)
    ORDER BY MaPhieuKham DESC;

    DECLARE @idx2 INT = 1;
    DECLARE @maxIdx2 INT = (SELECT COUNT(*) FROM @PKTable2);
    DECLARE @curPK2 INT;

    WHILE @idx2 <= @maxIdx2
    BEGIN
        SET @curPK2 = (SELECT MaPK FROM @PKTable2 WHERE ID = @idx2);

        INSERT INTO ChiTietDonThuoc (MaPhieuKham, MaThuoc, SoLuong, LieuDung)
        VALUES (@curPK2, @Thuoc1, 2, N'Ngày 2 lần sau ăn');

        SET @idx2 = @idx2 + 1;
    END
END

PRINT 'Bước 4: Xong.'
GO

-- ═══════════════════════════════════════
-- BƯỚC 5: Thêm HoaDon ★ QUAN TRỌNG NHẤT ★
-- ═══════════════════════════════════════
PRINT 'Bước 5: Thêm HoaDon...'

DECLARE @PKTable3 TABLE (ID INT IDENTITY, MaPK INT);
INSERT INTO @PKTable3 
SELECT TOP 10 MaPhieuKham FROM PhieuKham 
WHERE MaPhieuKham NOT IN (SELECT MaPhieuKham FROM HoaDon)
ORDER BY MaPhieuKham DESC;

DECLARE @idx3 INT = 1;
DECLARE @maxIdx3 INT = (SELECT COUNT(*) FROM @PKTable3);
DECLARE @curPK3 INT;
DECLARE @tienDV DECIMAL(18,2);
DECLARE @tienThuoc DECIMAL(18,2);
DECLARE @tongTien DECIMAL(18,2);
DECLARE @ngayTao DATETIME;

WHILE @idx3 <= @maxIdx3
BEGIN
    SET @curPK3 = (SELECT MaPK FROM @PKTable3 WHERE ID = @idx3);

    -- Tính tiền dịch vụ
    SET @tienDV = ISNULL((SELECT SUM(ThanhTien) FROM ChiTietDichVu WHERE MaPhieuKham = @curPK3), 0);

    -- Tính tiền thuốc
    SET @tienThuoc = ISNULL((
        SELECT SUM(ct.SoLuong * t.DonGia) 
        FROM ChiTietDonThuoc ct 
        JOIN Thuoc t ON ct.MaThuoc = t.MaThuoc 
        WHERE ct.MaPhieuKham = @curPK3
    ), 0);

    SET @tongTien = @tienDV + @tienThuoc;
    SET @ngayTao = (SELECT NgayKham FROM PhieuKham WHERE MaPhieuKham = @curPK3);

    INSERT INTO HoaDon (MaPhieuKham, TongTien, TongTienDichVu, TongThuoc, GiamGia, TienKhachTra, TienThua, PhuongThucThanhToan, NgayThanhToan, NgayTao, TrangThai)
    VALUES (
        @curPK3,
        @tongTien,
        @tienDV,
        @tienThuoc,
        0,
        @tongTien,
        0,
        N'Tiền mặt',
        @ngayTao,
        @ngayTao,
        1  -- Đã thanh toán
    );

    SET @idx3 = @idx3 + 1;
END

PRINT 'Bước 5: Xong.'
GO

-- ═══════════════════════════════════════
-- KIỂM TRA KẾT QUẢ
-- ═══════════════════════════════════════
PRINT '========== KẾT QUẢ =========='

SELECT 'BenhNhan' AS Bang, COUNT(*) AS SoBanGhi FROM BenhNhan;
SELECT 'PhieuKham' AS Bang, COUNT(*) AS SoBanGhi FROM PhieuKham WHERE IsDeleted = 0;
SELECT 'ChiTietDichVu' AS Bang, COUNT(*) AS SoBanGhi FROM ChiTietDichVu;
SELECT 'ChiTietDonThuoc' AS Bang, COUNT(*) AS SoBanGhi FROM ChiTietDonThuoc;
SELECT 'HoaDon' AS Bang, COUNT(*) AS SoBanGhi FROM HoaDon WHERE IsDeleted = 0;

PRINT ''
PRINT '--- DOANH THU THEO NGÀY (dữ liệu cho BaoCaoDoanhThuForm) ---'
SELECT 
    CAST(NgayTao AS DATE) AS Ngay, 
    COUNT(*) AS SoHD, 
    SUM(TongTien) AS DoanhThu, 
    SUM(TongThuoc) AS Thuoc, 
    SUM(TongTienDichVu) AS DichVu
FROM HoaDon 
WHERE IsDeleted = 0
GROUP BY CAST(NgayTao AS DATE)
ORDER BY Ngay DESC;

PRINT '========== SEED HOÀN TẤT =========='
GO
