using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DermaSoft.Helpers;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Form chính dành cho vai trò Quản Kho.
    /// Menu: Dashboard, Nhập Kho, Danh Mục Thuốc, Tồn Kho, Báo Cáo Kho, Hồ Sơ Cá Nhân.
    /// </summary>
    public partial class MainFormQuanKho : Form
    {
        private Guna2Button _menuHienTai = null;

        /// <summary>Cờ đánh dấu đang đăng xuất (true) hay đóng ứng dụng (false).</summary>
        internal bool DangXuat { get; private set; } = false;

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        public MainFormQuanKho()
        {
            InitializeComponent();
            if (this.DesignMode ||
                System.ComponentModel.LicenseManager.UsageMode ==
                System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            DoubleBufferHelper.BatDoubleBuffered(pnlMdiArea);
            CaiDatThongTinNguoiDung();
            TaoMenuSidebar();
            CapNhatNgayGio();
            HienThiMdiMacDinh();
        }

        // ══════════════════════════════════════════════════════════════════════
        // KHỞI TẠO
        // ══════════════════════════════════════════════════════════════════════

        private void CaiDatThongTinNguoiDung()
        {
            var nd = LoginForm.NguoiDungHienTai;
            if (nd == null) return;

            string chuCaiDau = string.IsNullOrEmpty(nd.HoTen)
                ? "K"
                : nd.HoTen.Substring(0, 1).ToUpper();

            lblTopbarAvatar.Text = chuCaiDau;
            lblUserName.Text = nd.HoTen;
            lblUserRole.Text = "📦 Quản Lý Kho";

            btnDangXuat.Click += BtnDangXuat_Click;
        }

        private void CapNhatNgayGio()
        {
            lblNgayGio.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");
            var timer = new Timer { Interval = 60_000 };
            timer.Tick += (s, e) =>
            {
                lblNgayGio.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");
            };
            timer.Start();
        }

        // ══════════════════════════════════════════════════════════════════════
        // SIDEBAR MENU
        // ══════════════════════════════════════════════════════════════════════

        private void TaoMenuSidebar()
        {
            pnlSidebarNav.Controls.Clear();
            int y = 4;

            // ── TỔNG QUAN ─────────────────────────────────────────────────────
            y = ThemSectionLabel("TỔNG QUAN", y);
            y = ThemMenuItem("📊", "Dashboard", y, active: true);

            // ── QUẢN LÝ KHO ──────────────────────────────────────────────────
            y = ThemSectionLabel("QUẢN LÝ KHO", y);
            y = ThemMenuItem("📦", "Nhập Kho", y);
            y = ThemMenuItem("💊", "Danh Mục Thuốc", y);
            y = ThemMenuItem("🏪", "Tồn Kho", y);

            // ── BÁO CÁO ──────────────────────────────────────────────────────
            y = ThemSectionLabel("BÁO CÁO", y);
            y = ThemMenuItem("📋", "Báo Cáo Kho", y);

            // ── HỆ THỐNG ─────────────────────────────────────────────────────
            y = ThemSectionLabel("HỆ THỐNG", y);
            y = ThemMenuItem("👤", "Hồ Sơ Cá Nhân", y);
        }

        private int ThemSectionLabel(string text, int y)
        {
            var lbl = new Label
            {
                Text = text.ToUpper(),
                Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 168, 230, 207),
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(310, 18),
                Location = new Point(18, y + 8),
            };
            pnlSidebarNav.Controls.Add(lbl);
            return y + 26;
        }

        private int ThemMenuItem(string icon, string text, int y, bool active = false)
        {
            var pnl = new Panel
            {
                Size = new Size(330, 38),
                Location = new Point(0, y),
                BackColor = active ? Color.FromArgb(38, 168, 230, 207) : Color.Transparent,
                Cursor = Cursors.Hand,
                Tag = text,
            };

            var borderLeft = new Panel
            {
                Size = new Size(3, 38),
                Location = new Point(0, 0),
                BackColor = active ? Color.FromArgb(168, 230, 207) : Color.Transparent,
            };
            pnl.Controls.Add(borderLeft);

            var lbl = new Label
            {
                Text = "  " + icon + "   " + text,
                Font = new Font("Segoe UI", 9f, active ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = active ? Color.White : Color.FromArgb(180, 255, 255, 255),
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(310, 38),
                Location = new Point(8, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
            };
            pnl.Controls.Add(lbl);

            Action<bool> setHover = (hover) =>
            {
                if (_menuHienTai?.Parent == pnl) return;
                pnl.BackColor = hover ? Color.FromArgb(20, 168, 230, 207) : Color.Transparent;
                borderLeft.BackColor = hover ? Color.FromArgb(100, 168, 230, 207) : Color.Transparent;
                lbl.ForeColor = hover ? Color.White : Color.FromArgb(180, 255, 255, 255);
            };

            pnl.MouseEnter += (s, e) => setHover(true);
            pnl.MouseLeave += (s, e) => setHover(false);
            lbl.MouseEnter += (s, e) => setHover(true);
            lbl.MouseLeave += (s, e) => setHover(false);

            EventHandler onClick = (s, e) => MenuItem_Click(pnl, borderLeft, lbl);
            pnl.Click += onClick;
            lbl.Click += onClick;

            pnlSidebarNav.Controls.Add(pnl);

            if (active)
                _menuHienTai = new Guna2Button { Parent = pnl, Visible = false, Tag = text };

            return y + 40;
        }

        // ══════════════════════════════════════════════════════════════════════
        // SỰ KIỆN MENU
        // ══════════════════════════════════════════════════════════════════════

        private void MenuItem_Click(Panel pnl, Panel borderLeft, Label lbl)
        {
            if (_menuHienTai?.Parent != null)
            {
                var oldPnl = _menuHienTai.Parent;
                oldPnl.BackColor = Color.Transparent;
                foreach (Control c in oldPnl.Controls)
                {
                    if (c is Panel p && p.Width == 3)
                        p.BackColor = Color.Transparent;
                    if (c is Label l)
                    {
                        l.ForeColor = Color.FromArgb(180, 255, 255, 255);
                        l.Font = new Font("Segoe UI", 9f);
                    }
                }
            }

            pnl.BackColor = Color.FromArgb(38, 168, 230, 207);
            borderLeft.BackColor = Color.FromArgb(168, 230, 207);
            lbl.ForeColor = Color.White;
            lbl.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            _menuHienTai = new Guna2Button { Parent = pnl, Visible = false, Tag = pnl.Tag };

            string tenMenu = pnl.Tag?.ToString() ?? "";
            lblTopbarTitle.Text = tenMenu;
            lblBreadcrumb.Text = "DermaSoft › " + tenMenu;
            lblTopbarTitle.Location = new Point(lblBreadcrumb.Right + 10, lblTopbarTitle.Location.Y);

            MoFormCon(tenMenu);
        }

        // ══════════════════════════════════════════════════════════════════════
        // MDI AREA
        // ══════════════════════════════════════════════════════════════════════

        private void HienThiMdiMacDinh()
        {
            MoFormCon("Dashboard");
        }

        private void MoFormCon(string tenMenu)
        {
            Form frm = null;
            switch (tenMenu)
            {
                case "Dashboard":       frm = new DashboardQuanKhoForm(); break;
                case "Nhập Kho":        frm = new NhapKhoForm(); break;
                case "Danh Mục Thuốc":  frm = new ThuocForm(); break;
                case "Tồn Kho":         frm = new TonKhoForm(); break;
                case "Báo Cáo Kho":     frm = new BaoCaoKhoForm(); break;
                case "Hồ Sơ Cá Nhân":   frm = new ProfileForm(); break;
            }

            DoubleBufferHelper.NhungFormCon(pnlMdiArea, frm);
        }

        // ══════════════════════════════════════════════════════════════════════
        // PAINT — Bo tròn avatar
        // ══════════════════════════════════════════════════════════════════════

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.DesignMode) return;
            BoTronLabel(lblTopbarAvatar);
        }

        private void BoTronLabel(Label lbl)
        {
            lbl.Paint += (s, pe) =>
            {
                var g = pe.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                int d = Math.Min(lbl.Width, lbl.Height);
                using (var brush = new SolidBrush(lbl.BackColor))
                using (var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                })
                {
                    g.Clear(lbl.Parent.BackColor);
                    g.FillEllipse(brush, 0, 0, d - 1, d - 1);
                    g.DrawString(lbl.Text, lbl.Font, Brushes.White,
                                 new RectangleF(0, 0, d, d), sf);
                }
            };
            lbl.Invalidate();
        }

        // ══════════════════════════════════════════════════════════════════════
        // EVENT HANDLERS
        // ══════════════════════════════════════════════════════════════════════

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = this.WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;
        }

        private void BtnDangXuat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoginForm.NguoiDungHienTai = null;
                DangXuat = true;
                this.Close();
            }
        }
    }
}
