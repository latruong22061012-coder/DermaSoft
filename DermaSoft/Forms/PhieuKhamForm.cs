using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Form Phiếu Khám Bệnh — Bác sĩ nhập triệu chứng, chẩn đoán,
    /// chọn dịch vụ đã thực hiện và kê đơn thuốc (FEFO).
    /// Mở từ DashboardBacSiForm (nút Bắt Đầu Khám) với MaPhieuKham cụ thể.
    /// </summary>
    public partial class PhieuKhamForm : Form
    {
        // ── State ─────────────────────────────────────────────────────────────
        private int _maPhieuKham = -1;
        private int _maBacSi = -1;
        private int _maLichHen = -1;  // Dùng để UPDATE LichHen.TrangThai=2 khi hoàn thành khám

        /// <summary>True nếu đang khám (có MaPhieuKham hợp lệ, chưa hoàn thành).</summary>
        internal bool DangKham => _maPhieuKham > 0 && !_daHoanThanh;
        private bool _daHoanThanh = false;

        // Danh sách checkbox dịch vụ (tạo động trong pnlDichVuList)
        private readonly List<CheckBox> _dsDichVuChk = new List<CheckBox>();

        // Tab đang active (dùng để vẽ underline)
        private Guna2Button _tabDangActive;

        // Màu theme dùng chung
        private static readonly Color ClrPrimary = ColorTranslator.FromHtml("#0F5C4D");
        private static readonly Color ClrGray = Color.FromArgb(107, 114, 128);
        private static readonly Color ClrBorder = Color.FromArgb(229, 231, 235);
        private static readonly Color ClrRowAlt = ColorTranslator.FromHtml("#F5FBF7");

        private Panel _pnlTabContent;  // Nội dung tab đang active (trừ Chẩn Đoán)

        // ═════════════════════════════════════════════════════════════════════
        // CONSTRUCTOR
        // ═════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Mở phiếu khám cụ thể (từ DashboardBacSiForm khi bấm "Bắt Đầu Khám").
        /// </summary>
        public PhieuKhamForm(int maPhieuKham)
        {
            InitializeComponent();
            _maPhieuKham = maPhieuKham;

            var nd = LoginForm.NguoiDungHienTai;
            if (nd != null) _maBacSi = nd.MaNguoiDung;

            DangKyEvents();
        }

        /// <summary>
        /// Constructor mặc định — dùng khi mở từ menu sidebar (hiển thị danh sách phiếu của BS).
        /// </summary>
        public PhieuKhamForm()
        {
            InitializeComponent();

            var nd = LoginForm.NguoiDungHienTai;
            if (nd != null) _maBacSi = nd.MaNguoiDung;

            DangKyEvents();
        }

        private void DangKyEvents()
        {
            this.Load += PhieuKhamForm_Load;

            btnHoanThanhKham.Click += BtnHoanThanhKham_Click;
            btnLuuNhap.Click += BtnLuuNhap_Click;
            btnThemThuoc.Click += BtnThemThuoc_Click;

            dgvDonThuoc.CellClick += DgvDonThuoc_CellClick;
            dgvDonThuoc.CellEndEdit += DgvDonThuoc_CellEndEdit;
            dgvDonThuoc.DefaultValuesNeeded += DgvDonThuoc_DefaultValuesNeeded;

            // Tab events
            btnTabChanDoan.Click += (s, e) => ChuyenTab(btnTabChanDoan);
            btnTabDichVu.Click += (s, e) => ChuyenTab(btnTabDichVu);
            btnTabKeDonThuoc.Click += (s, e) => ChuyenTab(btnTabKeDonThuoc);
            btnTabHinhAnh.Click += (s, e) => ChuyenTab(btnTabHinhAnh);
            btnTabGhiChu.Click += (s, e) => ChuyenTab(btnTabGhiChu);

            // Vẽ underline tab active
            pnlTabs.Paint += PnlTabs_Paint;
        }

        // ═════════════════════════════════════════════════════════════════════
        // LOAD
        // ═════════════════════════════════════════════════════════════════════

        private void PhieuKhamForm_Load(object sender, EventArgs e)
        {
            // cboTrangThai items
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[]
            {
                "Mới",              // 0 — PhieuKham vừa được tạo bởi Lễ Tân
                "Đang khám",        // 1 — Bác sĩ đã bắt đầu khám (btnLuuNhap)
                "Hoàn thành",       // 2 — Bác sĩ hoàn thành (btnHoanThanhKham)
                "Đã thanh toán",    // 3 — Lễ Tân đã thu tiền (InvoiceForm)
                "Hủy"               // 4 — Đã hủy
            });
            // BS không được tự đổi TrangThai thủ công — chỉ thay đổi qua các nút chức năng
            cboTrangThai.Enabled = false;

            CoDinhHeaderDGV();
            BoTronAvatar();

            if (_maPhieuKham > 0)
            {
                LoadThongTinPhieuKham();
                LoadDanhSachDichVu();
                LoadThuocFEFO();
                LoadDonThuocHienTai();
            }
            else
            {
                // Mở từ menu → hiển thị selector chọn phiếu khám
                HienThiSelectorPhieuKham();
            }

            // Tab mặc định = Chẩn Đoán
            _tabDangActive = btnTabChanDoan;
            pnlTabs.Invalidate();
        }

        // ═════════════════════════════════════════════════════════════════════
        // SELECTOR (mở từ menu không có MaPhieuKham)
        // ═════════════════════════════════════════════════════════════════════

        private void HienThiSelectorPhieuKham()
        {
            try
            {
                const string sql = @"
                    SELECT
                        pk.MaPhieuKham,
                        N'PK#' + RIGHT(N'000' + CAST(pk.MaPhieuKham AS NVARCHAR), 3)
                            + N' — ' + bn.HoTen
                            + N' (' + FORMAT(pk.NgayKham, 'HH:mm dd/MM') + N')'  AS TenHienThi
                    FROM PhieuKham pk
                    JOIN BenhNhan  bn ON pk.MaBenhNhan = bn.MaBenhNhan
                    LEFT JOIN LichHen lh ON pk.MaLichHen = lh.MaLichHen
                    WHERE pk.MaNguoiDung = @MaBS
                      AND CAST(pk.NgayKham AS DATE) = CAST(GETDATE() AS DATE)
                      AND pk.TrangThai   IN (0, 1)
                      AND pk.IsDeleted   = 0
                      AND (pk.MaLichHen IS NULL OR lh.TrangThai <> 3)
                    ORDER BY pk.NgayKham DESC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBS", _maBacSi));

                // Xóa thông tin BN mặc định — chưa chọn phiếu thì không hiện BN nào
                lblAvatar.Text = "?";
                lblTenBN.Text = "Chọn phiếu khám bên dưới";
                lblThongTinBN.Text = "—";
                lblDiUng.Text = "";
                txtTrieuChung.Text = "";
                txtChanDoan.Text = "";

                if (dt == null || dt.Rows.Count == 0)
                {
                    lblTenBN.Text = "Không có phiếu khám đang chờ";
                    lblThongTinBN.Text = "Lễ tân cần tiếp nhận bệnh nhân trước";
                    lblDiUng.Text = "";
                    btnHoanThanhKham.Enabled = false;
                    btnLuuNhap.Enabled = false;
                    return;
                }

                // Thêm dòng placeholder đầu tiên
                DataRow rPlaceholder = dt.NewRow();
                rPlaceholder["MaPhieuKham"] = -1;
                rPlaceholder["TenHienThi"] = "-- Chọn phiếu khám --";
                dt.Rows.InsertAt(rPlaceholder, 0);

                // Tạo ComboBox chọn phiếu khám overlay trên pnlContent
                var cboChon = new Guna2ComboBox
                {
                    Location = new Point(20, 20),
                    Width = 500,
                    Height = 40,
                    Font = new Font("Segoe UI", 10f),
                    BorderRadius = 8,
                    BorderColor = ClrPrimary,
                    FillColor = Color.White,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                };
                cboChon.DataSource = dt;
                cboChon.DisplayMember = "TenHienThi";
                cboChon.ValueMember = "MaPhieuKham";

                var btnChon = new Guna2Button
                {
                    Location = new Point(530, 20),
                    Width = 100,
                    Height = 40,
                    Text = "Mở",
                    FillColor = ClrPrimary,
                    ForeColor = Color.White,
                    BorderRadius = 8,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                };
                btnChon.Click += (s, e2) =>
                {
                    if (cboChon.SelectedValue == null) return;
                    int maPK = Convert.ToInt32(cboChon.SelectedValue);
                    if (maPK <= 0) return; // Placeholder "-- Chọn phiếu khám --"
                    _maPhieuKham = maPK;

                    // Xóa selector
                    pnlContent.Controls.Remove(cboChon);
                    pnlContent.Controls.Remove(btnChon);

                    // Load dữ liệu
                    LoadThongTinPhieuKham();
                    LoadDanhSachDichVu();
                    LoadThuocFEFO();
                    LoadDonThuocHienTai();

                    btnHoanThanhKham.Enabled = true;
                    btnLuuNhap.Enabled = true;
                };

                pnlContent.Controls.Add(cboChon);
                pnlContent.Controls.Add(btnChon);
                cboChon.BringToFront();
                btnChon.BringToFront();

                btnHoanThanhKham.Enabled = false;
                btnLuuNhap.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phiếu khám:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // LOAD THÔNG TIN PHIẾU KHÁM + BỆNH NHÂN
        // ═════════════════════════════════════════════════════════════════════

        private void LoadThongTinPhieuKham()
        {
            try
            {
                const string sql = @"
                    SELECT
                        pk.MaPhieuKham,
                        pk.MaLichHen,
                        pk.TrieuChung,
                        pk.ChanDoan,
                        pk.NgayTaiKham,
                        pk.TrangThai,
                        pk.GhiChu,
                        bn.MaBenhNhan,
                        bn.HoTen,
                        bn.NgaySinh,
                        bn.GioiTinh,
                        bn.SoDienThoai,
                        ISNULL(bn.TienSuBenhLy, N'') AS TienSuBenhLy
                    FROM PhieuKham pk
                    JOIN BenhNhan  bn ON pk.MaBenhNhan = bn.MaBenhNhan
                    WHERE pk.MaPhieuKham = @MaPK
                      AND pk.IsDeleted   = 0";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaPK", _maPhieuKham));

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy phiếu khám #" + _maPhieuKham,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataRow r = dt.Rows[0];

                // Lưu MaLichHen để UPDATE khi hoàn thành khám
                // (nullable — lịch tạo từ walk-in không có LichHen)
                _maLichHen = r["MaLichHen"] != DBNull.Value
                    ? Convert.ToInt32(r["MaLichHen"])
                    : -1;

                // ── Thông tin bệnh nhân ──────────────────────────────────────
                string hoTen = r["HoTen"].ToString();
                string sdt = r["SoDienThoai"].ToString();
                int maBN = Convert.ToInt32(r["MaBenhNhan"]);

                string tuoi = "";
                if (r["NgaySinh"] != DBNull.Value)
                {
                    var ns = Convert.ToDateTime(r["NgaySinh"]);
                    int age = DateTime.Today.Year - ns.Year;
                    if (DateTime.Today < ns.AddYears(age)) age--;
                    tuoi = age + " tuổi";
                }

                string gioiTinh = r["GioiTinh"] != DBNull.Value
                    ? (Convert.ToBoolean(r["GioiTinh"]) ? "Nam" : "Nữ")
                    : "—";

                // Avatar
                lblAvatar.Text = string.IsNullOrEmpty(hoTen) ? "?" : hoTen.Substring(0, 1).ToUpper();
                lblTenBN.Text = hoTen + " · BN" + maBN.ToString("D3");
                lblThongTinBN.Text = sdt + " · " + gioiTinh + " · " + tuoi;

                string tienSu = r["TienSuBenhLy"].ToString().Trim();
                lblDiUng.ForeColor = string.IsNullOrEmpty(tienSu)
                    ? Color.FromArgb(107, 114, 128)
                    : Color.FromArgb(217, 119, 6);
                lblDiUng.Text = string.IsNullOrEmpty(tienSu)
                    ? "✓ Không có tiền sử dị ứng đặc biệt"
                    : "⚠️ " + tienSu;

                // ── Thông tin phiếu khám ─────────────────────────────────────
                txtTrieuChung.Text = r["TrieuChung"]?.ToString() ?? "";
                txtChanDoan.Text = r["ChanDoan"]?.ToString() ?? "";

                if (r["NgayTaiKham"] != DBNull.Value)
                    dtpNgayTaiKham.Value = Convert.ToDateTime(r["NgayTaiKham"]);

                int trangThai = Convert.ToInt32(r["TrangThai"]);
                if (trangThai < cboTrangThai.Items.Count)
                    cboTrangThai.SelectedIndex = trangThai;
                else
                    cboTrangThai.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải phiếu khám:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // LOAD DANH SÁCH DỊCH VỤ (checkbox động)
        // ═════════════════════════════════════════════════════════════════════

        private void LoadDanhSachDichVu()
        {
            try
            {
                // Tất cả dịch vụ
                const string sqlDV = @"
                    SELECT MaDichVu, TenDichVu, DonGia FROM DichVu ORDER BY TenDichVu";
                DataTable dtDV = DatabaseConnection.ExecuteQuery(sqlDV);

                // Dịch vụ đã được chọn trong phiếu này
                const string sqlChon = @"
                    SELECT MaDichVu FROM ChiTietDichVu WHERE MaPhieuKham = @MaPK";
                DataTable dtChon = DatabaseConnection.ExecuteQuery(sqlChon,
                    p => p.AddWithValue("@MaPK", _maPhieuKham));

                var daDuocChon = new HashSet<int>();
                if (dtChon != null)
                    foreach (DataRow r in dtChon.Rows)
                        daDuocChon.Add(Convert.ToInt32(r["MaDichVu"]));

                // Xóa danh sách cũ
                pnlDichVuList.Controls.Clear();
                _dsDichVuChk.Clear();

                if (dtDV == null) return;

                int y = 0;
                int w = pnlDichVuList.ClientSize.Width;

                foreach (DataRow row in dtDV.Rows)
                {
                    int maDV = Convert.ToInt32(row["MaDichVu"]);
                    string tenDV = row["TenDichVu"].ToString();
                    decimal donGia = Convert.ToDecimal(row["DonGia"]);
                    bool chon = daDuocChon.Contains(maDV);

                    // Row container
                    var pnlRow = new Panel
                    {
                        Location = new Point(0, y),
                        Size = new Size(w - 2, 38),
                        BackColor = chon ? Color.FromArgb(240, 253, 244) : Color.Transparent,
                        Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                    };

                    // Checkbox
                    var chk = new CheckBox
                    {
                        Location = new Point(4, 10),
                        Size = new Size(18, 18),
                        Checked = chon,
                        Tag = new object[] { maDV, donGia },
                    };

                    // Label tên dịch vụ
                    var lblTen = new Label
                    {
                        Text = tenDV,
                        Font = new Font("Segoe UI", 9.5f),
                        ForeColor = Color.FromArgb(31, 41, 55),
                        Location = new Point(30, 10),
                        AutoSize = false,
                        Size = new Size(w - 170, 20),
                        BackColor = Color.Transparent,
                    };

                    // Label giá tiền
                    var lblGia = new Label
                    {
                        Text = FormatTien(donGia),
                        Font = new Font("Segoe UI", 9.5f),
                        ForeColor = chon ? ClrPrimary : ClrGray,
                        TextAlign = ContentAlignment.MiddleRight,
                        Anchor = AnchorStyles.Top | AnchorStyles.Right,
                        Size = new Size(120, 20),
                        Location = new Point(w - 124, 10),
                        BackColor = Color.Transparent,
                    };

                    // Tag gắn thêm lblGia và pnlRow vào chk để dùng trong event
                    chk.Tag = new object[] { maDV, donGia, lblGia, pnlRow };

                    chk.CheckedChanged += ChkDichVu_CheckedChanged;

                    pnlRow.Controls.Add(chk);
                    pnlRow.Controls.Add(lblTen);
                    pnlRow.Controls.Add(lblGia);
                    pnlDichVuList.Controls.Add(pnlRow);
                    _dsDichVuChk.Add(chk);

                    // Separator
                    var sep = new Panel
                    {
                        Location = new Point(0, y + 37),
                        Size = new Size(w, 1),
                        BackColor = ClrBorder,
                    };
                    pnlDichVuList.Controls.Add(sep);

                    y += 39;
                }

                CapNhatTongDichVu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách dịch vụ:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChkDichVu_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is CheckBox chk)) return;
            if (!(chk.Tag is object[] tag)) return;

            bool chon = chk.Checked;
            int maDV = Convert.ToInt32(tag[0]);
            decimal donGia = Convert.ToDecimal(tag[1]);
            var lblGia = tag[2] as Label;
            var pnlRow = tag[3] as Panel;

            // Cập nhật UI
            if (lblGia != null) lblGia.ForeColor = chon ? ClrPrimary : ClrGray;
            if (pnlRow != null) pnlRow.BackColor = chon
                ? Color.FromArgb(240, 253, 244) : Color.Transparent;

            // Lưu thẳng vào DB
            if (_maPhieuKham <= 0) return;
            try
            {
                if (chon)
                {
                    // Kiểm tra đã có chưa trước khi INSERT
                    DataTable ck = DatabaseConnection.ExecuteQuery(
                        "SELECT 1 FROM ChiTietDichVu WHERE MaPhieuKham=@MaPK AND MaDichVu=@MaDV",
                        p => { p.AddWithValue("@MaPK", _maPhieuKham); p.AddWithValue("@MaDV", maDV); });

                    if (ck == null || ck.Rows.Count == 0)
                        DatabaseConnection.ExecuteNonQuery(
                            "INSERT INTO ChiTietDichVu(MaPhieuKham,MaDichVu,SoLuong,ThanhTien) VALUES(@MaPK,@MaDV,1,@GiaTien)",
                            p => {
                                p.AddWithValue("@MaPK", _maPhieuKham);
                                p.AddWithValue("@MaDV", maDV);
                                p.AddWithValue("@GiaTien", donGia);
                            });
                }
                else
                {
                    DatabaseConnection.ExecuteNonQuery(
                        "DELETE FROM ChiTietDichVu WHERE MaPhieuKham=@MaPK AND MaDichVu=@MaDV",
                        p => { p.AddWithValue("@MaPK", _maPhieuKham); p.AddWithValue("@MaDV", maDV); });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật dịch vụ:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            CapNhatTongDichVu();
        }

        private void CapNhatTongDichVu()
        {
            decimal tong = 0;
            foreach (var chk in _dsDichVuChk)
            {
                if (chk.Checked && chk.Tag is object[] tag)
                    tong += Convert.ToDecimal(tag[1]);
            }
            lblTongDichVu.Text = FormatTien(tong);
        }

        // ═════════════════════════════════════════════════════════════════════
        // LOAD THUỐC TỪ VW_TonKhoTheoLo (FEFO)
        // ═════════════════════════════════════════════════════════════════════

        private void LoadThuocFEFO()
        {
            try
            {
                // Lấy tất cả thuốc còn tồn kho (SoLuongConLai > 0), ưu tiên lô FEFO lên đầu
                const string sql = @"
                    SELECT
                        MaThuoc,
                        TenThuoc + N'  —  HSD: ' + FORMAT(HanSuDung,'MM/yyyy')
                            + N'  (' + CAST(SoLuongConLai AS NVARCHAR) + N' còn)'  AS TenHienThi,
                        TenThuoc,
                        HanSuDung,
                        SoLuongConLai,
                        MaPhieuNhap
                    FROM VW_TonKhoTheoLo
                    WHERE SoLuongConLai > 0
                    ORDER BY TenThuoc, UuTienFEFO ASC, HanSuDung ASC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    cboThuoc.DataSource = null;
                    return;
                }

                cboThuoc.DataSource = dt;
                cboThuoc.DisplayMember = "TenHienThi";
                cboThuoc.ValueMember = "MaThuoc";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách thuốc:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // LOAD ĐƠN THUỐC HIỆN TẠI CỦA PHIẾU KHÁM
        // ═════════════════════════════════════════════════════════════════════

        private void LoadDonThuocHienTai()
        {
            try
            {
                const string sql = @"
                    SELECT
                        t.TenThuoc,
                        t.DonViTinh,
                        cdt.SoLuong,
                        ISNULL(cdt.LieuDung, N'') AS LieuDung,
                        ISNULL(
                            (SELECT TOP 1 FORMAT(v.HanSuDung,'MM/yyyy')
                             FROM VW_TonKhoTheoLo v
                             WHERE v.MaThuoc = cdt.MaThuoc AND v.UuTienFEFO = 1),
                            N'N/A') AS HanSuDung,
                        cdt.MaThuoc
                    FROM ChiTietDonThuoc cdt
                    JOIN Thuoc t ON cdt.MaThuoc = t.MaThuoc
                    WHERE cdt.MaPhieuKham = @MaPK
                    ORDER BY t.TenThuoc";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaPK", _maPhieuKham));

                dgvDonThuoc.DataSource = dt ?? new DataTable();

                // Style hàng xen kẽ
                foreach (DataGridViewRow row in dgvDonThuoc.Rows)
                    row.DefaultCellStyle.BackColor = row.Index % 2 == 0 ? Color.White : ClrRowAlt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải đơn thuốc:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // THÊM THUỐC VÀO ĐƠN
        // ═════════════════════════════════════════════════════════════════════

        private void BtnThemThuoc_Click(object sender, EventArgs e)
        {
            if (cboThuoc.SelectedValue == null || !(cboThuoc.SelectedValue is int))
            {
                MessageBox.Show("Vui lòng chọn thuốc từ danh sách.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maThuoc = Convert.ToInt32(cboThuoc.SelectedValue);

            if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương (≥ 1).",
                    "Số lượng không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }

            string lieuDung = txtLieuDung.Text.Trim();

            // Kiểm tra tồn kho
            DataTable dtTon = DatabaseConnection.ExecuteQuery(
                "SELECT SoLuongConLai FROM VW_TonKhoTheoLo WHERE MaThuoc = @MT AND UuTienFEFO = 1",
                p => p.AddWithValue("@MT", maThuoc));

            if (dtTon == null || dtTon.Rows.Count == 0)
            {
                MessageBox.Show("Thuốc này hiện không có trong kho.",
                    "Hết tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int tonKho = Convert.ToInt32(dtTon.Rows[0]["SoLuongConLai"]);
            if (soLuong > tonKho)
            {
                MessageBox.Show($"Số lượng yêu cầu ({soLuong}) vượt tồn kho ({tonKho}).",
                    "Không đủ tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Kiểm tra đã có trong đơn chưa
                DataTable dtCheck = DatabaseConnection.ExecuteQuery(
                    "SELECT 1 FROM ChiTietDonThuoc WHERE MaPhieuKham = @MaPK AND MaThuoc = @MT",
                    p => { p.AddWithValue("@MaPK", _maPhieuKham); p.AddWithValue("@MT", maThuoc); });

                bool daCoThuoc = dtCheck != null && dtCheck.Rows.Count > 0;

                if (daCoThuoc)
                {
                    var confirm = MessageBox.Show(
                        "Thuốc này đã có trong đơn. Cập nhật số lượng và liều dùng?",
                        "Xác nhận cập nhật", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm != DialogResult.Yes) return;

                    DatabaseConnection.ExecuteNonQuery(
                        "UPDATE ChiTietDonThuoc SET SoLuong=@SL, LieuDung=@LD WHERE MaPhieuKham=@MaPK AND MaThuoc=@MT",
                        p =>
                        {
                            p.AddWithValue("@SL", soLuong);
                            p.AddWithValue("@LD", lieuDung);
                            p.AddWithValue("@MaPK", _maPhieuKham);
                            p.AddWithValue("@MT", maThuoc);
                        });
                }
                else
                {
                    DatabaseConnection.ExecuteNonQuery(
                        "INSERT INTO ChiTietDonThuoc (MaPhieuKham, MaThuoc, SoLuong, LieuDung) VALUES (@MaPK,@MT,@SL,@LD)",
                        p =>
                        {
                            p.AddWithValue("@MaPK", _maPhieuKham);
                            p.AddWithValue("@MT", maThuoc);
                            p.AddWithValue("@SL", soLuong);
                            p.AddWithValue("@LD", lieuDung);
                        });
                }

                // Reset input
                txtSoLuong.Text = "";
                txtLieuDung.Text = "";
                if (cboThuoc.Items.Count > 0) cboThuoc.SelectedIndex = 0;

                LoadDonThuocHienTai();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm thuốc:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // XÓA THUỐC KHI BẤM NÚT ✕ TRONG LƯỚI
        // ═════════════════════════════════════════════════════════════════════

        private void DgvDonThuoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvDonThuoc.Columns[e.ColumnIndex]?.Name != "colXoa") return;

            DataTable dt = dgvDonThuoc.DataSource as DataTable;
            if (dt == null || e.RowIndex >= dt.Rows.Count) return;

            int maThuocXoa = Convert.ToInt32(dt.Rows[e.RowIndex]["MaThuoc"]);
            string tenThuoc = dt.Rows[e.RowIndex]["TenThuoc"].ToString();

            var confirm = MessageBox.Show(
                $"Xóa \"{tenThuoc}\" khỏi đơn thuốc?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                DatabaseConnection.ExecuteNonQuery(
                    "DELETE FROM ChiTietDonThuoc WHERE MaPhieuKham=@MaPK AND MaThuoc=@MT",
                    p =>
                    {
                        p.AddWithValue("@MaPK", _maPhieuKham);
                        p.AddWithValue("@MT", maThuocXoa);
                    });

                LoadDonThuocHienTai();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa thuốc:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvDonThuoc_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Sẽ xử lý inline edit SoLuong/LieuDung trực tiếp trên lưới ở phiên bản sau
        }

        private void DgvDonThuoc_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            // Placeholder
        }

        // ═════════════════════════════════════════════════════════════════════
        // LƯU — LƯU NHÁP (TrangThai=1) / HOÀN THÀNH KHÁM (TrangThai=2)
        // ═════════════════════════════════════════════════════════════════════

        private void BtnLuuNhap_Click(object sender, EventArgs e)
        {
            LuuPhieuKham(trangThai: 1);
        }

        private void BtnHoanThanhKham_Click(object sender, EventArgs e)
        {
            // Kiểm tra lịch hẹn liên kết có bị hủy không
            if (_maLichHen > 0)
            {
                DataTable dtLH = DatabaseConnection.ExecuteQuery(
                    "SELECT TrangThai FROM LichHen WHERE MaLichHen = @MaLH",
                    p => p.AddWithValue("@MaLH", _maLichHen));
                if (dtLH != null && dtLH.Rows.Count > 0
                    && Convert.ToInt32(dtLH.Rows[0]["TrangThai"]) == 3)
                {
                    MessageBox.Show(
                        "Lịch hẹn liên kết với phiếu khám này đã bị hủy.\nKhông thể hoàn thành khám.",
                        "Lịch hẹn đã hủy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(txtTrieuChung.Text))
            {
                MessageBox.Show("Vui lòng nhập triệu chứng trước khi hoàn thành.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTrieuChung.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtChanDoan.Text))
            {
                MessageBox.Show("Vui lòng nhập chẩn đoán trước khi hoàn thành.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtChanDoan.Focus();
                return;
            }

            // Kiểm tra có ít nhất 1 dịch vụ hoặc thuốc
            bool coDichVu = false;
            foreach (var chk in _dsDichVuChk)
                if (chk.Checked) { coDichVu = true; break; }

            DataTable dtThuoc = dgvDonThuoc.DataSource as DataTable;
            bool coThuoc = dtThuoc != null && dtThuoc.Rows.Count > 0;

            if (!coDichVu && !coThuoc)
            {
                var cont = MessageBox.Show(
                    "Chưa chọn dịch vụ hoặc thuốc nào.\nVẫn hoàn thành khám?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (cont != DialogResult.Yes) return;
            }

            var confirmHoan = MessageBox.Show(
                "Xác nhận hoàn thành khám?\n\nSau khi hoàn thành, lễ tân sẽ tiến hành thanh toán hóa đơn.",
                "Hoàn Thành Khám", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmHoan == DialogResult.Yes)
                LuuPhieuKham(trangThai: 2);
        }

        private void LuuPhieuKham(int trangThai)
        {
            if (_maPhieuKham <= 0) return;

            try
            {
                // Lấy ngày tái khám — nếu bằng ngày hôm nay thì coi như chưa đặt
                DateTime? ngayTaiKham = null;
                if (dtpNgayTaiKham.Value.Date > DateTime.Today)
                    ngayTaiKham = dtpNgayTaiKham.Value.Date;

                bool ok = DatabaseConnection.ExecuteTransaction((conn, tran) =>
                {
                    // 1. Cập nhật PhieuKham
                    // GhiChu (NVARCHAR(200), nullable) — NULL có chủ đích.
                    // Ghi chú lâm sàng được quản lý riêng trong bảng GhiChuKham
                    // để hỗ trợ nhiều ghi chú theo thời gian (xem Tab Ghi Chú).
                    using (var cmd = new System.Data.SqlClient.SqlCommand(@"
                        UPDATE PhieuKham SET
                            TrieuChung  = @TrieuChung,
                            ChanDoan    = @ChanDoan,
                            NgayTaiKham = @NgayTaiKham,
                            TrangThai   = @TrangThai
                        WHERE MaPhieuKham = @MaPK", conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@TrieuChung", txtTrieuChung.Text.Trim());
                        cmd.Parameters.AddWithValue("@ChanDoan", txtChanDoan.Text.Trim());
                        cmd.Parameters.AddWithValue("@NgayTaiKham", (object)ngayTaiKham ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                        cmd.Parameters.AddWithValue("@MaPK", _maPhieuKham);
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Nếu hoàn thành khám → cập nhật LichHen về TrangThai=2 (Hoàn thành)
                    if (trangThai == 2 && _maLichHen > 0)
                    {
                        using (var cmdLH = new System.Data.SqlClient.SqlCommand(@"
                            UPDATE LichHen
                            SET TrangThai = 2
                            WHERE MaLichHen = @MaLH
                              AND TrangThai <> 3", conn, tran))
                        {
                            cmdLH.Parameters.AddWithValue("@MaLH", _maLichHen);
                            cmdLH.ExecuteNonQuery();
                        }
                    }

                    // 3. Nếu hoàn thành khám → tạo HoaDon chờ thanh toán (TrangThai=0)
                    //    để Dashboard Lễ Tân hiển thị "Hóa đơn cần thu"
                    if (trangThai == 2)
                    {
                        // Kiểm tra chưa có HoaDon cho phiếu này
                        using (var cmdCk = new System.Data.SqlClient.SqlCommand(@"
                            SELECT COUNT(*) FROM HoaDon
                            WHERE MaPhieuKham = @MaPK AND IsDeleted = 0", conn, tran))
                        {
                            cmdCk.Parameters.AddWithValue("@MaPK", _maPhieuKham);
                            int daCoHD = Convert.ToInt32(cmdCk.ExecuteScalar());

                            if (daCoHD == 0)
                            {
                                // Tính tổng tiền dịch vụ
                                decimal tongDV = 0;
                                using (var cmdDV = new System.Data.SqlClient.SqlCommand(@"
                                    SELECT ISNULL(SUM(ThanhTien), 0) FROM ChiTietDichVu
                                    WHERE MaPhieuKham = @MaPK", conn, tran))
                                {
                                    cmdDV.Parameters.AddWithValue("@MaPK", _maPhieuKham);
                                    tongDV = Convert.ToDecimal(cmdDV.ExecuteScalar());
                                }

                                // Tính tổng tiền thuốc
                                decimal tongThuoc = 0;
                                using (var cmdTh = new System.Data.SqlClient.SqlCommand(@"
                                    SELECT ISNULL(SUM(cdt.SoLuong * t.DonGia), 0)
                                    FROM ChiTietDonThuoc cdt
                                    JOIN Thuoc t ON cdt.MaThuoc = t.MaThuoc
                                    WHERE cdt.MaPhieuKham = @MaPK", conn, tran))
                                {
                                    cmdTh.Parameters.AddWithValue("@MaPK", _maPhieuKham);
                                    tongThuoc = Convert.ToDecimal(cmdTh.ExecuteScalar());
                                }

                                decimal tongTien = tongDV + tongThuoc;

                                // INSERT HoaDon chờ thanh toán
                                using (var cmdHD = new System.Data.SqlClient.SqlCommand(@"
                                    INSERT INTO HoaDon
                                        (MaPhieuKham, TongTien, TongTienDichVu, TongThuoc,
                                         GiamGia, TienKhachTra, TienThua, TrangThai)
                                    VALUES
                                        (@MaPK, @TongTien, @TongDV, @TongThuoc,
                                         0, 0, 0, 0)", conn, tran))
                                {
                                    cmdHD.Parameters.AddWithValue("@MaPK", _maPhieuKham);
                                    cmdHD.Parameters.AddWithValue("@TongTien", tongTien);
                                    cmdHD.Parameters.AddWithValue("@TongDV", tongDV);
                                    cmdHD.Parameters.AddWithValue("@TongThuoc", tongThuoc);
                                    cmdHD.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                });

                if (!ok)
                {
                    MessageBox.Show("Lưu không thành công. Vui lòng thử lại.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (trangThai == 2)
                {
                    _daHoanThanh = true;
                    MessageBox.Show(
                        "✅ Hoàn thành khám thành công!\n\nLễ tân có thể tiến hành thu tiền tại InvoiceForm.",
                        "Hoàn Thành", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("💾 Đã lưu nháp phiếu khám.",
                        "Lưu nháp", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Cập nhật trạng thái hiển thị
                    cboTrangThai.SelectedIndex = trangThai < cboTrangThai.Items.Count ? trangThai : 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu phiếu khám:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // TAB SWITCHING
        // ═════════════════════════════════════════════════════════════════════

        private void ChuyenTab(Guna2Button tab)
        {
            // Reset tất cả tab inactive
            var tatCaTab = new[] { btnTabChanDoan, btnTabDichVu, btnTabKeDonThuoc, btnTabHinhAnh, btnTabGhiChu };
            foreach (var t in tatCaTab)
            {
                t.ForeColor = ClrGray;
                t.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Regular);
            }

            // Active tab
            tab.ForeColor = ClrPrimary;
            tab.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);
            _tabDangActive = tab;
            pnlTabs.Invalidate();

            // Xóa nội dung tab cũ
            XoaTabContent();

            if (tab == btnTabChanDoan)
            {
                tlpMain.Visible = true;
                pnlDonThuoc.Visible = true;

                if (_maPhieuKham > 0)
                {
                    LoadDanhSachDichVu();
                    LoadDonThuocHienTai();
                }
            }
            else
            {
                tlpMain.Visible = false;
                pnlDonThuoc.Visible = false;

                if (tab == btnTabDichVu) HienThiTabDichVu();
                else if (tab == btnTabKeDonThuoc) HienThiTabKeDonThuoc();
                else if (tab == btnTabHinhAnh) HienThiTabHinhAnh();
                else if (tab == btnTabGhiChu) HienThiTabGhiChu();
            }
        }

        private void XoaTabContent()
        {
            if (_pnlTabContent != null && pnlContent.Controls.Contains(_pnlTabContent))
            {
                pnlContent.Controls.Remove(_pnlTabContent);
                _pnlTabContent.Dispose();
                _pnlTabContent = null;
            }
        }

        private Panel TaoPnlTabContent()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(244, 247, 246),
                Padding = new Padding(0, 8, 0, 0),
            };
            pnlContent.Controls.Add(pnl);
            pnl.BringToFront();
            _pnlTabContent = pnl;
            return pnl;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // COPY TOÀN BỘ FILE NÀY VÀO PhieuKhamForm.cs
        // Tìm 4 method cũ (HienThiTabDichVu, HienThiTabKeDonThuoc,
        // HienThiTabHinhAnh, HienThiTabGhiChu) và THAY BẰNG code dưới đây
        // ═══════════════════════════════════════════════════════════════════════

        // ═══════════════════════════════════════════════════════════════════════
        // TAB DỊCH VỤ
        // ═══════════════════════════════════════════════════════════════════════
        private void HienThiTabDichVu()
        {
            var pnl = TaoPnlTabContent();

            var card = new Guna.UI2.WinForms.Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                BorderRadius = 12,
                Padding = new Padding(16, 0, 16, 12),
            };
            pnl.Controls.Add(card);

            // ── Tổng tiền (Bottom — thêm TRƯỚC) ────────────────────────────
            var pnlTong = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(245, 251, 247),
                Padding = new Padding(4, 0, 4, 0),
            };
            var lblTongLabel = new Label
            {
                Text = "Tổng dịch vụ:",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                AutoSize = true,
            };
            var lblTongVal = new Label
            {
                Text = "0đ",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = ClrPrimary,
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent,
                AutoSize = true,
            };
            pnlTong.Controls.Add(lblTongVal);
            pnlTong.Controls.Add(lblTongLabel);
            card.Controls.Add(pnlTong);

            // ── Toolbar thêm dịch vụ (Bottom — thêm TRƯỚC) ──────────────────
            var pnlSep2 = new Panel { Dock = DockStyle.Bottom, Height = 8, BackColor = Color.Transparent };
            card.Controls.Add(pnlSep2);

            var pnlAdd = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.Transparent,
            };

            var btnThem = new Guna.UI2.WinForms.Guna2Button
            {
                Dock = DockStyle.Right,
                Width = 110,
                Text = "+ Thêm DV",
                FillColor = ClrPrimary,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                Margin = new Padding(8, 0, 0, 0),
            };
            var txtSL = new Guna.UI2.WinForms.Guna2TextBox
            {
                Dock = DockStyle.Right,
                Width = 70,
                PlaceholderText = "SL",
                BorderRadius = 8,
                BorderColor = Color.FromArgb(209, 213, 219),
                FillColor = Color.White,
                Font = new Font("Segoe UI", 9.5f),
                Margin = new Padding(6, 0, 0, 0),
            };
            var cboDV = new Guna.UI2.WinForms.Guna2ComboBox
            {
                Dock = DockStyle.Fill,
                BorderRadius = 8,
                BorderColor = Color.FromArgb(209, 213, 219),
                FillColor = Color.White,
                Font = new Font("Segoe UI", 9.5f),
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            DataTable dtDV = DatabaseConnection.ExecuteQuery(
                "SELECT MaDichVu, TenDichVu + N'  —  ' + FORMAT(DonGia,'N0') + N'đ' AS TenHienThi, DonGia FROM DichVu ORDER BY TenDichVu");
            cboDV.DataSource = dtDV;
            cboDV.DisplayMember = "TenHienThi";
            cboDV.ValueMember = "MaDichVu";

            pnlAdd.Controls.Add(cboDV);
            pnlAdd.Controls.Add(txtSL);
            pnlAdd.Controls.Add(btnThem);
            card.Controls.Add(pnlAdd);

            // ── Separator (Bottom) ───────────────────────────────────────────
            var pnlSep3 = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = Color.FromArgb(229, 231, 235),
                Margin = new Padding(0, 4, 0, 4),
            };
            card.Controls.Add(pnlSep3);

            // ── DataGridView (Fill — thêm SAU Bottom) ────────────────────────
            var dgv = new Guna.UI2.WinForms.Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                ColumnHeadersHeight = 38,
                RowTemplate = { Height = 36 },
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(238, 246, 241),
            };
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ClrPrimary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 0, 0);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 242, 230);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 92, 77);

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDV_Ten", DataPropertyName = "TenDichVu", HeaderText = "Tên dịch vụ", FillWeight = 220, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDV_DonGia", DataPropertyName = "DonGia", HeaderText = "Đơn giá", FillWeight = 110, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDV_SoLuong", DataPropertyName = "SoLuong", HeaderText = "SL", FillWeight = 60, ReadOnly = false });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDV_ThanhTien", DataPropertyName = "ThanhTien", HeaderText = "Thành tiền", FillWeight = 110, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colDV_Xoa",
                HeaderText = "",
                FillWeight = 44,
                Text = "✕",
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = {
                    BackColor = Color.FromArgb(254,226,226), ForeColor = Color.FromArgb(185,28,28),
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                }
            });
            card.Controls.Add(dgv);

            // ── Title (Top — thêm SAU CÙNG) ──────────────────────────────────
            var pnlTitleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 0),
            };
            var sep = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(229, 231, 235) };
            var lblTitle = new Label
            {
                Text = "✨  Dịch Vụ Đã Thực Hiện",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = ClrPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Padding = new Padding(2, 0, 0, 0),
            };
            pnlTitleBar.Controls.Add(sep);
            pnlTitleBar.Controls.Add(lblTitle);
            card.Controls.Add(pnlTitleBar);

            // ── Load & Events ─────────────────────────────────────────────────
            Action loadDV = () =>
            {
                DataTable dt = DatabaseConnection.ExecuteQuery(@"
                    SELECT cdv.MaDichVu, dv.TenDichVu,
                           FORMAT(dv.DonGia,'N0') + N'đ'                AS DonGia,
                           cdv.SoLuong,
                           FORMAT(cdv.ThanhTien * cdv.SoLuong,'N0') + N'đ' AS ThanhTien
                    FROM ChiTietDichVu cdv
                    JOIN DichVu dv ON cdv.MaDichVu = dv.MaDichVu
                    WHERE cdv.MaPhieuKham = @MaPK
                    ORDER BY dv.TenDichVu",
                    p => p.AddWithValue("@MaPK", _maPhieuKham));
                dgv.DataSource = dt ?? new DataTable();

                object tongVal = DatabaseConnection.ExecuteScalar(
                    "SELECT ISNULL(SUM(ThanhTien * SoLuong),0) FROM ChiTietDichVu WHERE MaPhieuKham=@MaPK",
                    p => p.AddWithValue("@MaPK", _maPhieuKham));
                lblTongVal.Text = FormatTien(tongVal != null ? Convert.ToDecimal(tongVal) : 0);
            };
            loadDV();

            btnThem.Click += (s, e) =>
            {
                if (cboDV.SelectedValue == null) return;
                if (!int.TryParse(txtSL.Text.Trim(), out int sl) || sl <= 0) sl = 1;
                int maDV = Convert.ToInt32(cboDV.SelectedValue);
                decimal donGia = Convert.ToDecimal(((DataRowView)cboDV.SelectedItem)["DonGia"]);

                DataTable ck = DatabaseConnection.ExecuteQuery(
                    "SELECT 1 FROM ChiTietDichVu WHERE MaPhieuKham=@MaPK AND MaDichVu=@MaDV",
                    p => { p.AddWithValue("@MaPK", _maPhieuKham); p.AddWithValue("@MaDV", maDV); });

                if (ck != null && ck.Rows.Count > 0)
                    DatabaseConnection.ExecuteNonQuery(
                        "UPDATE ChiTietDichVu SET SoLuong=@SL, ThanhTien=@GiaTien WHERE MaPhieuKham=@MaPK AND MaDichVu=@MaDV",
                        p => { p.AddWithValue("@SL", sl); p.AddWithValue("@GiaTien", donGia); p.AddWithValue("@MaPK", _maPhieuKham); p.AddWithValue("@MaDV", maDV); });
                else
                    DatabaseConnection.ExecuteNonQuery(
                        "INSERT INTO ChiTietDichVu(MaPhieuKham,MaDichVu,SoLuong,ThanhTien) VALUES(@MaPK,@MaDV,@SL,@GiaTien)",
                        p => { p.AddWithValue("@MaPK", _maPhieuKham); p.AddWithValue("@MaDV", maDV); p.AddWithValue("@SL", sl); p.AddWithValue("@GiaTien", donGia); });

                txtSL.Text = "";
                loadDV();
            };

            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0 || dgv.Columns[e.ColumnIndex]?.Name != "colDV_Xoa") return;
                DataTable dt2 = (DataTable)dgv.DataSource;
                int maDVXoa = Convert.ToInt32(dt2.Rows[e.RowIndex]["MaDichVu"]);
                if (MessageBox.Show("Xóa dịch vụ này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                DatabaseConnection.ExecuteNonQuery(
                    "DELETE FROM ChiTietDichVu WHERE MaPhieuKham=@MaPK AND MaDichVu=@MaDV",
                    p => { p.AddWithValue("@MaPK", _maPhieuKham); p.AddWithValue("@MaDV", maDVXoa); });
                loadDV();
            };
        }

        // ═══════════════════════════════════════════════════════════════════════
        // TAB KÊ ĐƠN THUỐC
        // ═══════════════════════════════════════════════════════════════════════
        private void HienThiTabKeDonThuoc()
        {
            var pnl = TaoPnlTabContent();

            var card = new Guna.UI2.WinForms.Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                BorderRadius = 12,
                Padding = new Padding(16, 0, 16, 12),
            };
            pnl.Controls.Add(card);

            // ── Toolbar thêm thuốc (Bottom — thêm TRƯỚC) ─────────────────────
            var pnlSepBot = new Panel { Dock = DockStyle.Bottom, Height = 8, BackColor = Color.Transparent };
            card.Controls.Add(pnlSepBot);

            var pnlAdd = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.Transparent,
            };

            var btnThem2 = new Guna.UI2.WinForms.Guna2GradientButton
            {
                Dock = DockStyle.Right,
                Width = 100,
                Text = "+ Thêm",
                FillColor = ClrPrimary,
                FillColor2 = Color.SeaGreen,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                Margin = new Padding(8, 0, 0, 0),
            };
            var txtLieu2 = new Guna.UI2.WinForms.Guna2TextBox
            {
                Dock = DockStyle.Right,
                Width = 260,
                PlaceholderText = "Liều dùng (vd: 2 lần/ngày, sau ăn)",
                BorderRadius = 8,
                BorderColor = Color.FromArgb(209, 213, 219),
                FillColor = Color.White,
                Font = new Font("Segoe UI", 9.5f),
                Margin = new Padding(6, 0, 0, 0),
            };
            var txtSL2 = new Guna.UI2.WinForms.Guna2TextBox
            {
                Dock = DockStyle.Right,
                Width = 65,
                PlaceholderText = "SL",
                BorderRadius = 8,
                BorderColor = Color.FromArgb(209, 213, 219),
                FillColor = Color.White,
                Font = new Font("Segoe UI", 9.5f),
                Margin = new Padding(6, 0, 0, 0),
            };
            var cboThuoc2 = new Guna.UI2.WinForms.Guna2ComboBox
            {
                Dock = DockStyle.Fill,
                BorderRadius = 8,
                BorderColor = Color.FromArgb(209, 213, 219),
                FillColor = Color.White,
                Font = new Font("Segoe UI", 9.5f),
                DropDownStyle = ComboBoxStyle.DropDownList,
            };

            DataTable dtThuocFEFO = DatabaseConnection.ExecuteQuery(@"
                SELECT MaThuoc,
                       TenThuoc + N'  —  HSD: ' + FORMAT(HanSuDung,'MM/yyyy')
                       + N'  (' + CAST(SoLuongConLai AS NVARCHAR) + N' còn)' AS TenHienThi,
                       HanSuDung, SoLuongConLai
                FROM VW_TonKhoTheoLo
                WHERE SoLuongConLai > 0
                ORDER BY TenThuoc, UuTienFEFO ASC, HanSuDung ASC");
            cboThuoc2.DataSource = dtThuocFEFO;
            cboThuoc2.DisplayMember = "TenHienThi";
            cboThuoc2.ValueMember = "MaThuoc";

            pnlAdd.Controls.Add(cboThuoc2);
            pnlAdd.Controls.Add(txtSL2);
            pnlAdd.Controls.Add(txtLieu2);
            pnlAdd.Controls.Add(btnThem2);
            card.Controls.Add(pnlAdd);

            // ── Separator (Bottom) ───────────────────────────────────────────
            var pnlSep = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(229, 231, 235) };
            card.Controls.Add(pnlSep);

            // ── DataGridView (Fill — thêm SAU Bottom) ────────────────────────
            var dgv2 = new Guna.UI2.WinForms.Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                ColumnHeadersHeight = 38,
                RowTemplate = { Height = 36 },
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(238, 246, 241),
            };
            dgv2.EnableHeadersVisualStyles = false;
            dgv2.ColumnHeadersDefaultCellStyle.BackColor = ClrPrimary;
            dgv2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv2.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgv2.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 0, 0);
            dgv2.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgv2.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 242, 230);
            dgv2.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 92, 77);

            dgv2.Columns.Add(new DataGridViewTextBoxColumn { Name = "c2_Ten", DataPropertyName = "TenThuoc", HeaderText = "Tên thuốc", FillWeight = 200, ReadOnly = true });
            dgv2.Columns.Add(new DataGridViewTextBoxColumn { Name = "c2_DonVi", DataPropertyName = "DonViTinh", HeaderText = "Đơn vị", FillWeight = 80, ReadOnly = true });
            dgv2.Columns.Add(new DataGridViewTextBoxColumn { Name = "c2_SL", DataPropertyName = "SoLuong", HeaderText = "SL", FillWeight = 60, ReadOnly = false });
            dgv2.Columns.Add(new DataGridViewTextBoxColumn { Name = "c2_Lieu", DataPropertyName = "LieuDung", HeaderText = "Liều dùng", FillWeight = 220, ReadOnly = false });
            dgv2.Columns.Add(new DataGridViewTextBoxColumn { Name = "c2_HSD", DataPropertyName = "HanSuDung", HeaderText = "HSD", FillWeight = 90, ReadOnly = true });
            dgv2.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "c2_Xoa",
                HeaderText = "",
                FillWeight = 44,
                Text = "✕",
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = {
                    BackColor = Color.FromArgb(254,226,226), ForeColor = Color.FromArgb(185,28,28),
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                }
            });
            card.Controls.Add(dgv2);

            // ── Title (Top — thêm SAU CÙNG) ──────────────────────────────────
            var pnlTitleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 0),
            };
            var sepTop = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(229, 231, 235) };
            var lblTitle = new Label
            {
                Text = "💊  Kê Đơn Thuốc",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = ClrPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Padding = new Padding(2, 0, 0, 0),
            };
            pnlTitleBar.Controls.Add(sepTop);
            pnlTitleBar.Controls.Add(lblTitle);
            card.Controls.Add(pnlTitleBar);

            // ── Load & Events ─────────────────────────────────────────────────
            Action loadThuoc = () =>
            {
                DataTable dt = DatabaseConnection.ExecuteQuery(@"
                    SELECT t.TenThuoc, t.DonViTinh, cdt.SoLuong,
                           ISNULL(cdt.LieuDung, N'') AS LieuDung,
                           ISNULL((SELECT TOP 1 FORMAT(v.HanSuDung,'MM/yyyy')
                                   FROM VW_TonKhoTheoLo v
                                   WHERE v.MaThuoc = cdt.MaThuoc AND v.UuTienFEFO = 1), N'N/A') AS HanSuDung,
                           cdt.MaThuoc
                    FROM ChiTietDonThuoc cdt
                    JOIN Thuoc t ON cdt.MaThuoc = t.MaThuoc
                    WHERE cdt.MaPhieuKham = @MaPK
                    ORDER BY t.TenThuoc",
                    p => p.AddWithValue("@MaPK", _maPhieuKham));
                dgv2.DataSource = dt ?? new DataTable();
            };
            loadThuoc();

            btnThem2.Click += (s, e) =>
            {
                if (cboThuoc2.SelectedValue == null) return;
                int maThuoc = Convert.ToInt32(cboThuoc2.SelectedValue);
                if (!int.TryParse(txtSL2.Text.Trim(), out int sl) || sl <= 0) sl = 1;
                string lieu = txtLieu2.Text.Trim();

                // Kiểm tra tồn kho trước khi kê
                DataTable dtTon = DatabaseConnection.ExecuteQuery(
                    "SELECT ISNULL(SUM(SoLuongConLai), 0) AS TonKho FROM ChiTietNhapKho WHERE MaThuoc=@MT AND HanSuDung > GETDATE()",
                    p => p.AddWithValue("@MT", maThuoc));
                int tonKho = dtTon != null && dtTon.Rows.Count > 0
                    ? Convert.ToInt32(dtTon.Rows[0]["TonKho"]) : 0;
                if (tonKho < sl)
                {
                    MessageBox.Show($"Không đủ tồn kho. Hiện còn {tonKho} đơn vị.",
                        "Thiếu hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataTable ck = DatabaseConnection.ExecuteQuery(
                    "SELECT SoLuong FROM ChiTietDonThuoc WHERE MaPhieuKham=@MaPK AND MaThuoc=@MT",
                    p => { p.AddWithValue("@MaPK", _maPhieuKham); p.AddWithValue("@MT", maThuoc); });

                if (ck != null && ck.Rows.Count > 0)
                {
                    // Thuốc đã có trong đơn → tính chênh lệch SL để điều chỉnh tồn
                    int slCu = Convert.ToInt32(ck.Rows[0]["SoLuong"]);
                    int chenh = sl - slCu; // dương = thêm, âm = bớt

                    DatabaseConnection.ExecuteNonQuery(
                        "UPDATE ChiTietDonThuoc SET SoLuong=@SL, LieuDung=@LD WHERE MaPhieuKham=@MaPK AND MaThuoc=@MT",
                        p => { p.AddWithValue("@SL", sl); p.AddWithValue("@LD", lieu); p.AddWithValue("@MaPK", _maPhieuKham); p.AddWithValue("@MT", maThuoc); });

                    if (chenh != 0)
                    {
                        // Điều chỉnh tồn kho theo chênh lệch (FEFO — lô sớm hết hạn nhất)
                        DatabaseConnection.ExecuteNonQuery(@"
                            UPDATE TOP(1) ChiTietNhapKho
                            SET SoLuongConLai = SoLuongConLai - @Chenh
                            WHERE MaThuoc = @MT
                              AND HanSuDung = (
                                  SELECT TOP 1 HanSuDung FROM ChiTietNhapKho
                                  WHERE MaThuoc = @MT AND SoLuongConLai > 0
                                    AND HanSuDung > GETDATE()
                                  ORDER BY HanSuDung ASC);
                            UPDATE Thuoc SET SoLuongTon = SoLuongTon - @Chenh WHERE MaThuoc = @MT;",
                            p => { p.AddWithValue("@MT", maThuoc); p.AddWithValue("@Chenh", chenh); });
                    }
                }
                else
                {
                    // Thuốc mới → INSERT + giảm tồn kho
                    DatabaseConnection.ExecuteNonQuery(
                        "INSERT INTO ChiTietDonThuoc(MaPhieuKham,MaThuoc,SoLuong,LieuDung) VALUES(@MaPK,@MT,@SL,@LD)",
                        p => { p.AddWithValue("@MaPK", _maPhieuKham); p.AddWithValue("@MT", maThuoc); p.AddWithValue("@SL", sl); p.AddWithValue("@LD", lieu); });

                    // Giảm tồn kho theo FEFO
                    DatabaseConnection.ExecuteNonQuery(@"
                        UPDATE TOP(1) ChiTietNhapKho
                        SET SoLuongConLai = SoLuongConLai - @SL
                        WHERE MaThuoc = @MT
                          AND HanSuDung = (
                              SELECT TOP 1 HanSuDung FROM ChiTietNhapKho
                              WHERE MaThuoc = @MT AND SoLuongConLai >= @SL
                                AND HanSuDung > GETDATE()
                              ORDER BY HanSuDung ASC);
                        UPDATE Thuoc SET SoLuongTon = SoLuongTon - @SL WHERE MaThuoc = @MT;",
                        p => { p.AddWithValue("@MT", maThuoc); p.AddWithValue("@SL", sl); });
                }

                txtSL2.Text = "";
                txtLieu2.Text = "";
                loadThuoc();
            };

            dgv2.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0 || dgv2.Columns[e.ColumnIndex]?.Name != "c2_Xoa") return;
                DataTable dt2 = (DataTable)dgv2.DataSource;
                int maThuocXoa = Convert.ToInt32(dt2.Rows[e.RowIndex]["MaThuoc"]);
                int slHoanTra = Convert.ToInt32(dt2.Rows[e.RowIndex]["SoLuong"]);

                if (MessageBox.Show("Xóa thuốc này khỏi đơn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                DatabaseConnection.ExecuteNonQuery(
                    "DELETE FROM ChiTietDonThuoc WHERE MaPhieuKham=@MaPK AND MaThuoc=@MT",
                    p => { p.AddWithValue("@MaPK", _maPhieuKham); p.AddWithValue("@MT", maThuocXoa); });

                // Hoàn trả tồn kho — trả về lô sớm hết hạn nhất (FEFO)
                DatabaseConnection.ExecuteNonQuery(@"
                    UPDATE TOP(1) ChiTietNhapKho
                    SET SoLuongConLai = SoLuongConLai + @SL
                    WHERE MaThuoc = @MT
                      AND HanSuDung = (
                          SELECT TOP 1 HanSuDung FROM ChiTietNhapKho
                          WHERE MaThuoc = @MT
                          ORDER BY HanSuDung ASC);
                    UPDATE Thuoc SET SoLuongTon = SoLuongTon + @SL WHERE MaThuoc = @MT;",
                    p => { p.AddWithValue("@MT", maThuocXoa); p.AddWithValue("@SL", slHoanTra); });

                loadThuoc();
            };
        }

        // ═══════════════════════════════════════════════════════════════════════
        // TAB HÌNH ẢNH
        // ═══════════════════════════════════════════════════════════════════════
        private void HienThiTabHinhAnh()
        {
            var pnl = TaoPnlTabContent();

            var card = new Guna.UI2.WinForms.Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                BorderRadius = 12,
                Padding = new Padding(16, 0, 16, 12),
            };
            pnl.Controls.Add(card);

            // ── Title bar + nút Upload (Top — thêm SAU CÙNG) ─────────────────
            var pnlTitleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 6),
            };
            var sepTop = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(229, 231, 235) };

            var btnUpload = new Guna.UI2.WinForms.Guna2Button
            {
                Dock = DockStyle.Right,
                Width = 150,
                Text = "📷  Upload Ảnh",
                FillColor = ClrPrimary,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
            };
            var lblTitleHA = new Label
            {
                Text = "🖼️  Hình Ảnh Bệnh Lý",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = ClrPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Padding = new Padding(2, 0, 0, 0),
            };
            pnlTitleBar.Controls.Add(sepTop);
            pnlTitleBar.Controls.Add(btnUpload);
            pnlTitleBar.Controls.Add(lblTitleHA);

            // ── Gallery (Fill — thêm TRƯỚC title) ────────────────────────────
            var flowAnh = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(249, 250, 251),
                Padding = new Padding(8),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
            };
            card.Controls.Add(flowAnh);
            card.Controls.Add(pnlTitleBar);  // Title SAU CÙNG

            // ── Load ảnh ─────────────────────────────────────────────────────
            Action loadAnh = null;
            loadAnh = () =>
            {
                flowAnh.Controls.Clear();
                DataTable dtAnh = DatabaseConnection.ExecuteQuery(@"
                    SELECT MaHinhAnh, DuongDanAnh,
                           ISNULL(GhiChu, N'') AS GhiChu,
                           FORMAT(NgayChup,'dd/MM/yyyy HH:mm') AS NgayChupText
                    FROM HinhAnhBenhLy WHERE MaPhieuKham=@MaPK ORDER BY NgayChup DESC",
                    p => p.AddWithValue("@MaPK", _maPhieuKham));

                if (dtAnh == null || dtAnh.Rows.Count == 0)
                {
                    var lblEmpty = new Label
                    {
                        Text = "📷  Chưa có hình ảnh nào\nBấm \"Upload Ảnh\" để thêm",
                        Font = new Font("Segoe UI", 11f),
                        ForeColor = ClrGray,
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoSize = false,
                        Size = new Size(400, 120),
                        BackColor = Color.Transparent,
                        Margin = new Padding(80, 60, 0, 0),
                    };
                    flowAnh.Controls.Add(lblEmpty);
                    return;
                }

                foreach (DataRow row in dtAnh.Rows)
                {
                    int maAnh = Convert.ToInt32(row["MaHinhAnh"]);
                    string duongDan = row["DuongDanAnh"].ToString();
                    string ghiChu = row["GhiChu"].ToString();
                    string ngayText = row["NgayChupText"].ToString();

                    var cardAnh = new Panel
                    {
                        Size = new Size(210, 216),
                        Margin = new Padding(8),
                        BackColor = Color.White,
                        BorderStyle = BorderStyle.FixedSingle,
                    };

                    var pic = new PictureBox
                    {
                        Location = new Point(0, 0),
                        Size = new Size(208, 160),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.FromArgb(240, 253, 244),
                    };

                    if (System.IO.File.Exists(duongDan))
                    {
                        try { pic.Image = Image.FromFile(duongDan); }
                        catch { pic.BackColor = Color.FromArgb(229, 231, 235); }
                    }
                    else
                    {
                        pic.BackColor = Color.FromArgb(229, 231, 235);
                        pic.Controls.Add(new Label
                        {
                            Text = System.IO.Path.GetFileName(duongDan),
                            Font = new Font("Segoe UI", 8f),
                            ForeColor = ClrGray,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Dock = DockStyle.Fill,
                            BackColor = Color.Transparent,
                        });
                    }

                    var pnlAnhBottom = new Panel
                    {
                        Location = new Point(0, 160),
                        Size = new Size(208, 48),
                        BackColor = Color.White,
                        Padding = new Padding(4, 4, 4, 4),
                    };

                    var lblGhiChu = new Label
                    {
                        Text = string.IsNullOrEmpty(ghiChu) ? ngayText : ghiChu,
                        Font = new Font("Segoe UI", 8f),
                        ForeColor = Color.FromArgb(55, 65, 81),
                        Dock = DockStyle.Fill,
                        BackColor = Color.Transparent,
                        AutoEllipsis = false,
                        TextAlign = ContentAlignment.MiddleLeft,
                        // Hiển thị tối đa 2 dòng
                        MaximumSize = new Size(160, 40),
                    };

                    var btnXoaAnh = new Button
                    {
                        Text = "✕",
                        Dock = DockStyle.Right,
                        Width = 24,
                        Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                        ForeColor = Color.FromArgb(185, 28, 28),
                        BackColor = Color.FromArgb(254, 226, 226),
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Tag = maAnh,
                        TextAlign = ContentAlignment.MiddleCenter,
                    };
                    btnXoaAnh.FlatAppearance.BorderSize = 0;
                    btnXoaAnh.Click += (s2, e2) =>
                    {
                        if (MessageBox.Show("Xóa hình ảnh này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                        DatabaseConnection.ExecuteNonQuery(
                            "DELETE FROM HinhAnhBenhLy WHERE MaHinhAnh=@MaAnh",
                            p => p.AddWithValue("@MaAnh", (int)((Button)s2).Tag));
                        loadAnh();
                    };

                    pnlAnhBottom.Controls.Add(btnXoaAnh);
                    pnlAnhBottom.Controls.Add(lblGhiChu);
                    cardAnh.Controls.Add(pic);
                    cardAnh.Controls.Add(pnlAnhBottom);
                    flowAnh.Controls.Add(cardAnh);
                }
            };
            loadAnh();

            // ── Upload ────────────────────────────────────────────────────────
            btnUpload.Click += (s, e) =>
            {
                if (_maPhieuKham <= 0)
                {
                    MessageBox.Show(
                        "Chưa chọn phiếu khám cụ thể.\n\n" +
                        "Vui lòng mở Phiếu Khám từ Dashboard → Bắt Đầu Khám,\n" +
                        "sau đó quay lại tab Hình Ảnh để upload.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var ofd = new OpenFileDialog())
                {
                    ofd.Title = "Chọn hình ảnh bệnh lý";
                    ofd.Filter = "Hình ảnh|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Tất cả|*.*";
                    ofd.Multiselect = true;
                    if (ofd.ShowDialog() != DialogResult.OK) return;

                    string ghiChuNhap = "";
                    if (ofd.FileNames.Length == 1)
                        ghiChuNhap = Microsoft.VisualBasic.Interaction.InputBox(
                            "Ghi chú cho ảnh (có thể để trống):", "Ghi chú hình ảnh", "");

                    foreach (string filePath in ofd.FileNames)
                        DatabaseConnection.ExecuteNonQuery(
                            "INSERT INTO HinhAnhBenhLy(MaPhieuKham,DuongDanAnh,GhiChu,NgayChup) VALUES(@MaPK,@Path,@GhiChu,GETDATE())",
                            p =>
                            {
                                p.AddWithValue("@MaPK", _maPhieuKham);
                                p.AddWithValue("@Path", filePath);
                                p.AddWithValue("@GhiChu", string.IsNullOrEmpty(ghiChuNhap) ? (object)DBNull.Value : ghiChuNhap);
                            });
                    loadAnh();
                }
            };
        }

        // ═══════════════════════════════════════════════════════════════════════
        // TAB GHI CHÚ
        // ═══════════════════════════════════════════════════════════════════════
        private void HienThiTabGhiChu()
        {
            var pnl = TaoPnlTabContent();

            var tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55f));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45f));
            pnl.Controls.Add(tlp);

            // ── Cột trái: soạn ghi chú ───────────────────────────────────────
            var cardMoi = new Guna.UI2.WinForms.Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                BorderRadius = 12,
                Padding = new Padding(16, 0, 16, 12),
                Margin = new Padding(0, 0, 6, 0),
            };
            tlp.Controls.Add(cardMoi, 0, 0);

            // Bottom controls — thêm TRƯỚC ────────────────────────────────────
            var pnlBtnLuu = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 48,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 8, 0, 0),
            };
            var btnLuuGhiChu = new Guna.UI2.WinForms.Guna2GradientButton
            {
                Dock = DockStyle.Right,
                Width = 140,
                Text = "💾  Lưu Ghi Chú",
                FillColor = ClrPrimary,
                FillColor2 = Color.SeaGreen,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
            };
            var btnXoaNhap = new Guna.UI2.WinForms.Guna2Button
            {
                Dock = DockStyle.Right,
                Width = 90,
                Text = "Xóa nhập",
                FillColor = Color.White,
                BorderColor = Color.FromArgb(209, 213, 219),
                BorderThickness = 1,
                ForeColor = ClrGray,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 9f),
                TextAlign = HorizontalAlignment.Center,
                Margin = new Padding(0, 0, 8, 0),
            };
            pnlBtnLuu.Controls.Add(btnLuuGhiChu);
            pnlBtnLuu.Controls.Add(btnXoaNhap);
            cardMoi.Controls.Add(pnlBtnLuu);

            var lblCount = new Label
            {
                Text = "0 / 2000 ký tự",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = ClrGray,
                Dock = DockStyle.Bottom,
                Height = 20,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight,
            };
            cardMoi.Controls.Add(lblCount);

            // Fill control — thêm SAU Bottom ──────────────────────────────────
            var txtGhiChu = new Guna.UI2.WinForms.Guna2TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                PlaceholderText = "Nhập ghi chú lâm sàng, nhận xét về ca khám, lưu ý điều trị...",
                BorderRadius = 10,
                BorderColor = Color.FromArgb(15, 92, 77),
                FillColor = Color.FromArgb(252, 255, 254),
                Font = new Font("Segoe UI", 9.5f),
                Margin = new Padding(0, 0, 0, 4),
            };
            cardMoi.Controls.Add(txtGhiChu);

            // Top — thêm SAU CÙNG ─────────────────────────────────────────────
            var pnlTitleMoi = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 0),
            };
            var sepMoi = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(229, 231, 235) };
            var lblTitleMoi = new Label
            {
                Text = "📝  Ghi Chú Mới",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = ClrPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Padding = new Padding(2, 0, 0, 0),
            };
            pnlTitleMoi.Controls.Add(sepMoi);
            pnlTitleMoi.Controls.Add(lblTitleMoi);
            cardMoi.Controls.Add(pnlTitleMoi);

            // ── Cột phải: lịch sử ────────────────────────────────────────────
            var cardLS = new Guna.UI2.WinForms.Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                BorderRadius = 12,
                Padding = new Padding(16, 0, 16, 12),
                Margin = new Padding(6, 0, 0, 0),
            };
            tlp.Controls.Add(cardLS, 1, 0);

            // Scroll panel (Fill — thêm TRƯỚC title) ──────────────────────────
            var flowLS = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0, 4, 0, 0),
            };
            cardLS.Controls.Add(flowLS);

            // Title lịch sử (Top — thêm SAU CÙNG) ────────────────────────────
            var pnlTitleLS = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 0),
            };
            var sepLS = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(229, 231, 235) };
            var lblTitleLS = new Label
            {
                Text = "🗒️  Lịch Sử Ghi Chú",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = ClrPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Padding = new Padding(2, 0, 0, 0),
            };
            pnlTitleLS.Controls.Add(sepLS);
            pnlTitleLS.Controls.Add(lblTitleLS);
            cardLS.Controls.Add(pnlTitleLS);

            // ── Events ───────────────────────────────────────────────────────
            txtGhiChu.TextChanged += (s, e) =>
            {
                int len = txtGhiChu.Text.Length;
                lblCount.Text = $"{len} / 2000 ký tự";
                lblCount.ForeColor = len > 1800 ? Color.FromArgb(185, 28, 28) : ClrGray;
                if (len > 2000) txtGhiChu.Text = txtGhiChu.Text.Substring(0, 2000);
            };

            btnXoaNhap.Click += (s, e) => txtGhiChu.Text = "";

            Action loadLichSu = null;
            loadLichSu = () =>
            {
                flowLS.Controls.Clear();
                DataTable dtGC = DatabaseConnection.ExecuteQuery(@"
                    SELECT gc.MaGhiChu, gc.NoiDung,
                           FORMAT(gc.NgayGhi,'HH:mm  dd/MM/yyyy') AS NgayGhiText,
                           nd.HoTen AS TenBacSi
                    FROM GhiChuKham gc
                    JOIN NguoiDung  nd ON gc.BacSiGhi = nd.MaNguoiDung
                    WHERE gc.MaPhieuKham = @MaPK
                    ORDER BY gc.NgayGhi DESC",
                    p => p.AddWithValue("@MaPK", _maPhieuKham));

                if (dtGC == null || dtGC.Rows.Count == 0)
                {
                    flowLS.Controls.Add(new Label
                    {
                        Text = "Chưa có ghi chú nào.",
                        Font = new Font("Segoe UI", 9.5f),
                        ForeColor = ClrGray,
                        AutoSize = true,
                        BackColor = Color.Transparent,
                        Margin = new Padding(0, 16, 0, 0),
                    });
                    return;
                }

                // Width cột phải = ClientSize của flowLS (sẽ được tính khi resize)
                flowLS.Layout += (s2, e2) =>
                {
                    int cardW = Math.Max(flowLS.ClientSize.Width - 4, 100);
                    int lblW = Math.Max(cardW - 44, 100);
                    foreach (Control ctrl in flowLS.Controls)
                    {
                        ctrl.Width = cardW;
                        if (!(ctrl is Panel card)) continue;

                        int lblH = 40; // chiều cao mặc định nếu không có label
                        foreach (Control child in card.Controls)
                        {
                            // Tính lại Height của lblND (nội dung ghi chú)
                            if (child is Label lbl
                                && lbl.Dock == DockStyle.Top
                                && !lbl.AutoSize)
                            {
                                using (var g = Graphics.FromHwnd(IntPtr.Zero))
                                {
                                    SizeF sz = g.MeasureString(lbl.Text, lbl.Font, lblW);
                                    lbl.Width = lblW;
                                    lbl.Height = (int)sz.Height + 10;
                                    lblH = lbl.Height;
                                }
                            }
                        }
                        // Card height = pnlHeader(24) + lblND(lblH) + padding top+bottom(18)
                        card.Height = 24 + lblH + 18;
                    }
                };

                foreach (DataRow row in dtGC.Rows)
                {
                    int maGC = Convert.ToInt32(row["MaGhiChu"]);
                    string noiDung = row["NoiDung"].ToString();
                    string ngay = row["NgayGhiText"].ToString();
                    string tenBS = row["TenBacSi"].ToString();

                    var itemCard = new Panel
                    {
                        Width = flowLS.ClientSize.Width - 4,
                        Height = 60,   // placeholder — Layout event sẽ tính lại đúng
                        BackColor = Color.FromArgb(249, 250, 251),
                        Margin = new Padding(0, 0, 0, 8),
                        Padding = new Padding(10, 8, 10, 10),
                        BorderStyle = BorderStyle.FixedSingle,
                        AutoSize = false,   // tắt AutoSize, tự quản lý Height
                    };

                    var pnlHeader = new Panel
                    {
                        Dock = DockStyle.Top,
                        Height = 24,
                        BackColor = Color.Transparent,
                    };
                    var lblHeader = new Label
                    {
                        Text = $"🩺 {tenBS}",
                        Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                        ForeColor = ClrPrimary,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleLeft,
                        BackColor = Color.Transparent,
                    };
                    var lblNgay = new Label
                    {
                        Text = ngay,
                        Font = new Font("Segoe UI", 8f),
                        ForeColor = ClrGray,
                        Dock = DockStyle.Right,
                        AutoSize = true,
                        TextAlign = ContentAlignment.MiddleRight,
                        BackColor = Color.Transparent,
                    };
                    var btnXoaGC = new Button
                    {
                        Text = "✕",
                        Dock = DockStyle.Right,
                        Width = 22,
                        Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                        ForeColor = Color.FromArgb(185, 28, 28),
                        BackColor = Color.FromArgb(254, 226, 226),
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Tag = maGC,
                        TextAlign = ContentAlignment.MiddleCenter,
                    };
                    btnXoaGC.FlatAppearance.BorderSize = 0;
                    btnXoaGC.Click += (s2, e2) =>
                    {
                        if (MessageBox.Show("Xóa ghi chú này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                        DatabaseConnection.ExecuteNonQuery(
                            "DELETE FROM GhiChuKham WHERE MaGhiChu=@MaGC",
                            p => p.AddWithValue("@MaGC", (int)((Button)s2).Tag));
                        loadLichSu();
                    };
                    pnlHeader.Controls.Add(btnXoaGC);
                    pnlHeader.Controls.Add(lblNgay);
                    pnlHeader.Controls.Add(lblHeader);

                    var lblND = new Label
                    {
                        Text = noiDung,
                        Font = new Font("Segoe UI", 9f),
                        ForeColor = Color.FromArgb(31, 41, 55),
                        Dock = DockStyle.Top,
                        BackColor = Color.Transparent,
                        AutoSize = false,
                        Padding = new Padding(0, 4, 0, 0),
                    };
                    // Tính chiều cao label theo nội dung
                    using (var g = Graphics.FromHwnd(IntPtr.Zero))
                    {
                        int w = Math.Max(flowLS.ClientSize.Width - 44, 100);
                        SizeF sz = g.MeasureString(noiDung, lblND.Font, w);
                        lblND.Height = (int)sz.Height + 10;
                        lblND.Width = w;
                    }

                    itemCard.Controls.Add(lblND);
                    itemCard.Controls.Add(pnlHeader);
                    flowLS.Controls.Add(itemCard);
                }
            };
            loadLichSu();

            btnLuuGhiChu.Click += (s, e) =>
            {
                string nd = txtGhiChu.Text.Trim();
                if (string.IsNullOrEmpty(nd))
                {
                    MessageBox.Show("Vui lòng nhập nội dung ghi chú.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DatabaseConnection.ExecuteNonQuery(
                    "INSERT INTO GhiChuKham(MaPhieuKham,NoiDung,NgayGhi,BacSiGhi) VALUES(@MaPK,@ND,GETDATE(),@MaBS)",
                    p =>
                    {
                        p.AddWithValue("@MaPK", _maPhieuKham);
                        p.AddWithValue("@ND", nd);
                        p.AddWithValue("@MaBS", _maBacSi);
                    });
                txtGhiChu.Text = "";
                loadLichSu();
                MessageBox.Show("✅ Đã lưu ghi chú.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
        }

        // ═════════════════════════════════════════════════════════════════════
        // PAINT EVENTS
        // ═════════════════════════════════════════════════════════════════════

        private void PnlTabs_Paint(object sender, PaintEventArgs e)
        {
            // Vẽ đường kẻ ngang dưới cùng pnlTabs
            using (var pen = new Pen(ClrBorder, 1f))
                e.Graphics.DrawLine(pen, 0, pnlTabs.Height - 1, pnlTabs.Width, pnlTabs.Height - 1);

            // Vẽ underline xanh dưới tab active
            if (_tabDangActive == null) return;
            using (var pen = new Pen(ClrPrimary, 3f))
            {
                int x = _tabDangActive.Left;
                int w = _tabDangActive.Width;
                int y = pnlTabs.Height - 3;
                e.Graphics.DrawLine(pen, x, y, x + w, y);
            }
        }

        // Paint events từ Designer — để trống
        private void pnlTabs_Paint(object sender, PaintEventArgs e) { }
        private void pnlContent_Paint(object sender, PaintEventArgs e) { }
        private void pnlSpace_Paint(object sender, PaintEventArgs e) { }

        // ═════════════════════════════════════════════════════════════════════
        // HELPERS
        // ═════════════════════════════════════════════════════════════════════

        /// <summary>Bo tròn lblAvatar thành hình tròn bằng custom Paint.</summary>
        private void BoTronAvatar()
        {
            lblAvatar.Paint += (s, pe) =>
            {
                var g = pe.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                int d = Math.Min(lblAvatar.Width, lblAvatar.Height);

                using (var brush = new SolidBrush(lblAvatar.BackColor))
                using (var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                })
                {
                    g.Clear(lblAvatar.Parent?.BackColor ?? Color.White);
                    g.FillEllipse(brush, 0, 0, d - 1, d - 1);
                    g.DrawString(lblAvatar.Text, lblAvatar.Font, Brushes.White,
                        new RectangleF(0, 0, d, d), sf);
                }
            };
        }

        /// <summary>Set màu header DataGridView theo theme xanh phòng khám.</summary>
        private void CoDinhHeaderDGV()
        {
            dgvDonThuoc.EnableHeadersVisualStyles = false;

            var hs = dgvDonThuoc.ColumnHeadersDefaultCellStyle;
            hs.BackColor = ClrPrimary;
            hs.ForeColor = Color.White;
            hs.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            hs.Alignment = DataGridViewContentAlignment.MiddleLeft;
            hs.Padding = new Padding(4, 0, 0, 0);

            dgvDonThuoc.DefaultCellStyle.Font = new Font("Segoe UI", 9f);

            // Style nút ✕ trong cột xóa
            if (dgvDonThuoc.Columns["colXoa"] is DataGridViewButtonColumn colXoa)
            {
                colXoa.DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226);
                colXoa.DefaultCellStyle.ForeColor = Color.FromArgb(185, 28, 28);
                colXoa.DefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                colXoa.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colXoa.DefaultCellStyle.SelectionBackColor = Color.FromArgb(254, 202, 202);
            }
        }

        private static string FormatTien(decimal so)
        {
            if (so >= 1_000_000)
                return (so / 1_000_000m).ToString("0.#") + "M";
            if (so >= 1_000)
                return (so / 1_000m).ToString("0") + "K";
            return so.ToString("N0") + "đ";
        }
    }
}
