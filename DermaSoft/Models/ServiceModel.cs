namespace DermaSoft.Models
{
    /// <summary>
    /// Mô hình dữ liệu dịch vụ — đồng bộ với bảng <c>DichVu</c> trong SQL.
    /// </summary>
    internal class ServiceModel
    {
        public int      MaDichVu   { get; set; }  // MaDichVu   — Khóa chính
        public string   TenDichVu  { get; set; }  // TenDichVu  — Tên dịch vụ
        public decimal  DonGia     { get; set; }  // DonGia     — Đơn giá (VNĐ), mặc định 0
    }
}
