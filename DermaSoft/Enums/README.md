# 📋 DermaSoft Enums - Hướng Dẫn Sử Dụng

## 📌 Giới Thiệu

Tất cả các enum trong dự án DermaSoft được định nghĩa trong thư mục `DermaSoft\Enums\` và được đồng bộ với SQL Server database.

## 🗂️ Cấu Trúc Enums

### 1️⃣ **VaiTro** - Vai Trò Người Dùng
**File:** `VaiTroEnum.cs`

```csharp
internal enum VaiTro
{
    Admin = 1,    // Admin — Quản trị viên hệ thống
    BacSi = 2,    // BacSi — Bác sĩ (có quyền khám bệnh)
    LeTan = 3     // LeTan — Lễ tân (tiếp đón bệnh nhân)
}
```

**Cách sử dụng:**
```csharp
using DermaSoft.Enums;

VaiTroModel vaiTro = new VaiTroModel { 
    MaVaiTro = VaiTro.Admin, 
    TenVaiTro = "Admin" 
};
```

---

### 2️⃣ **TrangThaiPhieuKham** - Trạng Thái Phiếu Khám
**File:** `TrangThaiPhieuKhamEnum.cs`

```csharp
internal enum TrangThaiPhieuKham
{
    Moi = 0,           // Mới — Phiếu khám vừa được tạo
    DangKham = 1,      // Đang khám — Bác sĩ đang thực hiện khám
    HoanThanh = 2,     // Hoàn thành — Khám xong, có chẩn đoán
    DaThanhToan = 3,   // Đã thanh toán — Hóa đơn đã được thanh toán
    DaHuy = 4          // Đã hủy — Phiếu khám bị hủy
}
```

**Cách sử dụng:**
```csharp
PhieuKhamModel pk = new PhieuKhamModel {
    TrangThai = TrangThaiPhieuKham.HoanThanh
};

if (pk.TrangThai == TrangThaiPhieuKham.DaThanhToan) {
    // Xử lý thanh toán
}
```

---

### 3️⃣ **TrangThaiLichHen** - Trạng Thái Lịch Hẹn
**File:** `TrangThaiLichHenEnum.cs`

```csharp
internal enum TrangThaiLichHen
{
    ChoXacNhan = 0,    // Chờ xác nhận — Lịch hẹn vừa được đặt
    DaXacNhan = 1,     // Đã xác nhận — Lịch hẹn được xác nhận
    HoanThanh = 2,     // Hoàn thành — Bệnh nhân đã đến khám
    DaHuy = 3          // Đã hủy — Lịch hẹn bị hủy
}
```

---

### 4️⃣ **TrangThaiTaiKhoan** - Trạng Thái Tài Khoản
**File:** `TrangThaiTaiKhoanEnum.cs`

```csharp
internal enum TrangThaiTaiKhoan
{
    Khoa = 0,       // Khóa — Tài khoản bị khóa, không thể đăng nhập
    HoatDong = 1    // Hoạt động — Tài khoản đang hoạt động bình thường
}
```

---

### 5️⃣ **TrangThaiOTP** - Trạng Thái OTP
**File:** `TrangThaiOTPEnum.cs`

```csharp
internal enum TrangThaiOTP
{
    ChuaXacThuc = 0,   // Chưa xác thực — OTP vừa được gửi, chưa xác thực
    DaXacThuc = 1,     // Đã xác thực — OTP đã được xác thực thành công
    HetHan = 2         // Hết hạn — OTP đã hết hạn sử dụng
}
```

---

### 6️⃣ **DiemDanhGia** - Điểm Đánh Giá
**File:** `DiemDanhGiaEnum.cs`

```csharp
internal enum DiemDanhGia
{
    RatTe = 1,        // Rất tệ — 1 sao
    Te = 2,           // Tệ — 2 sao
    BinhThuong = 3,   // Bình thường — 3 sao
    Tot = 4,          // Tốt — 4 sao
    RatTot = 5        // Rất tốt — 5 sao
}
```

---

### 7️⃣ **LoaiThongBao** - Loại Thông Báo
**File:** `LoaiThongBaoEnum.cs`

```csharp
internal enum LoaiThongBao
{
    Email = 1,          // Email — Gửi thông báo qua email
    SMS = 2,            // SMS — Gửi thông báo qua SMS/SMS Twilio
    TrongUngDung = 3    // Trong ứng dụng — Thông báo hiển thị trong app
}
```

---

## ✅ Kiểm Tra Đồng Bộ SQL

Tất cả các enum được đồng bộ với constraints trong SQL Server:

| Enum | SQL Constraint | File |
|------|---|---|
| VaiTro | Bảng `VaiTro` (khởi tạo) | 01_Tables_Data.sql |
| TrangThaiPhieuKham | `CHK_TrangThaiPhieuKham` (0-4) | 05_Constraints.sql |
| TrangThaiLichHen | `CHK_TrangThaiLich` (0-3) | 05_Constraints.sql |
| TrangThaiTaiKhoan | `CHK_TrangThaiTK` (0-1) | 05_Constraints.sql |
| TrangThaiOTP | Bảng `XacThucOTP` | 01_Tables_Data.sql |
| DiemDanhGia | `CHK_DiemDanh` (1-5) | 05_Constraints.sql |
| LoaiThongBao | `CHK_LoaiThongBao` (1-3) | 05_Constraints.sql |

---

## 🔄 Cách Import

```csharp
// Thêm vào đầu file
using DermaSoft.Enums;

// Sử dụng bất kỳ enum nào
var vaiTro = VaiTro.Admin;
var trangThaiPhieuKham = TrangThaiPhieuKham.HoanThanh;
var trangThaiLichHen = TrangThaiLichHen.DaXacNhan;
```

---

## 💡 Best Practices

### ✅ Nên làm
```csharp
// Sử dụng enum khi so sánh
if (phieuKham.TrangThai == TrangThaiPhieuKham.DaThanhToan) {
    // Xử lý thanh toán
}

// Cast sang int khi cần lưu vào database
int statusId = (int)phieuKham.TrangThai;
```

### ❌ Không nên làm
```csharp
// Không sử dụng magic numbers
if (phieuKham.TrangThai == 3) {  // ❌ Không rõ ràng
    // ...
}

// Không sử dụng string trừ khi cần hiển thị
string status = "DaThanhToan";  // ❌ Dễ lỗi typo
```

---

## 🛠️ Khi Nào Cập Nhật Enums

1. **Khi thêm trạng thái mới**: Cập nhật enum và SQL constraint
2. **Khi xóa trạng thái**: Cất giữ enum (không xóa để tương thích ngược)
3. **Khi đổi tên**: Cập nhật cả enum và tên cột trong SQL

---

## 📝 Ví Dụ Hoàn Chỉnh

```csharp
using System;
using DermaSoft.Enums;
using DermaSoft.Models;

namespace DermaSoft
{
    class Program
    {
        static void Main()
        {
            // Tạo phiếu khám mới
            var phieuKham = new PhieuKhamModel
            {
                MaBenhNhan = 1,
                NgayKham = DateTime.Now,
                TrangThai = TrangThaiPhieuKham.Moi,  // ✅ Sử dụng enum
                TrieuChung = "Mụn, da dầu"
            };

            // Cập nhật trạng thái
            phieuKham.TrangThai = TrangThaiPhieuKham.DangKham;

            // Kiểm tra trạng thái
            if (phieuKham.TrangThai == TrangThaiPhieuKham.DangKham)
            {
                Console.WriteLine("Bác sĩ đang thực hiện khám");
            }

            // Hoàn thành khám
            phieuKham.TrangThai = TrangThaiPhieuKham.HoanThanh;
            phieuKham.ChanDoan = "Mụn trứng cá";
        }
    }
}
```

---

## 🎯 Lợi Ích

✅ **Type-safe**: Không thể nhập giá trị sai  
✅ **IntelliSense**: IDE tự động gợi ý  
✅ **Dễ bảo trì**: Rõ ràng ý nghĩa của từng trạng thái  
✅ **Đồng bộ SQL**: Luôn kết hợp với database constraints  
✅ **Dễ kiểm thử**: Mock enums trong unit tests  

---

**Cập nhật lần cuối:** 2026-03-23  
**Phiên bản:** 1.0
