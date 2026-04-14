USE DERMASOFT;
GO

PRINT '========== CREATING VIEWS =========='
GO

-- ============================================================
-- DROP CÁC VIEWS CŨ
-- ============================================================
DROP VIEW IF EXISTS VW_HoSoBenhAn;
DROP VIEW IF EXISTS VW_BaoCaoDoanhThu;
DROP VIEW IF EXISTS VW_TonKhoTheoLo;
DROP VIEW IF EXISTS VW_DanhSachTaiKhoan;
DROP VIEW IF EXISTS VW_BenhNhan_Active;
DROP VIEW IF EXISTS VW_PhieuKham_Active;
DROP VIEW IF EXISTS VW_HoaDon_Active;
GO

-- ============================================================
-- VIEW 1: VW_HoSoBenhAn
-- Mục Đích: Hồ sơ y tế bệnh nhân kèm lịch sử chuẩn đoán
-- ============================================================
PRINT 'Creating VW_HoSoBenhAn...'
GO
CREATE VIEW VW_HoSoBenhAn
AS
SELECT
    bn.MaBenhNhan,
    bn.HoTen,
    bn.NgaySinh,
    bn.GioiTinh,
    bn.SoDienThoai,
    pk.MaPhieuKham,
    pk.NgayKham,
    pk.ChanDoan,
    pk.TrangThai,
    dv.TenDichVu,
    t.MaThuoc,
    t.TenThuoc,
    COUNT(*) OVER (PARTITION BY bn.MaBenhNhan) AS SoLanKham
FROM BenhNhan bn
LEFT JOIN PhieuKham pk ON bn.MaBenhNhan = pk.MaBenhNhan AND pk.IsDeleted = 0
LEFT JOIN ChiTietDichVu cdt ON pk.MaPhieuKham = cdt.MaPhieuKham
LEFT JOIN DichVu dv ON cdt.MaDichVu = dv.MaDichVu
LEFT JOIN ChiTietDonThuoc ctdt ON pk.MaPhieuKham = ctdt.MaPhieuKham
LEFT JOIN Thuoc t ON ctdt.MaThuoc = t.MaThuoc
WHERE bn.IsDeleted = 0;
GO

-- ============================================================
-- VIEW 2: VW_BaoCaoDoanhThu
-- Mục Đích: Báo cáo doanh thu theo dịch vụ, tháng, nhân viên
-- ============================================================
PRINT 'Creating VW_BaoCaoDoanhThu...'
GO
CREATE VIEW VW_BaoCaoDoanhThu
AS
SELECT
    YEAR(hd.NgayThanhToan) AS Nam,
    MONTH(hd.NgayThanhToan) AS Thang,
    hd.MaHoaDon,
    hd.MaPhieuKham,
    pk.MaBenhNhan,
    bn.HoTen AS TenBenhNhan,
    hd.TongTienDichVu,
    hd.TongThuoc,
    hd.TongTien,
    hd.GiamGia,
    hd.TienKhachTra,
    hd.TienThua,
    hd.TrangThai,
    COUNT(*) OVER (PARTITION BY YEAR(hd.NgayThanhToan), MONTH(hd.NgayThanhToan)) AS SoHoaDonThang
FROM HoaDon hd
LEFT JOIN PhieuKham pk ON hd.MaPhieuKham = pk.MaPhieuKham
LEFT JOIN BenhNhan bn ON pk.MaBenhNhan = bn.MaBenhNhan
WHERE hd.IsDeleted = 0;
GO

-- ============================================================
-- VIEW 3: VW_TonKhoTheoLo
-- Mục Đích: Kho hàng theo lô/lô hàng với ưu tiên FIFO/FEFO
-- Hiển thị ngày hết hạn để phân bổ tối ưu
-- ============================================================
PRINT 'Creating VW_TonKhoTheoLo...'
GO
CREATE VIEW VW_TonKhoTheoLo
AS
SELECT
    t.MaThuoc,
    t.TenThuoc,
    ctk.MaPhieuNhap,
    pnk.NgayNhap,
    ctk.HanSuDung,
    ctk.SoLuongConLai,
    DATEDIFF(DAY, GETDATE(), ctk.HanSuDung) AS SoNgayConLai,
    CASE 
        WHEN ctk.HanSuDung < GETDATE() THEN N'HẾT HẠN'
        WHEN DATEDIFF(DAY, GETDATE(), ctk.HanSuDung) < 30 THEN N'Sắp hết hạn'
        WHEN DATEDIFF(DAY, GETDATE(), ctk.HanSuDung) < 90 THEN N'Cảnh báo'
        ELSE N'Bình thường'
    END AS TrangThaiHetHan,
    ROW_NUMBER() OVER (PARTITION BY t.MaThuoc ORDER BY ctk.HanSuDung ASC) AS UuTienFEFO
FROM Thuoc t
INNER JOIN ChiTietNhapKho ctk ON t.MaThuoc = ctk.MaThuoc
INNER JOIN PhieuNhapKho pnk ON ctk.MaPhieuNhap = pnk.MaPhieuNhap
WHERE ctk.SoLuongConLai > 0 AND ctk.HanSuDung > GETDATE();
GO

-- ============================================================
-- VIEW 4: VW_DanhSachTaiKhoan
-- Mục Đích: Danh sách tài khoản nhân viên để quản lý admin
-- CẬP NHẬT: Reference NguoiDung (không phải NhanVien)
-- ============================================================
PRINT 'Creating VW_DanhSachTaiKhoan...'
GO
CREATE VIEW VW_DanhSachTaiKhoan
AS
SELECT
    nd.MaNguoiDung,
    nd.HoTen,
    nd.Email,
    nd.SoDienThoai,
    nd.TenDangNhap,
    vt.TenVaiTro,
    CASE nd.TrangThaiTK
        WHEN 1 THEN N'Hoạt động'
        WHEN 0 THEN N'Khóa'
        ELSE N'Không xác định'
    END AS TrangThaiTaiKhoan,
    nd.NgayTao,
    CASE 
        WHEN nd.TrangThaiTK = 1 THEN N'✓ Đang hoạt động'
        ELSE N'✗ Đã khóa'
    END AS TrangThaiDisplay,
    nd.EmailVerifiedAt,
    nd.LastEmailVerificationAt
FROM NguoiDung nd
LEFT JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
WHERE nd.IsDeleted = 0;
GO

-- ============================================================
-- VIEW 5: VW_BenhNhan_Active
-- Mục Đích: Bệnh nhân hoạt động (isDeleted = 0)
-- ============================================================
PRINT 'Creating VW_BenhNhan_Active...'
GO
CREATE VIEW VW_BenhNhan_Active
AS
SELECT
    MaBenhNhan,
    HoTen,
    NgaySinh,
    GioiTinh,
    SoDienThoai,
    TienSuBenhLy
FROM BenhNhan
WHERE IsDeleted = 0;
GO

-- ============================================================
-- VIEW 6: VW_PhieuKham_Active
-- Mục Đích: Phiếu khám hoạt động (isDeleted = 0)
-- ============================================================
PRINT 'Creating VW_PhieuKham_Active...'
GO
CREATE VIEW VW_PhieuKham_Active
AS
SELECT
    pk.MaPhieuKham,
    pk.MaBenhNhan,
    pk.NgayKham,
    pk.TrieuChung,
    pk.ChanDoan,
    pk.TrangThai,
    pk.NgayTaiKham
FROM PhieuKham pk
WHERE pk.IsDeleted = 0;
GO

-- ============================================================
-- VIEW 7: VW_HoaDon_Active
-- Mục Đích: Hóa đơn hoạt động (isDeleted = 0)
-- ============================================================
PRINT 'Creating VW_HoaDon_Active...'
GO
CREATE VIEW VW_HoaDon_Active
AS
SELECT
    hd.MaHoaDon,
    hd.MaPhieuKham,
    hd.TongTienDichVu,
    hd.TongThuoc,
    hd.TongTien,
    hd.GiamGia,
    hd.TienKhachTra,
    hd.TienThua,
    hd.TrangThai,
    ISNULL(hd.NgayThanhToan, hd.NgayTao) AS NgayHoaDon
FROM HoaDon hd
WHERE hd.IsDeleted = 0;
GO

PRINT '✓ Tất cả Views đã được tạo thành công!'
GO
