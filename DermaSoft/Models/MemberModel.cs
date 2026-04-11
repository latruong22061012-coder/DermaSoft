using System;

namespace DermaSoft.Models
{
    /// <summary>
    /// Mô hình dữ liệu hạng thành viên — đồng bộ với bảng <c>HangThanhVien</c> trong SQL.
    /// 4 hạng theo migration: Đỏ (0đ) / Bạc (300đ) / Vàng (1000đ) / Kim Cương (5000đ).
    /// </summary>
    internal class HangThanhVienModel
    {
        public int      MaHang               { get; set; }  // MaHang               — Khóa chính
        public string   TenHang              { get; set; }  // TenHang              — Tên hạng
        public int      DiemToiThieu         { get; set; }  // DiemToiThieu         — Điểm tối thiểu để lên hạng
        public string   MauHangHex           { get; set; }  // MauHangHex           — Mã màu hiển thị (#RRGGBB)
        public string   GhiChu               { get; set; }  // GhiChu               — Ghi chú
        public decimal  PhanTramGiamDuocPham { get; set; }  // PhanTramGiamDuocPham — % giảm giá dược phẩm
        public decimal  PhanTramGiamTongHD   { get; set; }  // PhanTramGiamTongHD   — % giảm giá tổng hóa đơn
        public int      GiamGiaCodinh        { get; set; }  // GiamGiaCodinh        — Giảm giá cố định (VNĐ)
        public string   GhiChuKhuyenMai      { get; set; }  // GhiChuKhuyenMai      — Mô tả ưu đãi
    }

    /// <summary>
    /// Mô hình dữ liệu thẻ thành viên — đồng bộ với bảng <c>ThanhVienInfo</c> trong SQL.
    /// </summary>
    internal class MemberModel
    {
        public int      MaThanhVien     { get; set; }  // MaThanhVien     — Khóa chính
        public int      MaBenhNhan      { get; set; }  // MaBenhNhan      — Liên kết bệnh nhân (UNIQUE)
        public int      MaHang          { get; set; }  // MaHang          — Hạng thành viên (mặc định 1=Đỏ)
        public int      DiemTichLuy     { get; set; }  // DiemTichLuy     — Điểm tích lũy
        public int      SoLanKham       { get; set; }  // SoLanKham       — Tổng số lần khám
        public string   DuongDanAvatar  { get; set; }  // DuongDanAvatar  — Đường dẫn ảnh đại diện
        public decimal  TyLeHaiLong     { get; set; }  // TyLeHaiLong     — Tỷ lệ hài lòng (%)
        public DateTime NgayTaoTaiKhoan { get; set; }  // NgayTaoTaiKhoan — Ngày đăng ký thành viên

        // Cột JOIN từ bảng liên kết (dùng cho hiển thị UI)
        public string   TenBenhNhan     { get; set; }  // Tên bệnh nhân  (JOIN BenhNhan)
        public string   TenHang         { get; set; }  // Tên hạng       (JOIN HangThanhVien)
        public string   MauHangHex      { get; set; }  // Màu hạng       (JOIN HangThanhVien)
        public string SoDienThoai { get; set; }  // SĐT bệnh nhân (JOIN BenhNhan)
        public string MaBenhNhanCode { get; set; }  // "BN001" format để hiển thị UI
    }
}
