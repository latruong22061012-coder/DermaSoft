<<<<<<< HEAD
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Models;
=======
using System.Windows.Forms;
>>>>>>> d2fc9d190a76c0c366e0407bca6067fe95379af1

namespace DermaSoft.Forms
{
    /// <summary>
<<<<<<< HEAD
    /// Form Tiếp Nhận Bệnh Nhân — Lễ Tân tìm BN, tạo PhieuKham từ LichHen.
    /// Luồng: Tìm BN theo SĐT → chọn Bác sĩ + Lịch hẹn → Tiếp nhận → INSERT PhieuKham.
    /// </summary>
    public partial class TiepNhanForm : Form
    {
        // ── State ─────────────────────────────────────────────────────────────
        private int _maBNDangChon = -1;   // BN đang được tìm thấy
        private int _maLichHenChon = -1;  // Lịch hẹn đang chọn trong cmbLichHen

        // ── Màu badge hạng TV ──────────────────────────────────────────────────
        private static readonly Color ClrDo = Color.FromArgb(255, 76, 76);
        private static readonly Color ClrBac = Color.FromArgb(192, 192, 192);
        private static readonly Color ClrVang = Color.FromArgb(255, 193, 7);
        private static readonly Color ClrKimCuong = Color.FromArgb(137, 207, 240);

        // ═════════════════════════════════════════════════════════════════════
        // KHỞI TẠO
        // ═════════════════════════════════════════════════════════════════════

        public TiepNhanForm()
        {
            InitializeComponent();

            this.Load += TiepNhanForm_Load;
            btnTim.Click += BtnTim_Click;
            btnBNMoi.Click += BtnBNMoi_Click;
            txtTimKiem.KeyDown += TxtTimKiem_KeyDown;
            cmbBacSi.SelectedIndexChanged += CmbBacSi_SelectedIndexChanged;
            btnTiepNhan.Click += BtnTiepNhan_Click;
            btnHuy.Click += BtnHuy_Click;
        }

        private void TiepNhanForm_Load(object sender, EventArgs e)
        {
            LoadDanhSachBacSi();
            LoadQueue();
            DatCheDoBan(enabled: false);
        }

        // ═════════════════════════════════════════════════════════════════════
        // LOAD DỮ LIỆU
        // ═════════════════════════════════════════════════════════════════════

        /// <summary>Load danh sách bác sĩ đang hoạt động vào cmbBacSi.</summary>
        private void LoadDanhSachBacSi()
        {
            try
            {
                // Query đơn giản — tên bác sĩ không kèm ca
                const string sqlSimple = @"
                    SELECT MaNguoiDung, HoTen AS TenHienThi
                    FROM NguoiDung
                    WHERE MaVaiTro = 2
                      AND TrangThaiTK = 1
                      AND IsDeleted   = 0
                    ORDER BY HoTen";

                DataTable dt = DatabaseConnection.ExecuteQuery(sqlSimple);

                cmbBacSi.DataSource = dt;
                cmbBacSi.DisplayMember = "TenHienThi";
                cmbBacSi.ValueMember = "MaNguoiDung";

                if (dt.Rows.Count > 0)
                    cmbBacSi.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách bác sĩ:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Load queue bệnh nhân hôm nay (PhieuKham TrangThai = 0 hoặc 1).</summary>
        private void LoadQueue()
        {
            try
            {
                const string sql = @"
                    SELECT
                        ROW_NUMBER() OVER (ORDER BY pk.NgayKham ASC)   AS STT,
                        pk.MaPhieuKham,
                        bn.HoTen,
                        FORMAT(pk.NgayKham, N'HH:mm')                  AS GioTiepNhan,
                        ISNULL(nd.HoTen, N'Chưa phân')                 AS TenBacSi,
                        CASE pk.TrangThai
                            WHEN 0 THEN N'Chờ khám'
                            WHEN 1 THEN N'Đang khám'
                            WHEN 2 THEN N'Hoàn thành'
                            ELSE        N'Khác'
                        END                                             AS TrangThaiText
                    FROM PhieuKham pk
                    JOIN BenhNhan  bn ON pk.MaBenhNhan  = bn.MaBenhNhan
                    LEFT JOIN NguoiDung nd ON pk.MaNguoiDung = nd.MaNguoiDung
                    WHERE CAST(pk.NgayKham AS DATE) = CAST(GETDATE() AS DATE)
                      AND pk.TrangThai IN (0, 1)
                      AND pk.IsDeleted = 0
                    ORDER BY pk.NgayKham ASC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql);
                dgvQueue.AutoGenerateColumns = false;
                dgvQueue.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải queue:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load lịch hẹn hôm nay của BN được chọn vào cmbLichHen.
        /// Chỉ lấy lịch TrangThai = 0 (Chờ) hoặc 1 (Đã xác nhận).
        /// </summary>
        private void LoadLichHenCuaBN(int maBN)
        {
            try
            {
                const string sql = @"
                    SELECT
                        lh.MaLichHen,
                        N'LH#' + CAST(lh.MaLichHen AS NVARCHAR(10))
                            + N' - ' + FORMAT(lh.ThoiGianHen, N'HH:mm')
                            + N' ' + ISNULL(bn.HoTen, N'')           AS TenHienThi
                    FROM LichHen lh
                    JOIN BenhNhan bn ON lh.MaBenhNhan = bn.MaBenhNhan
                    WHERE lh.MaBenhNhan = @MaBN
                      AND lh.TrangThai IN (0, 1)
                      AND CAST(lh.ThoiGianHen AS DATE) = CAST(GETDATE() AS DATE)
                    ORDER BY lh.ThoiGianHen ASC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBN", maBN));

                // Thêm option "Không có lịch hẹn" ở đầu
                DataRow row = dt.NewRow();
                row["MaLichHen"] = -1;
                row["TenHienThi"] = "-- Không có lịch hẹn --";
                dt.Rows.InsertAt(row, 0);

                cmbLichHen.DataSource = dt;
                cmbLichHen.DisplayMember = "TenHienThi";
                cmbLichHen.ValueMember = "MaLichHen";
                cmbLichHen.SelectedIndex = 0;
            }
            catch
            {
                cmbLichHen.DataSource = null;
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // TÌM KIẾM BỆNH NHÂN
        // ═════════════════════════════════════════════════════════════════════

        private void TxtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                TimKiemBenhNhan();
            }
        }

        private void BtnTim_Click(object sender, EventArgs e) => TimKiemBenhNhan();

        private void TimKiemBenhNhan()
        {
            string tuKhoa = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại hoặc tên bệnh nhân.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                const string sql = @"
                    SELECT TOP 1
                        bn.MaBenhNhan,
                        bn.HoTen,
                        ISNULL(FORMAT(bn.NgaySinh, N'dd/MM/yyyy'), N'—') AS NgaySinhText,
                        CASE bn.GioiTinh WHEN 1 THEN N'Nam' WHEN 0 THEN N'Nữ' ELSE N'—' END AS GioiTinhText,
                        ISNULL(bn.SoDienThoai, N'—')                     AS SoDienThoai,
                        ISNULL(bn.TienSuBenhLy, N'')                     AS TienSuBenhLy,
                        ISNULL(htv.TenHang, N'')                         AS TenHang,
                        ISNULL(htv.MauHangHex, N'')                      AS MauHangHex
                    FROM BenhNhan bn
                    LEFT JOIN ThanhVienInfo   tvi ON bn.MaBenhNhan = tvi.MaBenhNhan
                    LEFT JOIN HangThanhVien   htv ON tvi.MaHang    = htv.MaHang
                    WHERE bn.IsDeleted = 0
                      AND (bn.SoDienThoai LIKE @TuKhoa OR bn.HoTen LIKE @TuKhoa)
                    ORDER BY bn.MaBenhNhan DESC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@TuKhoa", "%" + tuKhoa + "%"));

                if (dt == null || dt.Rows.Count == 0)
                {
                    pnlBNResult.Visible = false;
                    _maBNDangChon = -1;
                    DatCheDoBan(enabled: false);
                    MessageBox.Show($"Không tìm thấy bệnh nhân với từ khóa \"{tuKhoa}\".\nBấm BN Mới để tạo bệnh nhân mới.",
                        "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataRow r = dt.Rows[0];
                HienThiKetQuaBN(r);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiKetQuaBN(DataRow r)
        {
            _maBNDangChon = Convert.ToInt32(r["MaBenhNhan"]);

            // Điền thông tin BN vào card
            lblBNTen.Text = r["HoTen"]?.ToString() ?? "";
            lblBNInfo.Text = $"📞 {r["SoDienThoai"]}  •  📅 {r["NgaySinhText"]}  •  {(r["GioiTinhText"].ToString() == "Nam" ? "♂" : "♀")} {r["GioiTinhText"]}";

            string tienSu = r["TienSuBenhLy"]?.ToString() ?? "";
            lblBNTienSu.Text = string.IsNullOrWhiteSpace(tienSu) ? "" : "⚠️  " + tienSu;
            lblBNTienSu.Visible = !string.IsNullOrWhiteSpace(tienSu);

            // Badge hạng thành viên
            string tenHang = r["TenHang"]?.ToString() ?? "";
            if (!string.IsNullOrEmpty(tenHang))
            {
                lblBNHang.Text = tenHang;
                lblBNHang.BackColor = Color.Transparent;
                lblBNHang.AutoSize = false;
                lblBNHang.Visible = true;
            }
            else
            {
                lblBNHang.Visible = false;
            }

            // Avatar chữ cái đầu
            VeAvatar(lblBNTen.Text);

            pnlBNResult.Visible = true;

            // Điền txtBenhNhanDisplay bên phải
            txtBenhNhanDisplay.Text = lblBNTen.Text;

            // Load lịch hẹn của BN này
            LoadLichHenCuaBN(_maBNDangChon);
            DatCheDoBan(enabled: true);
        }

        private Color LayMauHang(string tenHang)
        {
            if (tenHang.Contains("Kim Cương")) return ClrKimCuong;
            if (tenHang.Contains("Vàng")) return ClrVang;
            if (tenHang.Contains("Bạc")) return ClrBac;
            if (tenHang.Contains("Đỏ")) return ClrDo;
            return Color.FromArgb(209, 213, 219);
        }

        // ═════════════════════════════════════════════════════════════════════
        // VẼ AVATAR TRÒN
        // ═════════════════════════════════════════════════════════════════════

        private void VeAvatar(string hoTen)
        {
            string chuCai = string.IsNullOrEmpty(hoTen) ? "?"
                            : hoTen.Trim()[0].ToString().ToUpper();

            int size = picAvatarBN.Width > 0 ? picAvatarBN.Width : 80;
            var bmp = new System.Drawing.Bitmap(size, size);

            using (var g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Vẽ nền tròn xanh
                using (var brush = new System.Drawing.SolidBrush(Color.FromArgb(15, 92, 77)))
                    g.FillEllipse(brush, 0, 0, size - 1, size - 1);

                // Vẽ chữ cái trắng ở giữa
                using (var font = new Font("Segoe UI", size * 0.38f, FontStyle.Bold, GraphicsUnit.Pixel))
                using (var brush = new System.Drawing.SolidBrush(Color.White))
                {
                    var sz = g.MeasureString(chuCai, font);
                    g.DrawString(chuCai, font, brush,
                        (size - sz.Width) / 2f,
                        (size - sz.Height) / 2f);
                }
            }

            picAvatarBN.Image = bmp;
            picAvatarBN.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        // ═════════════════════════════════════════════════════════════════════
        // BN MỚI — Mở PatientForm để tạo bệnh nhân mới
        // ═════════════════════════════════════════════════════════════════════

        private void BtnBNMoi_Click(object sender, EventArgs e)
        {
            var frm = new PatientForm();
            frm.TopLevel = true;
            frm.FormBorderStyle = FormBorderStyle.FixedDialog;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.MaximizeBox = false;
            frm.MinimizeBox = false;
            frm.ShowDialog(this);

            // Sau khi đóng PatientForm, điền lại SĐT vừa tạo nếu có
            // (PatientForm không trả về dữ liệu trực tiếp — người dùng quay lại tìm kiếm)
        }

        // ═════════════════════════════════════════════════════════════════════
        // CmbBacSi changed — không cần reload vì lịch hẹn theo BN, không theo BS
        // ═════════════════════════════════════════════════════════════════════

        private void CmbBacSi_SelectedIndexChanged(object sender, EventArgs e) { }

        // ═════════════════════════════════════════════════════════════════════
        // TIẾP NHẬN & TẠO PHIẾU KHÁM
        // ═════════════════════════════════════════════════════════════════════

        private void BtnTiepNhan_Click(object sender, EventArgs e)
        {
            // Validate bệnh nhân
            if (_maBNDangChon <= 0)
            {
                MessageBox.Show("Vui lòng tìm và chọn bệnh nhân trước.",
                    "Chưa chọn bệnh nhân", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTimKiem.Focus();
                return;
            }

            // Validate bác sĩ
            if (cmbBacSi.SelectedValue == null || !int.TryParse(cmbBacSi.SelectedValue.ToString(), out int maBacSi) || maBacSi <= 0)
            {
                MessageBox.Show("Vui lòng chọn bác sĩ phụ trách.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbBacSi.Focus();
                return;
            }

            // Lịch hẹn (tùy chọn)
            int? maLichHen = null;
            if (cmbLichHen.SelectedValue != null &&
                int.TryParse(cmbLichHen.SelectedValue.ToString(), out int lh) && lh > 0)
                maLichHen = lh;

            string trieuChung = txtTrieuChung.Text.Trim();
            string ghiChu = txtGhiChu.Text.Trim();

            // Xác nhận
            string tenBN = txtBenhNhanDisplay.Text;
            var confirm = MessageBox.Show(
                $"Xác nhận tiếp nhận bệnh nhân \"{tenBN}\"?\n\nBác sĩ: {cmbBacSi.Text}\nLịch hẹn: {(maLichHen.HasValue ? cmbLichHen.Text : "Không có")}",
                "Xác nhận tiếp nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                TaoPhieuKham(maBacSi, maLichHen, trieuChung, ghiChu);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo phiếu khám:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TaoPhieuKham(int maBacSi, int? maLichHen, string trieuChung, string ghiChu)
        {
            bool ok = DatabaseConnection.ExecuteTransaction((conn, tran) =>
            {
                // 1. INSERT PhieuKham
                var cmdPK = new System.Data.SqlClient.SqlCommand(@"
                    INSERT INTO PhieuKham
                        (MaBenhNhan, MaNguoiDung, MaLichHen, NgayKham, TrieuChung, GhiChu, TrangThai)
                    VALUES
                        (@MaBN, @MaBS, @MaLH, GETDATE(), @TrieuChung, @GhiChu, 0);
                    SELECT SCOPE_IDENTITY();", conn, tran);

                cmdPK.Parameters.AddWithValue("@MaBN", _maBNDangChon);
                cmdPK.Parameters.AddWithValue("@MaBS", maBacSi);
                cmdPK.Parameters.AddWithValue("@MaLH", maLichHen.HasValue ? (object)maLichHen.Value : DBNull.Value);
                cmdPK.Parameters.AddWithValue("@TrieuChung", string.IsNullOrEmpty(trieuChung) ? (object)DBNull.Value : trieuChung);
                cmdPK.Parameters.AddWithValue("@GhiChu", string.IsNullOrEmpty(ghiChu) ? (object)DBNull.Value : ghiChu);

                int maPhieuKham = Convert.ToInt32(cmdPK.ExecuteScalar());

                // 2. Cập nhật TrangThai LichHen → 2 (Hoàn thành) nếu có liên kết
                if (maLichHen.HasValue)
                {
                    var cmdLH = new System.Data.SqlClient.SqlCommand(
                        "UPDATE LichHen SET TrangThai = 1 WHERE MaLichHen = @MaLH",
                        conn, tran);
                    cmdLH.Parameters.AddWithValue("@MaLH", maLichHen.Value);
                    cmdLH.ExecuteNonQuery();
                }
            });

            if (ok)
            {
                MessageBox.Show($"Đã tiếp nhận và tạo phiếu khám cho \"{txtBenhNhanDisplay.Text}\" thành công! ✅",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetForm();
                LoadQueue(); // Refresh queue
            }
            else
            {
                MessageBox.Show("Không thể tạo phiếu khám. Vui lòng thử lại.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // HỦY
        // ═════════════════════════════════════════════════════════════════════

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        // ═════════════════════════════════════════════════════════════════════
        // HELPERS
        // ═════════════════════════════════════════════════════════════════════

        private void ResetForm()
        {
            _maBNDangChon = -1;
            txtTimKiem.Text = "";
            txtBenhNhanDisplay.Text = "";
            txtTrieuChung.Text = "";
            txtGhiChu.Text = "";
            pnlBNResult.Visible = false;
            cmbLichHen.DataSource = null;
            DatCheDoBan(enabled: false);
            txtTimKiem.Focus();
        }

        private void DatCheDoBan(bool enabled)
        {
            cmbBacSi.Enabled = enabled;
            cmbLichHen.Enabled = enabled;
            txtTrieuChung.Enabled = enabled;
            txtGhiChu.Enabled = enabled;
            btnTiepNhan.Enabled = enabled;
            btnHuy.Enabled = enabled;
        }

        // ═════════════════════════════════════════════════════════════════════
        // STUB EVENTS SINH BỞI DESIGNER
        // ═════════════════════════════════════════════════════════════════════

        private void label2_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2GradientButton1_Click(object sender, EventArgs e) => BtnTiepNhan_Click(sender, e);
    }
}
=======
    /// Form Tiếp Nhận Bệnh Nhân — frm-tiepnhan trong wireframe.
    /// Lễ tân: tìm kiếm BenhNhan, tạo PhieuKham mới từ LichHen.
    /// </summary>
    public partial class TiepNhanForm : Form
    {
        public TiepNhanForm()
        {
            InitializeComponent();
        }
    }
}
>>>>>>> d2fc9d190a76c0c366e0407bca6067fe95379af1
