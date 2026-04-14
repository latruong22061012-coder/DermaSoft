using System;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Enums;
using DermaSoft.Models;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Form Đăng Nhập — frm-login trong wireframe.
    /// Xác thực tài khoản NguoiDung (TenDangNhap + MatKhau) từ SQL Server DERMASOFT.
    /// </summary>
    public partial class LoginForm : Form
    {
        // ── Trạng thái hiển thị mật khẩu ──
        private bool _showPassword = false;

        // ── Người dùng đã đăng nhập (dùng toàn ứng dụng) ──
        internal static NguoiDungModel NguoiDungHienTai { get; set; }

        public LoginForm()
        {
            InitializeComponent();
            DangKyEvents();
            KiemTraKetNoi();
        }

        // ══════════════════════════════════════════════════════════
        // KHỞI TẠO
        // ══════════════════════════════════════════════════════════

        private void DangKyEvents()
        {
            this.btnDangNhap.Click += BtnDangNhap_Click;
            this.btnShowPassword.Click += BtnShowPassword_Click;
            this.lnkQuenMatKhau.LinkClicked += LnkQuenMatKhau_LinkClicked;
            this.txtTenDangNhap.KeyDown += TxtInput_KeyDown;
            this.txtMatKhau.KeyDown += TxtInput_KeyDown;
            this.txtTenDangNhap.TextChanged += TxtInput_TextChanged;
            this.txtMatKhau.TextChanged += TxtInput_TextChanged;

            // Kéo form bằng header (vì FormBorderStyle = None)
            this.pnlHeader.MouseDown += PnlHeader_MouseDown;
            this.lblAppName.MouseDown += PnlHeader_MouseDown;
            this.lblTagline.MouseDown += PnlHeader_MouseDown;
        }

        private void KiemTraKetNoi()
        {
            if (!DatabaseConnection.TestConnection(out string err))
            {
                HienThiLoi("⚠️  Không thể kết nối cơ sở dữ liệu. Vui lòng kiểm tra lại!");
                btnDangNhap.Enabled = false;
            }
        }

        // ══════════════════════════════════════════════════════════
        // XỬ LÝ SỰ KIỆN
        // ══════════════════════════════════════════════════════════

        // Nhấn Enter trong ô nhập → Đăng nhập
        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ThucHienDangNhap();
            }
        }

        // Xóa lỗi khi người dùng bắt đầu nhập lại
        private void TxtInput_TextChanged(object sender, EventArgs e)
        {
            if (pnlError.Visible)
                AnLoi();
        }

        // Nút Đăng Nhập
        private void BtnDangNhap_Click(object sender, EventArgs e)
        {
            ThucHienDangNhap();
        }

        // Nút hiện/ẩn mật khẩu
        private void BtnShowPassword_Click(object sender, EventArgs e)
        {
            _showPassword = !_showPassword;
            txtMatKhau.UseSystemPasswordChar = !_showPassword;
            btnShowPassword.Text = _showPassword ? "🙈" : "👁";
        }

        // Link Quên mật khẩu
        private void LnkQuenMatKhau_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(
                "Vui lòng liên hệ quản trị viên để được cấp lại mật khẩu.\n\nEmail: admin@darmaclinic.vn",
                "Quên mật khẩu",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // Kéo form (vì FormBorderStyle = None)
        private void PnlHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethod.ReleaseCapture();
                NativeMethod.SendMessage(this.Handle, NativeMethod.WM_NCLBUTTONDOWN, NativeMethod.HT_CAPTION, 0);
            }
        }

        // ══════════════════════════════════════════════════════════
        // LOGIC ĐĂNG NHẬP
        // ══════════════════════════════════════════════════════════

        private void ThucHienDangNhap()
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text;

            // ── Validate đầu vào ──
            if (string.IsNullOrEmpty(tenDangNhap))
            {
                HienThiLoi("⚠️  Vui lòng nhập tên đăng nhập!");
                txtTenDangNhap.Focus();
                return;
            }
            if (string.IsNullOrEmpty(matKhau))
            {
                HienThiLoi("⚠️  Vui lòng nhập mật khẩu!");
                txtMatKhau.Focus();
                return;
            }

            // ── Hiệu ứng loading ──
            btnDangNhap.Enabled = false;
            btnDangNhap.Text = "⏳   Đang kiểm tra...";

            try
            {
                NguoiDungModel nguoiDung = XacThucTaiKhoan(tenDangNhap, matKhau);

                if (nguoiDung == null)
                {
                    // Clear trước, rồi mới hiện lỗi — tránh TextChanged gọi AnLoi() ẩn ngay panel lỗi
                    txtMatKhau.TextChanged -= TxtInput_TextChanged;
                    txtMatKhau.Clear();
                    txtMatKhau.TextChanged += TxtInput_TextChanged;
                    HienThiLoi("⚠️  Tên đăng nhập hoặc mật khẩu không đúng!");
                    txtMatKhau.Focus();
                    return;
                }

                if (!nguoiDung.TrangThaiTK)
                {
                    HienThiLoi("🔒  Tài khoản của bạn đã bị khóa. Liên hệ quản trị viên!");
                    return;
                }

                // ── Lưu thông tin người dùng ──
                NguoiDungHienTai = nguoiDung;

                // ── Yêu cầu đổi mật khẩu lần đầu ──
                if (nguoiDung.DoiMatKhau)
                {
                    var changePass = new ChangePasswordForm();
                    changePass.ShowDialog(this);
                    // Nếu người dùng hủy đổi mật khẩu → không cho vào
                    if (!changePass.DoiMatKhauThanhCong)
                    {
                        NguoiDungHienTai = null;
                        return;
                    }
                }

                // ── Mở MainForm ──
                MoMainForm();
            }
            finally
            {
                btnDangNhap.Enabled = true;
                btnDangNhap.Text = "🔑   Đăng Nhập";
            }
        }

        /// <summary>
        /// Xác thực tài khoản từ SQL Server.
        /// Lấy hash bcrypt từ DB rồi verify bằng BCrypt.Net.
        /// Hỗ trợ cả hash $2y$ (PHP) lẫn $2a$ (chuẩn).
        /// </summary>
        private NguoiDungModel XacThucTaiKhoan(string tenDangNhap, string matKhau)
        {
            const string sql = @"
                SELECT nd.MaNguoiDung, nd.HoTen, nd.SoDienThoai, nd.Email,
                       nd.TenDangNhap, nd.MatKhau, nd.MaVaiTro, nd.TrangThaiTK,
                       nd.DoiMatKhau, nd.NgayTao, nd.AnhDaiDien, nd.IsDeleted,
                       vt.TenVaiTro
                FROM   NguoiDung nd
                JOIN   VaiTro    vt ON vt.MaVaiTro = nd.MaVaiTro
                WHERE  nd.TenDangNhap = @TenDangNhap
                  AND  nd.IsDeleted   = 0";

            DataTable dt = DatabaseConnection.ExecuteQuery(sql, p =>
            {
                p.AddWithValue("@TenDangNhap", tenDangNhap);
            });

            if (dt == null || dt.Rows.Count == 0)
                return null;

            DataRow r = dt.Rows[0];
            string hashInDb = r["MatKhau"].ToString();

            // Verify mật khẩu nhập vào với bcrypt hash trong DB
            if (!VerifyMatKhau(matKhau, hashInDb))
                return null;

            return new NguoiDungModel
            {
                MaNguoiDung = Convert.ToInt32(r["MaNguoiDung"]),
                HoTen = r["HoTen"].ToString(),
                SoDienThoai = r["SoDienThoai"].ToString(),
                Email = r["Email"].ToString(),
                TenDangNhap = r["TenDangNhap"].ToString(),
                MaVaiTro = Convert.ToInt32(r["MaVaiTro"]),
                TrangThaiTK = Convert.ToBoolean(r["TrangThaiTK"]),
                DoiMatKhau = Convert.ToBoolean(r["DoiMatKhau"]),
                NgayTao = Convert.ToDateTime(r["NgayTao"]),
                AnhDaiDien = r["AnhDaiDien"] == DBNull.Value ? null : r["AnhDaiDien"].ToString(),
                IsDeleted = Convert.ToBoolean(r["IsDeleted"]),
                TenVaiTro = r["TenVaiTro"].ToString()
            };
        }

        /// <summary>
        /// Verify mật khẩu an toàn — hỗ trợ $2y$ (PHP) và $2a$ (chuẩn).
        /// Nếu hash bị lỗi format → fallback so sánh plain-text (dữ liệu cũ).
        /// </summary>
        private bool VerifyMatKhau(string matKhauNhap, string hashInDb)
        {
            try
            {
                // BCrypt.Net-Next chỉ hỗ trợ $2a$, chuyển $2y$ → $2a$ để verify
                string hash = hashInDb;
                if (hash.StartsWith("$2y$"))
                    hash = "$2a$" + hash.Substring(4);

                return BCrypt.Net.BCrypt.Verify(matKhauNhap, hash);
            }
            catch
            {
                // Hash bị lỗi format hoặc dữ liệu cũ lưu plain-text
                // Fallback: so sánh trực tiếp (cho giai đoạn chuyển đổi)
                return matKhauNhap == hashInDb;
            }
        }

        private void MoMainForm()
        {
            // Auto điểm danh khi đăng nhập
            AutoDiemDanh();

            var nd = NguoiDungHienTai;
            Form main;

            switch ((DermaSoft.Enums.VaiTro)nd.MaVaiTro)
            {
                case DermaSoft.Enums.VaiTro.LeTan:
                    main = new MainFormLeTan();
                    break;

                case DermaSoft.Enums.VaiTro.BacSi:
                    main = new MainFormBacSi();
                    break;

                case DermaSoft.Enums.VaiTro.Admin:
                default:
                    main = new MainForm();
                    break;
            }

            main.Show();
            this.Hide();

            main.FormClosed += (s, e) =>
            {
                // Kiểm tra cờ DangXuat từ MainForm
                bool dangXuat = false;
                if (main is MainForm mf) dangXuat = mf.DangXuat;
                else if (main is MainFormBacSi mbs) dangXuat = mbs.DangXuat;
                else if (main is MainFormLeTan mlt) dangXuat = mlt.DangXuat;

                if (dangXuat)
                {
                    // Đăng xuất → quay về LoginForm
                    txtTenDangNhap.Clear();
                    txtMatKhau.Clear();
                    AnLoi();
                    this.Show();
                    txtTenDangNhap.Focus();
                }
                else
                {
                    // Đóng ứng dụng (nút ✕)
                    this.Close();
                }
            };
        }

        // ══════════════════════════════════════════════════════════
        // AUTO ĐIỂM DANH KHI ĐĂNG NHẬP
        // ══════════════════════════════════════════════════════════

        /// <summary>
        /// Tự động điểm danh cho người dùng khi đăng nhập.
        /// Cập nhật TrangThaiDiemDanh = 2 (Đã ĐD) cho các PhanCongCa
        /// của user hôm nay mà vẫn đang ở trạng thái 1 (Chưa ĐD).
        /// </summary>
        private void AutoDiemDanh()
        {
            if (NguoiDungHienTai == null) return;

            try
            {
                int maNguoiDung = NguoiDungHienTai.MaNguoiDung;

                int rows = DatabaseConnection.ExecuteNonQuery(@"
                    UPDATE PhanCongCa
                    SET TrangThaiDiemDanh = 2
                    WHERE MaNguoiDung = @MaNV
                      AND NgayLamViec = CAST(GETDATE() AS DATE)
                      AND TrangThaiDiemDanh = 1",
                    p => p.AddWithValue("@MaNV", maNguoiDung));
            }
            catch
            {
                // Không block đăng nhập nếu điểm danh lỗi
            }
        }

        // ══════════════════════════════════════════════════════════
        // HELPER — Hiển thị / Ẩn thông báo lỗi
        // ══════════════════════════════════════════════════════════

        private void HienThiLoi(string thongBao)
        {
            lblError.Text = thongBao;
            pnlError.Visible = true;

            // Animation lắc nhẹ textbox
            RungControl(pnlError);
        }

        private void AnLoi()
        {
            pnlError.Visible = false;
        }

        private async void RungControl(System.Windows.Forms.Control ctrl)
        {
            Point viTriGoc = ctrl.Location;
            int buoc = 5;
            for (int i = 0; i < 4; i++)
            {
                ctrl.Left = viTriGoc.X + buoc;
                await System.Threading.Tasks.Task.Delay(30);
                ctrl.Left = viTriGoc.X - buoc;
                await System.Threading.Tasks.Task.Delay(30);
            }
            ctrl.Left = viTriGoc.X;
        }

        private void lblAppName_Click(object sender, EventArgs e)
        {

        }

        private void lblFooter_Click(object sender, EventArgs e)
        {

        }

        private void lblError_Click(object sender, EventArgs e)
        {

        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lnkQuenMatKhau_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    // ══════════════════════════════════════════════════════════
    // Native API để kéo Form không có titlebar
    // ══════════════════════════════════════════════════════════
    internal static class NativeMethod
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}

