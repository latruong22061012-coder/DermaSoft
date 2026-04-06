USE DERMASOFT;
GO

PRINT '========== CREATING STORED PROCEDURES =========='
GO

-- ============================================================
-- DROP CÁC SPs CŨ (NẾU CÓ)
-- ============================================================
DROP PROCEDURE IF EXISTS SP_TaoTaiKhoan;
DROP PROCEDURE IF EXISTS SP_TaoTaiKhoanNhanVien;
DROP PROCEDURE IF EXISTS SP_KhoaTaiKhoanNhanVien;
DROP PROCEDURE IF EXISTS SP_MoTaiKhoanNhanVien;
DROP PROCEDURE IF EXISTS SP_CapNhatThongTinNhanVien;
DROP PROCEDURE IF EXISTS SP_LayThongTinNhanVien;
DROP PROCEDURE IF EXISTS SP_LayDanhSachNhanVien;
DROP PROCEDURE IF EXISTS SP_XacThucOTP;
DROP PROCEDURE IF EXISTS SP_GuiOTP;
DROP PROCEDURE IF EXISTS SP_DatLichHen;
DROP PROCEDURE IF EXISTS SP_TaoTaiKhoanNguoiDung;
DROP PROCEDURE IF EXISTS SP_KhoaTaiKhoanNguoiDung;
DROP PROCEDURE IF EXISTS SP_MoTaiKhoanNguoiDung;
DROP PROCEDURE IF EXISTS SP_CapNhatThongTinNguoiDung;
DROP PROCEDURE IF EXISTS SP_LayThongTinNguoiDung;
DROP PROCEDURE IF EXISTS SP_LayDanhSachNguoiDung;
DROP PROCEDURE IF EXISTS SP_LayNguoiDungTheoVaiTro;
DROP PROCEDURE IF EXISTS SP_GuiOTP_NguoiDung;
DROP PROCEDURE IF EXISTS SP_XacThucOTP_NguoiDung;
GO

-- ============================================================
-- 1. SP_TaoTaiKhoanNguoiDung - TẠO NGƯỜI DÙNG MỚI
-- ============================================================
PRINT 'Creating SP_TaoTaiKhoanNguoiDung...'
GO
CREATE PROCEDURE SP_TaoTaiKhoanNguoiDung
    @HoTen NVARCHAR(100),
    @SoDienThoai VARCHAR(15),
    @Email VARCHAR(100),
    @TenDangNhap VARCHAR(5),
    @MatKhau VARCHAR(255),
    @MaVaiTro INT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Kiểm tra TenDangNhap đã tồn tại
        IF EXISTS (SELECT 1 FROM NguoiDung WHERE TenDangNhap = @TenDangNhap)
        BEGIN
            RAISERROR(N'Tên đăng nhập đã tồn tại.', 16, 1);
            RETURN;
        END
        
        -- Kiểm tra SoDienThoai đã tồn tại
        IF EXISTS (SELECT 1 FROM NguoiDung WHERE SoDienThoai = @SoDienThoai)
        BEGIN
            RAISERROR(N'Số điện thoại đã được đăng ký.', 16, 1);
            RETURN;
        END

        -- Kiểm tra Email đã tồn tại
        IF EXISTS (SELECT 1 FROM NguoiDung WHERE Email = @Email)
        BEGIN
            RAISERROR(N'Email đã được đăng ký.', 16, 1);
            RETURN;
        END
        
        -- Tạo tài khoản mới
        INSERT INTO NguoiDung (HoTen, SoDienThoai, Email, TenDangNhap, MatKhau, MaVaiTro, TrangThaiTK, EmailVerifiedAt, LastEmailVerificationAt, EmailConfirmationNeeded)
        VALUES (@HoTen, @SoDienThoai, @Email, @TenDangNhap, @MatKhau, @MaVaiTro, 1, GETDATE(), GETDATE(), 0);
        
        DECLARE @MaNguoiDungTao INT = SCOPE_IDENTITY();
        
        SELECT @MaNguoiDungTao AS MaNguoiDung, N'Tài khoản đã được tạo thành công.' AS ThongBao;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END;
GO

-- ============================================================
-- 2. SP_KhoaTaiKhoanNguoiDung - KHÓA TÀI KHOẢN
-- ============================================================
PRINT 'Creating SP_KhoaTaiKhoanNguoiDung...'
GO
CREATE PROCEDURE SP_KhoaTaiKhoanNguoiDung
    @MaNguoiDung INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Kiểm tra người dùng có quyền admin không
        IF EXISTS (SELECT 1 FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung AND MaVaiTro = 1)
        BEGIN
            RAISERROR(N'Không thể khóa tài khoản admin!', 16, 1);
            RETURN;
        END
        
        -- Kiểm tra người dùng tồn tại
        IF NOT EXISTS (SELECT 1 FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung)
        BEGIN
            RAISERROR(N'Không tìm thấy người dùng với mã %d.', 16, 1, @MaNguoiDung);
            RETURN;
        END
        
        -- Khóa tài khoản
        UPDATE NguoiDung SET TrangThaiTK = 0 WHERE MaNguoiDung = @MaNguoiDung;
        
        SELECT N'Tài khoản đã bị khóa.' AS ThongBao;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END;
GO

-- ============================================================
-- 3. SP_MoTaiKhoanNguoiDung - MỞ KHÓA TÀI KHOẢN
-- ============================================================
PRINT 'Creating SP_MoTaiKhoanNguoiDung...'
GO
CREATE PROCEDURE SP_MoTaiKhoanNguoiDung
    @MaNguoiDung INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Kiểm tra người dùng tồn tại
        IF NOT EXISTS (SELECT 1 FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung)
        BEGIN
            RAISERROR(N'Không tìm thấy người dùng với mã %d.', 16, 1, @MaNguoiDung);
            RETURN;
        END
        
        -- Mở khóa tài khoản
        UPDATE NguoiDung SET TrangThaiTK = 1 WHERE MaNguoiDung = @MaNguoiDung;
        
        SELECT N'Tài khoản đã được mở khóa.' AS ThongBao;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END;
GO

-- ============================================================
-- 4. SP_CapNhatThongTinNguoiDung - CẬP NHẬT THÔNG TIN
-- ============================================================
PRINT 'Creating SP_CapNhatThongTinNguoiDung...'
GO
CREATE PROCEDURE SP_CapNhatThongTinNguoiDung
    @MaNguoiDung INT,
    @HoTen NVARCHAR(100) = NULL,
    @Email VARCHAR(100) = NULL,
    @SoDienThoai VARCHAR(15) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Kiểm tra người dùng tồn tại
        IF NOT EXISTS (SELECT 1 FROM NguoiDung WHERE MaNguoiDung = @MaNguoiDung)
        BEGIN
            RAISERROR(N'Không tìm thấy người dùng với mã %d.', 16, 1, @MaNguoiDung);
            RETURN;
        END
        
        -- Cập nhật thông tin
        UPDATE NguoiDung
        SET 
            HoTen = COALESCE(@HoTen, HoTen),
            Email = COALESCE(@Email, Email),
            SoDienThoai = COALESCE(@SoDienThoai, SoDienThoai)
        WHERE MaNguoiDung = @MaNguoiDung;
        
        SELECT N'Thông tin đã được cập nhật.' AS ThongBao;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END;
GO

-- ============================================================
-- 5. SP_LayThongTinNguoiDung - LẤY THÔNG TIN MỘT NGƯỜI DÙNG
-- ============================================================
PRINT 'Creating SP_LayThongTinNguoiDung...'
GO
CREATE PROCEDURE SP_LayThongTinNguoiDung
    @MaNguoiDung INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        SELECT 
            MaNguoiDung,
            HoTen,
            SoDienThoai,
            Email,
            TenDangNhap,
            MaVaiTro,
            TrangThaiTK,
            EmailVerifiedAt,
            LastEmailVerificationAt,
            NgayTao
        FROM NguoiDung
        WHERE MaNguoiDung = @MaNguoiDung;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END;
GO

-- ============================================================
-- 6. SP_LayDanhSachNguoiDung - LẤY DANH SÁCH NGƯỜI DÙNG
-- ============================================================
PRINT 'Creating SP_LayDanhSachNguoiDung...'
GO
CREATE PROCEDURE SP_LayDanhSachNguoiDung
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        SELECT 
            MaNguoiDung,
            HoTen,
            SoDienThoai,
            Email,
            TenDangNhap,
            MaVaiTro,
            TrangThaiTK,
            NgayTao
        FROM NguoiDung
        WHERE TrangThaiTK = 1
        ORDER BY HoTen;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END;
GO

-- ============================================================
-- 7. SP_LayNguoiDungTheoVaiTro - LẤY NGƯỜI DÙNG THEO VAI TRÒ
-- ============================================================
PRINT 'Creating SP_LayNguoiDungTheoVaiTro...'
GO
CREATE PROCEDURE SP_LayNguoiDungTheoVaiTro
    @MaVaiTro INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        SELECT 
            MaNguoiDung,
            HoTen,
            SoDienThoai,
            Email,
            TenDangNhap,
            MaVaiTro,
            TrangThaiTK
        FROM NguoiDung
        WHERE MaVaiTro = @MaVaiTro AND TrangThaiTK = 1
        ORDER BY HoTen;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END;
GO

-- ============================================================
-- 8. SP_GuiOTP_NguoiDung - GỬI OTP CHO XÁC THỰC
-- ============================================================
PRINT 'Creating SP_GuiOTP_NguoiDung...'
GO
CREATE PROCEDURE SP_GuiOTP_NguoiDung
    @SoDienThoai VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Xóa OTP cũ hết hạn
        DELETE FROM XacThucOTP
        WHERE SoDienThoai = @SoDienThoai AND NgayHetHan < GETDATE();
        
        -- Kiểm tra đã request quá nhiều lần (rate limiting)
        DECLARE @CountActive INT = 0;
        SELECT @CountActive = COUNT(*)
        FROM XacThucOTP
        WHERE SoDienThoai = @SoDienThoai AND TrangThai = 0 AND NgayHetHan > GETDATE();
        
        IF @CountActive >= 3
        BEGIN
            RAISERROR(N'Bạn đã yêu cầu quá nhiều lần. Vui lòng thử lại sau 1 phút.', 16, 1);
            RETURN;
        END
        
        -- Tạo OTP 6 chữ số
        DECLARE @MaOTP VARCHAR(6) = RIGHT('000000' + CAST(CAST(RAND() * 999999 AS INT) AS VARCHAR(6)), 6);
        
        WHILE LEN(@MaOTP) < 6
            SET @MaOTP = '0' + @MaOTP;
        
        -- Lưu vào DB
        INSERT INTO XacThucOTP (SoDienThoai, MaOTP, TrangThai, NgayTao, NgayHetHan, SoLanSai)
        VALUES (@SoDienThoai, @MaOTP, 0, GETDATE(), DATEADD(MINUTE, 5, GETDATE()), 0);
        
        DECLARE @MaXacThuc INT = SCOPE_IDENTITY();
        
        SELECT 
            @MaXacThuc AS MaXacThuc, 
            @MaOTP AS MaOTP, 
            DATEADD(MINUTE, 5, GETDATE()) AS NgayHetHan, 
            N'OTP đã được tạo. Có hiệu lực trong 5 phút.' AS ThongBao;
        
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END;
GO

-- ============================================================
-- 9. SP_XacThucOTP_NguoiDung - XÁC THỰC OTP
-- ============================================================
PRINT 'Creating SP_XacThucOTP_NguoiDung...'
GO
CREATE PROCEDURE SP_XacThucOTP_NguoiDung
    @SoDienThoai VARCHAR(15),
    @MaOTP VARCHAR(6)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Validate input
        IF ISNULL(LTRIM(@SoDienThoai), '') = '' OR ISNULL(LTRIM(@MaOTP), '') = ''
        BEGIN
            RAISERROR(N'Số điện thoại và mã OTP không được để trống.', 16, 1);
            RETURN;
        END
        
        IF LEN(@MaOTP) <> 6 OR NOT (ISNUMERIC(@MaOTP) = 1)
        BEGIN
            RAISERROR(N'Mã OTP phải có 6 chữ số.', 16, 1);
        END
        
        -- Lấy OTP gần nhất
        DECLARE @MaXacThuc INT, @TrangThai INT, @NgayHetHan DATETIME, @SoLanSai INT, @MaOTPDb VARCHAR(6);
        
        SELECT TOP 1 
            @MaXacThuc = MaXacThuc, 
            @TrangThai = TrangThai, 
            @NgayHetHan = NgayHetHan, 
            @SoLanSai = ISNULL(SoLanSai, 0), 
            @MaOTPDb = MaOTP
        FROM XacThucOTP
        WHERE SoDienThoai = @SoDienThoai AND TrangThai = 0
        ORDER BY NgayTao DESC;
        
        -- Kiểm tra OTP tồn tại
        IF @MaXacThuc IS NULL
        BEGIN
            RAISERROR(N'OTP không tồn tại hoặc đã được xác thực.', 16, 1);
            RETURN;
        END
        
        -- Kiểm tra OTP hết hạn
        IF GETDATE() > @NgayHetHan
        BEGIN
            UPDATE XacThucOTP SET TrangThai = 2 WHERE MaXacThuc = @MaXacThuc;
            RAISERROR(N'OTP đã hết hạn. Vui lòng yêu cầu OTP mới.', 16, 1);
            RETURN;
        END
        
        -- Kiểm tra số lần sai
        IF @SoLanSai >= 3
        BEGIN
            UPDATE XacThucOTP SET TrangThai = 2 WHERE MaXacThuc = @MaXacThuc;
            RAISERROR(N'OTP bị vô hiệu hóa sau 3 lần sai.', 16, 1);
            RETURN;
        End
        
        -- Kiểm tra OTP đúng
        IF @MaOTP <> @MaOTPDb
        BEGIN
            UPDATE XacThucOTP SET SoLanSai = SoLanSai + 1 WHERE MaXacThuc = @MaXacThuc;
            DECLARE @LoiSai NVARCHAR(200) = N'Mã OTP không đúng. Còn ' + CAST((3 - (@SoLanSai + 1)) AS NVARCHAR(1)) + N' lần.';
            RAISERROR(@LoiSai, 16, 1);
            RETURN;
        END
        
        -- Xác thực thành công
        UPDATE XacThucOTP SET TrangThai = 1, NgayTao = GETDATE() WHERE MaXacThuc = @MaXacThuc;
        
        SELECT N'Xác thực OTP thành công.' AS ThongBao;
        
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END;
GO

PRINT '✓ Tất cả Stored Procedures đã được tạo thành công!'
GO

-- ============================================================
-- SP_DatLichHen — Đặt lịch hẹn từ website (khách / thành viên)
-- Bảo vệ: Không ghi đè HoTen nếu BenhNhan đã tồn tại
-- ============================================================
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

        -- Tìm BenhNhan theo SĐT (không ghi đè HoTen nếu đã tồn tại)
        SELECT @MaBenhNhan = MaBenhNhan
        FROM   BenhNhan
        WHERE  SoDienThoai = @SoDienThoai AND IsDeleted = 0;

        IF @MaBenhNhan IS NULL
        BEGIN
            INSERT INTO BenhNhan (HoTen, SoDienThoai)
            VALUES (@HoTen, @SoDienThoai);
            SET @MaBenhNhan = SCOPE_IDENTITY();
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

PRINT '✓ SP_DatLichHen đã được tạo thành công!'
GO
