-- ═══════════════════════════════════════════════════════════════════════
-- MIGRATION BỔ SUNG: TẠO PHIẾU NHẬP BÙ CHO THUỐC XUẤT > NHẬP
-- ═══════════════════════════════════════════════════════════════════════
-- Chạy SAU khi MIGRATION_FIX_TONKHO.sql đã COMMIT thành công.
--
-- 2 thuốc bị xuất vượt nhập (do dữ liệu test/seed không đầy đủ):
--   - Tretinoin Cream 0.05% (MaThuoc=41): kê 48, chỉ nhập 10 → thiếu 38
--   - Sunscreen SPF50+ (MaThuoc=48): kê 17, chỉ nhập 10 → thiếu 7
--
-- Script tạo phiếu nhập bù để dữ liệu nhất quán (Nhập ≥ Xuất).
-- Sau đó chạy lại MIGRATION_FIX_TONKHO.sql để FEFO tính lại đúng.
-- ═══════════════════════════════════════════════════════════════════════

SET NOCOUNT ON;
BEGIN TRANSACTION;

PRINT '════════════════════════════════════════════════════════════';
PRINT '  BƯỚC 1: XÁC ĐỊNH THUỐC CẦN NHẬP BÙ';
PRINT '════════════════════════════════════════════════════════════';

-- Tìm tất cả thuốc có Xuất > Nhập (động, không hardcode MaThuoc)
SELECT 
    t.MaThuoc, t.TenThuoc,
    ISNULL(n.TongNhap, 0) AS TongNhap,
    ISNULL(x.TongXuat, 0) AS TongXuat,
    ISNULL(x.TongXuat, 0) - ISNULL(n.TongNhap, 0) AS CanBu
INTO #ThuocCanBu
FROM Thuoc t
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongNhap FROM ChiTietNhapKho GROUP BY MaThuoc) n ON t.MaThuoc = n.MaThuoc
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongXuat FROM ChiTietDonThuoc GROUP BY MaThuoc) x ON t.MaThuoc = x.MaThuoc
WHERE ISNULL(x.TongXuat, 0) > ISNULL(n.TongNhap, 0);

IF NOT EXISTS (SELECT 1 FROM #ThuocCanBu)
BEGIN
    PRINT 'Không có thuốc nào cần nhập bù. Hoàn tất!';
    DROP TABLE #ThuocCanBu;
    COMMIT TRANSACTION;
    RETURN;
END

SELECT * FROM #ThuocCanBu;

PRINT '';
PRINT '════════════════════════════════════════════════════════════';
PRINT '  BƯỚC 2: TẠO PHIẾU NHẬP BÙ';
PRINT '════════════════════════════════════════════════════════════';

-- Lấy MaNhaCungCap và MaNguoiDung (Admin) đầu tiên để tạo phiếu
DECLARE @MaNCC INT, @MaNguoiDung INT;

SELECT TOP 1 @MaNCC = MaNhaCungCap FROM NhaCungCap;
SELECT TOP 1 @MaNguoiDung = MaNguoiDung FROM NguoiDung WHERE MaVaiTro = 1; -- Admin

IF @MaNCC IS NULL OR @MaNguoiDung IS NULL
BEGIN
    PRINT 'LỖI: Không tìm thấy NhaCungCap hoặc NguoiDung (Admin).';
    DROP TABLE #ThuocCanBu;
    ROLLBACK TRANSACTION;
    RETURN;
END

-- Tạo 1 phiếu nhập bù chung
DECLARE @MaPhieuNhap INT;

INSERT INTO PhieuNhapKho (MaNhaCungCap, MaNguoiDung, NgayNhap, TongGiaTri)
VALUES (@MaNCC, @MaNguoiDung, GETDATE(), 0);

SET @MaPhieuNhap = SCOPE_IDENTITY();
PRINT 'Tạo phiếu nhập bù: MaPhieuNhap = ' + CAST(@MaPhieuNhap AS VARCHAR);

-- Thêm chi tiết cho từng thuốc cần bù
INSERT INTO ChiTietNhapKho (MaPhieuNhap, MaThuoc, SoLuong, SoLuongConLai, GiaNhap, HanSuDung)
SELECT 
    @MaPhieuNhap,
    cb.MaThuoc,
    cb.CanBu,           -- Số lượng = phần thiếu
    0,                   -- SoLuongConLai = 0 (đã kê hết rồi, nhập bù cho khớp)
    0,                   -- GiaNhap = 0 (phiếu bù, không có giá thực)
    DATEADD(YEAR, 2, GETDATE())  -- HSD 2 năm từ hôm nay
FROM #ThuocCanBu cb;

PRINT 'Đã thêm ' + CAST(@@ROWCOUNT AS VARCHAR) + ' dòng chi tiết nhập bù.';

-- Trigger TRG_NhapKho_CapNhatTon sẽ tự cộng SoLuongTon
-- Nhưng SoLuongConLai = 0 nên ta cần trừ lại SoLuongTon
-- (vì trigger cộng SoLuong, không phải SoLuongConLai)
UPDATE t
SET t.SoLuongTon = t.SoLuongTon - cb.CanBu
FROM Thuoc t
JOIN #ThuocCanBu cb ON t.MaThuoc = cb.MaThuoc;

PRINT 'Đã điều chỉnh SoLuongTon (trừ lại phần trigger tự cộng).';

PRINT '';
PRINT '════════════════════════════════════════════════════════════';
PRINT '  BƯỚC 3: KIỂM TRA SAU BÙ';
PRINT '════════════════════════════════════════════════════════════';

-- Kiểm tra: Nhập - Xuất = Tồn (phải = 0 dòng)
PRINT '--- Nhập - Xuất = Tồn (phải = 0 dòng) ---';
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

-- Kiểm tra: Cache vs Thực tế (phải = 0 dòng)
PRINT '--- Cache vs Thực tế (phải = 0 dòng) ---';
SELECT 
    t.MaThuoc, t.TenThuoc,
    t.SoLuongTon AS TonCache,
    ISNULL(SUM(ctk.SoLuongConLai), 0) AS TonThucTe,
    t.SoLuongTon - ISNULL(SUM(ctk.SoLuongConLai), 0) AS ChenhLech
FROM Thuoc t
LEFT JOIN ChiTietNhapKho ctk ON t.MaThuoc = ctk.MaThuoc
GROUP BY t.MaThuoc, t.TenThuoc, t.SoLuongTon
HAVING t.SoLuongTon <> ISNULL(SUM(ctk.SoLuongConLai), 0);

-- Kết quả cuối cùng
PRINT '--- Kết quả cuối cùng ---';
SELECT 
    t.MaThuoc, t.TenThuoc,
    t.SoLuongTon AS TonKho,
    ISNULL(n.TongNhap, 0) AS TongNhap,
    ISNULL(x.TongXuat, 0) AS TongXuat,
    ISNULL(n.TongNhap, 0) - ISNULL(x.TongXuat, 0) AS [Nhap-Xuat]
FROM Thuoc t
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongNhap FROM ChiTietNhapKho GROUP BY MaThuoc) n ON t.MaThuoc = n.MaThuoc
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongXuat FROM ChiTietDonThuoc GROUP BY MaThuoc) x ON t.MaThuoc = x.MaThuoc
WHERE ISNULL(n.TongNhap, 0) > 0
ORDER BY t.TenThuoc;

DROP TABLE #ThuocCanBu;

-- ═══════════════════════════════════════════════════════════════════════
-- Kiểm tra kết quả rồi quyết định:
COMMIT TRANSACTION;
PRINT '✅ ĐÃ COMMIT — Nhập bù hoàn tất!';

-- Nếu có lỗi → UNCOMMENT dòng dưới, COMMENT COMMIT:
-- ROLLBACK TRANSACTION;
-- PRINT '❌ ĐÃ ROLLBACK.';
