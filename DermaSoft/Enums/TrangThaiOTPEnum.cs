namespace DermaSoft.Enums
{
    /// <summary>
    /// Enum trạng thái XÁC THỰC OTP — khớp với bảng <c>XacThucOTP</c> trong SQL.
    /// </summary>
    internal enum TrangThaiOTP
    {
        /// <summary>Chưa xác thực — OTP vừa được gửi, chưa xác thực</summary>
        ChuaXacThuc = 0,

        /// <summary>Đã xác thực — OTP đã được xác thực thành công</summary>
        DaXacThuc = 1,

        /// <summary>Hết hạn — OTP đã hết hạn sử dụng</summary>
        HetHan = 2
    }
}
