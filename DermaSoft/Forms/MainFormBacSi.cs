using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    public partial class MainFormBacSi : Form
    {
        private Guna2Button _menuHienTai = null;

        public MainFormBacSi()
        {
            InitializeComponent();

            if (this.DesignMode ||
                System.ComponentModel.LicenseManager.UsageMode ==
                System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            CaiDatThongTinNguoiDung();
            TaoMenuSidebar();
            CapNhatNgayGio();
            HienThiMdiMacDinh();
        }

        private void CaiDatThongTinNguoiDung()
        {
            var nd = LoginForm.NguoiDungHienTai;
            if (nd == null) return;

            string chuCaiDau = string.IsNullOrEmpty(nd.HoTen) ? "B"
                : nd.HoTen.Substring(0, 1).ToUpper();

            lblTopbarAvatar.Text = chuCaiDau;
            lblUserName.Text = nd.HoTen;
            lblUserRole.Text = "🩺 Bác Sĩ";

            btnDangXuat.Click += BtnDangXuat_Click;
        }

        private void CapNhatNgayGio()
        {
            lblNgayGio.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");
            var timer = new Timer { Interval = 60_000 };
            timer.Tick += (s, e) => lblNgayGio.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");
            timer.Start();
        }

        private void TaoMenuSidebar()
        {
            pnlSidebarNav.Controls.Clear();
            int y = 4;

            y = ThemSectionLabel("TỔNG QUAN", y);
            y = ThemMenuItem("📊", "Dashboard", y, active: true);

            y = ThemSectionLabel("NGHIỆP VỤ", y);
            y = ThemMenuItem("👥", "Hồ Sơ Bệnh Nhân", y);
            y = ThemMenuItem("📋", "Phiếu Khám Bệnh", y);

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
                borderLeft.BackColor = hover ? Color.FromArgb(80, 168, 230, 207) : Color.Transparent;
                lbl.ForeColor = hover ? Color.White : Color.FromArgb(180, 255, 255, 255);
            };

            pnl.MouseEnter += (s, e) => setHover(true);
            pnl.MouseLeave += (s, e) => setHover(false);
            lbl.MouseEnter += (s, e) => setHover(true);
            lbl.MouseLeave += (s, e) => setHover(false);
            pnl.Click += (s, e) => MenuItem_Click(pnl, borderLeft, lbl);
            lbl.Click += (s, e) => MenuItem_Click(pnl, borderLeft, lbl);
            borderLeft.Click += (s, e) => MenuItem_Click(pnl, borderLeft, lbl);

            pnlSidebarNav.Controls.Add(pnl);
            return y + 42;
        }

        private void MenuItem_Click(Panel pnl, Panel borderLeft, Label lbl)
        {
            if (_menuHienTai?.Parent != null)
            {
                var oldPnl = _menuHienTai.Parent;
                oldPnl.BackColor = Color.Transparent;
                foreach (Control c in oldPnl.Controls)
                {
                    if (c is Panel p && p.Width == 3) p.BackColor = Color.Transparent;
                    if (c is Label l) { l.ForeColor = Color.FromArgb(180, 255, 255, 255); l.Font = new Font("Segoe UI", 9f); }
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

        private void HienThiMdiMacDinh() => MoFormCon("Dashboard");

        private void MoFormCon(string tenMenu)
        {
            pnlMdiArea.Controls.Clear();

            if (tenMenu == "Hồ Sơ Bệnh Nhân")
            {
                HienPanelTimKiemBenhNhan();
                return;
            }

            Form frm = null;
            switch (tenMenu)
            {
                case "Dashboard": frm = new DashboardBacSiForm(); break;
                case "Phiếu Khám Bệnh": frm = new PhieuKhamForm(); break;
                case "Hồ Sơ Cá Nhân": frm = new ProfileForm(); break;
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

        // ══════════════════════════════════════════════════════════════════════
        // PANEL TÌM KIẾM BỆNH NHÂN
        // ══════════════════════════════════════════════════════════════════════
        private DataGridView _dgvTimKiem;

        private void HienPanelTimKiemBenhNhan()
        {
            var pnlWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(246, 249, 247),
                Padding = new Padding(24)
            };

            var lblTitle = new Label
            {
                Text = "👥  Tìm Kiếm Hồ Sơ Bệnh Nhân",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = ColorScheme.PrimaryDark,
                AutoSize = true,
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };
            pnlWrapper.Controls.Add(lblTitle);

            var lblHint = new Label
            {
                Text = "Nhập tên hoặc số điện thoại rồi nhấn Tìm kiếm (hoặc Enter). Double-click dòng để mở hồ sơ.",
                Font = new Font("Segoe UI", 9F),
                ForeColor = ColorScheme.TextGray,
                AutoSize = true,
                Location = new Point(0, 36),
                BackColor = Color.Transparent
            };
            pnlWrapper.Controls.Add(lblHint);

            // ── Thanh tìm kiếm (FlowLayoutPanel đảm bảo gap 20px chắc chắn) ──
            var flpSearch = new FlowLayoutPanel
            {
                Location = new Point(0, 72),
                Size = new Size(650, 50),
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = false
            };

            var txtSearch = new Guna2TextBox
            {
                Size = new Size(390, 44),
                Margin = new Padding(0, 0, 20, 0),
                PlaceholderText = "Nhập tên hoặc SĐT...",
                Font = new Font("Segoe UI", 9F),
                BorderRadius = 8,
                BorderColor = Color.FromArgb(46, 139, 87)
            };

            var btnSearch = new Guna2Button
            {
                Size = new Size(130, 44),
                Margin = new Padding(0, 0, 0, 0),
                Text = "Tìm kiếm",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                FillColor = ColorScheme.PrimaryDark,
                ForeColor = Color.White,
                BorderRadius = 8
            };

            flpSearch.Controls.Add(txtSearch);
            flpSearch.Controls.Add(btnSearch);
            pnlWrapper.Controls.Add(flpSearch);

            // ── DataGridView kết quả ──────────────────────────────────────
            _dgvTimKiem = new DataGridView
            {
                Location = new Point(0, 140),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
                                              | AnchorStyles.Right | AnchorStyles.Bottom,
                Size = new Size(pnlWrapper.Width - 48, pnlWrapper.Height - 148),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersHeight = 40,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(226, 237, 232),
                ReadOnly = true,
                RowHeadersVisible = false,
                RowTemplate = { Height = 38 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9.5F),
                Cursor = Cursors.Hand
            };

            // [FIX] Header đồng màu (15,92,77) toàn bộ — thêm SelectionBackColor
            var headerStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(15, 92, 77),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                SelectionBackColor = Color.FromArgb(15, 92, 77),
                SelectionForeColor = Color.White
            };
            _dgvTimKiem.ColumnHeadersDefaultCellStyle = headerStyle;

            _dgvTimKiem.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ColorScheme.TextDark,
                Font = new Font("Segoe UI", 9.5F),
                SelectionBackColor = Color.FromArgb(221, 245, 229),
                SelectionForeColor = ColorScheme.PrimaryDark
            };
            _dgvTimKiem.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(240, 250, 245)
            };

            _dgvTimKiem.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaBenhNhan", HeaderText = "Mã BN", Name = "colMaBN", Visible = false });
            _dgvTimKiem.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaBNCode", HeaderText = "Mã BN", Name = "colMaBNCode", FillWeight = 10F, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            _dgvTimKiem.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HoTen", HeaderText = "Họ Tên", Name = "colHoTen", FillWeight = 30F, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            _dgvTimKiem.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoDienThoai", HeaderText = "Số Điện Thoại", Name = "colSDT", FillWeight = 18F, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            _dgvTimKiem.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GioiTinh", HeaderText = "Giới Tính", Name = "colGioiTinh", FillWeight = 12F, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            _dgvTimKiem.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NgaySinh", HeaderText = "Ngày Sinh", Name = "colNgaySinh", FillWeight = 15F, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            _dgvTimKiem.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLanKham", HeaderText = "Số Lần Khám", Name = "colSoLanKham", FillWeight = 12F, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            _dgvTimKiem.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HangTV", HeaderText = "Hạng TV", Name = "colHangTV", FillWeight = 13F, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            // [FIX] Double-click hoạt động ngay — không cần nhấn Tìm trước
            _dgvTimKiem.CellDoubleClick += (s, ev) =>
            {
                if (ev.RowIndex < 0) return;
                int maBN = Convert.ToInt32(_dgvTimKiem.Rows[ev.RowIndex].Cells["colMaBN"].Value);

                pnlMdiArea.Controls.Clear();
                var detailForm = new BenhNhanDetailForm(maBN);
                detailForm.TopLevel = false;
                detailForm.FormBorderStyle = FormBorderStyle.None;
                detailForm.Dock = DockStyle.Fill;
                pnlMdiArea.Controls.Add(detailForm);
                detailForm.Show();

                lblBreadcrumb.Text = "DermaSoft › Hồ Sơ Bệnh Nhân";
            };

            pnlWrapper.Controls.Add(_dgvTimKiem);

            // Resize handler
            pnlWrapper.Resize += (s, ev) =>
            {
                _dgvTimKiem.Size = new Size(
                    pnlWrapper.Width - 48,
                    pnlWrapper.Height - 164);
            };

            // Search logic
            Action timKiem = () => TimKiemBenhNhan(txtSearch.Text.Trim());
            btnSearch.Click += (s, ev) => timKiem();
            txtSearch.KeyDown += (s, ev) => { if (ev.KeyCode == Keys.Enter) timKiem(); };

            // [FIX] Load tất cả ngay khi panel mở — double-click khả dụng liền
            TimKiemBenhNhan("");

            pnlMdiArea.Controls.Add(pnlWrapper);
        }

        private void TimKiemBenhNhan(string keyword)
        {
            try
            {
                const string sql = @"
                    SELECT
                        bn.MaBenhNhan,
                        'BN' + RIGHT('000' + CAST(bn.MaBenhNhan AS VARCHAR), 3) AS MaBNCode,
                        bn.HoTen,
                        bn.SoDienThoai,
                        CASE bn.GioiTinh
                            WHEN 1 THEN N'Nam'
                            WHEN 0 THEN N'Nữ'
                            ELSE    N'—'
                        END AS GioiTinh,
                        ISNULL(CONVERT(VARCHAR, bn.NgaySinh, 103), N'—') AS NgaySinh,
                        ISNULL(tvi.SoLanKham, 0)                         AS SoLanKham,
                        ISNULL(htv.TenHang, N'Chưa đăng ký')             AS HangTV
                    FROM BenhNhan bn
                    LEFT JOIN ThanhVienInfo tvi ON bn.MaBenhNhan = tvi.MaBenhNhan
                    LEFT JOIN HangThanhVien htv ON tvi.MaHang    = htv.MaHang
                    WHERE bn.IsDeleted = 0
                      AND (
                            @Keyword = ''
                            OR bn.HoTen       LIKE '%' + @Keyword + '%'
                            OR bn.SoDienThoai LIKE '%' + @Keyword + '%'
                          )
                    ORDER BY bn.HoTen ASC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@Keyword", keyword ?? ""));

                if (_dgvTimKiem != null)
                    _dgvTimKiem.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm bệnh nhân: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        private void BtnDangXuat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?",
                "Đăng Xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            LoginForm.NguoiDungHienTai = null;
            this.Hide();
            new LoginForm().Show();
            this.Close();
        }

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
                using (var sf = new System.Drawing.StringFormat
                {
                    Alignment = System.Drawing.StringAlignment.Center,
                    LineAlignment = System.Drawing.StringAlignment.Center
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

        private void BtnMinimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;
        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = this.WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal : FormWindowState.Maximized;
        }
        private void BtnClose_Click(object sender, EventArgs e) => this.Close();
    }
}