# 📦 BẢNG TEST TỒN KHO & CẬP NHẬT KHO — DERMASOFT

> **Phiên bản:** 1.0  
> **Ngày tạo:** Tháng 06/2025  
> **Mục đích:** Kiểm tra đồng bộ dữ liệu tồn kho giữa SQL Server ↔ App  
> **Quy ước:** ✅ Pass | ❌ Fail | ⚠️ Cần kiểm tra | 🔄 Chưa test

---

## 📐 SƠ ĐỒ LUỒNG TỒN KHO

```
                    ┌─────────────────────────────────────────────────────┐
                    │               NGUỒN DỮ LIỆU TỒN KHO               │
                    ├─────────────────────────────────────────────────────┤
                    │  Thuoc.SoLuongTon        (Số nhanh — cache)        │
                    │  ChiTietNhapKho.SoLuongConLai  (Số chính xác/lô)  │
                    └────────────┬────────────────────────┬──────────────┘
                                 │                        │
                    ┌────────────▼──────┐    ┌────────────▼──────────┐
                    │   TĂNG TỒN KHO    │    │    GIẢM TỒN KHO      │
                    ├───────────────────┤    ├───────────────────────┤
                    │ NhapKhoForm       │    │ PhieuKhamForm         │
                    │ (INSERT nhập kho) │    │ (Kê đơn / Sửa đơn)   │
                    │                   │    │                       │
                    │ Trigger:          │    │ App code:             │
                    │ TRG_NhapKho_      │    │ UPDATE TOP(1)         │
                    │ CapNhatTon        │    │ ChiTietNhapKho        │
                    │ → Thuoc +=        │    │ (FEFO) → Thuoc -=     │
                    └───────────────────┘    └───────────────────────┘
                                                      │
                    ┌─────────────────────────────────▼──────────────┐
                    │             HOÀN TRẢ TỒN KHO                   │
                    ├────────────────────────────────────────────────┤
                    │ PhieuKhamForm (Xóa thuốc khỏi đơn)            │
                    │ → ChiTietNhapKho += (FEFO lô sớm nhất)        │
                    │ → Thuoc.SoLuongTon +=                          │
                    └────────────────────────────────────────────────┘
```

---

## 🗂️ CÁC BẢNG SQL LIÊN QUAN

| Bảng | Cột quan trọng | Vai trò |
|------|---------------|---------|
| `Thuoc` | `MaThuoc`, `TenThuoc`, `SoLuongTon`, `DonViTinh` | Master thuốc + **cache** tổng tồn |
| `ChiTietNhapKho` | `MaThuoc`, `SoLuong`, `SoLuongConLai`, `HanSuDung`, `MaPhieuNhap` | Tồn kho **chi tiết theo lô** |
| `PhieuNhapKho` | `MaPhieuNhap`, `NgayNhap`, `MaNCC` | Header phiếu nhập |
| `ChiTietDonThuoc` | `MaPhieuKham`, `MaThuoc`, `SoLuong`, `LieuDung` | Đơn thuốc kê cho BN |
| `PhieuKham` | `MaPhieuKham`, `TrangThai`, `IsDeleted` | Phiếu khám (context đơn thuốc) |

---

## 📊 CÔNG THỨC KIỂM TRA ĐỒNG BỘ

### Công thức 1: Tồn kho cache vs thực tế
```sql
-- Phải bằng nhau cho MỌI thuốc
SELECT 
    t.MaThuoc, t.TenThuoc,
    t.SoLuongTon AS TonCache,
    ISNULL(SUM(ctk.SoLuongConLai), 0) AS TonThucTe,
    t.SoLuongTon - ISNULL(SUM(ctk.SoLuongConLai), 0) AS ChenhLech
FROM Thuoc t
LEFT JOIN ChiTietNhapKho ctk ON t.MaThuoc = ctk.MaThuoc
    AND ctk.SoLuongConLai > 0
GROUP BY t.MaThuoc, t.TenThuoc, t.SoLuongTon
HAVING t.SoLuongTon <> ISNULL(SUM(ctk.SoLuongConLai), 0);
-- Kết quả mong đợi: 0 dòng (không có chênh lệch)
```

### Công thức 2: Tồn kho = Nhập − Xuất
```sql
-- Cho mỗi thuốc: SUM(SoLuongConLai) = SUM(SoLuong nhập) - SUM(SoLuong kê)
SELECT 
    t.MaThuoc, t.TenThuoc,
    ISNULL(nhap.TongNhap, 0) AS TongNhap,
    ISNULL(xuat.TongXuat, 0) AS TongXuat,
    ISNULL(ton.TongTon, 0) AS TongTonThucTe,
    ISNULL(nhap.TongNhap, 0) - ISNULL(xuat.TongXuat, 0) AS TonTinhToan,
    ISNULL(ton.TongTon, 0) - (ISNULL(nhap.TongNhap, 0) - ISNULL(xuat.TongXuat, 0)) AS ChenhLech
FROM Thuoc t
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongNhap FROM ChiTietNhapKho GROUP BY MaThuoc) nhap ON t.MaThuoc = nhap.MaThuoc
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongXuat FROM ChiTietDonThuoc GROUP BY MaThuoc) xuat ON t.MaThuoc = xuat.MaThuoc
LEFT JOIN (SELECT MaThuoc, SUM(SoLuongConLai) AS TongTon FROM ChiTietNhapKho GROUP BY MaThuoc) ton ON t.MaThuoc = ton.MaThuoc
WHERE ISNULL(nhap.TongNhap, 0) > 0
  AND ISNULL(ton.TongTon, 0) <> (ISNULL(nhap.TongNhap, 0) - ISNULL(xuat.TongXuat, 0))
ORDER BY ABS(ISNULL(ton.TongTon, 0) - (ISNULL(nhap.TongNhap, 0) - ISNULL(xuat.TongXuat, 0))) DESC;
-- Kết quả mong đợi: 0 dòng
```

---

## 🧪 PHẦN 1: NHẬP KHO (`NhapKhoForm`)

| # | Test Case | Bước thực hiện | Kết quả mong đợi | Kiểm tra SQL | Kết quả |
|---|-----------|---------------|-------------------|-------------|---------|
| 1.1 | Nhập kho thuốc mới (lần đầu) | Tạo phiếu nhập: Thuốc A, SL=100, HSD=12/2026 | INSERT `ChiTietNhapKho` (SoLuong=100, SoLuongConLai=100) | `SELECT SoLuongTon FROM Thuoc WHERE MaThuoc=X` → 100 | 🔄 |
| 1.2 | Nhập thêm lô mới (cùng thuốc) | Tạo phiếu nhập: Thuốc A, SL=50, HSD=06/2026 | Thêm 1 row `ChiTietNhapKho` mới | `SELECT SoLuongTon FROM Thuoc WHERE MaThuoc=X` → 150 | 🔄 |
| 1.3 | Trigger `TRG_NhapKho_CapNhatTon` | Sau INSERT `ChiTietNhapKho` | `Thuoc.SoLuongTon` tự động += SoLuong | So sánh: `SoLuongTon = SUM(SoLuongConLai)` | 🔄 |
| 1.4 | Nhập SL=0 | Nhập thuốc với SL=0 | App phải chặn (validation) hoặc không thay đổi tồn | `SoLuongTon` không đổi | 🔄 |
| 1.5 | Nhập nhiều thuốc 1 phiếu | Tạo phiếu nhập 3 thuốc khác nhau | Mỗi thuốc đều được += đúng SL | Chạy Công thức 1 → 0 dòng | 🔄 |

---

## 🧪 PHẦN 2: KÊ ĐƠN THUỐC — Form chính (`PhieuKhamForm.BtnThemThuoc_Click`)

| # | Test Case | Bước thực hiện | Kết quả mong đợi | Kiểm tra SQL | Kết quả |
|---|-----------|---------------|-------------------|-------------|---------|
| 2.1 | Kê thuốc mới vào đơn | Chọn Thuốc A (tồn=150), SL=10 | INSERT `ChiTietDonThuoc` + `ChiTietNhapKho.SoLuongConLai -= 10` + `Thuoc.SoLuongTon -= 10` | `SoLuongTon` = 140, `SUM(SoLuongConLai)` = 140 | 🔄 |
| 2.2 | Kê thuốc — FEFO đúng lô | Thuốc A có 2 lô: Lô1(HSD 06/2026, ConLai=50), Lô2(HSD 12/2026, ConLai=100). Kê SL=10 | Lô1 giảm: ConLai=40 (lô sớm hết hạn) | `SELECT SoLuongConLai FROM ChiTietNhapKho WHERE MaThuoc=X ORDER BY HanSuDung` → [40, 100] | 🔄 |
| 2.3 | Cập nhật SL thuốc đã kê (tăng) | Thuốc A đã kê SL=10, sửa thành SL=15 | Chênh lệch = +5 → `ChiTietNhapKho -= 5`, `Thuoc -= 5` | `SoLuongTon` = 135 | 🔄 |
| 2.4 | Cập nhật SL thuốc đã kê (giảm) | Thuốc A đã kê SL=15, sửa thành SL=8 | Chênh lệch = −7 → `ChiTietNhapKho += 7`, `Thuoc += 7` | `SoLuongTon` = 142 | 🔄 |
| 2.5 | Kê vượt tồn kho | Thuốc B (tồn=5), kê SL=10 | App hiện "Không đủ tồn kho" + chặn INSERT | `SoLuongTon` không đổi = 5 | 🔄 |
==Không thông báo số lượng vượt tồn và vẫn thêm được vào đơn ==
| 2.6 | Kê khi tồn = 0 | Thuốc C (tồn=0), kê SL=1 | App hiện "Thuốc này hiện không có trong kho" | Không INSERT | 🔄 |
| 2.7 | Kiểm tra tổng tồn (nhiều lô) | Thuốc có 3 lô (10+20+15=45), kê SL=30 | Kiểm tra TỔNG tồn = 45 ≥ 30 → cho phép | `SoLuongTon` = 15 | 🔄 |

---

## 🧪 PHẦN 3: KÊ ĐƠN THUỐC — Dialog chỉnh sửa (`PhieuKhamForm` — Dialog tab ĐƠN THUỐC)

| # | Test Case | Bước thực hiện | Kết quả mong đợi | Kiểm tra SQL | Kết quả |
|---|-----------|---------------|-------------------|-------------|---------|
| 3.1 | Thêm thuốc qua dialog | Double-click phiếu → Tab Đơn thuốc → Thêm Thuốc D, SL=5 | INSERT `ChiTietDonThuoc` + `ChiTietNhapKho -= 5` + `Thuoc -= 5` | Chạy Công thức 1 → 0 dòng | 🔄 |
| 3.2 | Cập nhật SL qua dialog | Đổi SL từ 5 → 8 | Chênh lệch = +3 → trừ thêm 3 | `SoLuongTon` giảm thêm 3 | 🔄 |
== Không có chức năng chỉ sửa dòng thuốc đã được thêm vào đơn chỉ có thể xóa rồi thêm lại ==
| 3.3 | Xóa thuốc qua dialog | Click nút Xóa trên dòng thuốc | DELETE `ChiTietDonThuoc` + `ChiTietNhapKho += 8` + `Thuoc += 8` | Tồn kho hoàn trả đúng SL | 🔄 |
| 3.4 | Kiểm tra tồn kho trước khi thêm (dialog) | Thêm thuốc có tồn < SL yêu cầu | App hiện "Không đủ tồn kho. Hiện còn X đơn vị" | Không INSERT | 🔄 |

---

## 🧪 PHẦN 4: XÓA THUỐC KHỎI ĐƠN — Form chính (`PhieuKhamForm` — Nút Xóa)

| # | Test Case | Bước thực hiện | Kết quả mong đợi | Kiểm tra SQL | Kết quả |
|---|-----------|---------------|-------------------|-------------|---------|
| 4.1 | Xóa thuốc khỏi đơn | Thuốc A kê SL=10, click Xóa | DELETE `ChiTietDonThuoc` + `ChiTietNhapKho += 10` + `Thuoc += 10` | `SoLuongTon` tăng lại 10 | 🔄 |
| 4.2 | Hoàn trả FEFO đúng lô | Xóa thuốc đã kê → hoàn trả về lô sớm hết hạn | `ChiTietNhapKho` lô sớm nhất += SL | Kiểm tra lô nào tăng | 🔄 |
| 4.3 | Xóa rồi kê lại | Xóa Thuốc A → Kê lại Thuốc A SL=5 | Tồn kho: +10 (hoàn) → −5 (kê lại) = net +5 | `SoLuongTon` = ban đầu + 5 | 🔄 |

---

## 🧪 PHẦN 5: THANH TOÁN HÓA ĐƠN (`InvoiceForm`)

| # | Test Case | Bước thực hiện | Kết quả mong đợi | Kiểm tra SQL | Kết quả |
|---|-----------|---------------|-------------------|-------------|---------|
| 5.1 | Thanh toán không ảnh hưởng tồn kho | Thanh toán phiếu có đơn thuốc | `Thuoc.SoLuongTon` KHÔNG thay đổi (đã trừ khi kê) | `SoLuongTon` trước TT = sau TT | 🔄 |
| 5.2 | PhieuKham chuyển TrangThai=3 | Sau thanh toán | `PhieuKham.TrangThai = 3` | `SELECT TrangThai FROM PhieuKham WHERE MaPhieuKham=X` → 3 | 🔄 |
| 5.3 | Tồn kho vẫn đồng bộ sau TT | Chạy Công thức 1 sau thanh toán | 0 dòng chênh lệch | Chạy query Công thức 1 | 🔄 |


---

## 🧪 PHẦN 6: BÁO CÁO KHO (`BaoCaoKhoForm`)

| # | Test Case | Bước thực hiện | Kết quả mong đợi | Kiểm tra SQL | Kết quả |
|---|-----------|---------------|-------------------|-------------|---------|
| 6.1 | Xuất kho chỉ tính phiếu TrangThai ≥ 2 | Có 2 phiếu: 1 Nháp (TT=0), 1 Hoàn thành (TT=2) | Chỉ phiếu TT=2 tính vào xuất | `SoXuat` = SL phiếu TT≥2 | 🔄 |
| 6.2 | Tồn đầu kỳ tính đúng | Tồn đầu kỳ = Tồn cuối kỳ − Nhập kỳ + Xuất kỳ | Công thức khớp | So sánh manual | 🔄 |
| 6.3 | Không tính phiếu đã xóa | Phiếu khám có `IsDeleted=1` | Không tính vào xuất kho | `SoXuat` không bao gồm phiếu đã xóa | 🔄 |

---

## 🧪 PHẦN 7: TỒN KHO FORM (`TonKhoForm`)

| # | Test Case | Bước thực hiện | Kết quả mong đợi | Kiểm tra SQL | Kết quả |
|---|-----------|---------------|-------------------|-------------|---------|
| 7.1 | Hiển thị tồn theo lô FEFO | Mở TonKhoForm | Thuốc sắp hết hạn hiện đầu (ưu tiên FEFO) | `ORDER BY HanSuDung ASC` | 🔄 |
| 7.2 | KPI hết hạn đúng | Có 2 lô hết hạn (HSD < today) | Card "Lô hết hạn" = 2 | `SELECT COUNT(*) ... WHERE HanSuDung < GETDATE()` | 🔄 |
| 7.3 | KPI sắp hết hạn | Có 3 lô HSD trong 30 ngày tới | Card "Sắp hết hạn" = 3 | `DATEDIFF(DAY, GETDATE(), HanSuDung) < 30` | 🔄 |
| 7.4 | Chỉ hiện lô còn tồn | Lô có SoLuongConLai=0 | Không hiện trong danh sách | `WHERE SoLuongConLai > 0` | 🔄 |
| 7.5 | Số liệu khớp với SQL | So sánh từng dòng trên form vs SQL | Tất cả khớp | Chạy query view vs UI | 🔄 |

---

## 🧪 PHẦN 8: TÍCH HỢP END-TO-END

| # | Test Case | Bước thực hiện | Kết quả mong đợi | Kiểm tra SQL | Kết quả |
|---|-----------|---------------|-------------------|-------------|---------|
| 8.1 | **Full flow: Nhập → Kê → TT** | 1) Nhập Thuốc X SL=100 → 2) Kê đơn SL=10 → 3) Thanh toán | Tồn kho cuối = 90 | `SoLuongTon`=90, `SUM(SoLuongConLai)`=90 | 🔄 |
| 8.2 | **Full flow: Nhập → Kê → Xóa → Kê lại** | 1) Nhập SL=50 → 2) Kê SL=20 → 3) Xóa thuốc → 4) Kê lại SL=15 | Tồn = 50−20+20−15 = 35 | Cả 2 bảng = 35 | 🔄 |
| 8.3 | **Multi-lô FEFO** | 1) Nhập Lô1(SL=10, HSD 03/2026) → 2) Nhập Lô2(SL=20, HSD 12/2026) → 3) Kê SL=15 | Lô1: ConLai=0 (hết), Lô2: ConLai=15 | Xem từng lô | 🔄 |
| 8.4 | **Đồng bộ sau nhiều thao tác** | Thực hiện 10+ thao tác nhập/kê/xóa ngẫu nhiên | Chạy Công thức 1 & 2 → 0 dòng | 2 query kiểm tra | 🔄 |
| 8.5 | **Concurrent: 2 BS kê cùng thuốc** | BS1 kê Thuốc A SL=5, BS2 kê Thuốc A SL=3 (cùng lúc) | Tồn giảm tổng 8, không âm, không conflict | `SoLuongTon` = ban đầu − 8 | 🔄 |

---

## 🧪 PHẦN 9: EDGE CASES & NEGATIVE TESTING

| # | Test Case | Bước thực hiện | Kết quả mong đợi | Kết quả |
|---|-----------|---------------|-------------------|---------|
| 9.1 | Kê SL = tồn kho chính xác | Tồn=10, kê SL=10 | Cho phép, tồn = 0 | 🔄 |
| 9.2 | Kê SL = tồn + 1 | Tồn=10, kê SL=11 | Từ chối "Không đủ tồn kho" | 🔄 |
| 9.3 | Thuốc hết hạn toàn bộ | Tất cả lô HSD < today | Báo "không có trong kho" (view lọc HSD > GETDATE) | 🔄 |
| 9.4 | SoLuongConLai bị âm | Thao tác lỗi → ConLai < 0 | Không được xảy ra. Nếu có → app phải chặn | 🔄 |
| 9.5 | Xóa thuốc khi phiếu đã thanh toán | Phiếu TrangThai=3, thử xóa thuốc | App phải chặn (readonly) | 🔄 |

---

## 🔧 SCRIPT KIỂM TRA NHANH (chạy trên SQL Server)

```sql
-- ═══════════════════════════════════════════════════
-- SCRIPT 1: Kiểm tra chênh lệch Thuoc.SoLuongTon vs ChiTietNhapKho
-- ═══════════════════════════════════════════════════
PRINT '=== KIỂM TRA CHÊNH LỆCH TỒN KHO ===';
SELECT 
    t.MaThuoc, t.TenThuoc,
    t.SoLuongTon AS [Cache (Thuoc)],
    ISNULL(SUM(ctk.SoLuongConLai), 0) AS [ThucTe (ChiTietNhapKho)],
    t.SoLuongTon - ISNULL(SUM(ctk.SoLuongConLai), 0) AS [Chenh Lech]
FROM Thuoc t
LEFT JOIN ChiTietNhapKho ctk ON t.MaThuoc = ctk.MaThuoc
GROUP BY t.MaThuoc, t.TenThuoc, t.SoLuongTon
HAVING t.SoLuongTon <> ISNULL(SUM(ctk.SoLuongConLai), 0)
ORDER BY ABS(t.SoLuongTon - ISNULL(SUM(ctk.SoLuongConLai), 0)) DESC;

-- ═══════════════════════════════════════════════════
-- SCRIPT 2: Kiểm tra SoLuongConLai bị âm
-- ═══════════════════════════════════════════════════
PRINT '=== KIỂM TRA LÔ BỊ ÂM ===';
SELECT ctk.*, t.TenThuoc
FROM ChiTietNhapKho ctk
JOIN Thuoc t ON ctk.MaThuoc = t.MaThuoc
WHERE ctk.SoLuongConLai < 0;

-- ═══════════════════════════════════════════════════
-- SCRIPT 3: Kiểm tra Nhập - Xuất = Tồn
-- ═══════════════════════════════════════════════════
PRINT '=== KIỂM TRA NHẬP - XUẤT = TỒN ===';
SELECT 
    t.MaThuoc, t.TenThuoc,
    ISNULL(n.TongNhap, 0) AS TongNhap,
    ISNULL(x.TongXuat, 0) AS TongXuat,
    ISNULL(r.TongTon, 0) AS TongTonLo,
    t.SoLuongTon AS TonCache,
    ISNULL(n.TongNhap, 0) - ISNULL(x.TongXuat, 0) AS [Nhap-Xuat],
    ISNULL(r.TongTon, 0) - (ISNULL(n.TongNhap, 0) - ISNULL(x.TongXuat, 0)) AS [ChenhLech]
FROM Thuoc t
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongNhap FROM ChiTietNhapKho GROUP BY MaThuoc) n ON t.MaThuoc = n.MaThuoc
LEFT JOIN (SELECT MaThuoc, SUM(SoLuong) AS TongXuat FROM ChiTietDonThuoc GROUP BY MaThuoc) x ON t.MaThuoc = x.MaThuoc
LEFT JOIN (SELECT MaThuoc, SUM(SoLuongConLai) AS TongTon FROM ChiTietNhapKho GROUP BY MaThuoc) r ON t.MaThuoc = r.MaThuoc
WHERE ISNULL(n.TongNhap, 0) > 0
ORDER BY t.TenThuoc;

-- ═══════════════════════════════════════════════════
-- SCRIPT 4: Sửa chữa tự động (nếu phát hiện lệch)
-- ═══════════════════════════════════════════════════
PRINT '=== SỬA CHỮA: Đồng bộ Thuoc.SoLuongTon từ ChiTietNhapKho ===';
-- UNCOMMENT để chạy:
-- UPDATE t
-- SET t.SoLuongTon = ISNULL(ctk.TonThucTe, 0)
-- FROM Thuoc t
-- LEFT JOIN (
--     SELECT MaThuoc, SUM(SoLuongConLai) AS TonThucTe
--     FROM ChiTietNhapKho
--     GROUP BY MaThuoc
-- ) ctk ON t.MaThuoc = ctk.MaThuoc
-- WHERE t.SoLuongTon <> ISNULL(ctk.TonThucTe, 0);
-- PRINT 'Đã đồng bộ ' + CAST(@@ROWCOUNT AS VARCHAR) + ' thuốc.';
```

---

## 📝 GHI CHÚ QUAN TRỌNG

### Thiết kế hiện tại: "Trừ kho khi kê đơn" (Reserve Stock)
- Tồn kho bị trừ **ngay khi bác sĩ kê đơn**, KHÔNG đợi thanh toán
- Phù hợp cho phòng khám: thuốc phát ngay cho bệnh nhân khi khám
- Nếu hủy/xóa thuốc khỏi đơn → hoàn trả tồn kho

### 2 nguồn dữ liệu tồn kho:
| Nguồn | Bảng | Vai trò | Ưu tiên |
|-------|------|---------|---------|
| **Cache** | `Thuoc.SoLuongTon` | Hiển thị nhanh, KPI | Phụ — có thể lệch |
| **Chính xác** | `ChiTietNhapKho.SoLuongConLai` | Chi tiết theo lô, FEFO | **Chủ — nguồn sự thật** |

### FEFO (First Expired, First Out):
- Khi kê đơn → trừ từ lô **sớm hết hạn nhất** (`ORDER BY HanSuDung ASC`)
- Khi hoàn trả → cộng về lô **sớm hết hạn nhất**
- View `VW_TonKhoTheoLo` cung cấp `UuTienFEFO` (ROW_NUMBER)
