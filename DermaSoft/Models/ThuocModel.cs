using System;

namespace DermaSoft.Models
{
    /// <summary>
    /// Mô hình dữ liệu thuốc/mỹ phẩm — đồng bộ với bảng <c>Thuoc</c> trong SQL.
    /// </summary>
    internal class ThuocModel
    {
        public int      MaThuoc    { get; set; }  // MaThuoc    — Khóa chính
        public string   TenThuoc   { get; set; }  // TenThuoc   — Tên thuốc/mỹ phẩm
        public string   DonViTinh  { get; set; }  // DonViTinh  — Đơn vị tính (viên, hộp, chai...)
        public decimal  DonGia     { get; set; }  // DonGia     — Đơn giá (VNĐ)
        public int      SoLuongTon { get; set; }  // SoLuongTon — Số lượng tồn kho
    }
}
