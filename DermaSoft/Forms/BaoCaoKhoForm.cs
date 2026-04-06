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
    /// Form Báo Cáo Kho — frm-baocaokho trong wireframe.
    /// Admin: xem xuất nhập tồn kho, cảnh báo tồn thấp/nguy hiểm.
    /// </summary>
    public partial class BaoCaoKhoForm : Form
    {
        // ── Controls ──
        private Panel pnlContent;
        private Guna2DateTimePicker dtpTuNgay;
        private Guna2DateTimePicker dtpDenNgay;
        private Guna2ComboBox cboThuoc;
        private DataGridView dgvKho;

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

        private bool _dangVeLai = false;

        // Lưu trạng thái filter
        private DateTime _savedTuNgay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        private DateTime _savedDenNgay = DateTime.Now;
        private int _savedThuocIdx = 0;

        // Dữ liệu
        private List<KhoRow> _danhSach = new List<KhoRow>();
        private List<KeyValuePair<int, string>> _dsThuoc = new List<KeyValuePair<int, string>>();

        private class KhoRow
        {
            public string TenThuoc;
            public string DonVi;
            public int TonDauKy;
            public int NhapKyNay;
            public int XuatKyNay;
            public int TonCuoiKy;
            public string TrangThai;
        }

        public BaoCaoKhoForm()
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

            LoadDanhSachThuoc();

            pnlContent.Resize += (s, e) => VeLaiForm();
            VeLaiForm();
        }

        private void LoadDanhSachThuoc()
        {
            _dsThuoc.Clear();
            _dsThuoc.Add(new KeyValuePair<int, string>(0, "Tất cả"));
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand("SELECT MaThuoc, TenThuoc FROM Thuoc ORDER BY TenThuoc", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        _dsThuoc.Add(new KeyValuePair<int, string>(reader.GetInt32(0), reader.GetString(1)));
                }
            }
            catch { }
        }

        private void VeLaiForm()
        {
            if (_dangVeLai) return;
            _dangVeLai = true;

            if (dtpTuNgay != null) _savedTuNgay = dtpTuNgay.Value.Date;
            if (dtpDenNgay != null) _savedDenNgay = dtpDenNgay.Value.Date;
            if (cboThuoc != null) _savedThuocIdx = cboThuoc.SelectedIndex;

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();

            int pad = 16;
            int contentW = pnlContent.ClientSize.Width - pad * 2;
            int y = pad;

            y = TaoHeader(pad, y, contentW);
            y = TaoFilterBar(pad, y, contentW);
            dtpTuNgay.Value = _savedTuNgay;
            dtpDenNgay.Value = _savedDenNgay;
            if (_savedThuocIdx < cboThuoc.Items.Count) cboThuoc.SelectedIndex = _savedThuocIdx;

            LoadDuLieu();

            int gridH = Math.Max(300, pnlContent.ClientSize.Height - y - pad);
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
                Text = "📦  Báo Cáo Xuất Nhập Tồn Kho",
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

            const int CBO_W = 220, BTN_LOC_W = 120, BTN_EXCEL_W = 140, GAP = 12, PAD = 16;
            int cy = 8;

            int fixedW = CBO_W + BTN_LOC_W + BTN_EXCEL_W + GAP * 4;
            int dtpW = Math.Max(160, (w - PAD * 2 - fixedW - GAP) / 2);

            int cx = PAD;

            // Từ ngày
            card.Controls.Add(new Label
            {
                Text = "Từ ngày", Font = AppFonts.Body, ForeColor = ColorScheme.TextGray,
                Location = new Point(cx, cy), AutoSize = true, BackColor = Color.Transparent,
            });
            dtpTuNgay = new Guna2DateTimePicker
            {
                Font = AppFonts.Body, ForeColor = ColorScheme.TextDark,
                Location = new Point(cx, cy + 22), Size = new Size(dtpW, 36),
                BorderRadius = 8, BorderColor = BorderInput, FillColor = Color.White,
                Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                Format = DateTimePickerFormat.Short,
            };
            card.Controls.Add(dtpTuNgay);
            cx += dtpW + GAP;

            // Đến ngày
            card.Controls.Add(new Label
            {
                Text = "Đến ngày", Font = AppFonts.Body, ForeColor = ColorScheme.TextGray,
                Location = new Point(cx, cy), AutoSize = true, BackColor = Color.Transparent,
            });
            dtpDenNgay = new Guna2DateTimePicker
            {
                Font = AppFonts.Body, ForeColor = ColorScheme.TextDark,
                Location = new Point(cx, cy + 22), Size = new Size(dtpW, 36),
                BorderRadius = 8, BorderColor = BorderInput, FillColor = Color.White,
                Value = DateTime.Now, Format = DateTimePickerFormat.Short,
            };
            card.Controls.Add(dtpDenNgay);
            cx += dtpW + GAP;

            // Thuốc
            card.Controls.Add(new Label
            {
                Text = "Thuốc", Font = AppFonts.Body, ForeColor = ColorScheme.TextGray,
                Location = new Point(cx, cy), AutoSize = true, BackColor = Color.Transparent,
            });
            cboThuoc = new Guna2ComboBox
            {
                Font = AppFonts.Body, ForeColor = ColorScheme.TextDark,
                Location = new Point(cx, cy + 22), Size = new Size(CBO_W, 36),
                BorderRadius = 8, BorderColor = BorderInput, FillColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            foreach (var item in _dsThuoc)
                cboThuoc.Items.Add(item.Value);
            cboThuoc.SelectedIndex = 0;
            card.Controls.Add(cboThuoc);
            cx += CBO_W + GAP;

            // Nút Lọc
            var btnLoc = new Guna2Button
            {
                Text = "🔍 Lọc", Font = AppFonts.BodyBold, ForeColor = Color.White,
                FillColor = ColorScheme.Primary, BorderRadius = 18,
                Location = new Point(cx, cy + 22), Size = new Size(BTN_LOC_W, 36),
                Cursor = Cursors.Hand,
            };
            btnLoc.Click += (s, e) => VeLaiForm();
            card.Controls.Add(btnLoc);
            cx += BTN_LOC_W + GAP;

            // Nút Xuất Excel
            var btnExcel = new Guna2Button
            {
                Text = "📥 Xuất Excel", Font = AppFonts.BodyBold,
                ForeColor = ColorScheme.PrimaryDark, FillColor = Color.White,
                BorderRadius = 18, BorderThickness = 1, BorderColor = ColorScheme.Primary,
                Location = new Point(cx, cy + 22), Size = new Size(BTN_EXCEL_W, 36),
                Cursor = Cursors.Hand,
            };
            btnExcel.Click += BtnXuatExcel_Click;
            card.Controls.Add(btnExcel);

            pnlContent.Controls.Add(card);
            return y + 80 + 12;
        }

        // ══════════════════════════════════════════
        // LOAD DỮ LIỆU
        // ══════════════════════════════════════════

        private void LoadDuLieu()
        {
            DateTime tuNgay = dtpTuNgay?.Value.Date ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime denNgay = dtpDenNgay?.Value.Date ?? DateTime.Now.Date;
            int thuocIdx = cboThuoc?.SelectedIndex ?? 0;
            int maThuocFilter = thuocIdx > 0 && thuocIdx < _dsThuoc.Count ? _dsThuoc[thuocIdx].Key : 0;

            _danhSach.Clear();

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string filterThuoc = maThuocFilter > 0 ? " AND t.MaThuoc = @MaThuoc" : "";

                    string sql = @"
                        SELECT 
                            t.TenThuoc,
                            t.DonViTinh,
                            t.SoLuongTon AS TonHienTai,
                            ISNULL(nhap.SoNhap, 0) AS NhapKyNay,
                            ISNULL(xuat.SoXuat, 0) AS XuatKyNay
                        FROM Thuoc t
                        LEFT JOIN (
                            SELECT ct.MaThuoc, SUM(ct.SoLuong) AS SoNhap
                            FROM ChiTietNhapKho ct
                            JOIN PhieuNhapKho pn ON ct.MaPhieuNhap = pn.MaPhieuNhap
                            WHERE CAST(pn.NgayNhap AS DATE) >= @Tu
                              AND CAST(pn.NgayNhap AS DATE) <= @Den
                            GROUP BY ct.MaThuoc
                        ) nhap ON t.MaThuoc = nhap.MaThuoc
                        LEFT JOIN (
                            SELECT ct.MaThuoc, SUM(ct.SoLuong) AS SoXuat
                            FROM ChiTietDonThuoc ct
                            JOIN PhieuKham pk ON ct.MaPhieuKham = pk.MaPhieuKham
                            WHERE CAST(pk.NgayKham AS DATE) >= @Tu
                              AND CAST(pk.NgayKham AS DATE) <= @Den
                              AND pk.IsDeleted = 0
                            GROUP BY ct.MaThuoc
                        ) xuat ON t.MaThuoc = xuat.MaThuoc
                        WHERE 1=1" + filterThuoc + @"
                        ORDER BY t.TenThuoc";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Tu", tuNgay);
                        cmd.Parameters.AddWithValue("@Den", denNgay);
                        if (maThuocFilter > 0)
                            cmd.Parameters.AddWithValue("@MaThuoc", maThuocFilter);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string tenThuoc = reader.GetString(0);
                                string donVi = reader.GetString(1);
                                int tonHienTai = Convert.ToInt32(reader.GetValue(2));
                                int nhapKy = Convert.ToInt32(reader.GetValue(3));
                                int xuatKy = Convert.ToInt32(reader.GetValue(4));

                                // Tồn cuối kỳ = tồn hiện tại (SoLuongTon)
                                // Tồn đầu kỳ = tồn cuối kỳ - nhập kỳ này + xuất kỳ này
                                int tonCuoiKy = tonHienTai;
                                int tonDauKy = tonCuoiKy - nhapKy + xuatKy;
                                if (tonDauKy < 0) tonDauKy = 0;

                                string trangThai;
                                if (tonCuoiKy <= AppSettings.NguongNguyHiem) trangThai = "Nguy hiểm";
                                else if (tonCuoiKy <= AppSettings.NguongThap) trangThai = "Thấp";
                                else trangThai = "OK";

                                _danhSach.Add(new KhoRow
                                {
                                    TenThuoc = tenThuoc,
                                    DonVi = donVi,
                                    TonDauKy = tonDauKy,
                                    NhapKyNay = nhapKy,
                                    XuatKyNay = xuatKy,
                                    TonCuoiKy = tonCuoiKy,
                                    TrangThai = trangThai,
                                });
                            }
                        }
                    }
                }
            }
            catch { }
        }

        // ══════════════════════════════════════════
        // DATAGRIDVIEW
        // ══════════════════════════════════════════

        private void TaoGrid(int x, int y, int w, int h)
        {
            dgvKho = new DataGridView
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
            dgvKho.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorScheme.PrimaryDark,
                ForeColor = Color.White,
                Font = AppFonts.BodyBold,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvKho.ColumnHeadersHeight = 42;
            dgvKho.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvKho.EnableHeadersVisualStyles = false;
            dgvKho.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowOdd,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
                Padding = new Padding(12, 0, 0, 0),
            };
            dgvKho.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = RowAlt,
                ForeColor = ColorScheme.TextMid,
                SelectionBackColor = ColorScheme.PrimaryPale,
                SelectionForeColor = ColorScheme.TextDark,
            };

            dgvKho.CellPainting += DgvKho_CellPainting;

            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenThuoc", HeaderText = "Thuốc", FillWeight = 22 });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonVi", HeaderText = "Đơn vị", FillWeight = 10 });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "TonDauKy", HeaderText = "Tồn đầu kỳ", FillWeight = 13 });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "NhapKyNay", HeaderText = "Nhập kỳ này", FillWeight = 13 });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "XuatKyNay", HeaderText = "Xuất kỳ này", FillWeight = 13 });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "TonCuoiKy", HeaderText = "Tồn cuối kỳ", FillWeight = 13 });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { Name = "TrangThai", HeaderText = "Trạng thái", FillWeight = 14 });

            dgvKho.CellFormatting += DgvKho_CellFormatting;

            foreach (var row in _danhSach)
            {
                dgvKho.Rows.Add(
                    row.TenThuoc,
                    row.DonVi,
                    row.TonDauKy.ToString(),
                    row.NhapKyNay.ToString(),
                    row.XuatKyNay.ToString(),
                    row.TonCuoiKy.ToString(),
                    row.TrangThai
                );
            }

            pnlContent.Controls.Add(dgvKho);
        }

        // ══════════════════════════════════════════
        // GRID EVENTS
        // ══════════════════════════════════════════

        private void DgvKho_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1) return;

            e.Handled = true;
            var rect = e.CellBounds;

            using (var brush = new LinearGradientBrush(
                new Rectangle(0, rect.Y, dgvKho.Width, rect.Height),
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

        private void DgvKho_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvKho.Columns[e.ColumnIndex].Name;

            if (colName == "TenThuoc")
            {
                e.CellStyle.Font = AppFonts.BodyBold;
                e.CellStyle.ForeColor = ColorScheme.TextDark;
            }

            if (colName == "TonCuoiKy")
            {
                e.CellStyle.Font = AppFonts.BodyBold;

                string tt = dgvKho.Rows[e.RowIndex].Cells["TrangThai"].Value?.ToString() ?? "";
                if (tt == "Nguy hiểm") e.CellStyle.ForeColor = BadgeDangerFg;
                else if (tt == "Thấp") e.CellStyle.ForeColor = BadgeWarningFg;
                else e.CellStyle.ForeColor = ColorScheme.TextDark;
            }

            if (colName == "TrangThai" && e.Value != null)
            {
                string tt = e.Value.ToString();
                if (tt == "OK")
                {
                    e.CellStyle.BackColor = BadgeOkBg;
                    e.CellStyle.ForeColor = BadgeOkFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else if (tt == "Thấp")
                {
                    e.CellStyle.BackColor = BadgeWarningBg;
                    e.CellStyle.ForeColor = BadgeWarningFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
                else if (tt == "Nguy hiểm")
                {
                    e.CellStyle.BackColor = BadgeDangerBg;
                    e.CellStyle.ForeColor = BadgeDangerFg;
                    e.CellStyle.Font = AppFonts.Badge;
                }
            }
        }

        // ══════════════════════════════════════════
        // XUẤT EXCEL (CSV UTF-8)
        // ══════════════════════════════════════════

        private void BtnXuatExcel_Click(object sender, EventArgs e)
        {
            if (_danhSach.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files (*.csv)|*.csv";
                sfd.FileName = "BaoCaoKho_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";
                sfd.Title = "Xuất Báo Cáo Kho";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Thuốc,Đơn vị,Tồn đầu kỳ,Nhập kỳ này,Xuất kỳ này,Tồn cuối kỳ,Trạng thái");

                    foreach (var row in _danhSach)
                    {
                        sb.AppendLine(
                            "\"" + row.TenThuoc + "\"," +
                            row.DonVi + "," +
                            row.TonDauKy + "," +
                            row.NhapKyNay + "," +
                            row.XuatKyNay + "," +
                            row.TonCuoiKy + "," +
                            row.TrangThai
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
