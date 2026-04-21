-- ═══════════════════════════════════════════════════════════════════════
-- 🔧 SCRIPT SỬA DỮ LIỆU TỒN KHO BỊ LỆCH — DERMASOFT
-- ═══════════════════════════════════════════════════════════════════════
-- Chạy script này trên SSMS để sửa dữ liệu bị lệch Nhập - Xuất ≠ Tồn.
-- Script sẽ tính lại SoLuongConLai cho các lô theo FEFO.
-- ═══════════════════════════════════════════════════════════════════════

SET NOCOUNT ON;

PRINT '════════════════════════════════════════════════════════════';
PRINT '  🔧 BẮT ĐẦU SỬA DỮ LIỆU TỒN KHO BỊ LỆCH';
PRINT '  Thời gian: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '════════════════════════════════════════════════════════════';
PRINT '';

-- Bước 1: Hiển thị thuốc bị lệch TRƯỚC khi sửa
PRINT '--- Thuốc bị lệch Nhập - Xuất ≠ Tồn (TRƯỚC khi sửa) ---';
SELECT
    t.MaThuoc, t.TenThuoc,
    ISNULL(n.S, 0) AS TongNhap,
    ISNULL(x.S, 0) AS TongXuat,
    ISNULL(r.S, 0) AS TongConLai,
    (ISNULL(n.S, 0) - ISNULL(x.S, 0)) AS ConLaiDung,
    ISNULL(r.S, 0) - (ISNULL(n.S, 0) - ISNULL(x.S, 0)) AS Lech
FROM Thuoc t
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS S FROM ChiTietNhapKho GROUP BY MaThuoc) n ON t.MaThuoc = n.MaThuoc
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS S FROM ChiTietDonThuoc GROUP BY MaThuoc) x ON t.MaThuoc = x.MaThuoc
LEFT JOIN (SELECT MaThuoc, SUM(SoLuongConLai) AS S FROM ChiTietNhapKho GROUP BY MaThuoc) r ON t.MaThuoc = r.MaThuoc
WHERE ISNULL(n.S, 0) > 0
  AND ISNULL(r.S, 0) <> (ISNULL(n.S, 0) - ISNULL(x.S, 0));

-- Bước 2: Sửa SoLuongConLai — reset tất cả lô của thuốc bị lệch rồi phân bổ lại theo FEFO
-- Với mỗi thuốc bị lệch: TongConLaiDung = TongNhap - TongXuat
-- Reset tất cả lô về SoLuong (đầy), rồi trừ đi TongXuat theo FEFO (lô sớm hết hạn trước)

DECLARE @MaThuocFix INT;

DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
    SELECT t.MaThuoc
    FROM Thuoc t
    LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS S FROM ChiTietNhapKho GROUP BY MaThuoc) n ON t.MaThuoc = n.MaThuoc
    LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS S FROM ChiTietDonThuoc GROUP BY MaThuoc) x ON t.MaThuoc = x.MaThuoc
    LEFT JOIN (SELECT MaThuoc, SUM(SoLuongConLai) AS S FROM ChiTietNhapKho GROUP BY MaThuoc) r ON t.MaThuoc = r.MaThuoc
    WHERE ISNULL(n.S, 0) > 0
      AND ISNULL(r.S, 0) <> (ISNULL(n.S, 0) - ISNULL(x.S, 0));

OPEN cur;
FETCH NEXT FROM cur INTO @MaThuocFix;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Tính tổng đã xuất
    DECLARE @TongXuat INT;
    SELECT @TongXuat = ISNULL(SUM(SoLuong), 0) FROM ChiTietDonThuoc WHERE MaThuoc = @MaThuocFix;

    -- Reset tất cả lô về đầy (SoLuongConLai = SoLuong)
    UPDATE ChiTietNhapKho SET SoLuongConLai = SoLuong WHERE MaThuoc = @MaThuocFix;

    -- Trừ theo FEFO (lô sớm hết hạn trước)
    DECLARE @Rem INT = @TongXuat;
    WHILE @Rem > 0
    BEGIN
        DECLARE @pn INT = NULL, @qty INT;
        SELECT TOP 1 @pn = MaPhieuNhap, @qty = SoLuongConLai
        FROM ChiTietNhapKho
        WHERE MaThuoc = @MaThuocFix AND SoLuongConLai > 0
        ORDER BY HanSuDung ASC;

        IF @pn IS NULL BREAK;

        DECLARE @d INT = CASE WHEN @qty >= @Rem THEN @Rem ELSE @qty END;
        UPDATE ChiTietNhapKho SET SoLuongConLai = SoLuongConLai - @d
        WHERE MaPhieuNhap = @pn AND MaThuoc = @MaThuocFix;
        SET @Rem = @Rem - @d;
    END

    -- Cập nhật cache Thuoc.SoLuongTon
    UPDATE Thuoc SET SoLuongTon = (
        SELECT ISNULL(SUM(SoLuongConLai), 0) FROM ChiTietNhapKho WHERE MaThuoc = @MaThuocFix
    ) WHERE MaThuoc = @MaThuocFix;

    PRINT '  ✅ Đã sửa MaThuoc = ' + CAST(@MaThuocFix AS VARCHAR);

    FETCH NEXT FROM cur INTO @MaThuocFix;
END

CLOSE cur;
DEALLOCATE cur;

-- Bước 3: Kiểm tra lại
PRINT '';
PRINT '--- Kiểm tra lại sau khi sửa ---';

DECLARE @soLech INT;
SELECT @soLech = COUNT(*) FROM (
    SELECT t.MaThuoc
    FROM Thuoc t
    LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS S FROM ChiTietNhapKho GROUP BY MaThuoc) n ON t.MaThuoc = n.MaThuoc
    LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS S FROM ChiTietDonThuoc GROUP BY MaThuoc) x ON t.MaThuoc = x.MaThuoc
    LEFT JOIN (SELECT MaThuoc, SUM(SoLuongConLai) AS S FROM ChiTietNhapKho GROUP BY MaThuoc) r ON t.MaThuoc = r.MaThuoc
    WHERE ISNULL(n.S, 0) > 0
      AND ISNULL(r.S, 0) <> (ISNULL(n.S, 0) - ISNULL(x.S, 0))
) x;

IF @soLech = 0
    PRINT '  ✅ Test 1.2 PASS — Nhập - Xuất = Tồn cho mọi thuốc!';
ELSE
    PRINT '  ❌ Vẫn còn ' + CAST(@soLech AS VARCHAR) + ' thuốc bị lệch!';

PRINT '';
PRINT '  Hoàn thành: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '════════════════════════════════════════════════════════════';
