using System;
using System.Collections.Generic;
using DermaSoft.Models;
using DermaSoft.Enums;

namespace DermaSoft.Services
{
    /// <summary>
    /// Cung cấp dữ liệu mẫu cho toàn bộ ứng dụng.
    /// Tên trường được đặt khớp với tên cột trong SQL (tiếng Việt).
    /// Sau này thay thế bằng kết nối SQL Server thực.
    /// </summary>
    internal static class DataService
    {
        // ── Vai trò ──────────────────────────────────────────────
        public static List<VaiTroModel> GetVaiTro()
        {
            return new List<VaiTroModel>
            {
                new VaiTroModel { MaVaiTro = VaiTro.Admin, TenVaiTro = "Admin"   },
                new VaiTroModel { MaVaiTro = VaiTro.BacSi, TenVaiTro = "Bác Sĩ"  },
                new VaiTroModel { MaVaiTro = VaiTro.LeTan, TenVaiTro = "Lễ Tân"  },
            };
        }

        // ── Bệnh nhân ─────────────────────────────────────────────
        public static List<PatientModel> GetBenhNhan()
        {
            return new List<PatientModel>
            {
                new PatientModel { MaBenhNhan=1, HoTen="Nguyễn Thị Lan",  NgaySinh=new DateTime(1990,3,15),  GioiTinh=false, SoDienThoai="0901234567", TienSuBenhLy="Da dầu, dễ bị mụn",   IsDeleted=false },
                new PatientModel { MaBenhNhan=2, HoTen="Trần Văn Minh",   NgaySinh=new DateTime(1985,7,22),  GioiTinh=true,  SoDienThoai="0912345678", TienSuBenhLy="Da thường",             IsDeleted=false },
                new PatientModel { MaBenhNhan=3, HoTen="Lê Thị Hoa",      NgaySinh=new DateTime(1995,11,8),  GioiTinh=false, SoDienThoai="0923456789", TienSuBenhLy="Da khô, nám",           IsDeleted=false },
                new PatientModel { MaBenhNhan=4, HoTen="Phạm Quang Hùng", NgaySinh=new DateTime(1988,5,30),  GioiTinh=true,  SoDienThoai="0934567890", TienSuBenhLy="Da hỗn hợp",            IsDeleted=false },
                new PatientModel { MaBenhNhan=5, HoTen="Hoàng Thị Mai",   NgaySinh=new DateTime(1993,9,12),  GioiTinh=false, SoDienThoai="0945678901", TienSuBenhLy="Da nhạy cảm",           IsDeleted=false },
            };
        }

        // ── Lịch hẹn ─────────────────────────────────────────────
        public static List<AppointmentModel> GetLichHen()
        {
            return new List<AppointmentModel>
            {
                new AppointmentModel { MaLichHen=1, MaBenhNhan=1, MaNguoiDung=2,    ThoiGianHen=DateTime.Today.AddHours(9),           TrangThai=TrangThaiLichHen.DaXacNhan,  GhiChu="Hẹn khám mụn",     TenBenhNhan="Nguyễn Thị Lan",  TenNguoiDung="BS. Trần Minh", SoDienThoaiKhach=null },
                new AppointmentModel { MaLichHen=2, MaBenhNhan=3, MaNguoiDung=2,    ThoiGianHen=DateTime.Today.AddHours(10),          TrangThai=TrangThaiLichHen.DaXacNhan,  GhiChu="Điều trị nám",      TenBenhNhan="Lê Thị Hoa",      TenNguoiDung="BS. Nguyễn Hà", SoDienThoaiKhach=null },
                new AppointmentModel { MaLichHen=3, MaBenhNhan=5, MaNguoiDung=null, ThoiGianHen=DateTime.Today.AddHours(14),          TrangThai=TrangThaiLichHen.ChoXacNhan, GhiChu="Đặt qua website",   TenBenhNhan="Hoàng Thị Mai",   TenNguoiDung=null,            SoDienThoaiKhach="0945678901" },
                new AppointmentModel { MaLichHen=4, MaBenhNhan=2, MaNguoiDung=2,    ThoiGianHen=DateTime.Today.AddDays(1).AddHours(9),TrangThai=TrangThaiLichHen.DaXacNhan,  GhiChu="Laser nám",         TenBenhNhan="Trần Văn Minh",   TenNguoiDung="BS. Trần Minh", SoDienThoaiKhach=null },
                new AppointmentModel { MaLichHen=5, MaBenhNhan=4, MaNguoiDung=null, ThoiGianHen=DateTime.Today.AddDays(1).AddHours(15),TrangThai=TrangThaiLichHen.ChoXacNhan,GhiChu=null,               TenBenhNhan="Phạm Quang Hùng", TenNguoiDung=null,            SoDienThoaiKhach="0934567890" },
            };
        }

        // ── Dịch vụ ───────────────────────────────────────────────
        public static List<ServiceModel> GetDichVu()
        {
            return new List<ServiceModel>
            {
                new ServiceModel { MaDichVu=1, TenDichVu="Chăm sóc da cơ bản",       DonGia=280000  },
                new ServiceModel { MaDichVu=2, TenDichVu="Trị mụn chuyên sâu",        DonGia=500000  },
                new ServiceModel { MaDichVu=3, TenDichVu="Peel da AHA/BHA",            DonGia=350000  },
                new ServiceModel { MaDichVu=4, TenDichVu="Điều trị nám laser",         DonGia=2000000 },
                new ServiceModel { MaDichVu=5, TenDichVu="Căng da mặt RF",             DonGia=1200000 },
                new ServiceModel { MaDichVu=6, TenDichVu="Tiêm filler môi",            DonGia=3500000 },
                new ServiceModel { MaDichVu=7, TenDichVu="Tiêm Botox",                 DonGia=4500000 },
                new ServiceModel { MaDichVu=8, TenDichVu="Tẩy tế bào chết toàn thân", DonGia=450000  },
            };
        }

        // ── Hạng thành viên (đồng bộ HangThanhVien + Migration) ──
        public static List<HangThanhVienModel> GetHangThanhVien()
        {
            return new List<HangThanhVienModel>
            {
                new HangThanhVienModel { MaHang=1, TenHang="Thành Viên Đỏ",        DiemToiThieu=0,    MauHangHex="#FF4C4C", PhanTramGiamDuocPham=0,  PhanTramGiamTongHD=0,  GiamGiaCodinh=100000, GhiChuKhuyenMai="Giảm 100.000đ cho hóa đơn đầu tiên" },
                new HangThanhVienModel { MaHang=2, TenHang="Thành Viên Bạc",       DiemToiThieu=300,  MauHangHex="#C0C0C0", PhanTramGiamDuocPham=5,  PhanTramGiamTongHD=0,  GiamGiaCodinh=0,      GhiChuKhuyenMai="Giảm 5% sản phẩm dược phẩm" },
                new HangThanhVienModel { MaHang=3, TenHang="Thành Viên Vàng",      DiemToiThieu=1000, MauHangHex="#FFD700", PhanTramGiamDuocPham=10, PhanTramGiamTongHD=0,  GiamGiaCodinh=0,      GhiChuKhuyenMai="Giảm 10% sản phẩm dược phẩm" },
                new HangThanhVienModel { MaHang=4, TenHang="Thành Viên Kim Cương", DiemToiThieu=5000, MauHangHex="#89CFF0", PhanTramGiamDuocPham=0,  PhanTramGiamTongHD=10, GiamGiaCodinh=0,      GhiChuKhuyenMai="Giảm 10% tổng hóa đơn" },
            };
        }

        // ── Người dùng (nhân viên/bác sĩ) ─────────────────────────
        public static List<NguoiDungModel> GetNguoiDung()
        {
            return new List<NguoiDungModel>
            {
                new NguoiDungModel { MaNguoiDung=1, HoTen="Admin DarmaSoft", SoDienThoai="0900000000", Email="admin@darmaclinic.vn",   TenDangNhap="aB1cD", MaVaiTro=(int)VaiTro.Admin, TrangThaiTK=true, TenVaiTro="Admin",  NgayTao=new DateTime(2024,1,1), IsDeleted=false },
                new NguoiDungModel { MaNguoiDung=2, HoTen="BS. Trần Minh",  SoDienThoai="0901000001", Email="tranminh@darmaclinic.vn", TenDangNhap="xK2mN", MaVaiTro=(int)VaiTro.BacSi, TrangThaiTK=true, TenVaiTro="Bác Sĩ", NgayTao=new DateTime(2024,1,5), IsDeleted=false },
                new NguoiDungModel { MaNguoiDung=3, HoTen="Lê Thị Lễ Tân", SoDienThoai="0901000002", Email="letan@darmaclinic.vn",    TenDangNhap="pQ3rS", MaVaiTro=(int)VaiTro.LeTan, TrangThaiTK=true, TenVaiTro="Lễ Tân", NgayTao=new DateTime(2024,2,1), IsDeleted=false },
            };
        }

        // ── Chỉ số tổng quan (KPI Dashboard) ──────────────────────
        public static int GetTongBenhNhan()       => 248;
        public static int GetLichHenHomNay()      => 12;
        public static decimal GetDoanhThuThang()  => 45_600_000m;
        public static int GetBenhNhanMoiThang()   => 18;

        // ── Doanh thu theo tháng (dùng cho biểu đồ) ───────────────
        public static Dictionary<string, decimal> GetDoanhThuTheoThang()
        {
            return new Dictionary<string, decimal>
            {
                { "T1",  32_000_000m }, { "T2",  28_500_000m }, { "T3",  41_200_000m },
                { "T4",  38_700_000m }, { "T5",  45_100_000m }, { "T6",  52_300_000m },
                { "T7",  48_900_000m }, { "T8",  55_600_000m }, { "T9",  43_800_000m },
                { "T10", 49_200_000m }, { "T11", 58_100_000m }, { "T12", 45_600_000m },
            };
        }

        // ── Tỉ lệ sử dụng dịch vụ (dùng cho biểu đồ tròn) ────────
        public static Dictionary<string, int> GetTiLeSuDungDichVu()
        {
            return new Dictionary<string, int>
            {
                { "Chăm sóc da", 42 }, { "Trị mụn", 28 }, { "Laser", 15 },
                { "Thẩm mỹ",     10 }, { "Công nghệ", 5 },
            };
        }
    }
}
