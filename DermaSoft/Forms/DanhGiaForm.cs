using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Form Đánh Giá — frm-danhgia trong wireframe.
    /// Quản lý bảng DanhGia: điểm đánh giá 1–5 sao (CHK_DiemDanh), nhận xét.
    /// </summary>
    public partial class DanhGiaForm : Form
    {
        // ── Controls ──
        private Panel pnlContent;
        private Guna2TextBox txtTimKiem;
        private Guna2ComboBox cboSao;
        private DataGridView dgvDanhGia;

        // ── KPI labels ──
        private Label lblKpiTrungBinh;
        private Label lblKpi5Sao;
        private Label lblKpi4Sao;
        private Label lblKpi3Sao;
        private Label lblKpiDuoi3Sao;

        // Màu đồng bộ
        private static readonly Color GoldAccent = Color.FromArgb(184, 138, 40);
        private static readonly Color GridBorderColor = ColorTranslator.FromHtml("#EEF6F1");
        private static readonly Color RowAlt = ColorTranslator.FromHtml("#F5FBF7");
        private static readonly Color RowOdd = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color BorderInput = ColorTranslator.FromHtml("#D1E8DC");
        private static readonly Color BadgeOkBg = ColorTranslator.FromHtml("#DCFCE7");
        private static readonly Color BadgeOkFg = ColorTranslator.FromHtml("#166534");
        private static readonly Color BadgeWarningBg = ColorTranslator.FromHtml("#FEF3C7");
        private static readonly Color BadgeWarningFg = ColorTranslator.FromHtml("#D97706");
        private static readonly Color BadgeDangerBg = ColorTranslator.FromHtml("#FEE2E2");
        private static readonly Color BadgeDangerFg = ColorTranslator.FromHtml("#DC2626");
        private static readonly Color BadgeInfoBg = ColorTranslator.FromHtml("#DBEAFE");
        private static readonly Color BadgeInfoFg = ColorTranslator.FromHtml("#1E40AF");

        private bool _dangVeLai = false;

        // Lưu trạng thái filter
        private string _savedKeyword = "";
        private int _savedSaoIdx = 0;

        // Dữ liệu
        private List<DanhGiaRow> _danhSach = new List<DanhGiaRow>();
        private int _tongSo = 0;
        private int _count5 = 0, _count4 = 0, _count3 = 0, _countDuoi3 = 0;
        private double _diemTB = 0;

        private class DanhGiaRow
        {
            public DateTime NgayDanhGia;
            public string TenBenhNhan;
            public string MaPhieuKham;
            public int Diem;
            public string NhanXet;
        }

        public DanhGiaForm()
        {
            InitializeComponent();
            TaoBoCuc();
        }

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

            if (txtTimKiem != null) _savedKeyword = txtTimKiem.Text;
            if (cboSao != null) _savedSaoIdx = cboSao.SelectedIndex;

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();

            int pad = 16;
            int contentW = pnlContent.ClientSize.Width - pad * 2;
            int y = pad;

            // ── Header ──
            y = TaoHeader(pad, y, contentW);

            // ── 5 KPI Cards ──
            y = TaoKpiCards(pad, y, contentW);

            // ── Filter bar ──
            y = TaoFilterBar(pad, y, contentW);
            txtTimKiem.Text = _savedKeyword;
            if (_savedSaoIdx < cboSao.Items.Count) cboSao.SelectedIndex = _savedSaoIdx;

            // ── Load dữ liệu ──
            LoadDuLieu();

            // ── DataGridView ──
            int gridH = Math.Max(260, pnlContent.ClientSize.Height - y - pad);
            TaoGrid(pad, y, contentW, gridH);

            pnlContent.ResumeLayout();
            _dangVeLai = false;
        }

        // ══════════════════════════════════════════
        // HEADER
        // ══════════════════════════════════════════

        private int TaoHeader(int x, int y, int w)
        {
            var pnlHeader = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, 44),
            };
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
                Text = "⭐  Đánh Giá Bệnh Nhân",
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
        // 5 KPI CARDS
        // ══════════════════════════════════════════

        private int TaoKpiCards(int x0, int y, int totalW)
        {
            int gap = 16;
            int cardW = (totalW - gap * 4) / 5;
            int cardH = 140;

            var kpis = new[]
            {
                new { Icon = "⭐", Value = lblKpiTrungBinh = new Label(), Title = "Điểm trung bình", Accent = ColorScheme.Gold },
                new { Icon = "🌟", Value = lblKpi5Sao = new Label(), Title = "5 sao",               Accent = ColorScheme.Success },
                new { Icon = "✨", Value = lblKpi4Sao = new Label(), Title = "4 sao",               Accent = ColorScheme.Primary },
                new { Icon = "👍", Value = lblKpi3Sao = new Label(), Title = "3 sao",               Accent = ColorScheme.Info },
                new { Icon = "👎", Value = lblKpiDuoi3Sao = new Label(), Title = "< 3 sao",          Accent = ColorScheme.Danger },
            };

            for (int i = 0; i < kpis.Length; i++)
            {
                var kpi = kpis[i];
                int cx = x0 + i * (cardW + gap);
                var card = TaoCard(cx, y, cardW, cardH);

                // Accent bar top
                card.Controls.Add(new Panel { Size = new Size(cardW, 3), Location = new Point(0, 0), BackColor = kpi.Accent });

                // Icon
                card.Controls.Add(new Label
                {
                    Text = kpi.Icon, Font = new Font("Segoe UI Emoji", 18f),
                    Location = new Point(16, 12), AutoSize = true, BackColor = Color.Transparent,
                });

                // Value
                kpi.Value.Font = AppFonts.H1;
                kpi.Value.ForeColor = ColorScheme.TextDark;
                kpi.Value.Location = new Point(16, 45);
                kpi.Value.AutoSize = true;
                kpi.Value.BackColor = Color.Transparent;
                kpi.Value.Text = "—";
                card.Controls.Add(kpi.Value);

                // Title
                card.Controls.Add(new Label
                {
                    Text = kpi.Title, Font = AppFonts.Body, ForeColor = ColorScheme.TextGray,
                    Location = new Point(16, cardH - 26), AutoSize = true, BackColor = Color.Transparent,
                });

                pnlContent.Controls.Add(card);
            }

            return y + cardH + 12;
        }

        // ══════════════════════════════════════════
        // FILTER BAR
        // ══════════════════════════════════════════

        private int TaoFilterBar(int x, int y, int w)
        {
            var card = TaoCard(x, y, w, 56);

            const int CBO_W = 160, GAP = 12, PAD = 16;
            int cy = 10;
            int searchW = w - PAD * 2 - CBO_W - GAP;
            int cx = PAD;

            // Search
            txtTimKiem = new Guna2TextBox
            {
                Font = AppFonts.Body, ForeColor = ColorScheme.TextDark,
                Location = new Point(cx, cy), Size = new Size(searchW, 36),
                PlaceholderText = "🔍 Tìm theo tên bệnh nhân...",
                PlaceholderForeColor = ColorScheme.TextLight,
                BorderRadius = 18, BorderColor = BorderInput,
                FocusedState = { BorderColor = ColorScheme.Primary },
                HoverState = { BorderColor = ColorScheme.Primary },
                FillColor = Color.White, Cursor = Cursors.IBeam,
            };
            txtTimKiem.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) VeLaiForm(); };
            card.Controls.Add(txtTimKiem);
            cx += searchW + GAP;

            // ComboBox Sao
            cboSao = new Guna2ComboBox
            {
                Font = AppFonts.Body, ForeColor = ColorScheme.TextDark,
                Location = new Point(cx, cy), Size = new Size(CBO_W, 36),
                BorderRadius = 8, BorderColor = BorderInput, FillColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            cboSao.Items.AddRange(new object[] { "Tất cả sao", "5 sao", "4 sao", "3 sao", "2 sao", "1 sao" });
            cboSao.SelectedIndex = 0;
            cboSao.SelectedIndexChanged += (s, e) => VeLaiForm();
            card.Controls.Add(cboSao);

            pnlContent.Controls.Add(card);
            return y + 56 + 12;
        }

        // ══════════════════════════════════════════
        // LOAD DỮ LIỆU
        // ══════════════════════════════════════════

        private void LoadDuLieu()
        {
            string keyword = txtTimKiem?.Text?.Trim() ?? "";
            int saoIdx = cboSao?.SelectedIndex ?? 0;
            int filterDiem = saoIdx == 0 ? 0 : (6 - saoIdx); // 1→5, 2→4, 3→3, 4→2, 5→1

            _danhSach.Clear();
            _tongSo = 0; _count5 = 0; _count4 = 0; _count3 = 0; _countDuoi3 = 0; _diemTB = 0;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    // KPI tổng (không filter theo keyword/sao)
                    using (var cmd = new SqlCommand(
                        @"SELECT DiemDanh, COUNT(*) AS SoLuong
                          FROM DanhGia
                          GROUP BY DiemDanh", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        int totalSum = 0;
                        while (reader.Read())
                        {
                            int diem = Convert.ToInt32(reader.GetValue(0));
                            int sl = Convert.ToInt32(reader.GetValue(1));
                            _tongSo += sl;
                            totalSum += diem * sl;

                            if (diem == 5) _count5 = sl;
                            else if (diem == 4) _count4 = sl;
                            else if (diem == 3) _count3 = sl;
                            else _countDuoi3 += sl;
                        }
                        _diemTB = _tongSo > 0 ? (double)totalSum / _tongSo : 0;
                    }

                    // Chi tiết (có filter)
                    string filterSao = filterDiem > 0 ? " AND dg.DiemDanh = @Diem" : "";
                    string sql = @"
                        SELECT dg.NgayDanhGia, bn.HoTen, dg.MaPhieuKham, dg.DiemDanh, dg.NhanXet
                        FROM DanhGia dg
                        JOIN BenhNhan bn ON dg.MaBenhNhan = bn.MaBenhNhan
                        WHERE (@Keyword = '' OR bn.HoTen LIKE '%' + @Keyword + '%')" + filterSao + @"
                        ORDER BY dg.NgayDanhGia DESC";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Keyword", keyword);
                        if (filterDiem > 0)
                            cmd.Parameters.AddWithValue("@Diem", filterDiem);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _danhSach.Add(new DanhGiaRow
                                {
                                    NgayDanhGia = reader.GetDateTime(0),
                                    TenBenhNhan = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                    MaPhieuKham = "PK" + Convert.ToInt32(reader.GetValue(2)).ToString("D3"),
                                    Diem = Convert.ToInt32(reader.GetValue(3)),
                                    NhanXet = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                });
                            }
                        }
                    }
                }
            }
            catch { }

            // Gán KPI
            if (lblKpiTrungBinh != null) lblKpiTrungBinh.Text = _diemTB > 0 ? _diemTB.ToString("0.0") + " ⭐" : "—";
            if (lblKpi5Sao != null) lblKpi5Sao.Text = _tongSo > 0 ? ((double)_count5 / _tongSo * 100).ToString("0.0") + "%" : "0%";
            if (lblKpi4Sao != null) lblKpi4Sao.Text = _tongSo > 0 ? ((double)_count4 / _tongSo * 100).ToString("0.0") + "%" : "0%";
            if (lblKpi3Sao != null) lblKpi3Sao.Text = _tongSo > 0 ? ((double)_count3 / _tongSo * 100).ToString("0.0") + "%" : "0%";
            if (lblKpiDuoi3Sao != null) lblKpiDuoi3Sao.Text = _tongSo > 0 ? ((double)_countDuoi3 / _tongSo * 100).ToString("0.0") + "%" : "0%";
        }

        // ══════════════════════════════════════════
        // DATAGRIDVIEW
        // ══════════════════════════════════════════

        private void TaoGrid(int x, int y, int w, int h)
        {
            dgvDanhGia = new DataGridView
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
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
            dgvDanhGia.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorScheme.PrimaryDark,
                ForeColor = Color.White,
                Font = AppFonts.BodyBold,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvDanhGia.ColumnHeadersHeight = 42;
            dgvDanhGia.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvDanhGia.EnableHeadersVisualStyles = false;
            dgvDanhGia.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowOdd,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvDanhGia.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowAlt,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
            };

            dgvDanhGia.CellPainting += DgvDanhGia_CellPainting;

            dgvDanhGia.Columns.Add(new DataGridViewTextBoxColumn { Name = "NgayDG", HeaderText = "Ngày đánh giá", FillWeight = 15 });
            dgvDanhGia.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenBN", HeaderText = "Tên bệnh nhân", FillWeight = 20 });
            dgvDanhGia.Columns.Add(new DataGridViewTextBoxColumn { Name = "PhieuKham", HeaderText = "Phiếu Khám", FillWeight = 12 });
            dgvDanhGia.Columns.Add(new DataGridViewTextBoxColumn { Name = "Diem", HeaderText = "Điểm", FillWeight = 12 });
            dgvDanhGia.Columns.Add(new DataGridViewTextBoxColumn { Name = "NhanXet", HeaderText = "Nhận xét", FillWeight = 41 });

            dgvDanhGia.CellFormatting += DgvDanhGia_CellFormatting;

            foreach (var row in _danhSach)
            {
                string stars = new string('⭐', row.Diem);
                dgvDanhGia.Rows.Add(
                    row.NgayDanhGia.ToString("dd/MM/yyyy"),
                    row.TenBenhNhan,
                    row.MaPhieuKham,
                    stars,
                    row.NhanXet
                );
            }

            pnlContent.Controls.Add(dgvDanhGia);
        }

        // ══════════════════════════════════════════
        // GRID EVENTS
        // ══════════════════════════════════════════

        private void DgvDanhGia_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1) return;

            e.Handled = true;
            var rect = e.CellBounds;

            using (var brush = new LinearGradientBrush(
                new Rectangle(0, rect.Y, dgvDanhGia.Width, rect.Height),
                ColorScheme.PrimaryDark, Color.FromArgb(180, GoldAccent.R, GoldAccent.G, GoldAccent.B),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            if (e.Value != null)
            {
                var textRect = new Rectangle(rect.X + 12, rect.Y, rect.Width - 12, rect.Height);
                TextRenderer.DrawText(e.Graphics, e.Value.ToString(), AppFonts.BodyBold,
                    textRect, Color.White, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            }
        }

        private void DgvDanhGia_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvDanhGia.Columns[e.ColumnIndex].Name;

            if (colName == "TenBN")
            {
                e.CellStyle.Font = AppFonts.BodyBold;
                e.CellStyle.ForeColor = ColorScheme.TextDark;
            }

            if (colName == "PhieuKham")
            {
                e.CellStyle.Font = AppFonts.Badge;
                e.CellStyle.BackColor = BadgeInfoBg;
                e.CellStyle.ForeColor = BadgeInfoFg;
            }

            if (colName == "Diem" && e.Value != null)
            {
                int starCount = e.Value.ToString().Length; // emoji count
                if (starCount >= 5)
                {
                    e.CellStyle.BackColor = BadgeOkBg;
                    e.CellStyle.ForeColor = BadgeOkFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else if (starCount >= 3)
                {
                    e.CellStyle.BackColor = BadgeWarningBg;
                    e.CellStyle.ForeColor = BadgeWarningFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else
                {
                    e.CellStyle.BackColor = BadgeDangerBg;
                    e.CellStyle.ForeColor = BadgeDangerFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
            }
        }

        // ══════════════════════════════════════════
        // HELPERS
        // ══════════════════════════════════════════

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
