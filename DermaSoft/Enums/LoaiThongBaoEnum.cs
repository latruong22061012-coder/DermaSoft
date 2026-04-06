namespace DermaSoft.Enums
{
    /// <summary>
    /// Enum loại thông báo lịch hẹn — khớp với ràng buộc <c>CHK_LoaiThongBao</c> trong SQL (1–3).
    /// Dùng cho bảng <c>LichHen_Notification</c>.
    /// </summary>
    internal enum LoaiThongBao
    {
        /// <summary>Email — Gửi thông báo qua email</summary>
        Email = 1,

        /// <summary>SMS — Gửi thông báo qua SMS/SMS Twilio</summary>
        SMS = 2,

        /// <summary>Trong ứng dụng — Thông báo hiển thị trong app</summary>
        TrongUngDung = 3
    }
}
