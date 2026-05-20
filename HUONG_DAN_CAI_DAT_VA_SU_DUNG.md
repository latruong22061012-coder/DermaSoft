# 📘 HƯỚNG DẪN CÀI ĐẶT & SỬ DỤNG — DermaSoft (Phần mềm Quản lý Phòng khám Da liễu)

> Tài liệu này hướng dẫn **chi tiết từng bước** để cài đặt, cấu hình cơ sở dữ liệu, build và chạy ứng dụng **DermaSoft** trên một máy tính / máy chủ mới. Sau phần cài đặt là **hướng dẫn sử dụng** toàn bộ các phân hệ của hệ thống theo từng vai trò người dùng.

---

## 🧭 MỤC LỤC

1. [Tổng quan dự án](#1-tổng-quan-dự-án)
2. [Yêu cầu hệ thống](#2-yêu-cầu-hệ-thống)
3. [Công cụ cần cài đặt](#3-công-cụ-cần-cài-đặt)
4. [Lấy mã nguồn về máy](#4-lấy-mã-nguồn-về-máy)
5. [Thiết lập cơ sở dữ liệu SQL Server](#5-thiết-lập-cơ-sở-dữ-liệu-sql-server)
6. [Cấu hình kết nối (App.config)](#6-cấu-hình-kết-nối-appconfig)
7. [Khôi phục NuGet & Build dự án](#7-khôi-phục-nuget--build-dự-án)
8. [Chạy ứng dụng lần đầu](#8-chạy-ứng-dụng-lần-đầu)
9. [Triển khai (publish) tới máy người dùng cuối](#9-triển-khai-publish-tới-máy-người-dùng-cuối)
10. [Khắc phục sự cố thường gặp](#10-khắc-phục-sự-cố-thường-gặp)
11. [HƯỚNG DẪN SỬ DỤNG — Theo vai trò](#11-hướng-dẫn-sử-dụng--theo-vai-trò)
12. [Quy trình nghiệp vụ end-to-end](#12-quy-trình-nghiệp-vụ-end-to-end)
13. [FAQ & Mẹo vận hành](#13-faq--mẹo-vận-hành)

---

## 1. Tổng quan dự án

| Hạng mục | Thông tin |
|---|---|
| **Tên dự án** | DermaSoft — Phần mềm quản lý phòng khám chăm sóc thẩm mỹ |
| **Loại ứng dụng** | Windows Forms desktop (1 file `.exe`) |
| **Ngôn ngữ** | C# |
| **Framework** | .NET Framework **4.8** |
| **Cơ sở dữ liệu** | Microsoft SQL Server (2016+ khuyến nghị) |
| **UI Library** | Guna.UI2.WinForms 2.0.4.7 |
| **Mã hoá mật khẩu** | BCrypt.Net-Next 4.0.3 |
| **Biểu đồ thống kê** | OxyPlot 2.1.2 |
| **Repository** | https://github.com/latruong22061012-coder/DermaSoft |

**Các phân hệ chính:** Quản lý nhân viên — Bệnh nhân — Lịch hẹn — Phiếu khám — Đơn thuốc — Dịch vụ — Hoá đơn / Thanh toán — Kho thuốc (Nhập / Tồn) — Phân ca làm việc — Bảng lương — Thẻ thành viên & Hạng — Báo cáo doanh thu / kho — Cấu hình hệ thống.

---

## 2. Yêu cầu hệ thống

### Máy chủ cài SQL Server
| Thành phần | Tối thiểu | Khuyến nghị |
|---|---|---|
| Hệ điều hành | Windows 10 / Windows Server 2016 | Windows 11 / Server 2019+ |
| CPU | 2 core | 4 core+ |
| RAM | 4 GB | 8 GB+ |
| Đĩa trống | 5 GB | 20 GB+ |
| SQL Server | 2016 Express | 2019/2022 Standard |

### Máy client chạy ứng dụng
| Thành phần | Yêu cầu |
|---|---|
| Hệ điều hành | Windows 10 (1809+) hoặc Windows 11 |
| .NET Framework | **4.8** (đã có sẵn từ Windows 10 1903+) |
| RAM | 2 GB trở lên |
| Độ phân giải | 1280×720 trở lên (UI tối ưu cho 1366×768+) |
| Kết nối mạng | Truy cập được tới SQL Server (cùng LAN hoặc Internet/VPN) |

> ✅ Có thể cài SQL Server và ứng dụng trên **cùng một máy** (mô hình 1-máy nhỏ).

---

## 3. Công cụ cần cài đặt

Trên máy **phát triển / triển khai**, cài lần lượt theo thứ tự sau:

### 3.1. Microsoft SQL Server (BẮT BUỘC)
- Tải bản **SQL Server 2019/2022 Express** (miễn phí) tại: https://www.microsoft.com/sql-server/sql-server-downloads
- Khi cài chọn **Basic** hoặc **Custom → Database Engine Services**.
- **Quan trọng:** chọn chế độ xác thực **Mixed Mode (SQL Server and Windows Authentication)** và đặt mật khẩu cho user `sa`.

### 3.2. SQL Server Management Studio (SSMS) (BẮT BUỘC)
- Tải SSMS 19+ tại: https://aka.ms/ssmsfullsetup
- Dùng để chạy các script `.sql` tạo database.

### 3.3. Visual Studio 2022 (Khuyến nghị nếu cần build)
- Tải **Visual Studio Community 2022** (miễn phí): https://visualstudio.microsoft.com/downloads/
- Khi cài, **chọn workload**:
  - ✅ **.NET desktop development**
  - ✅ Trong panel bên phải, đảm bảo có mục **".NET Framework 4.8 targeting pack"**
  - ✅ (Tuỳ chọn) **Data storage and processing** — kèm SQL Server tools

### 3.4. .NET Framework 4.8 Runtime (BẮT BUỘC trên máy chạy `.exe`)
- Nếu máy client chưa có, tải tại: https://dotnet.microsoft.com/download/dotnet-framework/net48
- Bản Runtime đủ để chạy, không cần Developer Pack.

### 3.5. Git (Khuyến nghị)
- Tải tại: https://git-scm.com/download/win — dùng để `clone` mã nguồn.

---

## 4. Lấy mã nguồn về máy

Mở **PowerShell** hoặc **Command Prompt**:

```powershell
cd C:\
mkdir Projects
cd Projects
git clone https://github.com/latruong22061012-coder/DermaSoft.git
cd DermaSoft
```

Cấu trúc thư mục quan trọng sau khi clone:

```
DermaSoft\
├─ DermaSoft.sln                    ← Mở bằng Visual Studio
├─ DermaSoft.csproj                 ← Project file
├─ App.config                       ← Chuỗi kết nối SQL Server
├─ packages.config                  ← Danh sách NuGet
├─ DermaSoft\
│  ├─ Data\
│  │  ├─ 01_Tables_Data.sql        ⭐ Tạo DB & insert dữ liệu mẫu
│  │  ├─ 02_StoredProcedures.sql
│  │  ├─ 03_Triggers.sql
│  │  ├─ 04_Views.sql
│  │  ├─ 05_Constraints.sql
│  │  ├─ 05_SeedTestData.sql       (tuỳ chọn — seed thêm)
│  │  ├─ MIGRATION_*.sql           ← Các migration nâng cấp
│  │  └─ FIX_*.sql / TEST_*.sql    ← Patch & test
│  ├─ Forms\                        ← Các form Windows
│  ├─ Models\, Services\, Theme\, Helpers\, Enums\
│  └─ DatabaseConnection.cs
└─ Properties\, bin\, obj\
```

---

## 5. Thiết lập cơ sở dữ liệu SQL Server

> ⚠️ **THỨ TỰ CHẠY SCRIPT RẤT QUAN TRỌNG** — chạy đúng theo số thứ tự sau, **không bỏ bước**.

### Bước 5.1 — Mở SSMS và kết nối SQL Server

1. Mở **SSMS** → kết nối đến server (vd: `localhost`, `.\SQLEXPRESS`, hoặc tên server thực).
2. Đăng nhập bằng **SQL Server Authentication**:
   - Login: `sa`
   - Password: (mật khẩu bạn đặt khi cài)

### Bước 5.2 — Chạy các script theo đúng thứ tự

Mở **từng file** sau trong SSMS (File → Open → File), rồi nhấn **F5 (Execute)**.

| # | Tên file | Mục đích | Bắt buộc? |
|---|---|---|---|
| 1 | `DermaSoft\Data\01_Tables_Data.sql` | Tạo database `DERMASOFT`, tạo toàn bộ bảng + dữ liệu khởi tạo (vai trò, hạng thành viên, ca làm việc, admin user, nhà cung cấp…) | ✅ **Bắt buộc** |
| 2 | `DermaSoft\Data\02_StoredProcedures.sql` | Tạo các Stored Procedure (báo cáo, tính toán…) | ✅ Bắt buộc |
| 3 | `DermaSoft\Data\03_Triggers.sql` | Tạo Trigger (cấp điểm tự động, kiểm tra tồn kho…) | ✅ Bắt buộc |
| 4 | `DermaSoft\Data\04_Views.sql` | Tạo các View hỗ trợ truy vấn | ✅ Bắt buộc |
| 5 | `DermaSoft\Data\05_Constraints.sql` | Thêm các CHECK / UNIQUE constraint bổ sung | ✅ Bắt buộc |
| 6 | `DermaSoft\Data\05_SeedTestData.sql` | (Tuỳ chọn) Seed dữ liệu mẫu để test: bệnh nhân, thuốc, lịch hẹn… | ⚪ Tuỳ chọn |

### Bước 5.3 — Chạy các MIGRATION (NÂNG CẤP)

Sau khi đã có schema gốc, chạy tiếp **các migration** theo thứ tự bảng chữ cái (vì các migration được đặt tên theo tính năng, mỗi script đều có `IF NOT EXISTS` để an toàn chạy lại):

```
DermaSoft\Data\MIGRATION_APPLY_FIXES.sql            (fix tổng hợp)
DermaSoft\Data\MIGRATION_BANGLUONG.sql              (bảng lương)
DermaSoft\Data\MIGRATION_DONGBO_DOANHTHU.sql        (đồng bộ doanh thu)
DermaSoft\Data\MIGRATION_FIX_TONKHO.sql             (tồn kho bước 1)
DermaSoft\Data\MIGRATION_FIX_TONKHO_BUOC2.sql       (tồn kho bước 2)
DermaSoft\Data\MIGRATION_KHOATHE_THANHVIEN.sql      (khoá thẻ thành viên)
DermaSoft\Data\MIGRATION_SOTHUTU.sql                (số thứ tự khám)
DermaSoft\Data\MIGRATION_SOTHUTU_LICHHEN.sql        (số thứ tự lịch hẹn)
DermaSoft\Data\MIGRATION_PHANCONG_3CA.sql           (tối đa 3 ca/ngày + SiềuChuẩnNgày 8→12)
```

> ⚠️ Khuyến nghị **mỗi script chạy 1 lần, đọc cửa sổ Messages** để chắc chắn không có lỗi đỏ.

### Bước 5.4 — (Tuỳ chọn) Chạy FIX & TEST

- `FIX_TONKHO_LECH.sql` — chỉ chạy nếu có lệch tồn kho thực tế.
- `TEST_TONKHO_AUTO.sql` — script kiểm thử, **KHÔNG chạy trên DB production**.

### Bước 5.5 — Kiểm tra database

Trong SSMS, mở Query Window mới và chạy:

```sql
USE DERMASOFT;
GO
SELECT name FROM sys.tables ORDER BY name;
SELECT TOP 5 * FROM NguoiDung;
SELECT * FROM VaiTro;
SELECT * FROM HangThanhVien;
```

Phải thấy: đầy đủ các bảng (`NguoiDung`, `BenhNhan`, `LichHen`, `PhieuKham`, `HoaDon`, `Thuoc`, `PhanCongCa`, `ThanhVienInfo`, `HangThanhVien`, …) và **1 user admin** với `TenDangNhap = 'aB1cD'`.

---

## 6. Cấu hình kết nối (App.config)

Mở file **`App.config`** ở thư mục gốc dự án (cạnh `DermaSoft.sln`). Mặc định:

```xml
<connectionStrings>
  <add name="DERMASOFT"
       connectionString="Server=localhost;Database=DERMASOFT;User Id=sa;Password=DarmaSoft2026;TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### Sửa lại cho phù hợp môi trường mới

| Tham số | Sửa thành | Ví dụ |
|---|---|---|
| `Server` | Tên/IP máy chủ SQL | `localhost`, `.\SQLEXPRESS`, `192.168.1.10`, `SERVER01\SQLEXPRESS` |
| `Database` | Giữ nguyên | `DERMASOFT` |
| `User Id` | Tài khoản SQL Login | `sa` (hoặc user riêng đã GRANT quyền) |
| `Password` | Mật khẩu tương ứng | (đặt trong SSMS) |
| `TrustServerCertificate` | Để `True` nếu chứng chỉ self-signed | `True` |

> 💡 **Khuyến nghị bảo mật:** Tạo SQL Login riêng (vd: `dermasoft_app`) chỉ có quyền `db_datareader`, `db_datawriter` và `EXECUTE` trên DB `DERMASOFT`, không dùng `sa` trong production.

```sql
USE DERMASOFT;
CREATE LOGIN dermasoft_app WITH PASSWORD = 'Strong#Pwd2026';
CREATE USER dermasoft_app FOR LOGIN dermasoft_app;
ALTER ROLE db_datareader ADD MEMBER dermasoft_app;
ALTER ROLE db_datawriter ADD MEMBER dermasoft_app;
GRANT EXECUTE TO dermasoft_app;
```

### Cho phép kết nối từ máy khác (nếu app & SQL trên 2 máy)
1. **SQL Server Configuration Manager** → SQL Server Network Configuration → Protocols → bật **TCP/IP** → Restart service.
2. **Windows Firewall** → mở **port 1433** (TCP) Inbound.
3. SSMS → Server Properties → Connections → tick **Allow remote connections**.

---

## 7. Khôi phục NuGet & Build dự án

### Cách 1 — Bằng Visual Studio 2022

1. Mở `DermaSoft.sln` bằng Visual Studio 2022.
2. Khi VS hỏi restore NuGet → bấm **Yes** (hoặc menu **Project → Restore NuGet Packages**).
3. Chờ tải đủ các gói (`Guna.UI2.WinForms`, `BCrypt.Net-Next`, `OxyPlot.WindowsForms`, …).
4. Menu **Build → Build Solution** (phím tắt **Ctrl+Shift+B**).
5. Output mong đợi: `========== Build: 1 succeeded, 0 failed ==========`.

### Cách 2 — Bằng dòng lệnh (MSBuild)

```powershell
# Đảm bảo đã có nuget.exe trong PATH (hoặc dùng dotnet)
cd C:\Projects\DermaSoft
nuget restore DermaSoft.sln
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" DermaSoft.sln /p:Configuration=Debug
```

Sau khi build xong, file thực thi nằm tại:

```
C:\Projects\DermaSoft\bin\Debug\DermaSoft.exe
```

---

## 8. Chạy ứng dụng lần đầu

1. Trong Visual Studio: nhấn **F5** (Debug) hoặc **Ctrl+F5** (Run without debug).
2. Hoặc mở Explorer → vào `bin\Debug\` → chạy **`DermaSoft.exe`**.
3. Form **đăng nhập** hiện ra.

### Tài khoản Admin mặc định (đã seed trong `01_Tables_Data.sql`)

| Trường | Giá trị |
|---|---|
| **Tên đăng nhập** | `latruong` |
| **Mật khẩu** | `latruong2206@` |

> Lần đầu đăng nhập có thể bị bắt **đổi mật khẩu** (cờ `DoiMatKhau = 1`) — làm theo hướng dẫn trên form.

Sau khi đăng nhập, hệ thống điều hướng theo **vai trò**:
- **Admin (1)** → `MainForm` (toàn quyền)
- **Bác sĩ (2)** → `MainFormBacSi`
- **Lễ tân (3)** → `MainFormLeTan`
- **Quản kho (5)** → `MainFormQuanKho`

---

## 9. Triển khai (publish) tới máy người dùng cuối

### Bước 9.1 — Build Release
```
Visual Studio → chọn cấu hình Release → Build → Build Solution
```
Sản phẩm tại: `bin\Release\`

### Bước 9.2 — Copy thư mục `bin\Release\` lên máy đích
Bao gồm tất cả các file:
```
DermaSoft.exe
DermaSoft.exe.config        ← (chính là App.config sau khi build)
*.dll (Guna.UI2.WinForms.dll, BCrypt.Net-Next.dll, OxyPlot.*.dll …)
```

### Bước 9.3 — Trên máy đích
1. Cài **.NET Framework 4.8 Runtime** (nếu chưa có).
2. Sửa **`DermaSoft.exe.config`** → connectionString trỏ đúng SQL Server.
3. Đảm bảo máy đích **ping được** SQL Server và mở port 1433.
4. Chạy `DermaSoft.exe`.

> 💡 Có thể tạo shortcut + đặt vào `shell:startup` để app tự chạy khi đăng nhập Windows.

---

## 10. Khắc phục sự cố thường gặp

| Triệu chứng | Nguyên nhân & Cách xử lý |
|---|---|
| `Cannot open database "DERMASOFT" requested by the login.` | Chưa chạy `01_Tables_Data.sql`, hoặc connection string sai tên DB. |
| `Login failed for user 'sa'.` | Sai mật khẩu, hoặc SQL Server chưa bật **Mixed Mode**. Vào SSMS → Server Properties → Security → SQL Server and Windows Authentication mode → Restart service. |
| `A network-related or instance-specific error...` | SQL Service chưa bật, hoặc chưa bật TCP/IP, hoặc Firewall chặn port 1433. |
| Build báo thiếu `Guna.UI2.WinForms` | Chưa restore NuGet → menu Tools → NuGet Package Manager → Restore. |
| Khi đăng nhập báo "Sai mật khẩu" với admin mặc định | Chỉ áp dụng mật khẩu mặc định **`Admin@2026`** nếu DB vừa được tạo mới từ script. Nếu đã đổi rồi, dùng mật khẩu mới. |
| Form hiển thị bị vỡ font / icon | Cài thêm font **Segoe UI Emoji** (mặc định Windows đã có). |
| Báo lỗi liên quan **DaKhoa**, **NgayKhoa**, **SoThuTu** | Chưa chạy đầy đủ các `MIGRATION_*.sql`. Quay lại bước 5.3. |
| Tồn kho âm hoặc lệch | Chạy `MIGRATION_FIX_TONKHO.sql` rồi `MIGRATION_FIX_TONKHO_BUOC2.sql`. |

---

# 📖 11. HƯỚNG DẪN SỬ DỤNG — Theo vai trò

Sau khi đăng nhập, mỗi vai trò có một **Main Form** với menu điều hướng phù hợp. Dưới đây mô tả các chức năng theo từng vai trò.

## 11.1. 🔐 Đăng nhập & Đổi mật khẩu (chung cho mọi vai trò)

**Form:** `LoginForm`
- Nhập **Tên đăng nhập** + **Mật khẩu** → bấm **Đăng nhập**.
- Hệ thống xác thực bằng **BCrypt** (mật khẩu được hash trong DB, không lưu plain text).
- Sai 5 lần liên tiếp có thể bị tạm khoá (theo cấu hình).
- Nếu cờ `DoiMatKhau = 1` → tự động mở `ChangePasswordForm` để buộc đổi mật khẩu.

**Form:** `ChangePasswordForm`
- Nhập mật khẩu cũ + mật khẩu mới (≥ 8 ký tự, có chữ hoa, chữ thường, số).
- Mật khẩu mới được hash BCrypt rồi lưu vào DB.

**Form:** `ProfileForm`
- Cập nhật ảnh đại diện, email, SĐT.
- Xác thực email (gửi OTP qua email — nếu đã cấu hình SMTP).

---

## 11.2. 👑 Vai trò ADMIN — `MainForm`

Có quyền truy cập **tất cả** các phân hệ.

### a) Quản lý nhân viên (`StaffForm`)
- Thêm / Sửa nhân viên (HoTen, SoDienThoai, Email, Vai trò, Trạng thái TK).
- Mật khẩu mặc định khi tạo mới = giá trị key `MK_MAC_DINH` trong bảng `CaiDatHeThong` (mặc định **`Temp@2026`**) — nhân viên bị buộc đổi khi đăng nhập lần đầu.
- **Reset mật khẩu** về mặc định.
- Khoá / mở khoá tài khoản (`TrangThaiTK`).

### b) Phân công ca (`PhanCongCaForm`)
- Chọn **tuần** hoặc **ngày** → xem lịch phân ca tất cả nhân viên.
- Chọn nhân viên + ca làm (Sáng/Chiều/Tối) + ngày → **Lưu Phân Công**.
- Click vào dòng trong bảng để **chỉnh sửa**.
- ⚠️ **LƯU Ý NGHIỆP VỤ:** Sau khi đã phân ca, **KHÔNG được xoá** lịch — chỉ được cập nhật (đổi nhân viên / đổi ca / đổi ngày). Nút "Xoá Phân Công" đã được ẩn theo quy định.

### c) Cấu hình lương (`CauHinhLuongForm`) & Bảng lương (`BangLuongForm`)
- Cấu hình mức lương theo vai trò.
- Tính lương tháng dựa trên số ca đã chấm công + thưởng/phạt.
- In **Phiếu lương** (`PhieuLuongPrinter`).

### d) Cấu hình hệ thống (`SettingsForm`)
- Ngưỡng tồn kho: **Thấp / Nguy hiểm**.
- Mật khẩu mặc định nhân viên (`MK_MAC_DINH`).
- Thông tin phòng khám (tên, slogan, địa chỉ, hotline, giờ mở cửa…) — hiển thị trên hoá đơn in.
- Sao lưu / phục hồi cấu hình.

### e) Quản lý hoá đơn (`QuanLyHoaDonForm`)
- Tra cứu **mọi hoá đơn** (đã/chưa thanh toán).
- Xem chi tiết: dịch vụ, thuốc, giảm giá, phương thức TT.
- Sửa giảm giá / phương thức trước khi khách thanh toán (TrangThai = 0).
- **Không** xoá hoá đơn đã thanh toán.

### f) Báo cáo
- **Doanh thu** (`BaoCaoDoanhThuForm`) — lọc theo ngày/tháng/năm, theo bác sĩ, theo dịch vụ.
- **Kho** (`BaoCaoKhoForm`) — nhập / xuất / tồn theo kỳ.
- **Tổng hợp** (`ReportForm`) — biểu đồ OxyPlot.



---

## 11.3. 🏥 Vai trò LỄ TÂN — `MainFormLeTan`

### a) Dashboard (`DashboardLeTanForm`)
- Hiển thị: lịch hẹn hôm nay, số phiếu chờ khám, số hoá đơn chờ thanh toán, biểu đồ lượt khám tuần.

### b) Quản lý bệnh nhân (`PatientForm`)
- Thêm / Sửa hồ sơ bệnh nhân (HoTen, NgaySinh, GioiTinh, SDT, Địa chỉ, Tiền sử bệnh).
- Xem chi tiết (`BenhNhanDetailForm`): lịch sử khám, hoá đơn, hình ảnh da liễu, đánh giá.
- Tìm kiếm theo tên / SĐT / mã BN.

### c) Lịch hẹn (`AppointmentForm`)
- Đặt lịch hẹn: chọn BN + Bác sĩ + Ngày giờ + Dịch vụ dự kiến.
- Hệ thống tự sinh **Số Thứ Tự** trong ngày (sau khi đã chạy `MIGRATION_SOTHUTU_LICHHEN.sql`).
- Trạng thái: Chưa đến / Đã đến / Đã khám / Đã huỷ.

### d) Tiếp nhận (`TiepNhanForm`)
- Khi BN tới khám: từ lịch hẹn → **Chuyển sang phiếu khám** (tạo `PhieuKham` trạng thái 1 = Chờ khám).
- Phát số thứ tự cho BS.

### e) Thanh toán / Lập hoá đơn (`InvoiceForm`)
- Chọn phiếu khám đã hoàn tất → form tự tải dịch vụ, thuốc, tính tổng.
- Áp dụng **giảm giá theo hạng thành viên** (Đỏ / Bạc / Vàng / Kim Cương) tự động.
  - ⚠️ Nếu thẻ thành viên đang **bị khoá** (cột `DaKhoa = 1`), hệ thống **sẽ KHÔNG áp dụng ưu đãi** dù khách vẫn còn điểm tích lũy.
- Nhập tiền khách trả → tính tiền thừa → chọn phương thức (Tiền mặt / Chuyển khoản / Thẻ).
- **Xác nhận thanh toán** → cập nhật `HoaDon.TrangThai = 1`, `PhieuKham.TrangThai = 3`, trigger tự cộng điểm tích lũy & nâng hạng.
- **In hoá đơn** (`HoaDonPrinter`) — preview trước khi in.

### f) Quản lý thành viên (`MemberForm`)
- Danh sách BN có thẻ thành viên, lọc theo hạng.
- **Đăng ký thẻ** cho BN mới.
- **🔒 Khoá thẻ** (thay cho "Huỷ thẻ"): điểm và lịch sử được **giữ nguyên**, nhưng **các lần thanh toán tiếp theo sẽ không được hưởng ưu đãi** theo cấp thẻ. Có thể **mở khoá** lại sau.

### g) Quản lý hoá đơn (`QuanLyHoaDonForm`)
- Xem danh sách hoá đơn theo ngày, lọc theo trạng thái / phương thức.
- In lại hoá đơn cũ.

### h) Đánh giá (`DanhGiaForm`)
- Ghi nhận đánh giá hài lòng của khách sau khám.
- Cập nhật `TyLeHaiLong` cho thẻ thành viên.

---

## 11.4. 🩺 Vai trò BÁC SĨ — `MainFormBacSi`

### a) Dashboard (`DashboardBacSiForm`)
- Danh sách BN cần khám trong ca, biểu đồ chẩn đoán phổ biến, lịch khám tuần.

### b) Phiếu khám (`PhieuKhamForm`)
- Mở từ Dashboard hoặc từ danh sách phiếu chờ.
- Nhập **Triệu chứng**, **Chẩn đoán**, **Ghi chú**.
- **Thêm dịch vụ chăm sóc da** (vào `ChiTietDichVu`).
- **Kê đơn thuốc** (vào `ChiTietDonThuoc`): chọn thuốc + số lượng + liều dùng + cách dùng. Hệ thống kiểm tra tồn kho.
- **Đính kèm hình ảnh** (`HinhAnhForm`): upload ảnh tổn thương / trước-sau điều trị.
- **In đơn thuốc** (`DonThuocPrinter`).
- Khi khám xong: chuyển trạng thái sang **2 = Đã khám, chờ TT** → hệ thống tự tạo `HoaDon` trạng thái 0 (chờ TT).

### c) Lịch trực
- Xem ca trực của mình (chỉ đọc, không phân ca được).

---

## 11.5. 📦 Vai trò QUẢN KHO — `MainFormQuanKho`

### a) Dashboard (`DashboardQuanKhoForm`)
- Thuốc sắp hết hạn, thuốc dưới ngưỡng (Thấp / Nguy hiểm), nhập kho tuần này.

### b) Quản lý thuốc (`ThuocForm`)
- Thêm / Sửa thuốc (TenThuoc, DonVi, DonGia, NhaCungCap, HanSuDung, MoTa).
- Không xoá thuốc đã có giao dịch — chỉ ẩn (`IsDeleted = 1`).

### c) Nhập kho (`NhapKhoForm`)
- Tạo phiếu nhập: chọn NCC + danh sách thuốc + số lượng + đơn giá nhập.
- Lưu phiếu → **trigger tự cộng tồn kho** (`TonKho`).

### d) Tồn kho (`TonKhoForm`)
- Xem tồn từng thuốc, lọc theo mức độ (Đủ / Thấp / Nguy hiểm / Hết).
- Cảnh báo theo ngưỡng đã cấu hình.

### e) Báo cáo kho (`BaoCaoKhoForm`)
- Nhập / Xuất / Tồn theo kỳ, theo thuốc, theo NCC.

---

# 🔄 12. Quy trình nghiệp vụ end-to-end

Quy trình điển hình một khách hàng đến khám:

```
[LỄ TÂN]                       [BÁC SĨ]                    [LỄ TÂN]
  │                               │                            │
  ▼                               ▼                            ▼
1. Tạo / Tìm BN  ─►  2. Đặt lịch hẹn  ─►  3. Tiếp nhận
   (PatientForm)        (AppointmentForm)     (TiepNhanForm)
                                                   │
                                                   ▼
                                            4. Mở phiếu khám
                                              (PhieuKhamForm)
                                                   │  - Nhập chẩn đoán
                                                   │  - Thêm dịch vụ
                                                   │  - Kê đơn thuốc
                                                   │  - Đính kèm ảnh
                                                   ▼
                                       5. Kết thúc khám → TT = 2
                                          (HoaDon tự tạo TT=0)
                                                   │
                                                   ▼
                                         6. Lập hoá đơn
                                            (InvoiceForm)
                                            - Áp ưu đãi thẻ TV
                                              (nếu không bị khoá)
                                            - Nhập tiền khách
                                            - Xác nhận TT → TT=1
                                                   │
                                                   ▼
                                            7. In hoá đơn
                                            (HoaDonPrinter)
                                                   │
                                                   ▼
                                       8. Trigger cộng điểm
                                          & nâng hạng TV
                                                   │
                                                   ▼
                                         9. Đánh giá hài lòng
                                            (DanhGiaForm)
```

**Cuối tháng (Admin):**
- Tính lương (`BangLuongForm`) dựa trên `PhanCongCa` đã chấm.
- Xuất báo cáo doanh thu (`BaoCaoDoanhThuForm`).
- Sao lưu DB bằng SSMS (Tasks → Back Up).

---

# 💡 13. FAQ & Mẹo vận hành

**Q1: Tôi muốn chuyển server SQL sang máy khác?**
→ Backup DB từ SSMS (`.bak`), Restore trên server mới, sửa `App.config` → connectionString trỏ tới server mới.

**Q2: Quên mật khẩu admin?**
→ Vào SSMS, chạy:
```sql
USE DERMASOFT;
-- Đặt lại admin về mật khẩu "Admin@2026"
UPDATE NguoiDung
SET MatKhau = '$2a$10$63S1lK7cvNppSmkb7vnTs.O1sz/.83/lu0Gg3avuYtI8RKwAzGMfW',
    DoiMatKhau = 1
WHERE TenDangNhap = 'aB1cD';
```

**Q3: Cần đổi đơn vị tiền tệ / ngôn ngữ?**
→ Hệ thống mặc định **VNĐ** và **tiếng Việt**. Sửa trong `Theme/AppConstants.cs` và `CultureInfo("vi-VN")` ở các form nếu cần.

**Q4: Có thể chạy đa người dùng đồng thời không?**
→ Có. Mỗi máy client cài `.exe` riêng, cùng trỏ về 1 SQL Server. Đảm bảo SQL Server cho phép kết nối từ xa.

**Q5: Có gửi email OTP không?**
→ Có (`TrangThaiOTPEnum` đã định nghĩa). Tuy nhiên cần cấu hình SMTP trong `AppSettings.cs` (Host, Port, User, Password) trước khi sử dụng.

**Q6: Sao lưu định kỳ?**
→ Khuyến nghị: dùng SQL Server Agent (bản Standard+) hoặc Task Scheduler chạy:
```sql
BACKUP DATABASE DERMASOFT 
TO DISK = N'D:\Backup\DERMASOFT_yyyyMMdd.bak' 
WITH COMPRESSION, INIT;
```

**Q7: Thẻ thành viên — sự khác biệt giữa "Khoá" và "Huỷ"?**
→ Hệ thống **chỉ có Khoá / Mở khoá** (cột `DaKhoa`). Khi khoá:
- ✅ Điểm tích lũy & lịch sử giao dịch **được giữ nguyên**.
- ❌ Các lần thanh toán **tiếp theo không nhận ưu đãi** theo cấp thẻ.
- 🔓 Có thể mở khoá lại bất kỳ lúc nào.

**Q8: Phân công ca — Admin lỡ phân nhầm thì sao?**
→ Theo quy định nghiệp vụ, **không được xoá** lịch đã phân — chỉ được **cập nhật**. Click vào dòng tương ứng trong bảng phân ca → đổi nhân viên / ca / ngày → bấm **Cập Nhật**.

**Q9: Mỗi nhân viên được làm tối đa bao nhiêu ca một ngày?**
→ **Tối đa 3 ca / ngày** (Sáng + Chiều + Tối, tổng 12 tiếng). Không được phân trùng ca đã phân trước đó cho cùng nhân viên trong cùng ngày. Ràng buộc được áp ở cả tầng UI (`PhanCongCaForm`) và tầng DB (UNIQUE + trigger `TRG_PhanCongCa_MaxCaTrongNgay`). Tương ứng, `SoGioChuanNgay` trong `CauHinhLuong` đã được cập nhật từ **8 → 12** để logic tính tăng ca trong bảng lương khớp với khung ca mới.

---

## 📞 Hỗ trợ

- Repository: https://github.com/latruong22061012-coder/DermaSoft
- Issue tracker: tab **Issues** trên GitHub.
- Tài liệu API & schema chi tiết: thư mục `Wireframes/HUONG_DAN_TICH_HOP_VA_API.md` trong repo.

---

✅ **Hoàn tất!** Sau khi làm đúng các bước trên, bạn đã có một hệ thống DermaSoft hoạt động đầy đủ trên máy chủ mới.
