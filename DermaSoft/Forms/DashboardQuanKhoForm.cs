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

namespace DermaSoft.Forms
{
    /// <summary>
    /// Dashboard dành cho vai trò Quản Kho — thống kê kho &amp; thuốc.
    /// KPI: Tổng thuốc, Tổng tồn kho, Sắp hết hạn, Tồn thấp.
    /// Charts: Nhập kho 7 ngày, Thuốc sắp hết hạn, Top tồn thấp, Phiếu nhập gần đây.
    /// </summary>
    public partial class DashboardQuanKhoForm : Form
    {
        private Panel pnlContent;
        private bool _dangVeLai = false;

        public DashboardQuanKhoForm()
        {
            InitializeComponent();
            TaoBoCuc();
        }

        // ══════════════════════════════════════════
        // BỐ CỤC CHÍNH
        // ══════════════════════════════════════════

        private void TaoBoCuc()
        {
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ColorScheme.Background,
            };
            this.Controls.Add(pnlContent);
            pnlContent.Resize += (s, e) => VeLaiDashboard();
            VeLaiDashboard();
        }

        private void VeLaiDashboard()
        {
            if (_dangVeLai) return;
            _dangVeLai = true;

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();

            int pad = 16;
            int gap = 12;
            int contentW = pnlContent.ClientSize.Width - pad * 2;
            int contentH = pnlContent.ClientSize.Height - pad * 2;

            // Row 1: KPI cards — 20%
            int kpiH = Math.Max(130, (int)(contentH * 0.20));
            int y = pad;
            y = TaoKpiCards(pad, y, contentW, kpiH);

            // Còn lại chia 2 hàng
            int remainH = contentH - kpiH - gap * 2 - pad;
            int row2H = Math.Max(220, (int)(remainH * 0.55));
            int row3H = Math.Max(180, remainH - row2H - gap);

            // Row 2: Nhập kho 7 ngày + Thuốc sắp hết hạn
            int halfW = (contentW - gap) / 2;
            TaoNhapKho7Ngay(pad, y, halfW, row2H);
            TaoThuocSapHetHan(pad + halfW + gap, y, halfW, row2H);
            y += row2H + gap;

            // Row 3: Top tồn thấp + Phiếu nhập gần đây
            TaoTopTonThap(pad, y, halfW, row3H);
            TaoPhieuNhapGanDay(pad + halfW + gap, y, halfW, row3H);

            pnlContent.ResumeLayout();
            _dangVeLai = false;
        }

        // ══════════════════════════════════════════
        // ROW 1 — 4 KPI CARDS
        // ══════════════════════════════════════════

        private int TaoKpiCards(int x0, int y, int totalW, int cardH)
        {
            int gap = 16;
            int cardW = (totalW - gap * 3) / 4;

            int tongThuoc = 0, tongTonKho = 0, thuocSapHetHan = 0, thuocTonThap = 0;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    // Tổng số thuốc đang hoạt động
                    using (var cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Thuoc WHERE IsDeleted = 0", conn))
                    {
                        tongThuoc = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Tổng số lượng tồn kho (chỉ lô còn hạn)
                    using (var cmd = new SqlCommand(
                        @"SELECT ISNULL(SUM(ctk.SoLuongConLai), 0)
                          FROM ChiTietNhapKho ctk
                          WHERE ctk.SoLuongConLai > 0
                            AND ctk.HanSuDung >= CAST(GETDATE() AS DATE)", conn))
                    {
                        tongTonKho = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Thuốc sắp hết hạn + đã hết hạn (đồng bộ với TonKhoForm)
                    // Bao gồm: đã hết hạn (< today) + sắp hết hạn (< 90 ngày)
                    using (var cmd = new SqlCommand(
                        @"SELECT COUNT(DISTINCT ctk.MaThuoc) 
                          FROM ChiTietNhapKho ctk
                          WHERE ctk.SoLuongConLai > 0
                            AND ctk.HanSuDung <= DATEADD(DAY, 90, GETDATE())", conn))
                    {
                        thuocSapHetHan = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Thuốc tồn thấp — dùng ngưỡng theo đơn vị tính (đồng bộ với TonKhoForm & AppSettings)
                    using (var cmd = new SqlCommand(
                        @"SELECT t.DonViTinh, ISNULL(SUM(ctk.SoLuongConLai), 0) AS TonThucTe
                          FROM Thuoc t
                          LEFT JOIN ChiTietNhapKho ctk ON t.MaThuoc = ctk.MaThuoc 
                                AND ctk.SoLuongConLai > 0
                                AND ctk.HanSuDung >= CAST(GETDATE() AS DATE)
                          WHERE t.IsDeleted = 0
                          GROUP BY t.MaThuoc, t.DonViTinh
                          HAVING ISNULL(SUM(ctk.SoLuongConLai), 0) > 0", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string dvt = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            int ton = Convert.ToInt32(reader.GetValue(1));
                            int[] nguong = AppSettings.LayNguong(dvt);
                            if (ton <= nguong[0]) // nguong[0] = NguongThap
                                thuocTonThap++;
                        }
                    }
                }
            }
            catch { }

            var kpis = new[]
            {
                new { Icon = "💊", Value = tongThuoc.ToString(),       Title = "Tổng danh mục thuốc", Accent = ColorScheme.Primary, Sub = "" },
                new { Icon = "📦", Value = tongTonKho.ToString("N0"),  Title = "Tổng tồn kho (còn hạn)", Accent = ColorScheme.Info,    Sub = "" },
                new { Icon = "⏰", Value = thuocSapHetHan.ToString(),  Title = "Hết hạn / Sắp hết hạn", Accent = ColorScheme.Warning, Sub = thuocSapHetHan > 0 ? "▼ Cần xử lý" : "" },
                new { Icon = "🚨", Value = thuocTonThap.ToString(),    Title = "Thuốc tồn thấp",     Accent = ColorScheme.Danger,  Sub = thuocTonThap > 0 ? "▼ Cần nhập thêm" : "" },
            };

            for (int i = 0; i < kpis.Length; i++)
            {
                var kpi = kpis[i];
                int cx = x0 + i * (cardW + gap);
                var card = TaoCard(cx, y, cardW, cardH);

                card.Controls.Add(new Panel { Size = new Size(cardW, 3), Location = new Point(0, 0), BackColor = kpi.Accent });

                card.Controls.Add(new Label
                {
                    Text = kpi.Icon, Font = new Font("Segoe UI Emoji", 20f),
                    Location = new Point(16, 16), AutoSize = true, BackColor = Color.Transparent,
                });
                card.Controls.Add(new Label
                {
                    Text = kpi.Value, Font = AppFonts.H1, ForeColor = ColorScheme.TextDark,
                    Location = new Point(16, 50), AutoSize = true, BackColor = Color.Transparent,
                });
                card.Controls.Add(new Label
                {
                    Text = kpi.Title, Font = AppFonts.Body, ForeColor = ColorScheme.TextGray,
                    Location = new Point(16, cardH - 35), AutoSize = true, BackColor = Color.Transparent,
                });

                if (!string.IsNullOrEmpty(kpi.Sub))
                {
                    card.Controls.Add(new Label
                    {
                        Text = kpi.Sub, Font = AppFonts.Badge, ForeColor = kpi.Accent,
                        Location = new Point(cardW - 140, cardH - 30), AutoSize = true, BackColor = Color.Transparent,
                    });
                }

                pnlContent.Controls.Add(card);
            }

            return y + cardH + 12;
        }

        // ══════════════════════════════════════════
        // ROW 2 LEFT — NHẬP KHO 7 NGÀY
        // ══════════════════════════════════════════

        private void TaoNhapKho7Ngay(int x, int y, int w, int h)
        {
            var card = TaoCard(x, y, w, h);

            card.Controls.Add(new Label
            {
                Text = "📊  Nhập Kho 7 Ngày Gần Nhất",
                Font = AppFonts.H4, ForeColor = ColorScheme.TextDark,
                Location = new Point(16, 12), AutoSize = true, BackColor = Color.Transparent,
            });

            var data = new List<KeyValuePair<string, int>>();
            string queryError = null;
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    @"SELECT CAST(DATEADD(DAY, -n.n, GETDATE()) AS DATE) AS Ngay,
                             ISNULL((SELECT COUNT(*) FROM PhieuNhapKho 
                                     WHERE CAST(NgayNhap AS DATE) = CAST(DATEADD(DAY, -n.n, GETDATE()) AS DATE)), 0) AS SoPhieu
                      FROM (SELECT 0 AS n UNION SELECT 1 UNION SELECT 2 UNION SELECT 3 
                            UNION SELECT 4 UNION SELECT 5 UNION SELECT 6) n
                      ORDER BY Ngay ASC", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ngay = reader.IsDBNull(0) ? "" : reader.GetDateTime(0).ToString("dd/M");
                        var soPhieu = reader.IsDBNull(1) ? 0 : Convert.ToInt32(reader.GetValue(1));
                        data.Add(new KeyValuePair<string, int>(ngay, soPhieu));
                    }
                }
            }
            catch (Exception ex) { queryError = ex.Message; }

            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                int chartX = 50, chartY = 50, chartH = h - 90, chartW = w - 100;

                if (queryError != null)
                {
                    using (var br = new SolidBrush(ColorScheme.Danger))
                        g.DrawString("Lỗi: " + queryError, AppFonts.Small, br, 16, h / 2);
                    return;
                }

                if (data.Count == 0 || data.All(d => d.Value == 0))
                {
                    using (var br = new SolidBrush(ColorScheme.TextLight))
                    {
                        string msg = "Chưa có phiếu nhập kho trong 7 ngày";
                        var sz = g.MeasureString(msg, AppFonts.Body);
                        g.DrawString(msg, AppFonts.Body, br, (w - sz.Width) / 2, h / 2);
                    }
                    return;
                }

                int maxVal = data.Max(d => d.Value);
                if (maxVal == 0) maxVal = 1;

                int barGap = 12;
                int barW = Math.Min(60, Math.Max(20, (chartW - (data.Count - 1) * barGap) / data.Count));
                int totalBarsW = data.Count * barW + (data.Count - 1) * barGap;
                int startX = chartX + (chartW - totalBarsW) / 2;

                for (int i = 0; i < data.Count; i++)
                {
                    int bx = startX + i * (barW + barGap);
                    int barH = (int)((double)data[i].Value / maxVal * (chartH - 30));
                    if (barH < 2 && data[i].Value > 0) barH = 2;
                    int by = chartY + chartH - barH;

                    bool isToday = (i == data.Count - 1);
                    var barColor = isToday ? ColorScheme.Gold : ColorScheme.Primary;

                    using (var brush = new SolidBrush(barColor))
                        g.FillRectangle(brush, bx, by, barW, barH);

                    if (data[i].Value > 0)
                    {
                        string valText = data[i].Value.ToString();
                        using (var f = new Font("Segoe UI", 8f, FontStyle.Bold))
                        using (var br = new SolidBrush(ColorScheme.TextDark))
                        {
                            var sz = g.MeasureString(valText, f);
                            g.DrawString(valText, f, br, bx + (barW - sz.Width) / 2, by - 18);
                        }
                    }

                    using (var f = new Font("Segoe UI", 7.5f))
                    using (var br = new SolidBrush(ColorScheme.TextGray))
                    {
                        var sz = g.MeasureString(data[i].Key, f);
                        g.DrawString(data[i].Key, f, br, bx + (barW - sz.Width) / 2, chartY + chartH + 6);
                    }
                }
            };

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // ROW 2 RIGHT — THUỐC SẮP HẾT HẠN
        // ══════════════════════════════════════════

        private void TaoThuocSapHetHan(int x, int y, int w, int h)
        {
            var card = TaoCard(x, y, w, h);

            card.Controls.Add(new Label
            {
                Text = "⏰  Thuốc Sắp Hết Hạn",
                Font = AppFonts.H4, ForeColor = ColorScheme.TextDark,
                Location = new Point(16, 12), AutoSize = true, BackColor = Color.Transparent,
            });

            // Table header
            int col1 = 12, col2 = (int)(w * 0.40), col3 = (int)(w * 0.65), col4 = w - 90;
            int headerY = 44;

            var pnlHeader = new Panel
            {
                Location = new Point(0, headerY),
                Size = new Size(w, 28),
                BackColor = ColorScheme.PrimaryDark,
            };
            card.Controls.Add(pnlHeader);

            pnlHeader.Controls.Add(new Label { Text = "Tên thuốc", Font = AppFonts.Badge, ForeColor = Color.White, Location = new Point(col1, 4), AutoSize = true, BackColor = Color.Transparent });
            pnlHeader.Controls.Add(new Label { Text = "SL còn", Font = AppFonts.Badge, ForeColor = Color.White, Location = new Point(col2, 4), AutoSize = true, BackColor = Color.Transparent });
            pnlHeader.Controls.Add(new Label { Text = "Còn lại", Font = AppFonts.Badge, ForeColor = Color.White, Location = new Point(col3, 4), AutoSize = true, BackColor = Color.Transparent });
            pnlHeader.Controls.Add(new Label { Text = "Mức", Font = AppFonts.Badge, ForeColor = Color.White, Location = new Point(col4, 4), AutoSize = true, BackColor = Color.Transparent });

            var pnlData = new Panel
            {
                Location = new Point(0, headerY + 28),
                Size = new Size(w, h - headerY - 28),
                AutoScroll = true,
                BackColor = Color.White,
            };
            card.Controls.Add(pnlData);

            var rows = new List<Tuple<string, int, int>>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    @"SELECT t.TenThuoc, ctk.SoLuongConLai, 
                             DATEDIFF(DAY, GETDATE(), ctk.HanSuDung) AS ConLai
                      FROM ChiTietNhapKho ctk
                      INNER JOIN Thuoc t ON t.MaThuoc = ctk.MaThuoc
                      WHERE ctk.SoLuongConLai > 0
                        AND ctk.HanSuDung <= DATEADD(DAY, 90, GETDATE()) 
                        AND ctk.HanSuDung > GETDATE()
                      ORDER BY ctk.HanSuDung ASC", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string ten = reader.GetString(0);
                        int sl = Convert.ToInt32(reader.GetValue(1));
                        int conLai = Convert.ToInt32(reader.GetValue(2));
                        rows.Add(Tuple.Create(ten, sl, conLai));
                    }
                }
            }
            catch { }

            int rowHeight = 30;
            int innerH = Math.Max(pnlData.Height, rows.Count * rowHeight + 8);
            var pnlInner = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(pnlData.Width - SystemInformation.VerticalScrollBarWidth - 2, innerH),
                BackColor = Color.White,
            };
            pnlData.Controls.Add(pnlInner);

            int rowY = 4;
            foreach (var row in rows)
            {
                string tenDisplay = row.Item1.Length > 18 ? row.Item1.Substring(0, 18) + "..." : row.Item1;
                pnlInner.Controls.Add(new Label { Text = tenDisplay, Font = AppFonts.Body, ForeColor = ColorScheme.TextDark, Location = new Point(col1, rowY), AutoSize = true, BackColor = Color.Transparent });
                pnlInner.Controls.Add(new Label { Text = row.Item2.ToString(), Font = AppFonts.Body, ForeColor = ColorScheme.TextDark, Location = new Point(col2, rowY), AutoSize = true, BackColor = Color.Transparent });
                pnlInner.Controls.Add(new Label { Text = row.Item3 + " ngày", Font = AppFonts.Small, ForeColor = ColorScheme.TextGray, Location = new Point(col3, rowY), AutoSize = true, BackColor = Color.Transparent });

                Color badgeColor = row.Item3 < 30 ? ColorScheme.Danger : ColorScheme.Warning;
                string mucText = row.Item3 < 30 ? "Gấp" : "Chú ý";
                pnlInner.Controls.Add(new Label
                {
                    Text = mucText, Font = AppFonts.Badge, ForeColor = Color.White,
                    BackColor = badgeColor, TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(col4, rowY - 2), Size = new Size(60, 22),
                });

                rowY += rowHeight;
            }

            if (rows.Count == 0)
                pnlInner.Controls.Add(new Label { Text = "✅ Không có thuốc sắp hết hạn", Font = AppFonts.Body, ForeColor = ColorScheme.Success, Location = new Point(col1, rowY), AutoSize = true, BackColor = Color.Transparent });

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // ROW 3 LEFT — TOP THUỐC TỒN THẤP
        // ══════════════════════════════════════════

        private void TaoTopTonThap(int x, int y, int w, int h)
        {
            var card = TaoCard(x, y, w, h);

            card.Controls.Add(new Label
            {
                Text = "📉  Top Thuốc Tồn Thấp",
                Font = AppFonts.H4, ForeColor = ColorScheme.TextDark,
                Location = new Point(16, 12), AutoSize = true, BackColor = Color.Transparent,
            });

            var pnlScroll = new Panel
            {
                Location = new Point(0, 42),
                Size = new Size(w, h - 42),
                AutoScroll = true,
                BackColor = Color.White,
            };
            card.Controls.Add(pnlScroll);

            var alerts = new List<Tuple<string, int, string, Color>>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    @"SELECT t.TenThuoc, ISNULL(SUM(ctk.SoLuongConLai), 0) AS TonThucTe, t.DonViTinh
                      FROM Thuoc t
                      LEFT JOIN ChiTietNhapKho ctk ON t.MaThuoc = ctk.MaThuoc 
                            AND ctk.SoLuongConLai > 0
                            AND ctk.HanSuDung >= CAST(GETDATE() AS DATE)
                      WHERE t.IsDeleted = 0
                      GROUP BY t.TenThuoc, t.DonViTinh
                      ORDER BY TonThucTe ASC", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string ten = reader.GetString(0);
                        int sl = Convert.ToInt32(reader.GetValue(1));
                        string dvt = reader.IsDBNull(2) ? "" : reader.GetString(2);

                        // Áp dụng ngưỡng theo đơn vị tính (đồng bộ TonKhoForm & AppSettings)
                        int[] nguong = AppSettings.LayNguong(dvt);
                        int nguongThap = nguong[0];
                        int nguongNguyHiem = nguong[1];

                        // Chỉ hiển thị thuốc có tồn <= ngưỡng thấp
                        if (sl > nguongThap) continue;

                        Color mau;
                        if (sl == 0) mau = ColorScheme.Danger;
                        else if (sl <= nguongNguyHiem) mau = ColorScheme.Danger;
                        else mau = ColorScheme.Warning;

                        alerts.Add(Tuple.Create(ten, sl, dvt, mau));
                        if (alerts.Count >= 10) break;
                    }
                }
            }
            catch { }

            int rowY = 8;
            foreach (var item in alerts)
            {
                var bar = new Panel
                {
                    Location = new Point(16, rowY),
                    Size = new Size(w - 48, 32),
                    BackColor = Color.FromArgb(30, item.Item4),
                };

                bar.Controls.Add(new Panel
                {
                    Location = new Point(0, 0),
                    Size = new Size(3, 32),
                    BackColor = item.Item4,
                });

                string icon = item.Item2 == 0 ? "🔴" : (item.Item4 == ColorScheme.Danger ? "🚨" : "🟡");
                string tenDisplay = item.Item1.Length > 20 ? item.Item1.Substring(0, 20) + "..." : item.Item1;
                bar.Controls.Add(new Label
                {
                    Text = icon + " " + tenDisplay + " — " + item.Item2 + " " + item.Item3,
                    Font = AppFonts.Small, ForeColor = ColorScheme.TextDark,
                    Location = new Point(12, 6), AutoSize = true, BackColor = Color.Transparent,
                });

                pnlScroll.Controls.Add(bar);
                rowY += 40;
            }

            if (alerts.Count == 0)
                pnlScroll.Controls.Add(new Label { Text = "✅ Kho đầy đủ", Font = AppFonts.Body, ForeColor = ColorScheme.Success, Location = new Point(16, 8), AutoSize = true, BackColor = Color.Transparent });

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // ROW 3 RIGHT — PHIẾU NHẬP GẦN ĐÂY
        // ══════════════════════════════════════════

        private void TaoPhieuNhapGanDay(int x, int y, int w, int h)
        {
            var card = TaoCard(x, y, w, h);

            card.Controls.Add(new Label
            {
                Text = "📋  Phiếu Nhập Gần Đây",
                Font = AppFonts.H4, ForeColor = ColorScheme.TextDark,
                Location = new Point(16, 12), AutoSize = true, BackColor = Color.Transparent,
            });

            var pnlScroll = new Panel
            {
                Location = new Point(0, 42),
                Size = new Size(w, h - 42),
                AutoScroll = true,
                BackColor = Color.White,
            };
            card.Controls.Add(pnlScroll);

            var phieuNhaps = new List<Tuple<string, string, string, string>>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    @"SELECT TOP 10 pn.MaPhieuNhap, 
                             CONVERT(VARCHAR(10), pn.NgayNhap, 103) AS NgayNhap,
                             ncc.TenNhaCungCap,
                             FORMAT(pn.TongGiaTri, 'N0') AS TongGT
                      FROM PhieuNhapKho pn
                      JOIN NhaCungCap ncc ON pn.MaNhaCungCap = ncc.MaNhaCungCap
                      ORDER BY pn.NgayNhap DESC", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string maPN = "PN" + Convert.ToInt32(reader.GetValue(0)).ToString("D3");
                        string ngay = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string ncc = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        string tongGT = reader.IsDBNull(3) ? "0" : reader.GetString(3);
                        phieuNhaps.Add(Tuple.Create(maPN, ngay, ncc, tongGT));
                    }
                }
            }
            catch { }

            int rowY = 8;
            foreach (var pn in phieuNhaps)
            {
                string nccShort = pn.Item3.Length > 22 ? pn.Item3.Substring(0, 22) + "..." : pn.Item3;

                pnlScroll.Controls.Add(new Label
                {
                    Text = pn.Item1, Font = AppFonts.Badge, ForeColor = ColorScheme.PrimaryDark,
                    Location = new Point(16, rowY + 2), AutoSize = true, BackColor = Color.Transparent,
                });
                pnlScroll.Controls.Add(new Label
                {
                    Text = pn.Item2 + " — " + nccShort,
                    Font = AppFonts.Small, ForeColor = ColorScheme.TextGray,
                    Location = new Point(80, rowY + 4), AutoSize = true, BackColor = Color.Transparent,
                });
                pnlScroll.Controls.Add(new Label
                {
                    Text = pn.Item4 + "đ", Font = AppFonts.Badge, ForeColor = ColorScheme.Gold,
                    Location = new Point(w - 120, rowY + 2), AutoSize = true, BackColor = Color.Transparent,
                });

                // Divider
                pnlScroll.Controls.Add(new Panel
                {
                    Location = new Point(16, rowY + 26),
                    Size = new Size(w - 64, 1),
                    BackColor = ColorScheme.Border,
                });

                rowY += 34;
            }

            if (phieuNhaps.Count == 0)
                pnlScroll.Controls.Add(new Label { Text = "Chưa có phiếu nhập kho", Font = AppFonts.Body, ForeColor = ColorScheme.TextLight, Location = new Point(16, 8), AutoSize = true, BackColor = Color.Transparent });

            pnlContent.Controls.Add(card);
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
    }
}
