using System;
using DermaSoft.Enums;

namespace DermaSoft.Models
{
    /// <summary>
    /// Mô hình dữ liệu phiếu khám — đồng bộ với bảng <c>PhieuKham</c> trong SQL.
    /// </summary>
    internal class PhieuKhamModel
    {
        public int               MaPhieuKham  { get; set; }  // MaPhieuKham  — Khóa chính
        public int               MaBenhNhan   { get; set; }  // MaBenhNhan   — Liên kết bệnh nhân
        public int               MaNguoiDung  { get; set; }  // MaNguoiDung  — Bác sĩ/nhân viên thực hiện
        public int?              MaLichHen    { get; set; }  // MaLichHen    — Lịch hẹn liên quan (có thể NULL)
        public DateTime          NgayKham     { get; set; }  // NgayKham     — Ngày khám
        public string            TrieuChung   { get; set; }  // TrieuChung   — Triệu chứng
        public string            ChanDoan     { get; set; }  // ChanDoan     — Chẩn đoán
        public DateTime?         NgayTaiKham  { get; set; }  // NgayTaiKham  — Ngày tái khám (có thể NULL)
        public TrangThaiPhieuKham TrangThai   { get; set; }  // TrangThai    — 0=Mới, 1=Đang, 2=Xong, 3=TT, 4=Hủy
        public string            GhiChu       { get; set; }  // GhiChu       — Ghi chú
        public bool              IsDeleted    { get; set; }  // IsDeleted    — Xóa mềm

        // Cột JOIN từ bảng liên kết (dùng cho hiển thị UI)
        public string            TenBenhNhan  { get; set; }  // Tên bệnh nhân (JOIN BenhNhan)
        public string            TenNguoiDung { get; set; }  // Tên bác sĩ    (JOIN NguoiDung)
    }
}
