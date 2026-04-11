using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DermaSoft.Data
{
    /// <summary>
    /// Quản lý kết nối SQL Server DERMASOFT.
    /// Dùng pattern Singleton — toàn ứng dụng dùng chung 1 connection string.
    /// </summary>
    internal static class DatabaseConnection
    {
        // ── Connection string đọc từ App.config (connectionStrings section) ──
        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                    _connectionString = System.Configuration.ConfigurationManager
                        .ConnectionStrings["DERMASOFT"]?.ConnectionString
                        ?? DefaultConnectionString;
                return _connectionString;
            }
        }

        /// <summary>Giá trị mặc định nếu App.config chưa có entry.</summary>
        private const string DefaultConnectionString =
            @"Server=localhost;Database=DERMASOFT;User Id=sa;Password=DarmaSoft2026;" +
            @"TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;";

        // ──────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Mở và trả về SqlConnection mới. Caller chịu trách nhiệm đóng/dispose.
        /// </summary>
        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// Kiểm tra kết nối có hoạt động không.
        /// Trả về true nếu ping SQL Server thành công.
        /// </summary>
        public static bool TestConnection(out string errorMessage)
        {
            errorMessage = null;
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT 1", conn))
                        cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        // ──────────────────────────────────────────────────────────────────────
        // HELPER METHODS — ExecuteQuery, ExecuteNonQuery, ExecuteScalar
        // ──────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Thực thi câu SELECT và trả về DataTable.
        /// </summary>
        public static DataTable ExecuteQuery(string sql, Action<SqlParameterCollection> addParams = null)
        {
            var dt = new DataTable();
            try
            {
                using (var conn = GetConnection())
                using (var cmd = new SqlCommand(sql, conn))
                {
                    addParams?.Invoke(cmd.Parameters);
                    using (var adapter = new SqlDataAdapter(cmd))
                        adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                HandleError("ExecuteQuery", ex);
            }
            return dt;
        }

        /// <summary>
        /// Thực thi câu INSERT/UPDATE/DELETE và trả về số hàng bị ảnh hưởng.
        /// </summary>
        public static int ExecuteNonQuery(string sql, Action<SqlParameterCollection> addParams = null)
        {
            try
            {
                using (var conn = GetConnection())
                using (var cmd = new SqlCommand(sql, conn))
                {
                    addParams?.Invoke(cmd.Parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                HandleError("ExecuteNonQuery", ex);
                return -1;
            }
        }

        /// <summary>
        /// Thực thi câu SQL trả về 1 giá trị đơn (vd: COUNT, SCOPE_IDENTITY).
        /// </summary>
        public static object ExecuteScalar(string sql, Action<SqlParameterCollection> addParams = null)
        {
            try
            {
                using (var conn = GetConnection())
                using (var cmd = new SqlCommand(sql, conn))
                {
                    addParams?.Invoke(cmd.Parameters);
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                HandleError("ExecuteScalar", ex);
                return null;
            }
        }

        /// <summary>
        /// Thực thi Stored Procedure và trả về DataTable.
        /// </summary>
        public static DataTable ExecuteStoredProcedure(string spName, Action<SqlParameterCollection> addParams = null)
        {
            var dt = new DataTable();
            try
            {
                using (var conn = GetConnection())
                using (var cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    addParams?.Invoke(cmd.Parameters);
                    using (var adapter = new SqlDataAdapter(cmd))
                        adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                HandleError("ExecuteStoredProcedure: " + spName, ex);
            }
            return dt;
        }

        /// <summary>
        /// Thực thi nhiều câu SQL trong cùng 1 Transaction.
        /// action nhận SqlConnection + SqlTransaction để thực thi các lệnh.
        /// Trả về true nếu thành công, false nếu bị rollback.
        /// </summary>
        public static bool ExecuteTransaction(Action<SqlConnection, SqlTransaction> action)
        {
            using (var conn = GetConnection())
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    action(conn, tran);
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    HandleError("ExecuteTransaction", ex);
                    return false;
                }
            }
        }

        // ──────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Ghi log lỗi và hiện thông báo cho người dùng.
        /// </summary>
        private static void HandleError(string context, Exception ex)
        {
            string msg = $"[DB Error - {context}]\n{ex.Message}";
            System.Diagnostics.Debug.WriteLine(msg);

            // Chỉ hiện MessageBox khi chạy trên UI thread
            if (Application.OpenForms.Count > 0)
            {
                MessageBox.Show(
                    $"Lỗi kết nối cơ sở dữ liệu:\n{ex.Message}",
                    "Lỗi Database",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
