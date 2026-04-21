-- ═══════════════════════════════════════════════════════════════════════
-- 🔧 ĐỒNG BỘ DOANH THU App ↔ Web
-- ═══════════════════════════════════════════════════════════════════════
-- 
-- ⚠️  PHÁT HIỆN: Web đang TRỪ GiamGia 2 LẦN
--
-- InvoiceForm lưu:
--   TongTienDichVu = 500,000  (DV gốc)
--   TongThuoc      = 200,000  (T gốc)
--   GiamGia        =  50,000  (GG)
--   TongTien       = 650,000  (= DV+T-GG, ĐÃ trừ GG rồi!)
--
-- App:  SUM(TongTien)         = 650,000   ✅ Đúng
-- Web:  SUM(TongTien - GG)    = 600,000   ❌ Trừ GG lần 2!
--
-- → CÔNG THỨC ĐÚNG: SUM(TongTien) 
-- → WEB CẦN SỬA: bỏ "- GiamGia" trong query doanh thu
--
-- Script này:
--   1. Fix NgayThanhToan NULL
--   2. Kiểm tra & hiển thị doanh thu đúng
-- ═══════════════════════════════════════════════════════════════════════

SET NOCOUNT ON;

PRINT '════════════════════════════════════════════════════════════';
PRINT '  🔧 ĐỒNG BỘ DOANH THU App ↔ Web';
PRINT '  Thời gian: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '════════════════════════════════════════════════════════════';
PRINT '';

-- ── BƯỚC 1: Fix NgayThanhToan NULL cho hóa đơn đã thanh toán ──
DECLARE @soNull INT;
SELECT @soNull = COUNT(*) FROM HoaDon
WHERE TrangThai = 1 AND NgayThanhToan IS NULL AND IsDeleted = 0;

PRINT '  HĐ có TrangThai=1 nhưng NgayThanhToan NULL: ' + CAST(@soNull AS VARCHAR);

UPDATE HoaDon
SET NgayThanhToan = NgayTao
WHERE TrangThai = 1 AND NgayThanhToan IS NULL AND IsDeleted = 0;

PRINT '  ✅ Đã fill NgayThanhToan cho ' + CAST(@@ROWCOUNT AS VARCHAR) + ' hóa đơn.';
PRINT '';

-- ── BƯỚC 2: Kiểm tra tính đúng đắn của TongTien ──
PRINT '--- Hóa đơn có TongTien ≠ (DV + Thuoc - GG) ---';
PRINT '  Các hóa đơn này có thể có giảm giá ẩn (áp dụng nhưng không ghi vào cột GiamGia)';

SELECT
    MaHoaDon,
    TongTienDichVu AS [DV],
    TongThuoc AS [Thuoc],
    GiamGia AS [GG],
    TongTien AS [TongTien_HienTai],
    (ISNULL(TongTienDichVu,0) + ISNULL(TongThuoc,0) - ISNULL(GiamGia,0)) AS [DV+T-GG],
    TongTien - (ISNULL(TongTienDichVu,0) + ISNULL(TongThuoc,0) - ISNULL(GiamGia,0)) AS [Chenh]
FROM HoaDon
WHERE IsDeleted = 0
  AND ABS(TongTien - (ISNULL(TongTienDichVu,0) + ISNULL(TongThuoc,0) - ISNULL(GiamGia,0))) > 0.01;

PRINT '';

-- ── BƯỚC 3: Doanh thu đúng ──
PRINT '════════════════════════════════════════════════════════════';
PRINT '  📊 DOANH THU (công thức: SUM(TongTien))';
PRINT '  TongTien đã = DV + Thuốc - GiảmGiá (InvoiceForm tính sẵn)';
PRINT '════════════════════════════════════════════════════════════';

-- Tổng toàn bộ
SELECT
    COUNT(*)                             AS [Số HĐ đã TT],
    FORMAT(SUM(TongTien), 'N0')          AS [DoanhThu_Dung (SUM TongTien)],
    FORMAT(SUM(TongTien - ISNULL(GiamGia,0)), 'N0')  AS [DoanhThu_Sai (TongTien-GG, trừ 2 lần!)],
    FORMAT(SUM(GiamGia), 'N0')           AS [Chênh lệch = SUM(GiamGia)]
FROM HoaDon
WHERE IsDeleted = 0 AND TrangThai = 1;

PRINT '';

-- Theo tháng
PRINT '--- Theo tháng ---';
SELECT
    FORMAT(COALESCE(NgayThanhToan, NgayTao), 'yyyy-MM') AS [Tháng],
    COUNT(*)                              AS [Số HĐ],
    FORMAT(SUM(TongTien), 'N0')           AS [DoanhThu_Dung],
    FORMAT(SUM(TongTien - ISNULL(GiamGia,0)), 'N0') AS [DoanhThu_Sai_Web],
    FORMAT(SUM(GiamGia), 'N0')            AS [TongGiamGia]
FROM HoaDon
WHERE IsDeleted = 0 AND TrangThai = 1
GROUP BY FORMAT(COALESCE(NgayThanhToan, NgayTao), 'yyyy-MM')
ORDER BY [Tháng] DESC;

PRINT '';
PRINT '════════════════════════════════════════════════════════════';
PRINT '  ⚠️  NẾU "DoanhThu_Sai_Web" KHỚP VỚI SỐ WEB ĐANG HIỆN';
PRINT '  → Web đang dùng TongTien - GiamGia (trừ 2 lần!)';
PRINT '  → SỬA WEB: đổi sang SUM(TongTien)';
PRINT '  → App đã dùng SUM(TongTien) → ĐÚNG';
PRINT '════════════════════════════════════════════════════════════';
