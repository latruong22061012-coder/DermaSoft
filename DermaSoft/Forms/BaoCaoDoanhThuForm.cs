using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Form Báo Cáo Doanh Thu — frm-baocao trong wireframe.
    /// Admin: xem VW_BaoCaoDoanhThu, biểu đồ theo tháng/năm.
    /// </summary>
    public partial class BaoCaoDoanhThuForm : Form
    {
        // ── Controls ──
        private Panel pnlContent;
        private Guna2DateTimePicker dtpTuNgay;
        private Guna2DateTimePicker dtpDenNgay;
        private Guna2ComboBox cboLocTheo;
        private DataGridView dgvChiTiet;

        // ── KPI labels ──
        private Label lblKpiDoanhThu;
        private Label lblKpiLuotKham;
        private Label lblKpiTrungBinh;
        private Label lblKpiDoanhThuThuoc;

        // ── Chart data ──
        private List<KeyValuePair<string, decimal>> _chartData = new List<KeyValuePair<string, decimal>>();

        // Màu đồng bộ
        private static readonly Color GoldAccent = Color.FromArgb(184, 138, 40);
        private static readonly Color GridBorderColor = ColorTranslator.FromHtml("#EEF6F1");
        private static readonly Color RowAlt = ColorTranslator.FromHtml("#F5FBF7");
        private static readonly Color RowOdd = ColorTranslator.FromHtml("#FCFFFE");
        private static readonly Color BorderInput = ColorTranslator.FromHtml("#D1E8DC");

        private bool _dangVeLai = false;

        // Lưu trạng thái filter qua các lần VeLaiForm()
        private DateTime _savedTuNgay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        private DateTime _savedDenNgay = DateTime.Now;
        private int _savedLocIdx = 0;

        public BaoCaoDoanhThuForm()
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

            // Lưu giá trị filter hiện tại trước khi clear
            if (dtpTuNgay != null) _savedTuNgay = dtpTuNgay.Value.Date;
            if (dtpDenNgay != null) _savedDenNgay = dtpDenNgay.Value.Date;
            if (cboLocTheo != null) _savedLocIdx = cboLocTheo.SelectedIndex;

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();

            int pad = 16;
            int gap = 12;
            int contentW = pnlContent.ClientSize.Width - pad * 2;

            int y = pad;

            // ── Row 0: Header ──
            y = TaoHeader(pad, y, contentW);

            // ── Row 1: Filter bar (khôi phục giá trị đã lưu) ──
            y = TaoFilterBar(pad, y, contentW);
            dtpTuNgay.Value = _savedTuNgay;
            dtpDenNgay.Value = _savedDenNgay;
            cboLocTheo.SelectedIndex = _savedLocIdx;

            // ── Row 2: 4 KPI cards (tạo trước để labels tồn tại) ──
            y = TaoKpiCards(pad, y, contentW);

            // ── Load dữ liệu (gán vào labels đã tạo) ──
            LoadDuLieu();

            // ── Row 3: Chart (trái) + DataGridView (phải) ──
            int remainH = Math.Max(320, pnlContent.ClientSize.Height - y - pad);
            int halfW = (contentW - gap) / 2;
            TaoChartDoanhThu(pad, y, halfW, remainH);
            TaoGridChiTiet(pad + halfW + gap, y, halfW, remainH);

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
                Text = "📊  Báo Cáo Doanh Thu ",
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
        // FILTER BAR
        // ══════════════════════════════════════════

        private int TaoFilterBar(int x, int y, int w)
        {
            var card = TaoCard(x, y, w, 80);

            // Kích thước cố định cho ComboBox + 2 Button
            const int CBO_W = 180, BTN_LOC_W = 160, BTN_EXCEL_W = 140, GAP = 12, PAD = 16;
            int cy = 8;

            // Phần còn lại chia đều cho 2 DatePicker
            int fixedW = CBO_W + BTN_LOC_W + BTN_EXCEL_W + GAP * 4;
            int dtpW = Math.Max(160, (w - PAD * 2 - fixedW - GAP) / 2);

            int cx = PAD;

            // Từ ngày
            card.Controls.Add(new Label
            {
                Text = "Từ ngày",
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextGray,
                Location = new Point(cx, cy),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            dtpTuNgay = new Guna2DateTimePicker
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Location = new Point(cx, cy + 22),
                Size = new Size(dtpW, 36),
                BorderRadius = 8,
                BorderColor = BorderInput,
                FillColor = Color.White,
                Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                Format = DateTimePickerFormat.Short,
            };
            card.Controls.Add(dtpTuNgay);
            cx += dtpW + GAP;

            // Đến ngày
            card.Controls.Add(new Label
            {
                Text = "Đến ngày",
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextGray,
                Location = new Point(cx, cy),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            dtpDenNgay = new Guna2DateTimePicker
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Location = new Point(cx, cy + 22),
                Size = new Size(dtpW, 36),
                BorderRadius = 8,
                BorderColor = BorderInput,
                FillColor = Color.White,
                Value = DateTime.Now,
                Format = DateTimePickerFormat.Short,
            };
            card.Controls.Add(dtpDenNgay);
            cx += dtpW + GAP;

            // Lọc theo
            card.Controls.Add(new Label
            {
                Text = "Lọc theo",
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextGray,
                Location = new Point(cx, cy),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            cboLocTheo = new Guna2ComboBox
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Location = new Point(cx, cy + 22),
                Size = new Size(CBO_W, 36),
                BorderRadius = 8,
                BorderColor = BorderInput,
                FillColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            cboLocTheo.Items.AddRange(new object[] { "Theo ngày", "Theo tháng" });
            cboLocTheo.SelectedIndex = 0;
            card.Controls.Add(cboLocTheo);
            cx += CBO_W + GAP;

            // Nút Lọc Báo Cáo
            var btnLoc = new Guna2Button
            {
                Text = "🔍 Lọc Báo Cáo",
                Font = AppFonts.BodyBold,
                ForeColor = Color.White,
                FillColor = ColorScheme.Primary,
                BorderRadius = 18,
                Location = new Point(cx, cy + 22),
                Size = new Size(BTN_LOC_W, 36),
                Cursor = Cursors.Hand,
            };
            btnLoc.Click += (s, e) => VeLaiForm();
            card.Controls.Add(btnLoc);
            cx += BTN_LOC_W + GAP;

            // Nút Xuất Excel
            var btnExcel = new Guna2Button
            {
                Text = "📥 Xuất Excel",
                Font = AppFonts.BodyBold,
                ForeColor = ColorScheme.PrimaryDark,
                FillColor = Color.White,
                BorderRadius = 18,
                BorderThickness = 1,
                BorderColor = ColorScheme.Primary,
                Location = new Point(cx, cy + 22),
                Size = new Size(BTN_EXCEL_W, 36),
                Cursor = Cursors.Hand,
            };
            btnExcel.Click += BtnXuatExcel_Click;
            card.Controls.Add(btnExcel);

            pnlContent.Controls.Add(card);
            return y + 80 + 12;
        }

        // ══════════════════════════════════════════
        // 4 KPI CARDS
        // ══════════════════════════════════════════

        private int TaoKpiCards(int x0, int y, int totalW)
        {
            int gap = 16;
            int cardW = (totalW - gap * 3) / 4;
            int cardH = 180;

            var kpis = new[]
            {
                new { Icon = "💰", Value = lblKpiDoanhThu = new Label(), Title = "Tổng doanh thu tháng", Accent = ColorScheme.Gold },
                new { Icon = "📋", Value = lblKpiLuotKham = new Label(), Title = "Tổng lượt khám",       Accent = ColorScheme.Primary },
                new { Icon = "⭐", Value = lblKpiTrungBinh = new Label(), Title = "Trung bình/phiếu",     Accent = GoldAccent },
                new { Icon = "💊", Value = lblKpiDoanhThuThuoc = new Label(), Title = "Doanh thu thuốc", Accent = ColorScheme.Info },
            };

            for (int i = 0; i < kpis.Length; i++)
            {
                var kpi = kpis[i];
                int cx = x0 + i * (cardW + gap);
                var card = TaoCard(cx, y, cardW, cardH);

                // Accent bar top (giống Dashboard)
                card.Controls.Add(new Panel { Size = new Size(cardW, 3), Location = new Point(0, 0), BackColor = kpi.Accent });

                // Icon emoji trực tiếp (giống Dashboard)
                card.Controls.Add(new Label
                {
                    Text = kpi.Icon,
                    Font = new Font("Segoe UI Emoji", 20f),
                    Location = new Point(16, 16),
                    AutoSize = true,
                    BackColor = Color.Transparent,
                });

                // Value
                kpi.Value.Font = AppFonts.H1;
                kpi.Value.ForeColor = ColorScheme.TextDark;
                kpi.Value.Location = new Point(16, 55);
                kpi.Value.AutoSize = true;
                kpi.Value.BackColor = Color.Transparent;
                kpi.Value.Text = "—";
                card.Controls.Add(kpi.Value);

                // Title
                card.Controls.Add(new Label
                {
                    Text = kpi.Title,
                    Font = AppFonts.Body,
                    ForeColor = ColorScheme.TextGray,
                    Location = new Point(16, cardH - 30),
                    AutoSize = true,
                    BackColor = Color.Transparent,
                });

                pnlContent.Controls.Add(card);
            }

            return y + cardH + 12;
        }

        // ══════════════════════════════════════════
        // LOAD DỮ LIỆU
        // ══════════════════════════════════════════

        private DateTime _tuNgay;
        private DateTime _denNgay;
        private bool _locTheoThang = false; // false=theo ngày, true=theo tháng
        private List<DoanhThuRow> _danhSachChiTiet = new List<DoanhThuRow>();

        private class DoanhThuRow
        {
            public DateTime Ngay;
            public int SoHoaDon;
            public decimal DoanhThu;
            public decimal Thuoc;
            public decimal DichVu;
        }

        private void LoadDuLieu()
        {
            _tuNgay = dtpTuNgay?.Value.Date ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            _denNgay = dtpDenNgay?.Value.Date ?? DateTime.Now.Date;
            _locTheoThang = (cboLocTheo?.SelectedIndex ?? 0) == 1;

            decimal tongDoanhThu = 0;
            int tongLuotKham = 0;
            decimal tongThuoc = 0;
            decimal tongDichVu = 0;
            _danhSachChiTiet.Clear();
            _chartData.Clear();

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    // Query tổng hợp
                    using (var cmd = new SqlCommand(
                        @"SELECT 
                            ISNULL(SUM(hd.TongTien), 0),
                            COUNT(*),
                            ISNULL(SUM(hd.TongThuoc), 0),
                            ISNULL(SUM(hd.TongTienDichVu), 0)
                          FROM HoaDon hd
                          WHERE hd.IsDeleted = 0
                            AND CAST(hd.NgayTao AS DATE) >= @Tu
                            AND CAST(hd.NgayTao AS DATE) <= @Den", conn))
                    {
                        cmd.Parameters.AddWithValue("@Tu", _tuNgay);
                        cmd.Parameters.AddWithValue("@Den", _denNgay);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tongDoanhThu = Convert.ToDecimal(reader.GetValue(0));
                                tongLuotKham = Convert.ToInt32(reader.GetValue(1));
                                tongThuoc = Convert.ToDecimal(reader.GetValue(2));
                                tongDichVu = Convert.ToDecimal(reader.GetValue(3));
                            }
                        }
                    }

                    // Query chi tiết — theo ngày hoặc theo tháng
                    string sql = _locTheoThang
                        ? @"SELECT 
                                DATEFROMPARTS(YEAR(hd.NgayTao), MONTH(hd.NgayTao), 1) AS Ngay,
                                COUNT(*) AS SoHD,
                                ISNULL(SUM(hd.TongTien), 0) AS DoanhThu,
                                ISNULL(SUM(hd.TongThuoc), 0) AS Thuoc,
                                ISNULL(SUM(hd.TongTienDichVu), 0) AS DichVu
                              FROM HoaDon hd
                              WHERE hd.IsDeleted = 0
                                AND CAST(hd.NgayTao AS DATE) >= @Tu
                                AND CAST(hd.NgayTao AS DATE) <= @Den
                              GROUP BY YEAR(hd.NgayTao), MONTH(hd.NgayTao)
                              ORDER BY Ngay DESC"
                        : @"SELECT 
                                CAST(hd.NgayTao AS DATE) AS Ngay,
                                COUNT(*) AS SoHD,
                                ISNULL(SUM(hd.TongTien), 0) AS DoanhThu,
                                ISNULL(SUM(hd.TongThuoc), 0) AS Thuoc,
                                ISNULL(SUM(hd.TongTienDichVu), 0) AS DichVu
                              FROM HoaDon hd
                              WHERE hd.IsDeleted = 0
                                AND CAST(hd.NgayTao AS DATE) >= @Tu
                                AND CAST(hd.NgayTao AS DATE) <= @Den
                              GROUP BY CAST(hd.NgayTao AS DATE)
                              ORDER BY Ngay DESC";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Tu", _tuNgay);
                        cmd.Parameters.AddWithValue("@Den", _denNgay);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _danhSachChiTiet.Add(new DoanhThuRow
                                {
                                    Ngay = reader.GetDateTime(0),
                                    SoHoaDon = Convert.ToInt32(reader.GetValue(1)),
                                    DoanhThu = Convert.ToDecimal(reader.GetValue(2)),
                                    Thuoc = Convert.ToDecimal(reader.GetValue(3)),
                                    DichVu = Convert.ToDecimal(reader.GetValue(4)),
                                });
                            }
                        }
                    }
                }
            }
            catch { }

            // KPI
            if (lblKpiDoanhThu != null) lblKpiDoanhThu.Text = FormatTien(tongDoanhThu);
            if (lblKpiLuotKham != null) lblKpiLuotKham.Text = tongLuotKham.ToString();
            if (lblKpiTrungBinh != null) lblKpiTrungBinh.Text = tongLuotKham > 0 ? FormatTien(tongDoanhThu / tongLuotKham) : "0đ";
            if (lblKpiDoanhThuThuoc != null) lblKpiDoanhThuThuoc.Text = FormatTien(tongThuoc);

            // Chart data (ngày cũ bên trái)
            string labelFmt = _locTheoThang ? "MM/yyyy" : "dd/MM";
            _chartData = _danhSachChiTiet
                .OrderBy(r => r.Ngay)
                .Select(r => new KeyValuePair<string, decimal>(r.Ngay.ToString(labelFmt), r.DoanhThu))
                .ToList();
        }

        // ══════════════════════════════════════════
        // CHART DOANH THU THEO NGÀY
        // ══════════════════════════════════════════

        private void TaoChartDoanhThu(int x, int y, int w, int h)
        {
            var card = TaoCard(x, y, w, h);

            string kieuLoc = _locTheoThang ? "Theo Tháng" : "Theo Ngày";
            string khoangTG = _tuNgay.ToString("dd/MM/yyyy") + " — " + _denNgay.ToString("dd/MM/yyyy");

            card.Controls.Add(new Label
            {
                Text = "📊  Doanh Thu " + kieuLoc + " (" + khoangTG + ")",
                Font = AppFonts.H4,
                ForeColor = ColorScheme.TextDark,
                Location = new Point(16, 12),
                AutoSize = true,
                BackColor = Color.Transparent,
            });

            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                int chartX = 50, chartY = 50, chartH = h - 90, chartW = w - 80;

                if (_chartData.Count == 0 || _chartData.All(d => d.Value == 0))
                {
                    using (var br = new SolidBrush(ColorScheme.TextLight))
                    {
                        string msg = "Chưa có dữ liệu doanh thu trong khoảng thời gian này";
                        var sz = g.MeasureString(msg, AppFonts.Body);
                        g.DrawString(msg, AppFonts.Body, br, (w - sz.Width) / 2, h / 2);
                    }
                    return;
                }

                decimal maxVal = _chartData.Max(d => d.Value);
                if (maxVal == 0) maxVal = 1;

                int barGap = 6;
                int maxBarW = 80;
                int barW = Math.Min(maxBarW, Math.Max(16, (chartW - (_chartData.Count - 1) * barGap) / Math.Max(1, _chartData.Count)));
                int totalBarsW = _chartData.Count * barW + (_chartData.Count - 1) * barGap;
                int startX = chartX + (chartW - totalBarsW) / 2;

                for (int i = 0; i < _chartData.Count; i++)
                {
                    int bx = startX + i * (barW + barGap);
                    int barH = (int)((double)_chartData[i].Value / (double)maxVal * (chartH - 30));
                    if (barH < 2 && _chartData[i].Value > 0) barH = 2;
                    int by = chartY + chartH - barH;

                    // Gradient bar: PrimaryDark → Gold
                    float ratio = (float)i / Math.Max(1, _chartData.Count - 1);
                    var barColor = BlendColor(ColorScheme.PrimaryDark, GoldAccent, ratio);

                    using (var brush = new SolidBrush(barColor))
                    {
                        var rect = new Rectangle(bx, by, barW, barH);
                        g.FillRectangle(brush, rect);
                    }

                    // Value trên đầu bar
                    if (_chartData[i].Value > 0)
                    {
                        string valText = FormatTien(_chartData[i].Value);
                        using (var f = new Font("Segoe UI", 6.5f, FontStyle.Bold))
                        using (var br = new SolidBrush(ColorScheme.TextDark))
                        {
                            var sz = g.MeasureString(valText, f);
                            g.DrawString(valText, f, br, bx + (barW - sz.Width) / 2, by - 16);
                        }
                    }

                    // Nhãn ngày
                    using (var f = new Font("Segoe UI", 7f))
                    using (var br = new SolidBrush(ColorScheme.TextGray))
                    {
                        var sz = g.MeasureString(_chartData[i].Key, f);
                        g.DrawString(_chartData[i].Key, f, br, bx + (barW - sz.Width) / 2, chartY + chartH + 6);
                    }
                }
            };

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // GRID CHI TIẾT THEO NGÀY
        // ══════════════════════════════════════════

        private void TaoGridChiTiet(int x, int y, int w, int h)
        {
            var card = TaoCard(x, y, w, h);

            dgvChiTiet = new DataGridView
            {
                Location = new Point(0, 0),
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
                RowTemplate = { Height = 38 },
            };
            dgvChiTiet.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorScheme.PrimaryDark,
                ForeColor = Color.White,
                Font = AppFonts.BodyBold,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
            };
            dgvChiTiet.ColumnHeadersHeight = 38;
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvChiTiet.EnableHeadersVisualStyles = false;
            dgvChiTiet.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowOdd,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
                Padding = new Padding(8, 0, 0, 0),
            };
            dgvChiTiet.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowAlt,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
            };

            dgvChiTiet.CellPainting += DgvChiTiet_CellPainting;

            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ngay", HeaderText = "Ngày", FillWeight = 18 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoHD", HeaderText = "Số HĐ", FillWeight = 12 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "DoanhThu", HeaderText = "Doanh thu", FillWeight = 22 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "Thuoc", HeaderText = "Thuốc", FillWeight = 22 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "DichVu", HeaderText = "Dịch vụ", FillWeight = 22 });

            dgvChiTiet.CellFormatting += DgvChiTiet_CellFormatting;

            // Fill data
            string gridDateFmt = _locTheoThang ? "MM/yyyy" : "dd/MM";
            foreach (var row in _danhSachChiTiet)
            {
                dgvChiTiet.Rows.Add(
                    row.Ngay.ToString(gridDateFmt),
                    row.SoHoaDon.ToString(),
                    FormatTien(row.DoanhThu),
                    FormatTien(row.Thuoc),
                    FormatTien(row.DichVu)
                );
            }

            card.Controls.Add(dgvChiTiet);
            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // GRID EVENTS
        // ══════════════════════════════════════════

        private void DgvChiTiet_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1) return;

            e.Handled = true;
            var rect = e.CellBounds;

            using (var brush = new LinearGradientBrush(
                new Rectangle(0, rect.Y, dgvChiTiet.Width, rect.Height),
                ColorScheme.PrimaryDark, Color.FromArgb(180, GoldAccent.R, GoldAccent.G, GoldAccent.B),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            if (e.Value != null)
            {
                var textRect = new Rectangle(rect.X + 8, rect.Y, rect.Width - 8, rect.Height);
                TextRenderer.DrawText(e.Graphics, e.Value.ToString(), AppFonts.BodyBold,
                    textRect, Color.White, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            }
        }

        private void DgvChiTiet_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvChiTiet.Columns[e.ColumnIndex].Name;

            if (colName == "SoHD")
            {
                e.CellStyle.Font = AppFonts.BodyBold;
                e.CellStyle.ForeColor = ColorScheme.Primary;
            }

            if (colName == "DoanhThu")
            {
                e.CellStyle.Font = AppFonts.BodyBold;
                e.CellStyle.ForeColor = GoldAccent;
            }
        }

        // ══════════════════════════════════════════
        // XUẤT EXCEL (CSV UTF-8)
        // ══════════════════════════════════════════

        private void BtnXuatExcel_Click(object sender, EventArgs e)
        {
            if (_danhSachChiTiet.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files (*.csv)|*.csv";
                sfd.FileName = "BaoCaoDoanhThu_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";
                sfd.Title = "Xuất Báo Cáo Doanh Thu";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Ngày,Số HĐ,Doanh thu,Thuốc,Dịch vụ");

                    string csvDateFmt = _locTheoThang ? "MM/yyyy" : "dd/MM/yyyy";
                    foreach (var row in _danhSachChiTiet)
                    {
                        sb.AppendLine(
                            row.Ngay.ToString(csvDateFmt) + "," +
                            row.SoHoaDon + "," +
                            row.DoanhThu.ToString("0") + "," +
                            row.Thuoc.ToString("0") + "," +
                            row.DichVu.ToString("0")
                        );
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

        private string FormatTien(decimal soTien)
        {
            if (soTien >= 1_000_000_000)
                return (soTien / 1_000_000_000m).ToString("0.#") + "B";
            if (soTien >= 1_000_000)
                return (soTien / 1_000_000m).ToString("0.#") + "M";
            if (soTien >= 1_000)
                return (soTien / 1_000m).ToString("0.#") + "K";
            return soTien.ToString("N0") + "đ";
        }

        private static Color BlendColor(Color from, Color to, float ratio)
        {
            ratio = Math.Max(0, Math.Min(1, ratio));
            int r = (int)(from.R + (to.R - from.R) * ratio);
            int g = (int)(from.G + (to.G - from.G) * ratio);
            int b = (int)(from.B + (to.B - from.B) * ratio);
            return Color.FromArgb(r, g, b);
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
