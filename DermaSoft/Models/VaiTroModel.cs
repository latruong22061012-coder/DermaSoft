using DermaSoft.Enums;

namespace DermaSoft.Models
{
    /// <summary>
    /// Mô hình dữ liệu vai trò — đồng bộ với bảng <c>VaiTro</c> trong SQL.
    /// Dữ liệu khởi tạo: 1=Admin, 2=Bác Sĩ, 3=Lễ Tân, 4=Bệnh Nhân, 5=Quản Kho.
    /// </summary>
    internal class VaiTroModel
    {
        public VaiTro  MaVaiTro  { get; set; }  // MaVaiTro  — Khóa chính
        public string  TenVaiTro { get; set; }  // TenVaiTro — Tên vai trò

        /// <summary>
        /// Tạo VaiTroModel từ mã vai trò và tên
        /// </summary>
        public VaiTroModel(VaiTro maVaiTro, string tenVaiTro)
        {
            MaVaiTro = maVaiTro;
            TenVaiTro = tenVaiTro;
        }

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public VaiTroModel()
        {
        }
    }
}
