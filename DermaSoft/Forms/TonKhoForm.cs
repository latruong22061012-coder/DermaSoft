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
    public partial class TonKhoForm : Form
    {
        // ── KPI cards ──
        private Label lblKpiHetHan;
        private Label lblKpiSapHet;
        private Label lblKpiCanhBao;
        private Label lblKpiBinhThuong;

        // ── Filter row ──
        private Guna2TextBox txtSearch;
        private Guna2ComboBox cboTrangThai;

        // ── Grid ──
        private DataGridView dgvTonKho;

        // Màu đồng bộ
        private static readonly Color BorderInput = ColorTranslator.FromHtml("#D1E8DC");
        private static readonly Color InputBg = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color GoldAccent = Color.FromArgb(184, 138, 40);
        private static readonly Color RowAlt = ColorTranslator.FromHtml("#F5FBF7");
        private static readonly Color RowOdd = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color GridBorderColor = ColorTranslator.FromHtml("#EEF6F1");

        private static readonly Color BadgeActiveBg = ColorTranslator.FromHtml("#DCFCE7");
        private static readonly Color BadgeActiveFg = ColorTranslator.FromHtml("#166534");
        private static readonly Color BadgeInfoBg = ColorTranslator.FromHtml("#DBEAFE");
        private static readonly Color BadgeInfoFg = ColorTranslator.FromHtml("#1E40AF");
        private static readonly Color BadgeWarningBg = ColorTranslator.FromHtml("#FEF3C7");
        private static readonly Color BadgeWarningFg = ColorTranslator.FromHtml("#D97706");
        private static readonly Color BadgeDangerBg = ColorTranslator.FromHtml("#FEE2E2");
        private static readonly Color BadgeDangerFg = ColorTranslator.FromHtml("#DC2626");

        public TonKhoForm()
        {
            InitializeComponent();
            TaoBoCuc();
            LoadData();
        }

        // ══════════════════════════════════════════
        // BỐ CỤC CHÍNH — 3 sections dọc
        // ══════════════════════════════════════════

        private void TaoBoCuc()
        {
            this.Padding = new Padding(20);
            this.BackColor = ColorScheme.Background;

            var pnlMain = new Panel { Dock = DockStyle.Fill, BackColor = ColorScheme.Background };
            this.Controls.Add(pnlMain);

            // Sections
            var pnlGrid = TaoGridSection();
            pnlGrid.Dock = DockStyle.Fill;

            var pnlKpi = TaoKpiSection();
            pnlKpi.Dock = DockStyle.Top;

            var pnlFilter = TaoFilterSection();
            pnlFilter.Dock = DockStyle.Top;

            // Dock order: Fill first, then Top after
            pnlMain.Controls.Add(pnlGrid);     // Fill
            pnlMain.Controls.Add(pnlFilter);   // Top (below KPI)
            pnlMain.Controls.Add(pnlKpi);      // Top (topmost)
        }

        // ══════════════════════════════════════════
        // SECTION 1 — 4 KPI CARDS (Dashboard style)
        // ══════════════════════════════════════════

        private Panel TaoKpiSection()
        {
            var pnl = new Panel { Height = 150, BackColor = Color.Transparent };

            lblKpiHetHan = new Label();
            lblKpiSapHet = new Label();
            lblKpiCanhBao = new Label();
            lblKpiBinhThuong = new Label();

            var cards = new[]
            {
                new { Icon = "🚫", Title = "Lô hết hạn",        Accent = ColorScheme.Danger,  Lbl = lblKpiHetHan },
                new { Icon = "⏰", Title = "Sắp hết hạn (<30d)", Accent = ColorTranslator.FromHtml("#F97316"), Lbl = lblKpiSapHet },
                new { Icon = "⚠️", Title = "Cảnh báo (<90d)",    Accent = ColorScheme.Warning, Lbl = lblKpiCanhBao },
                new { Icon = "✅", Title = "Bình thường",         Accent = ColorScheme.Success, Lbl = lblKpiBinhThuong },
            };

            var cardPanels = new Panel[4];

            for (int i = 0; i < cards.Length; i++)
            {
                var c = cards[i];
                var card = new Panel { BackColor = Color.White };
                card.Paint += (s, e) =>
                {
                    var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var path = TaoRoundedRect(new Rectangle(0, 0, ((Panel)s).Width - 1, ((Panel)s).Height - 1), 12))
                    using (var pen = new Pen(ColorScheme.Border, 1f))
                        g.DrawPath(pen, path);
                };

                // Accent bar top
                var barColor = c.Accent;
                var bar = new Panel { Size = new Size(60, 4), Location = new Point(16, 0), BackColor = barColor };
                card.Controls.Add(bar);

                // Number (large)
                c.Lbl.Text = "0";
                c.Lbl.Font = new Font("Segoe UI", 28f, FontStyle.Bold);
                c.Lbl.ForeColor = ColorScheme.TextDark;
                c.Lbl.Location = new Point(14, 14);
                c.Lbl.AutoSize = true;
                c.Lbl.BackColor = Color.Transparent;
                card.Controls.Add(c.Lbl);

                // Title (well below the number — font 28f bold renders ~48px tall)
                card.Controls.Add(new Label
                {
                    Text = c.Title, Font = AppFonts.Body, ForeColor = ColorScheme.TextGray,
                    Location = new Point(16, 72), AutoSize = true, BackColor = Color.Transparent,
                });

                
               
                

                cardPanels[i] = card;
                pnl.Controls.Add(card);
            }

            // Responsive layout
            const int GAP = 14;
            pnl.Resize += (s, e) =>
            {
                int pw = pnl.ClientSize.Width;
                if (pw <= 0) return;
                int cardW = (pw - GAP * 3) / 4;
                int cardH = pnl.Height - 8;
                for (int i = 0; i < cardPanels.Length; i++)
                    cardPanels[i].SetBounds(i * (cardW + GAP), 0, cardW, cardH);
            };

            return pnl;
        }

        // ══════════════════════════════════════════
        // SECTION 2 — SEARCH + FILTER + EXPORT
        // ══════════════════════════════════════════

        private Panel TaoFilterSection()
        {
            var pnl = new Panel { Height = 52, BackColor = Color.Transparent };

            var pnlSearchWrap = new Panel { BackColor = Color.Transparent };
            var pnlCboWrap = new Panel { BackColor = Color.Transparent };
            var pnlBtnWrap = new Panel { BackColor = Color.Transparent };

            txtSearch = new Guna2TextBox
            {
                Font = AppFonts.Body, ForeColor = ColorScheme.TextDark,
                Dock = DockStyle.Top, Height = 36,
                BorderRadius = 8, BorderColor = BorderInput, FillColor = InputBg,
                PlaceholderText = "🔍 Tìm thuốc...",
                FocusedState = { BorderColor = ColorScheme.Primary },
                HoverState = { BorderColor = ColorScheme.Primary },
            };
            txtSearch.TextChanged += (s, e) => ApplyFilter();
            pnlSearchWrap.Controls.Add(txtSearch);

            cboTrangThai = new Guna2ComboBox
            {
                Font = AppFonts.Body, ForeColor = ColorScheme.TextDark,
                Dock = DockStyle.Top, Height = 36,
                BorderRadius = 8, BorderColor = BorderInput, FillColor = InputBg,
                FocusedState = { BorderColor = ColorScheme.Primary },
                HoverState = { BorderColor = ColorScheme.Primary },
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            cboTrangThai.Items.AddRange(new object[] { "Tất cả trạng thái", "Lô hết hạn", "Sắp hết hạn", "Cảnh báo", "Bình thường" });
            cboTrangThai.SelectedIndex = 0;
            cboTrangThai.SelectedIndexChanged += (s, e) => ApplyFilter();
            pnlCboWrap.Controls.Add(cboTrangThai);

            var btnExcel = new Guna2Button
            {
                Text = "📥 Xuất Excel",
                Font = AppFonts.BodyBold, ForeColor = Color.White,
                FillColor = ColorScheme.Primary, BorderRadius = 18,
                Dock = DockStyle.Top, Height = 36, Cursor = Cursors.Hand,
            };
            btnExcel.Click += BtnExcel_Click;
            pnlBtnWrap.Controls.Add(btnExcel);

            pnl.Controls.Add(pnlSearchWrap);
            pnl.Controls.Add(pnlCboWrap);
            pnl.Controls.Add(pnlBtnWrap);

            const int BTN_W = 160, CBO_W = 210, GAP = 10;
            pnl.Resize += (s, e) =>
            {
                int pw = pnl.ClientSize.Width;
                if (pw <= 0) return;
                int searchW = pw - CBO_W - BTN_W - GAP * 2;
                if (searchW < 100) searchW = 100;
                pnlSearchWrap.SetBounds(0, 8, searchW, 42);
                pnlCboWrap.SetBounds(searchW + GAP, 8, CBO_W, 42);
                pnlBtnWrap.SetBounds(pw - BTN_W, 8, BTN_W, 42);
            };

            return pnl;
        }

        // ══════════════════════════════════════════
        // SECTION 3 — DATAGRIDVIEW TỒN KHO
        // ══════════════════════════════════════════

        private Panel TaoGridSection()
        {
            var pnlOuter = new Panel { BackColor = Color.Transparent, Padding = new Padding(0, 6, 0, 0) };

            var pnlCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(0) };
            pnlCard.Paint += (s, e) =>
            {
                var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = TaoRoundedRect(new Rectangle(0, 0, pnlCard.Width - 1, pnlCard.Height - 1), 12))
                using (var pen = new Pen(ColorScheme.Border, 1f))
                    g.DrawPath(pen, path);
            };
            pnlOuter.Controls.Add(pnlCard);

            dgvTonKho = new DataGridView
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
                RowTemplate = { Height = 42 },
            };
            dgvTonKho.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorScheme.PrimaryDark, ForeColor = Color.White,
                Font = AppFonts.BodyBold, Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvTonKho.ColumnHeadersHeight = 42;
            dgvTonKho.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvTonKho.EnableHeadersVisualStyles = false;
            dgvTonKho.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowOdd, ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale, SelectionForeColor = ColorScheme.TextDark,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvTonKho.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowAlt, ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale, SelectionForeColor = ColorScheme.TextDark,
            };

            dgvTonKho.CellPainting += DgvTonKho_CellPainting;
            dgvTonKho.CellFormatting += DgvTonKho_CellFormatting;

            // Columns
            dgvTonKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenThuoc", HeaderText = "Thuốc", FillWeight = 22 });
            dgvTonKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "LoNhap", HeaderText = "Lô Nhập", FillWeight = 10 });
            dgvTonKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "NgayNhap", HeaderText = "Ngày Nhập", FillWeight = 12 });
            dgvTonKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "HanSD", HeaderText = "Hạn SD", FillWeight = 10 });
            dgvTonKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "ConLai", HeaderText = "Còn lại (Ngày)", FillWeight = 12 });
            dgvTonKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "SLTon", HeaderText = "SL Tồn", FillWeight = 9 });
            dgvTonKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "UuTien", HeaderText = "Ưu Tiên (FEFO)", FillWeight = 11 });
            dgvTonKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "TrangThai", HeaderText = "Trạng Thái", FillWeight = 14 });

            // Hidden
            dgvTonKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "TrangThaiRaw", Visible = false });
            dgvTonKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoNgayConLaiRaw", Visible = false });

            pnlCard.Controls.Add(dgvTonKho);

            return pnlOuter;
        }

        // ══════════════════════════════════════════
        // GRADIENT HEADER PAINT
        // ══════════════════════════════════════════

        private void DgvTonKho_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1) return;

            e.Handled = true;
            var rect = e.CellBounds;

            using (var brush = new LinearGradientBrush(
                new Rectangle(0, rect.Y, dgvTonKho.Width, rect.Height),
                ColorScheme.PrimaryDark, Color.FromArgb(180, GoldAccent.R, GoldAccent.G, GoldAccent.B),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            if (e.ColumnIndex == 0)
            {
                using (var br = new SolidBrush(Color.White))
                {
                    e.Graphics.FillRectangle(br, rect.X, rect.Y, 8, 8);
                    using (var path = new GraphicsPath())
                    {
                        path.AddArc(rect.X, rect.Y, 16, 16, 180, 90);
                        path.AddLine(rect.X + 8, rect.Y, rect.X, rect.Y + 8);
                        path.CloseFigure();
                        using (var brush2 = new LinearGradientBrush(
                            new Rectangle(0, rect.Y, dgvTonKho.Width, rect.Height),
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
        // CELL FORMATTING — Badge trạng thái
        // ══════════════════════════════════════════

        private void DgvTonKho_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvTonKho.Columns[e.ColumnIndex].Name;

            if (colName == "TenThuoc")
            {
                e.CellStyle.Font = AppFonts.BodyBold;
                e.CellStyle.ForeColor = ColorScheme.TextDark;
            }

            if (colName == "LoNhap")
            {
                e.CellStyle.Font = AppFonts.Badge;
                e.CellStyle.ForeColor = ColorScheme.PrimaryDark;
            }

            if (colName == "ConLai" && e.Value != null)
            {
                var raw = dgvTonKho.Rows[e.RowIndex].Cells["SoNgayConLaiRaw"].Value;
                if (raw != null)
                {
                    int days;
                    if (int.TryParse(raw.ToString(), out days))
                    {
                        if (days <= 0) e.CellStyle.ForeColor = BadgeDangerFg;
                        else if (days < 30) e.CellStyle.ForeColor = ColorTranslator.FromHtml("#F97316");
                        else if (days < 90) e.CellStyle.ForeColor = BadgeWarningFg;
                        else e.CellStyle.ForeColor = BadgeActiveFg;
                        e.CellStyle.Font = AppFonts.BodyBold;
                    }
                }
            }

            if (colName == "UuTien" && e.Value != null)
            {
                string v = e.Value.ToString();
                if (v.Contains("#1"))
                {
                    e.CellStyle.BackColor = BadgeActiveBg;
                    e.CellStyle.ForeColor = BadgeActiveFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else
                {
                    e.CellStyle.ForeColor = ColorScheme.TextGray;
                    e.CellStyle.Font = AppFonts.Small;
                }
            }

            if (colName == "TrangThai" && e.Value != null)
            {
                string v = e.Value.ToString();
                if (v.Contains("HẾT HẠN"))
                {
                    e.CellStyle.BackColor = BadgeDangerBg;
                    e.CellStyle.ForeColor = BadgeDangerFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else if (v.Contains("Sắp hết"))
                {
                    e.CellStyle.BackColor = ColorTranslator.FromHtml("#FFF7ED");
                    e.CellStyle.ForeColor = ColorTranslator.FromHtml("#F97316");
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else if (v.Contains("Cảnh báo"))
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
        }

        // ══════════════════════════════════════════
        // LOAD DATA
        // ══════════════════════════════════════════

        private List<object[]> _allRows = new List<object[]>();

        private void LoadData()
        {
            _allRows.Clear();
            int cntHetHan = 0, cntSapHet = 0, cntCanhBao = 0, cntBinhThuong = 0;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    @"SELECT t.TenThuoc,
                             ctk.MaPhieuNhap,
                             pnk.NgayNhap,
                             ctk.HanSuDung,
                             DATEDIFF(DAY, GETDATE(), ctk.HanSuDung) AS SoNgayConLai,
                             ctk.SoLuongConLai,
                             ROW_NUMBER() OVER (PARTITION BY t.MaThuoc ORDER BY ctk.HanSuDung ASC) AS UuTienFEFO,
                             CASE 
                                 WHEN ctk.HanSuDung < GETDATE() THEN N'HẾT HẠN'
                                 WHEN DATEDIFF(DAY, GETDATE(), ctk.HanSuDung) < 30 THEN N'Sắp hết hạn'
                                 WHEN DATEDIFF(DAY, GETDATE(), ctk.HanSuDung) < 90 THEN N'Cảnh báo'
                                 ELSE N'Bình thường'
                             END AS TrangThaiHetHan
                      FROM Thuoc t
                      INNER JOIN ChiTietNhapKho ctk ON t.MaThuoc = ctk.MaThuoc
                      INNER JOIN PhieuNhapKho pnk ON ctk.MaPhieuNhap = pnk.MaPhieuNhap
                      WHERE ctk.SoLuongConLai > 0
                      ORDER BY ctk.HanSuDung ASC, t.TenThuoc", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tenThuoc = reader.GetString(0);
                        int maPhieu = Convert.ToInt32(reader.GetValue(1));
                        DateTime ngayNhap = reader.GetDateTime(2);
                        DateTime hanSD = reader.GetDateTime(3);
                        int soNgay = Convert.ToInt32(reader.GetValue(4));
                        int slTon = Convert.ToInt32(reader.GetValue(5));
                        long fefo = Convert.ToInt64(reader.GetValue(6));
                        string trangThai = reader.GetString(7);

                        string loNhap = "PN#" + maPhieu.ToString("D4");
                        string ngayNhapStr = ngayNhap.ToString("dd/MM/yyyy");
                        string hanSDStr = hanSD.ToString("MM/yyyy");
                        string conLaiStr = soNgay <= 0 ? "Đã hết hạn" : soNgay + " ngày";
                        string fefoStr = "#" + fefo;

                        string trangThaiIcon;
                        if (trangThai.Contains("HẾT HẠN")) { trangThaiIcon = "🚫 " + trangThai; cntHetHan++; }
                        else if (trangThai.Contains("Sắp hết")) { trangThaiIcon = "⏰ " + trangThai; cntSapHet++; }
                        else if (trangThai.Contains("Cảnh báo")) { trangThaiIcon = "⚠️ " + trangThai; cntCanhBao++; }
                        else { trangThaiIcon = "✅ " + trangThai; cntBinhThuong++; }

                        _allRows.Add(new object[]
                        {
                            tenThuoc, loNhap, ngayNhapStr, hanSDStr, conLaiStr, slTon.ToString(), fefoStr, trangThaiIcon,
                            trangThai, soNgay.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Update KPI
            lblKpiHetHan.Text = cntHetHan.ToString();
            lblKpiSapHet.Text = cntSapHet.ToString();
            lblKpiCanhBao.Text = cntCanhBao.ToString();
            lblKpiBinhThuong.Text = cntBinhThuong.ToString();

            ApplyFilter();
        }

        // ══════════════════════════════════════════
        // FILTER
        // ══════════════════════════════════════════

        private void ApplyFilter()
        {
            dgvTonKho.Rows.Clear();

            string search = txtSearch?.Text?.Trim().ToLower() ?? "";
            int trangThaiIdx = cboTrangThai?.SelectedIndex ?? 0;

            foreach (var row in _allRows)
            {
                string tenThuoc = row[0].ToString().ToLower();
                string loNhap = row[1].ToString().ToLower();
                string trangThaiRaw = row[8].ToString();

                // Search filter
                if (!string.IsNullOrEmpty(search))
                {
                    if (!tenThuoc.Contains(search) && !loNhap.Contains(search))
                        continue;
                }

                // TrangThai filter
                if (trangThaiIdx > 0)
                {
                    bool match = false;
                    switch (trangThaiIdx)
                    {
                        case 1: match = trangThaiRaw.Contains("HẾT HẠN"); break;
                        case 2: match = trangThaiRaw.Contains("Sắp hết"); break;
                        case 3: match = trangThaiRaw.Contains("Cảnh báo"); break;
                        case 4: match = trangThaiRaw.Contains("Bình thường"); break;
                    }
                    if (!match) continue;
                }

                dgvTonKho.Rows.Add(row);
            }
        }

        // ══════════════════════════════════════════
        // XUẤT EXCEL (CSV)
        // ══════════════════════════════════════════

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            if (dgvTonKho.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var sfd = new SaveFileDialog
            {
                Title = "Xuất Tồn Kho",
                Filter = "CSV Files|*.csv",
                FileName = "TonKho_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv",
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("Thuốc,Lô Nhập,Ngày Nhập,Hạn SD,Còn lại (Ngày),SL Tồn,Ưu Tiên (FEFO),Trạng Thái");

                foreach (DataGridViewRow row in dgvTonKho.Rows)
                {
                    var vals = new string[8];
                    for (int i = 0; i < 8; i++)
                        vals[i] = "\"" + (row.Cells[i].Value?.ToString() ?? "").Replace("\"", "\"\"") + "\"";
                    sb.AppendLine(string.Join(",", vals));
                }

                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("Xuất file thành công!\n" + sfd.FileName, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
