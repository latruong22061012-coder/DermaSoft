using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DermaSoft.Enums;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    public partial class MainForm : Form
    {
        private Guna2Button _menuHienTai = null;

        public MainForm()
        {
            InitializeComponent();
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;
            CaiDatThongTinNguoiDung();
            TaoMenuSidebar();
            CapNhatNgayGio();
            HienThiMdiMacDinh();
        }

        // ══════════════════════════════════════════
        // KHỞI TẠO
        // ══════════════════════════════════════════

        private void CaiDatThongTinNguoiDung()
        {
            var nd = LoginForm.NguoiDungHienTai;
            if (nd == null) return;

            string chuCaiDau = string.IsNullOrEmpty(nd.HoTen) ? "?" : nd.HoTen.Substring(0, 1).ToUpper();
            lblTopbarAvatar.Text  = chuCaiDau;
            lblUserName.Text      = nd.HoTen;

            string iconVaiTro;
            string tenVaiTro;
            switch ((VaiTro)nd.MaVaiTro)
            {
                case VaiTro.Admin:
                    iconVaiTro = "\uD83D\uDEE1\uFE0F";
                    tenVaiTro  = "Qu\u1ea3n Tr\u1ecb H\u1ec7 Th\u1ed1ng";
                    break;
                case VaiTro.BacSi:
                    iconVaiTro = "\uD83E\uDE7A";
                    tenVaiTro  = "B\u00e1c S\u0129";
                    break;
                case VaiTro.LeTan:
                    iconVaiTro = "\uD83D\uDCCB";
                    tenVaiTro  = "L\u1ec5 T\u00e2n";
                    break;
                default:
                    iconVaiTro = "\uD83D\uDC64";
                    tenVaiTro  = nd.TenVaiTro;
                    break;
            }
            lblUserRole.Text = iconVaiTro + " " + tenVaiTro;

            btnDangXuat.Click += BtnDangXuat_Click;
        }

        private void CapNhatNgayGio()
        {
            var timer = new Timer { Interval = 60000 };
            timer.Tick += (s, e) =>
            {
                lblNgayGio.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");
            };
            timer.Start();
            lblNgayGio.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");
        }

        // ══════════════════════════════════════════
        // SIDEBAR MENU
        // ══════════════════════════════════════════

        private void TaoMenuSidebar()
        {
            pnlSidebarNav.Controls.Clear();

            var nd = LoginForm.NguoiDungHienTai;
            VaiTro vaiTro = nd != null ? (VaiTro)nd.MaVaiTro : VaiTro.Admin;

            int y = 4;

            y = ThemSectionLabel("T\u1ed5ng Quan", y);
            y = ThemMenuItem("\uD83D\uDCCA", "Dashboard", y, true);

            if (vaiTro == VaiTro.Admin)
            {
                y = ThemSectionLabel("Nh\u00e2n S\u1ef1 & Ca", y);
                y = ThemMenuItem("\uD83D\uDC65", "Nh\u00e2n Vi\u00ean", y);
                y = ThemMenuItem("\uD83D\uDCC5", "Ph\u00e2n C\u00f4ng Ca", y);

                y = ThemSectionLabel("Kho & Thu\u1ed1c", y);
                y = ThemMenuItem("\uD83D\uDC8A", "Danh M\u1ee5c Thu\u1ed1c", y);
                y = ThemMenuItem("\uD83D\uDCE6", "Nh\u1eadp Kho", y);
                y = ThemMenuItem("\uD83C\uDFEA", "T\u1ed3n Kho", y);

                y = ThemSectionLabel("D\u1ecbch V\u1ee5 & B\u00e1o C\u00e1o", y);
                y = ThemMenuItem("\u2728", "D\u1ecbch V\u1ee5", y);
                y = ThemMenuItem("\uD83D\uDCC8", "B\u00e1o C\u00e1o Doanh Thu", y);
                y = ThemMenuItem("\uD83D\uDCCB", "B\u00e1o C\u00e1o Kho", y);

                y = ThemSectionLabel("H\u1ec7 Th\u1ed1ng", y);
                y = ThemMenuItem("\u2B50", "\u0110\u00e1nh Gi\u00e1", y);
                y = ThemMenuItem("\u2699\uFE0F", "C\u00e0i \u0110\u1eb7t", y);
                y = ThemMenuItem("\uD83D\uDC64", "H\u1ed3 S\u01a1 C\u00e1 Nh\u00e2n", y);
            }
            else if (vaiTro == VaiTro.BacSi)
            {
                y = ThemSectionLabel("Kh\u00e1m B\u1ec7nh", y);
                y = ThemMenuItem("\uD83D\uDCC5", "L\u1ecbch H\u1eb9n", y);
                y = ThemMenuItem("\uD83E\uDE7A", "Phi\u1ebfu Kh\u00e1m", y);

                y = ThemSectionLabel("H\u1ec7 Th\u1ed1ng", y);
                y = ThemMenuItem("\uD83D\uDC64", "H\u1ed3 S\u01a1 C\u00e1 Nh\u00e2n", y);
            }
            else if (vaiTro == VaiTro.LeTan)
            {
                y = ThemSectionLabel("Ti\u1ebfp \u0110\u00f3n", y);
                y = ThemMenuItem("\uD83D\uDCC5", "L\u1ecbch H\u1eb9n", y);
                y = ThemMenuItem("\uD83D\uDC65", "B\u1ec7nh Nh\u00e2n", y);

                y = ThemSectionLabel("H\u1ec7 Th\u1ed1ng", y);
                y = ThemMenuItem("\uD83D\uDC64", "H\u1ed3 S\u01a1 C\u00e1 Nh\u00e2n", y);
            }
        }

        private int ThemSectionLabel(string text, int y)
        {
            var lbl = new Label
            {
                Text      = text.ToUpper(),
                Font      = new Font("Segoe UI", 7f, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 168, 230, 207),
                BackColor = Color.Transparent,
                AutoSize  = false,
                Size      = new Size(240, 18),
                Location  = new Point(18, y + 8),
            };
            pnlSidebarNav.Controls.Add(lbl);
            return y + 26;
        }

        private int ThemMenuItem(string icon, string text, int y, bool active = false)
        {
            var pnl = new Panel
            {
                Size      = new Size(270, 34),
                Location  = new Point(0, y),
                BackColor = active
                    ? Color.FromArgb(38, 168, 230, 207)
                    : Color.Transparent,
                Cursor    = Cursors.Hand,
                Tag       = text
            };

            var borderLeft = new Panel
            {
                Size      = new Size(3, 34),
                Location  = new Point(0, 0),
                BackColor = active
                    ? Color.FromArgb(168, 230, 207)
                    : Color.Transparent,
            };
            pnl.Controls.Add(borderLeft);

            var lbl = new Label
            {
                Text      = "  " + icon + "   " + text,
                Font      = new Font("Segoe UI", 9f, active ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = active
                    ? Color.White
                    : Color.FromArgb(180, 255, 255, 255),
                BackColor = Color.Transparent,
                AutoSize  = false,
                Size      = new Size(250, 34),
                Location  = new Point(8, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor    = Cursors.Hand,
            };
            pnl.Controls.Add(lbl);

            Action<bool> setHover = (hover) =>
            {
                if (pnl == _menuHienTai?.Parent) return;
                pnl.BackColor        = hover ? Color.FromArgb(20, 168, 230, 207) : Color.Transparent;
                borderLeft.BackColor = hover ? Color.FromArgb(100, 168, 230, 207) : Color.Transparent;
                lbl.ForeColor        = hover ? Color.White : Color.FromArgb(180, 255, 255, 255);
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

            return y + 36;
        }

        // ══════════════════════════════════════════
        // SỰ KIỆN MENU
        // ══════════════════════════════════════════

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

            pnl.BackColor        = Color.FromArgb(38, 168, 230, 207);
            borderLeft.BackColor = Color.FromArgb(168, 230, 207);
            lbl.ForeColor        = Color.White;
            lbl.Font             = new Font("Segoe UI", 9f, FontStyle.Bold);

            _menuHienTai = new Guna2Button { Parent = pnl, Visible = false, Tag = pnl.Tag };

            string tenMenu = pnl.Tag?.ToString() ?? "";
            lblTopbarTitle.Text = tenMenu;
            lblBreadcrumb.Text  = "DermaSoft \u203A " + tenMenu;
            lblTopbarTitle.Location = new Point(lblBreadcrumb.Right + 10, lblTopbarTitle.Location.Y);

            MoFormCon(tenMenu);
        }

        // ══════════════════════════════════════════
        // MDI AREA
        // ══════════════════════════════════════════

        private void HienThiMdiMacDinh()
        {
            MoFormCon("Dashboard");
        }

        private void MoFormCon(string tenMenu)
        {
            pnlMdiArea.Controls.Clear();

            Form frm = null;
            switch (tenMenu)
            {
                case "Dashboard":          frm = new DashboardForm(); break;
                case "Nh\u00e2n Vi\u00ean":           frm = new StaffForm(); break;
                case "Ph\u00e2n C\u00f4ng Ca":         frm = new PhanCongCaForm(); break;
                case "Danh M\u1ee5c Thu\u1ed1c":      frm = new ThuocForm(); break;
                case "Nh\u1eadp Kho":             frm = new NhapKhoForm(); break;
                case "T\u1ed3n Kho":              frm = new TonKhoForm(); break;
                case "D\u1ecbch V\u1ee5":              frm = new ServiceForm(); break;
                case "B\u00e1o C\u00e1o Doanh Thu":    frm = new BaoCaoDoanhThuForm(); break;
                case "B\u00e1o C\u00e1o Kho":          frm = new BaoCaoKhoForm(); break;
                case "\u0110\u00e1nh Gi\u00e1":            frm = new DanhGiaForm(); break;
                case "C\u00e0i \u0110\u1eb7t":            frm = new SettingsForm(); break;
                case "H\u1ed3 S\u01a1 C\u00e1 Nh\u00e2n":       frm = new ProfileForm(); break;
                case "L\u1ecbch H\u1eb9n":            frm = new AppointmentForm(); break;
                case "Phi\u1ebfu Kh\u00e1m":          frm = new PhieuKhamForm(); break;
                case "B\u1ec7nh Nh\u00e2n":           frm = new PatientForm(); break;
            }

            if (frm != null)
            {
                frm.TopLevel = false;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Dock = DockStyle.Fill;
                pnlMdiArea.Controls.Add(frm);
                frm.Show();
            }
        }

        // ══════════════════════════════════════════
        // SỰ KIỆN DESIGNER
        // ══════════════════════════════════════════

        private void BtnDangXuat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "B\u1ea1n c\u00f3 ch\u1eafc ch\u1eafn mu\u1ed1n \u0111\u0103ng xu\u1ea5t?",
                "X\u00e1c nh\u1eadn \u0111\u0103ng xu\u1ea5t",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoginForm.NguoiDungHienTai = null;
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = this.WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        // ══════════════════════════════════════════
        // PAINT — Bo tròn avatar
        // ══════════════════════════════════════════

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
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.Clear(lbl.Parent.BackColor);
                    g.FillEllipse(brush, 0, 0, d - 1, d - 1);
                    g.DrawString(lbl.Text, lbl.Font, Brushes.White, new RectangleF(0, 0, d, d), sf);
                }
            };
            lbl.Invalidate();
        }

        private void lblUserRole_Click(object sender, EventArgs e)
        {

        }

        private void pnlTopbar_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
