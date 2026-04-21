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
    /// Form Bảng Lương — Admin quản lý lương nhân viên theo tháng.
    /// Chức năng: Chọn tháng → Tính lương tự động → Thêm thưởng/khấu trừ → Duyệt → Thanh toán
    /// 
    /// Quy tắc tính:
    ///   Bác Sĩ (THEO_BN):  SoBenhNhan × DonGia + SoBNTangCa × DonGia × HeSoTangCa
    ///   Lễ Tân (THEO_GIO): SoGioLam   × DonGia + SoGioTangCa × DonGia × HeSoTangCa
    ///   Admin (THEO_THANG): DonGia (cố định)
    /// </summary>
    public partial class BangLuongForm : Form
    {
        // ── Controls ──────────────────────────────────────────────────────────
        private Panel pnlContent;
        private Guna2DateTimePicker dtpThangNam;
        private Guna2Button btnTinhLuong;
        private Guna2Button btnDuyet;
        private Guna2Button btnThanhToan;
        private Guna2Button btnXuatPhieu;
        private Guna2Button btnHoanTacDuyet;
        private Guna2Button btnLichSuTT;
        private DataGridView dgvBangLuong;

        // ── KPI labels ────────────────────────────────────────────────────────
        private Label lblKpiTongNV;
        private Label lblKpiTongLuong;
        private Label lblKpiDaDuyet;
        private Label lblKpiDaTT;

        // ── Màu đồng bộ ──────────────────────────────────────────────────────
        private static readonly Color GoldAccent = Color.FromArgb(184, 138, 40);
        private static readonly Color BorderInput = ColorTranslator.FromHtml("#D1E8DC");

        private bool _dangVeLai = false;
        private bool _dangXuLy = false;  // Guard chặn VeLaiForm khi đang xử lý nghiệp vụ
        private DateTime _savedThang = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        private System.Windows.Forms.Timer _timerAutoUpdate;

        public BangLuongForm()
        {
            InitializeComponent();
            TaoBoCuc();
            KhoiTaoAutoUpdate();
        }

        /// <summary>
        /// Timer tự động cập nhật bảng lương nháp mỗi 5 phút.
        /// Chỉ cập nhật tháng hiện tại + TrangThai = 0.
        /// </summary>
        private void KhoiTaoAutoUpdate()
        {
            _timerAutoUpdate = new System.Windows.Forms.Timer { Interval = 5 * 60 * 1000 }; // 5 phút
            _timerAutoUpdate.Tick += (s, e) => CapNhatLuongNhapTuDong(false);
            _timerAutoUpdate.Start();
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
            if (_dangVeLai || _dangXuLy) return;
            _dangVeLai = true;

            if (dtpThangNam != null) _savedThang = new DateTime(dtpThangNam.Value.Year, dtpThangNam.Value.Month, 1);

            // Reset field references trước khi clear controls
            // để tránh LoadDuLieu ghi data lên control đã bị disposed
            dtpThangNam = null;
            dgvBangLuong = null;
            lblKpiTongNV = null;
            lblKpiTongLuong = null;
            lblKpiDaDuyet = null;
            lblKpiDaTT = null;

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();

            int pad = 16;
            int contentW = pnlContent.ClientSize.Width - pad * 2;
            int y = pad;

            y = TaoHeader(pad, y, contentW);
            y = TaoToolBar(pad, y, contentW);
            y = TaoKpiCards(pad, y, contentW);

            int remainH = Math.Max(300, pnlContent.ClientSize.Height - y - pad);
            TaoDanhSach(pad, y, contentW, remainH);

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
                using (var b = new SolidBrush(GoldAccent))
                    g.FillEllipse(b, 0, 7, 30, 30);
                TextRenderer.DrawText(g, "💰", new Font("Segoe UI Emoji", 12f), new Point(4, 11), Color.White);
                TextRenderer.DrawText(g, "BẢNG LƯƠNG NHÂN VIÊN", AppFonts.H3, new Point(38, 10), ColorScheme.TextDark);
            };
            pnlContent.Controls.Add(pnlHeader);
            return y + 50;
        }

        // ══════════════════════════════════════════════════════════════════════
        // TOOLBAR — Chọn tháng + Nút hành động
        // ══════════════════════════════════════════════════════════════════════

        private int TaoToolBar(int x, int y, int w)
        {
            int toolbarH = 64;
            var pnlToolbar = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, toolbarH),
                BackColor = Color.White,
            };
            pnlToolbar.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(BorderInput))
                using (var path = TaoRoundedRect(0, 0, pnlToolbar.Width - 1, pnlToolbar.Height - 1, 8))
                    g.DrawPath(pen, path);
            };

            int btnH = 44;
            int btnY = (toolbarH - btnH) / 2;

            // ── Chọn tháng ──
            var lblThang = new Label
            {
                Text = "Tháng:",
                Font = AppFonts.BodyBold,
                ForeColor = ColorScheme.TextDark,
                AutoSize = true,
                Location = new Point(16, (toolbarH - 16) / 2),
            };
            pnlToolbar.Controls.Add(lblThang);

            dtpThangNam = new Guna2DateTimePicker
            {
                Location = new Point(76, btnY),
                Size = new Size(170, btnH),
                CustomFormat = "MM/yyyy",
                Format = DateTimePickerFormat.Custom,
                Font = AppFonts.Body,
                Value = _savedThang,
                FillColor = Color.White,
                BorderColor = BorderInput,
                BorderRadius = 8,
            };
            dtpThangNam.ValueChanged += (s, e) =>
            {
                if (!_dangVeLai) LoadDuLieu();
            };
            pnlToolbar.Controls.Add(dtpThangNam);

            // ── 6 nút — căn phải, dàn đều ──
            int gap = 6;
            int rightEdge = w - 16;
            int totalGap = gap * 5;
            int availW = rightEdge - 260; // trừ phần chọn tháng bên trái
            int btnW = Math.Max(120, (availW - totalGap) / 6);

            btnXuatPhieu = TaoNut("🖨️ Xuất phiếu", rightEdge - btnW, btnY, btnW, btnH, ColorScheme.Danger);
            btnXuatPhieu.Click += BtnXuatPhieu_Click;
            pnlToolbar.Controls.Add(btnXuatPhieu);

            btnLichSuTT = TaoNut("📋 Lịch sử TT", rightEdge - btnW * 2 - gap, btnY, btnW, btnH, Color.FromArgb(107, 114, 128));
            btnLichSuTT.Click += BtnLichSuTT_Click;
            pnlToolbar.Controls.Add(btnLichSuTT);

            btnThanhToan = TaoNut("💳 Thanh toán", rightEdge - btnW * 3 - gap * 2, btnY, btnW, btnH, GoldAccent);
            btnThanhToan.Click += BtnThanhToan_Click;
            pnlToolbar.Controls.Add(btnThanhToan);

            btnHoanTacDuyet = TaoNut("↩️ Hoàn tác duyệt", rightEdge - btnW * 4 - gap * 3, btnY, btnW, btnH, ColorScheme.Warning);
            btnHoanTacDuyet.Click += BtnHoanTacDuyet_Click;
            pnlToolbar.Controls.Add(btnHoanTacDuyet);

            btnDuyet = TaoNut("✅ Duyệt lương", rightEdge - btnW * 5 - gap * 4, btnY, btnW, btnH, ColorScheme.Info);
            btnDuyet.Click += BtnDuyet_Click;
            pnlToolbar.Controls.Add(btnDuyet);

            btnTinhLuong = TaoNut("📊 Tính lương", rightEdge - btnW * 6 - gap * 5, btnY, btnW, btnH, ColorScheme.Primary);
            btnTinhLuong.Click += BtnTinhLuong_Click;
            pnlToolbar.Controls.Add(btnTinhLuong);

            pnlContent.Controls.Add(pnlToolbar);
            return y + toolbarH + 8;
        }

        private Guna2Button TaoNut(string text, int x, int y, int w, int h, Color bgColor)
        {
            return new Guna2Button
            {
                Text = text,
                Font = AppFonts.BodyBold,
                ForeColor = Color.White,
                FillColor = bgColor,
                BorderRadius = 8,
                Size = new Size(w, h),
                Location = new Point(x, y),
                Cursor = Cursors.Hand,
            };
        }

        private static GraphicsPath TaoRoundedRect(int x, int y, int w, int h, int r)
        {
            var path = new GraphicsPath();
            path.AddArc(x, y, r * 2, r * 2, 180, 90);
            path.AddArc(x + w - r * 2, y, r * 2, r * 2, 270, 90);
            path.AddArc(x + w - r * 2, y + h - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(x, y + h - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        // ══════════════════════════════════════════════════════════════════════
        // KPI CARDS — 4 thẻ tổng hợp
        // ══════════════════════════════════════════════════════════════════════

        private int TaoKpiCards(int x, int y, int w)
        {
            int cardW = (w - 12 * 3) / 4;
            int cardH = 80;

            string[] titles = { "Nhân viên", "Tổng lương", "Đã duyệt", "Đã thanh toán" };
            string[] icons = { "👥", "💰", "✅", "💳" };
            Color[] colors = { ColorScheme.Primary, GoldAccent, ColorScheme.Info, ColorScheme.Success };

            Label[] kpiLabels = new Label[4];

            for (int i = 0; i < 4; i++)
            {
                int cx = x + i * (cardW + 12);
                var card = new Panel
                {
                    Location = new Point(cx, y),
                    Size = new Size(cardW, cardH),
                    BackColor = Color.White,
                };
                int idx = i;
                card.Paint += (s, e) =>
                {
                    var g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var path = TaoRoundedRect(0, 0, card.Width - 1, card.Height - 1, 10))
                    using (var pen = new Pen(BorderInput))
                        g.DrawPath(pen, path);
                    using (var b = new SolidBrush(Color.FromArgb(30, colors[idx])))
                        g.FillEllipse(b, 12, 16, 40, 40);
                    TextRenderer.DrawText(g, icons[idx], new Font("Segoe UI Emoji", 14f), new Point(17, 21), colors[idx]);
                };

                var lblTitle = new Label
                {
                    Text = titles[i],
                    Font = AppFonts.Small,
                    ForeColor = ColorScheme.TextGray,
                    Location = new Point(60, 14),
                    AutoSize = true,
                };
                card.Controls.Add(lblTitle);

                var lblValue = new Label
                {
                    Text = "0",
                    Font = AppFonts.H4,
                    ForeColor = colors[i],
                    Location = new Point(60, 38),
                    AutoSize = true,
                };
                card.Controls.Add(lblValue);
                kpiLabels[i] = lblValue;

                pnlContent.Controls.Add(card);
            }

            lblKpiTongNV = kpiLabels[0];
            lblKpiTongLuong = kpiLabels[1];
            lblKpiDaDuyet = kpiLabels[2];
            lblKpiDaTT = kpiLabels[3];

            return y + cardH + 12;
        }

        // ══════════════════════════════════════════════════════════════════════
        // DANH SÁCH BẢNG LƯƠNG
        // ══════════════════════════════════════════════════════════════════════

        private void TaoDanhSach(int x, int y, int w, int h)
        {
            var card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = Color.White,
            };
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = TaoRoundedRect(0, 0, card.Width - 1, card.Height - 1, 10))
                using (var pen = new Pen(BorderInput))
                    g.DrawPath(pen, path);
            };

            dgvBangLuong = new DataGridView
            {
                Location = new Point(1, 1),
                Size = new Size(w - 2, h - 2),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = ColorScheme.Border,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = AppFonts.Body,
                    ForeColor = ColorScheme.TextDark,
                    SelectionBackColor = ColorScheme.PrimaryPale,
                    SelectionForeColor = ColorScheme.TextDark,
                    Padding = new Padding(6, 4, 6, 4),
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = AppFonts.BodyBold,
                    ForeColor = ColorScheme.TextMid,
                    BackColor = Color.FromArgb(249, 250, 251),
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Padding = new Padding(6, 0, 6, 0),
                },
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 38 },
                EnableHeadersVisualStyles = false,
            };
            dgvBangLuong.AutoGenerateColumns = false;

            // Cột ẩn
            dgvBangLuong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMaBL", DataPropertyName = "MaBangLuong", Visible = false });
            dgvBangLuong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTrangThaiGoc", DataPropertyName = "TrangThai", Visible = false });

            // Cột hiển thị
            dgvBangLuong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHoTen", DataPropertyName = "HoTen", HeaderText = "Nhân viên", FillWeight = 16F });
            dgvBangLuong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colVaiTro", DataPropertyName = "TenVaiTro", HeaderText = "Vai trò", FillWeight = 10F });
            dgvBangLuong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLoai", DataPropertyName = "LoaiTinhLuong", HeaderText = "Loại", FillWeight = 10F });
            dgvBangLuong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSoLieu", DataPropertyName = "SoLieuText", HeaderText = "Số liệu", FillWeight = 14F });
            dgvBangLuong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTangCa", DataPropertyName = "TangCaText", HeaderText = "Tăng ca", FillWeight = 14F });
            dgvBangLuong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLuongChinh", DataPropertyName = "LuongChinhText", HeaderText = "Lương chính", FillWeight = 12F });
            dgvBangLuong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLuongTC", DataPropertyName = "LuongTCText", HeaderText = "Lương TC", FillWeight = 10F });
            dgvBangLuong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colThuong", DataPropertyName = "ThuongText", HeaderText = "Thưởng", FillWeight = 10F });
            dgvBangLuong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colKhauTru", DataPropertyName = "KhauTruText", HeaderText = "Khấu trừ", FillWeight = 10F });
            dgvBangLuong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTongLuong", DataPropertyName = "TongLuongText", HeaderText = "Tổng lương", FillWeight = 14F });
            dgvBangLuong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTrangThaiHien", DataPropertyName = "TrangThaiText", HeaderText = "Trạng thái", FillWeight = 10F });

            dgvBangLuong.CellDoubleClick += DgvBangLuong_CellDoubleClick;

            card.Controls.Add(dgvBangLuong);
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

        private DateTime LayThangDangChon()
        {
            if (dtpThangNam == null) return _savedThang;
            return new DateTime(dtpThangNam.Value.Year, dtpThangNam.Value.Month, 1);
        }

        private void LoadKPI()
        {
            try
            {
                DateTime thang = LayThangDangChon();

                object val1 = DatabaseConnection.ExecuteScalar(@"
                    SELECT COUNT(*) FROM BangLuong WHERE ThangNam = @Thang",
                    p => p.AddWithValue("@Thang", thang));
                if (lblKpiTongNV != null) lblKpiTongNV.Text = val1?.ToString() ?? "0";

                object val2 = DatabaseConnection.ExecuteScalar(@"
                    SELECT ISNULL(SUM(TongLuong), 0) FROM BangLuong WHERE ThangNam = @Thang",
                    p => p.AddWithValue("@Thang", thang));
                decimal tongLuong = val2 != null && val2 != DBNull.Value ? Convert.ToDecimal(val2) : 0;
                if (lblKpiTongLuong != null) lblKpiTongLuong.Text = tongLuong.ToString("#,##0") + "đ";

                object val3 = DatabaseConnection.ExecuteScalar(@"
                    SELECT COUNT(*) FROM BangLuong WHERE ThangNam = @Thang AND TrangThai >= 1",
                    p => p.AddWithValue("@Thang", thang));
                if (lblKpiDaDuyet != null) lblKpiDaDuyet.Text = val3?.ToString() ?? "0";

                object val4 = DatabaseConnection.ExecuteScalar(@"
                    SELECT COUNT(*) FROM BangLuong WHERE ThangNam = @Thang AND TrangThai = 2",
                    p => p.AddWithValue("@Thang", thang));
                if (lblKpiDaTT != null) lblKpiDaTT.Text = val4?.ToString() ?? "0";
            }
            catch
            {
                if (lblKpiTongNV != null) lblKpiTongNV.Text = "!";
                if (lblKpiTongLuong != null) lblKpiTongLuong.Text = "!";
                if (lblKpiDaDuyet != null) lblKpiDaDuyet.Text = "!";
                if (lblKpiDaTT != null) lblKpiDaTT.Text = "!";
            }
        }

        private void LoadDanhSach()
        {
            try
            {
                DateTime thang = LayThangDangChon();

                string sql = @"
                    SELECT
                        bl.MaBangLuong,
                        nd.HoTen,
                        vt.TenVaiTro,
                        bl.LoaiTinhLuong,
                        CASE bl.LoaiTinhLuong
                            WHEN 'THEO_BN'    THEN CAST(bl.SoBenhNhan AS NVARCHAR) + N' BN'
                            WHEN 'THEO_GIO'   THEN FORMAT(bl.SoGioLam, N'#,##0.#') + N' giờ'
                            WHEN 'THEO_THANG' THEN N'Cố định'
                            ELSE N'—'
                        END AS SoLieuText,
                        CASE bl.LoaiTinhLuong
                            WHEN 'THEO_BN'    THEN CAST(bl.SoBNTangCa AS NVARCHAR) + N' BN (×' + FORMAT(bl.HeSoTangCa, N'0.#') + N')'
                            WHEN 'THEO_GIO'   THEN FORMAT(bl.SoGioTangCa, N'#,##0.#') + N' giờ (×' + FORMAT(bl.HeSoTangCa, N'0.#') + N')'
                            WHEN 'THEO_THANG' THEN N'—'
                            ELSE N'—'
                        END AS TangCaText,
                        FORMAT(bl.LuongChinh, N'#,##0') + N'đ'   AS LuongChinhText,
                        FORMAT(bl.LuongTangCa, N'#,##0') + N'đ'  AS LuongTCText,
                        FORMAT(bl.ThuongThem, N'#,##0') + N'đ'   AS ThuongText,
                        FORMAT(bl.KhauTru, N'#,##0') + N'đ'      AS KhauTruText,
                        FORMAT(bl.TongLuong, N'#,##0') + N'đ'    AS TongLuongText,
                        CASE bl.TrangThai
                            WHEN 0 THEN N'📝 Nháp'
                            WHEN 1 THEN N'✅ Đã duyệt'
                            WHEN 2 THEN N'💳 Đã TT'
                            ELSE N'—'
                        END AS TrangThaiText,
                        bl.TrangThai
                    FROM BangLuong bl
                    JOIN NguoiDung nd ON bl.MaNguoiDung = nd.MaNguoiDung
                    JOIN VaiTro vt ON bl.MaVaiTro = vt.MaVaiTro
                    WHERE bl.ThangNam = @Thang
                    ORDER BY vt.MaVaiTro, nd.HoTen";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@Thang", thang));

                if (dgvBangLuong != null)
                {
                    dgvBangLuong.DataSource = null;  // Force rebind — clear trước
                    dgvBangLuong.DataSource = dt;
                    TomauTrangThai();
                    dgvBangLuong.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bảng lương:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TomauTrangThai()
        {
            if (dgvBangLuong == null) return;
            foreach (DataGridViewRow row in dgvBangLuong.Rows)
            {
                if (row.IsNewRow) continue;
                var cell = row.Cells["colTrangThaiHien"];
                string tt = cell.Value?.ToString() ?? "";

                if (tt.Contains("Đã TT"))
                {
                    cell.Style.BackColor = Color.FromArgb(220, 252, 231);
                    cell.Style.ForeColor = Color.FromArgb(21, 101, 52);
                }
                else if (tt.Contains("Đã duyệt"))
                {
                    cell.Style.BackColor = Color.FromArgb(219, 234, 254);
                    cell.Style.ForeColor = Color.FromArgb(30, 64, 175);
                }
                else if (tt.Contains("Nháp"))
                {
                    cell.Style.BackColor = Color.FromArgb(254, 243, 199);
                    cell.Style.ForeColor = Color.FromArgb(146, 64, 14);
                }
                cell.Style.Font = AppFonts.Badge;
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cell.Style.SelectionBackColor = cell.Style.BackColor;
                cell.Style.SelectionForeColor = cell.Style.ForeColor;

                // Tô đậm cột tổng lương
                var cellTong = row.Cells["colTongLuong"];
                cellTong.Style.Font = AppFonts.BodyBold;
                cellTong.Style.ForeColor = GoldAccent;
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // TÍNH LƯƠNG TỰ ĐỘNG
        // ══════════════════════════════════════════════════════════════════════

        private void BtnTinhLuong_Click(object sender, EventArgs e)
        {
            DateTime thang = LayThangDangChon();

            // Kiểm tra đã tính chưa
            object exists = DatabaseConnection.ExecuteScalar(@"
                SELECT COUNT(*) FROM BangLuong WHERE ThangNam = @Thang",
                p => p.AddWithValue("@Thang", thang));

            int soHienCo = exists != null ? Convert.ToInt32(exists) : 0;

            bool forceTinhLai = false;

            if (soHienCo > 0)
            {
                // Đếm số bảng lương đã duyệt/đã TT và nháp
                object countDaDuyetObj = DatabaseConnection.ExecuteScalar(@"
                    SELECT COUNT(*) FROM BangLuong WHERE ThangNam = @Thang AND TrangThai >= 1",
                    p => p.AddWithValue("@Thang", thang));
                int soDaDuyet = countDaDuyetObj != null ? Convert.ToInt32(countDaDuyetObj) : 0;
                int soNhap = soHienCo - soDaDuyet;

                if (soDaDuyet > 0 && soNhap == 0)
                {
                    // Tất cả đã duyệt/TT → hỏi có muốn tính lại không
                    var confirm = MessageBox.Show(
                        $"Tháng {thang:MM/yyyy} đã có {soHienCo} bảng lương\n" +
                        $"({soDaDuyet} đã duyệt/thanh toán, không còn bảng lương Nháp).\n\n" +
                        "Bạn có muốn TÍNH LẠI TẤT CẢ?\n\n" +
                        "• Số liệu cũ đã được lưu trong Lịch sử thanh toán\n" +
                        "• Bảng lương sẽ được reset hoàn toàn về Nháp\n" +
                        "• Thưởng/Khấu trừ/Ghi chú cũ sẽ bị xóa\n" +
                        "• Số liệu mới sẽ được tính lại từ đầu",
                        "Xác nhận tính lại", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirm != DialogResult.Yes) return;
                    forceTinhLai = true;
                }
                else
                {
                    string msg = $"Tháng {thang:MM/yyyy} đã có bảng lương ({soHienCo} NV).\n\n";
                    msg += $"• Cập nhật lại {soNhap} bảng lương Nháp\n";
                    if (soDaDuyet > 0)
                        msg += $"• {soDaDuyet} bảng lương Đã duyệt/Đã TT — giữ nguyên\n";
                    msg += "\nTiếp tục?";

                    var confirm = MessageBox.Show(msg,
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirm != DialogResult.Yes) return;
                }
            }

            TinhVaCapNhatLuong(thang, true, forceTinhLai);
        }

        /// <summary>
        /// Tự động cập nhật bảng lương Nháp cho tháng hiện tại (gọi từ Timer).
        /// </summary>
        private void CapNhatLuongNhapTuDong(bool hienThongBao)
        {
            DateTime thangHienTai = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            // Chỉ auto-update nếu đang xem tháng hiện tại
            if (LayThangDangChon() != thangHienTai) return;

            // Chỉ update nếu đã có bảng lương nháp
            object countNhap = DatabaseConnection.ExecuteScalar(@"
                SELECT COUNT(*) FROM BangLuong WHERE ThangNam = @Thang AND TrangThai = 0",
                p => p.AddWithValue("@Thang", thangHienTai));

            int soNhap = countNhap != null ? Convert.ToInt32(countNhap) : 0;
            if (soNhap == 0) return;

            TinhVaCapNhatLuong(thangHienTai, hienThongBao, false);
        }

        /// <summary>
        /// Logic tính lương chính — INSERT NV mới + UPDATE NV đã có.
        /// forceTinhLai = true → tính lại cả bảng lương Đã duyệt/Đã TT (reset về Nháp).
        /// </summary>
        private void TinhVaCapNhatLuong(DateTime thang, bool hienThongBao, bool forceTinhLai)
        {
            DateTime dauThang = thang;
            DateTime cuoiThang = thang.AddMonths(1).AddSeconds(-1);
            _dangXuLy = true;

            try
            {
                // ── Bước 1: Lấy tất cả NV hoạt động ──
                DataTable dtNV = DatabaseConnection.ExecuteQuery(@"
                    SELECT nd.MaNguoiDung, nd.MaVaiTro
                    FROM NguoiDung nd
                    WHERE nd.IsDeleted = 0 AND nd.TrangThaiTK = 1",
                    p => p.AddWithValue("@Thang", thang));

                int countInsert = 0;
                int countUpdate = 0;
                int countReset = 0;

                foreach (DataRow nv in dtNV.Rows)
                {
                    int maNguoiDung = Convert.ToInt32(nv["MaNguoiDung"]);
                    int maVaiTro = Convert.ToInt32(nv["MaVaiTro"]);

                    // Kiểm tra đã có bảng lương chưa
                    DataTable dtExist = DatabaseConnection.ExecuteQuery(@"
                        SELECT MaBangLuong, TrangThai FROM BangLuong
                        WHERE MaNguoiDung = @MaNV AND ThangNam = @Thang",
                        p =>
                        {
                            p.AddWithValue("@MaNV", maNguoiDung);
                            p.AddWithValue("@Thang", thang);
                        });

                    bool daCoRecord = dtExist.Rows.Count > 0;
                    int maBangLuong = daCoRecord ? Convert.ToInt32(dtExist.Rows[0]["MaBangLuong"]) : 0;
                    int trangThai = daCoRecord ? Convert.ToInt32(dtExist.Rows[0]["TrangThai"]) : 0;

                    // Bỏ qua bảng lương đã duyệt / đã TT (trừ khi forceTinhLai)
                    if (daCoRecord && trangThai >= 1 && !forceTinhLai) continue;

                    // Lấy cấu hình lương — kiểm tra TRƯỚC khi reset
                    DataTable dtCH = DatabaseConnection.ExecuteQuery(@"
                        SELECT TOP 1 LoaiTinhLuong, DonGia, HeSoTangCa, SoGioChuanNgay, SoCaChuanNgay
                        FROM CauHinhLuong
                        WHERE MaVaiTro = @MaVT AND NgayHieuLuc <= @Ngay
                        ORDER BY NgayHieuLuc DESC",
                        p =>
                        {
                            p.AddWithValue("@MaVT", maVaiTro);
                            p.AddWithValue("@Ngay", cuoiThang);
                        });

                    if (dtCH.Rows.Count == 0) continue; // Không có cấu hình → bỏ qua, KHÔNG reset

                    // Nếu forceTinhLai và record đã duyệt/TT → đếm reset
                    // (UPDATE thực tế sẽ gộp chung bên dưới)
                    if (daCoRecord && trangThai >= 1 && forceTinhLai)
                    {
                        countReset++;
                    }

                    DataRow ch = dtCH.Rows[0];
                    string loai = ch["LoaiTinhLuong"].ToString();
                    decimal donGia = Convert.ToDecimal(ch["DonGia"]);
                    decimal heSoTC = Convert.ToDecimal(ch["HeSoTangCa"]);
                    int gioChuanNgay = Convert.ToInt32(ch["SoGioChuanNgay"]);

                    // ── Tính số liệu ──
                    int soBN = 0, soBNTC = 0;
                    decimal soGio = 0, soGioTC = 0;
                    int soCaDD = 0, soCaVang = 0;
                    decimal luongChinh = 0, luongTC = 0;

                    if (loai == "THEO_BN")
                    {
                        DataTable dtBN = DatabaseConnection.ExecuteQuery(@"
                            SELECT
                                pk.MaPhieuKham,
                                pk.NgayKham,
                                CASE
                                    WHEN EXISTS (
                                        SELECT 1 FROM PhanCongCa pcc
                                        JOIN CaLamViec ca ON pcc.MaCa = ca.MaCa
                                        WHERE pcc.MaNguoiDung = @MaNV
                                          AND pcc.NgayLamViec = CAST(pk.NgayKham AS DATE)
                                          AND pcc.TrangThaiDiemDanh = 2
                                          AND CAST(pk.NgayKham AS TIME) BETWEEN ca.GioBatDau AND ca.GioKetThuc
                                    ) THEN 0 ELSE 1
                                END AS LaTangCa
                            FROM PhieuKham pk
                            WHERE pk.MaNguoiDung = @MaNV
                              AND pk.TrangThai >= 2
                              AND pk.IsDeleted = 0
                              AND pk.NgayKham BETWEEN @DauThang AND @CuoiThang",
                            p =>
                            {
                                p.AddWithValue("@MaNV", maNguoiDung);
                                p.AddWithValue("@DauThang", dauThang);
                                p.AddWithValue("@CuoiThang", cuoiThang);
                            });

                        foreach (DataRow bn in dtBN.Rows)
                        {
                            if (Convert.ToInt32(bn["LaTangCa"]) == 0) soBN++;
                            else soBNTC++;
                        }

                        luongChinh = soBN * donGia;
                        luongTC = soBNTC * donGia * heSoTC;
                    }
                    else if (loai == "THEO_GIO")
                    {
                        DataTable dtGio = DatabaseConnection.ExecuteQuery(@"
                            SELECT
                                pcc.NgayLamViec,
                                DATEDIFF(MINUTE, ca.GioBatDau, ca.GioKetThuc) / 60.0 AS SoGio
                            FROM PhanCongCa pcc
                            JOIN CaLamViec ca ON pcc.MaCa = ca.MaCa
                            WHERE pcc.MaNguoiDung = @MaNV
                              AND pcc.TrangThaiDiemDanh = 2
                              AND pcc.NgayLamViec BETWEEN @DauThang AND @CuoiThang
                            ORDER BY pcc.NgayLamViec",
                            p =>
                            {
                                p.AddWithValue("@MaNV", maNguoiDung);
                                p.AddWithValue("@DauThang", dauThang);
                                p.AddWithValue("@CuoiThang", cuoiThang.Date);
                            });

                        var gioTheoNgay = new System.Collections.Generic.Dictionary<DateTime, decimal>();
                        foreach (DataRow g in dtGio.Rows)
                        {
                            DateTime ngay = Convert.ToDateTime(g["NgayLamViec"]).Date;
                            decimal gio = Convert.ToDecimal(g["SoGio"]);
                            if (!gioTheoNgay.ContainsKey(ngay)) gioTheoNgay[ngay] = 0;
                            gioTheoNgay[ngay] += gio;
                        }

                        foreach (var kv in gioTheoNgay)
                        {
                            if (kv.Value <= gioChuanNgay)
                                soGio += kv.Value;
                            else
                            {
                                soGio += gioChuanNgay;
                                soGioTC += kv.Value - gioChuanNgay;
                            }
                        }

                        luongChinh = soGio * donGia;
                        luongTC = soGioTC * donGia * heSoTC;

                        object valDD = DatabaseConnection.ExecuteScalar(@"
                            SELECT COUNT(*) FROM PhanCongCa
                            WHERE MaNguoiDung = @MaNV AND TrangThaiDiemDanh = 2
                              AND NgayLamViec BETWEEN @D AND @C",
                            p => { p.AddWithValue("@MaNV", maNguoiDung); p.AddWithValue("@D", dauThang); p.AddWithValue("@C", cuoiThang.Date); });
                        soCaDD = valDD != null ? Convert.ToInt32(valDD) : 0;

                        object valVang = DatabaseConnection.ExecuteScalar(@"
                            SELECT COUNT(*) FROM PhanCongCa
                            WHERE MaNguoiDung = @MaNV AND TrangThaiDiemDanh = 1
                              AND NgayLamViec BETWEEN @D AND @C",
                            p => { p.AddWithValue("@MaNV", maNguoiDung); p.AddWithValue("@D", dauThang); p.AddWithValue("@C", cuoiThang.Date); });
                        soCaVang = valVang != null ? Convert.ToInt32(valVang) : 0;
                    }
                    else // THEO_THANG
                    {
                        luongChinh = donGia;
                        luongTC = 0;
                    }

                    // Lấy thưởng/khấu trừ cũ (nếu đang UPDATE bảng lương Nháp thường) để giữ nguyên
                    // Khi forceTinhLai: số liệu cũ đã lưu lịch sử → reset về 0
                    decimal thuongCu = 0, khauTruCu = 0;
                    string ghiChuCu = "";
                    if (daCoRecord && !forceTinhLai)
                    {
                        DataTable dtOld = DatabaseConnection.ExecuteQuery(@"
                            SELECT ThuongThem, KhauTru, GhiChu FROM BangLuong WHERE MaBangLuong = @MaBL",
                            p => p.AddWithValue("@MaBL", maBangLuong));
                        if (dtOld.Rows.Count > 0)
                        {
                            thuongCu = Convert.ToDecimal(dtOld.Rows[0]["ThuongThem"]);
                            khauTruCu = Convert.ToDecimal(dtOld.Rows[0]["KhauTru"]);
                            ghiChuCu = dtOld.Rows[0]["GhiChu"]?.ToString() ?? "";
                        }
                    }

                    decimal tongLuong = luongChinh + luongTC + thuongCu - khauTruCu;
                    if (tongLuong < 0) tongLuong = 0;

                    if (daCoRecord)
                    {
                        // UPDATE — gộp tất cả thay đổi vào 1 câu SQL duy nhất
                        // forceTinhLai: reset TrangThai=0, NguoiDuyet=NULL, NgayDuyet=NULL + xóa Thưởng/KhấuTrừ
                        // normal: giữ nguyên TrangThai, chỉ cập nhật số liệu
                        string updateSql = forceTinhLai
                            ? @"UPDATE BangLuong SET
                                    TrangThai = 0, NguoiDuyet = NULL, NgayDuyet = NULL,
                                    MaVaiTro = @MaVT, LoaiTinhLuong = @Loai,
                                    DonGia = @DonGia, HeSoTangCa = @HeSoTC,
                                    SoBenhNhan = @SoBN, SoBNTangCa = @SoBNTC,
                                    SoGioLam = @SoGio, SoGioTangCa = @SoGioTC,
                                    SoCaDiemDanh = @SoCaDD, SoCaVang = @SoCaVang,
                                    LuongChinh = @LuongChinh, LuongTangCa = @LuongTC,
                                    ThuongThem = 0, KhauTru = 0, GhiChu = NULL,
                                    TongLuong = @TongLuong
                                WHERE MaBangLuong = @MaBL"
                            : @"UPDATE BangLuong SET
                                    MaVaiTro = @MaVT, LoaiTinhLuong = @Loai,
                                    DonGia = @DonGia, HeSoTangCa = @HeSoTC,
                                    SoBenhNhan = @SoBN, SoBNTangCa = @SoBNTC,
                                    SoGioLam = @SoGio, SoGioTangCa = @SoGioTC,
                                    SoCaDiemDanh = @SoCaDD, SoCaVang = @SoCaVang,
                                    LuongChinh = @LuongChinh, LuongTangCa = @LuongTC,
                                    ThuongThem = @Thuong, KhauTru = @KhauTru, GhiChu = @GhiChu,
                                    TongLuong = @TongLuong
                                WHERE MaBangLuong = @MaBL";

                        DatabaseConnection.ExecuteNonQuery(updateSql,
                            p =>
                            {
                                p.AddWithValue("@MaVT", maVaiTro);
                                p.AddWithValue("@Loai", loai);
                                p.AddWithValue("@DonGia", donGia);
                                p.AddWithValue("@HeSoTC", heSoTC);
                                p.AddWithValue("@SoBN", soBN);
                                p.AddWithValue("@SoBNTC", soBNTC);
                                p.AddWithValue("@SoGio", soGio);
                                p.AddWithValue("@SoGioTC", soGioTC);
                                p.AddWithValue("@SoCaDD", soCaDD);
                                p.AddWithValue("@SoCaVang", soCaVang);
                                p.AddWithValue("@LuongChinh", luongChinh);
                                p.AddWithValue("@LuongTC", luongTC);
                                if (!forceTinhLai)
                                {
                                    p.AddWithValue("@Thuong", thuongCu);
                                    p.AddWithValue("@KhauTru", khauTruCu);
                                    p.AddWithValue("@GhiChu", string.IsNullOrEmpty(ghiChuCu) ? (object)DBNull.Value : ghiChuCu);
                                }
                                p.AddWithValue("@TongLuong", tongLuong);
                                p.AddWithValue("@MaBL", maBangLuong);
                            });
                        countUpdate++;
                    }
                    else
                    {
                        // INSERT mới
                        DatabaseConnection.ExecuteNonQuery(@"
                            INSERT INTO BangLuong
                                (MaNguoiDung, MaVaiTro, ThangNam, LoaiTinhLuong, DonGia, HeSoTangCa,
                                 SoBenhNhan, SoBNTangCa, SoGioLam, SoGioTangCa,
                                 SoCaDiemDanh, SoCaVang,
                                 LuongChinh, LuongTangCa, ThuongThem, KhauTru, TongLuong,
                                 TrangThai)
                            VALUES
                                (@MaNV, @MaVT, @Thang, @Loai, @DonGia, @HeSoTC,
                                 @SoBN, @SoBNTC, @SoGio, @SoGioTC,
                                 @SoCaDD, @SoCaVang,
                                 @LuongChinh, @LuongTC, 0, 0, @TongLuong,
                                 0)",
                            p =>
                            {
                                p.AddWithValue("@MaNV", maNguoiDung);
                                p.AddWithValue("@MaVT", maVaiTro);
                                p.AddWithValue("@Thang", thang);
                                p.AddWithValue("@Loai", loai);
                                p.AddWithValue("@DonGia", donGia);
                                p.AddWithValue("@HeSoTC", heSoTC);
                                p.AddWithValue("@SoBN", soBN);
                                p.AddWithValue("@SoBNTC", soBNTC);
                                p.AddWithValue("@SoGio", soGio);
                                p.AddWithValue("@SoGioTC", soGioTC);
                                p.AddWithValue("@SoCaDD", soCaDD);
                                p.AddWithValue("@SoCaVang", soCaVang);
                                p.AddWithValue("@LuongChinh", luongChinh);
                                p.AddWithValue("@LuongTC", luongTC);
                                p.AddWithValue("@TongLuong", tongLuong);
                            });
                        countInsert++;
                    }
                }

                // ── Reload dữ liệu TRƯỚC khi hiện thông báo ──
                // (MessageBox có thể trigger Resize → VeLaiForm → ghi đè data)
                _dangXuLy = false;
                LoadDuLieu();

                if (hienThongBao)
                {
                    string msg = $"✅ Bảng lương tháng {thang:MM/yyyy}:\n";
                    if (countInsert > 0) msg += $"• Thêm mới: {countInsert} NV\n";
                    if (countUpdate > 0) msg += $"• Cập nhật: {countUpdate} NV\n";
                    if (countReset > 0) msg += $"• Tính lại (reset về Nháp): {countReset} NV\n";
                    if (countInsert == 0 && countUpdate == 0 && countReset == 0) msg += "• Không có thay đổi\n";
                    msg += "\n💡 Bảng lương Nháp tự động cập nhật mỗi 5 phút.";

                    MessageBox.Show(msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _dangXuLy = false;
                if (hienThongBao)
                    MessageBox.Show("Lỗi tính lương:\n" + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // DUYỆT LƯƠNG — TrangThai 0 → 1
        // ══════════════════════════════════════════════════════════════════════

        private void BtnDuyet_Click(object sender, EventArgs e)
        {
            DateTime thang = LayThangDangChon();

            object countNhap = DatabaseConnection.ExecuteScalar(@"
                SELECT COUNT(*) FROM BangLuong WHERE ThangNam = @Thang AND TrangThai = 0",
                p => p.AddWithValue("@Thang", thang));

            int soNhap = countNhap != null ? Convert.ToInt32(countNhap) : 0;
            if (soNhap == 0)
            {
                MessageBox.Show("Không có bảng lương nháp nào để duyệt.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Duyệt {soNhap} bảng lương nháp tháng {thang:MM/yyyy}?",
                "Xác nhận duyệt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            _dangXuLy = true;
            int maNguoiDuyet = LoginForm.NguoiDungHienTai?.MaNguoiDung ?? 0;

            DatabaseConnection.ExecuteNonQuery(@"
                UPDATE BangLuong
                SET TrangThai = 1, NguoiDuyet = @NguoiDuyet, NgayDuyet = GETDATE()
                WHERE ThangNam = @Thang AND TrangThai = 0",
                p =>
                {
                    p.AddWithValue("@Thang", thang);
                    p.AddWithValue("@NguoiDuyet", maNguoiDuyet);
                });

            _dangXuLy = false;
            LoadDuLieu();

            MessageBox.Show($"✅ Đã duyệt {soNhap} bảng lương.", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ══════════════════════════════════════════════════════════════════════
        // HOÀN TÁC DUYỆT — TrangThai 1 → 0
        // ══════════════════════════════════════════════════════════════════════

        private void BtnHoanTacDuyet_Click(object sender, EventArgs e)
        {
            DateTime thang = LayThangDangChon();

            object countDaDuyet = DatabaseConnection.ExecuteScalar(@"
                SELECT COUNT(*) FROM BangLuong WHERE ThangNam = @Thang AND TrangThai = 1",
                p => p.AddWithValue("@Thang", thang));

            int soDaDuyet = countDaDuyet != null ? Convert.ToInt32(countDaDuyet) : 0;
            if (soDaDuyet == 0)
            {
                MessageBox.Show("Không có bảng lương đã duyệt nào để hoàn tác.\n" +
                    "(Bảng lương đã thanh toán không thể hoàn tác)",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Hoàn tác duyệt {soDaDuyet} bảng lương tháng {thang:MM/yyyy}?\n\n" +
                "Bảng lương sẽ chuyển về trạng thái Nháp để chỉnh sửa lại.\n" +
                "(Bảng lương đã thanh toán không bị ảnh hưởng)",
                "Xác nhận hoàn tác", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            _dangXuLy = true;
            DatabaseConnection.ExecuteNonQuery(@"
                UPDATE BangLuong
                SET TrangThai = 0, NguoiDuyet = NULL, NgayDuyet = NULL
                WHERE ThangNam = @Thang AND TrangThai = 1",
                p => p.AddWithValue("@Thang", thang));

            _dangXuLy = false;
            LoadDuLieu();

            MessageBox.Show($"↩️ Đã hoàn tác duyệt {soDaDuyet} bảng lương về trạng thái Nháp.",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ══════════════════════════════════════════════════════════════════════
        // THANH TOÁN LƯƠNG — TrangThai 1 → 2 + INSERT LichSuTraLuong
        // ══════════════════════════════════════════════════════════════════════

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            // Kiểm tra người dùng đang đăng nhập
            var nguoiDung = LoginForm.NguoiDungHienTai;
            if (nguoiDung == null || nguoiDung.MaNguoiDung <= 0)
            {
                MessageBox.Show("Lỗi: Không xác định được người dùng đang đăng nhập.\nVui lòng đăng nhập lại.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime thang = LayThangDangChon();

            DataTable dtDaDuyet = DatabaseConnection.ExecuteQuery(@"
                SELECT MaBangLuong, TongLuong FROM BangLuong
                WHERE ThangNam = @Thang AND TrangThai = 1",
                p => p.AddWithValue("@Thang", thang));

            if (dtDaDuyet.Rows.Count == 0)
            {
                MessageBox.Show("Không có bảng lương đã duyệt để thanh toán.\nVui lòng duyệt trước.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal tongTien = 0;
            foreach (DataRow r in dtDaDuyet.Rows)
                tongTien += Convert.ToDecimal(r["TongLuong"]);

            var confirm = MessageBox.Show(
                $"Thanh toán lương tháng {thang:MM/yyyy}?\n\n" +
                $"Số nhân viên: {dtDaDuyet.Rows.Count}\n" +
                $"Tổng tiền: {tongTien:#,##0}đ\n\n" +
                "Xác nhận thanh toán?",
                "Xác nhận thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            int nguoiTra = nguoiDung.MaNguoiDung;
            string loiChiTiet = null;

            _dangXuLy = true;
            bool ok = false;
            try
            {
                ok = DatabaseConnection.ExecuteTransaction((conn, tran) =>
                {
                    // Đảm bảo bảng LichSuTraLuong tồn tại
                    using (var cmdCheck = new SqlCommand(@"
                        IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'LichSuTraLuong')
                        BEGIN
                            CREATE TABLE LichSuTraLuong (
                                MaTraLuong   INT IDENTITY(1,1) PRIMARY KEY,
                                MaBangLuong  INT            NOT NULL,
                                SoTienTra    DECIMAL(18,2)  NOT NULL,
                                PhuongThuc   NVARCHAR(50),
                                NgayTra      DATETIME       DEFAULT GETDATE(),
                                NguoiTra     INT            NOT NULL,
                                GhiChu       NVARCHAR(255),
                                FOREIGN KEY (MaBangLuong) REFERENCES BangLuong(MaBangLuong),
                                FOREIGN KEY (NguoiTra)    REFERENCES NguoiDung(MaNguoiDung)
                            );
                        END", conn, tran))
                    {
                        cmdCheck.ExecuteNonQuery();
                    }

                    foreach (DataRow r in dtDaDuyet.Rows)
                    {
                        int maBL = Convert.ToInt32(r["MaBangLuong"]);
                        decimal soTien = Convert.ToDecimal(r["TongLuong"]);

                        // UPDATE BangLuong → Đã TT
                        using (var cmd = new SqlCommand(@"
                            UPDATE BangLuong SET TrangThai = 2 WHERE MaBangLuong = @MaBL", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@MaBL", maBL);
                            cmd.ExecuteNonQuery();
                        }

                        // INSERT LichSuTraLuong — bỏ qua NV có lương 0đ (CHECK: SoTienTra > 0)
                        if (soTien > 0)
                        {
                            using (var cmd = new SqlCommand(@"
                                INSERT INTO LichSuTraLuong (MaBangLuong, SoTienTra, PhuongThuc, NguoiTra, GhiChu)
                                VALUES (@MaBL, @SoTien, N'Chuyển khoản', @NguoiTra, @GhiChu)", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@MaBL", maBL);
                                cmd.Parameters.AddWithValue("@SoTien", soTien);
                                cmd.Parameters.AddWithValue("@NguoiTra", nguoiTra);
                                cmd.Parameters.AddWithValue("@GhiChu", $"Thanh toán lương tháng {thang:MM/yyyy}");
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                loiChiTiet = ex.Message;
            }

            _dangXuLy = false;
            LoadDuLieu();

            if (ok)
            {
                MessageBox.Show($"✅ Đã thanh toán lương cho {dtDaDuyet.Rows.Count} nhân viên.\n" +
                    $"Tổng: {tongTien:#,##0}đ",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (loiChiTiet != null)
            {
                MessageBox.Show($"❌ Thanh toán thất bại:\n{loiChiTiet}",
                    "Lỗi thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // LỊCH SỬ THANH TOÁN LƯƠNG
        // ══════════════════════════════════════════════════════════════════════

        private void BtnLichSuTT_Click(object sender, EventArgs e)
        {
            DateTime thang = LayThangDangChon();

            int dlgW = 960;
            int dlgH = 640;
            int pad = 24;

            using (var dlg = new Form
            {
                Text = $"Lịch sử thanh toán lương — Tháng {thang:MM/yyyy}",
                Size = new Size(dlgW, dlgH),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.Sizable,
                MaximizeBox = true,
                MinimizeBox = false,
                MinimumSize = new Size(760, 480),
                BackColor = ColorScheme.Background,
            })
            {
                int clientW = dlg.ClientSize.Width;
                int clientH = dlg.ClientSize.Height;

                // ── Header vàng gold ──
                var pnlHeader = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 76,
                    BackColor = GoldAccent,
                };
                var lblTitle = new Label
                {
                    Text = $"📋 Lịch sử thanh toán lương",
                    Font = AppFonts.H3,
                    ForeColor = Color.White,
                    AutoSize = false,
                    Size = new Size(clientW - 30, 30),
                    Location = new Point(20, 12),
                };
                var lblSubTitle = new Label
                {
                    Text = $"Tháng {thang:MM/yyyy}  •  Danh sách tất cả giao dịch thanh toán",
                    Font = AppFonts.Body,
                    ForeColor = Color.FromArgb(210, 255, 255, 255),
                    AutoSize = false,
                    Size = new Size(clientW - 30, 22),
                    Location = new Point(20, 46),
                };
                pnlHeader.Controls.Add(lblTitle);
                pnlHeader.Controls.Add(lblSubTitle);
                dlg.Controls.Add(pnlHeader);

                // ── KPI tổng hợp ──
                var pnlKpi = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 60,
                    BackColor = Color.White,
                    Padding = new Padding(pad, 10, pad, 10),
                };
                pnlKpi.Paint += (s, ev) =>
                {
                    using (var pen = new Pen(BorderInput))
                        ev.Graphics.DrawLine(pen, 0, pnlKpi.Height - 1, pnlKpi.Width, pnlKpi.Height - 1);
                };

                var lblKpiSoGD = new Label
                {
                    Font = AppFonts.Body,
                    ForeColor = ColorScheme.TextMid,
                    AutoSize = true,
                    Location = new Point(pad, 18),
                };
                pnlKpi.Controls.Add(lblKpiSoGD);

                var lblKpiTong = new Label
                {
                    Font = AppFonts.H4,
                    ForeColor = GoldAccent,
                    AutoSize = true,
                    Location = new Point(300, 14),
                };
                pnlKpi.Controls.Add(lblKpiTong);

                dlg.Controls.Add(pnlKpi);

                // ── DataGridView ──
                var pnlBody = new Panel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(pad, 12, pad, pad),
                    BackColor = ColorScheme.Background,
                };

                var dgvLichSu = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                    GridColor = ColorScheme.Border,
                    RowHeadersVisible = false,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    ReadOnly = true,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Font = AppFonts.Body,
                        ForeColor = ColorScheme.TextDark,
                        SelectionBackColor = ColorScheme.PrimaryPale,
                        SelectionForeColor = ColorScheme.TextDark,
                        Padding = new Padding(8, 4, 8, 4),
                    },
                    ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                    {
                        Font = AppFonts.BodyBold,
                        ForeColor = ColorScheme.TextMid,
                        BackColor = Color.FromArgb(249, 250, 251),
                        Alignment = DataGridViewContentAlignment.MiddleLeft,
                        Padding = new Padding(8, 0, 8, 0),
                    },
                    ColumnHeadersHeight = 42,
                    RowTemplate = { Height = 40 },
                    EnableHeadersVisualStyles = false,
                    AutoGenerateColumns = false,
                };

                dgvLichSu.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSTT", HeaderText = "#", FillWeight = 5F });
                dgvLichSu.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHoTen", HeaderText = "Nhân viên", FillWeight = 18F });
                dgvLichSu.Columns.Add(new DataGridViewTextBoxColumn { Name = "colVaiTro", HeaderText = "Vai trò", FillWeight = 12F });
                dgvLichSu.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSoTien", HeaderText = "Số tiền", FillWeight = 14F });
                dgvLichSu.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPhuongThuc", HeaderText = "Phương thức", FillWeight = 12F });
                dgvLichSu.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNgayTra", HeaderText = "Ngày thanh toán", FillWeight = 16F });
                dgvLichSu.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNguoiTra", HeaderText = "Người thực hiện", FillWeight = 14F });
                dgvLichSu.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGhiChu", HeaderText = "Ghi chú", FillWeight = 14F });

                // Load dữ liệu — dùng LEFT JOIN để hiển thị TẤT CẢ NV đã thanh toán
                // (bao gồm NV có lương 0đ không có record trong LichSuTraLuong)
                try
                {
                    DataTable dtLS = DatabaseConnection.ExecuteQuery(@"
                        SELECT
                            bl.MaBangLuong,
                            nd.HoTen,
                            vt.TenVaiTro,
                            ISNULL(ls.SoTienTra, bl.TongLuong) AS SoTienTra,
                            ISNULL(ls.PhuongThuc, CASE WHEN bl.TrangThai = 2 THEN N'—' ELSE NULL END) AS PhuongThuc,
                            ls.NgayTra,
                            ndTra.HoTen AS NguoiTraTen,
                            ISNULL(ls.GhiChu, CASE WHEN bl.TrangThai = 2 AND bl.TongLuong = 0 THEN N'Lương 0đ — không tạo phiếu chi' ELSE NULL END) AS GhiChu,
                            bl.TrangThai
                        FROM BangLuong bl
                        JOIN NguoiDung nd ON bl.MaNguoiDung = nd.MaNguoiDung
                        JOIN VaiTro vt ON bl.MaVaiTro = vt.MaVaiTro
                        LEFT JOIN LichSuTraLuong ls ON ls.MaBangLuong = bl.MaBangLuong
                        LEFT JOIN NguoiDung ndTra ON ls.NguoiTra = ndTra.MaNguoiDung
                        WHERE bl.ThangNam = @Thang AND bl.TrangThai = 2
                        ORDER BY ls.NgayTra DESC, nd.HoTen",
                        p => p.AddWithValue("@Thang", thang));

                    decimal tongTienLS = 0;
                    int stt = 0;

                    foreach (DataRow r in dtLS.Rows)
                    {
                        stt++;
                        decimal soTien = Convert.ToDecimal(r["SoTienTra"]);
                        tongTienLS += soTien;
                        string ngayTraText = r["NgayTra"] != DBNull.Value
                            ? Convert.ToDateTime(r["NgayTra"]).ToString("dd/MM/yyyy HH:mm")
                            : "—";

                        int idx = dgvLichSu.Rows.Add(
                            stt,
                            r["HoTen"].ToString(),
                            r["TenVaiTro"].ToString(),
                            soTien.ToString("#,##0") + "đ",
                            r["PhuongThuc"]?.ToString() ?? "—",
                            ngayTraText,
                            r["NguoiTraTen"] != DBNull.Value ? r["NguoiTraTen"].ToString() : "—",
                            r["GhiChu"] != DBNull.Value ? r["GhiChu"].ToString() : ""
                        );

                        // Tô đậm cột tiền
                        dgvLichSu.Rows[idx].Cells["colSoTien"].Style.Font = AppFonts.BodyBold;
                        dgvLichSu.Rows[idx].Cells["colSoTien"].Style.ForeColor = soTien > 0 ? GoldAccent : ColorScheme.TextGray;
                    }

                    int soCoTien = 0;
                    foreach (DataRow r2 in dtLS.Rows)
                    {
                        if (Convert.ToDecimal(r2["SoTienTra"]) > 0) soCoTien++;
                    }
                    lblKpiSoGD.Text = $"📊 Tổng NV đã TT: {dtLS.Rows.Count}  |  Có phiếu chi: {soCoTien}";
                    lblKpiTong.Text = $"💰 Tổng đã thanh toán: {tongTienLS:#,##0}đ";
                }
                catch (Exception ex)
                {
                    lblKpiSoGD.Text = "Lỗi tải dữ liệu";
                    lblKpiTong.Text = ex.Message;
                }

                pnlBody.Controls.Add(dgvLichSu);
                dlg.Controls.Add(pnlBody);

                dlg.ShowDialog();
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // XUẤT PHIẾU LƯƠNG — In phiếu cho NV đang chọn (PrintPreviewDialog)
        // ══════════════════════════════════════════════════════════════════════

        private void BtnXuatPhieu_Click(object sender, EventArgs e)
        {
            if (dgvBangLuong == null || dgvBangLuong.CurrentRow == null || dgvBangLuong.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên trong bảng lương.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvBangLuong.CurrentRow;
            int maBL = Convert.ToInt32(row.Cells["colMaBL"].Value);

            DataTable dt = DatabaseConnection.ExecuteQuery(@"
                SELECT bl.*, nd.HoTen, nd.SoDienThoai, vt.TenVaiTro,
                       ttpk.TenPhongKham, ttpk.DiaChi AS DiaChiPK, ttpk.SoDienThoai AS SdtPK
                FROM BangLuong bl
                JOIN NguoiDung nd ON bl.MaNguoiDung = nd.MaNguoiDung
                JOIN VaiTro vt ON bl.MaVaiTro = vt.MaVaiTro
                CROSS JOIN (SELECT TOP 1 * FROM ThongTinPhongKham) ttpk
                WHERE bl.MaBangLuong = @MaBL",
                p => p.AddWithValue("@MaBL", maBL));

            if (dt.Rows.Count == 0) return;
            DataRow r = dt.Rows[0];

            var printer = new PhieuLuongPrinter(
                tenPK: r["TenPhongKham"]?.ToString() ?? "DermaSoft Clinic",
                diaChiPK: r["DiaChiPK"]?.ToString() ?? "",
                sdtPK: r["SdtPK"]?.ToString() ?? "",
                hoTen: r["HoTen"].ToString(),
                sdt: r["SoDienThoai"]?.ToString() ?? "",
                vaiTro: r["TenVaiTro"].ToString(),
                thangNam: Convert.ToDateTime(r["ThangNam"]),
                loai: r["LoaiTinhLuong"].ToString(),
                donGia: Convert.ToDecimal(r["DonGia"]),
                heSoTC: Convert.ToDecimal(r["HeSoTangCa"]),
                soBN: Convert.ToInt32(r["SoBenhNhan"]),
                soBNTC: Convert.ToInt32(r["SoBNTangCa"]),
                soGio: Convert.ToDecimal(r["SoGioLam"]),
                soGioTC: Convert.ToDecimal(r["SoGioTangCa"]),
                luongChinh: Convert.ToDecimal(r["LuongChinh"]),
                luongTC: Convert.ToDecimal(r["LuongTangCa"]),
                thuong: Convert.ToDecimal(r["ThuongThem"]),
                khauTru: Convert.ToDecimal(r["KhauTru"]),
                tongLuong: Convert.ToDecimal(r["TongLuong"]),
                ghiChu: r["GhiChu"]?.ToString() ?? ""
            );

            printer.MoXemTruoc(this);
        }

        // ══════════════════════════════════════════════════════════════════════
        // CHI TIẾT — Double-click → Dialog sửa Thưởng/Khấu trừ
        // ══════════════════════════════════════════════════════════════════════

        private void DgvBangLuong_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvBangLuong.Rows[e.RowIndex];
            int maBL = Convert.ToInt32(row.Cells["colMaBL"].Value);
            int trangThai = Convert.ToInt32(row.Cells["colTrangThaiGoc"].Value);
            string hoTen = row.Cells["colHoTen"].Value?.ToString() ?? "";
            string vaiTro = row.Cells["colVaiTro"].Value?.ToString() ?? "";

            DataTable dt = DatabaseConnection.ExecuteQuery(@"
                SELECT ThuongThem, KhauTru, GhiChu, TrangThai,
                       LoaiTinhLuong, DonGia, HeSoTangCa,
                       SoBenhNhan, SoBNTangCa, SoGioLam, SoGioTangCa,
                       SoCaDiemDanh, SoCaVang,
                       LuongChinh, LuongTangCa, TongLuong
                FROM BangLuong WHERE MaBangLuong = @MaBL",
                p => p.AddWithValue("@MaBL", maBL));

            if (dt.Rows.Count == 0) return;
            DataRow r = dt.Rows[0];

            decimal thuong = Convert.ToDecimal(r["ThuongThem"]);
            decimal khauTru = Convert.ToDecimal(r["KhauTru"]);
            string ghiChu = r["GhiChu"]?.ToString() ?? "";
            decimal luongChinh = Convert.ToDecimal(r["LuongChinh"]);
            decimal luongTC = Convert.ToDecimal(r["LuongTangCa"]);
            decimal tongLuong = Convert.ToDecimal(r["TongLuong"]);
            string loai = r["LoaiTinhLuong"]?.ToString() ?? "";
            decimal donGia = Convert.ToDecimal(r["DonGia"]);
            decimal heSoTC = Convert.ToDecimal(r["HeSoTangCa"]);
            int soBN = Convert.ToInt32(r["SoBenhNhan"]);
            int soBNTC = Convert.ToInt32(r["SoBNTangCa"]);
            decimal soGio = Convert.ToDecimal(r["SoGioLam"]);
            decimal soGioTC = Convert.ToDecimal(r["SoGioTangCa"]);
            int soCaDD = Convert.ToInt32(r["SoCaDiemDanh"]);
            int soCaVang = Convert.ToInt32(r["SoCaVang"]);
            bool chiDoc = trangThai >= 1;

            string trangThaiText = trangThai == 0 ? "📝 Nháp" : trangThai == 1 ? "✅ Đã duyệt" : "💳 Đã thanh toán";

            int dlgW = 620;
            int dlgH = 920;
            int pad = 28;
            int fieldW = dlgW - pad * 2 - 16;

            using (var dlg = new Form
            {
                Text = $"Chi tiết lương — {hoTen}",
                Size = new Size(dlgW, dlgH),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = ColorScheme.Background,
            })
            {
                int clientW = dlg.ClientSize.Width;
                int clientH = dlg.ClientSize.Height;
                fieldW = clientW - pad * 2;

                // ── Header vàng gold ──
                var pnlHeader = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 76,
                    BackColor = GoldAccent,
                };
                var lblTitle = new Label
                {
                    Text = $"💰 Chi tiết lương — {hoTen}",
                    Font = AppFonts.H3,
                    ForeColor = Color.White,
                    AutoSize = false,
                    Size = new Size(clientW - 30, 30),
                    Location = new Point(20, 12),
                };
                var lblSubTitle = new Label
                {
                    Text = $"{vaiTro}  •  {trangThaiText}",
                    Font = AppFonts.Body,
                    ForeColor = Color.FromArgb(210, 255, 255, 255),
                    AutoSize = false,
                    Size = new Size(clientW - 30, 22),
                    Location = new Point(20, 46),
                };
                pnlHeader.Controls.Add(lblTitle);
                pnlHeader.Controls.Add(lblSubTitle);
                dlg.Controls.Add(pnlHeader);

                // ── Scrollable content ──
                var pnlBody = new Panel
                {
                    Location = new Point(0, 76),
                    Size = new Size(clientW, clientH - 76),
                    AutoScroll = true,
                    BackColor = ColorScheme.Background,
                };
                dlg.Controls.Add(pnlBody);

                // Trừ thêm scrollbar width để card + TextBox không bị tràn
                int scrollBarW = SystemInformation.VerticalScrollBarWidth;
                fieldW = clientW - pad * 2 - scrollBarW;

                int dy = 20;

                // ── Section: Thông tin tính lương ──
                dy = ThemSectionHeader(pnlBody, "📋 THÔNG TIN TÍNH LƯƠNG", pad, dy, fieldW);

                var pnlInfo = ThemCard(pnlBody, pad, dy, fieldW, 0);
                int iy = 16;
                string loaiText = loai == "THEO_BN" ? "Theo bệnh nhân" : loai == "THEO_GIO" ? "Theo giờ" : "Cố định/tháng";
                iy = ThemDongThongTin(pnlInfo, "Loại tính lương:", loaiText, 12, iy, fieldW - 24);
                iy = ThemDongThongTin(pnlInfo, "Đơn giá:", donGia.ToString("#,##0") + "đ", 12, iy, fieldW - 24);
                if (loai != "THEO_THANG")
                    iy = ThemDongThongTin(pnlInfo, "Hệ số tăng ca:", "×" + heSoTC.ToString("0.#"), 12, iy, fieldW - 24);
                pnlInfo.Height = iy + 12;
                dy += pnlInfo.Height + 16;

                // ── Section: Số liệu làm việc ──
                if (loai != "THEO_THANG")
                {
                    dy = ThemSectionHeader(pnlBody, "📊 SỐ LIỆU LÀM VIỆC", pad, dy, fieldW);
                    var pnlSoLieu = ThemCard(pnlBody, pad, dy, fieldW, 0);
                    int sy = 16;

                    if (loai == "THEO_BN")
                    {
                        sy = ThemDongThongTin(pnlSoLieu, "BN trong ca:", $"{soBN} bệnh nhân", 12, sy, fieldW - 24);
                        sy = ThemDongThongTin(pnlSoLieu, "BN tăng ca:", $"{soBNTC} bệnh nhân", 12, sy, fieldW - 24);
                    }
                    else
                    {
                        sy = ThemDongThongTin(pnlSoLieu, "Giờ thường:", $"{soGio:#,##0.#} giờ", 12, sy, fieldW - 24);
                        sy = ThemDongThongTin(pnlSoLieu, "Giờ tăng ca:", $"{soGioTC:#,##0.#} giờ", 12, sy, fieldW - 24);
                        sy = ThemDongThongTin(pnlSoLieu, "Ca điểm danh:", $"{soCaDD} ca", 12, sy, fieldW - 24);
                        sy = ThemDongThongTin(pnlSoLieu, "Ca vắng:", $"{soCaVang} ca", 12, sy, fieldW - 24);
                    }
                    pnlSoLieu.Height = sy + 12;
                    dy += pnlSoLieu.Height + 16;
                }

                // ── Section: Chi tiết lương ──
                dy = ThemSectionHeader(pnlBody, "💵 CHI TIẾT LƯƠNG", pad, dy, fieldW);
                var pnlLuong = ThemCard(pnlBody, pad, dy, fieldW, 0);
                int ly = 16;

                if (loai == "THEO_BN")
                {
                    ly = ThemDongTien(pnlLuong, "Lương BN trong ca", $"{soBN} × {donGia:#,##0}đ", luongChinh, 12, ly, fieldW - 24, false);
                    ly = ThemDongTien(pnlLuong, "Lương BN tăng ca", $"{soBNTC} × {donGia:#,##0}đ × {heSoTC:0.#}", luongTC, 12, ly, fieldW - 24, false);
                }
                else if (loai == "THEO_GIO")
                {
                    ly = ThemDongTien(pnlLuong, "Lương giờ thường", $"{soGio:#,##0.#}h × {donGia:#,##0}đ", luongChinh, 12, ly, fieldW - 24, false);
                    ly = ThemDongTien(pnlLuong, "Lương giờ tăng ca", $"{soGioTC:#,##0.#}h × {donGia:#,##0}đ × {heSoTC:0.#}", luongTC, 12, ly, fieldW - 24, false);
                }
                else
                {
                    ly = ThemDongTien(pnlLuong, "Lương cố định", "", luongChinh, 12, ly, fieldW - 24, false);
                }

                // Separator
                var sepLuong = new Panel { Location = new Point(16, ly + 4), Size = new Size(fieldW - 32, 1), BackColor = ColorScheme.Border };
                pnlLuong.Controls.Add(sepLuong);
                ly += 16;

                ly = ThemDongTien(pnlLuong, "Lương chính", "", luongChinh, 12, ly, fieldW - 24, false);
                ly = ThemDongTien(pnlLuong, "Lương tăng ca", "", luongTC, 12, ly, fieldW - 24, false);
                ly = ThemDongTien(pnlLuong, "Thưởng thêm", "", thuong, 12, ly, fieldW - 24, false, ColorScheme.Success, "+");
                ly = ThemDongTien(pnlLuong, "Khấu trừ", "", khauTru, 12, ly, fieldW - 24, false, ColorScheme.Danger, "-");

                // Separator tổng
                var sepTong = new Panel { Location = new Point(16, ly + 4), Size = new Size(fieldW - 32, 2), BackColor = GoldAccent };
                pnlLuong.Controls.Add(sepTong);
                ly += 16;
                ly = ThemDongTien(pnlLuong, "TỔNG LƯƠNG", "", tongLuong, 16, ly, fieldW - 32, true);
                ly += 6;

                pnlLuong.Height = ly + 12;
                dy += pnlLuong.Height + 20;

                // ── Section: Chỉnh sửa (Thưởng / Khấu trừ / Ghi chú) ──
                dy = ThemSectionHeader(pnlBody, chiDoc ? "🔒 THÔNG TIN BỔ SUNG (CHỈ XEM)" : "✏️ CHỈNH SỬA", pad, dy, fieldW);
                var pnlEdit = ThemCard(pnlBody, pad, dy, fieldW, 0);
                int ey = 20;

                // Thưởng
                pnlEdit.Controls.Add(new Label { Text = "Thưởng thêm (đ):", Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextDark, Location = new Point(20, ey), AutoSize = true });
                ey += 28;
                int txtFixedW = fieldW - 98;
                var txtThuong = new Guna2TextBox
                {
                    Text = thuong.ToString("#,##0"),
                    Location = new Point(20, ey),
                    Size = new Size(txtFixedW, 44),
                    Font = AppFonts.Body,
                    FillColor = chiDoc ? Color.FromArgb(245, 245, 245) : Color.White,
                    BorderColor = BorderInput,
                    BorderRadius = 8,
                    ReadOnly = chiDoc,
                };
                pnlEdit.Controls.Add(txtThuong);
                ey += 56;

                // Khấu trừ
                pnlEdit.Controls.Add(new Label { Text = "Khấu trừ (đ):", Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextDark, Location = new Point(20, ey), AutoSize = true });
                ey += 28;
                var txtKhauTru = new Guna2TextBox
                {
                    Text = khauTru.ToString("#,##0"),
                    Location = new Point(20, ey),
                    Size = new Size(txtFixedW, 44),
                    Font = AppFonts.Body,
                    FillColor = chiDoc ? Color.FromArgb(245, 245, 245) : Color.White,
                    BorderColor = BorderInput,
                    BorderRadius = 8,
                    ReadOnly = chiDoc,
                };
                pnlEdit.Controls.Add(txtKhauTru);
                ey += 56;

                // Ghi chú
                pnlEdit.Controls.Add(new Label { Text = "Ghi chú:", Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextDark, Location = new Point(20, ey), AutoSize = true });
                ey += 28;
                var txtGhiChu = new Guna2TextBox
                {
                    Text = ghiChu,
                    Location = new Point(20, ey),
                    Size = new Size(txtFixedW, 44),
                    Font = AppFonts.Body,
                    FillColor = chiDoc ? Color.FromArgb(245, 245, 245) : Color.White,
                    BorderColor = BorderInput,
                    BorderRadius = 8,
                    ReadOnly = chiDoc,
                };
                pnlEdit.Controls.Add(txtGhiChu);
                ey += 58 + 10;

                // Tổng lương preview (cập nhật realtime)
                var lblTongPreview = new Label
                {
                    Text = $"Tổng lương: {tongLuong:#,##0}đ",
                    Font = AppFonts.H3,
                    ForeColor = GoldAccent,
                    Location = new Point(20, ey),
                    AutoSize = true,
                };
                pnlEdit.Controls.Add(lblTongPreview);

                if (!chiDoc)
                {
                    // Realtime preview
                    System.EventHandler updatePreview = (s2, e2) =>
                    {
                        decimal t, k;
                        if (!decimal.TryParse(txtThuong.Text.Replace(",", "").Replace(".", ""), out t)) t = 0;
                        if (!decimal.TryParse(txtKhauTru.Text.Replace(",", "").Replace(".", ""), out k)) k = 0;
                        decimal preview = luongChinh + luongTC + t - k;
                        if (preview < 0) preview = 0;
                        lblTongPreview.Text = $"Tổng lương: {preview:#,##0}đ";
                    };
                    txtThuong.TextChanged += updatePreview;
                    txtKhauTru.TextChanged += updatePreview;

                    var btnLuu = new Guna2Button
                    {
                        Text = "💾 Lưu thay đổi",
                        Font = AppFonts.BodyBold,
                        ForeColor = Color.White,
                        FillColor = ColorScheme.Primary,
                        BorderRadius = 10,
                        Size = new Size(180, 46),
                        Location = new Point(fieldW - 200, ey),
                        Cursor = Cursors.Hand,
                    };
                    btnLuu.Click += (s2, e2) =>
                    {
                        decimal newThuong, newKhauTru;
                        if (!decimal.TryParse(txtThuong.Text.Replace(",", "").Replace(".", ""), out newThuong))
                            newThuong = 0;
                        if (!decimal.TryParse(txtKhauTru.Text.Replace(",", "").Replace(".", ""), out newKhauTru))
                            newKhauTru = 0;

                        decimal newTong = luongChinh + luongTC + newThuong - newKhauTru;
                        if (newTong < 0) newTong = 0;

                        DatabaseConnection.ExecuteNonQuery(@"
                            UPDATE BangLuong
                            SET ThuongThem = @Thuong, KhauTru = @KhauTru,
                                TongLuong = @Tong, GhiChu = @GhiChu
                            WHERE MaBangLuong = @MaBL",
                            p =>
                            {
                                p.AddWithValue("@Thuong", newThuong);
                                p.AddWithValue("@KhauTru", newKhauTru);
                                p.AddWithValue("@Tong", newTong);
                                p.AddWithValue("@GhiChu", txtGhiChu.Text.Trim());
                                p.AddWithValue("@MaBL", maBL);
                            });

                        MessageBox.Show("✅ Đã cập nhật thành công.", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dlg.Close();
                    };
                    pnlEdit.Controls.Add(btnLuu);
                    ey += 56;
                }
                else
                {
                    ey += 36;
                }

                pnlEdit.Height = ey + 16;
                dy += pnlEdit.Height + 24;

                // Spacer cuối để pnlBody scroll có padding bottom
                var pnlSpacer = new Panel
                {
                    Location = new Point(0, dy),
                    Size = new Size(1, 20),
                    BackColor = Color.Transparent,
                };
                pnlBody.Controls.Add(pnlSpacer);

                dlg.ShowDialog();
            }

            LoadDuLieu();
        }

        // ── Helpers cho dialog chi tiết ──

        private int ThemSectionHeader(Panel parent, string text, int x, int y, int w)
        {
            var lbl = new Label
            {
                Text = text,
                Font = AppFonts.H4,
                ForeColor = ColorScheme.TextMid,
                Location = new Point(x, y),
                AutoSize = true,
            };
            parent.Controls.Add(lbl);
            return y + 30;
        }

        private Panel ThemCard(Panel parent, int x, int y, int w, int h)
        {
            var card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, h > 0 ? h : 200),
                BackColor = Color.White,
            };
            card.Paint += (s, e2) =>
            {
                var g = e2.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = TaoRoundedRect(0, 0, card.Width - 1, card.Height - 1, 10))
                using (var pen = new Pen(BorderInput))
                    g.DrawPath(pen, path);
            };
            parent.Controls.Add(card);
            return card;
        }

        private int ThemDongThongTin(Panel parent, string label, string value, int x, int y, int w)
        {
            parent.Controls.Add(new Label
            {
                Text = label,
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextGray,
                Location = new Point(x, y),
                Size = new Size(180, 24),
            });
            parent.Controls.Add(new Label
            {
                Text = value,
                Font = AppFonts.BodyBold,
                ForeColor = ColorScheme.TextDark,
                Location = new Point(x + 180, y),
                Size = new Size(w - 180, 24),
            });
            return y + 32;
        }

        private int ThemDongTien(Panel parent, string label, string formula, decimal amount,
            int x, int y, int w, bool bold, Color? color = null, string prefix = "")
        {
            Color c = color ?? (bold ? GoldAccent : ColorScheme.TextDark);
            Font f = bold ? AppFonts.H4 : AppFonts.Body;
            Font fLabel = bold ? AppFonts.BodyBold : AppFonts.Body;

            parent.Controls.Add(new Label
            {
                Text = label,
                Font = fLabel,
                ForeColor = bold ? GoldAccent : ColorScheme.TextGray,
                Location = new Point(x, y),
                Size = new Size(220, 26),
            });

            string amountText = prefix + amount.ToString("#,##0") + "đ";
            var lblAmount = new Label
            {
                Text = amountText,
                Font = f,
                ForeColor = c,
                Location = new Point(x + 220, y),
                Size = new Size(w - 220, 26),
                TextAlign = ContentAlignment.MiddleRight,
            };
            parent.Controls.Add(lblAmount);

            return y + (bold ? 34 : 30);
        }
    }
}
