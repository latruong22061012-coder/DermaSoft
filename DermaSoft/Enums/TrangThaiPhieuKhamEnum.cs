namespace DermaSoft.Enums
{
    /// <summary>
    /// Enum trạng thái phiếu khám — khớp với ràng buộc <c>CHK_TrangThaiPhieuKham</c> trong SQL (0–4).
    /// </summary>
    internal enum TrangThaiPhieuKham
    {
        /// <summary>Mới — Phiếu khám vừa được tạo</summary>
        Moi = 0,

        /// <summary>Đang khám — Bác sĩ đang thực hiện khám</summary>
        DangKham = 1,

        /// <summary>Hoàn thành — Khám xong, có chẩn đoán</summary>
        HoanThanh = 2,

        /// <summary>Đã thanh toán — Hóa đơn đã được thanh toán</summary>
        DaThanhToan = 3,

        /// <summary>Đã hủy — Phiếu khám bị hủy</summary>
        DaHuy = 4
    }
}
