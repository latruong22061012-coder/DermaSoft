using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DermaSoft.Data;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Form Đổi Mật Khẩu — frm-changepass trong wireframe.
    /// Dùng cho lần đăng nhập đầu tiên (DoiMatKhau = 1) hoặc khi người dùng tự đổi.
    /// </summary>
    public partial class ChangePasswordForm : Form
    {
        public bool DoiMatKhauThanhCong { get; private set; } = false;

        public ChangePasswordForm()
        {
            InitializeComponent();
            DangKyEvents();
        }

        private void DangKyEvents()
        {
            this.btnLuuMatKhau.Click += BtnLuuMatKhau_Click;
            this.btnHuy.Click        += BtnHuy_Click;
            this.txtMatKhauMoi.TextChanged += TxtMatKhauMoi_TextChanged;
            this.txtMatKhauHienTai.KeyDown += TxtInput_KeyDown;
            this.txtMatKhauMoi.KeyDown     += TxtInput_KeyDown;
            this.txtXacNhan.KeyDown        += TxtInput_KeyDown;

            // Ẩn lỗi khi nhập lại
            this.txtMatKhauHienTai.TextChanged += (s, e) => AnLoi();
            this.txtXacNhan.TextChanged        += (s, e) => AnLoi();
        }

        // ══════════════════════════════════════════
        // SỰ KIỆN
        // ══════════════════════════════════════════

        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ThucHienDoiMatKhau();
            }
        }

        private void TxtMatKhauMoi_TextChanged(object sender, EventArgs e)
        {
            AnLoi();
            CapNhatDoManh(txtMatKhauMoi.Text);
        }

        private void BtnLuuMatKhau_Click(object sender, EventArgs e)
        {
            ThucHienDoiMatKhau();
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            DoiMatKhauThanhCong = false;
            this.Close();
        }

        // ══════════════════════════════════════════
        // LOGIC ĐỔI MẬT KHẨU
        // ══════════════════════════════════════════

        private void ThucHienDoiMatKhau()
        {
            string matKhauCu  = txtMatKhauHienTai.Text;
            string matKhauMoi = txtMatKhauMoi.Text;
            string xacNhan    = txtXacNhan.Text;

            // Validate
            if (string.IsNullOrEmpty(matKhauCu))
            {
                HienThiLoi("Vui lòng nhập mật khẩu hiện tại!");
                txtMatKhauHienTai.Focus();
                return;
            }
            if (string.IsNullOrEmpty(matKhauMoi))
            {
                HienThiLoi("Vui lòng nhập mật khẩu mới!");
                txtMatKhauMoi.Focus();
                return;
            }
            if (matKhauMoi.Length < 8)
            {
                HienThiLoi("Mật khẩu mới phải có ít nhất 8 ký tự!");
                txtMatKhauMoi.Focus();
                return;
            }
            if (!Regex.IsMatch(matKhauMoi, @"[A-Z]"))
            {
                HienThiLoi("Mật khẩu mới phải có ít nhất 1 chữ hoa!");
                txtMatKhauMoi.Focus();
                return;
            }
            if (!Regex.IsMatch(matKhauMoi, @"[0-9]"))
            {
                HienThiLoi("Mật khẩu mới phải có ít nhất 1 chữ số!");
                txtMatKhauMoi.Focus();
                return;
            }
            if (matKhauMoi != xacNhan)
            {
                HienThiLoi("Xác nhận mật khẩu không khớp!");
                txtXacNhan.Focus();
                return;
            }
            if (matKhauMoi == matKhauCu)
            {
                HienThiLoi("Mật khẩu mới không được trùng mật khẩu cũ!");
                txtMatKhauMoi.Focus();
                return;
            }

            // Kiểm tra mật khẩu cũ đúng không
            var nguoiDung = LoginForm.NguoiDungHienTai;
            if (nguoiDung == null)
            {
                HienThiLoi("Lỗi: Không tìm thấy thông tin người dùng!");
                return;
            }

            // Xác thực mật khẩu cũ bằng BCrypt
            const string sqlGetHash = @"
                SELECT MatKhau FROM NguoiDung
                WHERE MaNguoiDung = @MaND AND IsDeleted = 0";
            object hashObj = DatabaseConnection.ExecuteScalar(sqlGetHash, p =>
            {
                p.AddWithValue("@MaND", nguoiDung.MaNguoiDung);
            });

            if (hashObj == null || !VerifyMatKhau(matKhauCu, hashObj.ToString()))
            {
                HienThiLoi("Mật khẩu hiện tại không đúng!");
                txtMatKhauHienTai.Clear();
                txtMatKhauHienTai.Focus();
                return;
            }

            // Hash mật khẩu mới bằng BCrypt rồi lưu
            string matKhauMoiHash = BCrypt.Net.BCrypt.HashPassword(matKhauMoi, workFactor: 10);

            const string sqlUpdate = @"
                UPDATE NguoiDung
                SET MatKhau = @MatKhauMoi, DoiMatKhau = 0
                WHERE MaNguoiDung = @MaND";
            int rows = DatabaseConnection.ExecuteNonQuery(sqlUpdate, p =>
            {
                p.AddWithValue("@MatKhauMoi", matKhauMoiHash);
                p.AddWithValue("@MaND", nguoiDung.MaNguoiDung);
            });

            if (rows > 0)
            {
                nguoiDung.DoiMatKhau = false;
                DoiMatKhauThanhCong = true;
                MessageBox.Show(
                    "Đổi mật khẩu thành công!\nVui lòng ghi nhớ mật khẩu mới.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                HienThiLoi("Lỗi hệ thống! Không thể cập nhật mật khẩu.");
            }
        }

        // ══════════════════════════════════════════
        // ĐỘ MẠNH MẬT KHẨU
        // ══════════════════════════════════════════

        private void CapNhatDoManh(string matKhau)
        {
            int diem = 0;
            if (matKhau.Length >= 8) diem += 25;
            if (matKhau.Length >= 12) diem += 10;
            if (Regex.IsMatch(matKhau, @"[A-Z]")) diem += 20;
            if (Regex.IsMatch(matKhau, @"[a-z]")) diem += 15;
            if (Regex.IsMatch(matKhau, @"[0-9]")) diem += 15;
            if (Regex.IsMatch(matKhau, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]")) diem += 15;

            string tenDoManh;
            Color  mauDoManh;
            Color  mauBar;

            if (diem == 0 || string.IsNullOrEmpty(matKhau))
            {
                tenDoManh = "";
                mauDoManh = Color.FromArgb(156, 163, 175);
                mauBar    = Color.FromArgb(156, 163, 175);
            }
            else if (diem < 30)
            {
                tenDoManh = "Yếu";
                mauDoManh = Color.FromArgb(220, 38, 38);
                mauBar    = Color.FromArgb(239, 68, 68);
            }
            else if (diem < 55)
            {
                tenDoManh = "Trung bình";
                mauDoManh = Color.FromArgb(245, 158, 11);
                mauBar    = Color.FromArgb(251, 191, 36);
            }
            else if (diem < 80)
            {
                tenDoManh = "Khá";
                mauDoManh = Color.FromArgb(34, 197, 94);
                mauBar    = Color.FromArgb(74, 222, 128);
            }
            else
            {
                tenDoManh = "Mạnh";
                mauDoManh = Color.FromArgb(22, 163, 74);
                mauBar    = Color.FromArgb(22, 163, 74);
            }

            lblDoManhGiaTri.Text      = tenDoManh;
            lblDoManhGiaTri.ForeColor = mauDoManh;
            barDoManh.Value           = Math.Min(diem, 100);
            barDoManh.ProgressColor   = mauBar;
            barDoManh.ProgressColor2  = mauBar;
        }

        // ══════════════════════════════════════════
        // HELPER
        // ══════════════════════════════════════════

        private void HienThiLoi(string thongBao)
        {
            lblError.Text    = "\u26A0\uFE0F  " + thongBao;
            pnlError.Visible = true;
        }

        private void AnLoi()
        {
            if (pnlError.Visible)
                pnlError.Visible = false;
        }

        /// <summary>
        /// Verify mật khẩu an toàn — hỗ trợ $2y$ (PHP) và $2a$ (chuẩn).
        /// </summary>
        private bool VerifyMatKhau(string matKhauNhap, string hashInDb)
        {
            try
            {
                string hash = hashInDb;
                if (hash.StartsWith("$2y$"))
                    hash = "$2a$" + hash.Substring(4);

                return BCrypt.Net.BCrypt.Verify(matKhauNhap, hash);
            }
            catch
            {
                return matKhauNhap == hashInDb;
            }
        }

        private void btnHuy_Click_1(object sender, EventArgs e)
        {

        }

        private void barDoManh_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pnlTitleLogo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtMatKhauHienTai_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
