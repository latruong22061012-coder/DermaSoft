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

        /// <summary>Nạp danh sách bác sĩ vào cmbBacSi.</summary>
        private void LoadDanhSachBacSi()
        {
            try
            {
                const string sql = @"
                    SELECT MaNguoiDung, HoTen
                    FROM   NguoiDung
                    WHERE  MaVaiTro = 2        -- VaiTro.BacSi
                      AND  TrangThaiTK = 1
                      AND  IsDeleted   = 0
                    ORDER BY HoTen";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql);
                cmbBacSi.DataSource = dt;
                cmbBacSi.DisplayMember = "HoTen";
                cmbBacSi.ValueMember = "MaNguoiDung";

                if (dt != null && dt.Rows.Count > 0)
                    cmbBacSi.SelectedIndex = 0;
            }
            catch { }
        }

        /// <summary>Load các khung giờ hẹn (07:00 → 20:00, bước 30 phút) vào cmbGioHen.</summary>
        private void KhoiTaoCmbGioHen()
        {
            cmbGioHen.Items.Clear();
            for (int h = 7; h <= 20; h++)
            {
                cmbGioHen.Items.Add($"{h:D2}:00");
                if (h < 20)
                    cmbGioHen.Items.Add($"{h:D2}:30");
            }
            // Mặc định 09:00
            cmbGioHen.SelectedItem = "09:00";
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
                            WHEN 2 THEN N'Hoàn thành'
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
                case 3: return 2; // Hoàn thành
                case 4: return 3; // Đã hủy
                default: return -1;
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN — Click ô Thao tác trong DataGridView
        // ══════════════════════════════════════════════════════════════════

        private void DgvLichHen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvLichHen.Columns["colThaoTac"].Index)
                return;

            DataTable dt = (DataTable)dgvLichHen.DataSource;
            int maLichHen = Convert.ToInt32(dt.Rows[e.RowIndex]["MaLichHen"]);
            int trangThai = Convert.ToInt32(dt.Rows[e.RowIndex]["TrangThai"]);
            string thaoTac = dgvLichHen.Rows[e.RowIndex].Cells["colThaoTac"].Value?.ToString() ?? "";

            if (thaoTac == "XN | Hủy")
            {
                // Hiện context menu hoặc dialog chọn
                XacNhanHoacHuy(maLichHen);
            }
            else if (thaoTac == "Sửa")
            {
                SuaLichHen(maLichHen);
            }
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

        private void SuaLichHen(int maLichHen)
        {
            // TODO: Mở dialog sửa lịch hẹn
            MessageBox.Show($"Sửa lịch hẹn #{maLichHen} — tính năng đang phát triển.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            // Nếu không phải SĐT và không phải SĐT → bắt buộc phải nhập SĐT mới tạo được BN
            if (!laSdt && string.IsNullOrEmpty(sdtDat))
            {
                MessageBox.Show(
                    "Vui lòng nhập số điện thoại (10 chữ số bắt đầu bằng 0) để tạo lịch hẹn.",
                    "Cần số điện thoại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
