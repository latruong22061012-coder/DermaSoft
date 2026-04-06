using System;

namespace DermaSoft.Models
{
    /// <summary>
    /// Mô hình dữ liệu người dùng (nhân viên/bác sĩ/admin) — đồng bộ với bảng <c>NguoiDung</c> trong SQL.
    /// </summary>
    internal class NguoiDungModel
    {
        public int      MaNguoiDung   { get; set; }  // MaNguoiDung   — Khóa chính
        public string   HoTen         { get; set; }  // HoTen         — Họ và tên
        public string   SoDienThoai   { get; set; }  // SoDienThoai   — Số điện thoại (UNIQUE)
        public string   Email         { get; set; }  // Email         — Email (UNIQUE)
        public string   TenDangNhap   { get; set; }  // TenDangNhap   — Tên đăng nhập (UNIQUE, 5 ký tự)
        public int      MaVaiTro      { get; set; }  // MaVaiTro      — 1=Admin, 2=Bác sĩ, 3=Lễ tân
        public bool     TrangThaiTK   { get; set; }  // TrangThaiTK   — true=Hoạt động, false=Khóa
        public bool     DoiMatKhau    { get; set; }  // DoiMatKhau    — Yêu cầu đổi mật khẩu lần đầu
        public DateTime NgayTao       { get; set; }  // NgayTao       — Ngày tạo tài khoản
        public string   AnhDaiDien    { get; set; }  // AnhDaiDien    — Đường dẫn ảnh đại diện
        public bool     IsDeleted     { get; set; }  // IsDeleted     — Xóa mềm

        // Cột JOIN từ bảng liên kết (dùng cho hiển thị UI)
        public string   TenVaiTro     { get; set; }  // Tên vai trò  — JOIN từ bảng VaiTro

        /// <summary>Hiển thị trạng thái dạng chuỗi tiếng Việt.</summary>
        public string TrangThaiText => TrangThaiTK ? "Hoạt động" : "Khóa";
    }
}
