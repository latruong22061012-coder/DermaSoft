using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DermaSoft.Data;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Form Quản Lý Lịch Hẹn dành cho Lễ Tân.
    /// Layout: Trái = Calendar + Form tạo nhanh | Phải = Search + DataGridView danh sách
    /// </summary>
    public partial class AppointmentForm : Form
    {
        // ── Màu badge trạng thái ─────────────────────────────────────────
        private static readonly Color ClrXNBg = Color.FromArgb(220, 252, 231); // Xanh lá nhạt
        private static readonly Color ClrXNFg = Color.FromArgb(21, 101, 52);
        private static readonly Color ClrChoBg = Color.FromArgb(254, 243, 199); // Vàng nhạt
        private static readonly Color ClrChoFg = Color.FromArgb(146, 64, 14);
        private static readonly Color ClrHuyBg = Color.FromArgb(254, 226, 226); // Đỏ nhạt
        private static readonly Color ClrHuyFg = Color.FromArgb(153, 27, 27);
        private static readonly Color ClrDoneBg = Color.FromArgb(243, 244, 246);
        private static readonly Color ClrDoneFg = Color.FromArgb(107, 114, 128);

        public AppointmentForm()
        {
            InitializeComponent();

            // Gắn sự kiện
            mcLichHen.DateChanged += McLichHen_DateChanged;
            btnTaoLich.Click += BtnTaoLich_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            cmbFilter.SelectedIndexChanged += CmbFilter_SelectedIndexChanged;
            dgvLichHen.CellClick += DgvLichHen_CellClick;

            this.Load += (s, e) => KhoiTao();
        }

        // ══════════════════════════════════════════════════════════════════
        // KHỞI TẠO
        // ══════════════════════════════════════════════════════════════════

        private void KhoiTao()
        {
            cmbFilter.SelectedIndex = 0;
            LoadDanhSachBacSi();
            KhoiTaoCmbGioHen();   // ← thêm dòng này


            // ── Thêm cột cho dgvLichHen ───────────────────────────────────────
            dgvLichHen.AutoGenerateColumns = false;
            dgvLichHen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLichHen.Columns.Clear();
            dgvLichHen.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGio", DataPropertyName = "ThoiGianHen", HeaderText = "Giờ", FillWeight = 60F, ReadOnly = true });
            dgvLichHen.Columns.Add(new DataGridViewTextBoxColumn { Name = "colBenhNhan", DataPropertyName = "HoTen", HeaderText = "Bệnh nhân", FillWeight = 170F, ReadOnly = true });
            dgvLichHen.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSDT", DataPropertyName = "SoDienThoai", HeaderText = "SĐT", FillWeight = 110F, ReadOnly = true });
            dgvLichHen.Columns.Add(new DataGridViewTextBoxColumn { Name = "colBacSi", DataPropertyName = "TenBacSi", HeaderText = "Bác sĩ", FillWeight = 140F, ReadOnly = true });
            dgvLichHen.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGhiChu", DataPropertyName = "GhiChu", HeaderText = "Ghi chú", FillWeight = 140F, ReadOnly = true });
            dgvLichHen.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTrangThai", DataPropertyName = "TrangThaiText", HeaderText = "Trạng thái", FillWeight = 95F, ReadOnly = true });
            dgvLichHen.Columns.Add(new DataGridViewTextBoxColumn { Name = "colThaoTac", DataPropertyName = "ThaoTacText", HeaderText = "Thao tác", FillWeight = 80F, ReadOnly = true });
            // ─────────────────────────────────────────────────────────────────

            LoadLichHen(mcLichHen.SelectionStart);

            // FIX: Căn giữa MonthCalendar trong pnlCalendar
            CenterTrongPanel(pnlCalendar, mcLichHen);
            pnlCalendar.Resize += (s, e) => CenterTrongPanel(pnlCalendar, mcLichHen);
        }

        /// <summary>Căn giữa một control con trong panel cha.</summary>
        private void CenterTrongPanel(Guna.UI2.WinForms.Guna2Panel parent, Control child)
        {
            child.Location = new Point(
                (parent.ClientSize.Width - child.Width) / 2,
                (parent.ClientSize.Height - child.Height) / 2
            );
        }

        // ══════════════════════════════════════════════════════════════════
        // LOAD DỮ LIỆU
        // ══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Nạp danh sách bác sĩ vào cmbBacSi.
        /// Ưu tiên BS có phân ca làm việc ngày được chọn, vẫn hiện BS khác.
        /// </summary>
        private void LoadDanhSachBacSi()
        {
            LoadDanhSachBacSi(mcLichHen.SelectionStart);
        }

        private void LoadDanhSachBacSi(DateTime ngay)
        {
            try
            {
                const string sql = @"
                    SELECT
                        nd.MaNguoiDung,
                        nd.HoTen + ISNULL(
                            N' — ' + (
                                SELECT STRING_AGG(clv.TenCa, N', ')
                                FROM PhanCongCa pcc
                                JOIN CaLamViec clv ON pcc.MaCa = clv.MaCa
                                WHERE pcc.MaNguoiDung = nd.MaNguoiDung
                                  AND pcc.NgayLamViec = @Ngay
                            ),
                            N' (Không có ca)'
                        ) AS TenHienThi,
                        CASE WHEN EXISTS (
                            SELECT 1 FROM PhanCongCa pcc2
                            WHERE pcc2.MaNguoiDung = nd.MaNguoiDung
                              AND pcc2.NgayLamViec = @Ngay
                        ) THEN 0 ELSE 1 END AS ThuTu
                    FROM NguoiDung nd
                    WHERE nd.MaVaiTro = 2
                      AND nd.TrangThaiTK = 1
                      AND nd.IsDeleted = 0
                    ORDER BY ThuTu, nd.HoTen";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@Ngay", ngay.Date));

                cmbBacSi.DataSource = dt;
                cmbBacSi.DisplayMember = "TenHienThi";
                cmbBacSi.ValueMember = "MaNguoiDung";

                if (dt != null && dt.Rows.Count > 0)
                    cmbBacSi.SelectedIndex = 0;
            }
            catch { }
        }

        /// <summary>
        /// Load các khung giờ hẹn đồng bộ với giờ hoạt động phòng khám (trước 1 tiếng đóng cửa).
        /// Đọc GioMoCua/GioDongCua từ ThongTinPhongKham, bước 30 phút.
        /// </summary>
        private void KhoiTaoCmbGioHen()
        {
            cmbGioHen.Items.Clear();

            int gioBatDau = 8;   // mặc định
            int gioKetThuc = 17; // mặc định

            try
            {
                var result = DatabaseConnection.ExecuteQuery(
                    "SELECT TOP 1 GioMoCua, GioDongCua FROM ThongTinPhongKham ORDER BY MaThongTin DESC");
                if (result != null && result.Rows.Count > 0)
                {
                    var row = result.Rows[0];
                    if (row["GioMoCua"] != DBNull.Value)
                        gioBatDau = ((TimeSpan)row["GioMoCua"]).Hours;
                    if (row["GioDongCua"] != DBNull.Value)
                        gioKetThuc = ((TimeSpan)row["GioDongCua"]).Hours - 1; // trước 1 tiếng đóng cửa
                }
            }
            catch { /* fallback mặc định */ }

            if (gioKetThuc <= gioBatDau) gioKetThuc = gioBatDau + 1;

            for (int h = gioBatDau; h <= gioKetThuc; h++)
            {
                cmbGioHen.Items.Add($"{h:D2}:00");
                if (h < gioKetThuc)
                    cmbGioHen.Items.Add($"{h:D2}:30");
            }

            // Mặc định giờ đầu tiên
            if (cmbGioHen.Items.Count > 0)
                cmbGioHen.SelectedIndex = 0;
        }

        /// <summary>
        /// Load lịch hẹn theo ngày được chọn trên calendar.
        /// Áp dụng thêm filter trạng thái và từ khoá tìm kiếm nếu có.
        /// </summary>
        private void LoadLichHen(DateTime ngay, string tuKhoa = "", int trangThai = -1)
        {
            try
            {
                string sql = @"
                    SELECT
                        lh.MaLichHen,
                        FORMAT(lh.ThoiGianHen, 'HH:mm')            AS ThoiGianHen,
                        ISNULL(bn.HoTen,
                            CASE WHEN lh.SoDienThoaiKhach IS NOT NULL
                                 THEN N'SĐT: ' + lh.SoDienThoaiKhach
                                 ELSE N'Chưa rõ' END)               AS HoTen,
                        ISNULL(bn.SoDienThoai,
                            ISNULL(lh.SoDienThoaiKhach, ''))        AS SoDienThoai,
                        ISNULL(nd.HoTen, N'Chưa phân công')         AS TenBacSi,
                        ISNULL(lh.GhiChu, N'—')                     AS GhiChu,
                        lh.TrangThai,
                        CASE lh.TrangThai
                            WHEN 0 THEN N'Chờ XN'
                            WHEN 1 THEN N'✓ XN'
                            WHEN 2 THEN N'Đã tiếp nhận'
                            WHEN 3 THEN N'Đã hủy'
                            ELSE        N'Không rõ'
                        END AS TrangThaiText,
                        CASE lh.TrangThai
                            WHEN 0 THEN N'XN | Hủy'
                            WHEN 1 THEN N'Sửa'
                            ELSE        N''
                        END AS ThaoTacText
                    FROM LichHen lh
                    LEFT JOIN BenhNhan  bn ON lh.MaBenhNhan  = bn.MaBenhNhan
                    LEFT JOIN NguoiDung nd ON lh.MaNguoiDung = nd.MaNguoiDung
                    WHERE CAST(lh.ThoiGianHen AS DATE) = @Ngay";

                // Filter trạng thái
                if (trangThai >= 0)
                    sql += " AND lh.TrangThai = @TrangThai";

                // Tìm kiếm theo tên / SĐT
                if (!string.IsNullOrWhiteSpace(tuKhoa))
                    sql += @" AND (bn.HoTen LIKE @TuKhoa
                                OR bn.SoDienThoai LIKE @TuKhoa
                                OR lh.SoDienThoaiKhach LIKE @TuKhoa)";

                sql += " ORDER BY lh.ThoiGianHen ASC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql, p =>
                {
                    p.AddWithValue("@Ngay", ngay.Date);
                    if (trangThai >= 0)
                        p.AddWithValue("@TrangThai", trangThai);
                    if (!string.IsNullOrWhiteSpace(tuKhoa))
                        p.AddWithValue("@TuKhoa", "%" + tuKhoa.Trim() + "%");
                });

                dgvLichHen.AutoGenerateColumns = false;
                dgvLichHen.DataSource = dt;
                TomauBadge();

                // Cập nhật label ngày + số lịch
                int soLich = dt?.Rows.Count ?? 0;
                lblNgayHien.Text = $"📅  Lịch hẹn ngày {ngay:dd/MM/yyyy} — {soLich} lịch";

                // Đặt ngày hẹn mặc định trong form tạo lịch
                dtpThoiGian.Value = ngay.Date.AddHours(9);
            }
            catch (Exception ex)
            {
                lblNgayHien.Text = "⚠️  Lỗi tải dữ liệu: " + ex.Message;
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // TÔ MÀU BADGE — Trạng thái + Thao tác
        // ══════════════════════════════════════════════════════════════════

        private void TomauBadge()
        {
            foreach (DataGridViewRow row in dgvLichHen.Rows)
            {
                if (row.IsNewRow) continue;

                string tt = row.Cells["colTrangThai"].Value?.ToString() ?? "";

                Color bg, fg;
                switch (tt)
                {
                    case "✓ XN":
                        bg = ClrXNBg; fg = ClrXNFg; break;
                    case "Chờ XN":
                        bg = ClrChoBg; fg = ClrChoFg; break;
                    case "Đã hủy":
                        bg = ClrHuyBg; fg = ClrHuyFg; break;
                    case "Đã tiếp nhận":
                        bg = Color.FromArgb(219, 234, 254); fg = Color.FromArgb(30, 64, 175); break;
                    default:
                        bg = ClrDoneBg; fg = ClrDoneFg; break;
                }

                var tsStyle = row.Cells["colTrangThai"].Style;
                tsStyle.BackColor = bg;
                tsStyle.ForeColor = fg;
                tsStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                tsStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tsStyle.SelectionBackColor = bg;
                tsStyle.SelectionForeColor = fg;
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN — Calendar chọn ngày
        // ══════════════════════════════════════════════════════════════════

        private void McLichHen_DateChanged(object sender, DateRangeEventArgs e)
        {
            LoadLichHen(e.Start, txtSearch.Text, LayTrangThaiFilter());
            LoadDanhSachBacSi(e.Start);
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN — Tìm kiếm + Filter
        // ══════════════════════════════════════════════════════════════════

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadLichHen(mcLichHen.SelectionStart, txtSearch.Text, LayTrangThaiFilter());
        }

        private void CmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLichHen(mcLichHen.SelectionStart, txtSearch.Text, LayTrangThaiFilter());
        }

        /// <summary>Chuyển index cmbFilter → giá trị TrangThai DB (-1 = tất cả).</summary>
        private int LayTrangThaiFilter()
        {
            switch (cmbFilter.SelectedIndex)
            {
                case 1: return 0; // Chờ xác nhận
                case 2: return 1; // Đã xác nhận
                case 3: return 2; // Đã tiếp nhận
                case 4: return 3; // Đã hủy
                default: return -1; // Tất cả
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN — Click ô Thao tác trong DataGridView
        // ══════════════════════════════════════════════════════════════════

        private void DgvLichHen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataTable dt = (DataTable)dgvLichHen.DataSource;
            int maLichHen = Convert.ToInt32(dt.Rows[e.RowIndex]["MaLichHen"]);
            int trangThai = Convert.ToInt32(dt.Rows[e.RowIndex]["TrangThai"]);

            // Click cột Thao tác → XN/Hủy/Sửa
            if (e.ColumnIndex == dgvLichHen.Columns["colThaoTac"].Index)
            {
                string thaoTac = dgvLichHen.Rows[e.RowIndex].Cells["colThaoTac"].Value?.ToString() ?? "";
                if (thaoTac == "XN | Hủy")
                    XacNhanHoacHuy(maLichHen);
                else if (thaoTac == "Sửa")
                    SuaLichHen(maLichHen);
                return;
            }

            // Click bất kỳ cột khác → xem chi tiết
            HienChiTietLichHen(maLichHen);
        }

        private void XacNhanHoacHuy(int maLichHen)
        {
            // Bước 1: Hỏi xác nhận
            var ketQua = MessageBox.Show(
                "Bạn muốn xác nhận lịch hẹn này không?\n\n" +
                "▶  Chọn [Có]  → Xác nhận lịch hẹn\n" +
                "▶  Chọn [Không] → Hủy lịch hẹn",
                "Thao Tác Lịch Hẹn",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (ketQua == DialogResult.Yes)
            {
                // Xác nhận lịch hẹn
                CapNhatTrangThai(maLichHen, 1, "xác nhận");
            }
            else if (ketQua == DialogResult.No)
            {
                // Hỏi thêm 1 bước trước khi hủy để tránh nhấn nhầm
                var xacNhanHuy = MessageBox.Show(
                    "Bạn có chắc muốn HỦY lịch hẹn này không?\nHành động này không thể hoàn tác.",
                    "Xác Nhận Hủy Lịch Hẹn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (xacNhanHuy == DialogResult.Yes)
                    CapNhatTrangThai(maLichHen, 3, "hủy");
            }
            // Cancel → không làm gì
        }

        /// <summary>Hiện chi tiết lịch hẹn trong dialog.</summary>
        private void HienChiTietLichHen(int maLichHen)
        {
            try
            {
                const string sql = @"
                    SELECT
                        lh.MaLichHen,
                        FORMAT(lh.ThoiGianHen, 'HH:mm dd/MM/yyyy') AS ThoiGian,
                        ISNULL(bn.HoTen, N'Chưa rõ')                AS TenBN,
                        ISNULL(bn.SoDienThoai, ISNULL(lh.SoDienThoaiKhach, N'—')) AS SoDienThoai,
                        ISNULL(bn.GioiTinh, 0)                      AS GioiTinh,
                        ISNULL(FORMAT(bn.NgaySinh, 'dd/MM/yyyy'), N'—') AS NgaySinh,
                        ISNULL(nd.HoTen, N'Chưa phân công')         AS TenBacSi,
                        ISNULL(lh.GhiChu, N'')                      AS GhiChu,
                        CASE lh.TrangThai
                            WHEN 0 THEN N'Chờ xác nhận'
                            WHEN 1 THEN N'Đã xác nhận'
                            WHEN 2 THEN N'Đã tiếp nhận'
                            WHEN 3 THEN N'Đã hủy'
                            ELSE N'Không rõ'
                        END AS TrangThai
                    FROM LichHen lh
                    LEFT JOIN BenhNhan  bn ON lh.MaBenhNhan  = bn.MaBenhNhan
                    LEFT JOIN NguoiDung nd ON lh.MaNguoiDung = nd.MaNguoiDung
                    WHERE lh.MaLichHen = @MaLH";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaLH", maLichHen));

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy lịch hẹn.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRow r = dt.Rows[0];
                string msg = $"📋 CHI TIẾT LỊCH HẸN #{maLichHen}\n" +
                    $"{'─', 40}\n" +
                    $"🕐  Thời gian:  {r["ThoiGian"]}\n" +
                    $"👤  Bệnh nhân:  {r["TenBN"]}\n" +
                    $"📞  SĐT:        {r["SoDienThoai"]}\n" +
                    $"🎂  Ngày sinh:   {r["NgaySinh"]}\n" +
                    $"🩺  Bác sĩ:     {r["TenBacSi"]}\n" +
                    $"📝  Ghi chú:     {r["GhiChu"]}\n" +
                    $"📌  Trạng thái:  {r["TrangThai"]}";

                MessageBox.Show(msg, "Chi tiết lịch hẹn",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Sửa lịch hẹn: cho phép thay đổi bác sĩ, giờ hẹn, ghi chú.</summary>
        private void SuaLichHen(int maLichHen)
        {
            try
            {
                // Load thông tin hiện tại
                DataTable dtLH = DatabaseConnection.ExecuteQuery(
                    "SELECT MaNguoiDung, ThoiGianHen, GhiChu FROM LichHen WHERE MaLichHen = @MaLH",
                    p => p.AddWithValue("@MaLH", maLichHen));

                if (dtLH == null || dtLH.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy lịch hẹn.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRow lr = dtLH.Rows[0];
                DateTime thoiGianCu = Convert.ToDateTime(lr["ThoiGianHen"]);
                string ghiChuCu = lr["GhiChu"]?.ToString() ?? "";
                int? maBSCu = lr["MaNguoiDung"] != DBNull.Value ? Convert.ToInt32(lr["MaNguoiDung"]) : (int?)null;

                // Tạo dialog sửa
                using (var dlg = new Form
                {
                    Text = $"Sửa lịch hẹn #{maLichHen}",
                    Size = new Size(420, 340),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false, MinimizeBox = false,
                    BackColor = Color.White,
                    Font = new Font("Segoe UI", 9.5f)
                })
                {
                    int y = 16, xLbl = 16, xCtrl = 120, wCtrl = 260;

                    dlg.Controls.Add(new Label { Text = "Bác sĩ:", Location = new Point(xLbl, y + 4), AutoSize = true });
                    var cboBS = new ComboBox { Location = new Point(xCtrl, y), Width = wCtrl, DropDownStyle = ComboBoxStyle.DropDownList };
                    DataTable dtBS = DatabaseConnection.ExecuteQuery(
                        @"SELECT MaNguoiDung, HoTen FROM NguoiDung
                          WHERE MaVaiTro=2 AND TrangThaiTK=1 AND IsDeleted=0 ORDER BY HoTen");
                    cboBS.DisplayMember = "HoTen";
                    cboBS.ValueMember = "MaNguoiDung";
                    cboBS.DataSource = dtBS;
                    if (maBSCu.HasValue)
                    {
                        cboBS.SelectedValue = maBSCu.Value;
                    }
                    dlg.Controls.Add(cboBS);
                    y += 40;

                    dlg.Controls.Add(new Label { Text = "Ngày hẹn:", Location = new Point(xLbl, y + 4), AutoSize = true });
                    var dtpNgay = new DateTimePicker { Location = new Point(xCtrl, y), Width = wCtrl, Format = DateTimePickerFormat.Short, Value = thoiGianCu.Date };
                    dlg.Controls.Add(dtpNgay);
                    y += 40;

                    dlg.Controls.Add(new Label { Text = "Giờ hẹn:", Location = new Point(xLbl, y + 4), AutoSize = true });
                    var cboGio = new ComboBox { Location = new Point(xCtrl, y), Width = wCtrl, DropDownStyle = ComboBoxStyle.DropDownList };
                    for (int h = 7; h <= 20; h++)
                    {
                        cboGio.Items.Add($"{h:D2}:00");
                        if (h < 20) cboGio.Items.Add($"{h:D2}:30");
                    }
                    cboGio.SelectedItem = thoiGianCu.ToString("HH:mm");
                    if (cboGio.SelectedIndex < 0) cboGio.SelectedIndex = 0;
                    dlg.Controls.Add(cboGio);
                    y += 40;

                    dlg.Controls.Add(new Label { Text = "Ghi chú:", Location = new Point(xLbl, y + 4), AutoSize = true });
                    var txtGC = new TextBox { Location = new Point(xCtrl, y), Width = wCtrl, Height = 60, Multiline = true, Text = ghiChuCu };
                    dlg.Controls.Add(txtGC);
                    y += 80;

                    var btnLuu = new Button
                    {
                        Text = "💾 Lưu", Location = new Point(xCtrl, y), Width = 120, Height = 36,
                        BackColor = Color.FromArgb(15, 92, 77), ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
                    };
                    var btnHuy = new Button
                    {
                        Text = "Hủy", Location = new Point(xCtrl + 130, y), Width = 80, Height = 36,
                        FlatStyle = FlatStyle.Flat
                    };
                    btnHuy.Click += (s2, e2) => dlg.DialogResult = DialogResult.Cancel;

                    btnLuu.Click += (s2, e2) =>
                    {
                        string gioMoi = cboGio.SelectedItem?.ToString() ?? "09:00";
                        int hh = int.Parse(gioMoi.Split(':')[0]);
                        int mm = int.Parse(gioMoi.Split(':')[1]);
                        DateTime thoiGianMoi = dtpNgay.Value.Date.AddHours(hh).AddMinutes(mm);
                        int maBSMoi = Convert.ToInt32(cboBS.SelectedValue);
                        string ghiChuMoi = txtGC.Text.Trim();

                        int rows = DatabaseConnection.ExecuteNonQuery(
                            @"UPDATE LichHen SET MaNguoiDung=@MaBS, ThoiGianHen=@TG, GhiChu=@GC
                              WHERE MaLichHen=@MaLH",
                            p =>
                            {
                                p.AddWithValue("@MaBS", maBSMoi);
                                p.AddWithValue("@TG", thoiGianMoi);
                                p.AddWithValue("@GC", string.IsNullOrEmpty(ghiChuMoi) ? (object)DBNull.Value : ghiChuMoi);
                                p.AddWithValue("@MaLH", maLichHen);
                            });

                        if (rows > 0)
                        {
                            MessageBox.Show("Đã cập nhật lịch hẹn thành công! ✅",
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dlg.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show("Không cập nhật được.", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

                    dlg.Controls.Add(btnLuu);
                    dlg.Controls.Add(btnHuy);

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                        LoadLichHen(mcLichHen.SelectionStart, txtSearch.Text, LayTrangThaiFilter());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa lịch hẹn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CapNhatTrangThai(int maLichHen, int trangThai, string tenHanhDong)
        {
            try
            {
                int rows = DatabaseConnection.ExecuteNonQuery(
                    "UPDATE LichHen SET TrangThai = @TrangThai WHERE MaLichHen = @MaLichHen",
                    p =>
                    {
                        p.AddWithValue("@TrangThai", trangThai);
                        p.AddWithValue("@MaLichHen", maLichHen);
                    });

                if (rows > 0)
                {
                    MessageBox.Show($"Đã {tenHanhDong} lịch hẹn thành công.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadLichHen(mcLichHen.SelectionStart, txtSearch.Text, LayTrangThaiFilter());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN — Tạo lịch hẹn mới
        // ══════════════════════════════════════════════════════════════════

        private void BtnTaoLich_Click(object sender, EventArgs e)
        {
            string tenBN = txtBenhNhan.Text.Trim();

            if (string.IsNullOrEmpty(tenBN))
            {
                MessageBox.Show("Vui lòng nhập tên hoặc SĐT bệnh nhân.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBenhNhan.Focus();
                return;
            }
            try
            {
                int? maBacSi = null;
                if (cmbBacSi.SelectedValue != null &&
                    int.TryParse(cmbBacSi.SelectedValue.ToString(), out int id))
                    maBacSi = id;

                // Gộp ngày từ DatePicker + giờ từ ComboBox TRƯỚC
                string gioChon = cmbGioHen.SelectedItem?.ToString() ?? "09:00";
                int gio = int.Parse(gioChon.Split(':')[0]);
                int phut = int.Parse(gioChon.Split(':')[1]);
                DateTime thoiGianHen = dtpThoiGian.Value.Date.AddHours(gio).AddMinutes(phut);

                // Validate thời gian THẬT SỰ (sau khi đã gộp ngày + giờ)
                if (thoiGianHen < DateTime.Now.AddMinutes(-10))
                {
                    MessageBox.Show("Thời gian hẹn phải từ hiện tại trở đi.",
                        "Thời gian không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DatLichHen(tenBN, thoiGianHen, maBacSi, txtGhiChu.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo lịch: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DatLichHen(string tenHoacSDT, DateTime thoiGian, int? maBacSi, string ghiChu)
        {
            // Nếu ô nhập rỗng thì không làm gì
            if (string.IsNullOrWhiteSpace(tenHoacSDT)) return;

            // Xác định xem người dùng nhập SĐT hay tên
            bool laSdt = System.Text.RegularExpressions.Regex.IsMatch(tenHoacSDT, @"^0\d{9}$");
            string tenDat = laSdt ? "Khách" : tenHoacSDT;
            string sdtDat = laSdt ? tenHoacSDT : "";

            // Nếu nhập tên thay vì SĐT → yêu cầu nhập SĐT vào ô riêng
            if (!laSdt)
            {
                MessageBox.Show(
                    "Vui lòng nhập số điện thoại (10 chữ số bắt đầu bằng 0) để tạo lịch hẹn.\n" +
                    "Hệ thống sẽ tự tìm hoặc tạo hồ sơ bệnh nhân theo SĐT.",
                    "Cần số điện thoại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBenhNhan.Focus();
                return;
            }

            int rowsAffected = DatabaseConnection.ExecuteNonQuery(@"
                DECLARE @MaBN INT;

                -- Tìm bệnh nhân theo SĐT
                SELECT @MaBN = MaBenhNhan FROM BenhNhan
                WHERE SoDienThoai = @SDT AND IsDeleted = 0;

                -- Nếu chưa có thì tạo mới
                IF @MaBN IS NULL AND @SDT <> ''
                BEGIN
                    INSERT INTO BenhNhan (HoTen, SoDienThoai)
                    VALUES (@Ten, @SDT);
                    SET @MaBN = SCOPE_IDENTITY();
                END;

                -- Chỉ tạo lịch hẹn khi tìm được hoặc tạo được BN
                IF @MaBN IS NOT NULL
                    INSERT INTO LichHen (MaBenhNhan, MaNguoiDung, ThoiGianHen, GhiChu, TrangThai, SoDienThoaiKhach)
                    VALUES (@MaBN, @MaBacSi, @ThoiGian, @GhiChu, 0, @SDT);",
                p =>
                {
                    p.AddWithValue("@Ten", tenDat);
                    p.AddWithValue("@SDT", sdtDat);
                    p.AddWithValue("@MaBacSi", (object)maBacSi ?? DBNull.Value);
                    p.AddWithValue("@ThoiGian", thoiGian);
                    p.AddWithValue("@GhiChu", string.IsNullOrEmpty(ghiChu) ? (object)DBNull.Value : ghiChu);
                });

            if (rowsAffected > 0)
            {
                MessageBox.Show("Đã tạo lịch hẹn thành công! ✅",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBenhNhan.Text = "";
                txtGhiChu.Text = "";
                LoadLichHen(thoiGian.Date, txtSearch.Text, LayTrangThaiFilter());
                mcLichHen.SetDate(thoiGian.Date);
            }
            else
            {
                MessageBox.Show(
                    "Không tạo được lịch hẹn.\nVui lòng nhập đúng số điện thoại 10 chữ số.",
                    "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AppointmentForm_Load(object sender, EventArgs e)
        {

        }
    }
}
