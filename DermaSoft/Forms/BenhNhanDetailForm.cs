using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DermaSoft.Data;
using DermaSoft.Helpers;
using DermaSoft.Theme;

namespace DermaSoft.Forms
{
    public partial class BenhNhanDetailForm : Form
    {
        // ── State ─────────────────────────────────────────────────────────────
        private readonly int _maBenhNhan;
        private int _maPhieuKhamDangChon = -1;

        public BenhNhanDetailForm(int maBenhNhan)
        {
            InitializeComponent();
            _maBenhNhan = maBenhNhan;
            this.Load += BenhNhanDetailForm_Load;
        }

        // ══════════════════════════════════════════════════════════════════════
        // FORM LOAD
        // ══════════════════════════════════════════════════════════════════════
        private void BenhNhanDetailForm_Load(object sender, EventArgs e)
        {
            LoadThongTinBenhNhan();
            LoadLichSuKham();
            LoadStatCards();
        }

        // ══════════════════════════════════════════════════════════════════════
        // LEFT PANEL — Thông Tin Bệnh Nhân
        // ══════════════════════════════════════════════════════════════════════
        private void LoadThongTinBenhNhan()
        {
            try
            {
                const string sql = @"
                    SELECT
                        bn.HoTen,
                        bn.NgaySinh,
                        bn.GioiTinh,
                        bn.SoDienThoai,
                        bn.TienSuBenhLy,
                        tvi.DiemTichLuy,
                        tvi.DuongDanAvatar,
                        htv.TenHang,
                        htv.MauHangHex
                    FROM BenhNhan bn
                    LEFT JOIN ThanhVienInfo tvi ON bn.MaBenhNhan = tvi.MaBenhNhan
                    LEFT JOIN HangThanhVien htv ON tvi.MaHang    = htv.MaHang
                    WHERE bn.MaBenhNhan = @MaBN AND bn.IsDeleted = 0";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBN", _maBenhNhan));

                if (dt == null || dt.Rows.Count == 0) return;
                DataRow r = dt.Rows[0];

                lblTenBN.Text = r["HoTen"]?.ToString() ?? "—";

                string sdt = r["SoDienThoai"]?.ToString() ?? "—";
                string gt = r["GioiTinh"] != DBNull.Value
                    ? (Convert.ToBoolean(r["GioiTinh"]) ? "♂" : "♀") : "—";
                string tuoi = r["NgaySinh"] != DBNull.Value
                    ? (DateTime.Today.Year - Convert.ToDateTime(r["NgaySinh"]).Year) + " tuổi" : "—";
                lblThongTinCo.Text = $"{sdt} · {gt} · {tuoi}";

                if (r["TenHang"] != DBNull.Value)
                {
                    int diem = r["DiemTichLuy"] != DBNull.Value ? Convert.ToInt32(r["DiemTichLuy"]) : 0;
                    lblBadgeText.Text = $"⭐ {r["TenHang"]} · {diem:N0} điểm";
                    pnlBadge.Visible = true;
                    string hex = r["MauHangHex"]?.ToString() ?? "#0F5C4D";
                    try { pnlBadge.FillColor = ColorTranslator.FromHtml(hex); }
                    catch { pnlBadge.FillColor = ColorScheme.PrimaryDark; }
                }
                else
                {
                    pnlBadge.Visible = false;
                }

                string tienSu = r["TienSuBenhLy"]?.ToString();
                if (!string.IsNullOrWhiteSpace(tienSu))
                {
                    lblTienSuND.Text = tienSu;
                    pnlTienSu.Visible = true;
                }
                else
                {
                    pnlTienSu.Visible = false;
                }

                string avatar = r["DuongDanAvatar"]?.ToString();
                if (!string.IsNullOrEmpty(avatar) && File.Exists(avatar))
                {
                    Image img = ImageHelper.LoadImageSafe(avatar);
                    if (img != null)
                        ImageHelper.SetImage(picAvatar, img);
                    else
                        SetDefaultAvatar();
                }
                else
                {
                    SetDefaultAvatar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin bệnh nhân: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetDefaultAvatar()
        {
            string ten = lblTenBN.Text;
            string chu = ten.Length > 0
                ? ten.Substring(ten.LastIndexOf(' ') + 1, 1).ToUpper() : "?";
            var bmp = new Bitmap(80, 80);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(168, 230, 207));
                using (var f = new Font("Segoe UI", 30F, FontStyle.Bold))
                using (var br = new SolidBrush(ColorScheme.PrimaryDark))
                {
                    SizeF sz = g.MeasureString(chu, f);
                    g.DrawString(chu, f, br, (80 - sz.Width) / 2f, (80 - sz.Height) / 2f);
                }
            }
            picAvatar.Image?.Dispose();
            picAvatar.Image = bmp;
        }

        // ══════════════════════════════════════════════════════════════════════
        // LEFT PANEL — Lịch Sử Khám (TreeView)
        // ══════════════════════════════════════════════════════════════════════
        private void LoadLichSuKham()
        {
            try
            {
                const string sql = @"
                    SELECT pk.MaPhieuKham, pk.NgayKham, YEAR(pk.NgayKham) AS Nam
                    FROM PhieuKham pk
                    WHERE pk.MaBenhNhan = @MaBN AND pk.IsDeleted = 0
                      AND pk.TrangThai >= 2
                    ORDER BY pk.NgayKham DESC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBN", _maBenhNhan));

                trvLichSu.BeginUpdate();
                trvLichSu.Nodes.Clear();

                if (dt == null || dt.Rows.Count == 0)
                {
                    trvLichSu.Nodes.Add(new TreeNode("(Chưa có lịch khám)")
                    { ForeColor = ColorScheme.TextGray });
                    trvLichSu.EndUpdate();
                    return;
                }

                int namHienTai = -1;
                TreeNode nodNam = null;
                int dem = 0;

                foreach (DataRow row in dt.Rows)
                {
                    int nam = Convert.ToInt32(row["Nam"]);
                    int maPK = Convert.ToInt32(row["MaPhieuKham"]);
                    DateTime ngay = Convert.ToDateTime(row["NgayKham"]);

                    if (nam != namHienTai)
                    {
                        if (nodNam != null)
                            nodNam.Text = $"{namHienTai} ({dem} lần)";
                        namHienTai = nam;
                        dem = 0;
                        nodNam = new TreeNode(nam.ToString());
                        trvLichSu.Nodes.Add(nodNam);
                    }
                    dem++;

                    var nodPK = new TreeNode($"PK#{maPK:D3} — {ngay:dd/MM}") { Tag = maPK };
                    nodNam?.Nodes.Add(nodPK);
                }
                if (nodNam != null)
                    nodNam.Text = $"{namHienTai} ({dem} lần)";

                if (trvLichSu.Nodes.Count > 0)
                {
                    trvLichSu.Nodes[0].Expand();
                    if (trvLichSu.Nodes[0].Nodes.Count > 0)
                        trvLichSu.SelectedNode = trvLichSu.Nodes[0].Nodes[0];
                }
                trvLichSu.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử khám: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // [FIX] TreeView OwnerDraw — node được chọn màu (15,92,77), chữ trắng đậm
        // ══════════════════════════════════════════════════════════════════════
        private void trvLichSu_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node == null) return;

            bool isLeaf = e.Node.Tag is int;           // node phiếu khám
            bool isSelected = (e.State & TreeNodeStates.Selected) != 0
                           || (e.State & TreeNodeStates.Focused) != 0;

            // Vẽ background full-width (FullRowSelect yêu cầu vẽ đủ chiều ngang)
            var fullRow = new Rectangle(0, e.Bounds.Y, trvLichSu.Width, e.Bounds.Height);

            if (isLeaf && isSelected)
            {
                // Node phiếu khám được chọn — nền (15,92,77), chữ trắng đậm
                using (var br = new SolidBrush(ColorScheme.PrimaryDark))
                    e.Graphics.FillRectangle(br, fullRow);

                using (var f = new Font(trvLichSu.Font, FontStyle.Bold))
                    TextRenderer.DrawText(e.Graphics, e.Node.Text, f, e.Bounds,
                        Color.White,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
            else if (isLeaf)
            {
                // Node phiếu khám bình thường
                e.Graphics.FillRectangle(Brushes.White, fullRow);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, trvLichSu.Font, e.Bounds,
                    ColorScheme.TextDark,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
            else
            {
                // Node năm (parent) — nền trắng, chữ xanh đậm bold
                e.Graphics.FillRectangle(Brushes.White, fullRow);
                using (var f = new Font(trvLichSu.Font, FontStyle.Bold))
                    TextRenderer.DrawText(e.Graphics, e.Node.Text, f, e.Bounds,
                        ColorScheme.PrimaryDark,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }

            e.DrawDefault = false;
        }

        // ══════════════════════════════════════════════════════════════════════
        // TAB 1 — Thông Tin Khám
        // ══════════════════════════════════════════════════════════════════════
        private void LoadThongTinKham(int maPhieuKham)
        {
            _maPhieuKhamDangChon = maPhieuKham;
            LoadCardPhieuKham(maPhieuKham);
            LoadDonThuocLanNay(maPhieuKham);
        }

        private void LoadCardPhieuKham(int maPhieuKham)
        {
            try
            {
                const string sql = @"
                    SELECT pk.MaPhieuKham, pk.NgayKham, pk.TrieuChung, pk.ChanDoan,
                           pk.NgayTaiKham, pk.TrangThai, nd.HoTen AS TenBacSi
                    FROM PhieuKham pk
                    JOIN NguoiDung nd ON pk.MaNguoiDung = nd.MaNguoiDung
                    WHERE pk.MaPhieuKham = @MaPK AND pk.IsDeleted = 0";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaPK", maPhieuKham));

                if (dt == null || dt.Rows.Count == 0) return;
                DataRow r = dt.Rows[0];

                DateTime ngayKham = Convert.ToDateTime(r["NgayKham"]);
                lblExamTitle.Text = $"📋  Phiếu Khám PK#{maPhieuKham:D3} — {ngayKham:dd/MM/yyyy}";
                lblDoctor.Text = $"Bác sĩ:          BS. {r["TenBacSi"]}";

                string tc = r["TrieuChung"]?.ToString();
                lblSymptom.Text = "Triệu chứng:  " + (string.IsNullOrWhiteSpace(tc) ? "—" : tc);

                string cd = r["ChanDoan"]?.ToString();
                lblDiagnosis.Text = "Chẩn đoán:    " + (string.IsNullOrWhiteSpace(cd) ? "—" : cd);

                lblReExam.Text = "Tái khám:       " +
                    (r["NgayTaiKham"] != DBNull.Value
                        ? Convert.ToDateTime(r["NgayTaiKham"]).ToString("dd/MM/yyyy") : "—");

                byte tt = Convert.ToByte(r["TrangThai"]);
                lblStatus.Text = "Trạng thái:     " + GetTrangThaiLabel(tt);
                lblStatus.ForeColor = GetTrangThaiColor(tt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải phiếu khám: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadDonThuocLanNay(int maPhieuKham)
        {
            try
            {
                const string sql = @"
                    SELECT t.TenThuoc, ct.SoLuong, ct.LieuDung
                    FROM ChiTietDonThuoc ct
                    JOIN Thuoc t ON ct.MaThuoc = t.MaThuoc
                    WHERE ct.MaPhieuKham = @MaPK
                    ORDER BY t.TenThuoc";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaPK", maPhieuKham));
                dgvPrescription.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải đơn thuốc: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadStatCards()
        {
            try
            {
                // Chỉ đếm phiếu khám đã thanh toán (TrangThai = 3)
                const string sql = @"
                    SELECT
                        COUNT(DISTINCT pk.MaPhieuKham) AS TongLanKham,
                        COUNT(DISTINCT ctd.MaDichVu)   AS SoLoaiDichVu,
                        COUNT(DISTINCT ctt.MaThuoc)    AS SoLoaiThuoc,
                        tvi.TyLeHaiLong
                    FROM BenhNhan bn
                    LEFT JOIN PhieuKham pk   ON bn.MaBenhNhan = pk.MaBenhNhan
                                             AND pk.IsDeleted = 0
                                             AND pk.TrangThai = 3
                    LEFT JOIN ChiTietDichVu ctd ON pk.MaPhieuKham = ctd.MaPhieuKham
                    LEFT JOIN ChiTietDonThuoc ctt ON pk.MaPhieuKham = ctt.MaPhieuKham
                    LEFT JOIN ThanhVienInfo tvi ON bn.MaBenhNhan = tvi.MaBenhNhan
                    WHERE bn.MaBenhNhan = @MaBN
                    GROUP BY tvi.TyLeHaiLong";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBN", _maBenhNhan));

                if (dt == null || dt.Rows.Count == 0) return;
                DataRow r = dt.Rows[0];

                lblStatValue1.Text = r["TongLanKham"].ToString();
                lblStatValue2.Text = r["SoLoaiDichVu"].ToString();
                lblStatValue3.Text = r["SoLoaiThuoc"].ToString();
                lblStatValue4.Text = r["TyLeHaiLong"] != DBNull.Value
                    ? $"{Convert.ToInt32(r["TyLeHaiLong"])}%" : "—";
            }
            catch
            {
                lblStatValue1.Text = "—";
                lblStatValue2.Text = "—";
                lblStatValue3.Text = "—";
                lblStatValue4.Text = "—";
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // TAB 2 — Đơn Thuốc toàn bộ
        // ══════════════════════════════════════════════════════════════════════
        private void LoadDonThuocFull()
        {
            try
            {
                const string sql = @"
                    SELECT CONVERT(VARCHAR, pk.NgayKham, 103) AS NgayKham,
                           t.TenThuoc, ct.SoLuong, ct.LieuDung
                    FROM ChiTietDonThuoc ct
                    JOIN Thuoc    t  ON ct.MaThuoc     = t.MaThuoc
                    JOIN PhieuKham pk ON ct.MaPhieuKham = pk.MaPhieuKham
                    WHERE pk.MaBenhNhan = @MaBN AND pk.IsDeleted = 0
                    ORDER BY pk.NgayKham DESC, t.TenThuoc";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBN", _maBenhNhan));
                dgvDonThuocFull.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử đơn thuốc: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // TAB 3 — Hình Ảnh Bệnh Lý
        // ══════════════════════════════════════════════════════════════════════
        private void LoadHinhAnh()
        {
            try
            {
                const string sql = @"
                    SELECT ha.MaHinhAnh, ha.DuongDanAnh, ha.GhiChu, ha.NgayChup, pk.MaPhieuKham
                    FROM HinhAnhBenhLy ha
                    JOIN PhieuKham pk ON ha.MaPhieuKham = pk.MaPhieuKham
                    WHERE pk.MaBenhNhan = @MaBN AND pk.IsDeleted = 0
                    ORDER BY ha.NgayChup DESC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBN", _maBenhNhan));

                ImageHelper.DisposeControlImages(flpHinhAnh);
                flpHinhAnh.Controls.Clear();

                if (dt == null || dt.Rows.Count == 0)
                {
                    lblHinhAnhEmpty.Visible = true;
                    flpHinhAnh.Visible = false;
                    return;
                }

                lblHinhAnhEmpty.Visible = false;
                flpHinhAnh.Visible = true;

                foreach (DataRow row in dt.Rows)
                {
                    string duongDan = row["DuongDanAnh"]?.ToString() ?? "";
                    string ghiChu = row["GhiChu"]?.ToString() ?? "";
                    DateTime ngay = row["NgayChup"] != DBNull.Value
                        ? Convert.ToDateTime(row["NgayChup"]) : DateTime.MinValue;
                    int maPK = Convert.ToInt32(row["MaPhieuKham"]);

                    var card = new Panel { Size = new Size(160, 200), Margin = new Padding(8), BackColor = Color.White, Cursor = Cursors.Hand };
                    var pic = new PictureBox { Size = new Size(160, 140), Location = new Point(0, 0), SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.FromArgb(240, 250, 245) };

                    if (!string.IsNullOrEmpty(duongDan) && File.Exists(duongDan))
                    {
                        Image img = ImageHelper.LoadImageSafe(duongDan);
                        if (img != null) pic.Image = img;
                    }

                    var lblInfo = new Label
                    {
                        Size = new Size(160, 56),
                        Location = new Point(0, 140),
                        Font = new Font("Segoe UI", 7.5F),
                        ForeColor = ColorScheme.TextGray,
                        BackColor = Color.White,
                        Padding = new Padding(4, 2, 4, 0),
                        Text = (string.IsNullOrWhiteSpace(ghiChu) ? $"PK#{maPK:D3}" : ghiChu)
                                    + (ngay != DateTime.MinValue ? $"\n{ngay:dd/MM/yyyy}" : "")
                    };

                    string captured = duongDan;
                    pic.Click += (s, ev) => MoAnhLon(captured);
                    card.Click += (s, ev) => MoAnhLon(captured);
                    card.Controls.Add(pic);
                    card.Controls.Add(lblInfo);
                    flpHinhAnh.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải hình ảnh: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MoAnhLon(string duongDan)
        {
            if (string.IsNullOrEmpty(duongDan) || !File.Exists(duongDan)) return;
            Image img = ImageHelper.LoadImageSafe(duongDan);
            if (img == null) return;
            var frm = new Form { Text = "Hình Ảnh Bệnh Lý", StartPosition = FormStartPosition.CenterScreen, Size = new Size(800, 600), BackColor = Color.Black };
            var pic = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.Black };
            pic.Image = img;
            frm.Controls.Add(pic);
            frm.ShowDialog(this);
            pic.Image?.Dispose();
        }

        // ══════════════════════════════════════════════════════════════════════
        // TAB 4 — Ghi Chú
        // ══════════════════════════════════════════════════════════════════════
        private void LoadGhiChu()
        {
            try
            {
                const string sql = @"
                    SELECT CONVERT(VARCHAR, pk.NgayKham, 103) AS NgayKham, pk.GhiChu
                    FROM PhieuKham pk
                    WHERE pk.MaBenhNhan = @MaBN AND pk.IsDeleted = 0
                      AND pk.GhiChu IS NOT NULL AND pk.GhiChu <> ''
                    ORDER BY pk.NgayKham DESC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBN", _maBenhNhan));
                dgvGhiChu.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải ghi chú: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // EVENT HANDLERS — TreeView
        // ══════════════════════════════════════════════════════════════════════
        private void trvLichSu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is int maPK)
            {
                LoadThongTinKham(maPK);
                SwitchTab(0);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // EVENT HANDLERS — Tab Buttons
        // ══════════════════════════════════════════════════════════════════════
        private void btnTabThongTin_Click(object sender, EventArgs e) => SwitchTab(0);
        private void btnTabDonThuoc_Click(object sender, EventArgs e) => SwitchTab(1);
        private void btnTabHinhAnh_Click(object sender, EventArgs e) => SwitchTab(2);
        private void btnTabGhiChu_Click(object sender, EventArgs e) => SwitchTab(3);

        // ══════════════════════════════════════════════════════════════════════
        // TAB SWITCHING
        // ══════════════════════════════════════════════════════════════════════
        private int _tabHienTai = 0;

        private void SwitchTab(int tabIndex)
        {
            if (tabIndex == _tabHienTai) return;
            _tabHienTai = tabIndex;

            pnlThongTinFull.Visible = false;
            pnlDonThuocFull.Visible = false;
            pnlHinhAnhFull.Visible = false;
            pnlGhiChuFull.Visible = false;

            SetTabIdle(btnTabThongTin);
            SetTabIdle(btnTabDonThuoc);
            SetTabIdle(btnTabHinhAnh);
            SetTabIdle(btnTabGhiChu);

            switch (tabIndex)
            {
                case 0:
                    SetTabActive(btnTabThongTin);
                    pnlThongTinFull.Visible = true;
                    break;
                case 1:
                    SetTabActive(btnTabDonThuoc);
                    pnlDonThuocFull.Visible = true;
                    LoadDonThuocFull();
                    break;
                case 2:
                    SetTabActive(btnTabHinhAnh);
                    pnlHinhAnhFull.Visible = true;
                    LoadHinhAnh();
                    break;
                case 3:
                    SetTabActive(btnTabGhiChu);
                    pnlGhiChuFull.Visible = true;
                    LoadGhiChu();
                    break;
            }
        }

        private void SetTabActive(Guna.UI2.WinForms.Guna2Button btn)
        {
            btn.FillColor = ColorScheme.PrimaryDark;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        private void SetTabIdle(Guna.UI2.WinForms.Guna2Button btn)
        {
            btn.FillColor = Color.Transparent;
            btn.ForeColor = ColorScheme.TextGray;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        }

        // ══════════════════════════════════════════════════════════════════════
        // HELPER — TrangThai
        // ══════════════════════════════════════════════════════════════════════
        private string GetTrangThaiLabel(byte tt)
        {
            switch (tt)
            {
                case 0: return "🔵 Mới";
                case 1: return "🟡 Đang khám";
                case 2: return "🟢 Hoàn thành";
                case 3: return "✅ Đã thanh toán";
                case 4: return "❌ Đã hủy";
                default: return "—";
            }
        }

        private Color GetTrangThaiColor(byte tt)
        {
            switch (tt)
            {
                case 0: return ColorScheme.Info;
                case 1: return ColorScheme.Warning;
                case 2: return ColorScheme.Success;
                case 3: return ColorScheme.PrimaryDark;
                case 4: return ColorScheme.Danger;
                default: return ColorScheme.TextGray;
            }
        }
    }
}
