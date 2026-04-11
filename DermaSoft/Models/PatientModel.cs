using System;

namespace DermaSoft.Models
{
    /// <summary>
    /// Mô hình dữ liệu bệnh nhân — đồng bộ với bảng <c>BenhNhan</c> trong SQL.
    /// </summary>
    internal class PatientModel
    {
        public int      MaBenhNhan    { get; set; }  // MaBenhNhan   — Khóa chính
        public string   HoTen         { get; set; }  // HoTen        — Họ và tên
        public DateTime? NgaySinh     { get; set; }  // NgaySinh     — Ngày sinh (có thể NULL)
        public bool?    GioiTinh      { get; set; }  // GioiTinh     — BIT: true=Nam, false=Nữ
        public string   SoDienThoai   { get; set; }  // SoDienThoai  — Số điện thoại (UNIQUE)
        public string   TienSuBenhLy  { get; set; }  // TienSuBenhLy — Tiền sử bệnh lý
        public bool     IsDeleted     { get; set; }  // IsDeleted    — Xóa mềm

        /// <summary>Tuổi tính từ NgàySinh. Trả về null nếu không có ngày sinh.</summary>
        public int? Tuoi
        {
            get
            {
                if (NgaySinh == null) return null;
                var today = DateTime.Today;
                int tuoi = today.Year - NgaySinh.Value.Year;
                if (NgaySinh.Value.Date > today.AddYears(-tuoi)) tuoi--;
                return tuoi;
            }
        }

        /// <summary>Hiển thị giới tính dạng chuỗi tiếng Việt.</summary>
        public string GioiTinhText => GioiTinh == true ? "Nam" : GioiTinh == false ? "Nữ" : "Không rõ";
    }
}
