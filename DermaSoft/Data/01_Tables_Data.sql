CREATE DATABASE DERMASOFT;
GO
USE DERMASOFT;
GO




-- ============================================================
-- PHẦN 1: NHÓM 1 - DANH MỤC & CON NGƯỜI
-- ============================================================

CREATE TABLE VaiTro (
    MaVaiTro   INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro  NVARCHAR(50) NOT NULL
);

CREATE TABLE DichVu (
    MaDichVu   INT IDENTITY(1,1) PRIMARY KEY,
    TenDichVu  NVARCHAR(100) NOT NULL,
    DonGia     DECIMAL(18,2) NOT NULL DEFAULT 0
);

CREATE TABLE NhaCungCap (
    MaNhaCungCap       INT IDENTITY(1,1) PRIMARY KEY,
    TenNhaCungCap      NVARCHAR(100) NOT NULL,
    SoDienThoaiLienHe  VARCHAR(15),
    DiaChi             NVARCHAR(200)
);

CREATE TABLE CaLamViec (
    MaCa        INT IDENTITY(1,1) PRIMARY KEY,
    TenCa       NVARCHAR(50) NOT NULL,
    GioBatDau   TIME NOT NULL,
    GioKetThuc  TIME NOT NULL
);

-- BẢNG CHÍNH SAU KHI MIGRATION
CREATE TABLE NguoiDung (
    MaNguoiDung    INT IDENTITY(1,1) PRIMARY KEY,
    HoTen          NVARCHAR(100) NOT NULL,
    SoDienThoai    VARCHAR(15)   NOT NULL UNIQUE,
    Email          VARCHAR(100),
    TenDangNhap    VARCHAR(50)   NOT NULL UNIQUE,
    MatKhau        VARCHAR(255)  NOT NULL,
    MaVaiTro       INT           NOT NULL,
    TrangThaiTK    BIT           NOT NULL DEFAULT 1,
    DoiMatKhau     BIT           NOT NULL DEFAULT 1,
    NgayTao        DATETIME      DEFAULT GETDATE(),
    IsDeleted      BIT           DEFAULT 0,
    AnhDaiDien     NVARCHAR(255) NULL,
    -- OTP & EMAIL VERIFICATION COLUMNS (TẠO THÊM)
    EmailVerifiedAt            DATETIME NULL,
    EmailConfirmToken          NVARCHAR(255) NULL,
    EmailConfirmTokenExpiry    DATETIME NULL,
    SoDienThoaiCu              NVARCHAR(20) NULL,
    NgayUpdateSoDienThoai      DATETIME NULL,
    EmailConfirmSentAt         DATETIME NULL,
    EmailConfirmationNeeded    BIT DEFAULT 0,
    LastEmailVerificationAt    DATETIME NULL,
    FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro),
    CONSTRAINT UQ_NguoiDung_Email UNIQUE (Email)
);

-- BẢNG KIỂM TOÁN CHO NguoiDung
CREATE TABLE NguoiDung_AuditLog (
    MaAudit INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    HanhDongCu NVARCHAR(100),
    ThoiGianThayDoi DATETIME DEFAULT GETDATE(),
    NgayTao DATETIME DEFAULT GETDATE(),
    DuLieuCu NVARCHAR(MAX),
    DuLieuMoi NVARCHAR(MAX),
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);

-- ============================================================
-- PHẦN 2: NHÓM 2 - KHO BÃI (DƯỢC - MỸ PHẨM)
-- ============================================================

CREATE TABLE Thuoc (
    MaThuoc      INT IDENTITY(1,1) PRIMARY KEY,
    TenThuoc     NVARCHAR(100) NOT NULL,
    DonViTinh    NVARCHAR(20)  NOT NULL,
    DonGia       DECIMAL(18,2) NOT NULL DEFAULT 0,
    SoLuongTon   INT           NOT NULL DEFAULT 0
);

CREATE TABLE PhieuNhapKho (
    MaPhieuNhap   INT IDENTITY(1,1) PRIMARY KEY,
    MaNhaCungCap  INT           NOT NULL,
    MaNguoiDung   INT           NOT NULL,
    NgayNhap      DATETIME      DEFAULT GETDATE(),
    TongGiaTri    DECIMAL(18,2) DEFAULT 0,
    FOREIGN KEY (MaNhaCungCap) REFERENCES NhaCungCap(MaNhaCungCap),
    FOREIGN KEY (MaNguoiDung)  REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE ChiTietNhapKho (
    MaPhieuNhap    INT           NOT NULL,
    MaThuoc        INT           NOT NULL,
    SoLuong        INT           NOT NULL,
    SoLuongConLai  INT           NOT NULL DEFAULT 0,
    GiaNhap        DECIMAL(18,2) NOT NULL,
    HanSuDung      DATE          NOT NULL,
    PRIMARY KEY (MaPhieuNhap, MaThuoc),
    FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhapKho(MaPhieuNhap),
    FOREIGN KEY (MaThuoc)     REFERENCES Thuoc(MaThuoc)
);

-- ============================================================
-- PHẦN 3: NHÓM 3 - NGHIỆP VỤ KHÁM CHỮA BỆNH
-- ============================================================

CREATE TABLE BenhNhan (
    MaBenhNhan    INT IDENTITY(1,1) PRIMARY KEY,
    HoTen         NVARCHAR(100) NOT NULL,
    NgaySinh      DATE,
    GioiTinh      BIT,
    SoDienThoai   VARCHAR(15)   NOT NULL UNIQUE,
    TienSuBenhLy  NVARCHAR(500),
    IsDeleted     BIT           DEFAULT 0
);

CREATE TABLE LichHen (
    MaLichHen          INT IDENTITY(1,1) PRIMARY KEY,
    MaBenhNhan         INT           NOT NULL,
    MaNguoiDung        INT           NULL,         -- NULL = lịch website chưa phân công
    ThoiGianHen        DATETIME      NOT NULL,
    TrangThai          TINYINT       DEFAULT 1,
    GhiChu             NVARCHAR(200),
    SoDienThoaiKhach   VARCHAR(15)   NULL,          -- SĐT khi đặt qua website
    FOREIGN KEY (MaBenhNhan)  REFERENCES BenhNhan(MaBenhNhan),
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung) ON DELETE SET NULL
);

CREATE TABLE PhieuKham (
    MaPhieuKham  INT IDENTITY(1,1) PRIMARY KEY,
    MaBenhNhan   INT          NOT NULL,
    MaNguoiDung  INT          NOT NULL,
    MaLichHen    INT          NULL,
    NgayKham     DATETIME     DEFAULT GETDATE(),
    TrieuChung   NVARCHAR(500),
    ChanDoan     NVARCHAR(500),
    NgayTaiKham  DATE,
    TrangThai    TINYINT      NOT NULL DEFAULT 0,
    GhiChu       NVARCHAR(200),
    IsDeleted    BIT          DEFAULT 0,
    FOREIGN KEY (MaBenhNhan)  REFERENCES BenhNhan(MaBenhNhan),
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaLichHen)   REFERENCES LichHen(MaLichHen)
);

CREATE TABLE PhanCongCa (
    MaPhanCong         INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung        INT  NOT NULL,
    MaCa               INT  NOT NULL,
    NgayLamViec        DATE NOT NULL,
    TrangThaiDiemDanh  TINYINT DEFAULT 1,
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaCa)        REFERENCES CaLamViec(MaCa)
);

CREATE TABLE ChiTietDichVu (
    MaPhieuKham  INT           NOT NULL,
    MaDichVu     INT           NOT NULL,
    SoLuong      INT           DEFAULT 1,
    ThanhTien    DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaPhieuKham, MaDichVu),
    FOREIGN KEY (MaPhieuKham) REFERENCES PhieuKham(MaPhieuKham),
    FOREIGN KEY (MaDichVu)    REFERENCES DichVu(MaDichVu)
);

CREATE TABLE ChiTietDonThuoc (
    MaPhieuKham  INT           NOT NULL,
    MaThuoc      INT           NOT NULL,
    SoLuong      INT           DEFAULT 1,
    LieuDung     NVARCHAR(100),
    PRIMARY KEY (MaPhieuKham, MaThuoc),
    FOREIGN KEY (MaPhieuKham) REFERENCES PhieuKham(MaPhieuKham),
    FOREIGN KEY (MaThuoc)     REFERENCES Thuoc(MaThuoc)
);

CREATE TABLE HinhAnhBenhLy (
    MaHinhAnh    INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuKham  INT            NOT NULL,
    DuongDanAnh  NVARCHAR(500)  NOT NULL,
    GhiChu       NVARCHAR(200),
    NgayChup     DATETIME       DEFAULT GETDATE(),
    FOREIGN KEY (MaPhieuKham) REFERENCES PhieuKham(MaPhieuKham)
);

-- ============================================================
-- PHẦN 4: NHÓM 4 - TÀI CHÍNH
-- ============================================================

CREATE TABLE ChiPhiHoatDong (
    MaChiPhi   INT IDENTITY(1,1) PRIMARY KEY,
    LoaiChiPhi NVARCHAR(100) NOT NULL,
    SoTien     DECIMAL(18,2) NOT NULL,
    NgayChi    DATETIME      DEFAULT GETDATE(),
    GhiChu     NVARCHAR(255),
    NguoiTao   INT           NOT NULL,
    FOREIGN KEY (NguoiTao) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE HoaDon (
    MaHoaDon               INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuKham            INT           NOT NULL UNIQUE,
    TongTien               DECIMAL(18,2) NOT NULL,
    TongTienDichVu         DECIMAL(18,2) DEFAULT 0,
    TongThuoc              DECIMAL(18,2) DEFAULT 0,
    GiamGia                DECIMAL(18,2) DEFAULT 0,
    TienKhachTra           DECIMAL(18,2) NOT NULL,
    TienThua               DECIMAL(18,2) DEFAULT 0,
    PhuongThucThanhToan    NVARCHAR(50),
    NgayThanhToan          DATETIME,
    NgayTao                DATETIME      DEFAULT GETDATE(),
    TrangThai              BIT           DEFAULT 0,
    IsDeleted              BIT           DEFAULT 0,
    FOREIGN KEY (MaPhieuKham) REFERENCES PhieuKham(MaPhieuKham)
);

-- ============================================================
-- PHẦN 5: NHÓM 5 - HỆ THỐNG THÀNH VIÊN & XÁC THỰC
-- ============================================================

CREATE TABLE HangThanhVien (
    MaHang                INT IDENTITY(1,1) PRIMARY KEY,
    TenHang               NVARCHAR(50) NOT NULL,
    DiemToiThieu          INT NOT NULL DEFAULT 0,
    MauHangHex            VARCHAR(7),
    GhiChu                NVARCHAR(255),
    PhanTramGiamDuocPham  DECIMAL(5,2) DEFAULT 0,
    PhanTramGiamTongHD    DECIMAL(5,2) DEFAULT 0,
    GiamGiaCodinh         INT DEFAULT 0,
    GhiChuKhuyenMai       NVARCHAR(255) NULL
);

CREATE TABLE ThanhVienInfo (
    MaThanhVien     INT IDENTITY(1,1) PRIMARY KEY,
    MaBenhNhan      INT           NOT NULL UNIQUE,
    MaHang          INT           NOT NULL DEFAULT 1,
    DiemTichLuy     INT           NOT NULL DEFAULT 0,
    SoLanKham       INT           NOT NULL DEFAULT 0,
    DuongDanAvatar  NVARCHAR(500),
    TyLeHaiLong     DECIMAL(5,2)  DEFAULT 0,
    NgayTaoTaiKhoan DATETIME      DEFAULT GETDATE(),
    FOREIGN KEY (MaBenhNhan) REFERENCES BenhNhan(MaBenhNhan),
    FOREIGN KEY (MaHang)     REFERENCES HangThanhVien(MaHang)
);

CREATE TABLE XacThucOTP (
    MaXacThuc       INT IDENTITY(1,1) PRIMARY KEY,
    SoDienThoai     VARCHAR(15)   NOT NULL,
    MaOTP           VARCHAR(6)    NOT NULL,
    TrangThai       TINYINT       DEFAULT 0,
    SoLanSai        INT           DEFAULT 0,
    NgayTao         DATETIME      DEFAULT GETDATE(),
    NgayHetHan      DATETIME
);

CREATE TABLE DanhGia (
    MaDanhGia      INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuKham    INT           NOT NULL,
    MaBenhNhan     INT           NOT NULL,
    DiemDanh       TINYINT       NOT NULL,
    NhanXet        NVARCHAR(500),
    NgayDanhGia    DATETIME      DEFAULT GETDATE(),
    FOREIGN KEY (MaPhieuKham) REFERENCES PhieuKham(MaPhieuKham),
    FOREIGN KEY (MaBenhNhan)  REFERENCES BenhNhan(MaBenhNhan)
);

CREATE TABLE ThongTinPhongKham (
    MaThongTin          INT IDENTITY(1,1) PRIMARY KEY,
    TenPhongKham        NVARCHAR(100) NOT NULL DEFAULT N'DarmaSoft Clinic',
    Logo                NVARCHAR(500),
    Slogan              NVARCHAR(255),
    DiaChi              NVARCHAR(300),
    SoDienThoai         VARCHAR(15),
    Email               VARCHAR(100),
    Website             VARCHAR(100),
    GioMoCua            TIME,
    GioDongCua          TIME,
    LichLamViecHangTuan NVARCHAR(200),
    MoTa                NVARCHAR(1000),
    DatCapNhatLuc       DATETIME DEFAULT GETDATE()
);

-- ============================================================
-- PHẦN 6: BẢNG LỊCH SỬ CHO KIỂM TOÁN
-- ============================================================

CREATE TABLE PhieuKham_LichSu (
    MaPhieuKham  INT,
    MaBenhNhan   INT,
    MaNguoiDung  INT,
    MaLichHen    INT,
    NgayKham     DATETIME,
    TrieuChung   NVARCHAR(500),
    ChanDoan     NVARCHAR(500),
    NgayTaiKham  DATE,
    TrangThai    TINYINT,
    ThoiGianBatDau DATETIME2,
    ThoiGianKetThuc DATETIME2
);

CREATE TABLE HoaDon_LichSu (
    MaHoaDon               INT,
    MaPhieuKham            INT,
    TongTien               DECIMAL(18,2),
    GiamGia                DECIMAL(18,2),
    TienKhachTra           DECIMAL(18,2),
    PhuongThucThanhToan    NVARCHAR(50),
    NgayThanhToan          DATETIME,
    TrangThai              BIT,
    ThoiGianBatDau DATETIME2,
    ThoiGianKetThuc DATETIME2
);

CREATE TABLE NhapKho_ChinhSua (
    MaChinhSua INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuNhap INT NOT NULL,
    MaThuoc INT NOT NULL,
    SoLuongCu INT NOT NULL,
    SoLuongMoi INT NOT NULL,
    LyDo NVARCHAR(500),
    NgayChinhSua DATETIME DEFAULT GETDATE(),
    NguoiChinhSua INT NOT NULL,
    FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhapKho(MaPhieuNhap),
    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc),
    FOREIGN KEY (NguoiChinhSua) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE GhiChuKham (
    MaGhiChu INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuKham INT NOT NULL,
    NoiDung NVARCHAR(MAX),
    NgayGhi DATETIME DEFAULT GETDATE(),
    BacSiGhi INT NOT NULL,
    FOREIGN KEY (MaPhieuKham) REFERENCES PhieuKham(MaPhieuKham),
    FOREIGN KEY (BacSiGhi) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE LichHen_Notification (
    MaThongBao INT IDENTITY(1,1) PRIMARY KEY,
    MaLichHen INT NOT NULL,
    LoaiThongBao NVARCHAR(50),
    TrangThai INT DEFAULT 0,
    NgayGui DATETIME,
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaLichHen) REFERENCES LichHen(MaLichHen)
);

-- ============================================================
-- PHẦN 7: INDEXES CHO HIỆU NĂNG (NguoiDung)
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_NguoiDung_SoDienThoai')
    CREATE INDEX IDX_NguoiDung_SoDienThoai ON NguoiDung(SoDienThoai);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_NguoiDung_Email')
    CREATE INDEX IDX_NguoiDung_Email ON NguoiDung(Email);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_NguoiDung_EmailConfirmToken')
    CREATE INDEX IDX_NguoiDung_EmailConfirmToken ON NguoiDung(EmailConfirmToken);
GO

-- ============================================================
-- PHẦN 8: DỮ LIỆU KHỞI TẠO
-- ============================================================

-- Tạo VaiTro
INSERT INTO VaiTro (TenVaiTro) VALUES (N'Admin');
INSERT INTO VaiTro (TenVaiTro) VALUES (N'Bác Sĩ');
INSERT INTO VaiTro (TenVaiTro) VALUES (N'Lễ Tân');
GO

-- Tạo HangThanhVien (bao gồm khuyến mãi theo hạng)
INSERT INTO HangThanhVien (TenHang, DiemToiThieu, MauHangHex, PhanTramGiamDuocPham, PhanTramGiamTongHD, GiamGiaCodinh, GhiChuKhuyenMai)
VALUES (N'Thành Viên Đỏ', 0, '#FF4C4C', 0, 0, 100000, N'Giảm 100.000đ cho hóa đơn đầu tiên');
INSERT INTO HangThanhVien (TenHang, DiemToiThieu, MauHangHex, PhanTramGiamDuocPham, PhanTramGiamTongHD, GiamGiaCodinh, GhiChuKhuyenMai)
VALUES (N'Thành Viên Bạc', 300, '#C0C0C0', 5, 0, 0, N'Giảm 5% sản phẩm dược phẩm (không áp dụng dịch vụ chăm sóc da)');
INSERT INTO HangThanhVien (TenHang, DiemToiThieu, MauHangHex, PhanTramGiamDuocPham, PhanTramGiamTongHD, GiamGiaCodinh, GhiChuKhuyenMai)
VALUES (N'Thành Viên Vàng', 1000, '#FFD700', 10, 0, 0, N'Giảm 10% sản phẩm dược phẩm (không áp dụng dịch vụ chăm sóc da)');
INSERT INTO HangThanhVien (TenHang, DiemToiThieu, MauHangHex, PhanTramGiamDuocPham, PhanTramGiamTongHD, GiamGiaCodinh, GhiChuKhuyenMai)
VALUES (N'Thành Viên Kim Cương', 5000, '#89CFF0', 0, 10, 0, N'Giảm 10% tổng hóa đơn');
GO

-- Tạo ThongTinPhongKham
INSERT INTO ThongTinPhongKham (TenPhongKham, Slogan, DiaChi, SoDienThoai, Email, Website, GioMoCua, GioDongCua)
VALUES (N'DarmaSoft Clinic', N'Chăm Sóc Da - SStyle Sống', N'123 Đường Nguyễn Hữu Cảnh, Q.Bình Thạnh, TP.HCM', '0909123456', 'contact@dermasoft.com', 'dermasoft.vn', '08:00:00', '17:00:00');
GO

-- ✅ TẠO ADMIN USER (SỬ DỤNG BCRYPT PASSWORD)
INSERT INTO NguoiDung 
(HoTen, SoDienThoai, Email, TenDangNhap, MatKhau, MaVaiTro, TrangThaiTK, EmailVerifiedAt, LastEmailVerificationAt, EmailConfirmationNeeded)
VALUES 
(N'Admin DarmaSoft Clinic', '0900000000', 'admin@darmaclinic.vn', 'aB1cD', 
 '$2y$10$p6N.Q6L7Q0a5K8M9d2Z7.OxXz9e2r5q8k7j3n4m5l6b2c9d8e7f6g5', 
 1, 1, GETDATE(), GETDATE(), 0);
GO

-- Tạo CaLamViec
INSERT INTO CaLamViec (TenCa, GioBatDau, GioKetThuc) 
VALUES (N'Ca Sáng', '08:00:00', '12:00:00');
INSERT INTO CaLamViec (TenCa, GioBatDau, GioKetThuc) 
VALUES (N'Ca Chiều', '13:00:00', '17:00:00');
INSERT INTO CaLamViec (TenCa, GioBatDau, GioKetThuc) 
VALUES (N'Ca Tối', '17:00:00', '21:00:00');
GO
Go
INSERT INTO NhaCungCap (TenNhaCungCap, SoDienThoaiLienHe, DiaChi)
VALUES 
(N'Công ty TNHH L''Oreal Việt Nam', '02839369142', N'Tầng 10, Tòa nhà Vincom, Quận 1, TP. HCM'),
(N'Công ty TNHH DKSH Việt Nam (Phân phối Eucerin)', '02838125830', N'Số 23 Đại lộ Độc Lập, KCN VSIP, Bình Dương'),
(N'La Roche-Posay Việt Nam', '1800545463', N'72 Lê Thánh Tôn, Bến Nghé, Quận 1, TP. HCM'),
(N'Vichy Laboratories Vietnam', '02839369143', N'Tòa nhà Gemadept, 6 Lê Thánh Tôn, Quận 1, TP. HCM'),
(N'Công ty TNHH Minthacare (Phân phối Bioderma)', '02862915486', N'Lầu 1, 40 Cao Thắng, Quận 3, TP. HCM'),
(N'Obagi Medical Vietnam', '1900633044', N'34-36 Nam Kỳ Khởi Nghĩa, Quận 1, TP. HCM'),
(N'Paula’s Choice Vietnam', '19006409', N'Tầng 1, 239 Cách Mạng Tháng 8, Quận 3, TP. HCM'),
(N'Công ty CP Dược Mỹ Phẩm Santen', '02838272645', N'Tòa nhà Bitexco, Quận 1, TP. HCM'),
(N'Dược Mỹ Phẩm Murad Vietnam', '1900633345', N'397B Võ Văn Tần, Quận 3, TP. HCM'),
(N'SkinCeuticals Vietnam', '02839369100', N'Tầng 45, Tòa nhà Bitexco Financial Tower, Quận 1, TP. HCM');
Go
PRINT '✅ DATABASE VÀ TABLES CREATED SUCCESSFULLY'
PRINT 'Admin User: aB1cD | Password: Admin@DarmaSoft2026'
GO
