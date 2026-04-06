namespace DermaSoft.Enums
{
    /// <summary>
    /// Enum vai trò người dùng — đồng bộ với bảng <c>VaiTro</c> trong SQL.
    /// Dữ liệu khởi tạo: 1=Admin, 2=Bác Sĩ, 3=Lễ Tân.
    /// </summary>
    internal enum VaiTro
    {
        /// <summary>Admin — Quản trị viên hệ thống</summary>
        Admin = 1,

        /// <summary>BacSi — Bác sĩ (có quyền khám bệnh)</summary>
        BacSi = 2,

        /// <summary>LeTan — Lễ tân (tiếp đón bệnh nhân)</summary>
        LeTan = 3
    }
}
