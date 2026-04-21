using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    public partial class ThuocForm : Form
    {
        // ── Controls chính ──
        private Panel pnlLeft;
        private Panel pnlRight;
        private DataGridView dgvThuoc;
        private Guna2TextBox txtTimKiem;

        // ── Form bên phải ──
        private Guna2TextBox txtTenThuoc;
        private Guna2TextBox txtDonViTinh;
        private Guna2TextBox txtDonGia;
        private Guna2TextBox txtGhiChu;
        private Guna2Button btnLuu;
        private Guna2Button btnXoa;
        private Label lblFormTitle;
        private Label lblError;
        private int _maThuocDangChon = -1;

        // Màu wireframe (đồng bộ StaffForm)
        private static readonly Color BorderInput = ColorTranslator.FromHtml("#D1E8DC");
        private static readonly Color InputBg = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color BadgeActiveBg = ColorTranslator.FromHtml("#DCFCE7");
        private static readonly Color BadgeActiveFg = ColorTranslator.FromHtml("#166534");
        private static readonly Color BadgeWarningBg = ColorTranslator.FromHtml("#FEF3C7");
        private static readonly Color BadgeWarningFg = ColorTranslator.FromHtml("#D97706");
        private static readonly Color BadgeDangerBg = ColorTranslator.FromHtml("#FEE2E2");
        private static readonly Color BadgeDangerFg = ColorTranslator.FromHtml("#DC2626");
        private static readonly Color GoldAccent = Color.FromArgb(184, 138, 40);
        private static readonly Color RowAlt = ColorTranslator.FromHtml("#F5FBF7");
        private static readonly Color RowOdd = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color GridBorderColor = ColorTranslator.FromHtml("#EEF6F1");
        private static readonly Color NotifChipBg = ColorScheme.PrimaryPale;

        public ThuocForm()
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

            var pnlBody = new Panel { Dock = DockStyle.Fill, BackColor = ColorScheme.Background };
            this.Controls.Add(pnlBody);

            pnlRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 420,
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
        // PANEL TRÁI — Search + Filter + DataGridView
        // ══════════════════════════════════════════

        private void TaoPanelTrai()
        {
            var pnlFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = 42,
                BackColor = Color.Transparent,
            };

            var pnlSearchWrap = new Panel { BackColor = Color.Transparent };
            var pnlBtnThemWrap = new Panel { BackColor = Color.Transparent };
            var pnlBtnExcelWrap = new Panel { BackColor = Color.Transparent };

            var btnTaoMoi = new Guna2Button
            {
                Text = "➕ Thêm Thuốc",
                Font = AppFonts.BodyBold,
                ForeColor = Color.White,
                FillColor = ColorScheme.Primary,
                BorderRadius = 18,
                Dock = DockStyle.Top,
                Height = 36,
                Cursor = Cursors.Hand,
            };
            btnTaoMoi.Click += (s, e) => ResetForm();
            pnlBtnThemWrap.Controls.Add(btnTaoMoi);

            var btnXuatExcel = new Guna2Button
            {
                Text = "📥 Xuất Excel",
                Font = AppFonts.BodyBold,
                ForeColor = ColorScheme.PrimaryDark,
                FillColor = Color.White,
                BorderRadius = 18,
                BorderThickness = 1,
                BorderColor = ColorScheme.Primary,
                Dock = DockStyle.Top,
                Height = 36,
                Cursor = Cursors.Hand,
            };
            btnXuatExcel.Click += BtnXuatExcel_Click;
            pnlBtnExcelWrap.Controls.Add(btnXuatExcel);

            txtTimKiem = new Guna2TextBox
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Dock = DockStyle.Top,
                Height = 36,
                PlaceholderText = "🔍 Tìm theo tên thuốc, đơn vị tính...",
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

            pnlFilter.Controls.Add(pnlSearchWrap);
            pnlFilter.Controls.Add(pnlBtnExcelWrap);
            pnlFilter.Controls.Add(pnlBtnThemWrap);

            const int BTN_THEM_W = 160, BTN_EXCEL_W = 140, GAP = 8, H = 42;
            EventHandler boTriFilter = (s, e) =>
            {
                int pw = pnlFilter.ClientSize.Width;
                if (pw <= 0) return;
                pnlBtnThemWrap.SetBounds(pw - BTN_THEM_W, 0, BTN_THEM_W, H);
                pnlBtnExcelWrap.SetBounds(pw - BTN_THEM_W - GAP - BTN_EXCEL_W, 0, BTN_EXCEL_W, H);
                int searchW = Math.Max(50, pw - BTN_THEM_W - BTN_EXCEL_W - GAP * 2);
                pnlSearchWrap.SetBounds(0, 0, searchW, H);
            };
            pnlFilter.Resize += boTriFilter;
            pnlFilter.Layout += (s, e) => boTriFilter(s, e);

            var pnlSpacer = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // ── DataGridView ──
            dgvThuoc = new DataGridView
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
            dgvThuoc.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorScheme.PrimaryDark,
                ForeColor = Color.White,
                Font = AppFonts.BodyBold,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvThuoc.ColumnHeadersHeight = 40;
            dgvThuoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvThuoc.EnableHeadersVisualStyles = false;
            dgvThuoc.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowOdd,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvThuoc.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowAlt,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
            };

            dgvThuoc.CellPainting += DgvThuoc_CellPainting;

            dgvThuoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaThuoc", HeaderText = "Mã", FillWeight = 8 });
            dgvThuoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenThuoc", HeaderText = "Tên thuốc / Mỹ phẩm", FillWeight = 28 });
            dgvThuoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonViTinh", HeaderText = "Đơn vị tính", FillWeight = 14 });
            dgvThuoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonGia", HeaderText = "Đơn giá", FillWeight = 16 });
            dgvThuoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "TonKho", HeaderText = "Tồn kho", FillWeight = 12 });
            dgvThuoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThaoTac", HeaderText = "Thao tác", FillWeight = 10 });

            // Hidden
            dgvThuoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaThuocHidden", Visible = false });
            dgvThuoc.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonGiaRaw", Visible = false });

            dgvThuoc.CellClick += DgvThuoc_CellClick;
            dgvThuoc.CellFormatting += DgvThuoc_CellFormatting;
            dgvThuoc.CellMouseMove += DgvThuoc_CellMouseMove;

            pnlLeft.Controls.Add(dgvThuoc);
            pnlLeft.Controls.Add(pnlSpacer);
            pnlLeft.Controls.Add(pnlFilter);
        }

        // ══════════════════════════════════════════
        // HEADER GRADIENT PAINT
        // ══════════════════════════════════════════

        private void DgvThuoc_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1) return;

            e.Handled = true;
            var rect = e.CellBounds;

            using (var brush = new LinearGradientBrush(
                new Rectangle(0, rect.Y, dgvThuoc.Width, rect.Height),
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
                            new Rectangle(0, rect.Y, dgvThuoc.Width, rect.Height),
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
        // PANEL PHẢI — Form Thêm/Sửa (card style)
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
            int W = 368;
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
                Text = "💊 Thêm Thuốc / Mỹ Phẩm",
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

            // ── Tên thuốc ──
            pnlCard.Controls.Add(TaoLabel("Tên thuốc / Mỹ phẩm", new Point(X, y), true));
            y += 20;
            txtTenThuoc = TaoGuna2TextBox(new Point(X, y), new Size(W, 36), "Nhập tên thuốc hoặc mỹ phẩm");
            pnlCard.Controls.Add(txtTenThuoc);
            y += 42;

            // ── Đơn vị tính ──
            pnlCard.Controls.Add(TaoLabel("Đơn vị tính", new Point(X, y), true));
            y += 20;
            txtDonViTinh = TaoGuna2TextBox(new Point(X, y), new Size(W, 36), "VD: Viên, Hộp, Chai, Tuýp...");
            pnlCard.Controls.Add(txtDonViTinh);
            y += 42;

            // ── Đơn giá ──
            pnlCard.Controls.Add(TaoLabel("Đơn giá (VNĐ)", new Point(X, y), true));
            y += 20;
            txtDonGia = TaoGuna2TextBox(new Point(X, y), new Size(W, 36), "VD: 50000");
            pnlCard.Controls.Add(txtDonGia);
            y += 42;

            // ── Ghi chú ──
            pnlCard.Controls.Add(TaoLabel("Ghi chú", new Point(X, y)));
            y += 20;
            txtGhiChu = TaoGuna2TextBox(new Point(X, y), new Size(W, 70), "Ghi chú thêm (không bắt buộc)");
            pnlCard.Controls.Add(txtGhiChu);
            y += 76;

            // ── Notif chip ──
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
                using (var pen = new Pen(ColorScheme.Primary, 3f))
                    g.DrawLine(pen, 2, 4, 2, pnlNote.Height - 4);
            };
            pnlNote.Controls.Add(new Label
            {
                Text = "ℹ️ Tồn kho được cập nhật qua Nhập Kho, không sửa trực tiếp.",
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

            // ── Nút Lưu ──
            btnLuu = new Guna2Button
            {
                Text = "💾 Lưu Thuốc",
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
                Text = "🗑️ Xóa Thuốc",
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
        // LOAD DANH SÁCH
        // ══════════════════════════════════════════

        private void LoadDanhSach()
        {
            dgvThuoc.Rows.Clear();

            string keyword = txtTimKiem?.Text.Trim() ?? "";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    @"SELECT t.MaThuoc, t.TenThuoc, t.DonViTinh, t.DonGia, 
                             ISNULL(ton.TonThucTe, 0) AS SoLuongTon
                      FROM Thuoc t
                      LEFT JOIN (
                          SELECT MaThuoc, SUM(SoLuongConLai) AS TonThucTe
                          FROM ChiTietNhapKho
                          WHERE SoLuongConLai > 0
                          GROUP BY MaThuoc
                      ) ton ON t.MaThuoc = ton.MaThuoc
                      WHERE t.IsDeleted = 0
                        AND (@Keyword = '' 
                             OR t.TenThuoc LIKE '%' + @Keyword + '%'
                             OR t.DonViTinh LIKE '%' + @Keyword + '%')
                      ORDER BY t.TenThuoc", conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", keyword);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int maThuoc = Convert.ToInt32(reader.GetValue(0));
                            string tenThuoc = reader.GetString(1);
                            string donViTinh = reader.GetString(2);
                            decimal donGia = Convert.ToDecimal(reader.GetValue(3));
                            int soLuongTon = Convert.ToInt32(reader.GetValue(4));

                            string maDisplay = "T" + maThuoc.ToString("D3");
                            string giaDisplay = donGia.ToString("N0") + "đ";

                            string tonDisplay;
                            if (soLuongTon == 0) tonDisplay = "❌ Hết hàng";
                            else if (soLuongTon <= 5) tonDisplay = "⚠️ " + soLuongTon;
                            else tonDisplay = "✅ " + soLuongTon;

                            int idx = dgvThuoc.Rows.Add(maDisplay, tenThuoc, donViTinh, giaDisplay, tonDisplay, "");
                            dgvThuoc.Rows[idx].Cells["MaThuocHidden"].Value = maThuoc;
                            dgvThuoc.Rows[idx].Cells["DonGiaRaw"].Value = donGia;
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

        private void DgvThuoc_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvThuoc.Columns[e.ColumnIndex].Name;

            if (colName == "TenThuoc")
            {
                e.CellStyle.Font = AppFonts.BodyBold;
                e.CellStyle.ForeColor = ColorScheme.TextDark;
            }

            if (colName == "DonGia")
            {
                e.CellStyle.Font = AppFonts.BodyBold;
                e.CellStyle.ForeColor = GoldAccent;
            }

            if (colName == "TonKho" && e.Value != null)
            {
                string v = e.Value.ToString();
                if (v.Contains("Hết"))
                {
                    e.CellStyle.BackColor = BadgeDangerBg;
                    e.CellStyle.ForeColor = BadgeDangerFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else if (v.Contains("⚠"))
                {
                    e.CellStyle.BackColor = BadgeWarningBg;
                    e.CellStyle.ForeColor = BadgeWarningFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else
                {
                    e.CellStyle.BackColor = BadgeActiveBg;
                    e.CellStyle.ForeColor = BadgeActiveFg;
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

        private void DgvThuoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int maThuoc = Convert.ToInt32(dgvThuoc.Rows[e.RowIndex].Cells["MaThuocHidden"].Value);
            LoadThongTinVaoForm(e.RowIndex, maThuoc);
        }

        private void DgvThuoc_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvThuoc.Columns[e.ColumnIndex].Name;
            dgvThuoc.Cursor = colName == "ThaoTac" ? Cursors.Hand : Cursors.Default;
        }

        // ══════════════════════════════════════════
        // LOAD THÔNG TIN VÀO FORM PHẢI
        // ══════════════════════════════════════════

        private void LoadThongTinVaoForm(int rowIndex, int maThuoc)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    "SELECT TenThuoc, DonViTinh, DonGia FROM Thuoc WHERE MaThuoc = @Ma", conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", maThuoc);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtTenThuoc.Text = reader.GetString(0);
                            txtDonViTinh.Text = reader.GetString(1);
                            txtDonGia.Text = Convert.ToDecimal(reader.GetValue(2)).ToString("0");
                        }
                    }
                }
            }
            catch { }

            txtGhiChu.Text = "";
            _maThuocDangChon = maThuoc;
            lblFormTitle.Text = "✏️ Sửa Thuốc — T" + maThuoc.ToString("D3");
            btnLuu.Text = "💾  Cập Nhật";
            btnXoa.Enabled = true;
            lblError.Text = "";
        }

        private void ResetForm()
        {
            _maThuocDangChon = -1;
            txtTenThuoc.Text = "";
            txtDonViTinh.Text = "";
            txtDonGia.Text = "";
            txtGhiChu.Text = "";
            lblFormTitle.Text = "💊 Thêm Thuốc / Mỹ Phẩm";
            btnLuu.Text = "💾  Lưu Thuốc";
            btnXoa.Enabled = false;
            lblError.Text = "";
        }

        // ══════════════════════════════════════════
        // NÚT LƯU / CẬP NHẬT
        // ══════════════════════════════════════════

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            string tenThuoc = txtTenThuoc.Text.Trim();
            string donViTinh = txtDonViTinh.Text.Trim();
            string donGiaText = txtDonGia.Text.Trim();

            if (string.IsNullOrEmpty(tenThuoc))
            {
                lblError.Text = "Tên thuốc không được để trống.";
                return;
            }
            if (string.IsNullOrEmpty(donViTinh))
            {
                lblError.Text = "Đơn vị tính không được để trống.";
                return;
            }
            if (!decimal.TryParse(donGiaText, out decimal donGia) || donGia < 0)
            {
                lblError.Text = "Đơn giá phải là số >= 0.";
                return;
            }

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    if (_maThuocDangChon == -1)
                    {
                        // Kiểm tra trùng tên
                        using (var chk = new SqlCommand(
                            "SELECT COUNT(*) FROM Thuoc WHERE TenThuoc = @Ten AND IsDeleted = 0", conn))
                        {
                            chk.Parameters.AddWithValue("@Ten", tenThuoc);
                            int count = Convert.ToInt32(chk.ExecuteScalar());
                            if (count > 0)
                            {
                                lblError.Text = "Thuốc \"" + tenThuoc + "\" đã tồn tại.";
                                return;
                            }
                        }

                        // TẠO MỚI
                        using (var cmd = new SqlCommand(
                            @"INSERT INTO Thuoc (TenThuoc, DonViTinh, DonGia, SoLuongTon)
                              VALUES (@Ten, @DVT, @Gia, 0)", conn))
                        {
                            cmd.Parameters.AddWithValue("@Ten", tenThuoc);
                            cmd.Parameters.AddWithValue("@DVT", donViTinh);
                            cmd.Parameters.AddWithValue("@Gia", donGia);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Thêm thuốc thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Kiểm tra trùng tên (trừ bản ghi hiện tại)
                        using (var chk = new SqlCommand(
                            "SELECT COUNT(*) FROM Thuoc WHERE TenThuoc = @Ten AND MaThuoc <> @Ma AND IsDeleted = 0", conn))
                        {
                            chk.Parameters.AddWithValue("@Ten", tenThuoc);
                            chk.Parameters.AddWithValue("@Ma", _maThuocDangChon);
                            int count = Convert.ToInt32(chk.ExecuteScalar());
                            if (count > 0)
                            {
                                lblError.Text = "Thuốc \"" + tenThuoc + "\" đã tồn tại.";
                                return;
                            }
                        }

                        // CẬP NHẬT
                        using (var cmd = new SqlCommand(
                            @"UPDATE Thuoc SET TenThuoc = @Ten, DonViTinh = @DVT, DonGia = @Gia
                              WHERE MaThuoc = @Ma", conn))
                        {
                            cmd.Parameters.AddWithValue("@Ten", tenThuoc);
                            cmd.Parameters.AddWithValue("@DVT", donViTinh);
                            cmd.Parameters.AddWithValue("@Gia", donGia);
                            cmd.Parameters.AddWithValue("@Ma", _maThuocDangChon);
                            int rows = cmd.ExecuteNonQuery();
                            if (rows == 0)
                            {
                                lblError.Text = "Không tìm thấy thuốc để cập nhật.";
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
            if (_maThuocDangChon == -1) return;

            var result = MessageBox.Show(
                "Xóa thuốc T" + _maThuocDangChon.ToString("D3") + "?\nThuốc đã nhập kho hoặc đã kê đơn sẽ không xóa được.",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand("UPDATE Thuoc SET IsDeleted = 1 WHERE MaThuoc = @Ma", conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", _maThuocDangChon);
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
        // XUẤT EXCEL (CSV UTF-8)
        // ══════════════════════════════════════════

        private void BtnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dgvThuoc.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files (*.csv)|*.csv";
                sfd.FileName = "DanhMucThuoc_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";
                sfd.Title = "Xuất Danh Mục Thuốc";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var sb = new StringBuilder();
                    // BOM cho Excel đọc UTF-8
                    sb.AppendLine("Mã,Tên thuốc / Mỹ phẩm,Đơn vị tính,Đơn giá,Tồn kho");

                    foreach (DataGridViewRow row in dgvThuoc.Rows)
                    {
                        string ma = row.Cells["MaThuoc"].Value?.ToString() ?? "";
                        string ten = row.Cells["TenThuoc"].Value?.ToString() ?? "";
                        string dvt = row.Cells["DonViTinh"].Value?.ToString() ?? "";
                        string gia = row.Cells["DonGiaRaw"].Value?.ToString() ?? "0";
                        string ton = row.Cells["TonKho"].Value?.ToString() ?? "0";

                        // Escape CSV
                        ten = "\"" + ten.Replace("\"", "\"\"") + "\"";

                        sb.AppendLine(ma + "," + ten + "," + dvt + "," + gia + "," + ton);
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));
                    MessageBox.Show("Xuất file thành công!\n" + sfd.FileName, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                Cursor = Cursors.IBeam,
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
