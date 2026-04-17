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
    /// Form Cài Đặt Hệ Thống — 6 chức năng thực tế cho admin.
    /// </summary>
    public partial class SettingsForm : Form
    {
        // ── Controls ──
        private Panel pnlContent;

        // 1. Thông tin phòng khám
        private Guna2TextBox txtTenPK, txtDiaChi, txtSDT, txtEmail, txtWebsite, txtSlogan;
        // 2. Giờ làm việc
        private Guna2TextBox txtGioMo, txtGioDong, txtLichTuan;
        // 3. Ngưỡng cảnh báo kho (managed in TaoSection3_CanhBaoKho)
        // 4. Mật khẩu mặc định
        private Guna2TextBox txtMKMacDinh;
        // 5. Sao lưu DB
        private Guna2TextBox txtBackupPath;
        // 6. Kiểm tra kết nối
        private Label lblConnStatus;

        // Màu đồng bộ
        private static readonly Color GoldAccent = Color.FromArgb(184, 138, 40);
        private static readonly Color BorderInput = ColorTranslator.FromHtml("#D1E8DC");
        private static readonly Color InputBg = ColorTranslator.FromHtml("#FCFFFE");

        private bool _dangVeLai = false;

        public SettingsForm()
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

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();

            int pad = 16;
            int gap = 16;
            int contentW = pnlContent.ClientSize.Width - pad * 2;

            int y = pad;

            y = TaoHeader(pad, y, contentW);

            // ── 2 cột layout ──
            int colW = (contentW - gap) / 2;

            // Tính chiều cao khả dụng cho 2 cột (trừ header + padding)
            int availH = Math.Max(500, pnlContent.ClientSize.Height - y - pad);
            // Cột trái: S1(ThôngTinPK 46%) + S4(MậtKhẩu 22%) + S5(SaoLưu 32%)
            int s1H = (int)(availH * 0.46);
            int s4H = (int)(availH * 0.22);
            int s5H = availH - s1H - s4H - gap * 2;
            // Cột phải: S2(GiờLV 26%) + S3(NgưỡngKho 46%) + S6(KếtNối 28%)
            int s2H = (int)(availH * 0.26);
            int s3H = (int)(availH * 0.46);
            int s6H = availH - s2H - s3H - gap * 2;

            // Cột trái: S1 → S4 → S5
            int yL = y;
            TaoSection1_ThongTinPK(pad, yL, colW, s1H); yL += s1H + gap;
            TaoSection4_MatKhauMacDinh(pad, yL, colW, s4H); yL += s4H + gap;
            TaoSection5_SaoLuuDB(pad, yL, colW, s5H);

            // Cột phải: S2 → S3 → S6
            int yR = y;
            int xR = pad + colW + gap;
            TaoSection2_GioLamViec(xR, yR, colW, s2H); yR += s2H + gap;
            TaoSection3_CanhBaoKho(xR, yR, colW, s3H); yR += s3H + gap;
            TaoSection6_KiemTraKetNoi(xR, yR, colW, s6H);

            // Load dữ liệu
            AppSettings.Load();
            LoadThongTinPK();

            pnlContent.ResumeLayout();
            _dangVeLai = false;
        }

        // ══════════════════════════════════════════
        // HEADER
        // ══════════════════════════════════════════

        private int TaoHeader(int x, int y, int w)
        {
            var pnlHeader = new Panel { Location = new Point(x, y), Size = new Size(w, 44) };
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
                Text = "⚙️  Cài Đặt Hệ Thống",
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
        // SECTION 1: THÔNG TIN PHÒNG KHÁM
        // ══════════════════════════════════════════

        private void TaoSection1_ThongTinPK(int x, int y, int w, int cardH)
        {
            var card = TaoCard(x, y, w, cardH);
            int cy = 16, cx = 16;
            int inputW = w - 32;
            int halfW = (inputW - 10) / 2;

            card.Controls.Add(TaoSectionTitle("🏥  Thông Tin Phòng Khám", cx, cy));
            cy += 32;

            card.Controls.Add(TaoLabel("Tên phòng khám", cx, cy));
            cy += 18;
            txtTenPK = TaoInput(cx, cy, inputW, "DermaSoft Clinic");
            card.Controls.Add(txtTenPK);
            cy += 40;

            card.Controls.Add(TaoLabel("Địa chỉ", cx, cy));
            cy += 18;
            txtDiaChi = TaoInput(cx, cy, inputW, "123 Nguyễn Huệ, Q1, TP.HCM");
            card.Controls.Add(txtDiaChi);
            cy += 40;

            card.Controls.Add(TaoLabel("SĐT", cx, cy));
            card.Controls.Add(TaoLabel("Email", cx + halfW + 10, cy));
            cy += 18;
            txtSDT = TaoInput(cx, cy, halfW, "028 1234 5678");
            card.Controls.Add(txtSDT);
            txtEmail = TaoInput(cx + halfW + 10, cy, halfW, "info@darmaclinic.vn");
            card.Controls.Add(txtEmail);
            cy += 40;

            card.Controls.Add(TaoLabel("Website", cx, cy));
            card.Controls.Add(TaoLabel("Slogan", cx + halfW + 10, cy));
            cy += 18;
            txtWebsite = TaoInput(cx, cy, halfW, "darmaclinic.vn");
            card.Controls.Add(txtWebsite);
            txtSlogan = TaoInput(cx + halfW + 10, cy, halfW, "Chăm sóc da — Tỏa sáng cuộc sống");
            card.Controls.Add(txtSlogan);
            cy += 42;

            var btnLuu1 = TaoBtnPrimary("💾 Lưu Thông Tin", cx, cy, inputW);
            btnLuu1.Click += BtnLuuThongTinPK_Click;
            card.Controls.Add(btnLuu1);

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // SECTION 2: GIỜ LÀM VIỆC
        // ══════════════════════════════════════════

        private void TaoSection2_GioLamViec(int x, int y, int w, int cardH)
        {
            var card = TaoCard(x, y, w, cardH);
            int cy = 16, cx = 16;
            int inputW = w - 32;
            int halfW = (inputW - 10) / 2;

            card.Controls.Add(TaoSectionTitle("🕐  Giờ Làm Việc", cx, cy));
            cy += 32;

            card.Controls.Add(TaoLabel("Giờ mở cửa", cx, cy));
            card.Controls.Add(TaoLabel("Giờ đóng cửa", cx + halfW + 10, cy));
            cy += 18;
            txtGioMo = TaoInput(cx, cy, halfW, "08:00");
            card.Controls.Add(txtGioMo);
            txtGioDong = TaoInput(cx + halfW + 10, cy, halfW, "17:00");
            card.Controls.Add(txtGioDong);
            cy += 40;

            card.Controls.Add(TaoLabel("Lịch làm việc hàng tuần", cx, cy));
            cy += 18;
            txtLichTuan = TaoInput(cx, cy, inputW, "Thứ 2 — Thứ 7");
            card.Controls.Add(txtLichTuan);
            cy += 42;

            var btnLuu2 = TaoBtnPrimary("💾 Lưu Giờ Làm Việc", cx, cy, inputW);
            btnLuu2.Click += BtnLuuGioLamViec_Click;
            card.Controls.Add(btnLuu2);

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // SECTION 3: NGƯỠNG CẢNH BÁO KHO
        // ══════════════════════════════════════════

        private DataGridView _dgvNguong;
        private Guna2ComboBox _cboThemDVT;

        private void TaoSection3_CanhBaoKho(int x, int y, int w, int cardH)
        {
            var card = TaoCard(x, y, w, cardH);
            int cy = 16, cx = 16;
            int inputW = w - 32;

            card.Controls.Add(TaoSectionTitle("📦  Ngưỡng Cảnh Báo Tồn Kho Theo Đơn Vị", cx, cy));
            cy += 32;

            card.Controls.Add(TaoLabel("Mỗi đơn vị tính có ngưỡng riêng (VD: Viên > Chai)", cx, cy));
            cy += 22;

            // DataGridView ngưỡng theo đơn vị
            int dgvH = cardH - cy - 90;
            if (dgvH < 80) dgvH = 80;

            _dgvNguong = new DataGridView
            {
                Location = new Point(cx, cy),
                Size = new Size(inputW, dgvH),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = ColorScheme.Border,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeight = 32,
                RowTemplate = { Height = 30 },
                EnableHeadersVisualStyles = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = AppFonts.Body,
                    ForeColor = ColorScheme.TextDark,
                    SelectionBackColor = ColorScheme.PrimaryPale,
                    SelectionForeColor = ColorScheme.TextDark,
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = AppFonts.BodyBold,
                    ForeColor = ColorScheme.TextMid,
                    BackColor = Color.FromArgb(249, 250, 251),
                },
            };
            _dgvNguong.AutoGenerateColumns = false;
            _dgvNguong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDVT", HeaderText = "Đơn vị tính", ReadOnly = true, FillWeight = 35 });
            _dgvNguong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colThap", HeaderText = "Mức Thấp (≤)", FillWeight = 30 });
            _dgvNguong.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNguy", HeaderText = "Mức Nguy hiểm (≤)", FillWeight = 35 });

            LoadDgvNguong();
            card.Controls.Add(_dgvNguong);
            cy += dgvH + 6;

            // Thêm đơn vị tính mới
            int addW = (inputW - 10) / 2;
            _cboThemDVT = new Guna2ComboBox
            {
                Location = new Point(cx, cy),
                Size = new Size(addW, 36),
                Font = AppFonts.Body,
                FillColor = InputBg,
                BorderColor = BorderInput,
                BorderRadius = 8,
                DropDownStyle = ComboBoxStyle.DropDown,
            };
            _cboThemDVT.Items.AddRange(new object[] { "Viên", "Chai", "Tuýp", "Lọ", "Hộp", "Ống", "Gói", "Vỉ", "Túi", "Cuộn" });
            card.Controls.Add(_cboThemDVT);

            var btnThem = TaoBtnPrimary("➕ Thêm ĐVT", cx + addW + 10, cy, addW);
            btnThem.Click += (s, e) =>
            {
                string dvt = _cboThemDVT.Text.Trim();
                if (string.IsNullOrEmpty(dvt)) return;

                // Kiểm tra trùng
                foreach (DataGridViewRow row in _dgvNguong.Rows)
                    if (row.Cells["colDVT"].Value?.ToString() == dvt) { _cboThemDVT.Text = ""; return; }

                int[] macDinh = AppSettings.NguongMacDinhTheoDonVi.ContainsKey(dvt)
                    ? AppSettings.NguongMacDinhTheoDonVi[dvt]
                    : new[] { AppSettings.NguongThap, AppSettings.NguongNguyHiem };
                _dgvNguong.Rows.Add(dvt, macDinh[0].ToString(), macDinh[1].ToString());
                _cboThemDVT.Text = "";
            };
            card.Controls.Add(btnThem);
            cy += 42;

            var btnLuu3 = TaoBtnPrimary("💾 Lưu Ngưỡng", cx, cy, inputW);
            btnLuu3.Click += BtnLuuNguong_Click;
            card.Controls.Add(btnLuu3);

            pnlContent.Controls.Add(card);
        }

        private void LoadDgvNguong()
        {
            if (_dgvNguong == null) return;
            _dgvNguong.Rows.Clear();

            var dsDVT = AppSettings.LayTatCaDonViTinh();
            foreach (string dvt in dsDVT)
            {
                int[] nguong = AppSettings.LayNguong(dvt);
                _dgvNguong.Rows.Add(dvt, nguong[0].ToString(), nguong[1].ToString());
            }
        }

        private void BtnLuuNguong_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (DataGridViewRow row in _dgvNguong.Rows)
            {
                if (row.IsNewRow) continue;
                string dvt = row.Cells["colDVT"].Value?.ToString() ?? "";
                if (string.IsNullOrEmpty(dvt)) continue;

                int valThap, valNguy;
                if (!int.TryParse(row.Cells["colThap"].Value?.ToString(), out valThap) || valThap < 0)
                {
                    MessageBox.Show($"Mức Thấp của \"{dvt}\" phải là số nguyên ≥ 0.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!int.TryParse(row.Cells["colNguy"].Value?.ToString(), out valNguy) || valNguy < 0)
                {
                    MessageBox.Show($"Mức Nguy hiểm của \"{dvt}\" phải là số nguyên ≥ 0.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                AppSettings.LuuNguongDonVi(dvt, valThap, valNguy);
                count++;
            }

            // Cập nhật fallback mặc định (dùng giá trị đầu tiên hoặc giữ nguyên)
            AppSettings.Save();

            string msg = $"✅ Đã lưu ngưỡng cho {count} đơn vị tính!\n\n";
            foreach (DataGridViewRow row in _dgvNguong.Rows)
            {
                if (row.IsNewRow) continue;
                string dvt = row.Cells["colDVT"].Value?.ToString() ?? "";
                msg += $"• {dvt}: Thấp ≤ {row.Cells["colThap"].Value}, Nguy hiểm ≤ {row.Cells["colNguy"].Value}\n";
            }
            MessageBox.Show(msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ══════════════════════════════════════════
        // SECTION 4: MẬT KHẨU MẶC ĐỊNH
        // ══════════════════════════════════════════

        private void TaoSection4_MatKhauMacDinh(int x, int y, int w, int cardH)
        {
            var card = TaoCard(x, y, w, cardH);
            int cy = 16, cx = 16;
            int inputW = w - 32;

            card.Controls.Add(TaoSectionTitle("🔑  Mật Khẩu Mặc Định Nhân Viên", cx, cy));
            cy += 32;

            card.Controls.Add(TaoLabel("Mật khẩu tạm khi tạo/reset tài khoản", cx, cy));
            cy += 18;
            txtMKMacDinh = TaoInput(cx, cy, inputW, "Temp@2026");
            txtMKMacDinh.Text = AppSettings.MatKhauMacDinh;
            card.Controls.Add(txtMKMacDinh);
            cy += 42;

            var btnLuu4 = TaoBtnPrimary("💾 Lưu Mật Khẩu", cx, cy, inputW);
            btnLuu4.Click += (s, e) =>
            {
                if (txtMKMacDinh.Text.Trim().Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải ít nhất 6 ký tự.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                AppSettings.MatKhauMacDinh = txtMKMacDinh.Text.Trim();
                AppSettings.Save();
                MessageBox.Show("Đã lưu mật khẩu mặc định: " + AppSettings.MatKhauMacDinh,
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            card.Controls.Add(btnLuu4);

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // SECTION 5: SAO LƯU DATABASE
        // ══════════════════════════════════════════

        private void TaoSection5_SaoLuuDB(int x, int y, int w, int cardH)
        {
            var card = TaoCard(x, y, w, cardH);
            int cy = 16, cx = 16;
            int inputW = w - 32;

            card.Controls.Add(TaoSectionTitle("💾  Sao Lưu Cơ Sở Dữ Liệu", cx, cy));
            cy += 32;

            card.Controls.Add(TaoLabel("Đường dẫn sao lưu", cx, cy));
            cy += 18;
            int browseW = 80;
            txtBackupPath = TaoInput(cx, cy, inputW - browseW - 8, "C:\\Backup\\DERMASOFT.bak");
            txtBackupPath.Text = "C:\\Backup\\DERMASOFT.bak";
            card.Controls.Add(txtBackupPath);

            var btnBrowse = new Guna2Button
            {
                Text = "📂", Font = AppFonts.Body,
                ForeColor = ColorScheme.PrimaryDark, FillColor = ColorScheme.PrimaryPale,
                BorderRadius = 8, BorderColor = ColorScheme.PrimaryLight, BorderThickness = 1,
                Location = new Point(cx + inputW - browseW, cy), Size = new Size(browseW, 36),
                Cursor = Cursors.Hand,
            };
            btnBrowse.Click += (s, e) =>
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Backup Files (*.bak)|*.bak";
                    sfd.FileName = "DERMASOFT_" + DateTime.Now.ToString("yyyyMMdd") + ".bak";
                    if (sfd.ShowDialog() == DialogResult.OK)
                        txtBackupPath.Text = sfd.FileName;
                }
            };
            card.Controls.Add(btnBrowse);
            cy += 42;

            var btnBackup = TaoBtnWarning("⚡ Sao Lưu Ngay", cx, cy, inputW);
            btnBackup.Click += BtnBackupDB_Click;
            card.Controls.Add(btnBackup);

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // SECTION 6: KIỂM TRA KẾT NỐI
        // ══════════════════════════════════════════

        private void TaoSection6_KiemTraKetNoi(int x, int y, int w, int cardH)
        {
            var card = TaoCard(x, y, w, cardH);
            int cy = 16, cx = 16;
            int inputW = w - 32;

            card.Controls.Add(TaoSectionTitle("🔌  Kiểm Tra Kết Nối Database", cx, cy));
            cy += 32;

            lblConnStatus = new Label
            {
                Text = "Chưa kiểm tra",
                Font = AppFonts.Body, ForeColor = ColorScheme.TextGray,
                Location = new Point(cx, cy), Size = new Size(inputW, 20),
                BackColor = Color.Transparent,
            };
            card.Controls.Add(lblConnStatus);
            cy += 28;

            // Server info
            card.Controls.Add(new Label
            {
                Text = "Server: localhost  |  Database: DERMASOFT",
                Font = AppFonts.Small, ForeColor = ColorScheme.TextLight,
                Location = new Point(cx, cy), AutoSize = true, BackColor = Color.Transparent,
            });
            cy += 28;

            var btnTest = TaoBtnPrimary("🔍 Kiểm Tra Kết Nối", cx, cy, (inputW - 10) / 2);
            btnTest.Click += BtnTestConnection_Click;
            card.Controls.Add(btnTest);

            var btnCleanup = TaoBtnDanger("🗑️ Dọn Dẹp Dữ Liệu Rác", cx + (inputW - 10) / 2 + 10, cy, (inputW - 10) / 2);
            btnCleanup.Click += BtnCleanup_Click;
            card.Controls.Add(btnCleanup);

            pnlContent.Controls.Add(card);
        }

        // ══════════════════════════════════════════
        // LOAD THÔNG TIN PHÒNG KHÁM
        // ══════════════════════════════════════════

        private void LoadThongTinPK()
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(
                    "SELECT TOP 1 TenPhongKham, DiaChi, SoDienThoai, Email, Website, Slogan, GioMoCua, GioDongCua, LichLamViecHangTuan FROM ThongTinPhongKham ORDER BY MaThongTin DESC", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (txtTenPK != null) txtTenPK.Text = reader.IsDBNull(0) ? "" : reader.GetString(0);
                        if (txtDiaChi != null) txtDiaChi.Text = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        if (txtSDT != null) txtSDT.Text = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        if (txtEmail != null) txtEmail.Text = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        if (txtWebsite != null) txtWebsite.Text = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        if (txtSlogan != null) txtSlogan.Text = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        if (txtGioMo != null) txtGioMo.Text = reader.IsDBNull(6) ? "08:00" : reader.GetTimeSpan(6).ToString(@"hh\:mm");
                        if (txtGioDong != null) txtGioDong.Text = reader.IsDBNull(7) ? "17:00" : reader.GetTimeSpan(7).ToString(@"hh\:mm");
                        if (txtLichTuan != null) txtLichTuan.Text = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadThongTinPK error: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════
        // BUTTON EVENTS
        // ══════════════════════════════════════════

        private void BtnLuuThongTinPK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenPK.Text.Trim()))
            {
                MessageBox.Show("Tên phòng khám không được để trống.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenPK.Focus();
                return;
            }

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    // Lấy MaThongTin của record hiện tại (nếu có)
                    int maThongTin = 0;
                    using (var cmd = new SqlCommand("SELECT TOP 1 MaThongTin FROM ThongTinPhongKham ORDER BY MaThongTin DESC", conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            maThongTin = Convert.ToInt32(result);
                    }

                    if (maThongTin == 0)
                    {
                        using (var cmd = new SqlCommand(
                            @"INSERT INTO ThongTinPhongKham (TenPhongKham, DiaChi, SoDienThoai, Email, Website, Slogan)
                              VALUES (@Ten, @DC, @SDT, @Email, @Web, @Slogan)", conn))
                        {
                            cmd.Parameters.AddWithValue("@Ten", txtTenPK.Text.Trim());
                            cmd.Parameters.AddWithValue("@DC", txtDiaChi.Text.Trim());
                            cmd.Parameters.AddWithValue("@SDT", txtSDT.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                            cmd.Parameters.AddWithValue("@Web", txtWebsite.Text.Trim());
                            cmd.Parameters.AddWithValue("@Slogan", txtSlogan.Text.Trim());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (var cmd = new SqlCommand(
                            @"UPDATE ThongTinPhongKham
                              SET TenPhongKham = @Ten,
                                  DiaChi = @DC,
                                  SoDienThoai = @SDT,
                                  Email = @Email,
                                  Website = @Web,
                                  Slogan = @Slogan,
                                  DatCapNhatLuc = GETDATE()
                              WHERE MaThongTin = @Id", conn))
                        {
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = maThongTin;
                            cmd.Parameters.AddWithValue("@Ten", txtTenPK.Text.Trim());
                            cmd.Parameters.AddWithValue("@DC", txtDiaChi.Text.Trim());
                            cmd.Parameters.AddWithValue("@SDT", txtSDT.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                            cmd.Parameters.AddWithValue("@Web", txtWebsite.Text.Trim());
                            cmd.Parameters.AddWithValue("@Slogan", txtSlogan.Text.Trim());

                            int rows = cmd.ExecuteNonQuery();
                            if (rows == 0)
                            {
                                MessageBox.Show("Không cập nhật được bản ghi (MaThongTin=" + maThongTin + ").",
                                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }

                // Reload dữ liệu từ DB để xác nhận
                LoadThongTinPK();
                MessageBox.Show("Lưu thông tin phòng khám thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi khi lưu:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnLuuGioLamViec_Click(object sender, EventArgs e)
        {
            // ── Validate input ──
            string rawMo = txtGioMo.Text.Trim();
            string rawDong = txtGioDong.Text.Trim();

            if (string.IsNullOrEmpty(rawMo) || string.IsNullOrEmpty(rawDong))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Giờ mở cửa và Giờ đóng cửa.\n\nĐịnh dạng: HH:mm (ví dụ: 08:00, 17:30)",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TimeSpan gioMo, gioDong;
            if (!TimeSpan.TryParse(rawMo, out gioMo))
            {
                MessageBox.Show("Giờ mở cửa không hợp lệ: \"" + rawMo + "\"\n\nĐịnh dạng đúng: HH:mm (ví dụ: 08:00)",
                    "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGioMo.Focus();
                return;
            }
            if (!TimeSpan.TryParse(rawDong, out gioDong))
            {
                MessageBox.Show("Giờ đóng cửa không hợp lệ: \"" + rawDong + "\"\n\nĐịnh dạng đúng: HH:mm (ví dụ: 17:00)",
                    "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGioDong.Focus();
                return;
            }

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    // Lấy MaThongTin
                    int maThongTin = 0;
                    using (var cmd = new SqlCommand("SELECT TOP 1 MaThongTin FROM ThongTinPhongKham ORDER BY MaThongTin DESC", conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            maThongTin = Convert.ToInt32(result);
                    }

                    if (maThongTin == 0)
                    {
                        // Chưa có record → INSERT
                        using (var cmd = new SqlCommand(
                            @"INSERT INTO ThongTinPhongKham (TenPhongKham, GioMoCua, GioDongCua, LichLamViecHangTuan)
                              VALUES (N'DermaSoft Clinic', @Mo, @Dong, @Lich)", conn))
                        {
                            cmd.Parameters.Add("@Mo", SqlDbType.Time).Value = gioMo;
                            cmd.Parameters.Add("@Dong", SqlDbType.Time).Value = gioDong;
                            cmd.Parameters.AddWithValue("@Lich",
                                string.IsNullOrEmpty(txtLichTuan.Text.Trim()) ? (object)DBNull.Value : txtLichTuan.Text.Trim());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Có record → UPDATE
                        using (var cmd = new SqlCommand(
                            @"UPDATE ThongTinPhongKham
                              SET GioMoCua = @Mo,
                                  GioDongCua = @Dong,
                                  LichLamViecHangTuan = @Lich,
                                  DatCapNhatLuc = GETDATE()
                              WHERE MaThongTin = @Id", conn))
                        {
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = maThongTin;
                            cmd.Parameters.Add("@Mo", SqlDbType.Time).Value = gioMo;
                            cmd.Parameters.Add("@Dong", SqlDbType.Time).Value = gioDong;
                            cmd.Parameters.AddWithValue("@Lich",
                                string.IsNullOrEmpty(txtLichTuan.Text.Trim()) ? (object)DBNull.Value : txtLichTuan.Text.Trim());

                            int rows = cmd.ExecuteNonQuery();
                            if (rows == 0)
                            {
                                MessageBox.Show("Không cập nhật được bản ghi (MaThongTin=" + maThongTin + ").\nVui lòng thử lại.",
                                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }

                // Reload dữ liệu từ DB để xác nhận
                LoadThongTinPK();
                MessageBox.Show("Lưu giờ làm việc thành công!\n\n• Giờ mở cửa: " + gioMo.ToString(@"hh\:mm")
                    + "\n• Giờ đóng cửa: " + gioDong.ToString(@"hh\:mm"),
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu giờ làm việc:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBackupDB_Click(object sender, EventArgs e)
        {
            string path = txtBackupPath.Text.Trim();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Vui lòng chọn đường dẫn sao lưu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string dir = System.IO.Path.GetDirectoryName(path);
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand("BACKUP DATABASE DERMASOFT TO DISK = @Path WITH FORMAT, INIT, NAME = 'DERMASOFT Backup'", conn))
                {
                    cmd.CommandTimeout = 120;
                    cmd.Parameters.AddWithValue("@Path", path);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Sao lưu thành công!\n" + path, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi sao lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnTestConnection_Click(object sender, EventArgs e)
        {
            string err;
            bool ok = DatabaseConnection.TestConnection(out err);
            if (ok)
            {
                lblConnStatus.Text = "✅ Kết nối thành công — " + DateTime.Now.ToString("HH:mm:ss");
                lblConnStatus.ForeColor = ColorScheme.Success;
            }
            else
            {
                lblConnStatus.Text = "❌ Lỗi: " + (err ?? "Không xác định");
                lblConnStatus.ForeColor = ColorScheme.Danger;
            }
        }

        private void BtnCleanup_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Dọn dẹp dữ liệu rác sẽ:\n\n" +
                "• Xóa vĩnh viễn phiếu khám đã xóa mềm (IsDeleted=1)\n" +
                "  cùng hóa đơn, chi tiết dịch vụ, đơn thuốc, hình ảnh, đánh giá liên quan\n" +
                "• Xóa vĩnh viễn bệnh nhân đã xóa mềm (không còn phiếu khám)\n" +
                "• Xóa OTP hết hạn\n" +
                "• Xóa audit log > 6 tháng\n\n" +
                "⚠️ Hành động không thể hoàn tác. Tiếp tục?",
                "Xác nhận dọn dẹp", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            try
            {
                int total = 0;
                var errors = new System.Text.StringBuilder();

                using (var conn = DatabaseConnection.GetConnection())
                {
                    // Thứ tự xóa: con trước → cha sau (tránh FK violation)
                    string[] sqls = new string[]
                    {
                        // 1. Xóa bảng con của PhieuKham đã xóa mềm
                        "DELETE FROM DanhGia WHERE MaPhieuKham IN (SELECT MaPhieuKham FROM PhieuKham WHERE IsDeleted = 1)",
                        "DELETE FROM HinhAnhBenhLy WHERE MaPhieuKham IN (SELECT MaPhieuKham FROM PhieuKham WHERE IsDeleted = 1)",
                        "DELETE FROM ChiTietDonThuoc WHERE MaPhieuKham IN (SELECT MaPhieuKham FROM PhieuKham WHERE IsDeleted = 1)",
                        "DELETE FROM ChiTietDichVu WHERE MaPhieuKham IN (SELECT MaPhieuKham FROM PhieuKham WHERE IsDeleted = 1)",
                        "DELETE FROM HoaDon WHERE MaPhieuKham IN (SELECT MaPhieuKham FROM PhieuKham WHERE IsDeleted = 1)",
                        "DELETE FROM PhieuKham_LichSu WHERE MaPhieuKham IN (SELECT MaPhieuKham FROM PhieuKham WHERE IsDeleted = 1)",
                        // 2. Xóa PhieuKham đã xóa mềm
                        "DELETE FROM PhieuKham WHERE IsDeleted = 1",
                        // 3. Xóa BenhNhan đã xóa mềm (chỉ nếu không còn FK tham chiếu)
                        @"DELETE FROM BenhNhan WHERE IsDeleted = 1
                          AND MaBenhNhan NOT IN (SELECT MaBenhNhan FROM PhieuKham)
                          AND MaBenhNhan NOT IN (SELECT MaBenhNhan FROM LichHen)
                          AND MaBenhNhan NOT IN (SELECT MaBenhNhan FROM DanhGia)
                          AND MaBenhNhan NOT IN (SELECT MaBenhNhan FROM ThanhVienInfo)",
                        // 4. Xóa OTP hết hạn
                        "DELETE FROM XacThucOTP WHERE NgayHetHan < GETDATE()",
                        // 5. Xóa audit log > 6 tháng
                        "DELETE FROM NguoiDung_AuditLog WHERE NgayTao < DATEADD(MONTH, -6, GETDATE())",
                    };

                    foreach (string sql in sqls)
                    {
                        try
                        {
                            using (var cmd = new SqlCommand(sql, conn))
                            {
                                cmd.CommandTimeout = 60;
                                total += cmd.ExecuteNonQuery();
                            }
                        }
                        catch (SqlException ex)
                        {
                            errors.AppendLine("• " + ex.Message);
                        }
                    }
                }

                string msg = "Dọn dẹp hoàn tất! Đã xóa " + total + " bản ghi.";
                if (errors.Length > 0)
                    msg += "\n\nMột số lệnh bị lỗi:\n" + errors.ToString();

                MessageBox.Show(msg, "Kết quả", MessageBoxButtons.OK,
                    errors.Length > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ══════════════════════════════════════════
        // HELPERS — UI FACTORY
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

        private Label TaoSectionTitle(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = ColorScheme.PrimaryDark,
                Location = new Point(x, y),
                AutoSize = true,
                BackColor = Color.Transparent,
            };
        }

        private Label TaoLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = AppFonts.Body,
                ForeColor = ColorScheme.TextGray,
                Location = new Point(x, y),
                AutoSize = true,
                BackColor = Color.Transparent,
            };
        }

        private Guna2TextBox TaoInput(int x, int y, int w, string placeholder)
        {
            return new Guna2TextBox
            {
                Font = AppFonts.Body, ForeColor = ColorScheme.TextDark,
                Location = new Point(x, y), Size = new Size(w, 36),
                PlaceholderText = placeholder, PlaceholderForeColor = ColorScheme.TextLight,
                BorderRadius = 8, BorderColor = BorderInput,
                FocusedState = { BorderColor = ColorScheme.Primary },
                HoverState = { BorderColor = ColorScheme.Primary },
                FillColor = InputBg,
            };
        }

        private Guna2Button TaoBtnPrimary(string text, int x, int y, int w)
        {
            return new Guna2Button
            {
                Text = text, Font = AppFonts.BodyBold, ForeColor = Color.White,
                FillColor = ColorScheme.Primary, BorderRadius = 18,
                Location = new Point(x, y), Size = new Size(w, 36), Cursor = Cursors.Hand,
            };
        }

        private Guna2Button TaoBtnWarning(string text, int x, int y, int w)
        {
            return new Guna2Button
            {
                Text = text, Font = AppFonts.BodyBold, ForeColor = Color.White,
                FillColor = ColorScheme.Warning, BorderRadius = 18,
                Location = new Point(x, y), Size = new Size(w, 36), Cursor = Cursors.Hand,
            };
        }

        private Guna2Button TaoBtnDanger(string text, int x, int y, int w)
        {
            return new Guna2Button
            {
                Text = text, Font = AppFonts.BodyBold, ForeColor = Color.White,
                FillColor = ColorScheme.Danger, BorderRadius = 18,
                Location = new Point(x, y), Size = new Size(w, 36), Cursor = Cursors.Hand,
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
