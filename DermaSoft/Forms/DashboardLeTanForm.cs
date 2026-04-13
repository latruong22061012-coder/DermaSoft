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
            LoadKpiCards();
            LoadLichHenHomNay();
            LoadHoaDonCanThu();
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

                    // Card 3: Hóa đơn chưa thanh toán (TrangThai = 0)
                    using (var cmd = new SqlCommand(
                        @"SELECT COUNT(*) FROM HoaDon
                          WHERE TrangThai = 0
                            AND IsDeleted = 0", conn))
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
                            WHEN 1 THEN N'Đã XN'
                            WHEN 2 THEN N'Hoàn thành'
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
            flpHoaDonList.Controls.Clear();

            try
            {
                const string sql = @"
                    SELECT TOP 10
                        hd.MaHoaDon,
                        hd.MaPhieuKham,
                        ISNULL(bn.HoTen, N'Bệnh nhân') AS TenBenhNhan,
                        hd.TongTien
                    FROM HoaDon hd
                    LEFT JOIN PhieuKham  pk ON hd.MaPhieuKham = pk.MaPhieuKham
                    LEFT JOIN BenhNhan   bn ON pk.MaBenhNhan   = bn.MaBenhNhan
                    WHERE hd.TrangThai = 0
                      AND hd.IsDeleted = 0
                    ORDER BY hd.NgayTao DESC";

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
                    int maHoaDon = Convert.ToInt32(row["MaHoaDon"]);
                    int maPhieu = Convert.ToInt32(row["MaPhieuKham"]);
                    string tenBN = row["TenBenhNhan"].ToString();
                    decimal tongTien = Convert.ToDecimal(row["TongTien"]);

                    flpHoaDonList.Controls.Add(TaoHoaDonCard(maHoaDon, maPhieu, tenBN, tongTien));
                }
            }
            catch { }
        }

        /// <summary>
        /// Tạo card nhỏ cho một hóa đơn trong danh sách bên phải.
        /// Layout: Tên BN (bold) | Phiếu #XXXX | Số tiền xanh lá
        /// </summary>
        private Panel TaoHoaDonCard(int maHoaDon, int maPhieu, string tenBN, decimal tongTien)
        {
            // Card container
            var card = new Panel
            {
                Size = new Size(flpHoaDonList.Width - 20, 72),
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Tag = maHoaDon,
                Padding = new Padding(12, 10, 12, 10),
            };
            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1f))
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            // Tên bệnh nhân
            var lblTen = new Label
            {
                Text = tenBN,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 46, 37),
                Location = new Point(12, 10),
                AutoSize = true,
                BackColor = Color.Transparent,
            };

            // Mã phiếu
            var lblPhieu = new Label
            {
                Text = $"Phiếu #{maPhieu:D4}",
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(12, 32),
                AutoSize = true,
                BackColor = Color.Transparent,
            };

            // Số tiền
            var lblTien = new Label
            {
                Text = $"{tongTien:#,##0} đ",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 92, 77),
                Location = new Point(12, 50),
                AutoSize = true,
                BackColor = Color.Transparent,
            };

            // Hover effect
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(245, 251, 247);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            card.Controls.Add(lblTen);
            card.Controls.Add(lblPhieu);
            card.Controls.Add(lblTien);

            return card;
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN CÁC NÚT ACTION
        // ══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Tiếp nhận bệnh nhân từ lịch hẹn đang chọn trong bảng.
        /// Tạo phiếu khám mới và chuyển trạng thái lịch hẹn → Hoàn thành (2).
        /// </summary>
        private void BtnTiepNhan_Click(object sender, EventArgs e)
        {
            if (dgvQueue.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một lịch hẹn để tiếp nhận.",
                    "Chưa chọn lịch hẹn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Mở TiepNhanForm với MaLichHen từ hàng đang chọn
            // Tạm thời thông báo placeholder
            MessageBox.Show("Tính năng Tiếp Nhận sẽ mở form Tiếp Nhận Bệnh Nhân.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>Mở form Quản Lý Lịch Hẹn để tạo lịch mới.</summary>
        private void BtnTaoLich_Click(object sender, EventArgs e)
        {
            // TODO: Điều hướng sang AppointmentForm qua MainFormLeTan
            MessageBox.Show("Tính năng Tạo Lịch Mới sẽ mở form Quản Lý Lịch Hẹn.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>Xác nhận lịch hẹn đang chọn (TrangThai 0 → 1).</summary>
        private void BtnXacNhan_Click(object sender, EventArgs e)
        {
            if (dgvQueue.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một lịch hẹn để xác nhận.",
                    "Chưa chọn lịch hẹn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Cập nhật TrangThai lịch hẹn lên 1 (Đã xác nhận) trong DB
            MessageBox.Show("Tính năng Xác Nhận sẽ cập nhật trạng thái lịch hẹn.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
