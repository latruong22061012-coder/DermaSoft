using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DermaSoft.Data;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Dashboard dành riêng cho Lễ Tân.
    /// Hiển thị: 4 KPI cards + bảng lịch hẹn hôm nay + danh sách hóa đơn cần thu.
    /// Tất cả dữ liệu lấy trực tiếp từ SQL Server qua DatabaseConnection.
    /// </summary>
    public partial class DashboardLeTanForm : Form
    {
        private System.Windows.Forms.Timer _refreshTimer;
        public DashboardLeTanForm()
        {
            InitializeComponent();

            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true);
            this.UpdateStyles();

            // Gắn sự kiện các nút action
            btnTiepNhan.Click += BtnTiepNhan_Click;
            btnTaoLich.Click += BtnTaoLich_Click;
            btnXacNhan.Click += BtnXacNhan_Click;

            // Load dữ liệu khi form hiển thị
            this.Load += (s, e) =>
            {
                LoadTatCa();

                // Tự động refresh mỗi 15 giây để nhận lịch hẹn mới từ Website
                _refreshTimer = new System.Windows.Forms.Timer();
                _refreshTimer.Interval = 15000; // 15 giây
                _refreshTimer.Tick += (ts, te) => LoadTatCa();
                _refreshTimer.Start();
            };

            // Dừng timer khi đóng form để tránh lỗi
            this.FormClosed += (s, e) => _refreshTimer?.Stop();
        }

        // ══════════════════════════════════════════════════════════════════
        // LOAD DỮ LIỆU — Gọi tất cả khi form load
        // ══════════════════════════════════════════════════════════════════

        private void LoadTatCa()
        {
            this.SuspendLayout();
            try
            {
                LoadKpiCards();
                LoadLichHenHomNay();
                LoadHoaDonCanThu();
            }
            finally
            {
                this.ResumeLayout(true);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // KPI CARDS — 4 chỉ số tổng quan
        // ══════════════════════════════════════════════════════════════════

        private void LoadKpiCards()
        {
            int lichHenHomNay = 0;
            int benhNhanDangCho = 0;
            int hoaDonCanThu = 0;
            int daTiepNhan = 0;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    // Card 1: Lịch hẹn hôm nay (trạng thái 0=Chờ xác nhận, 1=Đã xác nhận)
                    using (var cmd = new SqlCommand(
                        @"SELECT COUNT(*) FROM LichHen
                          WHERE CAST(ThoiGianHen AS DATE) = CAST(GETDATE() AS DATE)
                            AND TrangThai IN (0, 1)", conn))
                        lichHenHomNay = Convert.ToInt32(cmd.ExecuteScalar());

                    // Card 2: Bệnh nhân đang chờ (phiếu khám TrangThai = 0 Mới, chưa khám)
                    using (var cmd = new SqlCommand(
                        @"SELECT COUNT(*) FROM PhieuKham
                          WHERE CAST(NgayKham AS DATE) = CAST(GETDATE() AS DATE)
                            AND TrangThai = 0
                            AND IsDeleted = 0", conn))
                        benhNhanDangCho = Convert.ToInt32(cmd.ExecuteScalar());

                    // Card 3: Phiếu khám hoàn thành chưa thanh toán
                    using (var cmd = new SqlCommand(
                        @"SELECT COUNT(*) FROM PhieuKham pk
                          LEFT JOIN HoaDon hd ON pk.MaPhieuKham = hd.MaPhieuKham
                                              AND hd.IsDeleted = 0
                          WHERE pk.TrangThai = 2
                            AND pk.IsDeleted = 0
                            AND (hd.MaHoaDon IS NULL OR hd.TrangThai = 0)", conn))
                        hoaDonCanThu = Convert.ToInt32(cmd.ExecuteScalar());

                    // Card 4: Đã tiếp nhận hôm nay (phiếu khám có ngày hôm nay)
                    using (var cmd = new SqlCommand(
                        @"SELECT COUNT(*) FROM PhieuKham
                          WHERE CAST(NgayKham AS DATE) = CAST(GETDATE() AS DATE)
                            AND IsDeleted = 0", conn))
                        daTiepNhan = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch { /* Giữ giá trị 0 nếu lỗi kết nối */ }

            // Cập nhật lên các label
            lblCard1Value.Text = lichHenHomNay.ToString();
            lblCard2Value.Text = benhNhanDangCho.ToString();
            lblCard3Value.Text = hoaDonCanThu.ToString();
            lblCard4Value.Text = daTiepNhan.ToString();
        }

        // ══════════════════════════════════════════════════════════════════
        // BẢNG LỊCH HẸN HÔM NAY
        // ══════════════════════════════════════════════════════════════════

        private void LoadLichHenHomNay()
        {
            try
            {
                const string sql = @"
                    SELECT
                        lh.MaLichHen,
                        lh.TrangThai,
                        ISNULL(CAST(lh.SoThuTu AS NVARCHAR(10)), N'—') AS STT,
                        FORMAT(lh.ThoiGianHen, 'HH:mm')            AS ThoiGianHen,
                        ISNULL(bn.HoTen, 
                            CASE WHEN lh.SoDienThoaiKhach IS NOT NULL 
                                 THEN N'SĐT: ' + lh.SoDienThoaiKhach 
                                 ELSE N'Chưa rõ' END)               AS HoTen,
                        ISNULL(bn.SoDienThoai,
                            ISNULL(lh.SoDienThoaiKhach, ''))        AS SoDienThoai,
                        ISNULL(nd.HoTen, N'Chưa phân công')         AS TenBacSi,
                        CASE lh.TrangThai
                            WHEN 0 THEN N'Chờ XN'
                            WHEN 1 THEN N'Đã XN (STT: ' + ISNULL(CAST(lh.SoThuTu AS NVARCHAR(10)), N'—') + N')'
                            WHEN 2 THEN N'Đã tiếp nhận'
                            WHEN 3 THEN N'Đã hủy'
                            ELSE        N'Không rõ'
                        END                                          AS TrangThaiText
                    FROM LichHen lh
                    LEFT JOIN BenhNhan  bn ON lh.MaBenhNhan  = bn.MaBenhNhan
                    LEFT JOIN NguoiDung nd ON lh.MaNguoiDung = nd.MaNguoiDung
                    WHERE CAST(lh.ThoiGianHen AS DATE) = CAST(GETDATE() AS DATE)
                      AND lh.TrangThai IN (0, 1)
                    ORDER BY lh.ThoiGianHen ASC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql);
                dgvQueue.DataSource = dt;

                // Ẩn cột khóa — chỉ dùng nội bộ cho các nút action
                if (dgvQueue.Columns["MaLichHen"] != null)
                    dgvQueue.Columns["MaLichHen"].Visible = false;
                if (dgvQueue.Columns["TrangThai"] != null)
                    dgvQueue.Columns["TrangThai"].Visible = false;

                // Tô màu badge cột Trạng Thái sau khi bind
                TomauTrangThai();
            }
            catch { }
        }

        /// <summary>
        /// Tô màu cột Trạng Thái dạng badge:
        ///   Đã XN   → nền xanh lá nhạt, chữ xanh đậm
        ///   Chờ XN  → nền vàng nhạt, chữ nâu
        /// </summary>
        private void TomauTrangThai()
        {
            foreach (DataGridViewRow row in dgvQueue.Rows)
            {
                if (row.IsNewRow) continue;

                string tt = row.Cells["colTrangThai"].Value?.ToString() ?? "";

                Color bg, fg;
                switch (tt)
                {
                    case "Đã XN":
                        bg = Color.FromArgb(220, 252, 231);  // Xanh lá nhạt
                        fg = Color.FromArgb(21, 101, 52);    // Xanh đậm
                        break;
                    case "Chờ XN":
                        bg = Color.FromArgb(254, 243, 199);  // Vàng nhạt
                        fg = Color.FromArgb(146, 64, 14);    // Nâu
                        break;
                    default:
                        bg = Color.FromArgb(243, 244, 246);
                        fg = Color.FromArgb(107, 114, 128);
                        break;
                }

                row.Cells["colTrangThai"].Style.BackColor = bg;
                row.Cells["colTrangThai"].Style.ForeColor = fg;
                row.Cells["colTrangThai"].Style.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                row.Cells["colTrangThai"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                row.Cells["colTrangThai"].Style.SelectionBackColor = bg;
                row.Cells["colTrangThai"].Style.SelectionForeColor = fg;
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // DANH SÁCH HÓA ĐƠN CẦN THU
        // ══════════════════════════════════════════════════════════════════

        private void LoadHoaDonCanThu()
        {
            flpHoaDonList.SuspendLayout();
            flpHoaDonList.Controls.Clear();

            try
            {
                // Lấy phiếu khám hoàn thành (TrangThai=2) chưa có hóa đơn đã thanh toán
                // HOẶC hóa đơn đã tạo nhưng chưa thu (HoaDon.TrangThai=0)
                const string sql = @"
                    SELECT TOP 10
                        pk.MaPhieuKham,
                        ISNULL(bn.HoTen, N'Bệnh nhân')   AS TenBenhNhan,
                        CASE WHEN hd.MaHoaDon IS NOT NULL
                            THEN ISNULL(hd.TongTienDichVu,0) + ISNULL(hd.TongThuoc,0) - ISNULL(hd.GiamGia,0)
                            ELSE ISNULL(
                                (SELECT SUM(ctdv.ThanhTien) FROM ChiTietDichVu ctdv WHERE ctdv.MaPhieuKham = pk.MaPhieuKham), 0)
                            + ISNULL(
                                (SELECT SUM(cdt.SoLuong * t.DonGia) FROM ChiTietDonThuoc cdt JOIN Thuoc t ON cdt.MaThuoc = t.MaThuoc WHERE cdt.MaPhieuKham = pk.MaPhieuKham), 0)
                        END                                AS TongTien,
                        hd.MaHoaDon
                    FROM PhieuKham pk
                    JOIN BenhNhan bn ON pk.MaBenhNhan = bn.MaBenhNhan
                    LEFT JOIN HoaDon hd ON pk.MaPhieuKham = hd.MaPhieuKham
                                        AND hd.IsDeleted = 0
                    WHERE pk.IsDeleted = 0
                      AND pk.TrangThai = 2
                      AND (hd.MaHoaDon IS NULL OR hd.TrangThai = 0)
                    ORDER BY pk.NgayKham DESC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    // Hiển thị thông báo rỗng
                    flpHoaDonList.Controls.Add(new Label
                    {
                        Text = "✅  Không có hóa đơn cần thu",
                        Font = new Font("Segoe UI", 9f),
                        ForeColor = Color.FromArgb(107, 114, 128),
                        AutoSize = true,
                        Padding = new Padding(8, 12, 0, 0),
                    });
                    return;
                }

                foreach (DataRow row in dt.Rows)
                {
                    int maPhieu = Convert.ToInt32(row["MaPhieuKham"]);
                    int maHoaDon = row["MaHoaDon"] != DBNull.Value ? Convert.ToInt32(row["MaHoaDon"]) : 0;
                    string tenBN = row["TenBenhNhan"].ToString();
                    decimal tongTien = Convert.ToDecimal(row["TongTien"]);

                    flpHoaDonList.Controls.Add(TaoHoaDonCard(maHoaDon, maPhieu, tenBN, tongTien));
                }
            }
            catch { }
            finally
            {
                flpHoaDonList.ResumeLayout(true);
            }
        }

        /// <summary>
        /// Tạo card nhỏ cho một hóa đơn trong danh sách bên phải.
        /// Layout: Tên BN (bold) | Phiếu #XXXX + badge trạng thái | Số tiền xanh lá
        /// Click → điều hướng sang Thanh Toán Hóa Đơn.
        /// </summary>
        private Panel TaoHoaDonCard(int maHoaDon, int maPhieu, string tenBN, decimal tongTien)
        {
            // Card container
            var card = new Panel
            {
                Size = new Size(flpHoaDonList.Width - 20, 78),
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Tag = maPhieu, // Lưu MaPhieuKham để điều hướng
                Padding = new Padding(12, 10, 12, 10),
            };
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                // Viền trái xanh lá
                using (var brush = new SolidBrush(Color.FromArgb(15, 92, 77)))
                    g.FillRectangle(brush, 0, 0, 4, card.Height);
                // Viền ngoài
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1f))
                    g.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            // Tên bệnh nhân
            var lblTen = new Label
            {
                Text = tenBN,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 46, 37),
                Location = new Point(14, 8),
                AutoSize = true,
                BackColor = Color.Transparent,
            };

            // Mã phiếu + badge
            var lblPhieu = new Label
            {
                Text = $"Phiếu #{maPhieu:D4}",
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(14, 30),
                AutoSize = true,
                BackColor = Color.Transparent,
            };

            // Badge trạng thái
            var lblBadge = new Label
            {
                Text = "  Chờ TT  ",
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(146, 64, 14),
                BackColor = Color.FromArgb(254, 243, 199),
                AutoSize = true,
                Location = new Point(110, 31),
            };

            // Số tiền
            var lblTien = new Label
            {
                Text = $"{tongTien:#,##0} đ",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 92, 77),
                Location = new Point(14, 52),
                AutoSize = true,
                BackColor = Color.Transparent,
            };

            // Nút thanh toán nhanh
            var lblAction = new Label
            {
                Text = "Thanh toán →",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 92, 77),
                AutoSize = true,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
            };
            lblAction.Location = new Point(card.Width - lblAction.PreferredWidth - 18, 54);

            // Hover effect
            Action<bool> setHover = (hover) =>
            {
                card.BackColor = hover ? Color.FromArgb(240, 253, 244) : Color.White;
            };
            card.MouseEnter += (s, e) => setHover(true);
            card.MouseLeave += (s, e) => setHover(false);
            foreach (Control c in new Control[] { lblTen, lblPhieu, lblBadge, lblTien, lblAction })
            {
                c.MouseEnter += (s, e) => setHover(true);
                c.MouseLeave += (s, e) => setHover(false);
            }

            // Click → Điều hướng sang Thanh Toán Hóa Đơn
            EventHandler onClick = (s, e) => DieuHuongThanhToan(maPhieu);
            card.Click += onClick;
            lblTen.Click += onClick;
            lblPhieu.Click += onClick;
            lblBadge.Click += onClick;
            lblTien.Click += onClick;
            lblAction.Click += onClick;

            card.Controls.Add(lblTen);
            card.Controls.Add(lblPhieu);
            card.Controls.Add(lblBadge);
            card.Controls.Add(lblTien);
            card.Controls.Add(lblAction);

            return card;
        }

        /// <summary>
        /// Điều hướng sang menu Thanh Toán Hóa Đơn và tự động mở phiếu khám.
        /// </summary>
        private void DieuHuongThanhToan(int maPhieuKham)
        {
            _refreshTimer?.Stop();

            MainFormLeTan mainForm = null;
            Control parent = this.Parent;
            while (parent != null)
            {
                if (parent is MainFormLeTan mf) { mainForm = mf; break; }
                parent = parent.Parent;
            }

            if (mainForm != null)
            {
                mainForm.BeginInvoke(new Action(() =>
                {
                    mainForm.ChuyenMenuThanhToan(maPhieuKham);
                }));
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN CÁC NÚT ACTION
        // ══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Tiếp nhận bệnh nhân — điều hướng sang TiepNhanForm trong pnlMdiArea
        /// của MainFormLeTan.
        /// </summary>
        private void BtnTiepNhan_Click(object sender, EventArgs e)
        {
            // Lấy SĐT từ lịch hẹn đang chọn trên dgvQueue (để tự động tìm BN khi mở TiepNhanForm)
            string sdt = null;
            if (dgvQueue.CurrentRow != null && !dgvQueue.CurrentRow.IsNewRow)
            {
                // Đọc từ DataTable source vì tên cột DGV (colSDT) khác DataPropertyName (SoDienThoai)
                var dt = dgvQueue.DataSource as DataTable;
                if (dt != null && dgvQueue.CurrentRow.Index < dt.Rows.Count)
                {
                    sdt = dt.Rows[dgvQueue.CurrentRow.Index]["SoDienThoai"]?.ToString();
                }
            }

            DieuHuongFormCon("Tiếp Nhận Bệnh Nhân", sdt);
        }

        /// <summary>Mở form Quản Lý Lịch Hẹn trong pnlMdiArea.</summary>
        private void BtnTaoLich_Click(object sender, EventArgs e)
        {
            DieuHuongFormCon("Quản Lý Lịch Hẹn");
        }

        /// <summary>Xác nhận lịch hẹn đang chọn (TrangThai 0 → 1) + tự động cấp STT.</summary>
        private void BtnXacNhan_Click(object sender, EventArgs e)
        {
            if (dgvQueue.CurrentRow == null || dgvQueue.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn một lịch hẹn để xác nhận.",
                    "Chưa chọn lịch hẹn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra trạng thái hiện tại
            int trangThai = Convert.ToInt32(dgvQueue.CurrentRow.Cells["TrangThai"].Value);
            if (trangThai != 0)
            {
                MessageBox.Show("Lịch hẹn này đã được xác nhận.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int maLichHen = Convert.ToInt32(dgvQueue.CurrentRow.Cells["MaLichHen"].Value);
            string tenBN = dgvQueue.CurrentRow.Cells["colBenhNhan"]?.Value?.ToString() ?? "";

            // ── Tự động tính STT theo khung giờ đã đặt lịch ──
            int sttGoiY = TinhSoThuTuTiepTheo(maLichHen);

            // Cho phép tùy chỉnh STT
            string sttInput = Microsoft.VisualBasic.Interaction.InputBox(
                $"Xác nhận lịch hẹn cho \"{tenBN}\"\n\n" +
                $"Số thứ tự gợi ý (theo khung giờ): {sttGoiY}\n" +
                $"Nhập STT tùy chỉnh hoặc để nguyên:",
                "Xác nhận & Cấp số thứ tự",
                sttGoiY.ToString());

            // Người dùng bấm Cancel
            if (string.IsNullOrWhiteSpace(sttInput)) return;

            if (!int.TryParse(sttInput.Trim(), out int sttChon) || sttChon <= 0)
            {
                MessageBox.Show("Số thứ tự phải là số nguyên dương.",
                    "STT không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra STT đã được sử dụng chưa
            try
            {
                var dtCheck = DatabaseConnection.ExecuteQuery(
                    @"SELECT COUNT(*) AS C FROM LichHen
                      WHERE CAST(ThoiGianHen AS DATE) = CAST(GETDATE() AS DATE)
                        AND TrangThai = 1 AND SoThuTu = @STT",
                    p => p.AddWithValue("@STT", sttChon));
                int daTonTai = dtCheck != null && dtCheck.Rows.Count > 0
                    ? Convert.ToInt32(dtCheck.Rows[0]["C"]) : 0;
                if (daTonTai > 0)
                {
                    var xn = MessageBox.Show(
                        $"STT {sttChon} đã được sử dụng cho lịch hẹn khác hôm nay.\nBạn vẫn muốn dùng STT này?",
                        "STT trùng lặp", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (xn != DialogResult.Yes) return;
                }
            }
            catch { /* bỏ qua lỗi check */ }

            try
            {
                const string sql = "UPDATE LichHen SET TrangThai = 1, SoThuTu = @STT WHERE MaLichHen = @MaLH";
                DatabaseConnection.ExecuteNonQuery(sql,
                    p => { p.AddWithValue("@MaLH", maLichHen); p.AddWithValue("@STT", sttChon); });

                MessageBox.Show($"Đã xác nhận lịch hẹn thành công! ✅\nSố thứ tự: {sttChon}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadTatCa();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xác nhận lịch hẹn:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tính STT tiếp theo dựa trên khung giờ đặt lịch hẹn.
        /// Sắp xếp theo ThoiGianHen ASC, STT = thứ tự trong danh sách hôm nay.
        /// </summary>
        private int TinhSoThuTuTiepTheo(int maLichHen)
        {
            try
            {
                // Đếm số lịch hẹn đã xác nhận hôm nay + 1
                var dt = DatabaseConnection.ExecuteQuery(
                    @"SELECT ISNULL(MAX(SoThuTu), 0) + 1 AS STTMoi
                      FROM LichHen
                      WHERE CAST(ThoiGianHen AS DATE) = CAST(GETDATE() AS DATE)
                        AND TrangThai = 1
                        AND SoThuTu IS NOT NULL");
                if (dt != null && dt.Rows.Count > 0)
                    return Convert.ToInt32(dt.Rows[0]["STTMoi"]);
            }
            catch { }
            return 1;
        }

        /// <summary>
        /// Điều hướng: gọi MainFormLeTan.ChuyenMenu để
        /// sidebar highlight + topbar + form con đều cập nhật đồng bộ.
        /// Dùng BeginInvoke để trì hoãn — tránh Dispose form hiện tại
        /// trong khi đang thực thi event handler.
        /// </summary>
        private void DieuHuongFormCon(string tenMenu, string soDienThoaiBN = null)
        {
            // Dừng timer trước khi chuyển (form sắp bị Dispose)
            _refreshTimer?.Stop();

            // Duyệt parent chain thủ công để tìm MainFormLeTan
            MainFormLeTan mainForm = null;
            Control parent = this.Parent;
            while (parent != null)
            {
                if (parent is MainFormLeTan mf)
                {
                    mainForm = mf;
                    break;
                }
                parent = parent.Parent;
            }

            if (mainForm != null)
            {
                mainForm.BeginInvoke(new Action(() =>
                {
                    mainForm.ChuyenMenu(tenMenu, soDienThoaiBN);
                }));
            }
        }
    }
}
