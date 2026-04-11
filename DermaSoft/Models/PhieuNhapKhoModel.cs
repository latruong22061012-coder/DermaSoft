using System;

namespace DermaSoft.Models
{
    /// <summary>
    /// Mô hình dữ liệu phiếu nhập kho — đồng bộ với bảng <c>PhieuNhapKho</c> trong SQL.
    /// </summary>
    internal class PhieuNhapKhoModel
    {
        public int      MaPhieuNhap  { get; set; }  // MaPhieuNhap  — Khóa chính
        public int      MaNhaCungCap { get; set; }  // MaNhaCungCap — Nhà cung cấp
        public int      MaNguoiDung  { get; set; }  // MaNguoiDung  — Người nhập
        public DateTime NgayNhap     { get; set; }  // NgayNhap     — Ngày nhập kho
        public decimal  TongGiaTri   { get; set; }  // TongGiaTri   — Tổng giá trị phiếu nhập

        // Cột JOIN từ bảng liên kết (dùng cho hiển thị UI)
        public string   TenNhaCungCap { get; set; } // Tên nhà cung cấp (JOIN NhaCungCap)
        public string   TenNguoiNhap  { get; set; } // Tên người nhập   (JOIN NguoiDung)
    }
}
