-- ═══════════════════════════════════════════════════════════════════════
-- MIGRATION: SỬA CHỮA DỮ LIỆU TỒN KHO SAU BUG "KÊ ĐƠN KHÔNG TRỪ KHO"
-- ═══════════════════════════════════════════════════════════════════════
-- Nguyên nhân: BtnThemThuoc_Click (PhieuKhamForm) trước đây INSERT/UPDATE
--   ChiTietDonThuoc nhưng KHÔNG trừ ChiTietNhapKho.SoLuongConLai và 
--   Thuoc.SoLuongTon → tồn kho cache cao hơn thực tế.
--
-- Script này:
--   1. Phân tích chênh lệch hiện tại
--   2. Trừ ChiTietNhapKho.SoLuongConLai theo FEFO cho các đơn thuốc lịch sử
--   3. Đồng bộ Thuoc.SoLuongTon = SUM(ChiTietNhapKho.SoLuongConLai)
--
-- ⚠️ CHẠY TRONG TRANSACTION — có thể ROLLBACK nếu có lỗi
-- ═══════════════════════════════════════════════════════════════════════

SET NOCOUNT ON;
BEGIN TRANSACTION;

PRINT '════════════════════════════════════════════════════════════';
PRINT '  BƯỚC 0: PHÂN TÍCH TRƯỚC KHI SỬA';
PRINT '════════════════════════════════════════════════════════════';

-- Hiển thị chênh lệch hiện tại
SELECT 
    t.MaThuoc, t.TenThuoc,
    t.SoLuongTon AS TonCache_Truoc,
    ISNULL(ton.TonLo, 0) AS TonLo_Truoc,
    ISNULL(xuat.TongXuat, 0) AS TongDaKe,
    ISNULL(nhap.TongNhap, 0) AS TongDaNhap,
    ISNULL(nhap.TongNhap, 0) - ISNULL(xuat.TongXuat, 0) AS TonDungRa,
    t.SoLuongTon - (ISNULL(nhap.TongNhap, 0) - ISNULL(xuat.TongXuat, 0)) AS CacheLech
FROM Thuoc t
LEFT JOIN (SELECT MaThuoc, SUM(SoLuongConLai) AS TonLo FROM ChiTietNhapKho GROUP BY MaThuoc) ton ON t.MaThuoc = ton.MaThuoc
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongXuat FROM ChiTietDonThuoc GROUP BY MaThuoc) xuat ON t.MaThuoc = xuat.MaThuoc
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongNhap FROM ChiTietNhapKho GROUP BY MaThuoc) nhap ON t.MaThuoc = nhap.MaThuoc
WHERE t.SoLuongTon <> (ISNULL(nhap.TongNhap, 0) - ISNULL(xuat.TongXuat, 0))
ORDER BY ABS(t.SoLuongTon - (ISNULL(nhap.TongNhap, 0) - ISNULL(xuat.TongXuat, 0))) DESC;

PRINT '';
PRINT '════════════════════════════════════════════════════════════';
PRINT '  BƯỚC 1: TÍNH LẠI SoLuongConLai CHO TỪNG LÔ (FEFO)';
PRINT '════════════════════════════════════════════════════════════';

-- Chiến lược: Reset SoLuongConLai = SoLuong (ban đầu), rồi trừ lại
-- theo FEFO từ tất cả ChiTietDonThuoc

-- 1a. Reset tất cả lô về SoLuong gốc (= số nhập ban đầu)
UPDATE ChiTietNhapKho
SET SoLuongConLai = SoLuong;

PRINT 'Reset ' + CAST(@@ROWCOUNT AS VARCHAR) + ' lô về SoLuong gốc.';

-- 1b. Trừ từng đơn thuốc theo FEFO
-- Dùng cursor vì cần trừ tuần tự theo lô sớm hết hạn
DECLARE @MaThuoc INT, @SoLuongKe INT;
DECLARE @MaPhieuNhap INT, @ConLai INT, @TruBaoNhieu INT;
DECLARE @SoLuongCanTru INT;

DECLARE cur_DonThuoc CURSOR FOR
    SELECT ct.MaThuoc, ct.SoLuong
    FROM ChiTietDonThuoc ct
    ORDER BY ct.MaThuoc, ct.MaPhieuKham;

OPEN cur_DonThuoc;
FETCH NEXT FROM cur_DonThuoc INTO @MaThuoc, @SoLuongKe;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @SoLuongCanTru = @SoLuongKe;

    -- Trừ từ các lô FEFO (sớm hết hạn nhất trước)
    DECLARE cur_Lo CURSOR FOR
        SELECT MaPhieuNhap, SoLuongConLai
        FROM ChiTietNhapKho
        WHERE MaThuoc = @MaThuoc AND SoLuongConLai > 0
        ORDER BY HanSuDung ASC, MaPhieuNhap ASC;

    OPEN cur_Lo;
    FETCH NEXT FROM cur_Lo INTO @MaPhieuNhap, @ConLai;

    WHILE @@FETCH_STATUS = 0 AND @SoLuongCanTru > 0
    BEGIN
        IF @ConLai >= @SoLuongCanTru
        BEGIN
            -- Lô này đủ → trừ hết
            UPDATE ChiTietNhapKho
            SET SoLuongConLai = SoLuongConLai - @SoLuongCanTru
            WHERE MaThuoc = @MaThuoc AND MaPhieuNhap = @MaPhieuNhap;

            SET @SoLuongCanTru = 0;
        END
        ELSE
        BEGIN
            -- Lô này không đủ → trừ hết lô, chuyển sang lô tiếp
            SET @SoLuongCanTru = @SoLuongCanTru - @ConLai;

            UPDATE ChiTietNhapKho
            SET SoLuongConLai = 0
            WHERE MaThuoc = @MaThuoc AND MaPhieuNhap = @MaPhieuNhap;
        END

        FETCH NEXT FROM cur_Lo INTO @MaPhieuNhap, @ConLai;
    END

    CLOSE cur_Lo;
    DEALLOCATE cur_Lo;

    -- Nếu @SoLuongCanTru > 0 → đã kê nhiều hơn nhập (dữ liệu lỗi)
    IF @SoLuongCanTru > 0
    BEGIN
        PRINT 'CẢNH BÁO: Thuốc MaThuoc=' + CAST(@MaThuoc AS VARCHAR) 
            + ' kê vượt nhập ' + CAST(@SoLuongCanTru AS VARCHAR) + ' đơn vị!';
    END

    FETCH NEXT FROM cur_DonThuoc INTO @MaThuoc, @SoLuongKe;
END

CLOSE cur_DonThuoc;
DEALLOCATE cur_DonThuoc;

PRINT 'Hoàn thành trừ FEFO cho tất cả đơn thuốc.';

PRINT '';
PRINT '════════════════════════════════════════════════════════════';
PRINT '  BƯỚC 2: ĐỒNG BỘ Thuoc.SoLuongTon = SUM(SoLuongConLai)';
PRINT '════════════════════════════════════════════════════════════';

UPDATE t
SET t.SoLuongTon = ISNULL(ctk.TonThucTe, 0)
FROM Thuoc t
LEFT JOIN (
    SELECT MaThuoc, SUM(SoLuongConLai) AS TonThucTe
    FROM ChiTietNhapKho
    GROUP BY MaThuoc
) ctk ON t.MaThuoc = ctk.MaThuoc
WHERE t.SoLuongTon <> ISNULL(ctk.TonThucTe, 0);

PRINT 'Đã đồng bộ Thuoc.SoLuongTon cho ' + CAST(@@ROWCOUNT AS VARCHAR) + ' thuốc.';

PRINT '';
PRINT '════════════════════════════════════════════════════════════';
PRINT '  BƯỚC 3: KIỂM TRA SAU SỬA';
PRINT '════════════════════════════════════════════════════════════';

-- Kiểm tra 1: Cache vs thực tế
PRINT '--- Kiểm tra Cache vs Thực tế (phải = 0 dòng) ---';
SELECT 
    t.MaThuoc, t.TenThuoc,
    t.SoLuongTon AS TonCache_Sau,
    ISNULL(SUM(ctk.SoLuongConLai), 0) AS TonThucTe_Sau,
    t.SoLuongTon - ISNULL(SUM(ctk.SoLuongConLai), 0) AS ChenhLech
FROM Thuoc t
LEFT JOIN ChiTietNhapKho ctk ON t.MaThuoc = ctk.MaThuoc
GROUP BY t.MaThuoc, t.TenThuoc, t.SoLuongTon
HAVING t.SoLuongTon <> ISNULL(SUM(ctk.SoLuongConLai), 0);

-- Kiểm tra 2: Nhập - Xuất = Tồn
PRINT '--- Kiểm tra Nhập - Xuất = Tồn (phải = 0 dòng) ---';
SELECT 
    t.MaThuoc, t.TenThuoc,
    ISNULL(n.TongNhap, 0) AS TongNhap,
    ISNULL(x.TongXuat, 0) AS TongXuat,
    ISNULL(r.TongTon, 0) AS TonLo,
    t.SoLuongTon AS TonCache,
    ISNULL(n.TongNhap, 0) - ISNULL(x.TongXuat, 0) AS [Nhap-Xuat]
FROM Thuoc t
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongNhap FROM ChiTietNhapKho GROUP BY MaThuoc) n ON t.MaThuoc = n.MaThuoc
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongXuat FROM ChiTietDonThuoc GROUP BY MaThuoc) x ON t.MaThuoc = x.MaThuoc
LEFT JOIN (SELECT MaThuoc, SUM(SoLuongConLai) AS TongTon FROM ChiTietNhapKho GROUP BY MaThuoc) r ON t.MaThuoc = r.MaThuoc
WHERE ISNULL(n.TongNhap, 0) > 0
  AND ISNULL(r.TongTon, 0) <> (ISNULL(n.TongNhap, 0) - ISNULL(x.TongXuat, 0));

-- Kiểm tra 3: Không có lô âm
PRINT '--- Kiểm tra lô âm (phải = 0 dòng) ---';
SELECT ctk.MaThuoc, t.TenThuoc, ctk.MaPhieuNhap, ctk.SoLuongConLai
FROM ChiTietNhapKho ctk
JOIN Thuoc t ON ctk.MaThuoc = t.MaThuoc
WHERE ctk.SoLuongConLai < 0;

PRINT '';
PRINT '════════════════════════════════════════════════════════════';
PRINT '  KẾT QUẢ CUỐI CÙNG';
PRINT '════════════════════════════════════════════════════════════';

SELECT 
    t.MaThuoc, t.TenThuoc,
    t.SoLuongTon AS TonKho_DaSua,
    ISNULL(n.TongNhap, 0) AS TongNhap,
    ISNULL(x.TongXuat, 0) AS TongXuat
FROM Thuoc t
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongNhap FROM ChiTietNhapKho GROUP BY MaThuoc) n ON t.MaThuoc = n.MaThuoc
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongXuat FROM ChiTietDonThuoc GROUP BY MaThuoc) x ON t.MaThuoc = x.MaThuoc
WHERE ISNULL(n.TongNhap, 0) > 0
ORDER BY t.TenThuoc;

-- ═══════════════════════════════════════════════════════════════════════
-- ⚠️ XÁC NHẬN: Kiểm tra kết quả ở trên rồi quyết định COMMIT hay ROLLBACK
-- ═══════════════════════════════════════════════════════════════════════

-- Nếu kết quả ĐÚNG (0 dòng chênh lệch, 0 lô âm):
COMMIT TRANSACTION;
PRINT '✅ ĐÃ COMMIT — Tồn kho đã được sửa chữa thành công!';

-- Nếu kết quả SAI → UNCOMMENT dòng dưới, COMMENT dòng COMMIT ở trên:
-- ROLLBACK TRANSACTION;
-- PRINT '❌ ĐÃ ROLLBACK — Không có thay đổi nào được lưu.';
