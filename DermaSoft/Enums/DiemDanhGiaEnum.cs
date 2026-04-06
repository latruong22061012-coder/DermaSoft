namespace DermaSoft.Enums
{
    /// <summary>
    /// Enum điểm đánh giá — khớp với ràng buộc <c>CHK_DiemDanh</c> trong SQL (1–5).
    /// Dùng cho bảng <c>DanhGia</c>.
    /// </summary>
    internal enum DiemDanhGia
    {
        /// <summary>Rất tệ — 1 sao</summary>
        RatTe = 1,

        /// <summary>Tệ — 2 sao</summary>
        Te = 2,

        /// <summary>Bình thường — 3 sao</summary>
        BinhThuong = 3,

        /// <summary>Tốt — 4 sao</summary>
        Tot = 4,

        /// <summary>Rất tốt — 5 sao</summary>
        RatTot = 5
    }
}
