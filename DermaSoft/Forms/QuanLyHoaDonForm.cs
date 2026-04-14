using System;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Form Quản Lý Hóa Đơn — Admin xem lịch sử toàn bộ hóa đơn.
    /// Chức năng: Lọc theo ngày, trạng thái | 4 KPI | Danh sách | Chi tiết (Sửa/Xóa bên trong)
    /// Logic: Chỉ sửa/xóa hóa đơn chưa thanh toán (TrangThai=0). Đã TT → chỉ xem.
    /// </summary>
    public partial class QuanLyHoaDonForm : Form
    {
        // ── Controls ──────────────────────────────────────────────────────────
        private Panel pnlContent;
        private Guna2DateTimePicker dtpNgayLoc;
        private Guna2ComboBox cboTrangThai;
        private DataGridView dgvHoaDon;

        // ── KPI labels ────────────────────────────────────────────────────────
        private Label lblKpiDaTT;
        private Label lblKpiChuaTT;
        private Label lblKpiHomNay;
        private Label lblKpiThang;

        // ── Màu đồng bộ ──────────────────────────────────────────────────────
        private static readonly Color GoldAccent = Color.FromArgb(184, 138, 40);
        private static readonly Color BorderInput = ColorTranslator.FromHtml("#D1E8DC");

        private bool _dangVeLai = false;

        // Lưu trạng thái filter qua các lần VeLaiForm()
        private DateTime _savedNgay = DateTime.Today;
        private int _savedTrangThaiIdx = 0;

        public QuanLyHoaDonForm()
        {
            InitializeComponent();
            TaoBoCuc();
        }

        // ══════════════════════════════════════════════════════════════════════
        // BỐ CỤC CHÍNH
        // ══════════════════════════════════════════════════════════════════════

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
            if (dtpNgayLoc != null) _savedNgay = dtpNgayLoc.Value.Date;
            if (cboTrangThai != null && cboTrangThai.SelectedIndex >= 0)
                _savedTrangThaiIdx = cboTrangThai.SelectedIndex;

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();

            int pad = 16;
            int contentW = pnlContent.ClientSize.Width - pad * 2;
            int y = pad;

            // ── Row 0: Header ──
            y = TaoHeader(pad, y, contentW);

            // ── Row 1: Filter bar ──
            y = TaoFilterBar(pad, y, contentW);
            dtpNgayLoc.Value = _savedNgay;
            cboTrangThai.SelectedIndex = _savedTrangThaiIdx;

            // ── Row 2: 4 KPI cards ──
            y = TaoKpiCards(pad, y, contentW);

            // ── Row 3: DataGridView ──
            int remainH = Math.Max(300, pnlContent.ClientSize.Height - y - pad);
            TaoDanhSachHoaDon(pad, y, contentW, remainH);

            // ── Load dữ liệu ──
            LoadDuLieu();

            pnlContent.ResumeLayout();
            _dangVeLai = false;
        }

        // ══════════════════════════════════════════════════════════════════════
        // HEADER
        // ══════════════════════════════════════════════════════════════════════

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
                    ColorScheme.PrimaryDark,
                    Color.FromArgb(180, GoldAccent.R, GoldAccent.G, GoldAccent.B),
                    LinearGradientMode.Horizontal))
                using (var path = TaoRoundedRect(new Rectangle(0, 0, pnlHeader.Width - 1, pnlHeader.Height - 1), 10))
                {
                    g.FillPath(brush, path);
                }
            };

            pnlHeader.Controls.Add(new Label
            {
                Text = "🧾  Quản Lý Hóa Đơn",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(16, 10),
                AutoSize = true,
                BackColor = Color.Transparent,
            });

            pnlContent.Controls.Add(pnlHeader);
            return y + 44 + 12;
        }

        // ══════════════════════════════════════════════════════════════════════
        // FILTER BAR
        // ══════════════════════════════════════════════════════════════════════

        private int TaoFilterBar(int x, int y, int w)
        {
            var card = TaoCard(x, y, w, 80);

            const int GAP = 12, PAD = 16;
            int cy = 8, cx = PAD;

            // ── Ngày ──
            card.Controls.Add(new Label
            {
                Text = "Ngày",
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextGray,
                Location = new Point(cx, cy),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            dtpNgayLoc = new Guna2DateTimePicker
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Location = new Point(cx, cy + 22),
                Size = new Size(180, 36),
                BorderRadius = 8,
                BorderColor = BorderInput,
                FillColor = Color.White,
                Value = DateTime.Today,
                CustomFormat = "dd/MM/yyyy",
                Format = DateTimePickerFormat.Custom,
            };
            card.Controls.Add(dtpNgayLoc);
            cx += 180 + GAP;

            // ── Nút Hôm nay ──
            var btnHomNay = new Guna2Button
            {
                Text = "Hôm nay",
                Font = AppFonts.BodyBold,
                ForeColor = ColorScheme.PrimaryDark,
                FillColor = ColorScheme.PrimaryPale,
                BorderRadius = 18,
                Location = new Point(cx, cy + 22),
                Size = new Size(100, 36),
                Cursor = Cursors.Hand,
            };
            btnHomNay.Click += (s, e) =>
            {
                dtpNgayLoc.Value = DateTime.Today;
                LoadDuLieu();
            };
            card.Controls.Add(btnHomNay);
            cx += 100 + GAP;

            // ── Trạng thái ──
            card.Controls.Add(new Label
            {
                Text = "Trạng thái",
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextGray,
                Location = new Point(cx, cy),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            cboTrangThai = new Guna2ComboBox
            {
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextDark,
                Location = new Point(cx, cy + 22),
                Size = new Size(200, 36),
                BorderRadius = 8,
                BorderColor = BorderInput,
                FillColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Đã thanh toán", "Chưa thanh toán" });
            cboTrangThai.SelectedIndex = 0;
            card.Controls.Add(cboTrangThai);
            cx += 200 + GAP;

            // ── Nút Lọc ──
            var btnLoc = new Guna2Button
            {
                Text = "🔍 Lọc",
                Font = AppFonts.BodyBold,
                ForeColor = Color.White,
                FillColor = ColorScheme.Primary,
                BorderRadius = 18,
                Location = new Point(cx, cy + 22),
                Size = new Size(120, 36),
                Cursor = Cursors.Hand,
            };
            btnLoc.Click += (s, e) => LoadDuLieu();
            card.Controls.Add(btnLoc);

            pnlContent.Controls.Add(card);
            return y + 80 + 12;
        }

        // ══════════════════════════════════════════════════════════════════════
        // 4 KPI CARDS
        // ══════════════════════════════════════════════════════════════════════

        private int TaoKpiCards(int x0, int y, int totalW)
        {
            int gap = 16;
            int cardW = (totalW - gap * 3) / 4;
            int cardH = 150;

            var kpis = new[]
            {
                new { Icon = "✅", Value = lblKpiDaTT      = new Label(), Title = "Đã thanh toán",    Accent = ColorScheme.Success },
                new { Icon = "⏳", Value = lblKpiChuaTT    = new Label(), Title = "Chưa thanh toán",  Accent = ColorScheme.Warning },
                new { Icon = "📋", Value = lblKpiHomNay    = new Label(), Title = "Hóa đơn hôm nay",  Accent = ColorScheme.Primary },
                new { Icon = "📊", Value = lblKpiThang     = new Label(), Title = "Hóa đơn tháng",    Accent = GoldAccent },
            };

            for (int i = 0; i < kpis.Length; i++)
            {
                var kpi = kpis[i];
                int cx = x0 + i * (cardW + gap);
                var card = TaoCard(cx, y, cardW, cardH);

                // Accent bar top
                card.Controls.Add(new Panel
                {
                    Size = new Size(cardW, 3),
                    Location = new Point(0, 0),
                    BackColor = kpi.Accent,
                });

                // Icon emoji
                card.Controls.Add(new Label
                {
                    Text = kpi.Icon,
                    Font = new Font("Segoe UI Emoji", 20f),
                    Location = new Point(16, 14),
                    AutoSize = true,
                    BackColor = Color.Transparent,
                });

                // Value
                kpi.Value.Font = AppFonts.H1;
                kpi.Value.ForeColor = ColorScheme.TextDark;
                kpi.Value.Location = new Point(16, 50);
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

        // ══════════════════════════════════════════════════════════════════════
        // DANH SÁCH HÓA ĐƠN
        // ══════════════════════════════════════════════════════════════════════

        private void TaoDanhSachHoaDon(int x, int y, int w, int h)
        {
            var card = TaoCard(x, y, w, h);

            int titleH = 44;

            // Title bar (absolute position, NOT Dock)
            var pnlTitle = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(w, titleH),
                BackColor = Color.White,
            };
            pnlTitle.Controls.Add(new Label
            {
                Text = "📄  Danh sách hóa đơn",
                Font = AppFonts.H4,
                ForeColor = ColorScheme.PrimaryDark,
                Location = new Point(12, 12),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            card.Controls.Add(pnlTitle);

            // DataGridView (absolute position below title, NOT Dock)
            dgvHoaDon = new DataGridView
            {
                Location = new Point(0, titleH),
                Size = new Size(w, h - titleH),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersHeight = 40,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersVisible = true,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(226, 237, 232),
                ReadOnly = true,
                RowHeadersVisible = false,
                RowTemplate = { Height = 38 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = AppFonts.Body,
                AutoGenerateColumns = false,
            };

            // Header style
            dgvHoaDon.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorScheme.PrimaryDark,
                ForeColor = Color.White,
                Font = AppFonts.BodyBold,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                SelectionBackColor = ColorScheme.PrimaryDark,
                SelectionForeColor = Color.White,
            };

            // Row style
            dgvHoaDon.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ColorScheme.TextDark,
                SelectionBackColor = Color.FromArgb(221, 245, 229),
                SelectionForeColor = ColorScheme.PrimaryDark,
            };
            dgvHoaDon.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(240, 250, 245),
            };

            // Columns
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMaHD", DataPropertyName = "MaHoaDon", HeaderText = "Mã HĐ", FillWeight = 8F, Visible = false });
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMaHDCode", DataPropertyName = "MaHDCode", HeaderText = "Mã HĐ", FillWeight = 10F });
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTenBN", DataPropertyName = "TenBenhNhan", HeaderText = "Bệnh nhân", FillWeight = 18F });
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNgayTT", DataPropertyName = "NgayThanhToanText", HeaderText = "Ngày TT", FillWeight = 14F });
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTongDV", DataPropertyName = "TongDVText", HeaderText = "Dịch vụ", FillWeight = 12F });
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTongThuoc", DataPropertyName = "TongThuocText", HeaderText = "Thuốc", FillWeight = 12F });
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGiamGia", DataPropertyName = "GiamGiaText", HeaderText = "Giảm giá", FillWeight = 10F });
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTongTien", DataPropertyName = "TongTienText", HeaderText = "Tổng tiền", FillWeight = 14F });
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPhuongThuc", DataPropertyName = "PhuongThucThanhToan", HeaderText = "Phương thức", FillWeight = 12F });
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTrangThai", DataPropertyName = "TrangThaiText", HeaderText = "Trạng thái", FillWeight = 12F });

            // Double-click → Mở chi tiết
            dgvHoaDon.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0) MoChiTietHoaDon(LayMaHoaDonDangChon());
            };

            card.Controls.Add(dgvHoaDon);

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════════════════════════════════
        // LOAD DỮ LIỆU
        // ══════════════════════════════════════════════════════════════════════

        private void LoadDuLieu()
        {
            LoadKPI();
            LoadDanhSach();
        }

        private void LoadKPI()
        {
            try
            {
                DateTime ngay = dtpNgayLoc?.Value.Date ?? DateTime.Today;

                // KPI 1: Đã thanh toán (trong ngày đang chọn)
                object val1 = DatabaseConnection.ExecuteScalar(@"
                    SELECT COUNT(*) FROM HoaDon
                    WHERE TrangThai = 1 AND IsDeleted = 0
                      AND CAST(ISNULL(NgayThanhToan, NgayTao) AS DATE) = @Ngay",
                    p => p.AddWithValue("@Ngay", ngay));
                if (lblKpiDaTT != null)
                    lblKpiDaTT.Text = val1?.ToString() ?? "0";

                // KPI 2: Chưa thanh toán (trong ngày đang chọn)
                object val2 = DatabaseConnection.ExecuteScalar(@"
                    SELECT COUNT(*) FROM HoaDon
                    WHERE TrangThai = 0 AND IsDeleted = 0
                      AND CAST(NgayTao AS DATE) = @Ngay",
                    p => p.AddWithValue("@Ngay", ngay));
                if (lblKpiChuaTT != null)
                    lblKpiChuaTT.Text = val2?.ToString() ?? "0";

                // KPI 3: Hóa đơn hôm nay (đã thanh toán hôm nay)
                object val3 = DatabaseConnection.ExecuteScalar(@"
                    SELECT COUNT(*) FROM HoaDon
                    WHERE TrangThai = 1 AND IsDeleted = 0
                      AND CAST(ISNULL(NgayThanhToan, NgayTao) AS DATE) = CAST(GETDATE() AS DATE)");
                if (lblKpiHomNay != null)
                    lblKpiHomNay.Text = val3?.ToString() ?? "0";

                // KPI 4: Hóa đơn tháng (đã thanh toán trong tháng hiện tại)
                object val4 = DatabaseConnection.ExecuteScalar(@"
                    SELECT COUNT(*) FROM HoaDon
                    WHERE TrangThai = 1 AND IsDeleted = 0
                      AND MONTH(ISNULL(NgayThanhToan, NgayTao)) = MONTH(GETDATE())
                      AND YEAR(ISNULL(NgayThanhToan, NgayTao)) = YEAR(GETDATE())");
                if (lblKpiThang != null)
                    lblKpiThang.Text = val4?.ToString() ?? "0";
            }
            catch
            {
                if (lblKpiDaTT != null) lblKpiDaTT.Text = "!";
                if (lblKpiChuaTT != null) lblKpiChuaTT.Text = "!";
                if (lblKpiHomNay != null) lblKpiHomNay.Text = "!";
                if (lblKpiThang != null) lblKpiThang.Text = "!";
            }
        }

        private void LoadDanhSach()
        {
            try
            {
                DateTime ngay = dtpNgayLoc?.Value.Date ?? DateTime.Today;
                int trangThaiIdx = cboTrangThai?.SelectedIndex ?? 0;

                string sql = @"
                    SELECT
                        hd.MaHoaDon,
                        N'HĐ' + RIGHT(N'0000' + CAST(hd.MaHoaDon AS NVARCHAR(10)), 4) AS MaHDCode,
                        ISNULL(bn.HoTen, N'—')                                         AS TenBenhNhan,
                        CASE
                            WHEN hd.NgayThanhToan IS NOT NULL
                            THEN FORMAT(hd.NgayThanhToan, N'HH:mm dd/MM/yyyy')
                            ELSE N'Chưa TT'
                        END                                                              AS NgayThanhToanText,
                        FORMAT(ISNULL(hd.TongTienDichVu, 0), N'#,##0') + N'đ'           AS TongDVText,
                        FORMAT(ISNULL(hd.TongThuoc, 0), N'#,##0') + N'đ'                AS TongThuocText,
                        FORMAT(ISNULL(hd.GiamGia, 0), N'#,##0') + N'đ'                  AS GiamGiaText,
                        FORMAT(ISNULL(hd.TongTien, 0), N'#,##0') + N'đ'                 AS TongTienText,
                        ISNULL(hd.PhuongThucThanhToan, N'—')                             AS PhuongThucThanhToan,
                        CASE hd.TrangThai
                            WHEN 1 THEN N'✅ Đã TT'
                            WHEN 0 THEN N'⏳ Chưa TT'
                            ELSE N'—'
                        END                                                              AS TrangThaiText,
                        hd.TrangThai,
                        hd.MaPhieuKham
                    FROM HoaDon hd
                    LEFT JOIN PhieuKham pk ON hd.MaPhieuKham = pk.MaPhieuKham
                    LEFT JOIN BenhNhan  bn ON pk.MaBenhNhan  = bn.MaBenhNhan
                    WHERE hd.IsDeleted = 0
                      AND CAST(ISNULL(hd.NgayThanhToan, hd.NgayTao) AS DATE) = @Ngay";

                // Filter trạng thái
                if (trangThaiIdx == 1)
                    sql += " AND hd.TrangThai = 1";
                else if (trangThaiIdx == 2)
                    sql += " AND hd.TrangThai = 0";

                sql += " ORDER BY hd.MaHoaDon DESC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@Ngay", ngay));

                if (dgvHoaDon != null)
                {
                    dgvHoaDon.DataSource = dt;
                    TomauTrangThai();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hóa đơn:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Tô màu badge trạng thái theo dòng.</summary>
        private void TomauTrangThai()
        {
            if (dgvHoaDon == null) return;

            foreach (DataGridViewRow row in dgvHoaDon.Rows)
            {
                if (row.IsNewRow) continue;

                var cell = row.Cells["colTrangThai"];
                string tt = cell.Value?.ToString() ?? "";

                if (tt.Contains("Đã TT"))
                {
                    cell.Style.BackColor = Color.FromArgb(220, 252, 231);
                    cell.Style.ForeColor = Color.FromArgb(21, 101, 52);
                }
                else if (tt.Contains("Chưa TT"))
                {
                    cell.Style.BackColor = Color.FromArgb(254, 243, 199);
                    cell.Style.ForeColor = Color.FromArgb(146, 64, 14);
                }
                cell.Style.Font = AppFonts.Badge;
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cell.Style.SelectionBackColor = cell.Style.BackColor;
                cell.Style.SelectionForeColor = cell.Style.ForeColor;
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // CHI TIẾT HÓA ĐƠN — Dialog đẹp mắt, tích hợp Sửa/Xóa bên trong
        // ══════════════════════════════════════════════════════════════════════

        private void MoChiTietHoaDon(int maHD)
        {
            if (maHD <= 0) return;

            try
            {
                // ── Load dữ liệu hóa đơn ──
                DataTable dtHD = DatabaseConnection.ExecuteQuery(@"
                    SELECT
                        hd.MaHoaDon, hd.TrangThai,
                        ISNULL(bn.HoTen, N'—')                             AS TenBN,
                        ISNULL(bn.SoDienThoai, N'—')                       AS SoDienThoai,
                        ISNULL(nd.HoTen, N'—')                             AS TenBacSi,
                        FORMAT(pk.NgayKham, N'dd/MM/yyyy HH:mm')            AS NgayKham,
                        ISNULL(pk.ChanDoan, N'—')                           AS ChanDoan,
                        ISNULL(hd.TongTienDichVu, 0)                        AS TongDV,
                        ISNULL(hd.TongThuoc, 0)                             AS TongThuoc,
                        ISNULL(hd.GiamGia, 0)                               AS GiamGia,
                        ISNULL(hd.TongTien, 0)                              AS TongTien,
                        ISNULL(hd.PhuongThucThanhToan, N'—')                AS PhuongThuc,
                        ISNULL(hd.TienKhachTra, 0)                          AS TienKhach,
                        ISNULL(hd.TienThua, 0)                              AS TienThua,
                        CASE WHEN hd.NgayThanhToan IS NOT NULL
                             THEN FORMAT(hd.NgayThanhToan, N'dd/MM/yyyy HH:mm')
                             ELSE N'—' END                                   AS NgayTT,
                        hd.MaPhieuKham
                    FROM HoaDon hd
                    LEFT JOIN PhieuKham pk ON hd.MaPhieuKham = pk.MaPhieuKham
                    LEFT JOIN BenhNhan  bn ON pk.MaBenhNhan  = bn.MaBenhNhan
                    LEFT JOIN NguoiDung nd ON pk.MaNguoiDung = nd.MaNguoiDung
                    WHERE hd.MaHoaDon = @MaHD AND hd.IsDeleted = 0",
                    p => p.AddWithValue("@MaHD", maHD));

                if (dtHD == null || dtHD.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRow r = dtHD.Rows[0];
                int trangThai = Convert.ToInt32(r["TrangThai"]);
                bool daTT = trangThai == 1;
                decimal tongDV = Convert.ToDecimal(r["TongDV"]);
                decimal tongThuoc = Convert.ToDecimal(r["TongThuoc"]);
                decimal giamGia = Convert.ToDecimal(r["GiamGia"]);
                decimal tongTien = Convert.ToDecimal(r["TongTien"]);
                int maPK = r["MaPhieuKham"] != DBNull.Value ? Convert.ToInt32(r["MaPhieuKham"]) : 0;

                // ── Load chi tiết dịch vụ ──
                DataTable dtDV = null;
                DataTable dtThuoc = null;
                if (maPK > 0)
                {
                    dtDV = DatabaseConnection.ExecuteQuery(@"
                        SELECT dv.TenDichVu,  ctdv.SoLuong,
                               FORMAT(dv.DonGia, N'#,##0') + N'đ' AS DonGia,
                               FORMAT(ctdv.ThanhTien, N'#,##0') + N'đ' AS ThanhTien
                        FROM ChiTietDichVu ctdv
                        JOIN DichVu dv ON ctdv.MaDichVu = dv.MaDichVu
                        WHERE ctdv.MaPhieuKham = @MaPK",
                        p => p.AddWithValue("@MaPK", maPK));

                    dtThuoc = DatabaseConnection.ExecuteQuery(@"
                        SELECT t.TenThuoc, cdt.SoLuong,
                               FORMAT(t.DonGia, N'#,##0') + N'đ' AS DonGia,
                               FORMAT(cdt.SoLuong * t.DonGia, N'#,##0') + N'đ' AS ThanhTien
                        FROM ChiTietDonThuoc cdt
                        JOIN Thuoc t ON cdt.MaThuoc = t.MaThuoc
                        WHERE cdt.MaPhieuKham = @MaPK",
                        p => p.AddWithValue("@MaPK", maPK));
                }

                // ══════════════════════════════════════════════════════════════
                // TẠO DIALOG CHI TIẾT
                // ══════════════════════════════════════════════════════════════

                using (var dlg = new Form
                {
                    Text = $"Chi tiết hóa đơn #HĐ{maHD:D4}",
                    Size = new Size(780, 740),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    BackColor = ColorScheme.Background,
                    Font = new Font("Segoe UI", 9.5f),
                })
                {
                    var pnlScroll = new Panel
                    {
                        Dock = DockStyle.Fill,
                        AutoScroll = true,
                    };
                    dlg.Controls.Add(pnlScroll);

                    int margin = 28;
                    int scrollBarW = SystemInformation.VerticalScrollBarWidth;
                    int wInner = dlg.ClientSize.Width - margin * 2 - scrollBarW;
                    int yy = margin;

                    // ── Header gradient ──
                    var pnlHead = new Panel
                    {
                        Location = new Point(margin, yy),
                        Size = new Size(wInner, 64),
                    };
                    pnlHead.Paint += (s2, pe) =>
                    {
                        var g = pe.Graphics;
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        using (var br = new LinearGradientBrush(
                            new Rectangle(0, 0, pnlHead.Width, pnlHead.Height),
                            ColorScheme.PrimaryDark, Color.FromArgb(180, GoldAccent.R, GoldAccent.G, GoldAccent.B),
                            LinearGradientMode.Horizontal))
                        using (var path = TaoRoundedRect(new Rectangle(0, 0, pnlHead.Width - 1, pnlHead.Height - 1), 12))
                            g.FillPath(br, path);
                    };
                    pnlHead.Controls.Add(new Label
                    {
                        Text = $"🧾  Hóa đơn #HĐ{maHD:D4}",
                        Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                        ForeColor = Color.White,
                        Location = new Point(20, 6),
                        AutoSize = true,
                        BackColor = Color.Transparent,
                    });
                    string ttText = daTT ? "✅ Đã thanh toán" : "⏳ Chưa thanh toán";
                    Color ttBg = daTT ? Color.FromArgb(220, 252, 231) : Color.FromArgb(254, 243, 199);
                    Color ttFg = daTT ? Color.FromArgb(21, 101, 52) : Color.FromArgb(146, 64, 14);
                    var lblTT = new Label
                    {
                        Text = "  " + ttText + "  ",
                        Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                        ForeColor = ttFg,
                        BackColor = ttBg,
                        AutoSize = true,
                        Location = new Point(20, 36),
                    };
                    pnlHead.Controls.Add(lblTT);
                    pnlScroll.Controls.Add(pnlHead);
                    yy += 64 + 18;

                    // ── Thông tin bệnh nhân & bác sĩ (2-column clean layout) ──
                    int infoH = 140;
                    var pnlInfo = TaoPanelCard(margin, yy, wInner, infoH, "👤  Thông tin khám");
                    int leftX = 20, labelW = 95;
                    int rightX = wInner / 2 + 14;
                    int row1 = 40, row2 = 66, row3 = 92;

                    ThemDongInfo2(pnlInfo, "Bệnh nhân:", r["TenBN"].ToString(), leftX, labelW, row1);
                    ThemDongInfo2(pnlInfo, "SĐT:", r["SoDienThoai"].ToString(), leftX, labelW, row2);
                    ThemDongInfo2(pnlInfo, "Chẩn đoán:", r["ChanDoan"].ToString(), leftX, labelW, row3);

                    ThemDongInfo2(pnlInfo, "Bác sĩ:", r["TenBacSi"].ToString(), rightX, labelW, row1);
                    ThemDongInfo2(pnlInfo, "Ngày khám:", r["NgayKham"]?.ToString() ?? "—", rightX, labelW, row2);

                    pnlScroll.Controls.Add(pnlInfo);
                    yy += infoH + 14;

                    // ── Dịch vụ ──
                    if (dtDV != null && dtDV.Rows.Count > 0)
                    {
                        int dvH = Math.Min(40 + dtDV.Rows.Count * 28 + 10, 200);
                        var pnlDV = TaoPanelCard(margin, yy, wInner, dvH, $"✨  Dịch vụ ({dtDV.Rows.Count})");
                        var dgvDV = TaoMiniGrid(pnlDV, dtDV, new[] { "TenDichVu", "SoLuong", "DonGia", "ThanhTien" },
                            new[] { "Dịch vụ", "SL", "Đơn giá", "Thành tiền" }, new[] { 45F, 10F, 20F, 25F });
                        pnlScroll.Controls.Add(pnlDV);
                        yy += dvH + 12;
                    }

                    // ── Thuốc ──
                    if (dtThuoc != null && dtThuoc.Rows.Count > 0)
                    {
                        int thuocH = Math.Min(40 + dtThuoc.Rows.Count * 28 + 10, 200);
                        var pnlThuoc = TaoPanelCard(margin, yy, wInner, thuocH, $"💊  Thuốc ({dtThuoc.Rows.Count})");
                        var dgvT = TaoMiniGrid(pnlThuoc, dtThuoc, new[] { "TenThuoc", "SoLuong", "DonGia", "ThanhTien" },
                            new[] { "Thuốc", "SL", "Đơn giá", "Thành tiền" }, new[] { 45F, 10F, 20F, 25F });
                        pnlScroll.Controls.Add(pnlThuoc);
                        yy += thuocH + 12;
                    }

                    // ── Tổng kết tài chính ──
                    int tkInitH = daTT ? 260 : 210;
                    var pnlTK = TaoPanelCard(margin, yy, wInner, tkInitH, "💰  Tổng kết");
                    int tkY = 42;
                    int tkLabelX = 20, tkValueX = 140;

                    // Giảm giá — editable nếu chưa TT
                    TextBox txtGiamGia = null;
                    ComboBox cboPhuongThuc = null;

                    ThemDongTien(pnlTK, "Dịch vụ:", tongDV, tkLabelX, ref tkY, ColorScheme.TextDark);
                    ThemDongTien(pnlTK, "Thuốc:", tongThuoc, tkLabelX, ref tkY, ColorScheme.TextDark);

                    if (!daTT)
                    {
                        pnlTK.Controls.Add(new Label
                        {
                            Text = "Giảm giá:",
                            Font = AppFonts.Body,
                            ForeColor = ColorScheme.TextGray,
                            Location = new Point(tkLabelX, tkY + 2),
                            AutoSize = true,
                            BackColor = Color.Transparent,
                        });
                        txtGiamGia = new TextBox
                        {
                            Text = giamGia > 0 ? giamGia.ToString("N0") : "0",
                            Font = AppFonts.Body,
                            Location = new Point(tkValueX, tkY - 1),
                            Width = 180,
                            BorderStyle = BorderStyle.FixedSingle,
                        };
                        pnlTK.Controls.Add(txtGiamGia);
                        tkY += 32;
                    }
                    else
                    {
                        ThemDongTien(pnlTK, "Giảm giá:", giamGia, tkLabelX, ref tkY, ColorScheme.Danger);
                    }

                    // Separator
                    pnlTK.Controls.Add(new Panel
                    {
                        Location = new Point(tkLabelX, tkY),
                        Size = new Size(wInner - 36, 1),
                        BackColor = Color.FromArgb(226, 237, 232),
                    });
                    tkY += 8;

                    // Label tổng tiền
                    var lblTong = new Label
                    {
                        Text = $"Tổng tiền:    {tongTien:#,##0}đ",
                        Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                        ForeColor = ColorScheme.PrimaryDark,
                        Location = new Point(tkLabelX, tkY),
                        AutoSize = true,
                        BackColor = Color.Transparent,
                    };
                    pnlTK.Controls.Add(lblTong);
                    tkY += 38;

                    if (txtGiamGia != null)
                    {
                        txtGiamGia.TextChanged += (s2, e2) =>
                        {
                            decimal gg = TinhGiamGiaRaw(txtGiamGia.Text, tongDV + tongThuoc);
                            decimal t = Math.Max(0, tongDV + tongThuoc - gg);
                            lblTong.Text = $"Tổng tiền:    {t:#,##0}đ";
                        };
                    }

                    if (daTT)
                    {
                        ThemDongTien(pnlTK, "Phương thức:", 0, tkLabelX, ref tkY, ColorScheme.TextDark,
                            r["PhuongThuc"].ToString());
                        ThemDongTien(pnlTK, "Khách trả:", Convert.ToDecimal(r["TienKhach"]), tkLabelX, ref tkY, ColorScheme.TextDark);
                        ThemDongTien(pnlTK, "Tiền thừa:", Convert.ToDecimal(r["TienThua"]), tkLabelX, ref tkY, ColorScheme.Success);
                        ThemDongTien(pnlTK, "Ngày TT:", 0, tkLabelX, ref tkY, ColorScheme.TextGray,
                            r["NgayTT"]?.ToString() ?? "—");
                    }
                    else
                    {
                        pnlTK.Controls.Add(new Label
                        {
                            Text = "Phương thức:",
                            Font = AppFonts.Body,
                            ForeColor = ColorScheme.TextGray,
                            Location = new Point(tkLabelX, tkY + 2),
                            AutoSize = true,
                            BackColor = Color.Transparent,
                        });
                        cboPhuongThuc = new ComboBox
                        {
                            Font = AppFonts.Body,
                            Location = new Point(tkValueX, tkY - 1),
                            Width = 180,
                            DropDownStyle = ComboBoxStyle.DropDownList,
                        };
                        cboPhuongThuc.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "Thẻ" });
                        string ptCu = r["PhuongThuc"]?.ToString() ?? "";
                        cboPhuongThuc.SelectedItem = ptCu;
                        if (cboPhuongThuc.SelectedIndex < 0) cboPhuongThuc.SelectedIndex = 0;
                        pnlTK.Controls.Add(cboPhuongThuc);
                        tkY += 32;
                    }

                    pnlTK.Height = tkY + 14;
                    pnlScroll.Controls.Add(pnlTK);
                    yy += pnlTK.Height + 20;

                    // ── Buttons — centered ──
                    // Calculate total buttons width first to center them
                    const int BTN_GAP = 16;
                    int totalBtnW = 0;
                    if (!daTT) totalBtnW = 190 + BTN_GAP + 180 + BTN_GAP;
                    totalBtnW += 130;
                    int btnStartX = margin + (wInner - totalBtnW) / 2;

                    var pnlButtons = new Panel
                    {
                        Location = new Point(0, yy),
                        Size = new Size(dlg.ClientSize.Width, 50),
                        BackColor = Color.Transparent,
                    };

                    int bx = btnStartX;

                    if (!daTT)
                    {
                        var btnLuu = new Guna2Button
                        {
                            Text = "💾  Lưu thay đổi",
                            Font = AppFonts.BodyBold,
                            ForeColor = Color.White,
                            FillColor = ColorScheme.PrimaryDark,
                            BorderRadius = 12,
                            Size = new Size(190, 44),
                            Location = new Point(bx, 0),
                            Cursor = Cursors.Hand,
                        };
                        var txtGGRef = txtGiamGia;
                        var cboPTRef = cboPhuongThuc;
                        btnLuu.Click += (s2, e2) =>
                        {
                            decimal gg = TinhGiamGiaRaw(txtGGRef?.Text ?? "0", tongDV + tongThuoc);
                            decimal tMoi = Math.Max(0, tongDV + tongThuoc - gg);
                            string ptMoi = cboPTRef?.SelectedItem?.ToString() ?? "Tiền mặt";

                            int rows = DatabaseConnection.ExecuteNonQuery(@"
                                UPDATE HoaDon SET
                                    GiamGia = @GiamGia, TongTien = @TongTien,
                                    PhuongThucThanhToan = @PhuongThuc
                                WHERE MaHoaDon = @MaHD AND TrangThai = 0",
                                p =>
                                {
                                    p.AddWithValue("@GiamGia", gg);
                                    p.AddWithValue("@TongTien", tMoi);
                                    p.AddWithValue("@PhuongThuc", ptMoi);
                                    p.AddWithValue("@MaHD", maHD);
                                });

                            if (rows > 0)
                            {
                                MessageBox.Show("Đã cập nhật hóa đơn thành công! ✅",
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dlg.DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                MessageBox.Show("Không cập nhật được. Hóa đơn có thể đã được thanh toán.",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };
                        pnlButtons.Controls.Add(btnLuu);
                        bx += 190 + BTN_GAP;

                        var btnXoa = new Guna2Button
                        {
                            Text = "🗑  Xóa hóa đơn",
                            Font = AppFonts.BodyBold,
                            ForeColor = Color.White,
                            FillColor = ColorScheme.Danger,
                            BorderRadius = 12,
                            Size = new Size(180, 44),
                            Location = new Point(bx, 0),
                            Cursor = Cursors.Hand,
                        };
                        btnXoa.Click += (s2, e2) =>
                        {
                            var confirm = MessageBox.Show(
                                $"Bạn có chắc muốn xóa hóa đơn #HĐ{maHD:D4}?\n\nHành động này sẽ xóa mềm.",
                                "Xác nhận xóa",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (confirm != DialogResult.Yes) return;

                            int rows = DatabaseConnection.ExecuteNonQuery(
                                "UPDATE HoaDon SET IsDeleted = 1 WHERE MaHoaDon = @MaHD AND TrangThai = 0",
                                p => p.AddWithValue("@MaHD", maHD));

                            if (rows > 0)
                            {
                                MessageBox.Show("Đã xóa hóa đơn! ✅", "Thành công",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dlg.DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                MessageBox.Show("Không xóa được. Hóa đơn có thể đã được thanh toán.",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };
                        pnlButtons.Controls.Add(btnXoa);
                        bx += 180 + BTN_GAP;
                    }

                    var btnDong = new Guna2Button
                    {
                        Text = "✕  Đóng",
                        Font = AppFonts.BodyBold,
                        ForeColor = ColorScheme.TextDark,
                        FillColor = Color.FromArgb(229, 231, 235),
                        BorderRadius = 12,
                        Size = new Size(130, 44),
                        Location = new Point(bx, 0),
                        Cursor = Cursors.Hand,
                    };
                    btnDong.Click += (s2, e2) => dlg.DialogResult = DialogResult.Cancel;
                    pnlButtons.Controls.Add(btnDong);

                    pnlScroll.Controls.Add(pnlButtons);

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                        LoadDuLieu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện nút danh sách → mở chi tiết
        private void BtnXem_Click(object sender, EventArgs e)
        {
            MoChiTietHoaDon(LayMaHoaDonDangChon());
        }

        // Nút Sửa ngoài danh sách → mở chi tiết (sửa bên trong)
        private void BtnSua_Click(object sender, EventArgs e)
        {
            int maHD = LayMaHoaDonDangChon();
            if (maHD <= 0) return;

            // Kiểm tra trạng thái trước khi mở
            int trangThai = LayTrangThaiHoaDon(maHD);
            if (trangThai == 1)
            {
                MessageBox.Show("Hóa đơn này đã thanh toán, không thể sửa.\nBạn chỉ có thể xem chi tiết.",
                    "Không thể sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MoChiTietHoaDon(maHD);
        }

        // Nút Xóa ngoài danh sách
        private void BtnXoa_Click(object sender, EventArgs e)
        {
            int maHD = LayMaHoaDonDangChon();
            if (maHD <= 0) return;

            int trangThai = LayTrangThaiHoaDon(maHD);
            if (trangThai == 1)
            {
                MessageBox.Show("Hóa đơn này đã thanh toán, không thể xóa.",
                    "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa hóa đơn #HĐ{maHD:D4}?\n\nHành động này sẽ xóa mềm (có thể khôi phục).",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                int rows = DatabaseConnection.ExecuteNonQuery(
                    "UPDATE HoaDon SET IsDeleted = 1 WHERE MaHoaDon = @MaHD AND TrangThai = 0",
                    p => p.AddWithValue("@MaHD", maHD));

                if (rows > 0)
                {
                    MessageBox.Show("Đã xóa hóa đơn thành công! ✅",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDuLieu();
                }
                else
                {
                    MessageBox.Show("Không xóa được. Hóa đơn có thể đã được thanh toán.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa hóa đơn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // HELPERS
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Lấy MaHoaDon từ dòng đang chọn trong DataGridView.</summary>
        private int LayMaHoaDonDangChon()
        {
            if (dgvHoaDon == null || dgvHoaDon.CurrentRow == null || dgvHoaDon.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn trong danh sách.",
                    "Chưa chọn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }

            var val = dgvHoaDon.CurrentRow.Cells["colMaHD"].Value;
            if (val == null || !int.TryParse(val.ToString(), out int maHD) || maHD <= 0)
            {
                MessageBox.Show("Không đọc được mã hóa đơn.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            return maHD;
        }

        /// <summary>Lấy trạng thái hóa đơn (0=Chưa TT, 1=Đã TT).</summary>
        private int LayTrangThaiHoaDon(int maHD)
        {
            try
            {
                object val = DatabaseConnection.ExecuteScalar(
                    "SELECT TrangThai FROM HoaDon WHERE MaHoaDon = @MaHD AND IsDeleted = 0",
                    p => p.AddWithValue("@MaHD", maHD));
                return val != null ? Convert.ToInt32(val) : -1;
            }
            catch { return -1; }
        }

        /// <summary>Tính giảm giá từ text (hỗ trợ % và số tuyệt đối).</summary>
        private decimal TinhGiamGiaRaw(string raw, decimal tamTinh)
        {
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            raw = raw.Trim();
            if (raw.EndsWith("%"))
            {
                if (decimal.TryParse(raw.TrimEnd('%').Replace(",", "").Replace(".", ""), out decimal pt))
                    return tamTinh * Math.Min(pt, 100) / 100m;
                return 0;
            }
            decimal.TryParse(raw.Replace(",", "").Replace(".", ""), out decimal v);
            return v;
        }

        // ── UI Helpers cho dialog chi tiết ───────────────────────────────────

        private Panel TaoPanelCard(int x, int y, int w, int h, string title)
        {
            var pnl = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = Color.White,
            };
            pnl.Paint += (s, pe) =>
            {
                using (var pen = new Pen(ColorScheme.Border, 1f))
                    pe.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };
            pnl.Controls.Add(new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = ColorScheme.PrimaryDark,
                Location = new Point(14, 8),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            return pnl;
        }

        private void ThemDongInfo2(Panel pnl, string label, string value, int xStart, int labelW, int yPos)
        {
            pnl.Controls.Add(new Label
            {
                Text = label,
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextGray,
                Location = new Point(xStart, yPos),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            pnl.Controls.Add(new Label
            {
                Text = value,
                Font = AppFonts.BodyBold,
                ForeColor = ColorScheme.TextDark,
                Location = new Point(xStart + labelW, yPos),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
        }

        private void ThemDongTien(Panel pnl, string label, decimal value, int xStart, ref int y,
            Color valueColor, string textOverride = null)
        {
            pnl.Controls.Add(new Label
            {
                Text = label,
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextGray,
                Location = new Point(xStart, y + 2),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            pnl.Controls.Add(new Label
            {
                Text = textOverride ?? (value.ToString("#,##0") + "đ"),
                Font = AppFonts.BodyBold,
                ForeColor = valueColor,
                Location = new Point(140, y + 2),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            y += 28;
        }

        private DataGridView TaoMiniGrid(Panel parent, DataTable dt,
            string[] dataProps, string[] headers, float[] weights)
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersHeight = 30,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersVisible = true,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(226, 237, 232),
                ReadOnly = true,
                RowHeadersVisible = false,
                RowTemplate = { Height = 26 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = AppFonts.Small,
                AutoGenerateColumns = false,
            };
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(240, 250, 245),
                ForeColor = ColorScheme.PrimaryDark,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                SelectionBackColor = Color.FromArgb(240, 250, 245),
                SelectionForeColor = ColorScheme.PrimaryDark,
            };
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ColorScheme.TextDark,
                SelectionBackColor = Color.FromArgb(221, 245, 229),
                SelectionForeColor = ColorScheme.PrimaryDark,
            };

            for (int i = 0; i < dataProps.Length; i++)
            {
                dgv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = dataProps[i],
                    HeaderText = headers[i],
                    FillWeight = weights[i],
                });
            }

            // Thêm title (panel) lên trên rồi dgv fill phần còn lại
            // Title đã được add bởi TaoPanelCard, chỉ cần add dgv
            // Nhưng TaoPanelCard không có Dock controls — dùng manual layout
            dgv.Location = new Point(1, 34);
            dgv.Size = new Size(parent.Width - 2, parent.Height - 35);
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            dgv.DataSource = dt;
            parent.Controls.Add(dgv);

            return dgv;
        }

        // ── Layout Helpers ───────────────────────────────────────────────────

        private Panel TaoCard(int x, int y, int w, int h)
        {
            var pnl = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = Color.White,
            };
            pnl.Paint += (s, pe) =>
            {
                using (var pen = new Pen(ColorScheme.Border, 1f))
                    pe.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
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
