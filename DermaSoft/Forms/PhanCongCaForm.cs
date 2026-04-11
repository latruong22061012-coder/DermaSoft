using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    public partial class PhanCongCaForm : Form
    {
        // ── Controls chính ──
        private Panel pnlLeft;
        private Panel pnlRight;
        private DataGridView dgvPhanCong;

        // ── Filter row ──
        private Guna2DateTimePicker dtpTuanHienTai;
        private Guna2ComboBox cboLocNhanVien;
        private Guna2ComboBox cboXemTheo;
        private Label lblTieuDeTuan;

        // ── Form bên phải ──
        private Guna2ComboBox cboNhanVien;
        private Guna2ComboBox cboCaLam;
        private Guna2DateTimePicker dtpNgayLam;
        private Guna2Button btnLuu;
        private Guna2Button btnXoa;
        private Label lblFormTitle;
        private Label lblError;
        private int _maPhanCongDangChon = -1;

        // Màu wireframe (đồng bộ StaffForm)
        private static readonly Color BorderInput = ColorTranslator.FromHtml("#D1E8DC");
        private static readonly Color InputBg = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color BadgeActiveBg = ColorTranslator.FromHtml("#DCFCE7");
        private static readonly Color BadgeActiveFg = ColorTranslator.FromHtml("#166534");
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

        // Cache dữ liệu combo
        private List<KeyValuePair<int, string>> _dsNhanVien = new List<KeyValuePair<int, string>>();
        private List<KeyValuePair<int, string>> _dsCaLam = new List<KeyValuePair<int, string>>();

        public PhanCongCaForm()
        {
            InitializeComponent();
            TaoBoCuc();
            LoadComboData();
            LoadDanhSach();
        }

        // ══════════════════════════════════════════
        // BỐ CỤC CHÍNH — 2 cột: Trái (bảng) + Phải (form)
        // ══════════════════════════════════════════

        private void TaoBoCuc()
        {
            this.Padding = new Padding(20);
            this.BackColor = ColorScheme.Background;

            var pnlBody = new Panel { Dock = DockStyle.Fill, BackColor = ColorScheme.Background };
            this.Controls.Add(pnlBody);

            pnlRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 380,
                BackColor = Color.Transparent,
                Padding = new Padding(16, 0, 0, 0),
            };

            pnlLeft = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0),
                BackColor = ColorScheme.Background,
            };

            pnlBody.Controls.Add(pnlLeft);
            pnlBody.Controls.Add(pnlRight);

            TaoFormBenPhai();
            TaoPanelTrai();
        }

        // ══════════════════════════════════════════
        // PANEL TRÁI — Filter + DataGridView
        // ══════════════════════════════════════════

        private void TaoPanelTrai()
        {
            var pnlFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = 42,
                BackColor = Color.Transparent,
            };

            var pnlDateWrap = new Panel { BackColor = Color.Transparent };
            var pnlXemTheoWrap = new Panel { BackColor = Color.Transparent };
            var pnlCboWrap = new Panel { BackColor = Color.Transparent };
            var pnlBtnWrap = new Panel { BackColor = Color.Transparent };

            var btnTaoMoi = new Guna2Button
            {
                Text = "➕ Phân Công Mới",
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

            cboLocNhanVien = new Guna2ComboBox
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
            cboLocNhanVien.Items.Add("Tất cả nhân viên");
            cboLocNhanVien.SelectedIndex = 0;
            cboLocNhanVien.SelectedIndexChanged += (s, e) => LoadDanhSach();
            pnlCboWrap.Controls.Add(cboLocNhanVien);

            cboXemTheo = new Guna2ComboBox
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
            cboXemTheo.Items.AddRange(new object[] { "Theo tuần", "Theo ngày" });
            cboXemTheo.SelectedIndex = 0;
            cboXemTheo.SelectedIndexChanged += (s, e) => LoadDanhSach();
            pnlXemTheoWrap.Controls.Add(cboXemTheo);

            dtpTuanHienTai = new Guna2DateTimePicker
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Dock = DockStyle.Top,
                Height = 36,
                BorderRadius = 8,
                BorderColor = BorderInput,
                FillColor = InputBg,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today,
            };
            dtpTuanHienTai.ValueChanged += (s, e) => LoadDanhSach();
            pnlDateWrap.Controls.Add(dtpTuanHienTai);

            pnlFilter.Controls.Add(pnlDateWrap);
            pnlFilter.Controls.Add(pnlXemTheoWrap);
            pnlFilter.Controls.Add(pnlCboWrap);
            pnlFilter.Controls.Add(pnlBtnWrap);

            const int BTN_W = 180, CBO_W = 170, XT_W = 130, DATE_W = 140, GAP = 8, H = 42;
            EventHandler boTriFilter = (s, e) =>
            {
                int pw = pnlFilter.ClientSize.Width;
                if (pw <= 0) return;
                pnlBtnWrap.SetBounds(pw - BTN_W, 0, BTN_W, H);
                pnlCboWrap.SetBounds(pw - BTN_W - GAP - CBO_W, 0, CBO_W, H);
                pnlXemTheoWrap.SetBounds(DATE_W + GAP, 0, XT_W, H);
                pnlDateWrap.SetBounds(0, 0, DATE_W, H);
            };
            pnlFilter.Resize += boTriFilter;
            pnlFilter.Layout += (s, e) => boTriFilter(s, e);

            var pnlSpacer = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // ── DataGridView ──
            dgvPhanCong = new DataGridView
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
            dgvPhanCong.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorScheme.PrimaryDark,
                ForeColor = Color.White,
                Font = AppFonts.BodyBold,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvPhanCong.ColumnHeadersHeight = 40;
            dgvPhanCong.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPhanCong.EnableHeadersVisualStyles = false;
            dgvPhanCong.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowOdd,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvPhanCong.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowAlt,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
            };

            dgvPhanCong.CellPainting += DgvPhanCong_CellPainting;

            dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaPC", HeaderText = "Mã", FillWeight = 7 });
            dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn { Name = "HoTen", HeaderText = "Nhân viên", FillWeight = 20 });
            dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn { Name = "VaiTro", HeaderText = "Vai trò", FillWeight = 12 });
            dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenCa", HeaderText = "Ca làm", FillWeight = 14 });
            dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThoiGian", HeaderText = "Thời gian", FillWeight = 14 });
            dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn { Name = "NgayLam", HeaderText = "Ngày làm", FillWeight = 12 });
            dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn { Name = "DiemDanh", HeaderText = "Điểm danh", FillWeight = 11 });
            dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThaoTac", HeaderText = "Thao tác", FillWeight = 10 });

            // Hidden
            dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaPhanCong", Visible = false });
            dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaNguoiDung", Visible = false });
            dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaCa", Visible = false });
            dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn { Name = "TrangThaiDD", Visible = false });

            dgvPhanCong.CellClick += DgvPhanCong_CellClick;
            dgvPhanCong.CellFormatting += DgvPhanCong_CellFormatting;
            dgvPhanCong.CellMouseMove += DgvPhanCong_CellMouseMove;

            // ── Label tiêu đề tuần/ngày ──
            var pnlTieuDe = new Panel
            {
                Dock = DockStyle.Top,
                Height = 32,
                BackColor = Color.Transparent,
            };
            lblTieuDeTuan = new Label
            {
                Text = "",
                Font = AppFonts.H4,
                ForeColor = ColorScheme.PrimaryDark,
                Location = new Point(0, 4),
                AutoSize = true,
                BackColor = Color.Transparent,
            };
            pnlTieuDe.Controls.Add(lblTieuDeTuan);

            // WinForms dock order: Fill trước, rồi Top sau (Top add sau nằm trên)
            pnlLeft.Controls.Add(dgvPhanCong);       // Fill
            pnlLeft.Controls.Add(pnlTieuDe);          // Top — ngay trên grid
            pnlLeft.Controls.Add(pnlSpacer);           // Top — spacer
            pnlLeft.Controls.Add(pnlFilter);            // Top — filter row trên cùng
        }

        // ══════════════════════════════════════════
        // HEADER GRADIENT PAINT
        // ══════════════════════════════════════════

        private void DgvPhanCong_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1) return;

            e.Handled = true;
            var rect = e.CellBounds;

            using (var brush = new LinearGradientBrush(
                new Rectangle(0, rect.Y, dgvPhanCong.Width, rect.Height),
                ColorScheme.PrimaryDark, Color.FromArgb(180, GoldAccent.R, GoldAccent.G, GoldAccent.B),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

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
                            new Rectangle(0, rect.Y, dgvPhanCong.Width, rect.Height),
                            ColorScheme.PrimaryDark, Color.FromArgb(180, GoldAccent.R, GoldAccent.G, GoldAccent.B),
                            LinearGradientMode.Horizontal))
                        {
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            e.Graphics.FillPath(brush2, path);
                        }
                    }
                }
            }

            if (e.Value != null)
            {
                var textRect = new Rectangle(rect.X + 12, rect.Y, rect.Width - 12, rect.Height);
                TextRenderer.DrawText(e.Graphics, e.Value.ToString(), AppFonts.BodyBold,
                    textRect, Color.White, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            }
        }

        // ══════════════════════════════════════════
        // PANEL PHẢI — Form Phân Công (card style)
        // ══════════════════════════════════════════

        private void TaoFormBenPhai()
        {
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
            int W = 328;
            int y = 12;

            // ── Title ──
            var pnlTitleSection = new Panel { Location = new Point(X, y), Size = new Size(W, 36), BackColor = Color.Transparent };
            pnlTitleSection.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new LinearGradientBrush(new Rectangle(0, 2, 4, 16),
                    ColorScheme.PrimaryDark, GoldAccent, LinearGradientMode.Vertical))
                {
                    g.FillRectangle(br, 0, 4, 4, 18);
                }
                using (var pen = new Pen(ColorScheme.PrimaryPale, 1.5f))
                    g.DrawLine(pen, 0, 35, W, 35);
            };
            lblFormTitle = new Label
            {
                Text = "📅 Phân Công Ca Mới",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = ColorScheme.PrimaryDark,
                Location = new Point(12, 4),
                AutoSize = true,
                BackColor = Color.Transparent,
            };
            pnlTitleSection.Controls.Add(lblFormTitle);
            pnlCard.Controls.Add(pnlTitleSection);
            y += 48;

            // ── Nhân viên ──
            pnlCard.Controls.Add(TaoLabel("Nhân viên", new Point(X, y), true));
            y += 20;
            cboNhanVien = new Guna2ComboBox
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Size = new Size(W, 36),
                Location = new Point(X, y),
                BorderRadius = 8,
                BorderColor = BorderInput,
                FocusedState = { BorderColor = ColorScheme.Primary },
                HoverState = { BorderColor = ColorScheme.Primary },
                FillColor = InputBg,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            pnlCard.Controls.Add(cboNhanVien);
            y += 42;

            // ── Ca làm việc ──
            pnlCard.Controls.Add(TaoLabel("Ca làm việc", new Point(X, y), true));
            y += 20;
            cboCaLam = new Guna2ComboBox
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Size = new Size(W, 36),
                Location = new Point(X, y),
                BorderRadius = 8,
                BorderColor = BorderInput,
                FocusedState = { BorderColor = ColorScheme.Primary },
                HoverState = { BorderColor = ColorScheme.Primary },
                FillColor = InputBg,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            pnlCard.Controls.Add(cboCaLam);
            y += 42;

            // ── Ngày làm việc ──
            pnlCard.Controls.Add(TaoLabel("Ngày làm việc", new Point(X, y), true));
            y += 20;
            dtpNgayLam = new Guna2DateTimePicker
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Size = new Size(W, 36),
                Location = new Point(X, y),
                BorderRadius = 8,
                BorderColor = BorderInput,
                FillColor = InputBg,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today,
            };
            pnlCard.Controls.Add(dtpNgayLam);
            y += 46;

            // ── Notif chip ──
            var pnlNote = new Panel { Location = new Point(X, y), Size = new Size(W, 40), BackColor = Color.Transparent };
            pnlNote.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = TaoRoundedRect(new Rectangle(0, 0, pnlNote.Width - 1, pnlNote.Height - 1), 10))
                using (var fill = new SolidBrush(NotifChipBg))
                {
                    g.FillPath(fill, path);
                }
                using (var pen = new Pen(ColorScheme.Primary, 3f))
                    g.DrawLine(pen, 2, 4, 2, pnlNote.Height - 4);
            };
            pnlNote.Controls.Add(new Label
            {
                Text = "ℹ️ Mỗi NV chỉ được phân 1 ca/ngày. Trùng sẽ bị từ chối.",
                Font = AppFonts.Small,
                ForeColor = ColorScheme.TextMid,
                Location = new Point(14, 6),
                Size = new Size(W - 20, 28),
                BackColor = Color.Transparent,
            });
            pnlCard.Controls.Add(pnlNote);
            y += 48;

            // Error
            lblError = new Label { Font = AppFonts.Small, ForeColor = ColorScheme.Danger, Location = new Point(X, y), Size = new Size(W, 18), Text = "" };
            pnlCard.Controls.Add(lblError);
            y += 22;

            // ── Nút Lưu ──
            btnLuu = new Guna2Button
            {
                Text = "💾 Lưu Phân Công",
                Font = AppFonts.BodyBold,
                ForeColor = Color.White,
                FillColor = ColorScheme.Primary,
                BorderRadius = 20,
                Location = new Point(X, y),
                Size = new Size(W, 38),
                Cursor = Cursors.Hand,
            };
            btnLuu.Click += BtnLuu_Click;
            pnlCard.Controls.Add(btnLuu);
            y += 46;

            // ── Nút Xóa ──
            btnXoa = new Guna2Button
            {
                Text = "🗑️ Xóa Phân Công",
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
            btnXoa.Click += BtnXoa_Click;
            pnlCard.Controls.Add(btnXoa);
        }

        // ══════════════════════════════════════════
        // LOAD COMBO DATA
        // ══════════════════════════════════════════

        private void LoadComboData()
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    // Nhân viên (hoạt động, vai trò 1-3)
                    using (var cmd = new SqlCommand(
                        @"SELECT nd.MaNguoiDung, nd.HoTen + ' (' + vt.TenVaiTro + ')' AS Display
                          FROM NguoiDung nd
                          JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
                          WHERE nd.TrangThaiTK = 1 AND nd.IsDeleted = 0 AND nd.MaVaiTro IN (1,2,3)
                          ORDER BY nd.MaVaiTro, nd.HoTen", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        _dsNhanVien.Clear();
                        while (reader.Read())
                        {
                            int ma = Convert.ToInt32(reader.GetValue(0));
                            string display = reader.GetString(1);
                            _dsNhanVien.Add(new KeyValuePair<int, string>(ma, display));
                        }
                    }

                    // Ca làm việc
                    using (var cmd = new SqlCommand(
                        @"SELECT MaCa, TenCa + ' (' + CONVERT(VARCHAR(5), GioBatDau, 108) + ' - ' + CONVERT(VARCHAR(5), GioKetThuc, 108) + ')' AS Display
                          FROM CaLamViec ORDER BY MaCa", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        _dsCaLam.Clear();
                        while (reader.Read())
                        {
                            int ma = Convert.ToInt32(reader.GetValue(0));
                            string display = reader.GetString(1);
                            _dsCaLam.Add(new KeyValuePair<int, string>(ma, display));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Đổ vào combo form phải
            cboNhanVien.Items.Clear();
            foreach (var nv in _dsNhanVien)
                cboNhanVien.Items.Add(nv.Value);
            if (cboNhanVien.Items.Count > 0) cboNhanVien.SelectedIndex = 0;

            cboCaLam.Items.Clear();
            foreach (var ca in _dsCaLam)
                cboCaLam.Items.Add(ca.Value);
            if (cboCaLam.Items.Count > 0) cboCaLam.SelectedIndex = 0;

            // Đổ vào combo filter bên trái
            cboLocNhanVien.Items.Clear();
            cboLocNhanVien.Items.Add("Tất cả nhân viên");
            foreach (var nv in _dsNhanVien)
                cboLocNhanVien.Items.Add(nv.Value);
            cboLocNhanVien.SelectedIndex = 0;
        }

        // ══════════════════════════════════════════
        // LOAD DANH SÁCH
        // ══════════════════════════════════════════

        private void LoadDanhSach()
        {
            dgvPhanCong.Rows.Clear();

            DateTime ngayChon = dtpTuanHienTai?.Value.Date ?? DateTime.Today;
            bool xemTheoNgay = (cboXemTheo?.SelectedIndex ?? 0) == 1;

            DateTime tuNgay, denNgay;
            if (xemTheoNgay)
            {
                tuNgay = ngayChon;
                denNgay = ngayChon;
            }
            else
            {
                int dayOfWeek = ((int)ngayChon.DayOfWeek + 6) % 7; // Mon=0, Sun=6
                tuNgay = ngayChon.AddDays(-dayOfWeek);
                denNgay = tuNgay.AddDays(6);
            }

            // Cập nhật label tiêu đề
            if (lblTieuDeTuan != null)
            {
                if (xemTheoNgay)
                    lblTieuDeTuan.Text = "📌 Lịch Làm Việc Ngày " + tuNgay.ToString("dd/MM/yyyy (dddd)", new CultureInfo("vi-VN"));
                else
                    lblTieuDeTuan.Text = "📅 Lịch Làm Việc Tuần " + tuNgay.ToString("dd/MM") + " — " + denNgay.ToString("dd/MM/yyyy");
            }

            int filterNV = (cboLocNhanVien?.SelectedIndex ?? 0) - 1; // -1 = tất cả
            int maNVFilter = filterNV >= 0 && filterNV < _dsNhanVien.Count ? _dsNhanVien[filterNV].Key : 0;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    @"SELECT pc.MaPhanCong, nd.HoTen, vt.TenVaiTro, cl.TenCa,
                             CONVERT(VARCHAR(5), cl.GioBatDau, 108) + ' - ' + CONVERT(VARCHAR(5), cl.GioKetThuc, 108) AS ThoiGian,
                             pc.NgayLamViec, pc.TrangThaiDiemDanh,
                             pc.MaNguoiDung, pc.MaCa
                      FROM PhanCongCa pc
                      JOIN NguoiDung nd ON pc.MaNguoiDung = nd.MaNguoiDung
                      JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
                      JOIN CaLamViec cl ON pc.MaCa = cl.MaCa
                      WHERE pc.NgayLamViec BETWEEN @TuNgay AND @DenNgay
                        AND (@MaNV = 0 OR pc.MaNguoiDung = @MaNV)
                      ORDER BY pc.NgayLamViec, cl.GioBatDau, nd.HoTen", conn))
                {
                    cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@DenNgay", denNgay);
                    cmd.Parameters.AddWithValue("@MaNV", maNVFilter);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int maPC = Convert.ToInt32(reader.GetValue(0));
                            string hoTen = reader.GetString(1);
                            string vaiTro = reader.GetString(2);
                            string tenCa = reader.GetString(3);
                            string thoiGian = reader.GetString(4);
                            DateTime ngayLam = reader.GetDateTime(5);
                            int trangThai = Convert.ToInt32(reader.GetValue(6));
                            int maNguoiDung = Convert.ToInt32(reader.GetValue(7));
                            int maCa = Convert.ToInt32(reader.GetValue(8));

                            string maPCDisplay = "PC" + maPC.ToString("D3");
                            string ngayDisplay = ngayLam.ToString("dd/MM (ddd)", new CultureInfo("vi-VN"));

                            string vaiTroBadge;
                            if (vaiTro.Contains("Admin")) vaiTroBadge = "🛡️ Admin";
                            else if (vaiTro.Contains("Bác")) vaiTroBadge = "🩺 Bác Sĩ";
                            else vaiTroBadge = "🏥 Lễ Tân";

                            string ddText;
                            if (trangThai == 1) ddText = "⏳ Chưa ĐD";
                            else if (trangThai == 2) ddText = "✅ Đã ĐD";
                            else ddText = "❌ Vắng";

                            string caIcon;
                            if (tenCa.Contains("Sáng")) caIcon = "🌅 " + tenCa;
                            else if (tenCa.Contains("Chiều")) caIcon = "☀️ " + tenCa;
                            else caIcon = "🌙 " + tenCa;

                            int idx = dgvPhanCong.Rows.Add(maPCDisplay, hoTen, vaiTroBadge, caIcon, thoiGian, ngayDisplay, ddText, "");
                            dgvPhanCong.Rows[idx].Cells["MaPhanCong"].Value = maPC;
                            dgvPhanCong.Rows[idx].Cells["MaNguoiDung"].Value = maNguoiDung;
                            dgvPhanCong.Rows[idx].Cells["MaCa"].Value = maCa;
                            dgvPhanCong.Rows[idx].Cells["TrangThaiDD"].Value = trangThai;
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

        private void DgvPhanCong_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvPhanCong.Columns[e.ColumnIndex].Name;

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
                else
                {
                    e.CellStyle.BackColor = BadgeWarningBg;
                    e.CellStyle.ForeColor = BadgeWarningFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
            }

            if (colName == "TenCa" && e.Value != null)
            {
                string v = e.Value.ToString();
                if (v.Contains("Sáng"))
                {
                    e.CellStyle.BackColor = BadgeWarningBg;
                    e.CellStyle.ForeColor = BadgeWarningFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else if (v.Contains("Chiều"))
                {
                    e.CellStyle.BackColor = BadgeInfoBg;
                    e.CellStyle.ForeColor = BadgeInfoFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else
                {
                    e.CellStyle.BackColor = ColorTranslator.FromHtml("#EDE9FE");
                    e.CellStyle.ForeColor = ColorTranslator.FromHtml("#6D28D9");
                    e.CellStyle.Font = AppFonts.Badge;
                }
            }

            if (colName == "DiemDanh" && e.Value != null)
            {
                string v = e.Value.ToString();
                if (v.Contains("Đã"))
                {
                    e.CellStyle.BackColor = BadgeActiveBg;
                    e.CellStyle.ForeColor = BadgeActiveFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else if (v.Contains("Vắng"))
                {
                    e.CellStyle.BackColor = BadgeDangerBg;
                    e.CellStyle.ForeColor = BadgeDangerFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else
                {
                    e.CellStyle.BackColor = BadgeWarningBg;
                    e.CellStyle.ForeColor = BadgeWarningFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
            }

            if (colName == "ThaoTac")
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.SelectionBackColor = ColorScheme.PrimaryPale;
            }
        }

        // ══════════════════════════════════════════
        // CELL CLICK
        // ══════════════════════════════════════════

        private void DgvPhanCong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int maPC = Convert.ToInt32(dgvPhanCong.Rows[e.RowIndex].Cells["MaPhanCong"].Value);
            LoadThongTinVaoForm(e.RowIndex, maPC);
        }

        private void DgvPhanCong_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvPhanCong.Columns[e.ColumnIndex].Name;
            dgvPhanCong.Cursor = colName == "ThaoTac" ? Cursors.Hand : Cursors.Default;
        }

        // ══════════════════════════════════════════
        // LOAD THÔNG TIN VÀO FORM PHẢI
        // ══════════════════════════════════════════

        private void LoadThongTinVaoForm(int rowIndex, int maPC)
        {
            int maNV = Convert.ToInt32(dgvPhanCong.Rows[rowIndex].Cells["MaNguoiDung"].Value);
            int maCa = Convert.ToInt32(dgvPhanCong.Rows[rowIndex].Cells["MaCa"].Value);

            // Tìm index trong combo
            for (int i = 0; i < _dsNhanVien.Count; i++)
            {
                if (_dsNhanVien[i].Key == maNV)
                {
                    cboNhanVien.SelectedIndex = i;
                    break;
                }
            }
            for (int i = 0; i < _dsCaLam.Count; i++)
            {
                if (_dsCaLam[i].Key == maCa)
                {
                    cboCaLam.SelectedIndex = i;
                    break;
                }
            }

            // Parse ngày từ hidden field
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand("SELECT NgayLamViec FROM PhanCongCa WHERE MaPhanCong = @Ma", conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", maPC);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        dtpNgayLam.Value = Convert.ToDateTime(result);
                }
            }
            catch { }

            _maPhanCongDangChon = maPC;
            lblFormTitle.Text = "✏️ Sửa Phân Công — PC" + maPC.ToString("D3");
            btnLuu.Text = "💾  Cập Nhật";
            btnXoa.Enabled = true;
            lblError.Text = "";
        }

        private void ResetForm()
        {
            _maPhanCongDangChon = -1;
            if (cboNhanVien.Items.Count > 0) cboNhanVien.SelectedIndex = 0;
            if (cboCaLam.Items.Count > 0) cboCaLam.SelectedIndex = 0;
            dtpNgayLam.Value = DateTime.Today;
            lblFormTitle.Text = "📅 Phân Công Ca Mới";
            btnLuu.Text = "💾  Lưu Phân Công";
            btnXoa.Enabled = false;
            lblError.Text = "";
        }

        // ══════════════════════════════════════════
        // NÚT LƯU / CẬP NHẬT
        // ══════════════════════════════════════════

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (cboNhanVien.SelectedIndex < 0 || cboNhanVien.SelectedIndex >= _dsNhanVien.Count)
            {
                lblError.Text = "Vui lòng chọn nhân viên.";
                return;
            }
            if (cboCaLam.SelectedIndex < 0 || cboCaLam.SelectedIndex >= _dsCaLam.Count)
            {
                lblError.Text = "Vui lòng chọn ca làm.";
                return;
            }

            int maNV = _dsNhanVien[cboNhanVien.SelectedIndex].Key;
            int maCa = _dsCaLam[cboCaLam.SelectedIndex].Key;
            DateTime ngayLam = dtpNgayLam.Value.Date;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    if (_maPhanCongDangChon == -1)
                    {
                        // KIỂM TRA TRÙNG
                        using (var chk = new SqlCommand(
                            @"SELECT COUNT(*) FROM PhanCongCa 
                              WHERE MaNguoiDung = @MaNV AND NgayLamViec = @Ngay", conn))
                        {
                            chk.Parameters.AddWithValue("@MaNV", maNV);
                            chk.Parameters.AddWithValue("@Ngay", ngayLam);
                            int count = Convert.ToInt32(chk.ExecuteScalar());
                            if (count > 0)
                            {
                                lblError.Text = "NV này đã có ca trong ngày " + ngayLam.ToString("dd/MM/yyyy") + ".";
                                return;
                            }
                        }

                        // TẠO MỚI
                        using (var cmd = new SqlCommand(
                            @"INSERT INTO PhanCongCa (MaNguoiDung, MaCa, NgayLamViec, TrangThaiDiemDanh)
                              VALUES (@MaNV, @MaCa, @Ngay, 1)", conn))
                        {
                            cmd.Parameters.AddWithValue("@MaNV", maNV);
                            cmd.Parameters.AddWithValue("@MaCa", maCa);
                            cmd.Parameters.AddWithValue("@Ngay", ngayLam);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Phân công thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // KIỂM TRA TRÙNG (trừ bản ghi hiện tại)
                        using (var chk = new SqlCommand(
                            @"SELECT COUNT(*) FROM PhanCongCa 
                              WHERE MaNguoiDung = @MaNV AND NgayLamViec = @Ngay AND MaPhanCong <> @MaPC", conn))
                        {
                            chk.Parameters.AddWithValue("@MaNV", maNV);
                            chk.Parameters.AddWithValue("@Ngay", ngayLam);
                            chk.Parameters.AddWithValue("@MaPC", _maPhanCongDangChon);
                            int count = Convert.ToInt32(chk.ExecuteScalar());
                            if (count > 0)
                            {
                                lblError.Text = "NV này đã có ca trong ngày " + ngayLam.ToString("dd/MM/yyyy") + ".";
                                return;
                            }
                        }

                        // CẬP NHẬT
                        using (var cmd = new SqlCommand(
                            @"UPDATE PhanCongCa 
                              SET MaNguoiDung = @MaNV, MaCa = @MaCa, NgayLamViec = @Ngay
                              WHERE MaPhanCong = @MaPC", conn))
                        {
                            cmd.Parameters.AddWithValue("@MaNV", maNV);
                            cmd.Parameters.AddWithValue("@MaCa", maCa);
                            cmd.Parameters.AddWithValue("@Ngay", ngayLam);
                            cmd.Parameters.AddWithValue("@MaPC", _maPhanCongDangChon);
                            int rows = cmd.ExecuteNonQuery();
                            if (rows == 0)
                            {
                                lblError.Text = "Không tìm thấy phân công để cập nhật.";
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
        // NÚT XÓA
        // ══════════════════════════════════════════

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (_maPhanCongDangChon == -1) return;

            var result = MessageBox.Show(
                "Xóa phân công PC" + _maPhanCongDangChon.ToString("D3") + "?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand("DELETE FROM PhanCongCa WHERE MaPhanCong = @Ma", conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", _maPhanCongDangChon);
                    cmd.ExecuteNonQuery();
                }
                ResetForm();
                LoadDanhSach();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
