using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    public partial class StaffForm : Form
    {
        // ── Controls chính ──
        private Panel pnlLeft;
        private Panel pnlRight;
        private DataGridView dgvNhanVien;
        private Guna2TextBox txtTimKiem;
        private Guna2ComboBox cboLocVaiTro;

        // ── Form bên phải ──
        private Guna2TextBox txtHoTen;
        private Guna2TextBox txtSDT;
        private Guna2TextBox txtEmail;
        private Guna2TextBox txtTenDangNhap;
        private Guna2TextBox txtMatKhau;
        private Guna2ComboBox cboVaiTro;
        private Guna2Button btnTaoTK;
        private Guna2Button btnResetMK;
        private Guna2Button btnKhoaTK;
        private Guna2Button btnXoaNV;
        private Label lblFormTitle;
        private Label lblError;
        private int _maNVDangChon = -1;

        // Màu wireframe
        private static readonly Color BorderInput = ColorTranslator.FromHtml("#D1E8DC");
        private static readonly Color InputBg = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color BadgeActiveBg = ColorTranslator.FromHtml("#DCFCE7");
        private static readonly Color BadgeActiveFg = ColorTranslator.FromHtml("#166534");
        private static readonly Color BadgeInactiveBg = ColorTranslator.FromHtml("#FEF2F2");
        private static readonly Color BadgeInactiveFg = ColorTranslator.FromHtml("#991B1B");
        private static readonly Color BadgeInfoBg = ColorTranslator.FromHtml("#DBEAFE");
        private static readonly Color BadgeInfoFg = ColorTranslator.FromHtml("#1E40AF");
        private static readonly Color BadgeWarningBg = ColorTranslator.FromHtml("#FEF3C7");
        private static readonly Color BadgeWarningFg = ColorTranslator.FromHtml("#D97706");
        private static readonly Color BadgeDangerBg = ColorTranslator.FromHtml("#FEE2E2");
        private static readonly Color BadgeDangerFg = ColorTranslator.FromHtml("#DC2626");
        private static readonly Color GoldAccent = Color.FromArgb(184, 138, 40);
        private static readonly Color RowAlt = ColorTranslator.FromHtml("#F5FBF7");
        private static readonly Color RowOdd = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color GridBorderColor = ColorTranslator.FromHtml("#EEF6F1");
        private static readonly Color NotifChipBg = ColorScheme.PrimaryPale;

        public StaffForm()
        {
            InitializeComponent();
            TaoBoCuc();
            LoadDanhSach();
        }

        // ══════════════════════════════════════════
        // BỐ CỤC CHÍNH — 2 cột: Trái (bảng) + Phải (form)
        // ══════════════════════════════════════════

        private void TaoBoCuc()
        {
            this.Padding = new Padding(20);
            this.BackColor = ColorScheme.Background;

            // ── Vùng nội dung ──
            var pnlBody = new Panel { Dock = DockStyle.Fill, BackColor = ColorScheme.Background };
            this.Controls.Add(pnlBody);

            // ── Panel phải — Form tạo/sửa (card style) ──
            pnlRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 420,
                BackColor = Color.Transparent,
                Padding = new Padding(16, 0, 0, 0),
            };

            // ── Panel trái — Bảng ──
            pnlLeft = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 0, 0, 0),
                BackColor = ColorScheme.Background,
            };

            // ── QUAN TRỌNG: thứ tự add quyết định dock layout ──
            // Control add SAU (index cao) được layout TRƯỚC
            // → Fill phải add TRƯỚC, Right phải add SAU
            pnlBody.Controls.Add(pnlLeft);     // index 0 — layout sau — Fill lấp phần còn lại
            pnlBody.Controls.Add(pnlRight);    // index 1 — layout trước — Right chiếm 380px bên phải

            TaoFormBenPhai();
            TaoPanelTrai();
        }

        // ══════════════════════════════════════════
        // PANEL TRÁI — Search + Filter + DataGridView
        // ══════════════════════════════════════════

        private void TaoPanelTrai()
        {
            // ═══════════════════════════════════════════════════════
            // FILTER ROW
            // ═══════════════════════════════════════════════════════
            var pnlFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = 42,
                BackColor = Color.Transparent,
            };

            // -- Wrapper panels cho từng Guna2 control --
            // Guna2 controls tự override Size khi add trực tiếp vào panel
            // → Bọc trong Panel con, dùng Dock.Top cho Guna2 bên trong
            var pnlSearchWrap = new Panel { BackColor = Color.Transparent };
            var pnlCboWrap = new Panel { BackColor = Color.Transparent };
            var pnlBtnWrap = new Panel { BackColor = Color.Transparent };

            var btnTaoMoi = new Guna2Button
            {
                Text = "➕ Tạo Tài Khoản",
                Font = AppFonts.BodyBold,
                ForeColor = Color.White,
                FillColor = ColorScheme.Primary,
                BorderRadius = 18,
                Dock = DockStyle.Top,
                Height = 36,
                Cursor = Cursors.Hand,
            };
            btnTaoMoi.Click += (s, e) => ResetForm();
            pnlBtnWrap.Controls.Add(btnTaoMoi);

            cboLocVaiTro = new Guna2ComboBox
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Dock = DockStyle.Top,
                Height = 36,
                BorderRadius = 8,
                BorderColor = BorderInput,
                FocusedState = { BorderColor = ColorScheme.Primary },
                HoverState = { BorderColor = ColorScheme.Primary },
                FillColor = InputBg,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            cboLocVaiTro.Items.AddRange(new object[] { "Tất cả vai trò", "Admin", "Bác Sĩ", "Lễ Tân", "Quản Kho" });
            cboLocVaiTro.SelectedIndex = 0;
            cboLocVaiTro.SelectedIndexChanged += (s, e) => LoadDanhSach();
            pnlCboWrap.Controls.Add(cboLocVaiTro);

            txtTimKiem = new Guna2TextBox
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Dock = DockStyle.Top,
                Height = 36,
                PlaceholderText = "🔍 Tìm theo tên, username, SĐT...",
                PlaceholderForeColor = ColorScheme.TextLight,
                BorderRadius = 18,
                BorderColor = BorderInput,
                FocusedState = { BorderColor = ColorScheme.Primary },
                HoverState = { BorderColor = ColorScheme.Primary },
                FillColor = Color.White,
                Cursor = Cursors.IBeam,
            };
            txtTimKiem.TextChanged += (s, e) => LoadDanhSach();
            pnlSearchWrap.Controls.Add(txtTimKiem);

            // Add wrapper panels vào pnlFilter
            pnlFilter.Controls.Add(pnlSearchWrap);
            pnlFilter.Controls.Add(pnlCboWrap);
            pnlFilter.Controls.Add(pnlBtnWrap);

            // Layout: SetBounds trên WRAPPER PANELS (standard Panel), không trên Guna2
            const int BTN_W = 200, CBO_W = 150, GAP = 10, H = 42;
            EventHandler boTriFilter = (s, e) =>
            {
                int pw = pnlFilter.ClientSize.Width;
                if (pw <= 0) return;
                pnlBtnWrap.SetBounds(pw - BTN_W, 0, BTN_W, H);
                pnlCboWrap.SetBounds(pw - BTN_W - GAP - CBO_W, 0, CBO_W, H);
                int searchW = Math.Max(50, pw - BTN_W - CBO_W - GAP * 2);
                pnlSearchWrap.SetBounds(0, 0, searchW, H);
            };
            pnlFilter.Resize += boTriFilter;
            pnlFilter.Layout += (s, e) => boTriFilter(s, e);

            // ═══════════════════════════════════════════════════════
            // Spacer giữa filter và DataGridView
            // ═══════════════════════════════════════════════════════
            var pnlSpacer = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // ── DataGridView ──
            dgvNhanVien = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = GridBorderColor,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = AppFonts.Body,
                RowTemplate = { Height = 40 },
            };
            dgvNhanVien.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorScheme.PrimaryDark,
                ForeColor = Color.White,
                Font = AppFonts.BodyBold,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvNhanVien.ColumnHeadersHeight = 40;
            dgvNhanVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvNhanVien.EnableHeadersVisualStyles = false;
            dgvNhanVien.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowOdd,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvNhanVien.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowAlt,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
            };

            // Header gradient
            dgvNhanVien.CellPainting += DgvNhanVien_CellPainting;

            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaNV", HeaderText = "Mã NV", FillWeight = 8 });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { Name = "HoTen", HeaderText = "Họ tên", FillWeight = 17 });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { Name = "Username", HeaderText = "Username", FillWeight = 11 });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { Name = "VaiTro", HeaderText = "Vai trò", FillWeight = 12 });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { Name = "SDT", HeaderText = "SĐT", FillWeight = 12 });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "Email", FillWeight = 14 });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { Name = "TrangThai", HeaderText = "Trạng thái", FillWeight = 11 });

            // Cột Thao tác (chứa Sửa + Khóa/Mở)
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThaoTac", HeaderText = "Thao tác", FillWeight = 15 });

            // Hidden
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaNguoiDung", Visible = false });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { Name = "IsActive", Visible = false });

            dgvNhanVien.CellClick += DgvNhanVien_CellClick;
            dgvNhanVien.CellFormatting += DgvNhanVien_CellFormatting;
            dgvNhanVien.CellMouseMove += DgvNhanVien_CellMouseMove;

            // ── Thêm controls vào pnlLeft ──
            // WinForms dock order: add Fill TRƯỚC, rồi Top SAU
            // Control add SAU sẽ nằm TRÊN CÙNG trong dock layout
            pnlLeft.Controls.Add(dgvNhanVien);       // Fill — chiếm phần còn lại
            pnlLeft.Controls.Add(pnlSpacer);          // Top — spacer 10px giữa filter và grid
            pnlLeft.Controls.Add(pnlFilter);           // Top — filter row nằm trên cùng
        }

        // ══════════════════════════════════════════
        // HEADER GRADIENT PAINT
        // ══════════════════════════════════════════

        private void DgvNhanVien_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1) return;

            e.Handled = true;
            var rect = e.CellBounds;

            using (var brush = new LinearGradientBrush(
                new Rectangle(0, rect.Y, dgvNhanVien.Width, rect.Height),
                ColorScheme.PrimaryDark, Color.FromArgb(180, GoldAccent.R, GoldAccent.G, GoldAccent.B),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            // Rounded corners cho cột đầu và cuối
            if (e.ColumnIndex == 0)
            {
                using (var br = new SolidBrush(ColorScheme.Background))
                {
                    e.Graphics.FillRectangle(br, rect.X, rect.Y, 8, 8);
                    using (var path = new GraphicsPath())
                    {
                        path.AddArc(rect.X, rect.Y, 16, 16, 180, 90);
                        path.AddLine(rect.X + 8, rect.Y, rect.X, rect.Y + 8);
                        path.CloseFigure();
                        using (var brush2 = new LinearGradientBrush(
                            new Rectangle(0, rect.Y, dgvNhanVien.Width, rect.Height),
                            ColorScheme.PrimaryDark, Color.FromArgb(180, GoldAccent.R, GoldAccent.G, GoldAccent.B),
                            LinearGradientMode.Horizontal))
                        {
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            e.Graphics.FillPath(brush2, path);
                        }
                    }
                }
            }

            // Text
            if (e.Value != null)
            {
                var textRect = new Rectangle(rect.X + 12, rect.Y, rect.Width - 12, rect.Height);
                TextRenderer.DrawText(e.Graphics, e.Value.ToString(), AppFonts.BodyBold,
                    textRect, Color.White, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            }
        }

        // ══════════════════════════════════════════
        // PANEL PHẢI — Form Tạo / Sửa Tài Khoản (card style)
        // ══════════════════════════════════════════

        private void TaoFormBenPhai()
        {
            // Tạo panel card bên trong pnlRight
            var pnlCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20),
            };
            pnlCard.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var r = new Rectangle(0, 0, pnlCard.Width - 1, pnlCard.Height - 1);
                using (var path = TaoRoundedRect(r, 12))
                using (var pen = new Pen(Color.FromArgb(40, GoldAccent), 1.2f))
                {
                    g.DrawPath(pen, path);
                }
            };
            pnlRight.Controls.Add(pnlCard);

            const int X = 16;
            int W = 368;
            int y = 12;

            // ── Title với gradient bar bên trái + border bottom ──
            var pnlTitleSection = new Panel { Location = new Point(X, y), Size = new Size(W, 36), BackColor = Color.Transparent };
            pnlTitleSection.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // Gradient bar bên trái
                using (var br = new LinearGradientBrush(new Rectangle(0, 2, 4, 16),
                    ColorScheme.PrimaryDark, GoldAccent, LinearGradientMode.Vertical))
                {
                    g.FillRectangle(br, 0, 4, 4, 18);
                }
                // Border bottom
                using (var pen = new Pen(ColorScheme.PrimaryPale, 1.5f))
                    g.DrawLine(pen, 0, 35, W, 35);
            };
            lblFormTitle = new Label
            {
                Text = "👤 Tạo / Sửa Tài Khoản",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = ColorScheme.PrimaryDark,
                Location = new Point(12, 4),
                Size = new Size(W - 28, 28),
                AutoSize = false,
                BackColor = Color.Transparent,
            };
            pnlTitleSection.Controls.Add(lblFormTitle);
            pnlCard.Controls.Add(pnlTitleSection);
            y += 48;

            // ── Họ và tên ──
            pnlCard.Controls.Add(TaoLabel("Họ và tên", new Point(X, y), true));
            y += 20;
            txtHoTen = TaoGuna2TextBox(new Point(X, y), new Size(W, 36), "Nguyễn Văn A");
            pnlCard.Controls.Add(txtHoTen);
            y += 42;

            // ── SĐT + Vai trò (2 cột) ──
            int halfW = (W - 10) / 2;
            pnlCard.Controls.Add(TaoLabel("SĐT", new Point(X, y), true));
            pnlCard.Controls.Add(TaoLabel("Vai trò", new Point(X + halfW + 10, y), true));
            y += 20;
            txtSDT = TaoGuna2TextBox(new Point(X, y), new Size(halfW, 36), "0901...");
            pnlCard.Controls.Add(txtSDT);
            cboVaiTro = new Guna2ComboBox
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Size = new Size(halfW, 36),
                Location = new Point(X + halfW + 10, y),
                BorderRadius = 8,
                BorderColor = BorderInput,
                FocusedState = { BorderColor = ColorScheme.Primary },
                HoverState = { BorderColor = ColorScheme.Primary },
                FillColor = InputBg,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            cboVaiTro.Items.AddRange(new object[] { "Admin", "Bác Sĩ", "Lễ Tân", "Quản Kho" });
            cboVaiTro.SelectedIndex = 1;
            pnlCard.Controls.Add(cboVaiTro);
            y += 42;

            // ── Email ──
            pnlCard.Controls.Add(TaoLabel("Email", new Point(X, y), true));
            y += 20;
            txtEmail = TaoGuna2TextBox(new Point(X, y), new Size(W, 36), "bsnv@darmaclinic.vn");
            pnlCard.Controls.Add(txtEmail);
            y += 42;

            // ── Tên đăng nhập ──
            var lblTenDN = new Label
            {
                Font = AppFonts.BodyBold,
                ForeColor = ColorScheme.PrimaryDark,
                Location = new Point(X, y),
                AutoSize = true,
                BackColor = Color.Transparent,
            };
            lblTenDN.Text = "Tên đăng nhập *";
            pnlCard.Controls.Add(lblTenDN);
            var lblSmall = new Label
            {
                Text = "(5 ký tự, auto-gen)",
                Font = AppFonts.Small,
                ForeColor = ColorScheme.TextLight,
                Location = new Point(X + 110, y + 2),
                AutoSize = true,
                BackColor = Color.Transparent,
            };
            pnlCard.Controls.Add(lblSmall);
            y += 20;
            txtTenDangNhap = TaoGuna2TextBox(new Point(X, y), new Size(W - 88, 36), "vdNam");
            pnlCard.Controls.Add(txtTenDangNhap);
            var btnGen = new Guna2Button
            {
                Text = "🎲 Tạo",
                Font = AppFonts.Small,
                ForeColor = ColorScheme.PrimaryDark,
                FillColor = ColorScheme.PrimaryPale,
                BorderColor = ColorScheme.PrimaryLight,
                BorderThickness = 1,
                BorderRadius = 18,
                Location = new Point(X + W - 80, y),
                Size = new Size(80, 36),
                Cursor = Cursors.Hand,
            };
            btnGen.Click += (s, e) => txtTenDangNhap.Text = SinhTenDangNhap();
            pnlCard.Controls.Add(btnGen);
            y += 42;

            // ── Mật khẩu tạm ──
            pnlCard.Controls.Add(TaoLabel("Mật khẩu tạm", new Point(X, y), true));
            y += 20;
            txtMatKhau = TaoGuna2TextBox(new Point(X, y), new Size(W - 88, 36), "");
            txtMatKhau.Text = AppSettings.MatKhauMacDinh;
            txtMatKhau.UseSystemPasswordChar = true;
            pnlCard.Controls.Add(txtMatKhau);
            var btnEye = new Guna2Button
            {
                Text = "🎲",
                Font = AppFonts.Small,
                ForeColor = ColorScheme.PrimaryDark,
                FillColor = ColorScheme.PrimaryPale,
                BorderColor = ColorScheme.PrimaryLight,
                BorderThickness = 1,
                BorderRadius = 18,
                Location = new Point(X + W - 76, y),
                Size = new Size(76, 36),
                Cursor = Cursors.Hand,
            };
            btnEye.Click += (s, e) =>
            {
                txtMatKhau.UseSystemPasswordChar = !txtMatKhau.UseSystemPasswordChar;
                btnEye.Text = txtMatKhau.UseSystemPasswordChar ? "👁" : "🙈";
            };
            btnEye.Text = "👁";
            pnlCard.Controls.Add(btnEye);
            y += 44;

            // ── Notif chip (wireframe: .notif-chip) ──
            var pnlNote = new Panel { Location = new Point(X, y), Size = new Size(W, 52), BackColor = Color.Transparent };
            pnlNote.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = TaoRoundedRect(new Rectangle(0, 0, pnlNote.Width - 1, pnlNote.Height - 1), 10))
                using (var fill = new SolidBrush(NotifChipBg))
                {
                    g.FillPath(fill, path);
                }
                // Left border accent
                using (var pen = new Pen(ColorScheme.Primary, 3f))
                    g.DrawLine(pen, 2, 4, 2, pnlNote.Height - 4);
            };
            pnlNote.Controls.Add(new Label
            {
                Text = "ℹ️ NV sẽ phải đổi MK khi đăng nhập lần đầu (DoiMatKhau=1)",
                Font = AppFonts.Small,
                ForeColor = ColorScheme.TextMid,
                Location = new Point(14, 6),
                Size = new Size(W - 28, 40),
                BackColor = Color.Transparent,
                AutoSize = false,
            });
            pnlCard.Controls.Add(pnlNote);
            y += 60;

            // Error
            lblError = new Label { Font = AppFonts.Small, ForeColor = ColorScheme.Danger, Location = new Point(X, y), Size = new Size(W, 18), Text = "" };
            pnlCard.Controls.Add(lblError);
            y += 22;

            // ── Nút Tạo Tài Khoản (primary gradient) ──
            btnTaoTK = new Guna2Button
            {
                Text = "💾 Tạo Tài Khoản",
                Font = AppFonts.BodyBold,
                ForeColor = Color.White,
                FillColor = ColorScheme.Primary,
                BorderRadius = 20,
                Location = new Point(X, y),
                Size = new Size(W, 38),
                Cursor = Cursors.Hand,
            };
            btnTaoTK.Click += BtnTaoTK_Click;
            pnlCard.Controls.Add(btnTaoTK);
            y += 46;

            // ── Nút Reset Mật Khẩu (warning) ──
            btnResetMK = new Guna2Button
            {
                Text = "🔒 Reset Mật Khẩu",
                Font = AppFonts.BodyBold,
                ForeColor = Color.White,
                FillColor = ColorScheme.Warning,
                BorderRadius = 20,
                Location = new Point(X, y),
                Size = new Size(W, 34),
                Cursor = Cursors.Hand,
                Enabled = false,
                DisabledState = { FillColor = Color.FromArgb(180, ColorScheme.Warning), ForeColor = Color.FromArgb(180, Color.White) },
            };
            btnResetMK.Click += BtnResetMK_Click;
            pnlCard.Controls.Add(btnResetMK);
            y += 42;

            // ── Nút Khóa Tài Khoản (danger) ──
            btnKhoaTK = new Guna2Button
            {
                Text = "🚫 Khóa Tài Khoản",
                Font = AppFonts.BodyBold,
                ForeColor = Color.White,
                FillColor = ColorScheme.Danger,
                BorderRadius = 20,
                Location = new Point(X, y),
                Size = new Size(W, 34),
                Cursor = Cursors.Hand,
                Enabled = false,
                DisabledState = { FillColor = Color.FromArgb(180, ColorScheme.Danger), ForeColor = Color.FromArgb(180, Color.White) },
            };
            btnKhoaTK.Click += BtnKhoaTK_Click;
            pnlCard.Controls.Add(btnKhoaTK);
            y += 42;

            // ── Nút Xóa Nhân Viên (soft delete) ──
            btnXoaNV = new Guna2Button
            {
                Text = "🗑️ Xóa Nhân Viên",
                Font = AppFonts.BodyBold,
                ForeColor = BadgeDangerFg,
                FillColor = BadgeDangerBg,
                BorderColor = ColorScheme.Danger,
                BorderThickness = 1,
                BorderRadius = 20,
                Location = new Point(X, y),
                Size = new Size(W, 34),
                Cursor = Cursors.Hand,
                Enabled = false,
                DisabledState = { FillColor = Color.FromArgb(240, 240, 240), ForeColor = Color.FromArgb(180, 180, 180), BorderColor = Color.FromArgb(200, 200, 200) },
            };
            btnXoaNV.Click += BtnXoaNV_Click;
            pnlCard.Controls.Add(btnXoaNV);
        }

        // ══════════════════════════════════════════
        // LOAD DỮ LIỆU
        // ══════════════════════════════════════════

        private void LoadDanhSach()
        {
            dgvNhanVien.Rows.Clear();
            string keyword = txtTimKiem?.Text?.Trim() ?? "";
            // Mapping: cboLocVaiTro index → MaVaiTro (0=Tất cả, 1=Admin, 2=BacSi, 3=LeTan, 4=QuanKho)
            int[] filterMap = { 0, 1, 2, 3, 5 };
            int filterVT = filterMap[Math.Min(cboLocVaiTro?.SelectedIndex ?? 0, filterMap.Length - 1)];

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    @"SELECT nd.MaNguoiDung, nd.HoTen, nd.TenDangNhap, vt.TenVaiTro,
                             nd.SoDienThoai, nd.Email, nd.TrangThaiTK, nd.MaVaiTro
                      FROM NguoiDung nd
                      JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
                      WHERE nd.IsDeleted = 0
                        AND nd.MaVaiTro IN (1, 2, 3, 5)
                        AND (@Keyword = '' OR nd.HoTen LIKE '%' + @Keyword + '%' 
                             OR nd.TenDangNhap LIKE '%' + @Keyword + '%'
                             OR nd.SoDienThoai LIKE '%' + @Keyword + '%')
                        AND (@VaiTro = 0 OR nd.MaVaiTro = @VaiTro)
                      ORDER BY nd.MaVaiTro, nd.HoTen", conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", keyword);
                    cmd.Parameters.AddWithValue("@VaiTro", filterVT);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int ma = Convert.ToInt32(reader.GetValue(0));
                            string hoTen = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            string username = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            string vaiTro = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            string sdt = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            string email = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            bool active = !reader.IsDBNull(6) && Convert.ToBoolean(reader.GetValue(6));
                            int maVT = Convert.ToInt32(reader.GetValue(7));

                            string maNV = "NV" + ma.ToString("D3");
                            string ttText = active ? "Hoạt động" : "Đã khóa";

                            // Badge text cho vai trò
                            string vaiTroBadge;
                            if (maVT == 1) vaiTroBadge = "🛡️ Admin";
                            else if (maVT == 2) vaiTroBadge = "🩺 Bác Sĩ";
                            else if (maVT == 3) vaiTroBadge = "🏥 Lễ Tân";
                            else if (maVT == 5) vaiTroBadge = "📦 Quản Kho";
                            else vaiTroBadge = vaiTro;

                            int idx = dgvNhanVien.Rows.Add(maNV, hoTen, username, vaiTroBadge, sdt, email, ttText, "");
                            dgvNhanVien.Rows[idx].Cells["MaNguoiDung"].Value = ma;
                            dgvNhanVien.Rows[idx].Cells["IsActive"].Value = active;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════
        // CELL FORMATTING — Badge style
        // ══════════════════════════════════════════

        private void DgvNhanVien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvNhanVien.Columns[e.ColumnIndex].Name;

            if (colName == "HoTen")
            {
                e.CellStyle.Font = AppFonts.BodyBold;
                e.CellStyle.ForeColor = ColorScheme.TextDark;
            }

            if (colName == "VaiTro" && e.Value != null)
            {
                string v = e.Value.ToString();
                if (v.Contains("Admin"))
                {
                    e.CellStyle.BackColor = BadgeDangerBg;
                    e.CellStyle.ForeColor = BadgeDangerFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else if (v.Contains("Bác") || v.Contains("Bac"))
                {
                    e.CellStyle.BackColor = BadgeInfoBg;
                    e.CellStyle.ForeColor = BadgeInfoFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else if (v.Contains("Kho"))
                {
                    e.CellStyle.BackColor = BadgeActiveBg;
                    e.CellStyle.ForeColor = BadgeActiveFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else
                {
                    e.CellStyle.BackColor = BadgeWarningBg;
                    e.CellStyle.ForeColor = BadgeWarningFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
            }

            if (colName == "TrangThai" && e.Value != null)
            {
                bool active = e.Value.ToString().Contains("Hoạt");
                e.CellStyle.BackColor = active ? BadgeActiveBg : BadgeInactiveBg;
                e.CellStyle.ForeColor = active ? BadgeActiveFg : BadgeInactiveFg;
                e.CellStyle.Font = AppFonts.Badge;
            }

            if (colName == "ThaoTac")
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.SelectionBackColor = ColorScheme.PrimaryPale;
            }
        }

        // ══════════════════════════════════════════
        // CELL CLICK — Sửa / Khóa-Mở (dựa vào vùng click trong cột ThaoTac)
        // ══════════════════════════════════════════

        private void DgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int maNV = Convert.ToInt32(dgvNhanVien.Rows[e.RowIndex].Cells["MaNguoiDung"].Value);
            string colName = dgvNhanVien.Columns[e.ColumnIndex].Name;

            if (colName == "ThaoTac")
            {
                // Xác định click vào nút nào dựa trên vị trí X
                var cellRect = dgvNhanVien.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                var mousePos = dgvNhanVien.PointToClient(Cursor.Position);
                int relX = mousePos.X - cellRect.X;
                bool isActive = Convert.ToBoolean(dgvNhanVien.Rows[e.RowIndex].Cells["IsActive"].Value);

                if (relX < cellRect.Width / 2)
                {
                    // Nút "Sửa"
                    LoadThongTinVaoForm(maNV);
                }
                else
                {
                    // Nút "Khóa" hoặc "Mở"
                    string hoTen = dgvNhanVien.Rows[e.RowIndex].Cells["HoTen"].Value?.ToString() ?? "";
                    if (isActive) KhoaTaiKhoan(maNV, hoTen);
                    else MoKhoaTaiKhoan(maNV, hoTen);
                }
            }
            else
            {
                // Click vào dòng bất kỳ → load vào form
                LoadThongTinVaoForm(maNV);
            }
        }

        private void DgvNhanVien_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvNhanVien.Columns[e.ColumnIndex].Name;
            dgvNhanVien.Cursor = colName == "ThaoTac" ? Cursors.Hand : Cursors.Default;
        }

        // ══════════════════════════════════════════
        // LOAD THÔNG TIN VÀO FORM PHẢI
        // ══════════════════════════════════════════

        private void LoadThongTinVaoForm(int maNV)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    "SELECT HoTen, SoDienThoai, Email, MaVaiTro, TenDangNhap, TrangThaiTK FROM NguoiDung WHERE MaNguoiDung = @Ma", conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", maNV);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtHoTen.Text = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            txtSDT.Text = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            txtEmail.Text = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            int vt = Convert.ToInt32(reader.GetValue(3));
                            // Reverse mapping: MaVaiTro → cboVaiTro index
                            int vtIdx = 0;
                            if (vt == 1) vtIdx = 0;
                            else if (vt == 2) vtIdx = 1;
                            else if (vt == 3) vtIdx = 2;
                            else if (vt == 5) vtIdx = 3;
                            cboVaiTro.SelectedIndex = vtIdx;
                            txtTenDangNhap.Text = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            txtMatKhau.Text = ""; // Xóa mật khẩu — để trống = không đổi MK
                            bool active = Convert.ToBoolean(reader.GetValue(5));

                            _maNVDangChon = maNV;
                            lblFormTitle.Text = "✏️ Sửa Tài Khoản — NV" + maNV.ToString("D3");
                            btnTaoTK.Text = "💾  Cập Nhật";
                            btnResetMK.Enabled = true;
                            btnKhoaTK.Enabled = true;
                            btnXoaNV.Enabled = true;
                            btnKhoaTK.Text = active ? "🚫  Khóa Tài Khoản" : "🔓  Mở Khóa";
                            btnKhoaTK.FillColor = active ? ColorScheme.Danger : ColorScheme.Info;
                            lblError.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex) { lblError.Text = ex.Message; }
        }

        private void ResetForm()
        {
            _maNVDangChon = -1;
            txtHoTen.Text = "";
            txtSDT.Text = "";
            txtEmail.Text = "";
            txtTenDangNhap.Text = SinhTenDangNhap();
            txtMatKhau.Text = AppSettings.MatKhauMacDinh;
            cboVaiTro.SelectedIndex = 1;
            lblFormTitle.Text = "👤 Tạo Tài Khoản Mới";
            btnTaoTK.Text = "💾  Tạo Tài Khoản";
            btnResetMK.Enabled = false;
            btnKhoaTK.Enabled = false;
            btnKhoaTK.Text = "🚫  Khóa Tài Khoản";
            btnKhoaTK.FillColor = ColorScheme.Danger;
            btnXoaNV.Enabled = false;
            lblError.Text = "";
            txtHoTen.Focus();
        }

        // ══════════════════════════════════════════
        // NÚT TẠO / CẬP NHẬT
        // ══════════════════════════════════════════

        private void BtnTaoTK_Click(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string email = txtEmail.Text.Trim();
            string tenDN = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();
            // Mapping: cboVaiTro index → MaVaiTro (0=Admin, 1=BacSi, 2=LeTan, 3=QuanKho)
            int[] vaiTroMap = { 1, 2, 3, 5 };
            int maVaiTro = vaiTroMap[cboVaiTro.SelectedIndex];

            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(sdt))
            {
                lblError.Text = "Vui lòng nhập họ tên và SĐT.";
                return;
            }

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    if (_maNVDangChon == -1)
                    {
                        // TẠO MỚI
                        if (string.IsNullOrEmpty(tenDN) || tenDN.Length != 5)
                        {
                            lblError.Text = "Tên đăng nhập phải đúng 5 ký tự.";
                            return;
                        }
                        // Hash mật khẩu bằng BCrypt trước khi lưu vào DB
                        string matKhauHash = BCrypt.Net.BCrypt.HashPassword(matKhau, workFactor: 10);
                        using (var cmd = new SqlCommand("SP_TaoTaiKhoanNguoiDung", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@HoTen", hoTen);
                            cmd.Parameters.AddWithValue("@SoDienThoai", sdt);
                            cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                            cmd.Parameters.AddWithValue("@TenDangNhap", tenDN);
                            cmd.Parameters.AddWithValue("@MatKhau", matKhauHash);
                            cmd.Parameters.AddWithValue("@MaVaiTro", maVaiTro);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Tạo tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // CẬP NHẬT — build SQL động: luôn cập nhật HoTen, SĐT, Email, VaiTro
                        // + TenDangNhap nếu có nhập + MatKhau nếu có nhập mới
                        string sqlUpdate = @"UPDATE NguoiDung 
                              SET HoTen = @HoTen, 
                                  SoDienThoai = @SoDienThoai, 
                                  Email = @Email,
                                  MaVaiTro = @MaVaiTro";

                        if (!string.IsNullOrEmpty(tenDN))
                            sqlUpdate += ", TenDangNhap = @TenDangNhap";

                        if (!string.IsNullOrEmpty(matKhau))
                            sqlUpdate += ", MatKhau = @MatKhau, DoiMatKhau = 1";

                        sqlUpdate += " WHERE MaNguoiDung = @MaNguoiDung";

                        using (var cmd = new SqlCommand(sqlUpdate, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaNguoiDung", _maNVDangChon);
                            cmd.Parameters.AddWithValue("@HoTen", hoTen);
                            cmd.Parameters.AddWithValue("@SoDienThoai", sdt);
                            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value =
                                string.IsNullOrEmpty(email) ? (object)DBNull.Value : email;
                            cmd.Parameters.AddWithValue("@MaVaiTro", maVaiTro);

                            if (!string.IsNullOrEmpty(tenDN))
                                cmd.Parameters.AddWithValue("@TenDangNhap", tenDN);

                            if (!string.IsNullOrEmpty(matKhau))
                            {
                                string matKhauHash = BCrypt.Net.BCrypt.HashPassword(matKhau, workFactor: 10);
                                cmd.Parameters.AddWithValue("@MatKhau", matKhauHash);
                            }

                            int rows = cmd.ExecuteNonQuery();
                            if (rows == 0)
                            {
                                lblError.Text = "Không tìm thấy nhân viên để cập nhật.";
                                return;
                            }
                        }
                        MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                ResetForm();
                LoadDanhSach();
            }
            catch (SqlException ex) { lblError.Text = ex.Message; }
        }

        // ══════════════════════════════════════════
        // RESET MẬT KHẨU
        // ══════════════════════════════════════════

        private void BtnResetMK_Click(object sender, EventArgs e)
        {
            if (_maNVDangChon == -1) return;
            var result = MessageBox.Show("Reset mật khẩu về mặc định?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try
            {
                // Hash mật khẩu mặc định bằng BCrypt trước khi lưu vào DB
                string mkHash = BCrypt.Net.BCrypt.HashPassword(AppSettings.MatKhauMacDinh, workFactor: 10);
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    "UPDATE NguoiDung SET MatKhau = @MK, DoiMatKhau = 1 WHERE MaNguoiDung = @Ma", conn))
                {
                    cmd.Parameters.AddWithValue("@MK", mkHash);
                    cmd.Parameters.AddWithValue("@Ma", _maNVDangChon);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Reset mật khẩu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex) { lblError.Text = ex.Message; }
        }

        // ══════════════════════════════════════════
        // KHÓA / MỞ KHÓA
        // ══════════════════════════════════════════

        private void BtnKhoaTK_Click(object sender, EventArgs e)
        {
            if (_maNVDangChon == -1) return;
            // Fix: kiểm tra chính xác hơn — tài khoản đang ACTIVE thì mới khóa
            bool dangActive = btnKhoaTK.Text.Contains("Khóa Tài Khoản");

            if (dangActive)
                KhoaTaiKhoan(_maNVDangChon, txtHoTen.Text);
            else
                MoKhoaTaiKhoan(_maNVDangChon, txtHoTen.Text);
        }

        private void KhoaTaiKhoan(int maNV, string hoTen)
        {
            var result = MessageBox.Show("Khóa tài khoản \"" + hoTen + "\"?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand("SP_KhoaTaiKhoanNguoiDung", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNguoiDung", maNV);
                    cmd.ExecuteNonQuery();
                }
                ResetForm();
                LoadDanhSach();
            }
            catch (SqlException ex) { MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void MoKhoaTaiKhoan(int maNV, string hoTen)
        {
            var result = MessageBox.Show("Mở khóa tài khoản \"" + hoTen + "\"?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand("SP_MoTaiKhoanNguoiDung", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNguoiDung", maNV);
                    cmd.ExecuteNonQuery();
                }
                ResetForm();
                LoadDanhSach();
            }
            catch (SqlException ex) { MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ══════════════════════════════════════════
        // XÓA NHÂN VIÊN (SOFT DELETE)
        // ══════════════════════════════════════════

        private void BtnXoaNV_Click(object sender, EventArgs e)
        {
            if (_maNVDangChon == -1) return;

            // Không cho xóa chính mình
            var currentUser = LoginForm.NguoiDungHienTai;
            if (currentUser != null && currentUser.MaNguoiDung == _maNVDangChon)
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập!",
                    "Không được phép", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Xóa nhân viên \"{txtHoTen.Text}\"?\n\nNhân viên sẽ bị ẩn khỏi hệ thống (có thể khôi phục từ DB).",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    "UPDATE NguoiDung SET IsDeleted = 1, TrangThaiTK = 0 WHERE MaNguoiDung = @Ma", conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", _maNVDangChon);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Đã xóa nhân viên thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                LoadDanhSach();
            }
            catch (SqlException ex) { MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ══════════════════════════════════════════
        // HELPERS
        // ══════════════════════════════════════════

        private Label TaoLabel(string text, Point loc, bool required = false)
        {
            return new Label
            {
                Text = required ? text + " *" : text,
                Font = AppFonts.BodyBold,
                ForeColor = ColorScheme.PrimaryDark,
                Location = loc,
                AutoSize = true,
                BackColor = Color.Transparent,
            };
        }

        private Guna2TextBox TaoGuna2TextBox(Point loc, Size size, string placeholder)
        {
            return new Guna2TextBox
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Location = loc,
                Size = size,
                PlaceholderText = placeholder,
                PlaceholderForeColor = ColorScheme.TextLight,
                BorderRadius = 8,
                BorderColor = BorderInput,
                FocusedState = { BorderColor = ColorScheme.Primary },
                HoverState = { BorderColor = ColorScheme.Primary },
                FillColor = InputBg,
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

        private string SinhTenDangNhap()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var rnd = new Random();
            var result = new char[5];
            for (int i = 0; i < 5; i++)
                result[i] = chars[rnd.Next(chars.Length)];
            return new string(result);
        }
    }
}
