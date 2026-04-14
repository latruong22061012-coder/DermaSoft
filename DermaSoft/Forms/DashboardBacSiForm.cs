using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DermaSoft.Data;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Dashboard dành cho Bác Sĩ.
    /// Hiển thị: 3 KPI cards | Queue bệnh nhân chờ khám | Lịch làm việc tuần này
    /// </summary>
    public partial class DashboardBacSiForm : Form
    {
        private int _maBacSi = -1;

        public DashboardBacSiForm()
        {
            InitializeComponent();

            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true);
            this.UpdateStyles();

            this.Load += DashboardBacSiForm_Load;

            btnBatDauKham.Click += BtnBatDauKham_Click;
            btnXemHoSo.Click += BtnXemHoSo_Click;
        }

        private void DashboardBacSiForm_Load(object sender, EventArgs e)
        {
            var nd = LoginForm.NguoiDungHienTai;
            if (nd != null) _maBacSi = nd.MaNguoiDung;

            // ── Thêm cột dgvQueue ─────────────────────────────────────────────
            dgvQueue.AutoGenerateColumns = false;
            dgvQueue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvQueue.Columns.Clear();
            dgvQueue.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSTT", DataPropertyName = "STT", HeaderText = "#", FillWeight = 40F, ReadOnly = true });
            dgvQueue.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTenBenhNhan", DataPropertyName = "HoTen", HeaderText = "Bệnh nhân", FillWeight = 160F, ReadOnly = true });
            dgvQueue.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSDT", DataPropertyName = "SoDienThoai", HeaderText = "SĐT", FillWeight = 100F, ReadOnly = true });
            dgvQueue.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTrieuChung", DataPropertyName = "TrieuChung", HeaderText = "Triệu chứng", FillWeight = 140F, ReadOnly = true });
            dgvQueue.Columns.Add(new DataGridViewTextBoxColumn { Name = "colChoPhut", DataPropertyName = "ChoPhut", HeaderText = "Chờ", FillWeight = 60F, ReadOnly = true });

            // ── Thêm cột dgvLich ──────────────────────────────────────────────
            dgvLich.AutoGenerateColumns = false;
            dgvLich.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLich.Columns.Clear();
            dgvLich.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNgay", DataPropertyName = "NgayText", HeaderText = "Ngày", FillWeight = 130F, ReadOnly = true });
            dgvLich.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCa", DataPropertyName = "TenCa", HeaderText = "Ca", FillWeight = 110F, ReadOnly = true });
            dgvLich.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGio", DataPropertyName = "GioLamViec", HeaderText = "Giờ", FillWeight = 110F, ReadOnly = true });
            dgvLich.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDiemDanh", DataPropertyName = "DiemDanhText", HeaderText = "Điểm danh", FillWeight = 100F, ReadOnly = true });

            LoadStatCards();
            LoadQueue();
            LoadLichLamViec();
        }

        // ══════════════════════════════════════════════════════════════════
        // STAT CARDS
        // ══════════════════════════════════════════════════════════════════

        private void LoadStatCards()
        {
            try
            {
                // Card 1: Bệnh nhân đang chờ khám hôm nay (TrangThai = 0)
                object val1 = DatabaseConnection.ExecuteScalar(@"
                    SELECT COUNT(*) FROM PhieuKham
                    WHERE MaNguoiDung = @MaBS
                      AND CAST(NgayKham AS DATE) = CAST(GETDATE() AS DATE)
                      AND TrangThai = 0
                      AND IsDeleted = 0",
                    p => p.AddWithValue("@MaBS", _maBacSi));
                lblCard1Value.Text = val1?.ToString() ?? "0";

                // Card 2: Đã khám hôm nay (TrangThai >= 2 = Hoàn thành hoặc Đã TT)
                object val2 = DatabaseConnection.ExecuteScalar(@"
                    SELECT COUNT(*) FROM PhieuKham
                    WHERE MaNguoiDung = @MaBS
                      AND CAST(NgayKham AS DATE) = CAST(GETDATE() AS DATE)
                      AND TrangThai >= 2
                      AND IsDeleted = 0",
                    p => p.AddWithValue("@MaBS", _maBacSi));
                lblCard2Value.Text = val2?.ToString() ?? "0";

                // Card 3: Ca làm việc hiện tại
                string tenCa = LayTenCaHienTai();
                lblCard3Value.Text = string.IsNullOrEmpty(tenCa) ? "—" : tenCa;
            }
            catch (Exception ex)
            {
                lblCard1Value.Text = "!";
                lblCard2Value.Text = "!";
                lblCard3Value.Text = ex.Message.Length > 20 ? "Lỗi" : ex.Message;
            }
        }

        private string LayTenCaHienTai()
        {
            try
            {
                // Lấy ca làm việc của bác sĩ hôm nay mà giờ hiện tại nằm trong khoảng GioBatDau–GioKetThuc
                var now = DateTime.Now.TimeOfDay;
                object result = DatabaseConnection.ExecuteScalar(@"
                    SELECT TOP 1 clv.TenCa
                    FROM PhanCongCa pcc
                    JOIN CaLamViec clv ON pcc.MaCa = clv.MaCa
                    WHERE pcc.MaNguoiDung = @MaBS
                      AND pcc.NgayLamViec = CAST(GETDATE() AS DATE)
                      AND clv.GioBatDau <= CAST(GETDATE() AS TIME)
                      AND clv.GioKetThuc >= CAST(GETDATE() AS TIME)
                    ORDER BY clv.GioBatDau",
                    p => p.AddWithValue("@MaBS", _maBacSi));

                if (result == null || result == DBNull.Value)
                {
                    // Nếu không có ca đang chạy, lấy ca tiếp theo trong ngày
                    result = DatabaseConnection.ExecuteScalar(@"
                        SELECT TOP 1 clv.TenCa
                        FROM PhanCongCa pcc
                        JOIN CaLamViec clv ON pcc.MaCa = clv.MaCa
                        WHERE pcc.MaNguoiDung = @MaBS
                          AND pcc.NgayLamViec = CAST(GETDATE() AS DATE)
                        ORDER BY clv.GioBatDau",
                        p => p.AddWithValue("@MaBS", _maBacSi));
                }

                return result?.ToString() ?? "";
            }
            catch { return ""; }
        }

        // ══════════════════════════════════════════════════════════════════
        // QUEUE BỆNH NHÂN CHỜ KHÁM
        // ══════════════════════════════════════════════════════════════════

        private void LoadQueue()
        {
            try
            {
                const string sql = @"
                    SELECT
                        ROW_NUMBER() OVER (ORDER BY pk.NgayKham ASC)  AS STT,
                        pk.MaPhieuKham,
                        pk.MaBenhNhan,
                        bn.HoTen,
                        ISNULL(bn.SoDienThoai, N'—')                  AS SoDienThoai,
                        ISNULL(pk.TrieuChung, N'—')                   AS TrieuChung,
                        CAST(
                            DATEDIFF(MINUTE, pk.NgayKham, GETDATE())
                        AS NVARCHAR(10)) + N' phút'                   AS ChoPhut
                    FROM PhieuKham pk
                    JOIN BenhNhan  bn ON pk.MaBenhNhan = bn.MaBenhNhan
                    LEFT JOIN LichHen lh ON pk.MaLichHen = lh.MaLichHen
                    WHERE pk.MaNguoiDung = @MaBS
                      AND CAST(pk.NgayKham AS DATE) = CAST(GETDATE() AS DATE)
                      AND pk.TrangThai = 0
                      AND pk.IsDeleted = 0
                      AND (pk.MaLichHen IS NULL OR lh.TrangThai <> 3)
                    ORDER BY pk.NgayKham ASC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBS", _maBacSi));

                dgvQueue.AutoGenerateColumns = false;
                dgvQueue.DataSource = dt;

                // Tô màu hàng đầu tiên (bệnh nhân tiếp theo)
                TomauQueueRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải queue:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TomauQueueRow()
        {
            if (dgvQueue.Rows.Count == 0) return;

            // Hàng đầu tiên = bệnh nhân tiếp theo → tô xanh nhạt đậm hơn
            dgvQueue.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(209, 243, 220);
            dgvQueue.Rows[0].DefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        }

        // ══════════════════════════════════════════════════════════════════
        // LỊCH LÀM VIỆC TUẦN NÀY
        // ══════════════════════════════════════════════════════════════════

        private void LoadLichLamViec()
        {
            try
            {
                const string sql = @"
                    SELECT
                        N'Thứ ' + CAST(DATEPART(WEEKDAY, pcc.NgayLamViec) AS NVARCHAR(1))
                            + N' (' + FORMAT(pcc.NgayLamViec, 'dd/MM') + N')'  AS NgayText,
                        clv.TenCa,
                        FORMAT(clv.GioBatDau,  N'hh\:mm') + N'–'
                        + FORMAT(clv.GioKetThuc, N'hh\:mm')                    AS GioLamViec,
                        CASE pcc.TrangThaiDiemDanh
                            WHEN 1 THEN N'✓ Có mặt'
                            WHEN 0 THEN N'Chờ'
                            ELSE        N'—'
                        END                                                     AS DiemDanhText,
                        pcc.TrangThaiDiemDanh
                    FROM PhanCongCa pcc
                    JOIN CaLamViec  clv ON pcc.MaCa = clv.MaCa
                    WHERE pcc.MaNguoiDung = @MaBS
                      AND pcc.NgayLamViec BETWEEN CAST(GETDATE() AS DATE)
                                              AND DATEADD(DAY, 6, CAST(GETDATE() AS DATE))
                    ORDER BY pcc.NgayLamViec, clv.GioBatDau";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBS", _maBacSi));

                dgvLich.AutoGenerateColumns = false;
                dgvLich.DataSource = dt;

                // Tô màu theo trạng thái điểm danh
                TomauLichRow(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch làm việc:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TomauLichRow(DataTable dt)
        {
            for (int i = 0; i < dgvLich.Rows.Count; i++)
            {
                if (i >= dt.Rows.Count) break;
                int trangThai = Convert.ToInt32(dt.Rows[i]["TrangThaiDiemDanh"]);

                var style = dgvLich.Rows[i].Cells["colDiemDanh"].Style;
                if (trangThai == 1)
                {
                    // Có mặt → xanh lá
                    style.BackColor = Color.FromArgb(209, 243, 220);
                    style.ForeColor = Color.FromArgb(15, 92, 77);
                    style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }
                else
                {
                    // Chờ → vàng nhạt
                    style.BackColor = Color.FromArgb(255, 243, 199);
                    style.ForeColor = Color.FromArgb(146, 64, 14);
                    style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }
                style.SelectionBackColor = style.BackColor;
                style.SelectionForeColor = style.ForeColor;
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // BUTTONS
        // ══════════════════════════════════════════════════════════════════

        private void BtnBatDauKham_Click(object sender, EventArgs e)
        {
            if (dgvQueue.CurrentRow == null || dgvQueue.Rows.Count == 0)
            {
                MessageBox.Show("Không có bệnh nhân nào đang chờ.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dt = (DataTable)dgvQueue.DataSource;
            int rowIdx = dgvQueue.CurrentRow.Index;
            int maPhieuKham = Convert.ToInt32(dt.Rows[rowIdx]["MaPhieuKham"]);
            string tenBN = dt.Rows[rowIdx]["HoTen"]?.ToString() ?? "";

            var confirm = MessageBox.Show(
                $"Bắt đầu khám cho \"{tenBN}\"?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                // Kiểm tra lịch hẹn liên kết có bị hủy không
                DataTable dtCheck = DatabaseConnection.ExecuteQuery(@"
                    SELECT lh.TrangThai FROM PhieuKham pk
                    JOIN LichHen lh ON pk.MaLichHen = lh.MaLichHen
                    WHERE pk.MaPhieuKham = @MaPK AND pk.MaLichHen IS NOT NULL",
                    p => p.AddWithValue("@MaPK", maPhieuKham));
                if (dtCheck != null && dtCheck.Rows.Count > 0
                    && Convert.ToInt32(dtCheck.Rows[0]["TrangThai"]) == 3)
                {
                    MessageBox.Show(
                        "Lịch hẹn liên kết với phiếu khám này đã bị hủy.\nKhông thể bắt đầu khám.",
                        "Lịch hẹn đã hủy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadQueue();
                    return;
                }

                // Cập nhật trạng thái → Đang khám (1)
                DatabaseConnection.ExecuteNonQuery(
                    "UPDATE PhieuKham SET TrangThai = 1 WHERE MaPhieuKham = @MaPK",
                    p => p.AddWithValue("@MaPK", maPhieuKham));

                // Mở PhieuKhamForm vào pnlMdiArea của MainFormBacSi
                MoPhieuKhamForm(maPhieuKham);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXemHoSo_Click(object sender, EventArgs e)
        {
            if (dgvQueue.CurrentRow == null || dgvQueue.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn bệnh nhân trong danh sách.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dt = (DataTable)dgvQueue.DataSource;
            int rowIdx = dgvQueue.CurrentRow.Index;
            int maBN = Convert.ToInt32(dt.Rows[rowIdx]["MaBenhNhan"]);

            // Mở Hồ sơ bệnh nhân qua sidebar
            Panel pnlMdi = this.Parent as Panel;
            MainFormBacSi mainForm = pnlMdi?.Parent as MainFormBacSi;
            if (mainForm != null)
            {
                mainForm.ChuyenMenu("Hồ Sơ Bệnh Nhân");
            }

            var detailForm = new BenhNhanDetailForm(maBN);
            if (pnlMdi != null)
                Helpers.DoubleBufferHelper.NhungFormCon(pnlMdi, detailForm);
        }

        // ══════════════════════════════════════════════════════════════════
        // STUB EVENT từ Designer
        // ══════════════════════════════════════════════════════════════════
        private void pnlLichLamViec_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }

        /// <summary>
        /// Mở PhieuKhamForm với MaPhieuKham chỉ định vào pnlMdiArea của MainFormBacSi.
        /// DashboardBacSiForm nằm trong pnlMdiArea → this.Parent = pnlMdiArea.
        /// </summary>
        private void MoPhieuKhamForm(int maPhieuKham)
        {
            Panel pnlMdi = this.Parent as Panel;
            if (pnlMdi == null)
            {
                MessageBox.Show("Không tìm được vùng hiển thị.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cập nhật topbar + sidebar của MainFormBacSi
            MainFormBacSi mainForm = pnlMdi.Parent as MainFormBacSi;
            if (mainForm != null)
            {
                mainForm.ChuyenMenu("Phiếu Khám Bệnh");
            }

            var frm = new PhieuKhamForm(maPhieuKham);
            Helpers.DoubleBufferHelper.NhungFormCon(pnlMdi, frm);
        }


    }
}
