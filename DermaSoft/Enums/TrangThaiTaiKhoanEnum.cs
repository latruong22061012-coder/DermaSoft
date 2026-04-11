namespace DermaSoft.Enums
{
    /// <summary>
    /// Enum trạng thái tài khoản người dùng — khớp với ràng buộc <c>CHK_TrangThaiTK</c> trong SQL.
    /// </summary>
    internal enum TrangThaiTaiKhoan
    {
        /// <summary>Khóa — Tài khoản bị khóa, không thể đăng nhập</summary>
        Khoa = 0,

        /// <summary>Hoạt động — Tài khoản đang hoạt động bình thường</summary>
        HoatDong = 1
    }
}
