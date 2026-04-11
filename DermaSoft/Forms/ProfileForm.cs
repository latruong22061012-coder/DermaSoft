using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Models;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Form Hồ Sơ Cá Nhân — frm-profile trong wireframe.
    /// Mọi vai trò: xem/sửa thông tin NguoiDung, đổi mật khẩu, đổi ảnh đại diện.
    /// </summary>
    public partial class ProfileForm : Form
    {
        // ── Controls ──
        private Panel pnlContent;

        // Avatar
        private Panel pnlAvatar;
        private string _anhDaiDienPath;

        // Thông tin cá nhân
        private Guna2TextBox txtHoTen;
        private Guna2TextBox txtSDT;
        private Guna2TextBox txtEmail;
        private Label lblTenDangNhap;
        private Label lblVaiTro;
        private Label lblNgayTao;
        private Label lblTrangThai;

        // Đổi mật khẩu
        private Guna2TextBox txtMKCu;
        private Guna2TextBox txtMKMoi;
        private Guna2TextBox txtMKXacNhan;

        // Thông báo
        private Label lblMsg;

        // Màu đồng bộ
        private static readonly Color GoldAccent = Color.FromArgb(184, 138, 40);
        private static readonly Color BorderInput = ColorTranslator.FromHtml("#D1E8DC");
        private static readonly Color InputBg = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color BadgeOkBg = ColorTranslator.FromHtml("#DCFCE7");
        private static readonly Color BadgeOkFg = ColorTranslator.FromHtml("#166534");
        private static readonly Color BadgeDangerBg = ColorTranslator.FromHtml("#FEE2E2");
        private static readonly Color BadgeDangerFg = ColorTranslator.FromHtml("#DC2626");

        private bool _dangVeLai = false;

        public ProfileForm()
        {
            InitializeComponent();
            TaoBoCuc();
        }

        private NguoiDungModel User { get { return LoginForm.NguoiDungHienTai; } }

        // ══════════════════════════════════════════
        // BỐ CỤC CHÍNH
        // ══════════════════════════════════════════

        private void TaoBoCuc()
        {
            this.BackColor = ColorScheme.Background;
            this.Padding = new Padding(0);

            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ColorScheme.Background,
            };
            this.Controls.Add(pnlContent);

            pnlContent.Resize += (s, e) => VeLaiForm();
            VeLaiForm();
        }

        private void VeLaiForm()
        {
            if (_dangVeLai) return;
            _dangVeLai = true;

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();

            int pad = 16;
            int gap = 16;
            int contentW = pnlContent.ClientSize.Width - pad * 2;
            int y = pad;

            y = TaoHeader(pad, y, contentW);

            int colW = (contentW - gap) / 2;
            int availH = Math.Max(480, pnlContent.ClientSize.Height - y - pad);

            // Cột trái: Avatar + Thông tin + Nút lưu
            TaoCotTrai(pad, y, colW, availH);

            // Cột phải: Đổi mật khẩu + Thông tin tài khoản
            int xR = pad + colW + gap;
            TaoCotPhai(xR, y, colW, availH);

            LoadThongTin();

            pnlContent.ResumeLayout();
            _dangVeLai = false;
        }

        // ══════════════════════════════════════════
        // HEADER
        // ══════════════════════════════════════════

        private int TaoHeader(int x, int y, int w)
        {
            var pnlHeader = new Panel { Location = new Point(x, y), Size = new Size(w, 44) };
            pnlHeader.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new LinearGradientBrush(
                    new Rectangle(0, 0, pnlHeader.Width, pnlHeader.Height),
                    ColorScheme.PrimaryDark, Color.FromArgb(180, GoldAccent.R, GoldAccent.G, GoldAccent.B),
                    LinearGradientMode.Horizontal))
                using (var path = TaoRoundedRect(new Rectangle(0, 0, pnlHeader.Width - 1, pnlHeader.Height - 1), 10))
                {
                    g.FillPath(brush, path);
                }
            };
            pnlHeader.Controls.Add(new Label
            {
                Text = "👤  Hồ Sơ Cá Nhân",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(16, 10),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            pnlContent.Controls.Add(pnlHeader);
            return y + 44 + 12;
        }

        // ══════════════════════════════════════════
        // CỘT TRÁI: Avatar + Thông Tin Cá Nhân
        // ══════════════════════════════════════════

        private void TaoCotTrai(int x, int y, int w, int h)
        {
            var card = TaoCard(x, y, w, h);
            int cx = 28;
            int inputW = w - 56;

            // Tính spacing động dựa trên chiều cao khả dụng
            // Các phần tử cố định: avatar(120) + link(24) + title(32) + label+input(62)*3 + msg(24) + btn(44) = 430
            int fixedH = 120 + 24 + 32 + 62 + 62 + 24 + 44;
            int totalGaps = 8; // số khoảng cách giữa các phần tử
            int gapY = Math.Max(12, (h - fixedH) / totalGaps);

            int cy = gapY;

            // ── Avatar tròn ──
            int avatarSize = 120;
            int avatarX = (w - avatarSize) / 2;
            pnlAvatar = new Panel
            {
                Location = new Point(avatarX, cy),
                Size = new Size(avatarSize, avatarSize),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
            };
            pnlAvatar.Paint += PnlAvatar_Paint;
            pnlAvatar.Click += BtnDoiAvatar_Click;
            card.Controls.Add(pnlAvatar);
            cy += avatarSize + 8;

            // Link đổi ảnh
            var btnDoiAnh = new Label
            {
                Text = "📷 Đổi ảnh đại diện",
                Font = AppFonts.Body,
                ForeColor = ColorScheme.Primary,
                AutoSize = true,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
            };
            btnDoiAnh.Location = new Point((w - btnDoiAnh.PreferredWidth) / 2, cy);
            btnDoiAnh.Click += BtnDoiAvatar_Click;
            card.Controls.Add(btnDoiAnh);
            cy += 24 + gapY;

            // ── Đường kẻ phân cách ──
            card.Controls.Add(new Panel { Location = new Point(cx, cy), Size = new Size(inputW, 1), BackColor = ColorScheme.Border });
            cy += gapY;

            // ── Thông tin cá nhân ──
            card.Controls.Add(TaoSectionTitle("📝  Thông Tin Cá Nhân", cx, cy));
            cy += 32 + gapY / 2;

            card.Controls.Add(TaoLabel("Họ và tên", cx, cy));
            cy += 20;
            txtHoTen = TaoInput(cx, cy, inputW, "Nguyễn Văn A");
            card.Controls.Add(txtHoTen);
            cy += 42 + gapY;

            int halfW = (inputW - 16) / 2;
            card.Controls.Add(TaoLabel("Số điện thoại", cx, cy));
            card.Controls.Add(TaoLabel("Email", cx + halfW + 16, cy));
            cy += 20;
            txtSDT = TaoInput(cx, cy, halfW, "0901234567");
            card.Controls.Add(txtSDT);
            txtEmail = TaoInput(cx + halfW + 16, cy, halfW, "email@domain.com");
            card.Controls.Add(txtEmail);
            cy += 42 + gapY;

            // Thông báo
            lblMsg = new Label
            {
                Text = "",
                Font = AppFonts.Body,
                ForeColor = ColorScheme.Success,
                Location = new Point(cx, cy),
                Size = new Size(inputW, 22),
                BackColor = Color.Transparent,
            };
            card.Controls.Add(lblMsg);
            cy += 24;

            // Nút lưu — đặt ở dưới cùng card
            int btnY = Math.Max(cy, h - 44 - 28);
            var btnLuu = TaoBtnPrimary("💾 Lưu Thông Tin", cx, btnY, inputW);
            btnLuu.Click += BtnLuuThongTin_Click;
            card.Controls.Add(btnLuu);

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // CỘT PHẢI: Đổi MK + Thông Tin Tài Khoản
        // ══════════════════════════════════════════

        private void TaoCotPhai(int x, int y, int w, int h)
        {
            var card = TaoCard(x, y, w, h);
            int cx = 28;
            int inputW = w - 56;

            // Tính spacing động: info(~140) + divider + title(32) + input(62)*3 + btn(44) = ~460
            int fixedH = 140 + 1 + 32 + 62 * 3 + 44;
            int totalGaps = 10;
            int gapY = Math.Max(12, (h - fixedH) / totalGaps);

            int cy = gapY;

            // ── Thông tin tài khoản (read-only) ──
            card.Controls.Add(TaoSectionTitle("🔐  Thông Tin Tài Khoản", cx, cy));
            cy += 36;

            int halfW = (inputW - 16) / 2;

            card.Controls.Add(TaoLabel("Tên đăng nhập", cx, cy));
            card.Controls.Add(TaoLabel("Vai trò", cx + halfW + 16, cy));
            cy += 20;
            lblTenDangNhap = new Label
            {
                Font = AppFonts.H3, ForeColor = ColorScheme.TextDark,
                Location = new Point(cx, cy), Size = new Size(halfW, 28), BackColor = Color.Transparent,
            };
            card.Controls.Add(lblTenDangNhap);
            lblVaiTro = new Label
            {
                Font = AppFonts.Badge, ForeColor = Color.White,
                Location = new Point(cx + halfW + 16, cy + 2), AutoSize = true, BackColor = ColorScheme.Primary,
                Padding = new Padding(10, 5, 10, 5),
            };
            card.Controls.Add(lblVaiTro);
            cy += 28 + gapY;

            card.Controls.Add(TaoLabel("Ngày tạo tài khoản", cx, cy));
            card.Controls.Add(TaoLabel("Trạng thái", cx + halfW + 16, cy));
            cy += 20;
            lblNgayTao = new Label
            {
                Font = new Font("Segoe UI", 10.5f), ForeColor = ColorScheme.TextMid,
                Location = new Point(cx, cy), AutoSize = true, BackColor = Color.Transparent,
            };
            card.Controls.Add(lblNgayTao);
            lblTrangThai = new Label
            {
                Font = AppFonts.Badge,
                Location = new Point(cx + halfW + 16, cy + 2), AutoSize = true,
                Padding = new Padding(10, 5, 10, 5),
            };
            card.Controls.Add(lblTrangThai);
            cy += 28 + gapY;

            // ── Đường kẻ phân cách ──
            card.Controls.Add(new Panel { Location = new Point(cx, cy), Size = new Size(inputW, 1), BackColor = ColorScheme.Border });
            cy += gapY;

            // ── Đổi mật khẩu ──
            card.Controls.Add(TaoSectionTitle("🔑  Đổi Mật Khẩu", cx, cy));
            cy += 32 + gapY / 2;

            card.Controls.Add(TaoLabel("Mật khẩu hiện tại", cx, cy));
            cy += 20;
            txtMKCu = TaoInput(cx, cy, inputW, "••••••••");
            txtMKCu.UseSystemPasswordChar = true;
            card.Controls.Add(txtMKCu);
            cy += 42 + gapY;

            card.Controls.Add(TaoLabel("Mật khẩu mới", cx, cy));
            cy += 20;
            txtMKMoi = TaoInput(cx, cy, inputW, "Tối thiểu 6 ký tự");
            txtMKMoi.UseSystemPasswordChar = true;
            card.Controls.Add(txtMKMoi);
            cy += 42 + gapY;

            card.Controls.Add(TaoLabel("Xác nhận mật khẩu mới", cx, cy));
            cy += 20;
            txtMKXacNhan = TaoInput(cx, cy, inputW, "Nhập lại mật khẩu mới");
            txtMKXacNhan.UseSystemPasswordChar = true;
            card.Controls.Add(txtMKXacNhan);
            cy += 42 + gapY;

            // Nút đổi MK — đặt ở dưới cùng card
            int btnY = Math.Max(cy, h - 44 - 28);
            var btnDoiMK = TaoBtnWarning("🔒 Đổi Mật Khẩu", cx, btnY, inputW);
            btnDoiMK.Click += BtnDoiMatKhau_Click;
            card.Controls.Add(btnDoiMK);

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // AVATAR PAINT
        // ══════════════════════════════════════════

        private void PnlAvatar_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int size = pnlAvatar.Width;
            var rect = new Rectangle(0, 0, size - 1, size - 1);

            using (var path = new GraphicsPath())
            {
                path.AddEllipse(rect);
                g.SetClip(path);

                bool hasAvatar = false;
                if (!string.IsNullOrEmpty(_anhDaiDienPath) && File.Exists(_anhDaiDienPath))
                {
                    try
                    {
                        using (var img = Image.FromFile(_anhDaiDienPath))
                        {
                            g.DrawImage(img, rect);
                            hasAvatar = true;
                        }
                    }
                    catch { }
                }

                if (!hasAvatar)
                {
                    using (var brush = new LinearGradientBrush(rect, ColorScheme.PrimaryDark, ColorScheme.Primary, 45f))
                        g.FillEllipse(brush, rect);

                    string initials = "?";
                    if (User != null && !string.IsNullOrEmpty(User.HoTen))
                    {
                        var parts = User.HoTen.Trim().Split(' ');
                        initials = parts.Length >= 2
                            ? parts[0].Substring(0, 1).ToUpper() + parts[parts.Length - 1].Substring(0, 1).ToUpper()
                            : parts[0].Substring(0, 1).ToUpper();
                    }

                    using (var f = new Font("Segoe UI", 28f, FontStyle.Bold))
                    using (var br = new SolidBrush(Color.White))
                    {
                        var sz = g.MeasureString(initials, f);
                        g.DrawString(initials, f, br, (size - sz.Width) / 2, (size - sz.Height) / 2);
                    }
                }

                g.ResetClip();
            }

            // Viền tròn
            using (var pen = new Pen(ColorScheme.Primary, 3f))
                g.DrawEllipse(pen, rect);
        }

        // ══════════════════════════════════════════
        // LOAD THÔNG TIN
        // ══════════════════════════════════════════

        private void LoadThongTin()
        {
            if (User == null) return;

            if (txtHoTen != null) txtHoTen.Text = User.HoTen ?? "";
            if (txtSDT != null) txtSDT.Text = User.SoDienThoai ?? "";
            if (txtEmail != null) txtEmail.Text = User.Email ?? "";
            if (lblTenDangNhap != null) lblTenDangNhap.Text = User.TenDangNhap ?? "";
            if (lblVaiTro != null) lblVaiTro.Text = " " + (User.TenVaiTro ?? "N/A") + " ";
            if (lblNgayTao != null) lblNgayTao.Text = User.NgayTao.ToString("dd/MM/yyyy");

            if (lblTrangThai != null)
            {
                if (User.TrangThaiTK)
                {
                    lblTrangThai.Text = " Hoạt động ";
                    lblTrangThai.BackColor = BadgeOkBg;
                    lblTrangThai.ForeColor = BadgeOkFg;
                }
                else
                {
                    lblTrangThai.Text = " Bị khóa ";
                    lblTrangThai.BackColor = BadgeDangerBg;
                    lblTrangThai.ForeColor = BadgeDangerFg;
                }
            }

            _anhDaiDienPath = User.AnhDaiDien;
            pnlAvatar?.Invalidate();
        }

        // ══════════════════════════════════════════
        // LƯU THÔNG TIN CÁ NHÂN
        // ══════════════════════════════════════════

        private void BtnLuuThongTin_Click(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(hoTen))
            {
                HienMsg("❌ Họ tên không được để trống.", ColorScheme.Danger);
                return;
            }
            if (string.IsNullOrEmpty(sdt))
            {
                HienMsg("❌ Số điện thoại không được để trống.", ColorScheme.Danger);
                return;
            }

            try
            {
                int rows = DatabaseConnection.ExecuteNonQuery(
                    "UPDATE NguoiDung SET HoTen=@HT, SoDienThoai=@SDT, Email=@Email WHERE MaNguoiDung=@Ma",
                    p =>
                    {
                        p.AddWithValue("@HT", hoTen);
                        p.AddWithValue("@SDT", sdt);
                        p.AddWithValue("@Email", (object)email ?? DBNull.Value);
                        p.AddWithValue("@Ma", User.MaNguoiDung);
                    });

                if (rows > 0)
                {
                    User.HoTen = hoTen;
                    User.SoDienThoai = sdt;
                    User.Email = email;
                    HienMsg("✅ Cập nhật thông tin thành công!", ColorScheme.Success);
                }
                else
                {
                    HienMsg("⚠️ Không tìm thấy tài khoản.", ColorScheme.Warning);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                    HienMsg("❌ SĐT hoặc Email đã tồn tại trong hệ thống.", ColorScheme.Danger);
                else
                    HienMsg("❌ Lỗi: " + ex.Message, ColorScheme.Danger);
            }
        }

        // ══════════════════════════════════════════
        // ĐỔI MẬT KHẨU
        // ══════════════════════════════════════════

        private void BtnDoiMatKhau_Click(object sender, EventArgs e)
        {
            string mkCu = txtMKCu.Text;
            string mkMoi = txtMKMoi.Text;
            string mkXacNhan = txtMKXacNhan.Text;

            if (string.IsNullOrEmpty(mkCu))
            {
                HienMsg("❌ Vui lòng nhập mật khẩu hiện tại.", ColorScheme.Danger);
                return;
            }
            if (mkMoi.Length < 6)
            {
                HienMsg("❌ Mật khẩu mới phải ít nhất 6 ký tự.", ColorScheme.Danger);
                return;
            }
            if (mkMoi != mkXacNhan)
            {
                HienMsg("❌ Mật khẩu xác nhận không khớp.", ColorScheme.Danger);
                return;
            }
            if (mkCu == mkMoi)
            {
                HienMsg("❌ Mật khẩu mới phải khác mật khẩu cũ.", ColorScheme.Danger);
                return;
            }

            try
            {
                // Lấy hash hiện tại từ DB
                object hashObj = DatabaseConnection.ExecuteScalar(
                    "SELECT MatKhau FROM NguoiDung WHERE MaNguoiDung=@Ma",
                    p => p.AddWithValue("@Ma", User.MaNguoiDung));

                if (hashObj == null)
                {
                    HienMsg("❌ Không tìm thấy tài khoản.", ColorScheme.Danger);
                    return;
                }

                // Verify mật khẩu cũ
                if (!VerifyMatKhau(mkCu, hashObj.ToString()))
                {
                    HienMsg("❌ Mật khẩu hiện tại không đúng.", ColorScheme.Danger);
                    return;
                }

                // Hash mật khẩu mới bằng BCrypt
                string newHash = BCrypt.Net.BCrypt.HashPassword(mkMoi, workFactor: 10);

                int rows = DatabaseConnection.ExecuteNonQuery(
                    "UPDATE NguoiDung SET MatKhau=@MK, DoiMatKhau=0 WHERE MaNguoiDung=@Ma",
                    p =>
                    {
                        p.AddWithValue("@MK", newHash);
                        p.AddWithValue("@Ma", User.MaNguoiDung);
                    });

                if (rows > 0)
                {
                    User.DoiMatKhau = false;
                    txtMKCu.Text = "";
                    txtMKMoi.Text = "";
                    txtMKXacNhan.Text = "";
                    HienMsg("✅ Đổi mật khẩu thành công!", ColorScheme.Success);
                }
            }
            catch (Exception ex)
            {
                HienMsg("❌ Lỗi: " + ex.Message, ColorScheme.Danger);
            }
        }

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

        // ══════════════════════════════════════════
        // ĐỔI ẢNH ĐẠI DIỆN
        // ══════════════════════════════════════════

        private void BtnDoiAvatar_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh đại diện";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    // Copy ảnh vào thư mục Avatars
                    string appDir = Path.GetDirectoryName(Application.ExecutablePath);
                    string avatarDir = Path.Combine(appDir, "Avatars");
                    if (!Directory.Exists(avatarDir))
                        Directory.CreateDirectory(avatarDir);

                    string ext = Path.GetExtension(ofd.FileName);
                    string fileName = "avatar_" + User.MaNguoiDung + ext;
                    string destPath = Path.Combine(avatarDir, fileName);

                    File.Copy(ofd.FileName, destPath, true);

                    // Lưu đường dẫn vào DB
                    DatabaseConnection.ExecuteNonQuery(
                        "UPDATE NguoiDung SET AnhDaiDien=@Anh WHERE MaNguoiDung=@Ma",
                        p =>
                        {
                            p.AddWithValue("@Anh", destPath);
                            p.AddWithValue("@Ma", User.MaNguoiDung);
                        });

                    User.AnhDaiDien = destPath;
                    _anhDaiDienPath = destPath;
                    pnlAvatar.Invalidate();

                    HienMsg("✅ Đổi ảnh đại diện thành công!", ColorScheme.Success);
                }
                catch (Exception ex)
                {
                    HienMsg("❌ Lỗi: " + ex.Message, ColorScheme.Danger);
                }
            }
        }

        // ══════════════════════════════════════════
        // HELPERS
        // ══════════════════════════════════════════

        private void HienMsg(string text, Color color)
        {
            if (lblMsg == null) return;
            lblMsg.Text = text;
            lblMsg.ForeColor = color;
        }

        private Panel TaoCard(int x, int y, int w, int h)
        {
            var pnl = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = Color.White,
            };
            pnl.Paint += (s, e) =>
            {
                using (var pen = new Pen(ColorScheme.Border, 1f))
                    e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };
            return pnl;
        }

        private Label TaoSectionTitle(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = ColorScheme.PrimaryDark,
                Location = new Point(x, y),
                AutoSize = true,
                BackColor = Color.Transparent,
            };
        }

        private Label TaoLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextGray,
                Location = new Point(x, y),
                AutoSize = true,
                BackColor = Color.Transparent,
            };
        }

        private Guna2TextBox TaoInput(int x, int y, int w, string placeholder)
        {
            return new Guna2TextBox
            {
                Font = new Font("Segoe UI", 10.5f), ForeColor = ColorScheme.TextDark,
                Location = new Point(x, y), Size = new Size(w, 42),
                PlaceholderText = placeholder, PlaceholderForeColor = ColorScheme.TextLight,
                BorderRadius = 10, BorderColor = BorderInput,
                FocusedState = { BorderColor = ColorScheme.Primary },
                HoverState = { BorderColor = ColorScheme.Primary },
                FillColor = InputBg,
            };
        }

        private Guna2Button TaoBtnPrimary(string text, int x, int y, int w)
        {
            return new Guna2Button
            {
                Text = text, Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.White,
                FillColor = ColorScheme.Primary, BorderRadius = 20,
                Location = new Point(x, y), Size = new Size(w, 44), Cursor = Cursors.Hand,
            };
        }

        private Guna2Button TaoBtnWarning(string text, int x, int y, int w)
        {
            return new Guna2Button
            {
                Text = text, Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.White,
                FillColor = ColorScheme.Warning, BorderRadius = 20,
                Location = new Point(x, y), Size = new Size(w, 44), Cursor = Cursors.Hand,
            };
        }

        private static GraphicsPath TaoRoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
