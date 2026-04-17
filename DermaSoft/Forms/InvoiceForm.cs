using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using DermaSoft.Data;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Form Thanh Toán Hóa Đơn — Lễ Tân chọn phiếu khám → xem chi tiết → thu tiền.
    /// Luồng: Chọn PhieuKham → Tải → hiện BNInfo + DichVu + Thuoc + TongKet → Thu tiền → INSERT HoaDon
    /// </summary>
    public partial class InvoiceForm : Form
    {
        // ── State ─────────────────────────────────────────────────────────────
        private int _maPhieuKhamDangChon = -1;
        private decimal _tongDichVu = 0;
        private decimal _tongThuoc = 0;

        // ── Filter ngày ───────────────────────────────────────────────────────
        private Guna2DateTimePicker _dtpNgayLoc;
        private Label _lblSoPhieu;

        public InvoiceForm()
        {
            InitializeComponent();

            this.Load += InvoiceForm_Load;
            btnTai.Click += BtnTai_Click;
            cmbPhieuKham.SelectedIndexChanged += CmbPhieuKham_Changed;
            txtGiamGia.TextChanged += TxtGiamGia_TextChanged;
            btnXacNhan.Click += BtnXacNhan_Click;
            btnInHoaDon.Click += BtnInHoaDon_Click;
        }

        private void InvoiceForm_Load(object sender, EventArgs e)
        {
            cmbPhuongThuc.SelectedIndex = 0;
            LoadThongTinPhongKham();
            ThemPanelLocNgay();
            LoadDanhSachPhieuKham();
            DatTrangThaiForm(enabled: false);
            ResetTongKet();
        }

        /// <summary>
        /// Nhúng bộ lọc ngày vào pnlPhieuKham (cùng panel với ComboBox phiếu khám).
        /// Dùng Guna2DateTimePicker đồng bộ style với cmbPhieuKham + dtpThoiGian.
        /// </summary>
        private void ThemPanelLocNgay()
        {
            // Mở rộng pnlPhieuKham để chứa thêm dòng lọc ngày
            pnlPhieuKham.Height += 44;
            pnlPhieuKham.Padding = new Padding(0, 8, 0, 4);

            // Panel con chứa bộ lọc ngày — nằm dưới ComboBox
            var pnlNgay = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.Transparent,
            };

            // Label "Ngày khám:" — đồng bộ với lblPhieuKham (10.2F Bold, màu 15,92,77)
            var lblNgay = new Label
            {
                Text = "Ngày khám:",
                Font = new Font("Segoe UI", 10.2f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 92, 77),
                AutoSize = true,
                Location = new Point(0, 9),
            };

            // Guna2DateTimePicker — đồng bộ style với cmbPhieuKham (BorderRadius, BorderColor, Font)
            _dtpNgayLoc = new Guna2DateTimePicker
            {
                CustomFormat = "dd/MM/yyyy",
                Format = DateTimePickerFormat.Custom,
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(68, 88, 112),
                FillColor = Color.FromArgb(249, 250, 251),
                BorderColor = Color.SeaGreen,
                BorderRadius = 10,
                Value = DateTime.Today,
                Checked = true,
                Location = new Point(114, 1),
                Size = new Size(180, 36),
            };
            _dtpNgayLoc.ValueChanged += (s, ev) =>
            {
                _maPhieuKhamDangChon = -1;
                LoadDanhSachPhieuKham();
                ResetTongKet();
                DatTrangThaiForm(enabled: false);
            };

            // Nút "Hôm nay" — Guna2Button nhỏ gọn đồng bộ style
            var btnHomNay = new Guna2Button
            {
                Text = "Hôm nay",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 92, 77),
                FillColor = Color.FromArgb(221, 245, 230),
                BorderRadius = 10,
                Size = new Size(80, 30),
                Location = new Point(302, 4),
                Cursor = Cursors.Hand,
            };
            btnHomNay.Click += (s, ev) => _dtpNgayLoc.Value = DateTime.Today;

            // Label số phiếu
            _lblSoPhieu = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(390, 10),
            };

            pnlNgay.Controls.Add(lblNgay);
            pnlNgay.Controls.Add(_dtpNgayLoc);
            pnlNgay.Controls.Add(btnHomNay);
            pnlNgay.Controls.Add(_lblSoPhieu);

            pnlPhieuKham.Controls.Add(pnlNgay);
        }

        // ══════════════════════════════════════════════════════════════════════
        // LOAD THÔNG TIN PHÒNG KHÁM TỪ CÀI ĐẶT
        // ══════════════════════════════════════════════════════════════════════

        private void LoadThongTinPhongKham()
        {
            try
            {
                const string sql = @"
                    SELECT TOP 1 TenPhongKham, DiaChi, SoDienThoai
                    FROM ThongTinPhongKham
                    ORDER BY MaThongTin DESC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    string ten = r["TenPhongKham"]?.ToString();
                    string diaChi = r["DiaChi"]?.ToString();
                    string sdt = r["SoDienThoai"]?.ToString();

                    if (!string.IsNullOrEmpty(ten))
                        lblClinicName.Text = ten;
                    if (!string.IsNullOrEmpty(diaChi))
                        lblClinicAddress.Text = diaChi;
                    if (!string.IsNullOrEmpty(sdt))
                        lblClinicSDT.Text = "SĐT: " + sdt;
                }
            }
            catch { }
        }

        // ══════════════════════════════════════════════════════════════════════
        // LOAD DANH SÁCH PHIẾU KHÁM (chưa thanh toán, TrangThai = 2 Hoàn thành)
        // ══════════════════════════════════════════════════════════════════════

        private void LoadDanhSachPhieuKham()
        {
            try
            {
                // Lấy ngày lọc từ DateTimePicker (mặc định hôm nay)
                DateTime ngayLoc = _dtpNgayLoc != null ? _dtpNgayLoc.Value.Date : DateTime.Today;

                // Chỉ load phiếu khám theo ngày được chọn:
                //   - Chờ thanh toán (TrangThai=2, chưa có HoaDon TT=1)
                //   - Đã thanh toán (TrangThai=3, để xem lại + in hóa đơn)
                const string sql = @"
                    SELECT
                        pk.MaPhieuKham,
                        N'PK#' + RIGHT(N'0000' + CAST(pk.MaPhieuKham AS NVARCHAR(10)), 4)
                            + N' — ' + bn.HoTen
                            + N' — ' + FORMAT(pk.NgayKham, N'HH:mm')
                            + CASE pk.TrangThai
                                WHEN 2 THEN N' (Chờ thanh toán)'
                                WHEN 3 THEN N' (Đã thanh toán ✓)'
                                ELSE N''
                              END                             AS TenHienThi
                    FROM PhieuKham pk
                    JOIN BenhNhan  bn ON pk.MaBenhNhan = bn.MaBenhNhan
                    WHERE pk.IsDeleted = 0
                      AND CAST(pk.NgayKham AS DATE) = @NgayLoc
                      AND (
                            (pk.TrangThai = 2 AND NOT EXISTS (
                                SELECT 1 FROM HoaDon hd
                                WHERE hd.MaPhieuKham = pk.MaPhieuKham
                                  AND hd.TrangThai = 1
                                  AND hd.IsDeleted = 0))
                            OR
                            pk.TrangThai = 3
                           )
                    ORDER BY pk.TrangThai ASC, pk.NgayKham DESC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@NgayLoc", ngayLoc));

                // Cập nhật label số phiếu
                int soChoTT = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string ten = row["TenHienThi"]?.ToString() ?? "";
                    if (ten.Contains("Chờ thanh toán")) soChoTT++;
                }
                if (_lblSoPhieu != null)
                    _lblSoPhieu.Text = $"({soChoTT} chờ TT / {dt.Rows.Count} phiếu)";

                // Thêm dòng trống đầu
                DataRow r = dt.NewRow();
                r["MaPhieuKham"] = -1;
                r["TenHienThi"] = "-- Chọn phiếu khám --";
                dt.Rows.InsertAt(r, 0);

                cmbPhieuKham.DataSource = dt;
                cmbPhieuKham.DisplayMember = "TenHienThi";
                cmbPhieuKham.ValueMember = "MaPhieuKham";
                cmbPhieuKham.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phiếu khám:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // TẢI DỮ LIỆU PHIẾU KHÁM
        // ══════════════════════════════════════════════════════════════════════

        private void CmbPhieuKham_Changed(object sender, EventArgs e)
        {
            if (cmbPhieuKham.SelectedValue == null) return;
            if (!int.TryParse(cmbPhieuKham.SelectedValue.ToString(), out int maPK) || maPK <= 0)
            {
                // Chọn dòng "-- Chọn phiếu khám --" → reset form
                _maPhieuKhamDangChon = -1;
                lblBNTen.Text = "Loading...";
                lblNgayKham.Text = "Loading...";
                lblBacSi.Text = "Loading...";
                lblChanDoan.Text = "Loading...";
                dgvDichVu.DataSource = null;
                dgvThuoc.DataSource = null;
                ResetTongKet();
                DatTrangThaiForm(enabled: false);
                return;
            }

            _maPhieuKhamDangChon = maPK;
            TaiPhieuKham(maPK);
        }

        private void BtnTai_Click(object sender, EventArgs e)
        {
            if (cmbPhieuKham.SelectedValue == null ||
                !int.TryParse(cmbPhieuKham.SelectedValue.ToString(), out int maPK) || maPK <= 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu khám.",
                    "Chưa chọn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _maPhieuKhamDangChon = maPK;
            TaiPhieuKham(maPK);
        }

        private void TaiPhieuKham(int maPhieuKham)
        {
            try
            {
                // 1. Thông tin phiếu khám + bệnh nhân + hạng thành viên
                const string sqlPK = @"
                    SELECT
                        bn.HoTen,
                        FORMAT(pk.NgayKham, N'dd/MM/yyyy')  AS NgayKham,
                        nd.HoTen                            AS TenBacSi,
                        ISNULL(pk.ChanDoan, N'—')           AS ChanDoan,
                        pk.TrangThai,
                        ISNULL(htv.TenHang, N'')            AS TenHang,
                        ISNULL(htv.PhanTramGiamDuocPham, 0) AS PhanTramGiamDuocPham,
                        ISNULL(htv.PhanTramGiamTongHD, 0)   AS PhanTramGiamTongHD,
                        ISNULL(htv.GiamGiaCodinh, 0)        AS GiamGiaCodinh,
                        ISNULL(htv.GhiChuKhuyenMai, N'')    AS GhiChuKhuyenMai
                    FROM PhieuKham pk
                    JOIN BenhNhan  bn ON pk.MaBenhNhan  = bn.MaBenhNhan
                    JOIN NguoiDung nd ON pk.MaNguoiDung = nd.MaNguoiDung
                    LEFT JOIN ThanhVienInfo tvi ON bn.MaBenhNhan = tvi.MaBenhNhan
                    LEFT JOIN HangThanhVien htv ON tvi.MaHang    = htv.MaHang
                    WHERE pk.MaPhieuKham = @MaPK";

                DataTable dtPK = DatabaseConnection.ExecuteQuery(sqlPK,
                    p => p.AddWithValue("@MaPK", maPhieuKham));

                if (dtPK == null || dtPK.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy phiếu khám.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataRow pk = dtPK.Rows[0];
                lblBNTen.Text = pk["HoTen"].ToString();
                lblNgayKham.Text = pk["NgayKham"]?.ToString() ?? "—";
                lblBacSi.Text = pk["TenBacSi"]?.ToString() ?? "—";
                lblChanDoan.Text = pk["ChanDoan"].ToString();

                // 2. Chi tiết dịch vụ
                const string sqlDV = @"
                    SELECT
                        dv.TenDichVu,
                        ctdv.SoLuong,
                        FORMAT(dv.DonGia, N'#,##0') + N'đ'    AS DonGia,
                        FORMAT(ctdv.ThanhTien, N'#,##0') + N'đ' AS ThanhTien,
                        ctdv.ThanhTien                          AS ThanhTienRaw
                    FROM ChiTietDichVu ctdv
                    JOIN DichVu dv ON ctdv.MaDichVu = dv.MaDichVu
                    WHERE ctdv.MaPhieuKham = @MaPK";

                DataTable dtDV = DatabaseConnection.ExecuteQuery(sqlDV,
                    p => p.AddWithValue("@MaPK", maPhieuKham));

                dgvDichVu.AutoGenerateColumns = false;
                dgvDichVu.DataSource = dtDV;

                _tongDichVu = 0;
                foreach (DataRow row in dtDV.Rows)
                    _tongDichVu += Convert.ToDecimal(row["ThanhTienRaw"]);

                // 3. Chi tiết thuốc
                const string sqlThuoc = @"
                    SELECT
                        t.TenThuoc,
                        cdt.SoLuong,
                        FORMAT(t.DonGia, N'#,##0') + N'đ'                          AS DonGia,
                        FORMAT(cdt.SoLuong * t.DonGia, N'#,##0') + N'đ'            AS ThanhTien,
                        cdt.SoLuong * t.DonGia                                      AS ThanhTienRaw
                    FROM ChiTietDonThuoc cdt
                    JOIN Thuoc t ON cdt.MaThuoc = t.MaThuoc
                    WHERE cdt.MaPhieuKham = @MaPK";

                DataTable dtThuoc = DatabaseConnection.ExecuteQuery(sqlThuoc,
                    p => p.AddWithValue("@MaPK", maPhieuKham));

                dgvThuoc.AutoGenerateColumns = false;
                dgvThuoc.DataSource = dtThuoc;

                _tongThuoc = 0;
                foreach (DataRow row in dtThuoc.Rows)
                    _tongThuoc += Convert.ToDecimal(row["ThanhTienRaw"]);

                // 4. Cập nhật Tổng kết
                int trangThaiPK = Convert.ToInt32(pk["TrangThai"]);

                // Đồng bộ: load GiamGia + PhuongThucThanhToan từ HoaDon nếu đã tồn tại
                // (HoaDon có thể đã được Admin sửa trong QuanLyHoaDonForm)
                if (trangThaiPK == 3)
                {
                    // Phiếu đã thanh toán → load từ HoaDon đã TT (TrangThai=1)
                    try
                    {
                        var dtHD = DatabaseConnection.ExecuteQuery(
                            "SELECT GiamGia, PhuongThucThanhToan FROM HoaDon WHERE MaPhieuKham = @MaPK AND TrangThai = 1 AND IsDeleted = 0",
                            p2 => p2.AddWithValue("@MaPK", maPhieuKham));
                        if (dtHD != null && dtHD.Rows.Count > 0)
                        {
                            decimal giamGiaCu = Convert.ToDecimal(dtHD.Rows[0]["GiamGia"]);
                            txtGiamGia.Text = giamGiaCu > 0 ? giamGiaCu.ToString("N0") : "0";

                            string ptCu = dtHD.Rows[0]["PhuongThucThanhToan"]?.ToString() ?? "";
                            int idx = cmbPhuongThuc.FindStringExact(ptCu);
                            if (idx >= 0) cmbPhuongThuc.SelectedIndex = idx;
                        }
                        else
                            txtGiamGia.Text = "0";
                    }
                    catch { txtGiamGia.Text = "0"; }
                }
                else
                {
                    // Phiếu chưa thanh toán → kiểm tra HoaDon chờ TT (TrangThai=0)
                    // đã tồn tại (có thể đã được Admin sửa GiamGia/PhuongThuc)
                    bool daLayTuHoaDon = false;
                    try
                    {
                        var dtHD = DatabaseConnection.ExecuteQuery(
                            "SELECT GiamGia, PhuongThucThanhToan FROM HoaDon WHERE MaPhieuKham = @MaPK AND TrangThai = 0 AND IsDeleted = 0",
                            p2 => p2.AddWithValue("@MaPK", maPhieuKham));
                        if (dtHD != null && dtHD.Rows.Count > 0)
                        {
                            decimal giamGiaCu = Convert.ToDecimal(dtHD.Rows[0]["GiamGia"]);
                            if (giamGiaCu > 0)
                            {
                                txtGiamGia.Text = giamGiaCu.ToString("N0");
                                daLayTuHoaDon = true;
                            }

                            string ptCu = dtHD.Rows[0]["PhuongThucThanhToan"]?.ToString() ?? "";
                            if (!string.IsNullOrEmpty(ptCu))
                            {
                                int idx = cmbPhuongThuc.FindStringExact(ptCu);
                                if (idx >= 0) cmbPhuongThuc.SelectedIndex = idx;
                            }
                        }
                    }
                    catch { /* ignore — fallback to auto */ }

                    if (!daLayTuHoaDon)
                    {
                        // Không có HoaDon hoặc GiamGia=0 → auto giảm giá theo hạng thành viên
                        decimal giamTuDong = TinhGiamGiaThanhVien(pk);
                        txtGiamGia.Text = giamTuDong > 0 ? giamTuDong.ToString("N0") : "0";
                    }
                }

                TinhToanTongKet();
                CapNhatPreview();

                // Phiếu đã thanh toán: disable nhập liệu + nút TT, nhưng giữ nút In
                if (trangThaiPK == 3)
                {
                    txtGiamGia.Enabled = false;
                    txtTienKhach.Enabled = false;
                    cmbPhuongThuc.Enabled = false;
                    btnXacNhan.Enabled = false;
                    btnInHoaDon.Enabled = true;
                }
                else
                {
                    DatTrangThaiForm(enabled: true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải phiếu khám:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // TÍNH TOÁN TỔNG KẾT
        // ══════════════════════════════════════════════════════════════════════

        private void TinhToanTongKet()
        {
            lblTongDVValue.Text = FormatTien(_tongDichVu);
            lblTongThuocValue.Text = FormatTien(_tongThuoc);

            decimal tamTinh = _tongDichVu + _tongThuoc;
            lblTamTinhValue.Text = FormatTien(tamTinh);

            decimal giamGia = 0;
            string rawGiam = txtGiamGia.Text.Trim();
            if (rawGiam.EndsWith("%"))
            {
                // Giảm giá theo phần trăm
                decimal phanTram;
                if (decimal.TryParse(rawGiam.TrimEnd('%').Replace(",", "").Replace(".", ""), out phanTram))
                    giamGia = tamTinh * Math.Min(phanTram, 100) / 100m;
            }
            else
            {
                decimal.TryParse(rawGiam.Replace(",", "").Replace(".", ""), out giamGia);
            }

            decimal tongCuoi = Math.Max(0, tamTinh - giamGia);
            lblTongTien.Text = FormatTien(tongCuoi);

            // Tính tiền thừa
            TinhTienThua();
        }

        private void TinhTienThua()
        {
            decimal tong = 0;
            decimal.TryParse(lblTongTien.Text.Replace(",", "").Replace(".", "").Replace("đ", ""), out tong);

            decimal tienKhach = 0;
            decimal.TryParse(txtTienKhach.Text.Replace(",", "").Replace(".", ""), out tienKhach);

            decimal thua = tienKhach - tong;
            lblTienThua.Text = thua >= 0 ? FormatTien(thua) : "— " + FormatTien(Math.Abs(thua));
            lblTienThua.ForeColor = thua < 0
                ? Color.FromArgb(220, 38, 38)
                : Color.FromArgb(15, 92, 77);
        }

        private void TxtGiamGia_TextChanged(object sender, EventArgs e)
        {
            if (_maPhieuKhamDangChon <= 0) return;
            TinhToanTongKet();
            CapNhatPreview();
        }

        private void txtTienKhach_TextChanged(object sender, EventArgs e)
        {
            TinhTienThua();
        }

        // ══════════════════════════════════════════════════════════════════════
        // CẬP NHẬT MINI PREVIEW
        // ══════════════════════════════════════════════════════════════════════

        private void CapNhatPreview()
        {
            lblPreviewBN.Text = "BN: " + lblBNTen.Text;
            lblPreviewNgay.Text = lblNgayKham.Text;
            lblPreviewDVValue.Text = FormatTien(_tongDichVu);
            lblPreviewThuocValue.Text = FormatTien(_tongThuoc);
            label5.Text = lblTongTien.Text;
        }

        // ══════════════════════════════════════════════════════════════════════
        // XÁC NHẬN THANH TOÁN
        // ══════════════════════════════════════════════════════════════════════

        private void BtnXacNhan_Click(object sender, EventArgs e)
        {
            if (_maPhieuKhamDangChon <= 0)
            {
                MessageBox.Show("Vui lòng chọn và tải phiếu khám trước.",
                    "Chưa chọn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate tiền khách
            if (!decimal.TryParse(txtTienKhach.Text.Replace(",", "").Replace(".", ""), out decimal tienKhach) || tienKhach <= 0)
            {
                MessageBox.Show("Vui lòng nhập số tiền khách trả hợp lệ.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTienKhach.Focus();
                return;
            }

            decimal.TryParse(lblTongTien.Text.Replace(",", "").Replace(".", "").Replace("đ", ""), out decimal tongTien);

            if (tienKhach < tongTien)
            {
                MessageBox.Show($"Tiền khách trả ({FormatTien(tienKhach)}) chưa đủ!\nCần ít nhất {FormatTien(tongTien)}.",
                    "Không đủ tiền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal giamGia = TinhGiamGia();
            decimal tienThua = tienKhach - tongTien;
            string phuongThuc = cmbPhuongThuc.Text;

            var confirm = MessageBox.Show(
                $"Xác nhận thanh toán cho bệnh nhân \"{lblBNTen.Text}\"?\n\n" +
                $"Tổng tiền : {FormatTien(tongTien)}\n" +
                $"Khách trả : {FormatTien(tienKhach)}\n" +
                $"Tiền thừa : {FormatTien(tienThua)}\n" +
                $"Phương thức: {phuongThuc}",
                "Xác nhận thanh toán",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            ThucHienThanhToan(tongTien, _tongDichVu, _tongThuoc, giamGia, tienKhach, tienThua, phuongThuc);
        }

        private void ThucHienThanhToan(
            decimal tongTien, decimal tongDV, decimal tongThuoc,
            decimal giamGia, decimal tienKhach, decimal tienThua, string phuongThuc)
        {
            bool ok = DatabaseConnection.ExecuteTransaction((conn, tran) =>
            {
                int maHoaDon;

                // 1. Kiểm tra HoaDon đã tồn tại (tạo sẵn khi BS hoàn thành khám)
                var cmdCheck = new SqlCommand(
                    "SELECT MaHoaDon FROM HoaDon WHERE MaPhieuKham = @MaPK AND IsDeleted = 0",
                    conn, tran);
                cmdCheck.Parameters.AddWithValue("@MaPK", _maPhieuKhamDangChon);
                object existing = cmdCheck.ExecuteScalar();

                if (existing != null && existing != DBNull.Value)
                {
                    // HoaDon đã có → UPDATE thông tin thanh toán
                    maHoaDon = Convert.ToInt32(existing);
                    var cmdUpd = new SqlCommand(@"
                        UPDATE HoaDon SET
                            TongTien    = @TongTien,
                            TongTienDichVu = @TongDV,
                            TongThuoc   = @TongThuoc,
                            GiamGia     = @GiamGia,
                            TienKhachTra = @TienKhach,
                            TienThua    = @TienThua,
                            PhuongThucThanhToan = @PhuongThuc,
                            NgayThanhToan = GETDATE()
                        WHERE MaHoaDon = @MaHD", conn, tran);
                    cmdUpd.Parameters.AddWithValue("@TongTien", tongTien);
                    cmdUpd.Parameters.AddWithValue("@TongDV", tongDV);
                    cmdUpd.Parameters.AddWithValue("@TongThuoc", tongThuoc);
                    cmdUpd.Parameters.AddWithValue("@GiamGia", giamGia);
                    cmdUpd.Parameters.AddWithValue("@TienKhach", tienKhach);
                    cmdUpd.Parameters.AddWithValue("@TienThua", tienThua);
                    cmdUpd.Parameters.AddWithValue("@PhuongThuc", phuongThuc);
                    cmdUpd.Parameters.AddWithValue("@MaHD", maHoaDon);
                    cmdUpd.ExecuteNonQuery();
                }
                else
                {
                    // HoaDon chưa có → INSERT mới
                    var cmdHD = new SqlCommand(@"
                        INSERT INTO HoaDon
                            (MaPhieuKham, TongTien, TongTienDichVu, TongThuoc,
                             GiamGia, TienKhachTra, TienThua,
                             PhuongThucThanhToan, NgayThanhToan, TrangThai)
                        VALUES
                            (@MaPK, @TongTien, @TongDV, @TongThuoc,
                             @GiamGia, @TienKhach, @TienThua,
                             @PhuongThuc, GETDATE(), 0);
                        SELECT SCOPE_IDENTITY();", conn, tran);
                    cmdHD.Parameters.AddWithValue("@MaPK", _maPhieuKhamDangChon);
                    cmdHD.Parameters.AddWithValue("@TongTien", tongTien);
                    cmdHD.Parameters.AddWithValue("@TongDV", tongDV);
                    cmdHD.Parameters.AddWithValue("@TongThuoc", tongThuoc);
                    cmdHD.Parameters.AddWithValue("@GiamGia", giamGia);
                    cmdHD.Parameters.AddWithValue("@TienKhach", tienKhach);
                    cmdHD.Parameters.AddWithValue("@TienThua", tienThua);
                    cmdHD.Parameters.AddWithValue("@PhuongThuc", phuongThuc);
                    maHoaDon = Convert.ToInt32(cmdHD.ExecuteScalar());
                }

                // UPDATE TrangThai = 1 → Trigger TRG_HoaDon_CapPhatDiem sẽ tự chạy
                // và cộng điểm thưởng cho thành viên
                // Trước tiên: tự động mở thẻ thành viên nếu BN chưa có
                var cmdAutoMember = new SqlCommand(@"
                    IF NOT EXISTS (
                        SELECT 1 FROM ThanhVienInfo tvi
                        JOIN PhieuKham pk ON tvi.MaBenhNhan = pk.MaBenhNhan
                        WHERE pk.MaPhieuKham = @MaPK
                    )
                    BEGIN
                        INSERT INTO ThanhVienInfo (MaBenhNhan, MaHang, DiemTichLuy, SoLanKham)
                        SELECT pk.MaBenhNhan, 1, 0, 0
                        FROM PhieuKham pk
                        WHERE pk.MaPhieuKham = @MaPK
                    END", conn, tran);
                cmdAutoMember.Parameters.AddWithValue("@MaPK", _maPhieuKhamDangChon);
                cmdAutoMember.ExecuteNonQuery();

                var cmdUpHD = new SqlCommand(
                    "UPDATE HoaDon SET TrangThai = 1 WHERE MaHoaDon = @MaHD",
                    conn, tran);
                cmdUpHD.Parameters.AddWithValue("@MaHD", maHoaDon);
                cmdUpHD.ExecuteNonQuery();

                // 2. Cập nhật PhieuKham TrangThai = 3 (Đã thanh toán)
                var cmdPK = new SqlCommand(
                    "UPDATE PhieuKham SET TrangThai = 3 WHERE MaPhieuKham = @MaPK",
                    conn, tran);
                cmdPK.Parameters.AddWithValue("@MaPK", _maPhieuKhamDangChon);
                cmdPK.ExecuteNonQuery();

                // 3. Nâng hạng thành viên nếu đủ điểm (thay thế cho trigger)
                var cmdNangHang = new SqlCommand(@"
                    UPDATE tvi
                    SET MaHang = (
                        SELECT TOP 1 htv.MaHang
                        FROM HangThanhVien htv
                        WHERE htv.DiemToiThieu <= tvi.DiemTichLuy
                        ORDER BY htv.DiemToiThieu DESC
                    ),
                    SoLanKham = tvi.SoLanKham + 1
                    FROM ThanhVienInfo tvi
                    JOIN PhieuKham pk ON tvi.MaBenhNhan = pk.MaBenhNhan
                    WHERE pk.MaPhieuKham = @MaPK2", conn, tran);
                cmdNangHang.Parameters.AddWithValue("@MaPK2", _maPhieuKhamDangChon);
                cmdNangHang.ExecuteNonQuery();
            });

            if (ok)
            {
                // Giữ lại thông tin phiếu để in hóa đơn
                // Disable nhập liệu, chỉ giữ nút In hoạt động
                btnXacNhan.Enabled = false;
                txtGiamGia.Enabled = false;
                txtTienKhach.Enabled = false;
                cmbPhuongThuc.Enabled = false;
                btnInHoaDon.Enabled = true;

                var ketQua = MessageBox.Show(
                    $"✅ Thanh toán thành công!\n\nBệnh nhân: {lblBNTen.Text}\nTiền thừa trả lại: {FormatTien(tienThua)}\n\nBạn có muốn in hóa đơn ngay không?",
                    "Thanh toán thành công",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (ketQua == DialogResult.Yes)
                {
                    BtnInHoaDon_Click(null, EventArgs.Empty);
                }

                // Reload dropdown (phiếu vừa TT sẽ hiện "Đã thanh toán ✓")
                LoadDanhSachPhieuKham();
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // In Hóa Đơn
        // ══════════════════════════════════════════════════════════════════════
        private void BtnInHoaDon_Click(object sender, EventArgs e)
        {
            if (_maPhieuKhamDangChon <= 0)
            {
                MessageBox.Show("Vui lòng tải phiếu khám trước.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var printer = new HoaDonPrinter(
                    maPK: _maPhieuKhamDangChon,
                    tenBN: lblBNTen.Text,
                    ngayKham: lblNgayKham.Text,
                    tenBacSi: lblBacSi.Text,
                    chanDoan: lblChanDoan.Text,
                    tongDV: _tongDichVu,
                    tongThuoc: _tongThuoc,
                    giamGia: LayGiamGia(),
                    tongTien: LayTongTien(),
                    tienKhach: LayTienKhach(),
                    tienThua: LayTienThua(),
                    phuongThuc: cmbPhuongThuc.Text
                );

                printer.MoXemTruoc(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi in hóa đơn:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Helpers đọc giá trị từ form ─────────────────────────────────────

        /// <summary>
        /// Tính giảm giá tự động theo chính sách hạng thành viên.
        /// Ưu tiên: GiamGiaCodinh > PhanTramGiamTongHD > PhanTramGiamDuocPham.
        /// </summary>
        private decimal TinhGiamGiaThanhVien(DataRow pkRow)
        {
            string tenHang = pkRow["TenHang"]?.ToString() ?? "";
            if (string.IsNullOrEmpty(tenHang)) return 0;

            decimal giamCodinh = Convert.ToDecimal(pkRow["GiamGiaCodinh"]);
            decimal ptGiamTongHD = Convert.ToDecimal(pkRow["PhanTramGiamTongHD"]);
            decimal ptGiamDuocPham = Convert.ToDecimal(pkRow["PhanTramGiamDuocPham"]);

            decimal tamTinh = _tongDichVu + _tongThuoc;

            // 1. Giảm giá cố định (VD: Thành Viên Đỏ = 100.000đ)
            if (giamCodinh > 0)
                return Math.Min(giamCodinh, tamTinh);

            // 2. Giảm % tổng hóa đơn (VD: Kim Cương = 10% tổng)
            if (ptGiamTongHD > 0)
                return tamTinh * ptGiamTongHD / 100m;

            // 3. Giảm % dược phẩm (VD: Bạc=5%, Vàng=10% thuốc)
            if (ptGiamDuocPham > 0)
                return _tongThuoc * ptGiamDuocPham / 100m;

            return 0;
        }

        private decimal LayGiamGia()
        {
            return TinhGiamGia();
        }

        /// <summary>
        /// Tính giá trị giảm giá thực tế — hỗ trợ cả số tuyệt đối và phần trăm (VD: "10%").
        /// </summary>
        private decimal TinhGiamGia()
        {
            decimal tamTinh = _tongDichVu + _tongThuoc;
            string raw = txtGiamGia.Text.Trim();
            if (raw.EndsWith("%"))
            {
                if (decimal.TryParse(raw.TrimEnd('%').Replace(",", "").Replace(".", ""), out decimal pt))
                    return tamTinh * Math.Min(pt, 100) / 100m;
                return 0;
            }
            decimal.TryParse(raw.Replace(",", "").Replace(".", ""), out decimal v);
            return v;
        }

        private decimal LayTongTien()
        {
            decimal.TryParse(
                lblTongTien.Text.Replace(",", "").Replace(".", "").Replace("đ", ""),
                out decimal v);
            return v;
        }

        private decimal LayTienKhach()
        {
            decimal.TryParse(txtTienKhach.Text.Replace(",", "").Replace(".", ""), out decimal v);
            return v;
        }

        private decimal LayTienThua()
        {
            decimal tong = LayTongTien();
            decimal khach = LayTienKhach();
            return Math.Max(0, khach - tong);
        }

        // ══════════════════════════════════════════════════════════════════════
        // HELPERS
        // ══════════════════════════════════════════════════════════════════════

        private string FormatTien(decimal so) => so.ToString("#,##0") + "đ";

        private void ResetTongKet()
        {
            lblTongDVValue.Text = "0đ";
            lblTongThuocValue.Text = "0đ";
            lblTamTinhValue.Text = "0đ";
            lblTongTien.Text = "0đ";
            lblTienThua.Text = "0đ";
            lblTienThua.ForeColor = Color.FromArgb(15, 92, 77);
            lblPreviewBN.Text = "BN: —";
            lblPreviewNgay.Text = "—";
            lblPreviewDVValue.Text = "0đ";
            lblPreviewThuocValue.Text = "0đ";
            label5.Text = "0đ";
        }

        private void DatTrangThaiForm(bool enabled)
        {
            txtGiamGia.Enabled = enabled;
            txtTienKhach.Enabled = enabled;
            cmbPhuongThuc.Enabled = enabled;
            btnXacNhan.Enabled = enabled;
            btnInHoaDon.Enabled = enabled;
        }

        private void ResetForm()
        {
            _maPhieuKhamDangChon = -1;
            _tongDichVu = 0;
            _tongThuoc = 0;

            lblBNTen.Text = "Loading...";
            lblNgayKham.Text = "Loading...";
            lblBacSi.Text = "Loading...";
            lblChanDoan.Text = "Loading...";

            dgvDichVu.DataSource = null;
            dgvThuoc.DataSource = null;

            txtGiamGia.Text = "";
            txtTienKhach.Text = "";

            ResetTongKet();
            DatTrangThaiForm(enabled: false);

            // Reload dropdown
            LoadDanhSachPhieuKham();
        }

        // ══════════════════════════════════════════════════════════════════════
        // STUB EVENTS TỪ DESIGNER
        // ══════════════════════════════════════════════════════════════════════
        private void pnlLeft_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void pnlPhieuKham_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void tlpBNInfo_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void pnlChanDoan_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void panel2_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void pnlTongDV_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void pnlTongKet_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void pnlWrapperTongKet_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void lblGiamGiaLabel_Click(object sender, EventArgs e) { }
        private void lblClinicSDT_Click(object sender, EventArgs e) { }
    }
}
