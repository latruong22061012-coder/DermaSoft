using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
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
            LoadDanhSachPhieuKham();
            DatTrangThaiForm(enabled: false);
            ResetTongKet();
        }

        // ══════════════════════════════════════════════════════════════════════
        // LOAD DANH SÁCH PHIẾU KHÁM (chưa thanh toán, TrangThai = 2 Hoàn thành)
        // ══════════════════════════════════════════════════════════════════════

        private void LoadDanhSachPhieuKham()
        {
            try
            {
                const string sql = @"
                    SELECT
                        pk.MaPhieuKham,
                        N'PK#' + RIGHT(N'0000' + CAST(pk.MaPhieuKham AS NVARCHAR(10)), 4)
                            + N' — ' + bn.HoTen
                            + N' (Hoàn thành)'               AS TenHienThi
                    FROM PhieuKham pk
                    JOIN BenhNhan  bn ON pk.MaBenhNhan = bn.MaBenhNhan
                    WHERE pk.IsDeleted = 0
                      AND pk.TrangThai = 2
                      AND NOT EXISTS (
                            SELECT 1 FROM HoaDon hd
                            WHERE hd.MaPhieuKham = pk.MaPhieuKham
                              AND hd.TrangThai = 1
                              AND hd.IsDeleted  = 0)
                    ORDER BY pk.NgayKham DESC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql);

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

        private void CmbPhieuKham_Changed(object sender, EventArgs e) { }

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
                // 1. Thông tin phiếu khám + bệnh nhân
                const string sqlPK = @"
                    SELECT
                        bn.HoTen,
                        FORMAT(pk.NgayKham, N'dd/MM/yyyy')  AS NgayKham,
                        nd.HoTen                            AS TenBacSi,
                        ISNULL(pk.ChanDoan, N'—')           AS ChanDoan,
                        pk.TrangThai
                    FROM PhieuKham pk
                    JOIN BenhNhan  bn ON pk.MaBenhNhan  = bn.MaBenhNhan
                    JOIN NguoiDung nd ON pk.MaNguoiDung = nd.MaNguoiDung
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
                txtGiamGia.Text = "0";
                TinhToanTongKet();
                CapNhatPreview();
                DatTrangThaiForm(enabled: true);
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
            decimal.TryParse(txtGiamGia.Text.Replace(",", "").Replace(".", ""), out giamGia);

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

            decimal.TryParse(txtGiamGia.Text.Replace(",", "").Replace(".", ""), out decimal giamGia);
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
                // 1. INSERT HoaDon
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

                // Lấy MaHoaDon vừa tạo
                int maHoaDon = Convert.ToInt32(cmdHD.ExecuteScalar());

                // UPDATE TrangThai = 1 → Trigger TRG_HoaDon_CapPhatDiem sẽ tự chạy
                // và cộng điểm thưởng cho thành viên
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
                MessageBox.Show(
                    $"✅ Thanh toán thành công!\n\nBệnh nhân: {lblBNTen.Text}\nTiền thừa trả lại: {FormatTien(tienThua)}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetForm();
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

        private decimal LayGiamGia()
        {
            decimal.TryParse(txtGiamGia.Text.Replace(",", "").Replace(".", ""), out decimal v);
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