USE DERMASOFT;
GO

PRINT N'========== MIGRATION: BẢNG LƯƠNG =========='
GO

-- ─────────────────────────────────────────────────────────────
-- 1. TẠO BẢNG CauHinhLuong
-- ─────────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'CauHinhLuong')
BEGIN
    CREATE TABLE CauHinhLuong (
        MaCauHinh       INT IDENTITY(1,1) PRIMARY KEY,
        MaVaiTro        INT            NOT NULL,
        LoaiTinhLuong   NVARCHAR(20)   NOT NULL,
        DonGia          DECIMAL(18,2)  NOT NULL,
        HeSoTangCa      DECIMAL(3,1)   DEFAULT 1.5,
        HeSoNgayLe      DECIMAL(3,1)   DEFAULT 2.0,
        SoGioChuanNgay  INT            DEFAULT 8,
        SoCaChuanNgay   INT            DEFAULT 2,
        NgayHieuLuc     DATE           NOT NULL,
        GhiChu          NVARCHAR(255),
        FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
    );

    -- Seed đơn giá mặc định
    INSERT INTO CauHinhLuong (MaVaiTro, LoaiTinhLuong, DonGia, HeSoTangCa, HeSoNgayLe, SoGioChuanNgay, SoCaChuanNgay, NgayHieuLuc, GhiChu)
    VALUES
    (1, 'THEO_THANG', 25000000, 1.0, 1.0, 0, 0, '2026-04-01', N'Admin — lương cố định 25tr/tháng'),
    (2, 'THEO_BN',       250000, 1.5, 2.0, 8, 2, '2026-04-01', N'Bác Sĩ — 250k/BN hoàn thành, tăng ca ×1.5'),
    (3, 'THEO_GIO',       50000, 1.5, 2.0, 8, 2, '2026-04-01', N'Lễ Tân — 50k/giờ, tăng ca ×1.5');

    PRINT N'✓ Đã tạo bảng CauHinhLuong + seed 3 mức lương';
END
ELSE
    PRINT N'⊘ Bảng CauHinhLuong đã tồn tại — bỏ qua';
GO

-- ─────────────────────────────────────────────────────────────
-- 2. TẠO BẢNG BangLuong
-- ─────────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'BangLuong')
BEGIN
    CREATE TABLE BangLuong (
        MaBangLuong    INT IDENTITY(1,1) PRIMARY KEY,
        MaNguoiDung    INT            NOT NULL,
        MaVaiTro       INT            NOT NULL,
        ThangNam       DATE           NOT NULL,
        LoaiTinhLuong  NVARCHAR(20)   NOT NULL,
        DonGia         DECIMAL(18,2)  NOT NULL,
        HeSoTangCa     DECIMAL(3,1)   DEFAULT 1.5,
        -- Số liệu Bác Sĩ
        SoBenhNhan     INT            DEFAULT 0,
        SoBNTangCa     INT            DEFAULT 0,
        -- Số liệu Lễ Tân
        SoGioLam       DECIMAL(10,2)  DEFAULT 0,
        SoGioTangCa    DECIMAL(10,2)  DEFAULT 0,
        -- Chuyên cần
        SoCaDiemDanh   INT            DEFAULT 0,
        SoCaVang       INT            DEFAULT 0,
        -- Tiền
        LuongChinh     DECIMAL(18,2)  DEFAULT 0,
        LuongTangCa    DECIMAL(18,2)  DEFAULT 0,
        ThuongThem     DECIMAL(18,2)  DEFAULT 0,
        KhauTru        DECIMAL(18,2)  DEFAULT 0,
        TongLuong      DECIMAL(18,2)  DEFAULT 0,
        -- Trạng thái
        GhiChu         NVARCHAR(500),
        TrangThai      TINYINT        DEFAULT 0,
        NgayTao        DATETIME       DEFAULT GETDATE(),
        NguoiDuyet     INT            NULL,
        NgayDuyet      DATETIME       NULL,
        FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
        FOREIGN KEY (MaVaiTro)    REFERENCES VaiTro(MaVaiTro),
        FOREIGN KEY (NguoiDuyet)  REFERENCES NguoiDung(MaNguoiDung),
        CONSTRAINT UQ_BangLuong_NV_Thang UNIQUE (MaNguoiDung, ThangNam)
    );

    PRINT N'✓ Đã tạo bảng BangLuong';
END
ELSE
    PRINT N'⊘ Bảng BangLuong đã tồn tại — bỏ qua';
GO

-- ─────────────────────────────────────────────────────────────
-- 3. TẠO BẢNG LichSuTraLuong
-- ─────────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'LichSuTraLuong')
BEGIN
    CREATE TABLE LichSuTraLuong (
        MaTraLuong   INT IDENTITY(1,1) PRIMARY KEY,
        MaBangLuong  INT            NOT NULL,
        SoTienTra    DECIMAL(18,2)  NOT NULL,
        PhuongThuc   NVARCHAR(50),
        NgayTra      DATETIME       DEFAULT GETDATE(),
        NguoiTra     INT            NOT NULL,
        GhiChu       NVARCHAR(255),
        FOREIGN KEY (MaBangLuong) REFERENCES BangLuong(MaBangLuong),
        FOREIGN KEY (NguoiTra)    REFERENCES NguoiDung(MaNguoiDung)
    );

    PRINT N'✓ Đã tạo bảng LichSuTraLuong';
END
ELSE
    PRINT N'⊘ Bảng LichSuTraLuong đã tồn tại — bỏ qua';
GO

-- ═══════════════════════════════════════════════════════════════
PRINT N''
PRINT N'========================================='
PRINT N'  MIGRATION BẢNG LƯƠNG HOÀN TẤT'
PRINT N'========================================='
PRINT N''
PRINT N'BƯỚC TIẾP THEO:'
PRINT N'  Chạy 05_Constraints.sql để thêm CHECK + INDEX cho bảng lương'
PRINT N''
GO
