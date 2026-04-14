using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    public partial class NhapKhoForm : Form
    {
        // ── Header controls ──
        private Guna2ComboBox cboNCC;
        private Guna2DateTimePicker dtpNgayNhap;
        private Guna2TextBox txtNguoiNhap;
        private Guna2TextBox txtMaPhieu;

        // ── Chi tiết grid ──
        private DataGridView dgvChiTiet;
        private Label lblTongGiaTri;
        private Guna2Button _btnInPhieu;
        private int _lastSavedMaPhieu = -1;

        // Cache
        private List<KeyValuePair<int, string>> _dsNCC = new List<KeyValuePair<int, string>>();
        private List<KeyValuePair<int, string>> _dsThuoc = new List<KeyValuePair<int, string>>();

        // Màu wireframe (đồng bộ)
        private static readonly Color BorderInput = ColorTranslator.FromHtml("#D1E8DC");
        private static readonly Color InputBg = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color GoldAccent = Color.FromArgb(184, 138, 40);
        private static readonly Color RowAlt = ColorTranslator.FromHtml("#F5FBF7");
        private static readonly Color RowOdd = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color GridBorderColor = ColorTranslator.FromHtml("#EEF6F1");
        private static readonly Color NotifChipBg = ColorScheme.PrimaryPale;

        public NhapKhoForm()
        {
            InitializeComponent();
            TaoBoCuc();
            LoadComboData();
            GenMaPhieu();
            ThemDongMoi();
        }

        // ══════════════════════════════════════════
        // BỐ CỤC CHÍNH — Full page, 3 sections dọc
        // ══════════════════════════════════════════

        private void TaoBoCuc()
        {
            this.Padding = new Padding(20);
            this.BackColor = ColorScheme.Background;

            var pnlMain = new Panel { Dock = DockStyle.Fill, BackColor = ColorScheme.Background };
            this.Controls.Add(pnlMain);

            // Middle: Chi tiết nhập kho (Fill) — add FIRST
            var pnlMiddle = TaoChiTietSection();
            pnlMiddle.Dock = DockStyle.Fill;

            // Top: Thông tin phiếu nhập (card)
            var pnlTop = TaoHeaderSection();
            pnlTop.Dock = DockStyle.Top;

            // Bottom: Buttons
            var pnlBottom = TaoBottomSection();
            pnlBottom.Dock = DockStyle.Bottom;

            // WinForms dock: Fill first, then Top/Bottom after
            pnlMain.Controls.Add(pnlMiddle);  // Fill
            pnlMain.Controls.Add(pnlBottom);  // Bottom
            pnlMain.Controls.Add(pnlTop);     // Top
        }

        // ══════════════════════════════════════════
        // SECTION 1 — THÔNG TIN PHIẾU NHẬP
        // ══════════════════════════════════════════

        private Panel TaoHeaderSection()
        {
            var pnlOuter = new Panel { Height = 160, BackColor = Color.Transparent };

            var pnlCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(20) };
            pnlCard.Paint += (s, e) =>
            {
                var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = TaoRoundedRect(new Rectangle(0, 0, pnlCard.Width - 1, pnlCard.Height - 1), 12))
                using (var pen = new Pen(ColorScheme.Border, 1f))
                    g.DrawPath(pen, path);
            };
            pnlOuter.Controls.Add(pnlCard);

            // Title
            var pnlTitle = new Panel { Location = new Point(16, 10), Size = new Size(400, 28), BackColor = Color.Transparent };
            pnlTitle.Paint += (s, e) =>
            {
                using (var br = new LinearGradientBrush(new Rectangle(0, 2, 4, 16),
                    ColorScheme.PrimaryDark, GoldAccent, LinearGradientMode.Vertical))
                    e.Graphics.FillRectangle(br, 0, 2, 4, 18);
            };
            pnlTitle.Controls.Add(new Label
            {
                Text = "📋 Thông Tin Phiếu Nhập",
                Font = AppFonts.H4, ForeColor = ColorScheme.PrimaryDark,
                Location = new Point(12, 2), AutoSize = true, BackColor = Color.Transparent,
            });
            pnlCard.Controls.Add(pnlTitle);

            // Labels
            var lblNCC = TaoLabel("Nhà cung cấp *", new Point(0, 0));
            var lblNgay = TaoLabel("Ngày nhập *", new Point(0, 0));
            var lblNguoi = TaoLabel("Người nhập", new Point(0, 0));
            var lblMa = TaoLabel("Mã phiếu (auto)", new Point(0, 0));
            pnlCard.Controls.Add(lblNCC);
            pnlCard.Controls.Add(lblNgay);
            pnlCard.Controls.Add(lblNguoi);
            pnlCard.Controls.Add(lblMa);

            // Controls (wrapper panels for Guna2)
            var pnlNCCWrap = new Panel { BackColor = Color.Transparent };
            var pnlNgayWrap = new Panel { BackColor = Color.Transparent };
            var pnlNguoiWrap = new Panel { BackColor = Color.Transparent };
            var pnlMaWrap = new Panel { BackColor = Color.Transparent };

            cboNCC = new Guna2ComboBox
            {
                Font = AppFonts.Body, ForeColor = ColorScheme.TextDark,
                Dock = DockStyle.Top, Height = 36,
                BorderRadius = 8, BorderColor = BorderInput, FillColor = InputBg,
                FocusedState = { BorderColor = ColorScheme.Primary },
                HoverState = { BorderColor = ColorScheme.Primary },
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            pnlNCCWrap.Controls.Add(cboNCC);
            pnlCard.Controls.Add(pnlNCCWrap);

            dtpNgayNhap = new Guna2DateTimePicker
            {
                Font = AppFonts.Body, ForeColor = ColorScheme.TextDark,
                Dock = DockStyle.Top, Height = 36,
                BorderRadius = 8, BorderColor = BorderInput, FillColor = InputBg,
                Format = DateTimePickerFormat.Short, Value = DateTime.Today,
            };
            pnlNgayWrap.Controls.Add(dtpNgayNhap);
            pnlCard.Controls.Add(pnlNgayWrap);

            var nd = LoginForm.NguoiDungHienTai;
            txtNguoiNhap = new Guna2TextBox
            {
                Font = AppFonts.Body, ForeColor = ColorScheme.TextGray,
                Dock = DockStyle.Top, Height = 36,
                BorderRadius = 8, BorderColor = BorderInput, FillColor = Color.FromArgb(245, 245, 245),
                Text = nd != null ? nd.HoTen : "Admin",
                ReadOnly = true,
            };
            pnlNguoiWrap.Controls.Add(txtNguoiNhap);
            pnlCard.Controls.Add(pnlNguoiWrap);

            txtMaPhieu = new Guna2TextBox
            {
                Font = AppFonts.BodyBold, ForeColor = ColorScheme.PrimaryDark,
                Dock = DockStyle.Top, Height = 36,
                BorderRadius = 8, BorderColor = BorderInput, FillColor = Color.FromArgb(245, 245, 245),
                Text = "PN#0001", ReadOnly = true,
            };
            pnlMaWrap.Controls.Add(txtMaPhieu);
            pnlCard.Controls.Add(pnlMaWrap);

            // Responsive layout
            const int PAD = 16, GAP = 12, LBL_Y = 48, CTRL_Y = 68, CTRL_H = 36;
            EventHandler layoutFields = (s, e) =>
            {
                int cw = pnlCard.ClientSize.Width - PAD * 2;
                if (cw <= 0) return;
                int colW = (cw - GAP * 3) / 4;

                int x0 = PAD, x1 = PAD + colW + GAP, x2 = PAD + (colW + GAP) * 2, x3 = PAD + (colW + GAP) * 3;

                lblNCC.Location = new Point(x0, LBL_Y);
                lblNgay.Location = new Point(x1, LBL_Y);
                lblNguoi.Location = new Point(x2, LBL_Y);
                lblMa.Location = new Point(x3, LBL_Y);

                pnlNCCWrap.SetBounds(x0, CTRL_Y, colW, CTRL_H);
                pnlNgayWrap.SetBounds(x1, CTRL_Y, colW, CTRL_H);
                pnlNguoiWrap.SetBounds(x2, CTRL_Y, colW, CTRL_H);
                pnlMaWrap.SetBounds(x3, CTRL_Y, colW, CTRL_H);
            };
            pnlCard.Resize += layoutFields;
            pnlCard.Layout += (s, e) => layoutFields(s, e);

            return pnlOuter;
        }

        // ══════════════════════════════════════════
        // SECTION 2 — CHI TIẾT NHẬP KHO
        // ══════════════════════════════════════════

        private Panel TaoChiTietSection()
        {
            var pnlOuter = new Panel { BackColor = Color.Transparent, Padding = new Padding(0, 10, 0, 0) };

            var pnlCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(20) };
            pnlCard.Paint += (s, e) =>
            {
                var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = TaoRoundedRect(new Rectangle(0, 0, pnlCard.Width - 1, pnlCard.Height - 1), 12))
                using (var pen = new Pen(ColorScheme.Border, 1f))
                    g.DrawPath(pen, path);
            };
            pnlOuter.Controls.Add(pnlCard);

            // Title row
            var pnlTitleRow = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = Color.Transparent };
            var pnlTitleLeft = new Panel { Location = new Point(0, 0), Size = new Size(300, 28), BackColor = Color.Transparent };
            pnlTitleLeft.Paint += (s, e) =>
            {
                using (var br = new LinearGradientBrush(new Rectangle(0, 2, 4, 16),
                    ColorScheme.PrimaryDark, GoldAccent, LinearGradientMode.Vertical))
                    e.Graphics.FillRectangle(br, 0, 2, 4, 18);
            };
            pnlTitleLeft.Controls.Add(new Label
            {
                Text = "💊 Chi Tiết Nhập Kho",
                Font = AppFonts.H4, ForeColor = ColorScheme.PrimaryDark,
                Location = new Point(12, 2), AutoSize = true, BackColor = Color.Transparent,
            });
            pnlTitleRow.Controls.Add(pnlTitleLeft);

            var btnThemDong = new Guna2Button
            {
                Text = "➕ Thêm Dòng",
                Font = AppFonts.BodyBold, ForeColor = Color.White,
                FillColor = ColorScheme.Primary, BorderRadius = 18,
                Size = new Size(160, 32), Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            btnThemDong.Click += (s, e) => ThemDongMoi();
            pnlTitleRow.Controls.Add(btnThemDong);
            pnlTitleRow.Resize += (s, e) => btnThemDong.Location = new Point(pnlTitleRow.Width - 170, 4);

            pnlCard.Controls.Add(pnlTitleRow);

            // DataGridView
            dgvChiTiet = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.None,
                GridColor = GridBorderColor,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = AppFonts.Body,
                RowTemplate = { Height = 56 },
                EditMode = DataGridViewEditMode.EditOnEnter,
            };
            dgvChiTiet.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorScheme.PrimaryDark, ForeColor = Color.White,
                Font = AppFonts.BodyBold, Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvChiTiet.ColumnHeadersHeight = 44;
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvChiTiet.EnableHeadersVisualStyles = false;
            dgvChiTiet.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White, ForeColor = ColorScheme.TextMid,
                SelectionBackColor = Color.White, SelectionForeColor = ColorScheme.TextDark,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvChiTiet.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White, ForeColor = ColorScheme.TextMid,
                SelectionBackColor = Color.White, SelectionForeColor = ColorScheme.TextDark,
            };

            dgvChiTiet.CellPainting += DgvChiTiet_CellPainting;

            // Columns
            var colThuoc = new DataGridViewComboBoxColumn
            {
                Name = "Thuoc", HeaderText = "Thuốc / Mỹ phẩm", FillWeight = 28,
                FlatStyle = FlatStyle.Flat, DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
            };
            dgvChiTiet.Columns.Add(colThuoc);
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoLuong", HeaderText = "Số lượng", FillWeight = 12 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiaNhap", HeaderText = "Giá nhập", FillWeight = 14 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "HanSuDung", HeaderText = "Hạn sử dụng", FillWeight = 14 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThanhTien", HeaderText = "Thành tiền", FillWeight = 14, ReadOnly = true });

            var colXoa = new DataGridViewButtonColumn
            {
                Name = "Xoa", HeaderText = "", FillWeight = 5,
                Text = "✕", UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat, Width = 44,
            };
            dgvChiTiet.Columns.Add(colXoa);

            dgvChiTiet.CellEndEdit += DgvChiTiet_CellEndEdit;
            dgvChiTiet.CellContentClick += DgvChiTiet_CellContentClick;
            dgvChiTiet.CellFormatting += DgvChiTiet_CellFormatting;

            var pnlSpacer = new Panel { Dock = DockStyle.Top, Height = 6, BackColor = Color.Transparent };

            // Tổng giá trị row
            var pnlTong = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.Transparent };
            var lblCaption = new Label
            {
                Text = "TỔNG GIÁ TRỊ PHIẾU:",
                Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextMid,
                Dock = DockStyle.Right, AutoSize = true, Padding = new Padding(0, 10, 4, 0),
            };
            lblTongGiaTri = new Label
            {
                Text = "0đ",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold), ForeColor = ColorScheme.PrimaryDark,
                Dock = DockStyle.Right, AutoSize = true, Padding = new Padding(0, 6, 16, 0),
            };
            pnlTong.Controls.Add(lblTongGiaTri);
            pnlTong.Controls.Add(lblCaption);

            // WinForms dock: Fill first, then Top/Bottom
            pnlCard.Controls.Add(dgvChiTiet);   // Fill
            pnlCard.Controls.Add(pnlTong);       // Bottom
            pnlCard.Controls.Add(pnlSpacer);     // Top (below title)
            pnlCard.Controls.Add(pnlTitleRow);   // Top (topmost)

            return pnlOuter;
        }

        // ══════════════════════════════════════════
        // SECTION 3 — BOTTOM BUTTONS + NOTIF
        // ══════════════════════════════════════════

        private Panel TaoBottomSection()
        {
            var pnl = new Panel { Height = 50, BackColor = Color.Transparent };

            var pnlBtns = new Panel { Dock = DockStyle.Fill, Height = 44, BackColor = Color.Transparent };

            var btnLuu = new Guna2Button
            {
                Text = "💾 Lưu Phiếu Nhập",
                Font = AppFonts.BodyBold, ForeColor = Color.White,
                FillColor = ColorScheme.Primary, BorderRadius = 20,
                Size = new Size(180, 38), Cursor = Cursors.Hand,
            };
            btnLuu.Click += BtnLuu_Click;

            var btnHuy = new Guna2Button
            {
                Text = "Hủy",
                Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextMid,
                FillColor = Color.White, BorderRadius = 20,
                BorderThickness = 1, BorderColor = ColorScheme.Border,
                Size = new Size(100, 38), Cursor = Cursors.Hand,
            };
            btnHuy.Click += (s, e) => ResetForm();

            _btnInPhieu = new Guna2Button
            {
                Text = "🖨️ In Phiếu",
                Font = AppFonts.BodyBold, ForeColor = ColorScheme.PrimaryDark,
                FillColor = Color.White, BorderRadius = 20,
                BorderThickness = 1, BorderColor = ColorScheme.Primary,
                Size = new Size(130, 38), Cursor = Cursors.Hand,
                Enabled = false,
                DisabledState = { FillColor = Color.FromArgb(240, 240, 240), ForeColor = Color.Gray, BorderColor = Color.LightGray },
            };
            _btnInPhieu.Click += BtnInPhieu_Click;

            var btnLichSu = new Guna2Button
            {
                Text = "📜  Lịch Sử Nhập Kho",
                Font = AppFonts.BodyBold, ForeColor = ColorScheme.PrimaryDark,
                FillColor = ColorScheme.PrimaryPale, BorderRadius = 20,
                BorderThickness = 1, BorderColor = ColorScheme.PrimaryLight,
                Size = new Size(200, 38), Cursor = Cursors.Hand,
            };
            btnLichSu.Click += BtnLichSu_Click;

            pnlBtns.Controls.Add(btnLuu);
            pnlBtns.Controls.Add(_btnInPhieu);
            pnlBtns.Controls.Add(btnLichSu);
            pnlBtns.Controls.Add(btnHuy);
            pnlBtns.Resize += (s, e) =>
            {
                int pw = pnlBtns.Width;
                btnLuu.Location = new Point(pw - 180 - 16, 6);
                _btnInPhieu.Location = new Point(pw - 180 - 16 - 138, 6);
                btnLichSu.Location = new Point(pw - 180 - 16 - 138 - 208, 6);
                btnHuy.Location = new Point(pw - 180 - 16 - 138 - 208 - 108, 6);
            };

            pnl.Controls.Add(pnlBtns);

            return pnl;
        }

        // ══════════════════════════════════════════
        // GRADIENT HEADER PAINT
        // ══════════════════════════════════════════

        private void DgvChiTiet_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // ── Red circle delete button for data rows ──
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvChiTiet.Columns[e.ColumnIndex].Name == "Xoa")
            {
                e.Handled = true;
                var rect = e.CellBounds;

                using (var bg = new SolidBrush(Color.White))
                    e.Graphics.FillRectangle(bg, rect);

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                int sz = 28;
                int cx = rect.X + (rect.Width - sz) / 2;
                int cy = rect.Y + (rect.Height - sz) / 2;
                var circleRect = new Rectangle(cx, cy, sz, sz);

                using (var fill = new SolidBrush(ColorTranslator.FromHtml("#EF4444")))
                    e.Graphics.FillEllipse(fill, circleRect);

                TextRenderer.DrawText(e.Graphics, "✕", new Font("Segoe UI", 10f, FontStyle.Bold),
                    circleRect, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            // ── Rounded-rect border for editable data cells ──
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string colName = dgvChiTiet.Columns[e.ColumnIndex].Name;
                bool isEditable = colName == "Thuoc" || colName == "SoLuong" || colName == "GiaNhap" || colName == "HanSuDung";
                bool isThanhTien = colName == "ThanhTien";

                if (isEditable || isThanhTien)
                {
                    e.Handled = true;
                    var rect = e.CellBounds;
                    var g = e.Graphics;

                    // Clear cell background
                    using (var bg = new SolidBrush(Color.White))
                        g.FillRectangle(bg, rect);

                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    // Inset rect for the rounded box
                    int insetX = 6, insetY = 8;
                    var boxRect = new Rectangle(rect.X + insetX, rect.Y + insetY,
                        rect.Width - insetX * 2, rect.Height - insetY * 2);

                    if (isEditable)
                    {
                        // White fill + border
                        using (var path = TaoRoundedRect(boxRect, 10))
                        {
                            using (var fill = new SolidBrush(Color.White))
                                g.FillPath(fill, path);
                            using (var pen = new Pen(BorderInput, 1.2f))
                                g.DrawPath(pen, path);
                        }
                    }

                    // Draw text value
                    if (e.Value != null && !string.IsNullOrEmpty(e.Value.ToString()))
                    {
                        Color textColor = isThanhTien ? GoldAccent : ColorScheme.TextDark;
                        Font textFont = isThanhTien ? AppFonts.BodyBold : AppFonts.Body;
                        var textRect = new Rectangle(boxRect.X + 8, boxRect.Y, boxRect.Width - 16, boxRect.Height);
                        TextRenderer.DrawText(g, e.Value.ToString(), textFont,
                            textRect, textColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
                    }

                    // Draw dropdown arrow for ComboBox column
                    if (colName == "Thuoc")
                    {
                        var arrowPts = new Point[]
                        {
                            new Point(boxRect.Right - 18, boxRect.Y + boxRect.Height / 2 - 2),
                            new Point(boxRect.Right - 12, boxRect.Y + boxRect.Height / 2 + 3),
                            new Point(boxRect.Right - 6, boxRect.Y + boxRect.Height / 2 - 2),
                        };
                        using (var arrowBr = new SolidBrush(ColorScheme.TextLight))
                            g.FillPolygon(arrowBr, arrowPts);
                    }
                    return;
                }
            }

            // ── Gradient header ──
            if (e.RowIndex != -1) return;

            e.Handled = true;
            var headerRect = e.CellBounds;

            using (var brush = new LinearGradientBrush(
                new Rectangle(0, headerRect.Y, dgvChiTiet.Width, headerRect.Height),
                ColorScheme.PrimaryDark, Color.FromArgb(180, GoldAccent.R, GoldAccent.G, GoldAccent.B),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, headerRect);
            }

            if (e.ColumnIndex == 0)
            {
                using (var br = new SolidBrush(Color.White))
                {
                    e.Graphics.FillRectangle(br, headerRect.X, headerRect.Y, 8, 8);
                    using (var path = new GraphicsPath())
                    {
                        path.AddArc(headerRect.X, headerRect.Y, 16, 16, 180, 90);
                        path.AddLine(headerRect.X + 8, headerRect.Y, headerRect.X, headerRect.Y + 8);
                        path.CloseFigure();
                        using (var brush2 = new LinearGradientBrush(
                            new Rectangle(0, headerRect.Y, dgvChiTiet.Width, headerRect.Height),
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
                var textRect = new Rectangle(headerRect.X + 12, headerRect.Y, headerRect.Width - 12, headerRect.Height);
                TextRenderer.DrawText(e.Graphics, e.Value.ToString(), AppFonts.BodyBold,
                    textRect, Color.White, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            }
        }

        // ══════════════════════════════════════════
        // LOAD COMBO DATA
        // ══════════════════════════════════════════

        // ══════════════════════════════════════════
        // LỊCH SỬ NHẬP KHO
        // ══════════════════════════════════════════

        private void BtnLichSu_Click(object sender, EventArgs e)
        {
            var frm = new Form
            {
                Text = "📜 Lịch Sử Nhập Kho",
                Size = new Size(1000, 600),
                StartPosition = FormStartPosition.CenterParent,
                Font = AppFonts.Body,
                BackColor = ColorScheme.Background,
                FormBorderStyle = FormBorderStyle.Sizable,
                MinimumSize = new Size(780, 450),
            };

            // ── Header panel ──
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 16, 24, 8),
            };
            pnlHeader.Paint += (s2, pe) =>
            {
                using (var br = new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Rectangle(0, 0, pnlHeader.Width, pnlHeader.Height),
                    ColorScheme.PrimaryDark, Color.FromArgb(26, 95, 77),
                    System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
                    pe.Graphics.FillRectangle(br, 0, 0, pnlHeader.Width, pnlHeader.Height);
            };
            var lblTitle = new Label
            {
                Text = "📜  Lịch Sử Phiếu Nhập Kho",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(24, 10),
            };
            pnlHeader.Controls.Add(lblTitle);

            // ── Summary panel ──
            var pnlSummary = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = ColorScheme.PrimaryPale,
                Padding = new Padding(24, 10, 24, 10),
            };
            var lblSummary = new Label
            {
                Font = AppFonts.BodyBold,
                ForeColor = ColorScheme.PrimaryDark,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(24, 14),
            };
            pnlSummary.Controls.Add(lblSummary);

            // ── DataGridView ──
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = AppFonts.Body,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = ColorScheme.Border,
            };
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(245, 251, 247),
                ForeColor = ColorScheme.PrimaryDark,
                Font = AppFonts.BodyBold,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgv.ColumnHeadersHeight = 44;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.EnableHeadersVisualStyles = false;
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ColorScheme.TextDark,
                SelectionBackColor = Color.FromArgb(220, 245, 234),
                SelectionForeColor = ColorScheme.PrimaryDark,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(250, 253, 251),
                SelectionBackColor = Color.FromArgb(220, 245, 234),
                SelectionForeColor = ColorScheme.PrimaryDark,
            };
            dgv.RowTemplate.Height = 40;

            // Hover effect
            int _hoverRow = -1;
            dgv.CellMouseEnter += (s2, ce) =>
            {
                if (ce.RowIndex >= 0 && ce.RowIndex != _hoverRow)
                {
                    _hoverRow = ce.RowIndex;
                    dgv.InvalidateRow(ce.RowIndex);
                }
            };
            dgv.CellMouseLeave += (s2, ce) =>
            {
                if (ce.RowIndex >= 0)
                {
                    int old = _hoverRow;
                    _hoverRow = -1;
                    if (old >= 0 && old < dgv.Rows.Count) dgv.InvalidateRow(old);
                }
            };
            dgv.RowPrePaint += (s2, rpe) =>
            {
                if (rpe.RowIndex == _hoverRow && !dgv.Rows[rpe.RowIndex].Selected)
                {
                    dgv.Rows[rpe.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(232, 248, 240);
                }
                else if (rpe.RowIndex != _hoverRow && !dgv.Rows[rpe.RowIndex].Selected)
                {
                    dgv.Rows[rpe.RowIndex].DefaultCellStyle.BackColor =
                        rpe.RowIndex % 2 == 0 ? Color.White : Color.FromArgb(250, 253, 251);
                }
            };

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaPhieu", HeaderText = "Mã Phiếu", FillWeight = 10 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "NgayNhap", HeaderText = "Ngày Nhập", FillWeight = 12 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "NCC", HeaderText = "Nhà Cung Cấp", FillWeight = 20 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "NguoiNhap", HeaderText = "Người Nhập", FillWeight = 14 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "TongSoLuong", HeaderText = "Tổng SL Nhập", FillWeight = 12 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "TongGiaTri", HeaderText = "Tổng Giá Trị", FillWeight = 14 });

            // Cell formatting
            dgv.CellFormatting += (s2, cf) =>
            {
                if (cf.RowIndex < 0) return;
                string col = dgv.Columns[cf.ColumnIndex].Name;
                if (col == "MaPhieu")
                {
                    cf.CellStyle.Font = AppFonts.Badge;
                    cf.CellStyle.ForeColor = ColorScheme.PrimaryDark;
                }
                else if (col == "TongGiaTri")
                {
                    cf.CellStyle.Font = AppFonts.BodyBold;
                    cf.CellStyle.ForeColor = ColorScheme.PrimaryDark;
                }
                else if (col == "TongSoLuong")
                {
                    cf.CellStyle.Font = AppFonts.BodyBold;
                    cf.CellStyle.ForeColor = ColorScheme.TextMid;
                }
            };

            int totalPhieu = 0;
            int totalSL = 0;
            decimal totalGiaTri = 0;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(@"
                    SELECT pnk.MaPhieuNhap,
                           FORMAT(pnk.NgayNhap, 'dd/MM/yyyy') AS NgayNhap,
                           ISNULL(ncc.TenNhaCungCap, N'—') AS TenNCC,
                           ISNULL(nd.HoTen, N'—') AS NguoiNhap,
                           ISNULL(SUM(ct.SoLuong), 0) AS TongSoLuong,
                           ISNULL(SUM(ct.SoLuong * ct.GiaNhap), 0) AS TongGiaTri
                    FROM PhieuNhapKho pnk
                    LEFT JOIN NhaCungCap ncc ON pnk.MaNhaCungCap = ncc.MaNhaCungCap
                    LEFT JOIN NguoiDung nd ON pnk.MaNguoiDung = nd.MaNguoiDung
                    LEFT JOIN ChiTietNhapKho ct ON pnk.MaPhieuNhap = ct.MaPhieuNhap
                    GROUP BY pnk.MaPhieuNhap, pnk.NgayNhap, ncc.TenNhaCungCap, nd.HoTen
                    ORDER BY pnk.NgayNhap DESC, pnk.MaPhieuNhap DESC", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string maPhieu = "PN#" + Convert.ToInt32(reader.GetValue(0)).ToString("D4");
                        string ngayNhap = reader.GetString(1);
                        string tenNCC = reader.GetString(2);
                        string nguoiNhap = reader.GetString(3);
                        int tongSL = Convert.ToInt32(reader.GetValue(4));
                        decimal tongGiaTri = Convert.ToDecimal(reader.GetValue(5));

                        dgv.Rows.Add(maPhieu, ngayNhap, tenNCC, nguoiNhap,
                            tongSL.ToString("N0"), tongGiaTri.ToString("N0") + "đ");

                        totalPhieu++;
                        totalSL += tongSL;
                        totalGiaTri += tongGiaTri;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            lblSummary.Text = $"📊  Tổng: {totalPhieu} phiếu  |  {totalSL:N0} sản phẩm  |  {totalGiaTri:N0}đ";

            // Dock order: Fill first, then Top
            frm.Controls.Add(dgv);
            frm.Controls.Add(pnlSummary);
            frm.Controls.Add(pnlHeader);
            frm.ShowDialog(this);
        }

        private void LoadComboData()
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new SqlCommand(
                        "SELECT MaNhaCungCap, TenNhaCungCap FROM NhaCungCap ORDER BY TenNhaCungCap", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        _dsNCC.Clear();
                        while (reader.Read())
                            _dsNCC.Add(new KeyValuePair<int, string>(
                                Convert.ToInt32(reader.GetValue(0)), reader.GetString(1)));
                    }

                    using (var cmd = new SqlCommand(
                        "SELECT MaThuoc, TenThuoc + ' (' + DonViTinh + ')' FROM Thuoc WHERE IsDeleted = 0 ORDER BY TenThuoc", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        _dsThuoc.Clear();
                        while (reader.Read())
                            _dsThuoc.Add(new KeyValuePair<int, string>(
                                Convert.ToInt32(reader.GetValue(0)), reader.GetString(1)));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            cboNCC.Items.Clear();
            cboNCC.Items.Add("-- Chọn NCC --");
            foreach (var ncc in _dsNCC) cboNCC.Items.Add(ncc.Value);
            cboNCC.SelectedIndex = 0;

            // Cập nhật combo trong grid
            var colThuoc = dgvChiTiet.Columns["Thuoc"] as DataGridViewComboBoxColumn;
            if (colThuoc != null)
            {
                colThuoc.Items.Clear();
                foreach (var t in _dsThuoc) colThuoc.Items.Add(t.Value);
            }
        }

        private void GenMaPhieu()
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand("SELECT ISNULL(MAX(MaPhieuNhap), 0) + 1 FROM PhieuNhapKho", conn))
                {
                    int next = Convert.ToInt32(cmd.ExecuteScalar());
                    txtMaPhieu.Text = "PN#" + next.ToString("D4");
                }
            }
            catch { txtMaPhieu.Text = "PN#0001"; }
        }

        // ══════════════════════════════════════════
        // THÊM DÒNG MỚI
        // ══════════════════════════════════════════

        private void ThemDongMoi()
        {
            int idx = dgvChiTiet.Rows.Add();
            dgvChiTiet.Rows[idx].Cells["SoLuong"].Value = "1";
            dgvChiTiet.Rows[idx].Cells["GiaNhap"].Value = "0";
            dgvChiTiet.Rows[idx].Cells["HanSuDung"].Value = "";
            dgvChiTiet.Rows[idx].Cells["ThanhTien"].Value = "0đ";
        }

        // ══════════════════════════════════════════
        // TÍNH THÀNH TIỀN KHI EDIT
        // ══════════════════════════════════════════

        private void DgvChiTiet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvChiTiet.Columns[e.ColumnIndex].Name;

            if (colName == "SoLuong" || colName == "GiaNhap")
            {
                TinhThanhTienDong(e.RowIndex);
                TinhTong();
            }
        }

        private void TinhThanhTienDong(int rowIdx)
        {
            var row = dgvChiTiet.Rows[rowIdx];
            int sl = 0; decimal gia = 0;
            int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out sl);
            decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), out gia);
            decimal thanhtien = sl * gia;
            row.Cells["ThanhTien"].Value = thanhtien.ToString("N0") + "đ";
        }

        private void TinhTong()
        {
            decimal tong = 0;
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                int sl = 0; decimal gia = 0;
                int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out sl);
                decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), out gia);
                tong += sl * gia;
            }
            lblTongGiaTri.Text = tong.ToString("N0") + "đ";
        }

        // ══════════════════════════════════════════
        // XÓA DÒNG
        // ══════════════════════════════════════════

        private void DgvChiTiet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvChiTiet.Columns[e.ColumnIndex].Name == "Xoa")
            {
                if (dgvChiTiet.Rows.Count <= 1)
                {
                    MessageBox.Show("Phải có ít nhất 1 dòng chi tiết.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                dgvChiTiet.Rows.RemoveAt(e.RowIndex);
                TinhTong();
            }
        }

        // ══════════════════════════════════════════
        // CELL FORMATTING
        // ══════════════════════════════════════════

        private void DgvChiTiet_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvChiTiet.Columns[e.ColumnIndex].Name;

            if (colName == "ThanhTien")
            {
                e.CellStyle.Font = AppFonts.BodyBold;
                e.CellStyle.ForeColor = GoldAccent;
            }
            if (colName == "Xoa")
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.SelectionBackColor = Color.White;
                e.CellStyle.SelectionForeColor = Color.White;
                e.CellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            }
        }

        // ══════════════════════════════════════════
        // LƯU PHIẾU NHẬP
        // ══════════════════════════════════════════

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            // Validate NCC
            if (cboNCC.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate chi tiết
            if (dgvChiTiet.Rows.Count == 0)
            {
                MessageBox.Show("Chưa có dòng chi tiết nào.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate từng dòng
            for (int i = 0; i < dgvChiTiet.Rows.Count; i++)
            {
                var row = dgvChiTiet.Rows[i];
                var thuocVal = row.Cells["Thuoc"].Value;
                if (thuocVal == null || string.IsNullOrEmpty(thuocVal.ToString()))
                {
                    MessageBox.Show("Dòng " + (i + 1) + ": Chưa chọn thuốc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int sl = 0;
                if (!int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out sl) || sl <= 0)
                {
                    MessageBox.Show("Dòng " + (i + 1) + ": Số lượng phải > 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal gia = 0;
                if (!decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), out gia) || gia < 0)
                {
                    MessageBox.Show("Dòng " + (i + 1) + ": Giá nhập không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string hsd = row.Cells["HanSuDung"].Value?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(hsd))
                {
                    MessageBox.Show("Dòng " + (i + 1) + ": Hạn sử dụng không được trống.\nĐịnh dạng: MM/yyyy", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            int maNCC = _dsNCC[cboNCC.SelectedIndex - 1].Key;
            var nd = LoginForm.NguoiDungHienTai;
            int maNguoiDung = nd != null ? nd.MaNguoiDung : 1;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert PhieuNhapKho
                        int maPhieu;
                        using (var cmd = new SqlCommand(
                            @"INSERT INTO PhieuNhapKho (MaNhaCungCap, MaNguoiDung, NgayNhap, TongGiaTri)
                              VALUES (@NCC, @NV, @Ngay, 0);
                              SELECT SCOPE_IDENTITY();", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@NCC", maNCC);
                            cmd.Parameters.AddWithValue("@NV", maNguoiDung);
                            cmd.Parameters.AddWithValue("@Ngay", dtpNgayNhap.Value.Date);
                            maPhieu = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        decimal tongGia = 0;

                        // Insert ChiTietNhapKho
                        foreach (DataGridViewRow row in dgvChiTiet.Rows)
                        {
                            string thuocDisplay = row.Cells["Thuoc"].Value.ToString();
                            int maThuoc = 0;
                            for (int i = 0; i < _dsThuoc.Count; i++)
                            {
                                if (_dsThuoc[i].Value == thuocDisplay)
                                {
                                    maThuoc = _dsThuoc[i].Key;
                                    break;
                                }
                            }

                            int sl = int.Parse(row.Cells["SoLuong"].Value.ToString());
                            decimal gia = decimal.Parse(row.Cells["GiaNhap"].Value.ToString());
                            string hsdText = row.Cells["HanSuDung"].Value.ToString().Trim();

                            // Parse HSD: MM/yyyy → last day of month
                            DateTime hsd;
                            if (!DateTime.TryParseExact(hsdText, new[] { "MM/yyyy", "M/yyyy" },
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None, out hsd))
                            {
                                hsd = DateTime.Parse(hsdText);
                            }
                            else
                            {
                                hsd = new DateTime(hsd.Year, hsd.Month, DateTime.DaysInMonth(hsd.Year, hsd.Month));
                            }

                            using (var cmd = new SqlCommand(
                                @"INSERT INTO ChiTietNhapKho (MaPhieuNhap, MaThuoc, SoLuong, SoLuongConLai, GiaNhap, HanSuDung)
                                  VALUES (@MP, @MT, @SL, @SL, @Gia, @HSD)", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@MP", maPhieu);
                                cmd.Parameters.AddWithValue("@MT", maThuoc);
                                cmd.Parameters.AddWithValue("@SL", sl);
                                cmd.Parameters.AddWithValue("@Gia", gia);
                                cmd.Parameters.AddWithValue("@HSD", hsd);
                                cmd.ExecuteNonQuery();
                            }

                            tongGia += sl * gia;
                        }

                        // Update TongGiaTri
                        using (var cmd = new SqlCommand(
                            "UPDATE PhieuNhapKho SET TongGiaTri = @Tong WHERE MaPhieuNhap = @Ma", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@Tong", tongGia);
                            cmd.Parameters.AddWithValue("@Ma", maPhieu);
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        _lastSavedMaPhieu = maPhieu;
                        _btnInPhieu.Enabled = true;
                        MessageBox.Show("Lưu phiếu nhập PN#" + maPhieu.ToString("D4") + " thành công!\nTổng: " + tongGia.ToString("N0") + "đ\n\nBấm \"In Phiếu\" để in phiếu nhập kho.",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetForm();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════
        // RESET FORM
        // ══════════════════════════════════════════

        private void ResetForm()
        {
            cboNCC.SelectedIndex = 0;
            dtpNgayNhap.Value = DateTime.Today;
            dgvChiTiet.Rows.Clear();
            lblTongGiaTri.Text = "0đ";
            GenMaPhieu();
            ThemDongMoi();
        }

        // ══════════════════════════════════════════
        // IN PHIẾU NHẬP KHO
        // ══════════════════════════════════════════

        private void BtnInPhieu_Click(object sender, EventArgs e)
        {
            if (_lastSavedMaPhieu <= 0)
            {
                MessageBox.Show("Chưa có phiếu nhập nào để in.\nVui lòng lưu phiếu trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var printDoc = new PrintDocument();
                printDoc.DocumentName = "PhieuNhapKho_PN" + _lastSavedMaPhieu.ToString("D4");
                printDoc.PrintPage += PrintDoc_PrintPage;

                var preview = new PrintPreviewDialog
                {
                    Document = printDoc,
                    Width = 900,
                    Height = 700,
                    Text = "Xem Trước Phiếu Nhập Kho — PN#" + _lastSavedMaPhieu.ToString("D4"),
                };
                preview.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            float pageW = e.MarginBounds.Width;
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;

            var fontTitle = new Font("Segoe UI", 16f, FontStyle.Bold);
            var fontSubTitle = new Font("Segoe UI", 10f, FontStyle.Regular);
            var fontHeader = new Font("Segoe UI", 9f, FontStyle.Bold);
            var fontBody = new Font("Segoe UI", 9f, FontStyle.Regular);
            var fontTotal = new Font("Segoe UI", 12f, FontStyle.Bold);
            var brDark = new SolidBrush(ColorScheme.PrimaryDark);
            var brText = new SolidBrush(ColorScheme.TextDark);
            var brGold = new SolidBrush(GoldAccent);
            var penLine = new Pen(BorderInput, 1f);

            try
            {
                // Load data from DB
                string tenNCC = "", nguoiNhap = "", ngayNhap = "";
                decimal tongGia = 0;
                var chiTiet = new List<string[]>();

                using (var conn = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new SqlCommand(
                        @"SELECT ncc.TenNhaCungCap, nd.HoTen, pn.NgayNhap, pn.TongGiaTri
                          FROM PhieuNhapKho pn
                          JOIN NhaCungCap ncc ON pn.MaNhaCungCap = ncc.MaNhaCungCap
                          JOIN NguoiDung nd ON pn.MaNguoiDung = nd.MaNguoiDung
                          WHERE pn.MaPhieuNhap = @Ma", conn))
                    {
                        cmd.Parameters.AddWithValue("@Ma", _lastSavedMaPhieu);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tenNCC = reader.GetString(0);
                                nguoiNhap = reader.GetString(1);
                                ngayNhap = reader.GetDateTime(2).ToString("dd/MM/yyyy");
                                tongGia = Convert.ToDecimal(reader.GetValue(3));
                            }
                        }
                    }

                    using (var cmd = new SqlCommand(
                        @"SELECT t.TenThuoc, t.DonViTinh, ct.SoLuong, ct.GiaNhap, ct.HanSuDung,
                                 ct.SoLuong * ct.GiaNhap AS ThanhTien
                          FROM ChiTietNhapKho ct
                          JOIN Thuoc t ON ct.MaThuoc = t.MaThuoc
                          WHERE ct.MaPhieuNhap = @Ma", conn))
                    {
                        cmd.Parameters.AddWithValue("@Ma", _lastSavedMaPhieu);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                chiTiet.Add(new string[]
                                {
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    Convert.ToInt32(reader.GetValue(2)).ToString(),
                                    Convert.ToDecimal(reader.GetValue(3)).ToString("N0"),
                                    reader.GetDateTime(4).ToString("MM/yyyy"),
                                    Convert.ToDecimal(reader.GetValue(5)).ToString("N0") + "đ",
                                });
                            }
                        }
                    }
                }

                // ── HEADER ──
                string maPhieuStr = "PN#" + _lastSavedMaPhieu.ToString("D4");
                g.DrawString("PHIẾU NHẬP KHO", fontTitle, brDark, x + pageW / 2 - 100, y);
                y += 30;
                g.DrawString(maPhieuStr, fontTotal, brGold, x + pageW / 2 - 40, y);
                y += 30;
                g.DrawLine(new Pen(ColorScheme.PrimaryDark, 2f), x, y, x + pageW, y);
                y += 12;

                // ── INFO ──
                g.DrawString("Nhà cung cấp:", fontHeader, brDark, x, y);
                g.DrawString(tenNCC, fontBody, brText, x + 110, y);
                g.DrawString("Ngày nhập:", fontHeader, brDark, x + pageW / 2, y);
                g.DrawString(ngayNhap, fontBody, brText, x + pageW / 2 + 80, y);
                y += 22;
                g.DrawString("Người nhập:", fontHeader, brDark, x, y);
                g.DrawString(nguoiNhap, fontBody, brText, x + 110, y);
                y += 30;

                // ── TABLE HEADER ──
                float[] colWidths = { pageW * 0.05f, pageW * 0.28f, pageW * 0.10f, pageW * 0.10f, pageW * 0.15f, pageW * 0.14f, pageW * 0.18f };
                string[] headers = { "STT", "Tên thuốc", "ĐVT", "SL", "Giá nhập", "HSD", "Thành tiền" };

                using (var headerBr = new SolidBrush(ColorScheme.PrimaryDark))
                    g.FillRectangle(headerBr, x, y, pageW, 24);

                float hx = x;
                for (int i = 0; i < headers.Length; i++)
                {
                    g.DrawString(headers[i], fontHeader, Brushes.White, hx + 4, y + 4);
                    hx += colWidths[i];
                }
                y += 26;

                // ── TABLE ROWS ──
                for (int r = 0; r < chiTiet.Count; r++)
                {
                    var row = chiTiet[r];
                    if (r % 2 == 1)
                    {
                        using (var altBr = new SolidBrush(RowAlt))
                            g.FillRectangle(altBr, x, y, pageW, 22);
                    }

                    float rx = x;
                    g.DrawString((r + 1).ToString(), fontBody, brText, rx + 4, y + 3);
                    rx += colWidths[0];
                    g.DrawString(row[0], fontBody, brText, rx + 4, y + 3); // TenThuoc
                    rx += colWidths[1];
                    g.DrawString(row[1], fontBody, brText, rx + 4, y + 3); // DVT
                    rx += colWidths[2];
                    g.DrawString(row[2], fontBody, brText, rx + 4, y + 3); // SL
                    rx += colWidths[3];
                    g.DrawString(row[3], fontBody, brText, rx + 4, y + 3); // Gia
                    rx += colWidths[4];
                    g.DrawString(row[4], fontBody, brText, rx + 4, y + 3); // HSD
                    rx += colWidths[5];
                    g.DrawString(row[5], fontHeader, brGold, rx + 4, y + 3); // ThanhTien

                    y += 22;
                    g.DrawLine(penLine, x, y, x + pageW, y);
                }

                // ── TOTAL ──
                y += 14;
                g.DrawLine(new Pen(ColorScheme.PrimaryDark, 1.5f), x + pageW * 0.55f, y, x + pageW, y);
                y += 8;
                g.DrawString("TỔNG GIÁ TRỊ PHIẾU:", fontHeader, brDark, x + pageW * 0.55f, y);
                g.DrawString(tongGia.ToString("N0") + "đ", fontTotal, brGold, x + pageW * 0.78f, y - 2);

                // ── FOOTER ──
                y += 40;
                g.DrawString("Người nhập kho", fontHeader, brDark, x + 40, y);
                g.DrawString("Thủ kho", fontHeader, brDark, x + pageW - 140, y);
                y += 16;
                g.DrawString("(Ký, ghi rõ họ tên)", fontSubTitle, brText, x + 25, y);
                g.DrawString("(Ký, ghi rõ họ tên)", fontSubTitle, brText, x + pageW - 155, y);
            }
            finally
            {
                fontTitle.Dispose();
                fontSubTitle.Dispose();
                fontHeader.Dispose();
                fontBody.Dispose();
                fontTotal.Dispose();
                brDark.Dispose();
                brText.Dispose();
                brGold.Dispose();
                penLine.Dispose();
            }

            e.HasMorePages = false;
        }

        // ══════════════════════════════════════════
        // HELPERS
        // ══════════════════════════════════════════

        private Label TaoLabel(string text, Point loc)
        {
            return new Label
            {
                Text = text,
                Font = AppFonts.BodyBold, ForeColor = ColorScheme.PrimaryDark,
                Location = loc, AutoSize = true, BackColor = Color.Transparent,
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
