using System;
using System.Collections.Generic;

namespace DermaSoft.Models
{
    /// <summary>
    /// Trạng thái hóa đơn — khớp với cột <c>TrangThai BIT</c> trong bảng HoaDon SQL.
    /// </summary>
    internal enum TrangThaiHoaDon
    {
        ChuaThanhToan = 0,  // Chưa thanh toán
        DaThanhToan   = 1   // Đã thanh toán
    }

    /// <summary>
    /// Chi tiết từng dịch vụ trong phiếu khám — đồng bộ với bảng <c>ChiTietDichVu</c> trong SQL.
    /// </summary>
    internal class ChiTietDichVuModel
    {
        public int      MaPhieuKham { get; set; }  // MaPhieuKham — Khóa ngoại
        public int      MaDichVu    { get; set; }  // MaDichVu    — Khóa ngoại
        public string   TenDichVu   { get; set; }  // Tên dịch vụ  — JOIN từ bảng DichVu
        public int      SoLuong     { get; set; }  // SoLuong     — Mặc định 1
        public decimal  ThanhTien   { get; set; }  // ThanhTien   — Thành tiền
    }

    /// <summary>
    /// Mô hình dữ liệu hóa đơn — đồng bộ với bảng <c>HoaDon</c> trong SQL.
    /// </summary>
    internal class InvoiceModel
    {
        public int              MaHoaDon            { get; set; }  // MaHoaDon            — Khóa chính
        public int              MaPhieuKham          { get; set; }  // MaPhieuKham          — Liên kết phiếu khám (UNIQUE)
        public decimal          TongTien             { get; set; }  // TongTien             — Tổng tiền
        public decimal          TongTienDichVu       { get; set; }  // TongTienDichVu       — Tiền dịch vụ
        public decimal          TongThuoc            { get; set; }  // TongThuoc            — Tiền thuốc
        public decimal          GiamGia              { get; set; }  // GiamGia              — Số tiền giảm giá
        public decimal          TienKhachTra         { get; set; }  // TienKhachTra         — Tiền khách đưa
        public decimal          TienThua             { get; set; }  // TienThua             — Tiền thẺ
        public string           PhuongThucThanhToan  { get; set; }  // PhuongThucThanhToan  — Hình thức thanh toán
        public DateTime?        NgayThanhToan        { get; set; }  // NgayThanhToan        — Ngày thanh toán (có thể NULL)
        public DateTime         NgayTao              { get; set; }  // NgayTao              — Ngày tạo
        public TrangThaiHoaDon  TrangThai            { get; set; }  // TrangThai            — 0=Chưa, 1=Đã thanh toán
        public bool             IsDeleted            { get; set; }  // IsDeleted            — Xóa mềm

        // Cột JOIN từ bảng liên kết (dùng cho hiển thị UI)
        public string           TenBenhNhan          { get; set; }  // Tên bệnh nhân (JOIN qua PhieuKham)
        public List<ChiTietDichVuModel> DanhSachDichVu { get; set; } = new List<ChiTietDichVuModel>();
    }
}
