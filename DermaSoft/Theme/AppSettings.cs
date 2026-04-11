using System;
using System.Data.SqlClient;
using DermaSoft.Data;

namespace DermaSoft.Theme
{
    /// <summary>
    /// Lưu trữ cài đặt hệ thống — đọc/ghi bảng CaiDatHeThong (key-value).
    /// SettingsForm ghi vào, các form khác đọc từ đây.
    /// </summary>
    internal static class AppSettings
    {
        // ── Ngưỡng cảnh báo kho ──
        public static int NguongThap = 10;
        public static int NguongNguyHiem = 3;

        // ── Mật khẩu mặc định nhân viên ──
        public static string MatKhauMacDinh = "Temp@2026";

        /// <summary>
        /// Load tất cả cài đặt từ bảng CaiDatHeThong.
        /// Gọi 1 lần khi app khởi động.
        /// </summary>
        public static void Load()
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand("SELECT Khoa, GiaTri FROM CaiDatHeThong", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string key = reader.GetString(0);
                        string val = reader.GetString(1);
                        int num;

                        if (key == "NGUONG_THAP" && int.TryParse(val, out num)) NguongThap = num;
                        else if (key == "NGUONG_NGUY_HIEM" && int.TryParse(val, out num)) NguongNguyHiem = num;
                        else if (key == "MK_MAC_DINH" && val.Length > 0) MatKhauMacDinh = val;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Lưu 1 key vào bảng CaiDatHeThong (MERGE upsert).
        /// </summary>
        public static void Set(string khoa, string giaTri)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    @"IF EXISTS (SELECT 1 FROM CaiDatHeThong WHERE Khoa=@K)
                        UPDATE CaiDatHeThong SET GiaTri=@V WHERE Khoa=@K
                      ELSE
                        INSERT INTO CaiDatHeThong (Khoa, GiaTri) VALUES (@K, @V)", conn))
                {
                    cmd.Parameters.AddWithValue("@K", khoa);
                    cmd.Parameters.AddWithValue("@V", giaTri);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        /// <summary>
        /// Lưu tất cả cài đặt hiện tại vào DB.
        /// </summary>
        public static void Save()
        {
            Set("NGUONG_THAP", NguongThap.ToString());
            Set("NGUONG_NGUY_HIEM", NguongNguyHiem.ToString());
            Set("MK_MAC_DINH", MatKhauMacDinh);
        }
    }
}
