namespace DermaSoft.Enums
{
    /// <summary>
    /// Enum trạng thái lịch hẹn — khớp với ràng buộc <c>CHK_TrangThaiLich</c> trong SQL (0–3).
    /// </summary>
    internal enum TrangThaiLichHen
    {
        /// <summary>Chờ xác nhận — Lịch hẹn vừa được đặt</summary>
        ChoXacNhan = 0,

        /// <summary>Đã xác nhận — Lịch hẹn được xác nhận</summary>
        DaXacNhan = 1,

        /// <summary>Hoàn thành — Bệnh nhân đã đến khám</summary>
        HoanThanh = 2,

        /// <summary>Đã hủy — Lịch hẹn bị hủy</summary>
        DaHuy = 3
    }
}
