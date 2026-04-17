using System;
using System.Collections.Generic;
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
        // ── Ngưỡng cảnh báo kho mặc định (fallback) ──
        public static int NguongThap = 10;
        public static int NguongNguyHiem = 3;

        // ── Ngưỡng theo đơn vị tính ──
        // Key = DonViTinh (e.g. "Viên"), Value = (NguongThap, NguongNguyHiem)
        public static Dictionary<string, int[]> NguongTheoDonVi = new Dictionary<string, int[]>();

        // ── Danh sách đơn vị tính mặc định ──
        public static readonly string[] DonViTinhMacDinh = { "Viên", "Chai", "Tuýp", "Lọ", "Hộp", "Ống", "Gói" };
        public static readonly Dictionary<string, int[]> NguongMacDinhTheoDonVi = new Dictionary<string, int[]>
        {
            { "Viên", new[] { 50, 10 } },
            { "Chai", new[] { 10, 3 } },
            { "Tuýp", new[] { 10, 3 } },
            { "Lọ",   new[] { 10, 3 } },
            { "Hộp",  new[] { 15, 5 } },
            { "Ống",  new[] { 20, 5 } },
            { "Gói",  new[] { 20, 5 } },
        };

        // ── Mật khẩu mặc định nhân viên ──
        public static string MatKhauMacDinh = "Temp@2026";

        /// <summary>
        /// Lấy ngưỡng theo đơn vị tính. Trả về [NguongThap, NguongNguyHiem].
        /// Ưu tiên: NguongTheoDonVi → NguongMacDinhTheoDonVi → fallback mặc định.
        /// </summary>
        public static int[] LayNguong(string donViTinh)
        {
            if (!string.IsNullOrEmpty(donViTinh))
            {
                if (NguongTheoDonVi.ContainsKey(donViTinh))
                    return NguongTheoDonVi[donViTinh];
                if (NguongMacDinhTheoDonVi.ContainsKey(donViTinh))
                    return NguongMacDinhTheoDonVi[donViTinh];
            }
            return new[] { NguongThap, NguongNguyHiem };
        }

        /// <summary>
        /// Load tất cả cài đặt từ bảng CaiDatHeThong.
        /// Gọi 1 lần khi app khởi động.
        /// </summary>
        public static void Load()
        {
            try
            {
                NguongTheoDonVi.Clear();

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
                        else if (key.StartsWith("NGUONG_DVT_"))
                        {
                            // Format: NGUONG_DVT_Viên = "50,10"
                            string dvt = key.Substring("NGUONG_DVT_".Length);
                            string[] parts = val.Split(',');
                            if (parts.Length == 2)
                            {
                                int thap, nguy;
                                if (int.TryParse(parts[0], out thap) && int.TryParse(parts[1], out nguy))
                                    NguongTheoDonVi[dvt] = new[] { thap, nguy };
                            }
                        }
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

        /// <summary>
        /// Lưu ngưỡng cho 1 đơn vị tính.
        /// </summary>
        public static void LuuNguongDonVi(string donViTinh, int nguongThap, int nguongNguyHiem)
        {
            NguongTheoDonVi[donViTinh] = new[] { nguongThap, nguongNguyHiem };
            Set("NGUONG_DVT_" + donViTinh, nguongThap + "," + nguongNguyHiem);
        }

        /// <summary>
        /// Lấy tất cả đơn vị tính đang được cấu hình (từ DB + mặc định + DB thuốc thực tế).
        /// </summary>
        public static List<string> LayTatCaDonViTinh()
        {
            var result = new List<string>();

            // 1. Thêm mặc định
            foreach (string dvt in DonViTinhMacDinh)
                if (!result.Contains(dvt)) result.Add(dvt);

            // 2. Thêm từ DB cấu hình
            foreach (string dvt in NguongTheoDonVi.Keys)
                if (!result.Contains(dvt)) result.Add(dvt);

            // 3. Thêm từ bảng Thuoc thực tế
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand("SELECT DISTINCT DonViTinh FROM Thuoc WHERE IsDeleted = 0", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string dvt = reader.GetString(0);
                        if (!string.IsNullOrEmpty(dvt) && !result.Contains(dvt))
                            result.Add(dvt);
                    }
                }
            }
            catch { }

            return result;
        }
    }
}
