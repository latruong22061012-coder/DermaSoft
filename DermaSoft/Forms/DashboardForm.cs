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
    public partial class DashboardForm : Form
    {
        private Panel pnlContent;

        public DashboardForm()
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

            // Dùng FlowLayoutPanel cho responsive
            pnlContent.Resize += (s, e) => VeLaiDashboard();
            VeLaiDashboard();
        }

        private bool _dangVeLai = false;

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

            // Row 1: KPI cards — chiếm 20% chiều cao
            int kpiH = Math.Max(130, (int)(contentH * 0.20));
            int y = pad;
            y = TaoKpiCards(pad, y, contentW, kpiH);

            // Còn lại chia 2 hàng
            int remainH = contentH - kpiH - gap * 2 - pad;
            int row2H = Math.Max(220, (int)(remainH * 0.55));
            int row3H = Math.Max(180, remainH - row2H - gap);

            // Row 2: Doanh thu + Phiếu khám
            int halfW = (contentW - gap) / 2;
            TaoDoanhThu7Ngay(pad, y, halfW, row2H);
            TaoPhieuKhamCho(pad + halfW + gap, y, halfW, row2H);
            y += row2H + gap;

            // Row 3: Nhân viên online + Cảnh báo
            TaoNhanVienOnline(pad, y, halfW, row3H);
            TaoCanhBaoHeThong(pad + halfW + gap, y, halfW, row3H);

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

            // Query SQL
            decimal doanhThuHomNay = 0;
            int luotKhamHomNay = 0;
            int lichHenSapToi = 0;
            int thuocSapHetHan = 0;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    // Doanh thu hôm nay (từ HoaDon)
                    using (var cmd = new SqlCommand(
                        @"SELECT ISNULL(SUM(hd.TongTien), 0) 
                          FROM HoaDon hd 
                          WHERE CAST(hd.NgayTao AS DATE) = CAST(GETDATE() AS DATE) 
                            AND hd.IsDeleted = 0", conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != DBNull.Value) doanhThuHomNay = Convert.ToDecimal(result);
                    }

                    // Lượt khám hôm nay (PhieuKham ngày hôm nay)
                    using (var cmd = new SqlCommand(
                        @"SELECT COUNT(*) FROM PhieuKham 
                          WHERE CAST(NgayKham AS DATE) = CAST(GETDATE() AS DATE) 
                            AND IsDeleted = 0", conn))
                    {
                        luotKhamHomNay = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Lịch hẹn sắp tới (Chờ xác nhận + Đã xác nhận, từ hôm nay trở đi)
                    using (var cmd = new SqlCommand(
                        @"SELECT COUNT(*) FROM LichHen 
                          WHERE TrangThai IN (0, 1) 
                            AND CAST(ThoiGianHen AS DATE) >= CAST(GETDATE() AS DATE)", conn))
                    {
                        lichHenSapToi = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Thuốc sắp hết hạn (trong 30 ngày)
                    using (var cmd = new SqlCommand(
                        @"SELECT COUNT(*) FROM Thuoc 
                          WHERE HanSuDung IS NOT NULL 
                            AND HanSuDung <= DATEADD(DAY, 30, GETDATE()) 
                            AND HanSuDung > GETDATE()", conn))
                    {
                        thuocSapHetHan = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch { }

            var kpis = new[]
            {
                new { Icon = "\uD83D\uDCB0", Value = FormatTien(doanhThuHomNay), Title = "Doanh thu h\u00f4m nay",   Accent = ColorScheme.Gold,    Sub = "" },
                new { Icon = "\uD83E\uDE7A", Value = luotKhamHomNay.ToString(),     Title = "L\u01b0\u1ee3t kh\u00e1m h\u00f4m nay",   Accent = ColorScheme.Primary, Sub = "" },
                new { Icon = "\uD83D\uDCC5", Value = lichHenSapToi.ToString(),      Title = "L\u1ecbch h\u1eb9n s\u1eafp t\u1edbi",     Accent = ColorScheme.Info,    Sub = "" },
                new { Icon = "\u26A0\uFE0F",  Value = thuocSapHetHan.ToString(),     Title = "Thu\u1ed1c s\u1eafp h\u1ebft h\u1ea1n",   Accent = ColorScheme.Danger,  Sub = thuocSapHetHan > 0 ? "\u25BC C\u1ea7n x\u1eed l\u00fd ngay" : "" },
            };

            for (int i = 0; i < kpis.Length; i++)
            {
                var kpi = kpis[i];
                int cx = x0 + i * (cardW + gap);
                var card = TaoCard(cx, y, cardW, cardH);

                // Accent bar top
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
        // ROW 2 LEFT — DOANH THU 7 NGÀY
        // ══════════════════════════════════════════

        private void TaoDoanhThu7Ngay(int x, int y, int w, int h)
        {
            var card = TaoCard(x, y, w, h);

            card.Controls.Add(new Label
            {
                Text = "\uD83D\uDCCA  Doanh Thu 7 Ng\u00e0y G\u1ea7n Nh\u1ea5t",
                Font = AppFonts.H4, ForeColor = ColorScheme.TextDark,
                Location = new Point(16, 12), AutoSize = true, BackColor = Color.Transparent,
            });

            // Query 7 ngày
            var data = new List<KeyValuePair<string, decimal>>();
            string queryError = null;
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    @"SELECT CAST(DATEADD(DAY, -n.n, GETDATE()) AS DATE) AS Ngay,
                             ISNULL((SELECT SUM(TongTien) FROM HoaDon 
                                     WHERE CAST(NgayTao AS DATE) = CAST(DATEADD(DAY, -n.n, GETDATE()) AS DATE)), 0) AS Tong
                      FROM (SELECT 0 AS n UNION SELECT 1 UNION SELECT 2 UNION SELECT 3 
                            UNION SELECT 4 UNION SELECT 5 UNION SELECT 6) n
                      ORDER BY Ngay ASC", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ngay = reader.IsDBNull(0) ? "" : reader.GetDateTime(0).ToString("dd/M");
                        var tong = reader.IsDBNull(1) ? 0m : Convert.ToDecimal(reader.GetValue(1));
                        data.Add(new KeyValuePair<string, decimal>(ngay, tong));
                    }
                }
            }
            catch (Exception ex) { queryError = ex.Message; }

            // Vẽ bar chart bằng Paint
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                int chartX = 50, chartY = 50, chartH = h - 90, chartW = w - 100;

                if (queryError != null)
                {
                    using (var br = new SolidBrush(ColorScheme.Danger))
                    {
                        g.DrawString("L\u1ed7i: " + queryError, AppFonts.Small, br, 16, h / 2);
                    }
                    return;
                }

                if (data.Count == 0 || data.All(d => d.Value == 0))
                {
                    using (var br = new SolidBrush(ColorScheme.TextLight))
                    {
                        string msg = "Ch\u01b0a c\u00f3 d\u1eef li\u1ec7u doanh thu";
                        var sz = g.MeasureString(msg, AppFonts.Body);
                        g.DrawString(msg, AppFonts.Body, br, (w - sz.Width) / 2, h / 2);
                    }
                    return;
                }

                decimal maxVal = data.Max(d => d.Value);
                if (maxVal == 0) maxVal = 1;

                int gap = 12;
                int barW = Math.Min(60, Math.Max(20, (chartW - (data.Count - 1) * gap) / data.Count));
                int totalBarsW = data.Count * barW + (data.Count - 1) * gap;
                int startX = chartX + (chartW - totalBarsW) / 2;

                for (int i = 0; i < data.Count; i++)
                {
                    int bx = startX + i * (barW + gap);
                    int barH = (int)((double)data[i].Value / (double)maxVal * (chartH - 30));
                    if (barH < 2 && data[i].Value > 0) barH = 2;
                    int by = chartY + chartH - barH;

                    bool isToday = (i == data.Count - 1);
                    var barColor = isToday ? ColorScheme.Gold : ColorScheme.Primary;

                    using (var brush = new SolidBrush(barColor))
                    {
                        var rect = new Rectangle(bx, by, barW, barH);
                        g.FillRectangle(brush, rect);
                    }

                    // Value label (chỉ hiện khi > 0)
                    if (data[i].Value > 0)
                    {
                        string valText = FormatTien(data[i].Value);
                        using (var f = new Font("Segoe UI", 7f, FontStyle.Bold))
                        using (var br = new SolidBrush(ColorScheme.TextDark))
                        {
                            var sz = g.MeasureString(valText, f);
                            g.DrawString(valText, f, br, bx + (barW - sz.Width) / 2, by - 18);
                        }
                    }

                    // Date label
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
        // ROW 2 RIGHT — PHIẾU KHÁM CHỜ THANH TOÁN
        // ══════════════════════════════════════════

        private void TaoPhieuKhamCho(int x, int y, int w, int h)
        {
            var card = TaoCard(x, y, w, h);

            card.Controls.Add(new Label
            {
                Text = "\uD83D\uDCC5  L\u1ecbch H\u1eb9n S\u1eafp T\u1edbi",
                Font = AppFonts.H4, ForeColor = ColorScheme.TextDark,
                Location = new Point(16, 12), AutoSize = true, BackColor = Color.Transparent,
            });

            // Table header — cột tính theo % width
            int col1 = 12;
            int col2 = (int)(w * 0.18);
            int col3 = (int)(w * 0.48);
            int col4 = w - 90;
            int headerY = 44;

            var pnlHeader = new Panel
            {
                Location = new Point(0, headerY),
                Size = new Size(w, 28),
                BackColor = ColorScheme.PrimaryDark,
            };
            card.Controls.Add(pnlHeader);

            pnlHeader.Controls.Add(new Label { Text = "Ng\u00e0y gi\u1edd", Font = AppFonts.Badge, ForeColor = Color.White, Location = new Point(col1, 4), AutoSize = true, BackColor = Color.Transparent });
            pnlHeader.Controls.Add(new Label { Text = "B\u1ec7nh nh\u00e2n", Font = AppFonts.Badge, ForeColor = Color.White, Location = new Point(col2, 4), AutoSize = true, BackColor = Color.Transparent });
            pnlHeader.Controls.Add(new Label { Text = "Ghi ch\u00fa", Font = AppFonts.Badge, ForeColor = Color.White, Location = new Point(col3, 4), AutoSize = true, BackColor = Color.Transparent });
            pnlHeader.Controls.Add(new Label { Text = "TT", Font = AppFonts.Badge, ForeColor = Color.White, Location = new Point(col4, 4), AutoSize = true, BackColor = Color.Transparent });

            // Scrollable data area
            var pnlData = new Panel
            {
                Location = new Point(0, headerY + 28),
                Size = new Size(w, h - headerY - 28),
                AutoScroll = true,
                BackColor = Color.White,
            };
            card.Controls.Add(pnlData);

            // Query lịch hẹn sắp tới
            var rows = new List<Tuple<string, string, string, int>>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    @"SELECT TOP 10 
                             lh.ThoiGianHen, 
                             CASE 
                                WHEN bn.HoTen IS NOT NULL THEN bn.HoTen
                                WHEN lh.SoDienThoaiKhach IS NOT NULL THEN N'SĐT: ' + lh.SoDienThoaiKhach
                                ELSE N'Không rõ'
                             END AS TenBN,
                             ISNULL(lh.GhiChu, N'') AS GhiChu,
                             lh.TrangThai
                      FROM LichHen lh
                      LEFT JOIN BenhNhan bn ON lh.MaBenhNhan = bn.MaBenhNhan
                      WHERE lh.TrangThai IN (0, 1)
                        AND CAST(lh.ThoiGianHen AS DATE) >= CAST(GETDATE() AS DATE)
                      ORDER BY lh.ThoiGianHen ASC", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string thoiGian = reader.IsDBNull(0) ? "" : reader.GetDateTime(0).ToString("dd/MM HH:mm");
                        string tenBN = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string ghiChu = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        int tt = reader.IsDBNull(3) ? 0 : Convert.ToInt32(reader.GetValue(3));
                        rows.Add(Tuple.Create(thoiGian, tenBN, ghiChu, tt));
                    }
                }
            }
            catch (Exception ex)
            {
                pnlData.Controls.Add(new Label
                {
                    Text = "Lỗi: " + ex.Message,
                    Font = AppFonts.Small, ForeColor = ColorScheme.Danger,
                    Location = new Point(16, 4), AutoSize = true, BackColor = Color.Transparent,
                });
            }

            int rowY = 4;
            foreach (var row in rows)
            {
                pnlData.Controls.Add(new Label { Text = row.Item1, Font = AppFonts.Body, ForeColor = ColorScheme.TextDark, Location = new Point(col1, rowY), AutoSize = true, BackColor = Color.Transparent });
                pnlData.Controls.Add(new Label { Text = row.Item2, Font = AppFonts.Body, ForeColor = ColorScheme.TextDark, Location = new Point(col2, rowY), AutoSize = true, BackColor = Color.Transparent });
                int maxGhiChu = 15;
                string ghiChuDisplay = row.Item3.Length > maxGhiChu ? row.Item3.Substring(0, maxGhiChu) + "..." : row.Item3;
                pnlData.Controls.Add(new Label { Text = ghiChuDisplay, Font = AppFonts.Small, ForeColor = ColorScheme.TextGray, Location = new Point(col3, rowY), AutoSize = true, BackColor = Color.Transparent });

                string ttText = row.Item4 == 0 ? "Ch\u1edd" : "X\u00e1c nh\u1eadn";
                Color badgeColor = row.Item4 == 0 ? ColorScheme.Warning : ColorScheme.Success;
                pnlData.Controls.Add(new Label
                {
                    Text = ttText, Font = AppFonts.Badge, ForeColor = Color.White,
                    BackColor = badgeColor, TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(col4, rowY - 2), Size = new Size(70, 22),
                });

                rowY += 30;
            }

            if (rows.Count == 0)
                pnlData.Controls.Add(new Label { Text = "Kh\u00f4ng c\u00f3 l\u1ecbch h\u1eb9n s\u1eafp t\u1edbi", Font = AppFonts.Body, ForeColor = ColorScheme.TextLight, Location = new Point(col1, rowY), AutoSize = true, BackColor = Color.Transparent });

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // ROW 3 LEFT — NHÂN VIÊN ONLINE
        // ══════════════════════════════════════════

        private void TaoNhanVienOnline(int x, int y, int w, int h)
        {
            var card = TaoCard(x, y, w, h);

            card.Controls.Add(new Label
            {
                Text = "\uD83D\uDC65  Nh\u00e2n Vi\u00ean Online H\u00f4m Nay",
                Font = AppFonts.H4, ForeColor = ColorScheme.TextDark,
                Location = new Point(16, 12), AutoSize = true, BackColor = Color.Transparent,
            });

            // Scrollable area for staff list
            var pnlScroll = new Panel
            {
                Location = new Point(0, 42),
                Size = new Size(w, h - 42),
                AutoScroll = true,
                BackColor = Color.White,
            };
            card.Controls.Add(pnlScroll);

            var nhanVien = new List<Tuple<string, string, string>>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    @"SELECT nd.HoTen, vt.TenVaiTro 
                      FROM NguoiDung nd 
                      JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
                      WHERE nd.TrangThaiTK = 1 AND nd.IsDeleted = 0
                        AND nd.MaVaiTro IN (1, 2, 3)
                      ORDER BY nd.MaVaiTro, nd.HoTen", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string hoTen = reader.GetString(0);
                        string vaiTro = reader.GetString(1);
                        string chu = hoTen.Length > 0 ? hoTen.Substring(0, 1).ToUpper() : "?";
                        nhanVien.Add(Tuple.Create(hoTen, vaiTro, chu));
                    }
                }
            }
            catch { }

            int rowY = 8;
            foreach (var nv in nhanVien)
            {
                var lblAvatar = new Label
                {
                    Text = nv.Item3, Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = Color.White, BackColor = ColorScheme.Primary,
                    Size = new Size(34, 34), Location = new Point(16, rowY),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                BoTronLabel(lblAvatar);

                var lblTen = new Label { Text = nv.Item1, Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextDark, Location = new Point(60, rowY + 2), AutoSize = true, BackColor = Color.Transparent };
                var lblRole = new Label { Text = nv.Item2, Font = AppFonts.Tiny, ForeColor = ColorScheme.TextGray, Location = new Point(60, rowY + 20), AutoSize = true, BackColor = Color.Transparent };

                var lblOnline = new Label
                {
                    Text = "Online", Font = AppFonts.Badge, ForeColor = ColorScheme.Success,
                    Location = new Point(w - 100, rowY + 8), AutoSize = true, BackColor = Color.Transparent,
                };

                pnlScroll.Controls.Add(lblAvatar);
                pnlScroll.Controls.Add(lblTen);
                pnlScroll.Controls.Add(lblRole);
                pnlScroll.Controls.Add(lblOnline);

                rowY += 48;
            }

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // ROW 3 RIGHT — CẢNH BÁO HỆ THỐNG
        // ══════════════════════════════════════════

        private void TaoCanhBaoHeThong(int x, int y, int w, int h)
        {
            var card = TaoCard(x, y, w, h);

            card.Controls.Add(new Label
            {
                Text = "\u26A0\uFE0F  C\u1ea3nh B\u00e1o H\u1ec7 Th\u1ed1ng",
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

            var alerts = new List<Tuple<string, Color>>();

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    using (var cmd = new SqlCommand(
                        @"SELECT TOP 3 TenThuoc, SoLuongTon, DATEDIFF(DAY, GETDATE(), HanSuDung) AS ConLai
                          FROM Thuoc 
                          WHERE HanSuDung IS NOT NULL 
                            AND HanSuDung <= DATEADD(DAY, 30, GETDATE()) 
                            AND HanSuDung > GETDATE()
                          ORDER BY HanSuDung ASC", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ten = reader.GetString(0);
                            int sl = Convert.ToInt32(reader.GetValue(1));
                            int conLai = Convert.ToInt32(reader.GetValue(2));
                            alerts.Add(Tuple.Create(
                                "\uD83D\uDC8A " + ten + " h\u1ebft h\u1ea1n trong " + conLai + " ng\u00e0y (" + sl + " c\u00f2n)",
                                ColorScheme.Danger));
                        }
                    }

                    using (var cmd = new SqlCommand(
                        @"SELECT TOP 3 TenThuoc, SoLuongTon 
                          FROM Thuoc 
                          WHERE SoLuongTon <= 5 AND SoLuongTon > 0
                          ORDER BY SoLuongTon ASC", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ten = reader.GetString(0);
                            int sl = Convert.ToInt32(reader.GetValue(1));
                            alerts.Add(Tuple.Create(
                                "\uD83D\uDCE6 " + ten + " c\u00f2n " + sl + " \u2014 S\u1eafp h\u1ebft",
                                ColorScheme.Warning));
                        }
                    }
                }
            }
            catch { }

            if (alerts.Count == 0)
                alerts.Add(Tuple.Create("\u2705 H\u1ec7 th\u1ed1ng ho\u1ea1t \u0111\u1ed9ng b\u00ecnh th\u01b0\u1eddng", ColorScheme.Success));

            int rowY = 8;
            foreach (var alert in alerts)
            {
                var bar = new Panel
                {
                    Location = new Point(16, rowY),
                    Size = new Size(w - 48, 32),
                    BackColor = Color.FromArgb(30, alert.Item2),
                };

                bar.Controls.Add(new Panel
                {
                    Location = new Point(0, 0),
                    Size = new Size(3, 32),
                    BackColor = alert.Item2,
                });

                bar.Controls.Add(new Label
                {
                    Text = alert.Item1, Font = AppFonts.Small, ForeColor = ColorScheme.TextDark,
                    Location = new Point(12, 6), AutoSize = true, BackColor = Color.Transparent,
                });

                pnlScroll.Controls.Add(bar);
                rowY += 40;
            }

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

        private string FormatTien(decimal soTien)
        {
            if (soTien >= 1_000_000)
                return (soTien / 1_000_000m).ToString("0.#") + "M";
            if (soTien >= 1_000)
                return (soTien / 1_000m).ToString("0.#") + "K";
            return soTien.ToString("N0") + "\u0111";
        }

        private void BoTronLabel(Label lbl)
        {
            lbl.Paint += (s, pe) =>
            {
                var g = pe.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                int d = Math.Min(lbl.Width, lbl.Height);
                using (var brush = new SolidBrush(lbl.BackColor))
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.Clear(lbl.Parent.BackColor);
                    g.FillEllipse(brush, 0, 0, d - 1, d - 1);
                    g.DrawString(lbl.Text, lbl.Font, Brushes.White, new RectangleF(0, 0, d, d), sf);
                }
            };
        }
    }
}
