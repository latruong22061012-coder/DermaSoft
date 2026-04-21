-- ═══════════════════════════════════════════════════════════════════════
-- 📦 SCRIPT KIỂM TRA TỰ ĐỘNG TỒN KHO — DERMASOFT
-- ═══════════════════════════════════════════════════════════════════════
-- Chạy script này trên SSMS sau mỗi lần test hoặc deploy.
-- Kết quả: Bảng tổng hợp PASS/FAIL cho từng test case.
-- ═══════════════════════════════════════════════════════════════════════

SET NOCOUNT ON;

-- Bảng kết quả test
IF OBJECT_ID('tempdb..#KetQuaTest') IS NOT NULL DROP TABLE #KetQuaTest;
CREATE TABLE #KetQuaTest (
    STT         INT IDENTITY(1,1),
    Phan        NVARCHAR(50),
    MaTest      NVARCHAR(10),
    TenTest     NVARCHAR(200),
    KetQua      NVARCHAR(10),  -- ✅ PASS / ❌ FAIL / ⚠️ WARN
    ChiTiet     NVARCHAR(500)
);

PRINT '════════════════════════════════════════════════════════════';
PRINT '  📦 BẮT ĐẦU KIỂM TRA TỒN KHO TỰ ĐỘNG';
PRINT '  Thời gian: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '════════════════════════════════════════════════════════════';
PRINT '';

-- ═══════════════════════════════════════════════════════════════════════
-- PHẦN 1: TÍNH TOÀN VẸN DỮ LIỆU (Data Integrity)
-- ═══════════════════════════════════════════════════════════════════════

-- 1.1: Cache vs Thực tế — Thuoc.SoLuongTon = SUM(ChiTietNhapKho.SoLuongConLai)
DECLARE @soLech1 INT;
SELECT @soLech1 = COUNT(*) FROM (
    SELECT t.MaThuoc
    FROM Thuoc t
    LEFT JOIN ChiTietNhapKho ctk ON t.MaThuoc = ctk.MaThuoc
    GROUP BY t.MaThuoc, t.SoLuongTon
    HAVING t.SoLuongTon <> ISNULL(SUM(ctk.SoLuongConLai), 0)
) x;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'1.ToànVẹn', '1.1', N'Cache (Thuoc.SoLuongTon) = Thực tế (SUM SoLuongConLai))',
    CASE WHEN @soLech1 = 0 THEN N'✅ PASS' ELSE N'❌ FAIL' END,
    CASE WHEN @soLech1 = 0 THEN N'0 thuốc chênh lệch'
         ELSE CAST(@soLech1 AS NVARCHAR) + N' thuốc bị lệch cache' END);

-- 1.2: Nhập - Xuất = Tồn
DECLARE @soLech2 INT;
SELECT @soLech2 = COUNT(*) FROM (
    SELECT t.MaThuoc
    FROM Thuoc t
    LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS S FROM ChiTietNhapKho GROUP BY MaThuoc) n ON t.MaThuoc = n.MaThuoc
    LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS S FROM ChiTietDonThuoc GROUP BY MaThuoc) x ON t.MaThuoc = x.MaThuoc
    LEFT JOIN (SELECT MaThuoc, SUM(SoLuongConLai) AS S FROM ChiTietNhapKho GROUP BY MaThuoc) r ON t.MaThuoc = r.MaThuoc
    WHERE ISNULL(n.S, 0) > 0
      AND ISNULL(r.S, 0) <> (ISNULL(n.S, 0) - ISNULL(x.S, 0))
) x;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'1.ToànVẹn', '1.2', N'Nhập − Xuất = Tồn (cho mọi thuốc có nhập)',
    CASE WHEN @soLech2 = 0 THEN N'✅ PASS' ELSE N'❌ FAIL' END,
    CASE WHEN @soLech2 = 0 THEN N'0 thuốc chênh lệch'
         ELSE CAST(@soLech2 AS NVARCHAR) + N' thuốc Nhập-Xuất ≠ Tồn' END);

-- 1.3: Không có lô âm (SoLuongConLai < 0)
DECLARE @soLoAm INT;
SELECT @soLoAm = COUNT(*) FROM ChiTietNhapKho WHERE SoLuongConLai < 0;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'1.ToànVẹn', '1.3', N'Không có lô SoLuongConLai < 0',
    CASE WHEN @soLoAm = 0 THEN N'✅ PASS' ELSE N'❌ FAIL' END,
    CASE WHEN @soLoAm = 0 THEN N'0 lô âm'
         ELSE CAST(@soLoAm AS NVARCHAR) + N' lô bị âm!' END);

-- 1.4: Không có lô SoLuongConLai > SoLuong (tồn vượt nhập)
DECLARE @soLoVuot INT;
SELECT @soLoVuot = COUNT(*) FROM ChiTietNhapKho WHERE SoLuongConLai > SoLuong;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'1.ToànVẹn', '1.4', N'Không có lô SoLuongConLai > SoLuong (tồn vượt nhập)',
    CASE WHEN @soLoVuot = 0 THEN N'✅ PASS' ELSE N'❌ FAIL' END,
    CASE WHEN @soLoVuot = 0 THEN N'0 lô bất thường'
         ELSE CAST(@soLoVuot AS NVARCHAR) + N' lô ConLai > SoLuong!' END);

-- 1.5: Thuoc.SoLuongTon không âm
DECLARE @soThuocAm INT;
SELECT @soThuocAm = COUNT(*) FROM Thuoc WHERE SoLuongTon < 0;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'1.ToànVẹn', '1.5', N'Thuoc.SoLuongTon >= 0 (không âm)',
    CASE WHEN @soThuocAm = 0 THEN N'✅ PASS' ELSE N'❌ FAIL' END,
    CASE WHEN @soThuocAm = 0 THEN N'0 thuốc âm'
         ELSE CAST(@soThuocAm AS NVARCHAR) + N' thuốc SoLuongTon < 0!' END);

-- ═══════════════════════════════════════════════════════════════════════
-- PHẦN 2: RÀNG BUỘC THAM CHIẾU (Referential Integrity)
-- ═══════════════════════════════════════════════════════════════════════

-- 2.1: ChiTietNhapKho.MaThuoc tồn tại trong Thuoc
DECLARE @orphanCTK INT;
SELECT @orphanCTK = COUNT(*) FROM ChiTietNhapKho ctk
WHERE NOT EXISTS (SELECT 1 FROM Thuoc t WHERE t.MaThuoc = ctk.MaThuoc);

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'2.ThamChiếu', '2.1', N'ChiTietNhapKho.MaThuoc → Thuoc (không orphan)',
    CASE WHEN @orphanCTK = 0 THEN N'✅ PASS' ELSE N'❌ FAIL' END,
    CASE WHEN @orphanCTK = 0 THEN N'0 orphan'
         ELSE CAST(@orphanCTK AS NVARCHAR) + N' dòng ChiTietNhapKho không có Thuoc!' END);

-- 2.2: ChiTietDonThuoc.MaThuoc tồn tại trong Thuoc
DECLARE @orphanCDT INT;
SELECT @orphanCDT = COUNT(*) FROM ChiTietDonThuoc cdt
WHERE NOT EXISTS (SELECT 1 FROM Thuoc t WHERE t.MaThuoc = cdt.MaThuoc);

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'2.ThamChiếu', '2.2', N'ChiTietDonThuoc.MaThuoc → Thuoc (không orphan)',
    CASE WHEN @orphanCDT = 0 THEN N'✅ PASS' ELSE N'❌ FAIL' END,
    CASE WHEN @orphanCDT = 0 THEN N'0 orphan'
         ELSE CAST(@orphanCDT AS NVARCHAR) + N' dòng ChiTietDonThuoc không có Thuoc!' END);

-- 2.3: ChiTietNhapKho.MaPhieuNhap tồn tại trong PhieuNhapKho
DECLARE @orphanPNK INT;
SELECT @orphanPNK = COUNT(*) FROM ChiTietNhapKho ctk
WHERE NOT EXISTS (SELECT 1 FROM PhieuNhapKho pnk WHERE pnk.MaPhieuNhap = ctk.MaPhieuNhap);

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'2.ThamChiếu', '2.3', N'ChiTietNhapKho.MaPhieuNhap → PhieuNhapKho (không orphan)',
    CASE WHEN @orphanPNK = 0 THEN N'✅ PASS' ELSE N'❌ FAIL' END,
    CASE WHEN @orphanPNK = 0 THEN N'0 orphan'
         ELSE CAST(@orphanPNK AS NVARCHAR) + N' dòng không có PhieuNhapKho!' END);

-- 2.4: ChiTietDonThuoc.MaPhieuKham tồn tại trong PhieuKham
DECLARE @orphanPK INT;
SELECT @orphanPK = COUNT(*) FROM ChiTietDonThuoc cdt
WHERE NOT EXISTS (SELECT 1 FROM PhieuKham pk WHERE pk.MaPhieuKham = cdt.MaPhieuKham);

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'2.ThamChiếu', '2.4', N'ChiTietDonThuoc.MaPhieuKham → PhieuKham (không orphan)',
    CASE WHEN @orphanPK = 0 THEN N'✅ PASS' ELSE N'❌ FAIL' END,
    CASE WHEN @orphanPK = 0 THEN N'0 orphan'
         ELSE CAST(@orphanPK AS NVARCHAR) + N' dòng không có PhieuKham!' END);

-- ═══════════════════════════════════════════════════════════════════════
-- PHẦN 3: HẠN SỬ DỤNG & FEFO
-- ═══════════════════════════════════════════════════════════════════════

-- 3.1: Số lô đã hết hạn nhưng còn tồn
DECLARE @loHetHanConTon INT;
SELECT @loHetHanConTon = COUNT(*) FROM ChiTietNhapKho
WHERE HanSuDung < GETDATE() AND SoLuongConLai > 0;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'3.HSD_FEFO', '3.1', N'Lô hết hạn nhưng còn tồn (cần xử lý)',
    CASE WHEN @loHetHanConTon = 0 THEN N'✅ PASS' ELSE N'⚠️ WARN' END,
    CASE WHEN @loHetHanConTon = 0 THEN N'0 lô hết hạn còn tồn'
         ELSE CAST(@loHetHanConTon AS NVARCHAR) + N' lô hết hạn vẫn còn tồn kho' END);

-- 3.2: Số lô sắp hết hạn (< 30 ngày)
DECLARE @loSapHetHan INT;
SELECT @loSapHetHan = COUNT(*) FROM ChiTietNhapKho
WHERE HanSuDung > GETDATE()
  AND DATEDIFF(DAY, GETDATE(), HanSuDung) < 30
  AND SoLuongConLai > 0;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'3.HSD_FEFO', '3.2', N'Lô sắp hết hạn (< 30 ngày)',
    CASE WHEN @loSapHetHan = 0 THEN N'✅ PASS' ELSE N'⚠️ WARN' END,
    CASE WHEN @loSapHetHan = 0 THEN N'0 lô sắp hết hạn'
         ELSE CAST(@loSapHetHan AS NVARCHAR) + N' lô sắp hết hạn trong 30 ngày' END);

-- 3.3: Lô không có HanSuDung (NULL)
DECLARE @loKhongHSD INT;
SELECT @loKhongHSD = COUNT(*) FROM ChiTietNhapKho WHERE HanSuDung IS NULL;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'3.HSD_FEFO', '3.3', N'Không có lô thiếu HanSuDung (NULL)',
    CASE WHEN @loKhongHSD = 0 THEN N'✅ PASS' ELSE N'❌ FAIL' END,
    CASE WHEN @loKhongHSD = 0 THEN N'0 lô thiếu HSD'
         ELSE CAST(@loKhongHSD AS NVARCHAR) + N' lô HanSuDung = NULL!' END);

-- 3.4: VW_TonKhoTheoLo view tồn tại
DECLARE @viewExists INT;
SELECT @viewExists = COUNT(*) FROM sys.views WHERE name = 'VW_TonKhoTheoLo';

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'3.HSD_FEFO', '3.4', N'View VW_TonKhoTheoLo tồn tại',
    CASE WHEN @viewExists = 1 THEN N'✅ PASS' ELSE N'❌ FAIL' END,
    CASE WHEN @viewExists = 1 THEN N'View tồn tại'
         ELSE N'View VW_TonKhoTheoLo KHÔNG tồn tại!' END);

-- ═══════════════════════════════════════════════════════════════════════
-- PHẦN 4: TRIGGER & CƠ CHẾ TỰ ĐỘNG
-- ═══════════════════════════════════════════════════════════════════════

-- 4.1: Trigger TRG_NhapKho_CapNhatTon tồn tại
DECLARE @triggerExists INT;
SELECT @triggerExists = COUNT(*) FROM sys.triggers WHERE name = 'TRG_NhapKho_CapNhatTon';

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'4.Trigger', '4.1', N'Trigger TRG_NhapKho_CapNhatTon tồn tại',
    CASE WHEN @triggerExists = 1 THEN N'✅ PASS' ELSE N'❌ FAIL' END,
    CASE WHEN @triggerExists = 1 THEN N'Trigger tồn tại trên ChiTietNhapKho'
         ELSE N'Trigger KHÔNG tồn tại — nhập kho sẽ không tự cập nhật SoLuongTon!' END);

-- 4.2: Trigger không bị disable
DECLARE @triggerDisabled INT, @triggerExists2 INT;
SELECT @triggerExists2 = COUNT(*) FROM sys.triggers WHERE name = 'TRG_NhapKho_CapNhatTon';
SELECT @triggerDisabled = COUNT(*) FROM sys.triggers
WHERE name = 'TRG_NhapKho_CapNhatTon' AND is_disabled = 1;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'4.Trigger', '4.2', N'Trigger TRG_NhapKho_CapNhatTon đang ENABLED',
    CASE WHEN @triggerDisabled = 0 AND @triggerExists2 = 1 THEN N'✅ PASS'
         WHEN @triggerExists2 = 0 THEN N'❌ FAIL'
         ELSE N'❌ FAIL' END,
    CASE WHEN @triggerDisabled = 0 AND @triggerExists2 = 1 THEN N'Trigger đang hoạt động'
         WHEN @triggerExists2 = 0 THEN N'Trigger không tồn tại'
         ELSE N'Trigger bị DISABLED!' END);

-- ═══════════════════════════════════════════════════════════════════════
-- PHẦN 5: THỐNG KÊ KHO
-- ═══════════════════════════════════════════════════════════════════════

-- 5.1: Tổng số thuốc trong hệ thống
DECLARE @tongThuoc INT, @thuocCoTon INT, @thuocHetTon INT;
SELECT @tongThuoc = COUNT(*) FROM Thuoc WHERE IsDeleted = 0;
SELECT @thuocCoTon = COUNT(*) FROM Thuoc WHERE IsDeleted = 0 AND SoLuongTon > 0;
SELECT @thuocHetTon = COUNT(*) FROM Thuoc WHERE IsDeleted = 0 AND SoLuongTon = 0;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'5.ThốngKê', '5.1', N'Tổng thuốc / Còn tồn / Hết tồn',
    N'📊 INFO',
    N'Tổng: ' + CAST(@tongThuoc AS NVARCHAR) 
    + N' | Còn tồn: ' + CAST(@thuocCoTon AS NVARCHAR) 
    + N' | Hết tồn: ' + CAST(@thuocHetTon AS NVARCHAR));

-- 5.2: Tổng số lô trong kho
DECLARE @tongLo INT, @loConTon INT, @loHet INT;
SELECT @tongLo = COUNT(*) FROM ChiTietNhapKho;
SELECT @loConTon = COUNT(*) FROM ChiTietNhapKho WHERE SoLuongConLai > 0;
SELECT @loHet = COUNT(*) FROM ChiTietNhapKho WHERE SoLuongConLai = 0;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'5.ThốngKê', '5.2', N'Tổng lô / Còn tồn / Đã hết',
    N'📊 INFO',
    N'Tổng lô: ' + CAST(@tongLo AS NVARCHAR) 
    + N' | Còn: ' + CAST(@loConTon AS NVARCHAR) 
    + N' | Hết: ' + CAST(@loHet AS NVARCHAR));

-- 5.3: Tổng giá trị tồn kho (SoLuongConLai * GiaNhap)
DECLARE @tongGiaTri DECIMAL(18,2);
SELECT @tongGiaTri = ISNULL(SUM(CAST(SoLuongConLai AS DECIMAL(18,2)) * GiaNhap), 0)
FROM ChiTietNhapKho WHERE SoLuongConLai > 0;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'5.ThốngKê', '5.3', N'Tổng giá trị tồn kho (SoLuongConLai × GiaNhap)',
    N'📊 INFO',
    N'Giá trị: ' + FORMAT(@tongGiaTri, 'N0') + N'đ');

-- 5.4: Tổng đơn thuốc đã kê
DECLARE @tongDonThuoc INT, @tongSLKe INT;
SELECT @tongDonThuoc = COUNT(*), @tongSLKe = ISNULL(SUM(SoLuong), 0) FROM ChiTietDonThuoc;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'5.ThốngKê', '5.4', N'Tổng dòng đơn thuốc / Tổng SL đã kê',
    N'📊 INFO',
    N'Dòng đơn: ' + CAST(@tongDonThuoc AS NVARCHAR) 
    + N' | Tổng SL kê: ' + CAST(@tongSLKe AS NVARCHAR));

-- 5.5: Số phiếu nhập kho
DECLARE @tongPhieuNhap INT;
SELECT @tongPhieuNhap = COUNT(*) FROM PhieuNhapKho;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'5.ThốngKê', '5.5', N'Tổng phiếu nhập kho',
    N'📊 INFO',
    N'Phiếu nhập: ' + CAST(@tongPhieuNhap AS NVARCHAR));

-- ═══════════════════════════════════════════════════════════════════════
-- PHẦN 6: CẢNH BÁO TỒN KHO THẤP
-- ═══════════════════════════════════════════════════════════════════════

-- 6.1: Thuốc tồn kho thấp (≤ 10)
DECLARE @thuocTonThap INT;
SELECT @thuocTonThap = COUNT(*) FROM Thuoc
WHERE IsDeleted = 0 AND SoLuongTon > 0 AND SoLuongTon <= 10;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'6.CảnhBáo', '6.1', N'Thuốc tồn kho thấp (SoLuongTon ≤ 10)',
    CASE WHEN @thuocTonThap = 0 THEN N'✅ PASS' ELSE N'⚠️ WARN' END,
    CASE WHEN @thuocTonThap = 0 THEN N'0 thuốc tồn thấp'
         ELSE CAST(@thuocTonThap AS NVARCHAR) + N' thuốc tồn ≤ 10' END);

-- 6.2: Thuốc có phiếu khám chưa hoàn thành nhưng đã trừ kho
DECLARE @phieuChuaHT INT;
SELECT @phieuChuaHT = COUNT(DISTINCT pk.MaPhieuKham) 
FROM PhieuKham pk
JOIN ChiTietDonThuoc cdt ON pk.MaPhieuKham = cdt.MaPhieuKham
WHERE pk.TrangThai IN (0, 1) AND pk.IsDeleted = 0;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'6.CảnhBáo', '6.2', N'Phiếu khám chưa hoàn thành có đơn thuốc (đã trừ kho)',
    N'📊 INFO',
    CAST(@phieuChuaHT AS NVARCHAR) + N' phiếu (TrangThai 0/1) có đơn thuốc đã trừ kho');

-- ═══════════════════════════════════════════════════════════════════════
-- PHẦN 7: KIỂM TRA XUẤT NHẬP THEO THỜI GIAN (30 ngày gần nhất)
-- ═══════════════════════════════════════════════════════════════════════

-- 7.1: Số lượng nhập trong 30 ngày
DECLARE @nhap30 INT;
SELECT @nhap30 = ISNULL(SUM(ctk.SoLuong), 0)
FROM ChiTietNhapKho ctk
JOIN PhieuNhapKho pnk ON ctk.MaPhieuNhap = pnk.MaPhieuNhap
WHERE pnk.NgayNhap >= DATEADD(DAY, -30, GETDATE());

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'7.XuấtNhập', '7.1', N'Tổng SL nhập kho 30 ngày qua',
    N'📊 INFO',
    N'Nhập 30 ngày: ' + CAST(@nhap30 AS NVARCHAR) + N' đơn vị');

-- 7.2: Số lượng xuất (kê đơn) trong 30 ngày
DECLARE @xuat30 INT;
SELECT @xuat30 = ISNULL(SUM(cdt.SoLuong), 0)
FROM ChiTietDonThuoc cdt
JOIN PhieuKham pk ON cdt.MaPhieuKham = pk.MaPhieuKham
WHERE pk.NgayKham >= DATEADD(DAY, -30, GETDATE())
  AND pk.IsDeleted = 0;

INSERT INTO #KetQuaTest (Phan, MaTest, TenTest, KetQua, ChiTiet)
VALUES (N'7.XuấtNhập', '7.2', N'Tổng SL xuất kho (kê đơn) 30 ngày qua',
    N'📊 INFO',
    N'Xuất 30 ngày: ' + CAST(@xuat30 AS NVARCHAR) + N' đơn vị');

-- ═══════════════════════════════════════════════════════════════════════
-- KẾT QUẢ TỔNG HỢP
-- ═══════════════════════════════════════════════════════════════════════

PRINT '';
PRINT '════════════════════════════════════════════════════════════';
PRINT '  📋 KẾT QUẢ KIỂM TRA';
PRINT '════════════════════════════════════════════════════════════';

SELECT STT, Phan, MaTest, TenTest, KetQua, ChiTiet FROM #KetQuaTest ORDER BY STT;

-- Tổng kết
DECLARE @totalPass INT, @totalFail INT, @totalWarn INT, @totalInfo INT, @total INT;
SELECT @totalPass = COUNT(*) FROM #KetQuaTest WHERE KetQua LIKE N'%PASS%';
SELECT @totalFail = COUNT(*) FROM #KetQuaTest WHERE KetQua LIKE N'%FAIL%';
SELECT @totalWarn = COUNT(*) FROM #KetQuaTest WHERE KetQua LIKE N'%WARN%';
SELECT @totalInfo = COUNT(*) FROM #KetQuaTest WHERE KetQua LIKE N'%INFO%';
SELECT @total = COUNT(*) FROM #KetQuaTest;

PRINT '';
PRINT '════════════════════════════════════════════════════════════';
PRINT '  📊 TỔNG KẾT';
PRINT '════════════════════════════════════════════════════════════';
PRINT '  Tổng test:  ' + CAST(@total AS VARCHAR);
PRINT '  ✅ PASS:    ' + CAST(@totalPass AS VARCHAR);
PRINT '  ❌ FAIL:    ' + CAST(@totalFail AS VARCHAR);
PRINT '  ⚠️ WARN:    ' + CAST(@totalWarn AS VARCHAR);
PRINT '  📊 INFO:    ' + CAST(@totalInfo AS VARCHAR);
PRINT '';

IF @totalFail > 0
BEGIN
    PRINT '  ❌ CÓ LỖI CẦN SỬA — Xem chi tiết ở bảng kết quả ở trên.';
    PRINT '';
    PRINT '  Chi tiết thuốc bị lệch (nếu có):';

    -- Hiển thị chi tiết thuốc lệch cache
    IF EXISTS (
        SELECT 1 FROM Thuoc t
        LEFT JOIN ChiTietNhapKho ctk ON t.MaThuoc = ctk.MaThuoc
        GROUP BY t.MaThuoc, t.SoLuongTon
        HAVING t.SoLuongTon <> ISNULL(SUM(ctk.SoLuongConLai), 0)
    )
    BEGIN
        PRINT '  --- Cache vs Thực tế ---';
        SELECT 
            t.MaThuoc, t.TenThuoc,
            t.SoLuongTon AS [Cache],
            ISNULL(SUM(ctk.SoLuongConLai), 0) AS [ThucTe],
            t.SoLuongTon - ISNULL(SUM(ctk.SoLuongConLai), 0) AS [Lech]
        FROM Thuoc t
        LEFT JOIN ChiTietNhapKho ctk ON t.MaThuoc = ctk.MaThuoc
        GROUP BY t.MaThuoc, t.TenThuoc, t.SoLuongTon
        HAVING t.SoLuongTon <> ISNULL(SUM(ctk.SoLuongConLai), 0)
        ORDER BY ABS(t.SoLuongTon - ISNULL(SUM(ctk.SoLuongConLai), 0)) DESC;
    END

    -- Hiển thị lô âm
    IF EXISTS (SELECT 1 FROM ChiTietNhapKho WHERE SoLuongConLai < 0)
    BEGIN
        PRINT '  --- Lô bị âm ---';
        SELECT ctk.MaPhieuNhap, ctk.MaThuoc, t.TenThuoc, ctk.SoLuongConLai, ctk.HanSuDung
        FROM ChiTietNhapKho ctk
        JOIN Thuoc t ON ctk.MaThuoc = t.MaThuoc
        WHERE ctk.SoLuongConLai < 0;
    END
END
ELSE
BEGIN
    PRINT '  ✅ TẤT CẢ KIỂM TRA ĐỀU PASS — Tồn kho đồng bộ hoàn hảo!';
END

PRINT '';
PRINT '  Hoàn thành: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '════════════════════════════════════════════════════════════';

DROP TABLE #KetQuaTest;
