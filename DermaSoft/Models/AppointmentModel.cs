using System;
using DermaSoft.Enums;

namespace DermaSoft.Models
{
    /// <summary>
    /// Mô hình dữ liệu lịch hẹn — đồng bộ với bảng <c>LichHen</c> trong SQL.
    /// </summary>
    internal class AppointmentModel
    {
        public int              MaLichHen        { get; set; }  // MaLichHen        — Khóa chính
        public int              MaBenhNhan        { get; set; }  // MaBenhNhan        — Liên kết bệnh nhân
        public int?             MaNguoiDung       { get; set; }  // MaNguoiDung       — NULL nếu chưa phân công
        public DateTime         ThoiGianHen       { get; set; }  // ThoiGianHen       — Thời gian hẹn
        public TrangThaiLichHen TrangThai         { get; set; }  // TrangThai         — 0=Chờ, 1=Xác nhận, 2=Hoàn thành, 3=Hủy
        public string           GhiChu            { get; set; }  // GhiChu            — Ghi chú
        public string           SoDienThoaiKhach  { get; set; }  // SoDienThoaiKhach  — SĐT khi đặt qua website

        // Cột JOIN từ bảng liên kết (dùng cho hiển thị UI)
        public string           TenBenhNhan       { get; set; }  // Tên bệnh nhân (JOIN BenhNhan)
        public string           TenNguoiDung      { get; set; }  // Tên nhân viên phân công (JOIN NguoiDung)
    }
}
