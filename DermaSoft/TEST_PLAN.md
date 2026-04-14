# 📋 KẾ HOẠCH TEST HỆ THỐNG DERMASOFT

> **Phiên bản:** 2.0 (Cập nhật sau sửa lỗi)  
> **Phạm vi:** WinApp (.NET Framework 4.8) ↔ SQL Server `DERMASOFT` ↔ Website (PHP)  
> **Quy ước:** ✅ = Đã hoạt động | ⚠️ = Cần kiểm tra thêm | ❌ = Chưa hoạt động / Lỗi

---

## 🏗️ KIẾN TRÚC HỆ THỐNG

```
┌──────────────────┐                              ┌──────────────────┐
│   WINDOWS APP    │                              │     WEBSITE      │
│  (C# .NET 4.8)   │                              │     (PHP)        │
│                  │       ┌──────────────┐       │                  │
│  MainForm(Admin) │──────►│              │◄──────│  Trang đặt lịch  │
│  MainFormBacSi   │  R/W  │  SQL Server  │  R/W  │  Hồ sơ BN       │
│  MainFormLeTan   │◄──────│  DERMASOFT   │──────►│  Xem kết quả    │
│                  │       └──────────────┘       │                  │
└──────────────────┘                              └──────────────────┘
```

### Luồng đồng bộ chính

| Hướng | Dữ liệu | Cơ chế |
|-------|---------|--------|
| **Website → WinApp** | BN đặt lịch hẹn | Website gọi `SP_DatLichHen` → INSERT `LichHen` (TrangThai=0) + `BenhNhan` → WinApp polling 15s |
| **WinApp → Website** | BS khám + LT thanh toán | WinApp INSERT/UPDATE trực tiếp → Website SELECT hiển thị |

---

## 📂 CẤU TRÚC SQL (6 file)

| # | File | Mục đích | Chạy lại trên DB có dữ liệu? |
|---|------|---------|-------------------------------|
| 1 | `01_Tables_Data.sql` | CREATE TABLE + Seed data | ❌ Chỉ fresh deploy |
| 2 | `02_StoredProcedures.sql` | Stored Procedures | ✅ DROP IF EXISTS + CREATE |
| 3 | `03_Triggers.sql` | Triggers | ✅ DROP IF EXISTS + CREATE |
| 4 | `04_Views.sql` | Views | ✅ DROP IF EXISTS + CREATE |
| 5 | `05_Constraints.sql` | Constraints + Indexes | ✅ IF NOT EXISTS |
| 6 | `05_SeedTestData.sql` | Dữ liệu test (tùy chọn) | ✅ IF NOT EXISTS |
| — | `MIGRATION_APPLY_FIXES.sql` | Cập nhật DB hiện tại | ✅ Idempotent |

---

## 📊 BẢNG TRẠNG THÁI THAM CHIẾU

### LichHen.TrangThai (CHECK 0–3)

| Giá trị | Ý nghĩa | Ai tạo/cập nhật |
|---------|---------|-----------------|
| 0 | Chờ xác nhận | Website (`SP_DatLichHen`) / WinApp (`AppointmentForm`) |
| 1 | Đã xác nhận | WinApp — Lễ Tân xác nhận |
| 2 | Hoàn thành (Đã tiếp nhận) | WinApp — Lễ Tân tiếp nhận tạo phiếu khám |
| 3 | Đã hủy | Website / WinApp |

### PhieuKham.TrangThai (CHECK 0–4)

| Giá trị | Ý nghĩa | Ai tạo/cập nhật |
|---------|---------|-----------------|
| 0 | Chờ khám (Mới) | WinApp — Lễ Tân tiếp nhận |
| 1 | Đang khám | WinApp — Bác Sĩ bắt đầu khám |
| 2 | Hoàn thành (chưa TT) | WinApp — Bác Sĩ hoàn thành |
| 3 | Đã thanh toán | WinApp — Lễ Tân thanh toán |
| 4 | Đã hủy | WinApp |

### HoaDon.TrangThai (BIT)

| Giá trị | Ý nghĩa | Ai tạo/cập nhật |
|---------|---------|-----------------|
| 0 | Chưa thanh toán | WinApp — BS hoàn thành phiếu khám → auto INSERT |
| 1 | Đã thanh toán | WinApp — Lễ Tân thanh toán |

### PhanCongCa.TrangThaiDiemDanh (TINYINT)

| Giá trị | Ý nghĩa | Cơ chế |
|---------|---------|--------|
| 1 | Chưa điểm danh | Mặc định khi Admin phân công |
| 2 | Đã điểm danh | Auto khi nhân viên đăng nhập (`LoginForm.AutoDiemDanh`) |

### VaiTro (bảng VaiTro)

| MaVaiTro | TenVaiTro | MainForm tương ứng |
|----------|-----------|-------------------|
| 1 | Admin | `MainForm` |
| 2 | Bác Sĩ | `MainFormBacSi` |
| 3 | Lễ Tân | `MainFormLeTan` |

---

## 🔐 MODULE 1: ĐĂNG NHẬP & PHÂN QUYỀN

### Form: `LoginForm`

| # | Test Case | Bước thực hiện | Kết quả mong đợi | Bảng DB |
|---|-----------|---------------|-------------------|---------|
| 1.1 | Đăng nhập Admin | Nhập TenDangNhap + MatKhau admin (aB1cD) | Mở `MainForm` — menu đầy đủ 14 mục | `NguoiDung`, `VaiTro` |
| 1.2 | Đăng nhập Bác Sĩ | Nhập tài khoản MaVaiTro=2 | Mở `MainFormBacSi` — menu: Dashboard, Hồ Sơ BN, Phiếu Khám, Hồ Sơ Cá Nhân | `NguoiDung` |
| 1.3 | Đăng nhập Lễ Tân | Nhập tài khoản MaVaiTro=3 | Mở `MainFormLeTan` — menu: Dashboard, Lịch Hẹn, BN, Tiếp Nhận, Hóa Đơn, Thẻ TV, Hồ Sơ CN | `NguoiDung` |
| 1.4 | Sai mật khẩu | Nhập đúng username, sai password | pnlError hiện "Tên đăng nhập hoặc mật khẩu không đúng!" + animation rung | — |
| 1.5 | Tài khoản bị khóa | TrangThaiTK = 0 | Hiện "Tài khoản đã bị khóa" | `NguoiDung.TrangThaiTK` |
| 1.6 | Đổi MK lần đầu | DoiMatKhau = 1 | Mở `ChangePasswordForm` → phải đổi trước khi vào | `NguoiDung.DoiMatKhau` |
| 1.7 | Nhập trống username | Bỏ trống txtTenDangNhap → Enter | "Vui lòng nhập tên đăng nhập!", focus txtTenDangNhap | — |
| 1.8 | Nhập trống password | Nhập username, bỏ trống txtMatKhau → Enter | "Vui lòng nhập mật khẩu!", focus txtMatKhau | — |
| 1.9 | BCrypt $2y$ → $2a$ | Tạo TK từ Website PHP → đăng nhập WinApp | Chuyển $2y$ → $2a$ verify thành công | `NguoiDung.MatKhau` |
| 1.10 | Kết nối DB thất bại | Tắt SQL Server → mở app | `KiemTraKetNoi()` → disable nút Đăng Nhập, hiện cảnh báo | — |
| 1.11 | Auto điểm danh | Đăng nhập khi có PhanCongCa hôm nay | `TrangThaiDiemDanh` = 1 → UPDATE thành 2 | `PhanCongCa` |
| 1.12 | Đăng xuất | Click "Đăng Xuất" trên MainForm | `DangXuat = true` → quay về LoginForm, clear inputs | — |
| 1.13 | Đóng ứng dụng | Click ✕ trên MainForm | `DangXuat = false` → đóng toàn bộ app | — |
| 1.14 | Hiện/ẩn mật khẩu | Click btnShowPassword | Toggle `txtMatKhau.UseSystemPasswordChar` | — |
| 1.15 | Enter để đăng nhập | Nhập xong → nhấn Enter | `TxtInput_KeyDown` → gọi `BtnDangNhap_Click` | — |

---

## 👑 MODULE 2: ADMIN — MainForm

### Menu Sidebar Admin (14 mục)

```
📊 Dashboard
👥 Nhân Viên        📅 Phân Công Ca
💊 Danh Mục Thuốc   📦 Nhập Kho          🏪 Tồn Kho
✨ Dịch Vụ          📈 Báo Cáo Doanh Thu  📋 Báo Cáo Kho    🧾 Quản Lý Hóa Đơn
⭐ Đánh Giá         ⚙️ Cài Đặt            👤 Hồ Sơ Cá Nhân
```

### 2A. Dashboard Admin (`DashboardForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 2A.1 | KPI cards (4 thẻ) | Hiển thị số liệu: BN, Phiếu Khám, Doanh Thu, Lịch Hẹn | `BenhNhan`, `PhieuKham`, `HoaDon`, `LichHen` |
| 2A.2 | Biểu đồ doanh thu | OxyPlot render doanh thu 7 ngày gần nhất | `HoaDon` |

### 2B. Nhân Viên (`StaffForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 2B.1 | Xem danh sách NV | DataGridView hiện NguoiDung (IsDeleted=0) | `NguoiDung` |
| 2B.2 | Thêm nhân viên | INSERT + MatKhau = BCrypt hash | `NguoiDung` |
| 2B.3 | Sửa thông tin NV | UPDATE HoTen/SĐT/Email/VaiTro/TenDangNhap/MatKhau | `NguoiDung` |
| 2B.4 | Xóa nhân viên (soft) | SET IsDeleted=1, TrangThaiTK=0 — chặn xóa chính mình | `NguoiDung` |
| 2B.5 | Khóa/mở tài khoản | Toggle TrangThaiTK → NV bị khóa không đăng nhập được | `NguoiDung.TrangThaiTK` |
| 2B.6 | Reset mật khẩu | SET DoiMatKhau=1 → NV phải đổi MK lần đăng nhập sau | `NguoiDung.DoiMatKhau` |
| 2B.7 | Tìm kiếm NV | Filter theo tên/SĐT/email | `NguoiDung` |

### 2C. Phân Công Ca (`PhanCongCaForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 2C.1 | Xem lịch phân công | DataGridView hiện ca theo tuần/ngày + filter NV | `PhanCongCa`, `CaLamViec` |
| 2C.2 | Phân công ca mới | Chọn NV + Ca + Ngày → INSERT | `PhanCongCa` |
| 2C.3 | Xóa phân công | Chọn → Xóa → DELETE | `PhanCongCa` |
| 2C.4 | Cột điểm danh | Hiển thị trạng thái: 1=⏳ Chưa ĐD, 2=✅ Đã ĐD | `PhanCongCa.TrangThaiDiemDanh` |
| 2C.5 | Auto điểm danh | NV đăng nhập → TrangThaiDiemDanh 1→2 cho ca hôm nay | `PhanCongCa` |

### 2D. Danh Mục Thuốc (`ThuocForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 2D.1 | Xem danh sách thuốc | DataGridView + tồn kho, filter IsDeleted=0 | `Thuoc` |
| 2D.2 | Thêm thuốc mới | INSERT Thuoc | `Thuoc` |
| 2D.3 | Sửa thông tin thuốc | UPDATE Thuoc | `Thuoc` |
| 2D.4 | Xóa thuốc (soft) | SET IsDeleted=1 | `Thuoc.IsDeleted` |
| 2D.5 | Tìm kiếm thuốc | Filter theo tên/mã thuốc | `Thuoc` |

### 2E. Nhập Kho (`NhapKhoForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 2E.1 | Tạo phiếu nhập | Chọn NCC + thuốc + SL + hạn SD → INSERT | `PhieuNhapKho`, `ChiTietNhapKho` |
| 2E.2 | Trigger cập nhật tồn | `TRG_NhapKho_CapNhatTon` → SoLuongTon tăng | `Thuoc.SoLuongTon` |
| 2E.3 | Xem lịch sử nhập | Nút "📜 Lịch sử nhập" → popup bảng phiếu nhập theo ngày | `PhieuNhapKho` |

### 2F. Tồn Kho (`TonKhoForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 2F.1 | Xem tồn kho theo lô | View `VW_TonKhoTheoLo` — SoLuongConLai, HanSuDung, FEFO | `VW_TonKhoTheoLo` |
| 2F.2 | KPI cảnh báo hết hạn | Đếm thuốc HanSuDung < 30/90 ngày → highlight đỏ/vàng | `ChiTietNhapKho.HanSuDung` |
| 2F.3 | KPI cảnh báo tồn thấp | Đếm SoLuongConLai ≤ `AppSettings.NguongThap` | `CaiDatHeThong` → `AppSettings` |
| 2F.4 | Filter / tìm kiếm | Filter theo tên thuốc, trạng thái hạn | `VW_TonKhoTheoLo` |

### 2G. Dịch Vụ (`ServiceForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 2G.1 | CRUD dịch vụ | Thêm/Sửa/Xóa DichVu | `DichVu` |
| 2G.2 | Tìm kiếm dịch vụ | Filter theo tên | `DichVu` |

### 2H. Báo Cáo Doanh Thu (`BaoCaoDoanhThuForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 2H.1 | Xem báo cáo theo khoảng ngày | Biểu đồ + bảng doanh thu | `HoaDon`, `VW_BaoCaoDoanhThu` |
| 2H.2 | Tổng hợp đúng | TongTien = TongTienDichVu + TongThuoc − GiamGia | `HoaDon` |

### 2I. Báo Cáo Kho (`BaoCaoKhoForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 2I.1 | Xem thống kê nhập/xuất | Chọn khoảng ngày → hiện báo cáo | `PhieuNhapKho`, `ChiTietDonThuoc` |

### 2J. Quản Lý Hóa Đơn (`QuanLyHoaDonForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 2J.1 | 4 KPI cards | Đã TT, Chưa TT, Hôm nay, Tổng tháng | `HoaDon` |
| 2J.2 | Lọc theo ngày + trạng thái | Filter danh sách hóa đơn | `HoaDon` |
| 2J.3 | Xem chi tiết | Click hóa đơn → hiện thông tin đầy đủ | `HoaDon`, `PhieuKham`, `BenhNhan` |
| 2J.4 | Chỉ sửa/xóa chưa TT | TrangThai=0 → cho sửa/xóa. TrangThai=1 → chỉ xem | `HoaDon.TrangThai` |

### 2K. Đánh Giá (`DanhGiaForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 2K.1 | Xem đánh giá | Load danh sách đánh giá từ BN | `DanhGia` |

### 2L. Cài Đặt (`SettingsForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 2L.1 | Sửa thông tin phòng khám | UPDATE TenPhongKham/DiaChi/SĐT → InvoiceForm đọc đúng | `ThongTinPhongKham` |
| 2L.2 | Sửa giờ làm việc | UPDATE GioMoCua/GioDongCua → AppointmentForm đọc đúng | `ThongTinPhongKham` |
| 2L.3 | Ngưỡng cảnh báo kho | Ghi vào `CaiDatHeThong` → `AppSettings` reload → TonKhoForm dùng | `CaiDatHeThong` |
| 2L.4 | Test kết nối DB | Click test → `DatabaseConnection.TestConnection()` | — |

### 2M. Hồ Sơ Cá Nhân (`ProfileForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 2M.1 | Xem thông tin | Hiện HoTen, SĐT, Email, VaiTro từ session | `NguoiDung` |
| 2M.2 | Đổi mật khẩu | Nhập MK cũ + MK mới → UPDATE MatKhau (BCrypt hash) | `NguoiDung.MatKhau` |

---

## 🩺 MODULE 3: BÁC SĨ — MainFormBacSi

### Menu Sidebar Bác Sĩ (4 mục)

```
📊 Dashboard
👥 Hồ Sơ Bệnh Nhân   📋 Phiếu Khám Bệnh
👤 Hồ Sơ Cá Nhân
```

### 3A. Dashboard Bác Sĩ (`DashboardBacSiForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 3A.1 | KPI cards | BN chờ khám (TT=0), Đã khám hôm nay (TT≥2), Lịch hẹn | `PhieuKham`, `LichHen` |
| 3A.2 | Bảng BN chờ khám | PhieuKham hôm nay TrangThai=0, IsDeleted=0 | `PhieuKham` |
| 3A.3 | Nút "Bắt đầu khám" | Click → điều hướng sang PhieuKhamForm + sidebar highlight | `PhieuKham` |
| 3A.4 | Nút "Xem hồ sơ" | Click → điều hướng sang BenhNhanDetailForm + sidebar highlight | — |

### 3B. Hồ Sơ Bệnh Nhân (`BenhNhanDetailForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 3B.1 | Tìm BN theo SĐT/tên | DataGridView hiện kết quả | `BenhNhan` |
| 3B.2 | Xem chi tiết BN | Click → hiện thông tin + lịch sử khám + hình ảnh | `BenhNhan`, `PhieuKham`, `HinhAnhBenhLy` |
| 3B.3 | KPI tổng lần khám | Chỉ đếm PhieuKham TrangThai ≥ 2 (Hoàn thành trở lên) | `PhieuKham` |

### 3C. Phiếu Khám (`PhieuKhamForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 3C.1 | Load thông tin BN + phiếu | Hiện TenBN, TrieuChung, ChanDoan | `PhieuKham`, `BenhNhan` |
| 3C.2 | Chọn dịch vụ | Tick checkbox → INSERT/UPDATE ChiTietDichVu | `ChiTietDichVu` |
| 3C.3 | Kê đơn thuốc | Chọn thuốc + liều dùng → INSERT ChiTietDonThuoc | `ChiTietDonThuoc` |
| 3C.4 | ComboBox thuốc | Load toàn bộ thuốc IsDeleted=0, SoLuongTon > 0 | `Thuoc` |
| 3C.5 | Lưu chẩn đoán | Nhập ChanDoan + HuongDieuTri → UPDATE PhieuKham | `PhieuKham` |
| 3C.6 | Hoàn thành khám | Click → SET TrangThai=2 + INSERT HoaDon (TrangThai=0) | `PhieuKham`, `HoaDon` |
| 3C.7 | Trừ tồn kho | Kê đơn thuốc → SoLuongTon giảm đúng | `Thuoc.SoLuongTon` |
| 3C.8 | Header phòng khám | Hiện tên/địa chỉ/SĐT từ DB | `ThongTinPhongKham` |

---

## 📋 MODULE 4: LỄ TÂN — MainFormLeTan

### Menu Sidebar Lễ Tân (7 mục)

```
📊 Dashboard
📅 Quản Lý Lịch Hẹn   👥 Quản Lý Bệnh Nhân   🏥 Tiếp Nhận Bệnh Nhân
💳 Thanh Toán Hóa Đơn  🎖️ Thẻ Thành Viên
👤 Hồ Sơ Cá Nhân
```

### 4A. Dashboard Lễ Tân (`DashboardLeTanForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 4A.1 | 4 KPI cards | Lịch hẹn hôm nay (TT IN 0,1), BN đang chờ (PK TT=0), Hóa đơn cần thu (HD TT=0), Đã tiếp nhận | `LichHen`, `PhieuKham`, `HoaDon` |
| 4A.2 | Bảng lịch hẹn | LichHen hôm nay TrangThai IN (0,1) + badge Chờ XN / Đã XN | `LichHen` |
| 4A.3 | Xác nhận lịch hẹn | Chọn lịch Chờ XN → click XN → UPDATE TrangThai=1 | `LichHen.TrangThai` |
| 4A.4 | Auto refresh 15s | Timer 15s → `LoadTatCa()` → KPI + bảng tự cập nhật | — |
| 4A.5 | Lịch hẹn từ Website | BN đặt lịch qua Website → sau 15s hiện "Chờ XN" trên dashboard | `LichHen` (từ `SP_DatLichHen`) |
| 4A.6 | Panel hóa đơn cần thu | TOP 10 HoaDon TrangThai=0 + TenBN + TongTien | `HoaDon`, `PhieuKham`, `BenhNhan` |
| 4A.7 | Nút điều hướng | Tiếp Nhận / Tạo Lịch → chuyển form + sidebar highlight đúng | — |

### 4B. Quản Lý Lịch Hẹn (`AppointmentForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 4B.1 | Xem theo ngày | Chọn ngày trên MonthCalendar → hiện LichHen ngày đó | `LichHen` |
| 4B.2 | Tạo lịch hẹn nhanh | Nhập SĐT (10 số, bắt đầu 0) + giờ + BS → INSERT TrangThai=0 | `LichHen`, `BenhNhan` |
| 4B.3 | Giờ hẹn từ DB | Đọc GioMoCua/GioDongCua từ ThongTinPhongKham → tạo khung giờ | `ThongTinPhongKham` |
| 4B.4 | BS có ca làm | ComboBox BS hiện nhân viên + ca làm ngày đó (ưu tiên có ca) | `NguoiDung`, `PhanCongCa`, `CaLamViec` |
| 4B.5 | XN / Hủy lịch | Click cột Thao Tác → popup xác nhận → UPDATE TrangThai | `LichHen` |
| 4B.6 | Sửa lịch hẹn | TrangThai=1 → click "Sửa" → dialog đổi BS/giờ/ghi chú | `LichHen` |
| 4B.7 | Filter trạng thái | ComboBox filter: Tất cả / Chờ XN / Đã XN / Đã hủy | `LichHen` |
| 4B.8 | Tìm kiếm | Nhập tên/SĐT → filter bảng | `BenhNhan`, `LichHen` |
| 4B.9 | Xem chi tiết | Click bất kỳ cột (không phải Thao tác) → popup chi tiết | `LichHen`, `BenhNhan`, `NguoiDung` |

### 4C. Quản Lý Bệnh Nhân (`PatientForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 4C.1 | Xem danh sách BN | DataGridView BenhNhan IsDeleted=0 | `BenhNhan` |
| 4C.2 | Tìm kiếm BN | Filter theo SĐT/tên | `BenhNhan` |
| 4C.3 | Thêm BN mới | Điền form → INSERT | `BenhNhan` |
| 4C.4 | Sửa thông tin BN | Chọn → sửa → UPDATE | `BenhNhan` |
| 4C.5 | Xem thẻ thành viên | Chọn BN có thẻ TV → hiện Hạng, Điểm, Số lần khám | `ThanhVienInfo`, `HangThanhVien` |

### 4D. Tiếp Nhận Bệnh Nhân (`TiepNhanForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 4D.1 | Tìm BN theo SĐT | Nhập SĐT → hiện card BN: tên, SĐT, ngày sinh, tiền sử, hạng TV | `BenhNhan`, `ThanhVienInfo` |
| 4D.2 | Load BS hoạt động | cmbBacSi hiện NguoiDung MaVaiTro=2, TrangThaiTK=1, IsDeleted=0 | `NguoiDung` |
| 4D.3 | Load lịch hẹn BN | cmbLichHen hiện lịch hẹn hôm nay của BN | `LichHen` |
| 4D.4 | Tiếp nhận thành công | INSERT PhieuKham (TT=0) + UPDATE LichHen (TT=2) trong 1 transaction | `PhieuKham`, `LichHen` |
| 4D.5 | Queue bệnh nhân | DataGridView: PhieuKham hôm nay TrangThai IN (0,1), IsDeleted=0 | `PhieuKham` |
| 4D.6 | BN Mới | Click "BN Mới" → PatientForm dialog → tạo BN → đóng | `BenhNhan` |
| 4D.7 | Validate | Chưa chọn BN → click Tiếp Nhận → cảnh báo | — |
| 4D.8 | Hủy | Click Hủy → `ResetForm()` | — |

### 4E. Thanh Toán Hóa Đơn (`InvoiceForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 4E.1 | Load phiếu chưa TT | cmbPhieuKham hiện PhieuKham TrangThai=2 chưa có HoaDon TrangThai=1 | `PhieuKham`, `HoaDon` |
| 4E.2 | Tải chi tiết phiếu | Chọn → Tải → hiện BN, BS, ngày, dịch vụ, thuốc, tổng tiền | `PhieuKham`, `ChiTietDichVu`, `ChiTietDonThuoc` |
| 4E.3 | Áp dụng giảm giá | Nhập % hoặc số tiền → tổng cập nhật | — |
| 4E.4 | Giảm giá thành viên | BN có thẻ TV → auto tính PhanTramGiamDuocPham/PhanTramGiamTongHD | `ThanhVienInfo`, `HangThanhVien` |
| 4E.5 | Xác nhận thanh toán | INSERT/UPDATE HoaDon (TT=1) + UPDATE PhieuKham (TT=3) | `HoaDon`, `PhieuKham` |
| 4E.6 | Trigger cộng điểm | `TRG_HoaDon_CapPhatDiem` → cộng TongTien/1000 vào DiemTichLuy | `ThanhVienInfo.DiemTichLuy` |
| 4E.7 | In hóa đơn | Click In → `HoaDonPrinter` → PrintPreviewDialog | — |
| 4E.8 | Thông tin phòng khám | Header load từ ThongTinPhongKham, không hardcode | `ThongTinPhongKham` |
| 4E.9 | Lọc theo ngày | DateTimePicker → filter phiếu khám theo ngày | — |

### 4F. Thẻ Thành Viên (`MemberForm`)

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 4F.1 | Xem danh sách TV | BN + Hạng TV + Điểm + Số lần khám | `BenhNhan`, `ThanhVienInfo`, `HangThanhVien` |
| 4F.2 | Đăng ký thẻ mới | Chọn BN chưa có thẻ → INSERT ThanhVienInfo | `ThanhVienInfo` |
| 4F.3 | Hủy thẻ | Chọn BN có thẻ → DELETE ThanhVienInfo | `ThanhVienInfo` |
| 4F.4 | Filter theo hạng | Chọn hạng → filter danh sách | `HangThanhVien` |
| 4F.5 | Tier card | Panel phải hiện đúng hạng + màu (MauHangHex) + icon | `HangThanhVien.MauHangHex` |

---

## 🌐 MODULE 5: TÍCH HỢP WEBSITE ↔ WINAPP

### 5A. Website → WinApp

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 5A.1 | BN mới đặt lịch | `SP_DatLichHen` → INSERT BenhNhan (nếu mới) + INSERT LichHen TrangThai=0 | `BenhNhan`, `LichHen` |
| 5A.2 | BN cũ đặt lại | SĐT tồn tại → chỉ INSERT LichHen, không tạo BN mới | `LichHen` |
| 5A.3 | WinApp nhận lịch mới | DashboardLeTanForm polling 15s → lịch mới hiện "Chờ XN" | `LichHen` |
| 5A.4 | Trùng lịch cùng ngày | BN đã có lịch TrangThai IN (0,1) cùng ngày → RAISERROR | `LichHen` |

### 5B. WinApp → Website

| # | Test Case | Kết quả mong đợi | Bảng DB |
|---|-----------|-------------------|---------|
| 5B.1 | BS hoàn thành phiếu khám | PhieuKham TT=2 → Website SELECT hiện kết quả | `PhieuKham`, `ChiTietDichVu`, `ChiTietDonThuoc` |
| 5B.2 | LT thanh toán | HoaDon TT=1 → Website hiện "Đã thanh toán" | `HoaDon` |

---

## 🔄 MODULE 6: LUỒNG END-TO-END

### Luồng hoàn chỉnh: Đặt lịch → Khám → Thanh toán

| Bước | Vai trò | Hành động | Form | DB Operation |
|------|---------|-----------|------|-------------|
| 1 | BN (Web) | Đặt lịch online | Website | `SP_DatLichHen` → LichHen TT=0 |
| 2 | LT | Xác nhận lịch | DashboardLeTanForm | UPDATE LichHen TT=1 |
| 3 | LT | Tiếp nhận BN | TiepNhanForm | INSERT PhieuKham TT=0 + UPDATE LichHen TT=2 |
| 4 | BS | Bắt đầu khám | DashboardBacSiForm | UPDATE PhieuKham TT=1 |
| 5 | BS | Khám + kê đơn | PhieuKhamForm | INSERT ChiTietDichVu + ChiTietDonThuoc |
| 6 | BS | Hoàn thành | PhieuKhamForm | UPDATE PhieuKham TT=2 + INSERT HoaDon TT=0 |
| 7 | LT | Thanh toán | InvoiceForm | UPDATE HoaDon TT=1 + UPDATE PhieuKham TT=3 |
| 8 | BN (Web) | Xem kết quả | Website | SELECT PhieuKham + HoaDon |

---

## 🗄️ MODULE 7: DATABASE — TÍNH TOÀN VẸN

### 7A. Tables

| # | Test Case | Kết quả mong đợi |
|---|-----------|-------------------|
| 7A.1 | Soft delete | BenhNhan, PhieuKham, HoaDon, NguoiDung, Thuoc đều có IsDeleted |
| 7A.2 | Foreign Keys | Tất cả FK hoạt động đúng, LichHen.MaNguoiDung ON DELETE SET NULL |
| 7A.3 | Constraints | CHK_TrangThaiLich (0–3), CHK_TrangThaiPhieuKham (0–4), CHK_DiemDanh (1–5) |
| 7A.4 | Defaults | LichHen.TrangThai DEFAULT 0, PhanCongCa.TrangThaiDiemDanh DEFAULT 1 |

### 7B. Stored Procedures

| # | SP | Test Case |
|---|-----|-----------|
| 7B.1 | `SP_DatLichHen` | INSERT BN mới + LichHen TrangThai=0, chặn trùng ngày |
| 7B.2 | `SP_TaoTaiKhoanNguoiDung` | Validate unique TenDangNhap/SĐT/Email, INSERT + BCrypt hash |
| 7B.3 | `SP_KhoaTaiKhoanNguoiDung` | Chặn khóa Admin, UPDATE TrangThaiTK=0 |
| 7B.4 | `SP_GuiOTP_NguoiDung` | Rate limit 3 OTP/phút, tạo 6 chữ số, hạn 5 phút |
| 7B.5 | `SP_XacThucOTP_NguoiDung` | Verify OTP, đếm sai tối đa 3 lần, kiểm tra hạn |

### 7C. Triggers

| # | Trigger | Test Case |
|---|---------|-----------|
| 7C.1 | `TRG_NhapKho_CapNhatTon` | INSERT ChiTietNhapKho → SoLuongTon tăng |
| 7C.2 | `TRG_HoaDon_CapPhatDiem` | UPDATE HoaDon TT 0→1 → cộng TongTien/1000 vào DiemTichLuy |
| 7C.3 | `TRG_NguoiDung_LogChuyenDoi` | UPDATE NguoiDung → ghi audit log DuLieuCu/DuLieuMoi |
| 7C.4 | `TRG_PhieuKham_TaoLogChuyenDoi` | UPDATE PhieuKham → ghi vào PhieuKham_LichSu |
| 7C.5 | `TRG_HoaDon_TaoLogChuyenDoi` | UPDATE HoaDon → ghi vào HoaDon_LichSu |

### 7D. Views

| # | View | Test Case |
|---|------|-----------|
| 7D.1 | `VW_HoSoBenhAn` | JOIN đúng + filter IsDeleted=0 cho BenhNhan + PhieuKham |
| 7D.2 | `VW_BaoCaoDoanhThu` | Tổng hợp theo năm/tháng + filter HoaDon.IsDeleted=0 |
| 7D.3 | `VW_TonKhoTheoLo` | FEFO (HanSuDung ASC), SoLuongConLai > 0 |
| 7D.4 | `VW_DanhSachTaiKhoan` | JOIN VaiTro + filter NguoiDung.IsDeleted=0 |

---

## ⚙️ MODULE 8: HẠ TẦNG & CẤU HÌNH

| # | Test Case | Kết quả mong đợi |
|---|-----------|-------------------|
| 8.1 | Connection string | App.config → `DERMASOFT` key hoặc fallback `localhost\sa` |
| 8.2 | BCrypt library | `BCrypt.Net-Next` NuGet — hash + verify |
| 8.3 | OxyPlot | Biểu đồ doanh thu Dashboard Admin |
| 8.4 | Guna.UI2.WinForms | Controls hiện đại: Button, TextBox, ComboBox, DateTimePicker |
| 8.5 | Double buffering | `WS_EX_COMPOSITED` trên 3 MainForm + `DoubleBufferHelper` cho panels |
| 8.6 | AppSettings | Đọc `CaiDatHeThong` → NguongThap, NguongNguyHiem, MatKhauMacDinh |

---

## ✅ CHECKLIST SỬA LỖI ĐÃ HOÀN THÀNH

| # | Vấn đề | Mô tả | Trạng thái |
|---|--------|-------|-----------|
| C1 | SELECT debug đầu file | `01_Tables_Data.sql` — xóa `select * from NguoiDung/XacThucOTP` | ✅ Đã sửa |
| C2 | LichHen DEFAULT 1→0 | Lịch hẹn mới phải "Chờ xác nhận" = 0 | ✅ Đã sửa |
| C3 | SP_DatLichHen TrangThai=1→0 | Website đặt lịch → Chờ XN đúng logic | ✅ Đã sửa |
| C4 | SP_DatLichHen check IN(1,2)→IN(0,1) | Kiểm tra trùng lịch đúng trạng thái | ✅ Đã sửa |
| C5 | Trigger dùng TongTien thay TienKhachTra | Cộng điểm theo giá trị hóa đơn, không phải tiền khách đưa | ✅ Đã sửa |
| C6 | Trigger chống cộng trùng | Thêm `AND d.TrangThai = 0` — chỉ cộng khi 0→1 | ✅ Đã sửa |
| C7 | Thuoc thêm IsDeleted | Gộp migration vào CREATE TABLE gốc | ✅ Đã sửa |
| C8 | VW_HoSoBenhAn + IsDeleted | Thêm WHERE bn.IsDeleted=0, pk.IsDeleted=0 | ✅ Đã sửa |
| C9 | VW_DanhSachTaiKhoan + IsDeleted | Thêm WHERE nd.IsDeleted=0 | ✅ Đã sửa |
| C10 | Indexes trùng lặp | Xóa bản trùng trong 01, giữ bản trong 05 | ✅ Đã sửa |
| C11 | Gộp migration SQL | 9 file → 6 file + 1 migration apply | ✅ Đã sửa |
| C12 | CaiDatHeThong gộp vào gốc | Bảng settings tạo ngay trong 01_Tables_Data.sql | ✅ Đã sửa |
| C13 | LoginForm: sai MK không hiện lỗi | Unhook TextChanged trước khi Clear → tránh AnLoi() | ✅ Đã sửa |
| C14 | StaffForm: UPDATE không lưu TenDangNhap/MK | Thêm vào SQL UPDATE động | ✅ Đã sửa |
| C15 | StaffForm: thêm nút xóa NV | Soft-delete + chặn xóa chính mình | ✅ Đã sửa |
| C16 | TonKhoForm: KPI tồn thấp | Đổi sang đếm SoLuongConLai ≤ AppSettings.NguongThap | ✅ Đã sửa |
| C17 | NhapKhoForm: lịch sử nhập | Thêm popup bảng phiếu nhập theo ngày | ✅ Đã sửa |
| C18 | PhanCongCa: điểm danh | Auto điểm danh khi đăng nhập (LoginForm.AutoDiemDanh) | ✅ Đã sửa |
| C19 | FindForm() embedded | Form con TopLevel=false → duyệt this.Parent thủ công | ✅ Đã sửa |
| C20 | BeginInvoke điều hướng | Dashboard điều hướng → dùng BeginInvoke tránh dispose | ✅ Đã sửa |
| C21 | Sidebar highlight điều hướng | Nút action Dashboard → ChuyenMenu() cập nhật sidebar | ✅ Đã sửa |
| C22 | Timer stop khi dispose | _refreshTimer.Stop() trước Dispose | ✅ Đã sửa |
| C23 | InvoiceForm thông tin PK | Load từ ThongTinPhongKham, không hardcode | ✅ Đã sửa |
| C24 | BCrypt $2y$ vs $2a$ | LoginForm chuyển $2y$ → $2a$ trước verify | ✅ Đã sửa |
| C25 | Transaction tiếp nhận | TiepNhanForm: INSERT PhieuKham + UPDATE LichHen trong 1 transaction | ✅ Đã sửa |
