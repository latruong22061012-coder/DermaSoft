using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Form Cấu Hình Lương — Admin quản lý đơn giá, hệ số theo vai trò.
    /// Bảng CauHinhLuong hỗ trợ versioning theo NgayHieuLuc:
    ///   - Mỗi vai trò có thể có nhiều bản ghi, hệ thống lấy TOP 1 theo NgayHieuLuc DESC
    ///   - Admin có thể thêm cấu hình mới (ngày hiệu lực tương lai) hoặc sửa cấu hình hiện tại
    /// </summary>
    public partial class CauHinhLuongForm : Form
    {
        // ── Controls ──
        private Panel pnlContent;
        private DataGridView dgvCauHinh;

        // ── Màu đồng bộ ──
        private static readonly Color GoldAccent = Color.FromArgb(184, 138, 40);
        private static readonly Color BorderInput = ColorTranslator.FromHtml("#D1E8DC");

        private bool _dangVeLai = false;

        public CauHinhLuongForm()
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

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();

            int pad = 10;
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
                TextRenderer.DrawText(g, "⚙", new Font("Segoe UI Emoji", 12f), new Point(4, 11), Color.White);
                TextRenderer.DrawText(g, "CẤU HÌNH LƯƠNG THEO VAI TRÒ", AppFonts.H3, new Point(38, 10), ColorScheme.TextDark);
            };
            pnlContent.Controls.Add(pnlHeader);
            return y + 50;
        }

        // ══════════════════════════════════════════════════════════════════════
        // TOOLBAR
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

            var lblInfo = new Label
            {
                Text = "💡 Cấu hình lương áp dụng theo ngày hiệu lực. Hệ thống tự lấy cấu hình mới nhất khi tính lương.",
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextGray,
                AutoSize = true,
                Location = new Point(16, (toolbarH - 16) / 2),
            };
            pnlToolbar.Controls.Add(lblInfo);

            int rightEdge = w - 16;

            var btnThem = TaoNut("➕ Thêm cấu hình", rightEdge - 200, btnY, 200, btnH, ColorScheme.Primary);
            btnThem.Click += BtnThemCauHinh_Click;
            pnlToolbar.Controls.Add(btnThem);

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
        // KPI CARDS — 3 thẻ tổng hợp
        // ══════════════════════════════════════════════════════════════════════

        private Label lblKpiTongCauHinh;
        private Label lblKpiSoVaiTro;
        private Label lblKpiCapNhatGanNhat;

        private int TaoKpiCards(int x, int y, int w)
        {
            int cardW = (w - 12 * 2) / 3;
            int cardH = 80;

            string[] titles = { "Tổng cấu hình", "Vai trò", "Cập nhật gần nhất" };
            string[] icons = { "📋", "👥", "🕐" };
            Color[] colors = { ColorScheme.Primary, GoldAccent, ColorScheme.Info };

            Label[] kpiLabels = new Label[3];

            for (int i = 0; i < 3; i++)
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

            lblKpiTongCauHinh = kpiLabels[0];
            lblKpiSoVaiTro = kpiLabels[1];
            lblKpiCapNhatGanNhat = kpiLabels[2];

            return y + cardH + 12;
        }

        // ══════════════════════════════════════════════════════════════════════
        // DANH SÁCH CẤU HÌNH LƯƠNG
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

            dgvCauHinh = new DataGridView
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
            dgvCauHinh.AutoGenerateColumns = false;

            // Cột ẩn
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMaCH", DataPropertyName = "MaCauHinh", Visible = false });

            // Cột hiển thị
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "colVaiTro", DataPropertyName = "TenVaiTro", HeaderText = "Vai trò", FillWeight = 14F });
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLoai", DataPropertyName = "LoaiText", HeaderText = "Loại tính lương", FillWeight = 16F });
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDonGia", DataPropertyName = "DonGiaText", HeaderText = "Đơn giá", FillWeight = 14F });
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHeSoTC", DataPropertyName = "HeSoTCText", HeaderText = "Hệ số TC", FillWeight = 10F });
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHeSoLe", DataPropertyName = "HeSoLeText", HeaderText = "Hệ số lễ", FillWeight = 10F });
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGioChuan", DataPropertyName = "GioChuanText", HeaderText = "Giờ chuẩn/ngày", FillWeight = 12F });
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNgayHL", DataPropertyName = "NgayHLText", HeaderText = "Ngày hiệu lực", FillWeight = 12F });
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGhiChu", DataPropertyName = "GhiChu", HeaderText = "Ghi chú", FillWeight = 18F });
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTrangThai", DataPropertyName = "TrangThaiText", HeaderText = "Trạng thái", FillWeight = 12F });

            dgvCauHinh.CellDoubleClick += DgvCauHinh_CellDoubleClick;

            card.Controls.Add(dgvCauHinh);
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
                object val1 = DatabaseConnection.ExecuteScalar("SELECT COUNT(*) FROM CauHinhLuong");
                if (lblKpiTongCauHinh != null) lblKpiTongCauHinh.Text = val1?.ToString() ?? "0";

                object val2 = DatabaseConnection.ExecuteScalar("SELECT COUNT(DISTINCT MaVaiTro) FROM CauHinhLuong");
                if (lblKpiSoVaiTro != null) lblKpiSoVaiTro.Text = val2?.ToString() ?? "0";

                object val3 = DatabaseConnection.ExecuteScalar("SELECT MAX(NgayHieuLuc) FROM CauHinhLuong");
                if (lblKpiCapNhatGanNhat != null)
                {
                    if (val3 != null && val3 != DBNull.Value)
                        lblKpiCapNhatGanNhat.Text = Convert.ToDateTime(val3).ToString("dd/MM/yyyy");
                    else
                        lblKpiCapNhatGanNhat.Text = "—";
                }
            }
            catch
            {
                if (lblKpiTongCauHinh != null) lblKpiTongCauHinh.Text = "!";
                if (lblKpiSoVaiTro != null) lblKpiSoVaiTro.Text = "!";
                if (lblKpiCapNhatGanNhat != null) lblKpiCapNhatGanNhat.Text = "!";
            }
        }

        private void LoadDanhSach()
        {
            try
            {
                string sql = @"
                    SELECT
                        ch.MaCauHinh,
                        vt.TenVaiTro,
                        CASE ch.LoaiTinhLuong
                            WHEN 'THEO_BN'    THEN N'Theo bệnh nhân'
                            WHEN 'THEO_GIO'   THEN N'Theo giờ'
                            WHEN 'THEO_THANG' THEN N'Cố định/tháng'
                            ELSE ch.LoaiTinhLuong
                        END AS LoaiText,
                        FORMAT(ch.DonGia, N'#,##0') + N'đ' AS DonGiaText,
                        N'×' + FORMAT(ch.HeSoTangCa, N'0.#') AS HeSoTCText,
                        N'×' + FORMAT(ch.HeSoNgayLe, N'0.#') AS HeSoLeText,
                        CAST(ch.SoGioChuanNgay AS NVARCHAR) + N'h' AS GioChuanText,
                        FORMAT(ch.NgayHieuLuc, N'dd/MM/yyyy') AS NgayHLText,
                        ch.GhiChu,
                        CASE
                            WHEN ch.NgayHieuLuc <= GETDATE() THEN N'✅ Đang áp dụng'
                            ELSE N'🕐 Chờ hiệu lực'
                        END AS TrangThaiText
                    FROM CauHinhLuong ch
                    JOIN VaiTro vt ON ch.MaVaiTro = vt.MaVaiTro
                    ORDER BY vt.MaVaiTro, ch.NgayHieuLuc DESC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql);

                if (dgvCauHinh != null)
                {
                    dgvCauHinh.DataSource = dt;
                    TomauTrangThai();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải cấu hình lương:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TomauTrangThai()
        {
            if (dgvCauHinh == null) return;
            foreach (DataGridViewRow row in dgvCauHinh.Rows)
            {
                if (row.IsNewRow) continue;
                var cell = row.Cells["colTrangThai"];
                string tt = cell.Value?.ToString() ?? "";

                if (tt.Contains("Đang áp dụng"))
                {
                    cell.Style.BackColor = Color.FromArgb(220, 252, 231);
                    cell.Style.ForeColor = Color.FromArgb(21, 101, 52);
                }
                else
                {
                    cell.Style.BackColor = Color.FromArgb(254, 243, 199);
                    cell.Style.ForeColor = Color.FromArgb(146, 64, 14);
                }
                cell.Style.Font = AppFonts.Badge;
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cell.Style.SelectionBackColor = cell.Style.BackColor;
                cell.Style.SelectionForeColor = cell.Style.ForeColor;

                // Tô đậm đơn giá
                var cellDG = row.Cells["colDonGia"];
                cellDG.Style.Font = AppFonts.BodyBold;
                cellDG.Style.ForeColor = GoldAccent;
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // THÊM CẤU HÌNH MỚI
        // ══════════════════════════════════════════════════════════════════════

        private void BtnThemCauHinh_Click(object sender, EventArgs e)
        {
            HienDialogCauHinh(0, false);
        }

        // ══════════════════════════════════════════════════════════════════════
        // SỬA CẤU HÌNH — Double-click
        // ══════════════════════════════════════════════════════════════════════

        private void DgvCauHinh_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvCauHinh.Rows[e.RowIndex];
            int maCH = Convert.ToInt32(row.Cells["colMaCH"].Value);
            HienDialogCauHinh(maCH, true);
        }

        // ══════════════════════════════════════════════════════════════════════
        // DIALOG THÊM / SỬA CẤU HÌNH
        // ══════════════════════════════════════════════════════════════════════

        private void HienDialogCauHinh(int maCauHinh, bool laSua)
        {
            // Load dữ liệu cũ nếu sửa
            int maVaiTroCu = 0;
            string loaiCu = "THEO_GIO";
            decimal donGiaCu = 0, heSoTCCu = 1.5m, heSoLeCu = 2.0m;
            int gioChuanCu = 8, caChuanCu = 2;
            DateTime ngayHLCu = DateTime.Today;
            string ghiChuCu = "";

            if (laSua)
            {
                DataTable dtOld = DatabaseConnection.ExecuteQuery(@"
                    SELECT MaVaiTro, LoaiTinhLuong, DonGia, HeSoTangCa, HeSoNgayLe,
                           SoGioChuanNgay, SoCaChuanNgay, NgayHieuLuc, GhiChu
                    FROM CauHinhLuong WHERE MaCauHinh = @Id",
                    p => p.AddWithValue("@Id", maCauHinh));

                if (dtOld.Rows.Count == 0) return;
                DataRow r = dtOld.Rows[0];
                maVaiTroCu = Convert.ToInt32(r["MaVaiTro"]);
                loaiCu = r["LoaiTinhLuong"].ToString();
                donGiaCu = Convert.ToDecimal(r["DonGia"]);
                heSoTCCu = Convert.ToDecimal(r["HeSoTangCa"]);
                heSoLeCu = Convert.ToDecimal(r["HeSoNgayLe"]);
                gioChuanCu = Convert.ToInt32(r["SoGioChuanNgay"]);
                caChuanCu = Convert.ToInt32(r["SoCaChuanNgay"]);
                ngayHLCu = Convert.ToDateTime(r["NgayHieuLuc"]);
                ghiChuCu = r["GhiChu"]?.ToString() ?? "";
            }

            // Load danh sách vai trò
            DataTable dtVaiTro = DatabaseConnection.ExecuteQuery("SELECT MaVaiTro, TenVaiTro FROM VaiTro ORDER BY MaVaiTro");

            int dlgW = 620;
            int dlgH = 820;
            int pad = 28;

            using (var dlg = new Form
            {
                Text = laSua ? "Sửa cấu hình lương" : "Thêm cấu hình lương mới",
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

                // ── Header gold ──
                var pnlHeader = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 76,
                    BackColor = GoldAccent,
                };
                pnlHeader.Controls.Add(new Label
                {
                    Text = laSua ? "✏️ Sửa cấu hình lương" : "➕ Thêm cấu hình lương mới",
                    Font = AppFonts.H3,
                    ForeColor = Color.White,
                    AutoSize = false,
                    Size = new Size(clientW - 40, 30),
                    Location = new Point(20, 14),
                });
                pnlHeader.Controls.Add(new Label
                {
                    Text = "Cấu hình đơn giá, hệ số theo vai trò",
                    Font = AppFonts.Body,
                    ForeColor = Color.FromArgb(210, 255, 255, 255),
                    AutoSize = false,
                    Size = new Size(clientW - 40, 22),
                    Location = new Point(20, 46),
                });
                dlg.Controls.Add(pnlHeader);

                // ── Scrollable body ──
                var pnlBody = new Panel
                {
                    Location = new Point(0, 76),
                    Size = new Size(clientW, clientH - 76),
                    AutoScroll = true,
                    BackColor = ColorScheme.Background,
                };
                dlg.Controls.Add(pnlBody);

                int scrollBarW = SystemInformation.VerticalScrollBarWidth;
                int cardW = clientW - pad * 2 - scrollBarW;
                int innerPad = 20;
                int inputW = cardW - 98;
                int colGap = 24;
                int halfW = (inputW - colGap) / 2;

                int dy = 16;

                // ════════════════════════════════════════
                // CARD 1: Thông tin cơ bản
                // ════════════════════════════════════════
                dy = ThemSectionHeaderCH(pnlBody, "📋 THÔNG TIN CƠ BẢN", pad, dy, cardW);

                var card1 = ThemCardCH(pnlBody, pad, dy, cardW, 0);
                int cy = 20;

                // Vai trò
                card1.Controls.Add(new Label { Text = "Vai trò:", Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextDark, Location = new Point(innerPad, cy), AutoSize = true });
                cy += 28;
                var cboVaiTro = new Guna2ComboBox
                {
                    Location = new Point(innerPad, cy),
                    Size = new Size(inputW, 44),
                    Font = AppFonts.Body,
                    FillColor = Color.White,
                    BorderColor = BorderInput,
                    BorderRadius = 8,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                };
                cboVaiTro.DataSource = dtVaiTro;
                cboVaiTro.DisplayMember = "TenVaiTro";
                cboVaiTro.ValueMember = "MaVaiTro";
                if (laSua) cboVaiTro.SelectedValue = maVaiTroCu;
                card1.Controls.Add(cboVaiTro);
                cy += 56;

                // Loại tính lương
                card1.Controls.Add(new Label { Text = "Loại tính lương:", Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextDark, Location = new Point(innerPad, cy), AutoSize = true });
                cy += 28;
                var cboLoai = new Guna2ComboBox
                {
                    Location = new Point(innerPad, cy),
                    Size = new Size(inputW, 44),
                    Font = AppFonts.Body,
                    FillColor = Color.White,
                    BorderColor = BorderInput,
                    BorderRadius = 8,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                };
                cboLoai.Items.AddRange(new object[] { "THEO_BN", "THEO_GIO", "THEO_THANG" });
                cboLoai.SelectedItem = loaiCu;
                card1.Controls.Add(cboLoai);
                cy += 56;

                // Đơn giá
                card1.Controls.Add(new Label { Text = "Đơn giá (đ):", Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextDark, Location = new Point(innerPad, cy), AutoSize = true });
                cy += 28;
                var txtDonGia = new Guna2TextBox
                {
                    Text = donGiaCu.ToString("#,##0"),
                    Location = new Point(innerPad, cy),
                    Size = new Size(inputW, 44),
                    Font = AppFonts.Body,
                    FillColor = Color.White,
                    BorderColor = BorderInput,
                    BorderRadius = 8,
                };
                card1.Controls.Add(txtDonGia);
                cy += 56;

                card1.Height = cy + 12;
                dy += card1.Height + 16;

                // ════════════════════════════════════════
                // CARD 2: Hệ số & Ngưỡng
                // ════════════════════════════════════════
                dy = ThemSectionHeaderCH(pnlBody, "⚙ HỆ SỐ & NGƯỠNG", pad, dy, cardW);

                var card2 = ThemCardCH(pnlBody, pad, dy, cardW, 0);
                cy = 20;

                // Hệ số tăng ca + Hệ số ngày lễ (2 cột)
                card2.Controls.Add(new Label { Text = "Hệ số tăng ca:", Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextDark, Location = new Point(innerPad, cy), AutoSize = true });
                card2.Controls.Add(new Label { Text = "Hệ số ngày lễ:", Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextDark, Location = new Point(innerPad + halfW + colGap, cy), AutoSize = true });
                cy += 28;
                var txtHeSoTC = new Guna2TextBox
                {
                    Text = heSoTCCu.ToString("0.0"),
                    Location = new Point(innerPad, cy),
                    Size = new Size(halfW, 44),
                    Font = AppFonts.Body,
                    FillColor = Color.White,
                    BorderColor = BorderInput,
                    BorderRadius = 8,
                };
                card2.Controls.Add(txtHeSoTC);
                var txtHeSoLe = new Guna2TextBox
                {
                    Text = heSoLeCu.ToString("0.0"),
                    Location = new Point(innerPad + halfW + colGap, cy),
                    Size = new Size(halfW, 44),
                    Font = AppFonts.Body,
                    FillColor = Color.White,
                    BorderColor = BorderInput,
                    BorderRadius = 8,
                };
                card2.Controls.Add(txtHeSoLe);
                cy += 56;

                // Giờ chuẩn/ngày + Ca chuẩn/ngày (2 cột)
                card2.Controls.Add(new Label { Text = "Giờ chuẩn/ngày:", Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextDark, Location = new Point(innerPad, cy), AutoSize = true });
                card2.Controls.Add(new Label { Text = "Ca chuẩn/ngày:", Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextDark, Location = new Point(innerPad + halfW + colGap, cy), AutoSize = true });
                cy += 28;
                var txtGioChuan = new Guna2TextBox
                {
                    Text = gioChuanCu.ToString(),
                    Location = new Point(innerPad, cy),
                    Size = new Size(halfW, 44),
                    Font = AppFonts.Body,
                    FillColor = Color.White,
                    BorderColor = BorderInput,
                    BorderRadius = 8,
                };
                card2.Controls.Add(txtGioChuan);
                var txtCaChuan = new Guna2TextBox
                {
                    Text = caChuanCu.ToString(),
                    Location = new Point(innerPad + halfW + colGap, cy),
                    Size = new Size(halfW, 44),
                    Font = AppFonts.Body,
                    FillColor = Color.White,
                    BorderColor = BorderInput,
                    BorderRadius = 8,
                };
                card2.Controls.Add(txtCaChuan);
                cy += 56;

                card2.Height = cy + 12;
                dy += card2.Height + 16;

                // ════════════════════════════════════════
                // CARD 3: Hiệu lực & Ghi chú
                // ════════════════════════════════════════
                dy = ThemSectionHeaderCH(pnlBody, "📅 HIỆU LỰC & GHI CHÚ", pad, dy, cardW);

                var card3 = ThemCardCH(pnlBody, pad, dy, cardW, 0);
                cy = 20;

                // Ngày hiệu lực
                card3.Controls.Add(new Label { Text = "Ngày hiệu lực:", Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextDark, Location = new Point(innerPad, cy), AutoSize = true });
                cy += 28;
                var dtpNgayHL = new Guna2DateTimePicker
                {
                    Location = new Point(innerPad, cy),
                    Size = new Size(inputW, 44),
                    CustomFormat = "dd/MM/yyyy",
                    Format = DateTimePickerFormat.Custom,
                    Font = AppFonts.Body,
                    Value = ngayHLCu,
                    FillColor = Color.White,
                    BorderColor = BorderInput,
                    BorderRadius = 8,
                };
                card3.Controls.Add(dtpNgayHL);
                cy += 56;

                // Ghi chú
                card3.Controls.Add(new Label { Text = "Ghi chú:", Font = AppFonts.BodyBold, ForeColor = ColorScheme.TextDark, Location = new Point(innerPad, cy), AutoSize = true });
                cy += 28;
                var txtGhiChu = new Guna2TextBox
                {
                    Text = ghiChuCu,
                    Location = new Point(innerPad, cy),
                    Size = new Size(inputW, 44),
                    Font = AppFonts.Body,
                    FillColor = Color.White,
                    BorderColor = BorderInput,
                    BorderRadius = 8,
                };
                card3.Controls.Add(txtGhiChu);
                cy += 56;

                card3.Height = cy + 12;
                dy += card3.Height + 20;

                // ════════════════════════════════════════
                // BUTTONS
                // ════════════════════════════════════════
                int btnW = laSua ? (cardW - colGap) / 2 : cardW;

                var btnLuu = new Guna2Button
                {
                    Text = laSua ? "💾 Cập nhật" : "💾 Thêm mới",
                    Font = AppFonts.BodyBold,
                    ForeColor = Color.White,
                    FillColor = ColorScheme.Primary,
                    BorderRadius = 10,
                    Size = new Size(btnW, 48),
                    Location = new Point(pad, dy),
                    Cursor = Cursors.Hand,
                };
                pnlBody.Controls.Add(btnLuu);

                Guna2Button btnXoa = null;
                if (laSua)
                {
                    btnXoa = new Guna2Button
                    {
                        Text = "🗑️ Xóa cấu hình",
                        Font = AppFonts.BodyBold,
                        ForeColor = Color.White,
                        FillColor = ColorScheme.Danger,
                        BorderRadius = 10,
                        Size = new Size(btnW, 48),
                        Location = new Point(pad + btnW + colGap, dy),
                        Cursor = Cursors.Hand,
                    };
                    btnXoa.Click += (s2, e2) =>
                    {
                        var xacNhan = MessageBox.Show(
                            "Xóa cấu hình lương này?\nHành động không thể hoàn tác.",
                            "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (xacNhan != DialogResult.Yes) return;

                        try
                        {
                            DatabaseConnection.ExecuteNonQuery(
                                "DELETE FROM CauHinhLuong WHERE MaCauHinh = @Id",
                                p => p.AddWithValue("@Id", maCauHinh));

                            MessageBox.Show("✅ Đã xóa cấu hình.", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dlg.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xóa:\n" + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };
                    pnlBody.Controls.Add(btnXoa);
                }

                btnLuu.Click += (s2, e2) =>
                {
                    // Validate
                    if (cboVaiTro.SelectedValue == null)
                    {
                        MessageBox.Show("Vui lòng chọn vai trò.", "Thiếu thông tin",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (cboLoai.SelectedItem == null)
                    {
                        MessageBox.Show("Vui lòng chọn loại tính lương.", "Thiếu thông tin",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    decimal donGia;
                    if (!decimal.TryParse(txtDonGia.Text.Replace(",", "").Replace(".", ""), out donGia) || donGia <= 0)
                    {
                        MessageBox.Show("Đơn giá phải là số > 0.", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDonGia.Focus();
                        return;
                    }

                    decimal heSoTC;
                    if (!decimal.TryParse(txtHeSoTC.Text, out heSoTC) || heSoTC < 1)
                    {
                        MessageBox.Show("Hệ số tăng ca phải ≥ 1.0", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtHeSoTC.Focus();
                        return;
                    }

                    decimal heSoLe;
                    if (!decimal.TryParse(txtHeSoLe.Text, out heSoLe) || heSoLe < 1)
                    {
                        MessageBox.Show("Hệ số ngày lễ phải ≥ 1.0", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtHeSoLe.Focus();
                        return;
                    }

                    int gioChuan;
                    if (!int.TryParse(txtGioChuan.Text, out gioChuan) || gioChuan < 0)
                    {
                        MessageBox.Show("Giờ chuẩn/ngày phải là số ≥ 0.", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtGioChuan.Focus();
                        return;
                    }

                    int caChuan;
                    if (!int.TryParse(txtCaChuan.Text, out caChuan) || caChuan < 0)
                    {
                        MessageBox.Show("Ca chuẩn/ngày phải là số ≥ 0.", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCaChuan.Focus();
                        return;
                    }

                    int maVT = Convert.ToInt32(cboVaiTro.SelectedValue);
                    string loai = cboLoai.SelectedItem.ToString();
                    DateTime ngayHL = dtpNgayHL.Value.Date;
                    string ghiChu = txtGhiChu.Text.Trim();

                    try
                    {
                        if (laSua)
                        {
                            DatabaseConnection.ExecuteNonQuery(@"
                                UPDATE CauHinhLuong SET
                                    MaVaiTro = @MaVT, LoaiTinhLuong = @Loai, DonGia = @DonGia,
                                    HeSoTangCa = @HeSoTC, HeSoNgayLe = @HeSoLe,
                                    SoGioChuanNgay = @GioChuan, SoCaChuanNgay = @CaChuan,
                                    NgayHieuLuc = @NgayHL, GhiChu = @GhiChu
                                WHERE MaCauHinh = @Id",
                                p =>
                                {
                                    p.AddWithValue("@MaVT", maVT);
                                    p.AddWithValue("@Loai", loai);
                                    p.AddWithValue("@DonGia", donGia);
                                    p.AddWithValue("@HeSoTC", heSoTC);
                                    p.AddWithValue("@HeSoLe", heSoLe);
                                    p.AddWithValue("@GioChuan", gioChuan);
                                    p.AddWithValue("@CaChuan", caChuan);
                                    p.AddWithValue("@NgayHL", ngayHL);
                                    p.AddWithValue("@GhiChu", ghiChu);
                                    p.AddWithValue("@Id", maCauHinh);
                                });

                            MessageBox.Show("✅ Đã cập nhật cấu hình.", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            DatabaseConnection.ExecuteNonQuery(@"
                                INSERT INTO CauHinhLuong
                                    (MaVaiTro, LoaiTinhLuong, DonGia, HeSoTangCa, HeSoNgayLe,
                                     SoGioChuanNgay, SoCaChuanNgay, NgayHieuLuc, GhiChu)
                                VALUES
                                    (@MaVT, @Loai, @DonGia, @HeSoTC, @HeSoLe,
                                     @GioChuan, @CaChuan, @NgayHL, @GhiChu)",
                                p =>
                                {
                                    p.AddWithValue("@MaVT", maVT);
                                    p.AddWithValue("@Loai", loai);
                                    p.AddWithValue("@DonGia", donGia);
                                    p.AddWithValue("@HeSoTC", heSoTC);
                                    p.AddWithValue("@HeSoLe", heSoLe);
                                    p.AddWithValue("@GioChuan", gioChuan);
                                    p.AddWithValue("@CaChuan", caChuan);
                                    p.AddWithValue("@NgayHL", ngayHL);
                                    p.AddWithValue("@GhiChu", ghiChu);
                                });

                            MessageBox.Show("✅ Đã thêm cấu hình mới.", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        dlg.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi lưu cấu hình:\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                // Spacer cuối
                dy += 58;
                pnlBody.Controls.Add(new Panel
                {
                    Location = new Point(0, dy),
                    Size = new Size(1, 20),
                    BackColor = Color.Transparent,
                });

                dlg.ShowDialog();
            }

            LoadDuLieu();
        }

        // ── Helpers cho dialog cấu hình ──

        private int ThemSectionHeaderCH(Panel parent, string text, int x, int y, int w)
        {
            parent.Controls.Add(new Label
            {
                Text = text,
                Font = AppFonts.H4,
                ForeColor = ColorScheme.TextMid,
                Location = new Point(x, y),
                AutoSize = true,
            });
            return y + 28;
        }

        private Panel ThemCardCH(Panel parent, int x, int y, int w, int h)
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
    }
}
