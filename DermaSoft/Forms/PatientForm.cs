using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DermaSoft.Data;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Form Quản Lý Bệnh Nhân dành cho Lễ Tân.
    /// Trái: danh sách + tìm kiếm/lọc.
    /// Phải: chi tiết bệnh nhân + thẻ thành viên + 3 nút action.
    /// </summary>
    public partial class PatientForm : Form
    {
        // ── State ─────────────────────────────────────────────────────────────
        private int _maBNDangChon = -1;   // -1 = chưa chọn / đang thêm mới
        private bool _dangThemMoi = false;
        private string _tuKhoa = "";

        // ── Màu badge thành viên ──────────────────────────────────────────────
        private static readonly Color ClrDo = Color.FromArgb(255, 76, 76);
        private static readonly Color ClrBac = Color.FromArgb(192, 192, 192);
        private static readonly Color ClrVang = Color.FromArgb(255, 215, 0);
        private static readonly Color ClrKimCuong = Color.FromArgb(137, 207, 240);
        private static readonly Color ClrChuaCoThe = Color.FromArgb(209, 213, 219);
        private static readonly Color ClrTextDark = Color.FromArgb(55, 65, 81);

        // ═════════════════════════════════════════════════════════════════════
        // KHỞI TẠO
        // ═════════════════════════════════════════════════════════════════════

        public PatientForm()
        {
            InitializeComponent();

            // Gắn sự kiện
            this.Load += PatientForm_Load;
            btnThemMoi.Click += BtnThemMoi_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            btnLuuThayDoi.Click += BtnLuuThayDoi_Click;
            btnXemLichSuKham.Click += BtnXemLichSuKham_Click;
            btnXoa.Click += BtnXoa_Click;
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged;
            cmbLoc.SelectedIndexChanged += CmbLoc_SelectedIndexChanged;
            dgvBenhNhan.CellClick += DgvBenhNhan_CellClick;
        }

        private void PatientForm_Load(object sender, EventArgs e)
        {
            cmbLoc.SelectedIndex = 0;
            DatCheDoPanelPhai(enabled: false);
            LoadDanhSachBenhNhan();
        }

        // ═════════════════════════════════════════════════════════════════════
        // LOAD DANH SÁCH BỆNH NHÂN
        // ═════════════════════════════════════════════════════════════════════

        private void LoadDanhSachBenhNhan()
        {
            try
            {
                string sql = @"
                    SELECT
                        bn.MaBenhNhan,
                        'BN' + RIGHT('000' + CAST(bn.MaBenhNhan AS VARCHAR(10)), 3) AS MaBNText,
                        bn.HoTen,
                        ISNULL(FORMAT(bn.NgaySinh,'dd/MM/yyyy'), N'—')             AS NgaySinhText,
                        ISNULL(bn.SoDienThoai, N'—')                               AS SoDienThoai,
                        CASE bn.GioiTinh
                            WHEN 1 THEN N'Nam'
                            WHEN 0 THEN N'Nữ'
                            ELSE        N'—'
                        END                                                         AS GioiTinhText,
                        ISNULL(htv.TenHang, N'—')                                  AS HangThanhVien,
                        N'Sửa'                                                      AS ThaoTacText
                    FROM BenhNhan bn
                    LEFT JOIN ThanhVienInfo   tvi ON bn.MaBenhNhan = tvi.MaBenhNhan
                    LEFT JOIN HangThanhVien   htv ON tvi.MaHang    = htv.MaHang
                    WHERE bn.IsDeleted = 0";

                // Filter tìm kiếm
                if (!string.IsNullOrWhiteSpace(_tuKhoa))
                    sql += @" AND (bn.HoTen        LIKE @TuKhoa
                               OR bn.SoDienThoai  LIKE @TuKhoa
                               OR 'BN' + RIGHT('000' + CAST(bn.MaBenhNhan AS VARCHAR(10)), 3) LIKE @TuKhoa)";

                // Filter thành viên
                switch (cmbLoc.SelectedIndex)
                {
                    case 1: sql += " AND tvi.MaThanhVien IS NOT NULL"; break;  // Có thẻ
                    case 2: sql += " AND tvi.MaThanhVien IS NULL"; break;  // Chưa có thẻ
                }

                sql += " ORDER BY bn.HoTen ASC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql, p =>
                {
                    if (!string.IsNullOrWhiteSpace(_tuKhoa))
                        p.AddWithValue("@TuKhoa", "%" + _tuKhoa.Trim() + "%");
                });

                dgvBenhNhan.AutoGenerateColumns = false;
                dgvBenhNhan.DataSource = dt;

                TomauHangThanhVien();

                int soLuong = dt?.Rows.Count ?? 0;
                lblTongSo.Text = $"Tổng: {soLuong} bệnh nhân";
            }
            catch (Exception ex)
            {
                lblTongSo.Text = "⚠️ Lỗi tải dữ liệu";
                MessageBox.Show("Lỗi tải danh sách bệnh nhân:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // TÔ MÀU BADGE HẠNG THÀNH VIÊN
        // ═════════════════════════════════════════════════════════════════════

        private void TomauHangThanhVien()
        {
            foreach (DataGridViewRow row in dgvBenhNhan.Rows)
            {
                if (row.IsNewRow) continue;

                string hang = row.Cells["colThanhVien"].Value?.ToString() ?? "—";
                Color bg;

                if (hang.Contains("Kim Cương")) bg = ClrKimCuong;
                else if (hang.Contains("Vàng")) bg = ClrVang;
                else if (hang.Contains("Bạc")) bg = ClrBac;
                else if (hang.Contains("Đỏ")) bg = ClrDo;
                else bg = ClrChuaCoThe;

                var style = row.Cells["colThanhVien"].Style;
                style.BackColor = bg;
                style.ForeColor = ClrTextDark;
                style.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                style.SelectionBackColor = bg;
                style.SelectionForeColor = ClrTextDark;
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // LOAD CHI TIẾT BỆNH NHÂN VÀO PANEL PHẢI
        // ═════════════════════════════════════════════════════════════════════

        private void LoadChiTietBenhNhan(int maBN)
        {
            try
            {
                const string sql = @"
                    SELECT
                        bn.MaBenhNhan,
                        bn.HoTen,
                        bn.NgaySinh,
                        bn.GioiTinh,
                        bn.SoDienThoai,
                        bn.TienSuBenhLy
                    FROM BenhNhan bn
                    WHERE bn.MaBenhNhan = @MaBN AND bn.IsDeleted = 0";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBN", maBN));

                if (dt == null || dt.Rows.Count == 0) return;

                DataRow r = dt.Rows[0];

                txtHoTen.Text = r["HoTen"]?.ToString() ?? "";
                txtSDT.Text = r["SoDienThoai"]?.ToString() ?? "";
                txtTienSu.Text = r["TienSuBenhLy"]?.ToString() ?? "";

                // Ngày sinh
                if (r["NgaySinh"] != DBNull.Value)
                    dtpNgaySinh.Value = Convert.ToDateTime(r["NgaySinh"]);
                else
                    dtpNgaySinh.Value = new DateTime(1990, 1, 1);

                // Giới tính — GioiTinh: 1=Nam, 0=Nữ
                if (r["GioiTinh"] != DBNull.Value)
                    cmbGioiTinh.SelectedIndex = Convert.ToBoolean(r["GioiTinh"]) ? 1 : 0; // Nam=1, Nữ=0
                else
                    cmbGioiTinh.SelectedIndex = 0;

                LoadThanhVienInfo(maBN);
                DatCheDoPanelPhai(enabled: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết bệnh nhân:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // LOAD THÔNG TIN THẺ THÀNH VIÊN
        // ═════════════════════════════════════════════════════════════════════

        private void LoadThanhVienInfo(int maBN)
        {
            try
            {
                const string sql = @"
                    SELECT
                        htv.TenHang,
                        tvi.DiemTichLuy,
                        tvi.SoLanKham,
                        htv.MauHangHex
                    FROM ThanhVienInfo   tvi
                    JOIN HangThanhVien   htv ON tvi.MaHang = htv.MaHang
                    WHERE tvi.MaBenhNhan = @MaBN";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBN", maBN));

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    string tenHang = r["TenHang"]?.ToString() ?? "";
                    int diem = Convert.ToInt32(r["DiemTichLuy"]);
                    int soLan = Convert.ToInt32(r["SoLanKham"]);

                    lblThanhVienTitle.Text = $"💎  Thẻ Thành Viên";
                    lblThanhVienInfo.Text =
                        $"Hạng: {tenHang}\r\n" +
                        $"Điểm: {diem:N0} điểm\r\n" +
                        $"Số lần khám: {soLan} lần";
                }
                else
                {
                    lblThanhVienTitle.Text = "💎  Thẻ Thành Viên";
                    lblThanhVienInfo.Text = "Chưa có thẻ thành viên";
                }
            }
            catch
            {
                lblThanhVienInfo.Text = "Chưa có thẻ thành viên";
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // SỰ KIỆN — DataGridView click
        // ═════════════════════════════════════════════════════════════════════

        private void DgvBenhNhan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataTable dt = (DataTable)dgvBenhNhan.DataSource;
            if (dt == null || e.RowIndex >= dt.Rows.Count) return;

            // Click bất kỳ ô nào → load chi tiết
            _maBNDangChon = Convert.ToInt32(dt.Rows[e.RowIndex]["MaBenhNhan"]);
            _dangThemMoi = false;
            LoadChiTietBenhNhan(_maBNDangChon);
        }

        // ═════════════════════════════════════════════════════════════════════
        // SỰ KIỆN — Tìm kiếm & Lọc
        // ═════════════════════════════════════════════════════════════════════

        private void TxtTimKiem_TextChanged(object sender, EventArgs e)
        {
            _tuKhoa = txtTimKiem.Text;
            LoadDanhSachBenhNhan();
        }

        private void CmbLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDanhSachBenhNhan();
        }

        // ═════════════════════════════════════════════════════════════════════
        // SỰ KIỆN — Thêm Mới
        // ═════════════════════════════════════════════════════════════════════

        private void BtnThemMoi_Click(object sender, EventArgs e)
        {
            _maBNDangChon = -1;
            _dangThemMoi = true;

            // Reset form phải
            txtHoTen.Text = "";
            txtSDT.Text = "";
            txtTienSu.Text = "";
            dtpNgaySinh.Value = new DateTime(1990, 1, 1);
            cmbGioiTinh.SelectedIndex = 0;
            lblThanhVienTitle.Text = "💎  Thẻ Thành Viên";
            lblThanhVienInfo.Text = "Chưa có thẻ thành viên";

            DatCheDoPanelPhai(enabled: true);
            txtHoTen.Focus();
        }

        // ═════════════════════════════════════════════════════════════════════
        // SỰ KIỆN — Làm Mới
        // ═════════════════════════════════════════════════════════════════════

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            _tuKhoa = "";
            cmbLoc.SelectedIndex = 0;
            LoadDanhSachBenhNhan();
        }

        // ═════════════════════════════════════════════════════════════════════
        // SỰ KIỆN — Lưu Thay Đổi (INSERT hoặc UPDATE)
        // ═════════════════════════════════════════════════════════════════════

        private void BtnLuuThayDoi_Click(object sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ và tên bệnh nhân.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            // GioiTinh: Nam=index 1 → true(1), Nữ=index 0 → false(0)
            bool gioiTinh = cmbGioiTinh.SelectedIndex == 1;

            try
            {
                if (_dangThemMoi)
                    ThemMoiBenhNhan(gioiTinh);
                else
                    CapNhatBenhNhan(gioiTinh);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ThemMoiBenhNhan(bool gioiTinh)
        {
            // Kiểm tra SĐT trùng
            object check = DatabaseConnection.ExecuteScalar(
                "SELECT COUNT(*) FROM BenhNhan WHERE SoDienThoai = @SDT AND IsDeleted = 0",
                p => p.AddWithValue("@SDT", txtSDT.Text.Trim()));

            if (Convert.ToInt32(check) > 0)
            {
                MessageBox.Show("Số điện thoại này đã tồn tại trong hệ thống.",
                    "Trùng số điện thoại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            object newId = DatabaseConnection.ExecuteScalar(@"
                INSERT INTO BenhNhan (HoTen, NgaySinh, GioiTinh, SoDienThoai, TienSuBenhLy)
                VALUES (@HoTen, @NgaySinh, @GioiTinh, @SDT, @TienSu);
                SELECT SCOPE_IDENTITY();",
                p =>
                {
                    p.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                    p.AddWithValue("@NgaySinh", dtpNgaySinh.Value.Date);
                    p.AddWithValue("@GioiTinh", gioiTinh ? 1 : 0);
                    p.AddWithValue("@SDT", txtSDT.Text.Trim());
                    p.AddWithValue("@TienSu", string.IsNullOrWhiteSpace(txtTienSu.Text)
                                                    ? (object)DBNull.Value
                                                    : txtTienSu.Text.Trim());
                });

            MessageBox.Show("Đã thêm bệnh nhân mới thành công! ✅",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _maBNDangChon = Convert.ToInt32(newId);
            _dangThemMoi = false;
            LoadDanhSachBenhNhan();
            LoadChiTietBenhNhan(_maBNDangChon);
        }

        private void CapNhatBenhNhan(bool gioiTinh)
        {
            if (_maBNDangChon <= 0) return;

            // Kiểm tra SĐT trùng với bệnh nhân khác
            object check = DatabaseConnection.ExecuteScalar(
                "SELECT COUNT(*) FROM BenhNhan WHERE SoDienThoai = @SDT AND IsDeleted = 0 AND MaBenhNhan <> @MaBN",
                p =>
                {
                    p.AddWithValue("@SDT", txtSDT.Text.Trim());
                    p.AddWithValue("@MaBN", _maBNDangChon);
                });

            if (Convert.ToInt32(check) > 0)
            {
                MessageBox.Show("Số điện thoại này đã tồn tại ở bệnh nhân khác.",
                    "Trùng số điện thoại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            DatabaseConnection.ExecuteNonQuery(@"
                UPDATE BenhNhan
                SET HoTen        = @HoTen,
                    NgaySinh     = @NgaySinh,
                    GioiTinh     = @GioiTinh,
                    SoDienThoai  = @SDT,
                    TienSuBenhLy = @TienSu
                WHERE MaBenhNhan = @MaBN AND IsDeleted = 0",
                p =>
                {
                    p.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                    p.AddWithValue("@NgaySinh", dtpNgaySinh.Value.Date);
                    p.AddWithValue("@GioiTinh", gioiTinh ? 1 : 0);
                    p.AddWithValue("@SDT", txtSDT.Text.Trim());
                    p.AddWithValue("@TienSu", string.IsNullOrWhiteSpace(txtTienSu.Text)
                                                    ? (object)DBNull.Value
                                                    : txtTienSu.Text.Trim());
                    p.AddWithValue("@MaBN", _maBNDangChon);
                });

            MessageBox.Show("Đã cập nhật thông tin bệnh nhân thành công! ✅",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadDanhSachBenhNhan();
        }

        // ═════════════════════════════════════════════════════════════════════
        // SỰ KIỆN — Xóa (Soft Delete)
        // ═════════════════════════════════════════════════════════════════════

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (_maBNDangChon <= 0 || _dangThemMoi)
            {
                MessageBox.Show("Vui lòng chọn một bệnh nhân trong danh sách trước.",
                    "Chưa chọn bệnh nhân", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenBN = txtHoTen.Text;
            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn XÓA bệnh nhân \"{tenBN}\"?\n\nBệnh nhân sẽ bị ẩn khỏi hệ thống nhưng dữ liệu vẫn được lưu.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                DatabaseConnection.ExecuteNonQuery(
                    "UPDATE BenhNhan SET IsDeleted = 1 WHERE MaBenhNhan = @MaBN",
                    p => p.AddWithValue("@MaBN", _maBNDangChon));

                MessageBox.Show($"Đã xóa bệnh nhân \"{tenBN}\" thành công.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _maBNDangChon = -1;
                _dangThemMoi = false;
                DatCheDoPanelPhai(enabled: false);
                XoaTrangPanelPhai();
                LoadDanhSachBenhNhan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa bệnh nhân:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // SỰ KIỆN — Xem Lịch Sử Khám
        // ═════════════════════════════════════════════════════════════════════

        private void BtnXemLichSuKham_Click(object sender, EventArgs e)
        {
            if (_maBNDangChon <= 0 || _dangThemMoi)
            {
                MessageBox.Show("Vui lòng chọn một bệnh nhân trong danh sách trước.",
                    "Chưa chọn bệnh nhân", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                const string sql = @"
                    SELECT
                        pk.MaPhieuKham,
                        FORMAT(pk.NgayKham, 'dd/MM/yyyy HH:mm')     AS NgayKham,
                        ISNULL(nd.HoTen, N'Chưa rõ')                AS TenBacSi,
                        ISNULL(pk.ChanDoan, N'—')                    AS ChanDoan,
                        CASE pk.TrangThai
                            WHEN 0 THEN N'Mới'
                            WHEN 1 THEN N'Đang khám'
                            WHEN 2 THEN N'Hoàn thành'
                            WHEN 3 THEN N'Đã thanh toán'
                            WHEN 4 THEN N'Hủy'
                            ELSE        N'Không rõ'
                        END                                          AS TrangThaiText
                    FROM PhieuKham pk
                    LEFT JOIN NguoiDung nd ON pk.MaNguoiDung = nd.MaNguoiDung
                    WHERE pk.MaBenhNhan = @MaBN AND pk.IsDeleted = 0
                    ORDER BY pk.NgayKham DESC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBN", _maBNDangChon));

                // Hiển thị dạng dialog đơn giản
                HienThiLichSuKham(dt, txtHoTen.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử khám:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiLichSuKham(DataTable dt, string tenBN)
        {
            var frm = new Form
            {
                Text = $"Lịch sử khám — {tenBN}",
                Size = new Size(750, 480),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(246, 249, 247),
                Font = new Font("Segoe UI", 9F),
            };

            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowTemplate = { Height = 40 },
                ColumnHeadersHeight = 40,
                DataSource = dt,
            };

            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaPhieuKham", HeaderText = "Mã PK", FillWeight = 60 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NgayKham", HeaderText = "Ngày khám", FillWeight = 130 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenBacSi", HeaderText = "Bác sĩ", FillWeight = 140 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ChanDoan", HeaderText = "Chẩn đoán", FillWeight = 200 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TrangThaiText", HeaderText = "Trạng thái", FillWeight = 90 });

            // Header style
            var headerStyle = dgv.ColumnHeadersDefaultCellStyle;
            headerStyle.BackColor = Color.FromArgb(15, 92, 77);
            headerStyle.ForeColor = Color.White;
            headerStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.EnableHeadersVisualStyles = false;

            frm.Controls.Add(dgv);

            if (dt == null || dt.Rows.Count == 0)
            {
                var lblEmpty = new Label
                {
                    Text = "Bệnh nhân chưa có lịch sử khám.",
                    Dock = DockStyle.Top,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 50,
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(107, 114, 128),
                };
                frm.Controls.Add(lblEmpty);
            }

            frm.ShowDialog(this);
        }

        // ═════════════════════════════════════════════════════════════════════
        // HELPERS
        // ═════════════════════════════════════════════════════════════════════

        /// <summary>Enable/Disable toàn bộ control panel phải.</summary>
        private void DatCheDoPanelPhai(bool enabled)
        {
            txtHoTen.Enabled = enabled;
            dtpNgaySinh.Enabled = enabled;
            cmbGioiTinh.Enabled = enabled;
            txtSDT.Enabled = enabled;
            txtTienSu.Enabled = enabled;
            btnLuuThayDoi.Enabled = enabled;
            btnXemLichSuKham.Enabled = enabled && !_dangThemMoi;
            btnXoa.Enabled = enabled && !_dangThemMoi;
        }

        /// <summary>Xóa trắng tất cả field panel phải.</summary>
        private void XoaTrangPanelPhai()
        {
            txtHoTen.Text = "";
            txtSDT.Text = "";
            txtTienSu.Text = "";
            dtpNgaySinh.Value = new DateTime(1990, 1, 1);
            cmbGioiTinh.SelectedIndex = 0;
            lblThanhVienTitle.Text = "💎  Thẻ Thành Viên";
            lblThanhVienInfo.Text = "Chưa có thẻ thành viên";
        }

        // ═════════════════════════════════════════════════════════════════════
        // PAINT EVENTS (sinh bởi Designer — giữ trống)
        // ═════════════════════════════════════════════════════════════════════

        private void pnlLeft_Paint(object sender, PaintEventArgs e) { }

        private void pnlDetailButtons_Paint(object sender, PaintEventArgs e) { }
    }
}
