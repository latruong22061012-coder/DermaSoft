using System;
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
    public partial class ServiceForm : Form
    {
        // ── Controls chính ──
        private Panel pnlLeft;
        private Panel pnlRight;
        private DataGridView dgvDichVu;
        private Guna2TextBox txtTimKiem;

        // ── Form bên phải ──
        private Guna2TextBox txtTenDichVu;
        private Guna2TextBox txtDonGia;
        private Guna2Button btnLuu;
        private Guna2Button btnXoa;
        private Label lblFormTitle;
        private Label lblError;
        private int _maDichVuDangChon = -1;

        // Màu wireframe (đồng bộ ThuocForm)
        private static readonly Color BorderInput = ColorTranslator.FromHtml("#D1E8DC");
        private static readonly Color InputBg = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color GoldAccent = Color.FromArgb(184, 138, 40);
        private static readonly Color RowAlt = ColorTranslator.FromHtml("#F5FBF7");
        private static readonly Color RowOdd = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color GridBorderColor = ColorTranslator.FromHtml("#EEF6F1");
        private static readonly Color NotifChipBg = ColorScheme.PrimaryPale;

        public ServiceForm()
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
                Text = "➕ Thêm Dịch Vụ",
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
                PlaceholderText = "🔍 Tìm theo tên dịch vụ...",
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

            const int BTN_THEM_W = 200, BTN_EXCEL_W = 140, GAP = 8, H = 42;
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
            dgvDichVu = new DataGridView
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
            dgvDichVu.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorScheme.PrimaryDark,
                ForeColor = Color.White,
                Font = AppFonts.BodyBold,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvDichVu.ColumnHeadersHeight = 40;
            dgvDichVu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvDichVu.EnableHeadersVisualStyles = false;
            dgvDichVu.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowOdd,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvDichVu.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowAlt,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
            };

            dgvDichVu.CellPainting += DgvDichVu_CellPainting;

            dgvDichVu.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaDV", HeaderText = "Mã", FillWeight = 10 });
            dgvDichVu.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenDichVu", HeaderText = "Tên dịch vụ", FillWeight = 45 });
            dgvDichVu.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonGia", HeaderText = "Đơn giá", FillWeight = 25 });
            dgvDichVu.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThaoTac", HeaderText = "Thao tác", FillWeight = 12 });

            // Hidden
            dgvDichVu.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaDichVuHidden", Visible = false });
            dgvDichVu.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonGiaRaw", Visible = false });

            dgvDichVu.CellClick += DgvDichVu_CellClick;
            dgvDichVu.CellFormatting += DgvDichVu_CellFormatting;
            dgvDichVu.CellMouseMove += DgvDichVu_CellMouseMove;

            pnlLeft.Controls.Add(dgvDichVu);
            pnlLeft.Controls.Add(pnlSpacer);
            pnlLeft.Controls.Add(pnlFilter);
        }

        // ══════════════════════════════════════════
        // HEADER GRADIENT PAINT
        // ══════════════════════════════════════════

        private void DgvDichVu_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1) return;

            e.Handled = true;
            var rect = e.CellBounds;

            using (var brush = new LinearGradientBrush(
                new Rectangle(0, rect.Y, dgvDichVu.Width, rect.Height),
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
                            new Rectangle(0, rect.Y, dgvDichVu.Width, rect.Height),
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
        // PANEL PHẢI — Form Thêm/Sửa Dịch Vụ (card style)
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
                Text = "✨ Thêm Dịch Vụ Mới",
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

            // ── Tên dịch vụ ──
            pnlCard.Controls.Add(TaoLabel("Tên dịch vụ", new Point(X, y), true));
            y += 20;
            txtTenDichVu = TaoGuna2TextBox(new Point(X, y), new Size(W, 36), "VD: Chăm sóc da cơ bản");
            pnlCard.Controls.Add(txtTenDichVu);
            y += 42;

            // ── Đơn giá ──
            pnlCard.Controls.Add(TaoLabel("Đơn giá (VNĐ)", new Point(X, y), true));
            y += 20;
            txtDonGia = TaoGuna2TextBox(new Point(X, y), new Size(W, 36), "VD: 280000");
            pnlCard.Controls.Add(txtDonGia);
            y += 42;

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
                Text = "ℹ️ Đơn giá mặc định 0đ. Dịch vụ sẽ hiển thị trong Phiếu Khám.",
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
                Text = "💾 Lưu Dịch Vụ",
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
                Text = "🗑️ Xóa Dịch Vụ",
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
            dgvDichVu.Rows.Clear();

            string keyword = txtTimKiem?.Text.Trim() ?? "";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    @"SELECT MaDichVu, TenDichVu, DonGia
                      FROM DichVu
                      WHERE (@Keyword = '' OR TenDichVu LIKE '%' + @Keyword + '%')
                      ORDER BY TenDichVu", conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", keyword);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int maDV = Convert.ToInt32(reader.GetValue(0));
                            string tenDV = reader.GetString(1);
                            decimal donGia = Convert.ToDecimal(reader.GetValue(2));

                            string maDisplay = "DV" + maDV.ToString("D3");
                            string giaDisplay = donGia.ToString("N0") + "đ";

                            int idx = dgvDichVu.Rows.Add(maDisplay, tenDV, giaDisplay, "");
                            dgvDichVu.Rows[idx].Cells["MaDichVuHidden"].Value = maDV;
                            dgvDichVu.Rows[idx].Cells["DonGiaRaw"].Value = donGia;
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

        private void DgvDichVu_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvDichVu.Columns[e.ColumnIndex].Name;

            if (colName == "TenDichVu")
            {
                e.CellStyle.Font = AppFonts.BodyBold;
                e.CellStyle.ForeColor = ColorScheme.TextDark;
            }

            if (colName == "DonGia")
            {
                e.CellStyle.Font = AppFonts.BodyBold;
                e.CellStyle.ForeColor = GoldAccent;
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

        private void DgvDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int maDV = Convert.ToInt32(dgvDichVu.Rows[e.RowIndex].Cells["MaDichVuHidden"].Value);
            LoadThongTinVaoForm(e.RowIndex, maDV);
        }

        private void DgvDichVu_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvDichVu.Columns[e.ColumnIndex].Name;
            dgvDichVu.Cursor = colName == "ThaoTac" ? Cursors.Hand : Cursors.Default;
        }

        // ══════════════════════════════════════════
        // LOAD THÔNG TIN VÀO FORM PHẢI
        // ══════════════════════════════════════════

        private void LoadThongTinVaoForm(int rowIndex, int maDV)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    "SELECT TenDichVu, DonGia FROM DichVu WHERE MaDichVu = @Ma", conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", maDV);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtTenDichVu.Text = reader.GetString(0);
                            txtDonGia.Text = Convert.ToDecimal(reader.GetValue(1)).ToString("0");
                        }
                    }
                }
            }
            catch { }

            _maDichVuDangChon = maDV;
            lblFormTitle.Text = "✏️ Sửa Dịch Vụ — DV" + maDV.ToString("D3");
            btnLuu.Text = "💾  Cập Nhật";
            btnXoa.Enabled = true;
            lblError.Text = "";
        }

        private void ResetForm()
        {
            _maDichVuDangChon = -1;
            txtTenDichVu.Text = "";
            txtDonGia.Text = "";
            lblFormTitle.Text = "✨ Thêm Dịch Vụ Mới";
            btnLuu.Text = "💾  Lưu Dịch Vụ";
            btnXoa.Enabled = false;
            lblError.Text = "";
            txtTenDichVu.Focus();
        }

        // ══════════════════════════════════════════
        // NÚT LƯU / CẬP NHẬT
        // ══════════════════════════════════════════

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            string tenDV = txtTenDichVu.Text.Trim();
            string donGiaText = txtDonGia.Text.Trim();

            if (string.IsNullOrEmpty(tenDV))
            {
                lblError.Text = "Tên dịch vụ không được để trống.";
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
                    if (_maDichVuDangChon == -1)
                    {
                        // Kiểm tra trùng tên
                        using (var chk = new SqlCommand(
                            "SELECT COUNT(*) FROM DichVu WHERE TenDichVu = @Ten", conn))
                        {
                            chk.Parameters.AddWithValue("@Ten", tenDV);
                            int count = Convert.ToInt32(chk.ExecuteScalar());
                            if (count > 0)
                            {
                                lblError.Text = "Dịch vụ \"" + tenDV + "\" đã tồn tại.";
                                return;
                            }
                        }

                        // TẠO MỚI
                        using (var cmd = new SqlCommand(
                            @"INSERT INTO DichVu (TenDichVu, DonGia)
                              VALUES (@Ten, @Gia)", conn))
                        {
                            cmd.Parameters.AddWithValue("@Ten", tenDV);
                            cmd.Parameters.AddWithValue("@Gia", donGia);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Thêm dịch vụ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Kiểm tra trùng tên (trừ bản ghi hiện tại)
                        using (var chk = new SqlCommand(
                            "SELECT COUNT(*) FROM DichVu WHERE TenDichVu = @Ten AND MaDichVu <> @Ma", conn))
                        {
                            chk.Parameters.AddWithValue("@Ten", tenDV);
                            chk.Parameters.AddWithValue("@Ma", _maDichVuDangChon);
                            int count = Convert.ToInt32(chk.ExecuteScalar());
                            if (count > 0)
                            {
                                lblError.Text = "Dịch vụ \"" + tenDV + "\" đã tồn tại.";
                                return;
                            }
                        }

                        // CẬP NHẬT
                        using (var cmd = new SqlCommand(
                            @"UPDATE DichVu SET TenDichVu = @Ten, DonGia = @Gia
                              WHERE MaDichVu = @Ma", conn))
                        {
                            cmd.Parameters.AddWithValue("@Ten", tenDV);
                            cmd.Parameters.AddWithValue("@Gia", donGia);
                            cmd.Parameters.AddWithValue("@Ma", _maDichVuDangChon);
                            int rows = cmd.ExecuteNonQuery();
                            if (rows == 0)
                            {
                                lblError.Text = "Không tìm thấy dịch vụ để cập nhật.";
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
            if (_maDichVuDangChon == -1) return;

            var result = MessageBox.Show(
                "Xóa dịch vụ DV" + _maDichVuDangChon.ToString("D3") + "?\nDịch vụ đã được sử dụng trong phiếu khám sẽ không xóa được.",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand("DELETE FROM DichVu WHERE MaDichVu = @Ma", conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", _maDichVuDangChon);
                    cmd.ExecuteNonQuery();
                }
                ResetForm();
                LoadDanhSach();
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE") || ex.Number == 547)
                    MessageBox.Show("Không thể xóa dịch vụ này vì đã có phiếu khám sử dụng.",
                        "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════
        // XUẤT EXCEL (CSV UTF-8)
        // ══════════════════════════════════════════

        private void BtnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dgvDichVu.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files (*.csv)|*.csv";
                sfd.FileName = "DanhMucDichVu_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";
                sfd.Title = "Xuất Danh Mục Dịch Vụ";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Mã,Tên dịch vụ,Đơn giá");

                    foreach (DataGridViewRow row in dgvDichVu.Rows)
                    {
                        string ma = row.Cells["MaDV"].Value?.ToString() ?? "";
                        string ten = row.Cells["TenDichVu"].Value?.ToString() ?? "";
                        string gia = row.Cells["DonGiaRaw"].Value?.ToString() ?? "0";

                        ten = "\"" + ten.Replace("\"", "\"\"") + "\"";

                        sb.AppendLine(ma + "," + ten + "," + gia);
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
