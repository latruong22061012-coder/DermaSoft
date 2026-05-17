using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DermaSoft.Data;

namespace DermaSoft.Forms
{
    public partial class MemberForm : Form
    {
        // ══════════════════════════════════════════════════════════════════════
        // STATE
        // ══════════════════════════════════════════════════════════════════════
        private int _maBNDangChon = -1;
        private bool _daCoThe = false;
        private bool _daKhoa = false;
        private string _tuKhoa = "";

        // ── Màu badge ──────────────────────────────────────────────────────
        private static readonly Color ClrDo = Color.FromArgb(255, 76, 76);
        private static readonly Color ClrBac = Color.FromArgb(192, 192, 192);
        private static readonly Color ClrVang = Color.FromArgb(255, 215, 0);
        private static readonly Color ClrKimCuong = Color.FromArgb(137, 207, 240);
        private static readonly Color ClrChuaThe = Color.FromArgb(209, 213, 219);
        private static readonly Color ClrTextDark = Color.FromArgb(55, 65, 81);

        // ══════════════════════════════════════════════════════════════════════
        // KHỞI TẠO
        // ══════════════════════════════════════════════════════════════════════
        public MemberForm()
        {
            InitializeComponent();
            this.Load += MemberForm_Load;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            cboFilterHang.SelectedIndexChanged += CboFilterHang_SelectedIndexChanged;
            dgvBenhNhan.CellClick += DgvBenhNhan_CellClick;
            btnDangKyThe.Click += BtnDangKyThe_Click;
            btnHuyThe.Click += BtnHuyThe_Click;
        }

        private void MemberForm_Load(object sender, EventArgs e)
        {
            // Tự động đảm bảo schema DaKhoa/NgayKhoa tồn tại (idempotent)
            // → Không cần chạy migration SQL thủ công, tránh grid rỗng do
            //   lỗi "Invalid column name 'DaKhoa'" khi SELECT.
            DamBaoColumnKhoaThe();

            cboFilterHang.SelectedIndex = 0;
            CaiDatCot();
            DatTierCardMacDinh();
            HienThiTrangThaiChuaChon();
            LoadDanhSachThanhVien();
        }

        /// <summary>
        /// Tự động thêm cột DaKhoa / NgayKhoa vào ThanhVienInfo nếu chưa có.
        /// Đảm bảo chức năng Khóa Thẻ hoạt động không cần chạy migration thủ công.
        /// Chạy một lần duy nhất, an toàn khi gọi nhiều lần (idempotent).
        /// </summary>
        private void DamBaoColumnKhoaThe()
        {
            try
            {
                DatabaseConnection.ExecuteNonQuery(@"
                    IF NOT EXISTS (SELECT 1 FROM sys.columns
                                   WHERE object_id = OBJECT_ID('ThanhVienInfo')
                                     AND name = 'DaKhoa')
                        ALTER TABLE ThanhVienInfo ADD DaKhoa BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT 1 FROM sys.columns
                                   WHERE object_id = OBJECT_ID('ThanhVienInfo')
                                     AND name = 'NgayKhoa')
                        ALTER TABLE ThanhVienInfo ADD NgayKhoa DATETIME NULL;");
            }
            catch
            {
                // Bỏ qua — nếu không có quyền ALTER TABLE, query SELECT bên dưới
                // vẫn có thể fail, nhưng ít nhất đã cố gắng auto-migrate.
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // CẤU HÌNH CỘT
        // ══════════════════════════════════════════════════════════════════════
        private void CaiDatCot()
        {
            if (dgvBenhNhan.Columns["MaBenhNhan"] != null)
                dgvBenhNhan.Columns["MaBenhNhan"].Visible = false;
            if (dgvBenhNhan.Columns["MaThanhVien"] != null)
                dgvBenhNhan.Columns["MaThanhVien"].Visible = false;
            if (dgvBenhNhan.Columns["MaHang"] != null)
                dgvBenhNhan.Columns["MaHang"].Visible = false;
        }

        // ══════════════════════════════════════════════════════════════════════
        // LOAD DANH SÁCH — LEFT JOIN để hiện cả BN chưa có thẻ
        // ══════════════════════════════════════════════════════════════════════
        private void LoadDanhSachThanhVien()
        {
            try
            {
                string sql = @"
                    SELECT
                        bn.MaBenhNhan,
                        tvi.MaThanhVien,
                        tvi.MaHang,
                        ISNULL(tvi.DaKhoa, 0)                                   AS DaKhoa,
                        bn.HoTen,
                        ISNULL(htv.TenHang,  N'—')                              AS HangTV,
                        ISNULL(tvi.DiemTichLuy, 0)                              AS DiemTichLuy,
                        ISNULL(tvi.SoLanKham,   0)                              AS SoLanKham,
                        ISNULL(CAST(tvi.TyLeHaiLong AS INT), 0)                 AS TyLeHaiLong,
                        ISNULL(FORMAT(tvi.NgayTaoTaiKhoan, 'MM/yyyy'), N'—')    AS NgayTaoTV,
                        CASE
                             WHEN tvi.MaThanhVien IS NULL        THEN N'Chưa có thẻ'
                             WHEN ISNULL(tvi.DaKhoa, 0) = 1      THEN N'Đã khóa'
                             ELSE                                     N'Có thẻ'
                        END                                                      AS TrangThaiThe
                    FROM BenhNhan        bn
                    LEFT JOIN ThanhVienInfo   tvi ON bn.MaBenhNhan = tvi.MaBenhNhan
                    LEFT JOIN HangThanhVien   htv ON tvi.MaHang    = htv.MaHang
                    WHERE bn.IsDeleted = 0";

                // ── Filter tìm kiếm ──
                if (!string.IsNullOrWhiteSpace(_tuKhoa))
                    sql += @" AND (bn.HoTen       LIKE @TuKhoa
                               OR bn.SoDienThoai LIKE @TuKhoa)";

                // ── Filter hạng ──
                int idx = cboFilterHang.SelectedIndex;
                switch (idx)
                {
                    case 1: sql += " AND htv.TenHang = N'Thành Viên Đỏ'"; break;
                    case 2: sql += " AND htv.TenHang = N'Thành Viên Bạc'"; break;
                    case 3: sql += " AND htv.TenHang = N'Thành Viên Vàng'"; break;
                    case 4: sql += " AND htv.TenHang = N'Thành Viên Kim Cương'"; break;
                    case 5: sql += " AND tvi.MaThanhVien IS NULL"; break; // Chưa có thẻ
                }

                sql += " ORDER BY bn.HoTen ASC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql, p =>
                {
                    if (!string.IsNullOrWhiteSpace(_tuKhoa))
                        p.AddWithValue("@TuKhoa", "%" + _tuKhoa.Trim() + "%");
                });

                dgvBenhNhan.AutoGenerateColumns = false;
                dgvBenhNhan.DataSource = dt;

                TomauHangTV();
                TomauTrangThaiThe();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // TÔ MÀU BADGE CỘT HẠNG TV
        // ══════════════════════════════════════════════════════════════════════
        private void TomauHangTV()
        {
            foreach (DataGridViewRow row in dgvBenhNhan.Rows)
            {
                if (row.IsNewRow) continue;
                string hang = row.Cells["colHangTV"].Value?.ToString() ?? "";
                Color bg = LayMauHang(hang);
                if (bg == Color.Empty) continue;

                var style = row.Cells["colHangTV"].Style;
                style.BackColor = bg;
                style.ForeColor = ClrTextDark;
                style.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                style.SelectionBackColor = bg;
                style.SelectionForeColor = ClrTextDark;
            }
        }

        // ── Tô màu cột Thẻ TV ──────────────────────────────────────────────
        private void TomauTrangThaiThe()
        {
            foreach (DataGridViewRow row in dgvBenhNhan.Rows)
            {
                if (row.IsNewRow) continue;
                string tt = row.Cells["colTrangThaiThe"].Value?.ToString() ?? "";

                var style = row.Cells["colTrangThaiThe"].Style;
                style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                style.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);

                if (tt == "Có thẻ")
                {
                    style.BackColor = Color.FromArgb(220, 252, 231); // xanh nhạt
                    style.ForeColor = Color.FromArgb(21, 101, 52);
                    style.SelectionBackColor = Color.FromArgb(220, 252, 231);
                    style.SelectionForeColor = Color.FromArgb(21, 101, 52);
                }
                else if (tt == "Đã khóa")
                {
                    style.BackColor = Color.FromArgb(254, 226, 226); // đỏ nhạt
                    style.ForeColor = Color.FromArgb(185, 28, 28);
                    style.SelectionBackColor = Color.FromArgb(254, 226, 226);
                    style.SelectionForeColor = Color.FromArgb(185, 28, 28);
                }
                else
                {
                    style.BackColor = ClrChuaThe;
                    style.ForeColor = Color.FromArgb(107, 114, 128);
                    style.SelectionBackColor = ClrChuaThe;
                    style.SelectionForeColor = Color.FromArgb(107, 114, 128);
                }
            }
        }

        private Color LayMauHang(string tenHang)
        {
            switch (tenHang)
            {
                case "Thành Viên Đỏ": return ClrDo;
                case "Thành Viên Bạc": return ClrBac;
                case "Thành Viên Vàng": return ClrVang;
                case "Thành Viên Kim Cương": return ClrKimCuong;
                default: return Color.Empty;
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // CLICK HÀNG TRONG BẢNG
        // ══════════════════════════════════════════════════════════════════════
        private void DgvBenhNhan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataTable dt = (DataTable)dgvBenhNhan.DataSource;
            if (dt == null || e.RowIndex >= dt.Rows.Count) return;

            DataRow row = dt.Rows[e.RowIndex];
            _maBNDangChon = Convert.ToInt32(row["MaBenhNhan"]);
            _daCoThe = row["MaThanhVien"] != DBNull.Value;
            _daKhoa = _daCoThe && row.Table.Columns.Contains("DaKhoa")
                      && row["DaKhoa"] != DBNull.Value
                      && Convert.ToBoolean(row["DaKhoa"]);

            if (_daCoThe)
            {
                // Có thẻ → hiện thông tin chi tiết, disable đăng ký, enable khóa/mở khóa
                LoadChiTietThanhVien(_maBNDangChon);
                btnDangKyThe.Enabled = false;
                btnHuyThe.Enabled = true;
                CapNhatNutKhoaThe();
            }
            else
            {
                // Chưa có thẻ → hiện trạng thái chờ đăng ký, enable đăng ký
                HienThiTrangThaiChuaThe(row["HoTen"]?.ToString() ?? "");
                btnDangKyThe.Enabled = true;
                btnHuyThe.Enabled = false;
                btnHuyThe.Text = "🔒 Khóa Thẻ";
            }
        }

        /// <summary>Cập nhật text + màu nút dựa trên trạng thái khóa hiện tại.</summary>
        private void CapNhatNutKhoaThe()
        {
            if (_daKhoa)
            {
                btnHuyThe.Text = "🔓 Mở Khóa Thẻ";
                btnHuyThe.FillColor = Color.FromArgb(21, 101, 52);     // Xanh đậm
                btnHuyThe.FillColor2 = Color.FromArgb(34, 197, 94);    // Xanh lá
            }
            else
            {
                btnHuyThe.Text = "🔒 Khóa Thẻ";
                btnHuyThe.FillColor = Color.Maroon;
                btnHuyThe.FillColor2 = Color.OrangeRed;
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // LOAD CHI TIẾT THÀNH VIÊN
        // ══════════════════════════════════════════════════════════════════════
        private void LoadChiTietThanhVien(int maBN)
        {
            try
            {
                const string sql = @"
                    SELECT
                        bn.MaBenhNhan,
                        bn.HoTen,
                        bn.SoDienThoai,
                        'BN' + RIGHT('000' + CAST(bn.MaBenhNhan AS VARCHAR), 3) AS MaBNCode,
                        tvi.MaThanhVien,
                        tvi.MaHang,
                        tvi.DiemTichLuy,
                        tvi.SoLanKham,
                        CAST(tvi.TyLeHaiLong AS INT)                            AS TyLeHaiLong,
                        htv.TenHang,
                        htv.MauHangHex,
                        htv_next.TenHang      AS TenHangTiep,
                        htv_next.DiemToiThieu AS DiemHangTiep
                    FROM ThanhVienInfo tvi
                    JOIN BenhNhan       bn       ON tvi.MaBenhNhan = bn.MaBenhNhan
                    JOIN HangThanhVien  htv      ON tvi.MaHang     = htv.MaHang
                    LEFT JOIN HangThanhVien htv_next
                        ON htv_next.MaHang = (
                            SELECT MIN(h2.MaHang)
                            FROM HangThanhVien h2
                            WHERE h2.DiemToiThieu > htv.DiemToiThieu)
                    WHERE bn.MaBenhNhan = @MaBN AND bn.IsDeleted = 0";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaBN", maBN));

                if (dt == null || dt.Rows.Count == 0) return;
                DataRow r = dt.Rows[0];

                // ── Member Card ────────────────────────────────────────────
                string tenHang = r["TenHang"]?.ToString() ?? "";
                string mauHex = r["MauHangHex"]?.ToString() ?? "#0F5C4D";
                string tenBN = r["HoTen"]?.ToString() ?? "";
                string sdt = r["SoDienThoai"]?.ToString() ?? "—";
                string maBNCode = r["MaBNCode"]?.ToString() ?? "";
                int diem = Convert.ToInt32(r["DiemTichLuy"]);
                int soLanKham = Convert.ToInt32(r["SoLanKham"]);
                int tyLeHL = r["TyLeHaiLong"] != DBNull.Value
                                   ? Convert.ToInt32(r["TyLeHaiLong"]) : 0;

                lblHangBadge.Text = $"🏅  {tenHang.ToUpper()}";
                lblTenBN.Text = tenBN;
                lblSDTMaBN.Text = $"📞 {sdt}  ·  {maBNCode}";
                lblDiemValue.Text = $"{diem:N0} pts";

                // Màu gradient card theo hạng
                try
                {
                    Color c = ColorTranslator.FromHtml(mauHex);
                    pnlMemberCard.FillColor = ControlPaint.Dark(c, 0.3f);
                    pnlMemberCard.FillColor2 = c;
                }
                catch { }

                // ── Tiến độ lên hạng ──────────────────────────────────────
                if (r["TenHangTiep"] != DBNull.Value)
                {
                    int diemHangTiep = Convert.ToInt32(r["DiemHangTiep"]);
                    int conLai = diemHangTiep - diem;
                    int phanTram = diem > 0
                        ? Math.Min(100, diem * 100 / diemHangTiep) : 0;

                    lblConLai.Text = conLai > 0
                        ? $"Còn {conLai:N0} điểm để lên {r["TenHangTiep"]}"
                        : $"Đủ điều kiện lên {r["TenHangTiep"]}!";
                    pbTienDo.Value = phanTram;

                    // Panel thống kê tiến độ
                    lblTienDoSo.Text = $"{diem:N0} / {diemHangTiep:N0} điểm";
                    pbTienDoHang.Value = phanTram;
                }
                else
                {
                    lblConLai.Text = "🏆  Đã đạt hạng cao nhất!";
                    pbTienDo.Value = 100;
                    lblTienDoSo.Text = "Kim Cương — Hạng cao nhất";
                    pbTienDoHang.Value = 100;
                }

                // ── Stat cards ────────────────────────────────────────────
                lblStatKhamVal.Text = $"{soLanKham}";
                lblStatHaiLongVal.Text = $"{tyLeHL}%";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // HIỆN TRẠNG THÁI KHI CHƯA CHỌN AI
        // ══════════════════════════════════════════════════════════════════════
        private void HienThiTrangThaiChuaChon()
        {
            _maBNDangChon = -1;
            _daCoThe = false;
            lblHangBadge.Text = "—";
            lblTenBN.Text = "Chọn một thành viên";
            lblSDTMaBN.Text = "để xem chi tiết";
            lblDiemValue.Text = "— pts";
            lblConLai.Text = "—";
            pbTienDo.Value = 0;
            lblStatKhamVal.Text = "—";
            lblStatHaiLongVal.Text = "—";
            lblTienDoSo.Text = "—";
            pbTienDoHang.Value = 0;
            pnlMemberCard.FillColor = Color.FromArgb(15, 92, 77);
            pnlMemberCard.FillColor2 = Color.SeaGreen;
            btnDangKyThe.Enabled = false;
            btnHuyThe.Enabled = false;
        }

        // ── Hiện trạng thái BN chưa có thẻ được chọn ──────────────────────
        private void HienThiTrangThaiChuaThe(string tenBN)
        {
            lblHangBadge.Text = "Chưa có thẻ thành viên";
            lblTenBN.Text = tenBN;
            lblSDTMaBN.Text = "Nhấn Đăng Ký để tạo thẻ mới";
            lblDiemValue.Text = "0 pts";
            lblConLai.Text = "—";
            pbTienDo.Value = 0;
            lblStatKhamVal.Text = "0";
            lblStatHaiLongVal.Text = "0%";
            lblTienDoSo.Text = "—";
            pbTienDoHang.Value = 0;
            pnlMemberCard.FillColor = Color.FromArgb(100, 116, 139); // xám
            pnlMemberCard.FillColor2 = Color.FromArgb(148, 163, 184);
        }

        // ══════════════════════════════════════════════════════════════════════
        // ĐĂNG KÝ THẺ THÀNH VIÊN MỚI
        // ══════════════════════════════════════════════════════════════════════
        private void BtnDangKyThe_Click(object sender, EventArgs e)
        {
            if (_maBNDangChon <= 0 || _daCoThe) return;

            string tenBN = lblTenBN.Text;
            var xacNhan = MessageBox.Show(
                $"Đăng ký thẻ thành viên cho \"{tenBN}\"?\n\nHạng khởi đầu: Thành Viên Đỏ — 0 điểm",
                "Xác nhận đăng ký thẻ",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes) return;

            try
            {
                // Lấy MaHang của "Thành Viên Đỏ" (hạng thấp nhất)
                object maHangDo = DatabaseConnection.ExecuteScalar(
                    "SELECT TOP 1 MaHang FROM HangThanhVien ORDER BY DiemToiThieu ASC");

                if (maHangDo == null)
                {
                    MessageBox.Show("Không tìm thấy hạng thành viên trong hệ thống.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DatabaseConnection.ExecuteNonQuery(@"
                    INSERT INTO ThanhVienInfo
                        (MaBenhNhan, MaHang, DiemTichLuy, SoLanKham,
                         TyLeHaiLong, NgayTaoTaiKhoan)
                    VALUES
                        (@MaBN, @MaHang, 0, 0, 0, GETDATE())",
                    p =>
                    {
                        p.AddWithValue("@MaBN", _maBNDangChon);
                        p.AddWithValue("@MaHang", Convert.ToInt32(maHangDo));
                    });

                MessageBox.Show($"✅ Đã đăng ký thẻ thành viên cho \"{tenBN}\" thành công!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reload và chọn lại BN vừa đăng ký
                _daCoThe = true;
                _daKhoa = false;
                LoadDanhSachThanhVien();
                LoadChiTietThanhVien(_maBNDangChon);
                btnDangKyThe.Enabled = false;
                btnHuyThe.Enabled = true;
                CapNhatNutKhoaThe();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng ký thẻ:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // KHÓA / MỞ KHÓA THẺ THÀNH VIÊN
        // ══════════════════════════════════════════════════════════════════════
        private void BtnHuyThe_Click(object sender, EventArgs e)
        {
            if (_maBNDangChon <= 0 || !_daCoThe) return;

            string tenBN = lblTenBN.Text;
            bool moKhoa = _daKhoa; // Nếu đang khóa → bấm nút sẽ mở khóa

            string tieuDe = moKhoa ? "Xác nhận mở khóa thẻ" : "Xác nhận khóa thẻ";
            string thongBao = moKhoa
                ? $"🔓 Bạn có chắc muốn MỞ KHÓA thẻ thành viên của \"{tenBN}\"?\n\nKhách hàng sẽ được sử dụng thẻ và tích điểm như bình thường."
                : $"🔒 Bạn có chắc muốn KHÓA thẻ thành viên của \"{tenBN}\"?\n\nThẻ sẽ tạm ngưng sử dụng. Điểm tích lũy và lịch sử được giữ nguyên, có thể mở khóa lại sau.";

            var xacNhan = MessageBox.Show(thongBao, tieuDe,
                MessageBoxButtons.YesNo,
                moKhoa ? MessageBoxIcon.Question : MessageBoxIcon.Warning);

            if (xacNhan != DialogResult.Yes) return;

            try
            {
                if (moKhoa)
                {
                    DatabaseConnection.ExecuteNonQuery(@"
                        UPDATE ThanhVienInfo
                        SET DaKhoa = 0, NgayKhoa = NULL
                        WHERE MaBenhNhan = @MaBN",
                        p => p.AddWithValue("@MaBN", _maBNDangChon));

                    _daKhoa = false;
                    MessageBox.Show($"Đã mở khóa thẻ thành viên của \"{tenBN}\". 🔓",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DatabaseConnection.ExecuteNonQuery(@"
                        UPDATE ThanhVienInfo
                        SET DaKhoa = 1, NgayKhoa = GETDATE()
                        WHERE MaBenhNhan = @MaBN",
                        p => p.AddWithValue("@MaBN", _maBNDangChon));

                    _daKhoa = true;
                    MessageBox.Show($"Đã khóa thẻ thành viên của \"{tenBN}\". 🔒",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadDanhSachThanhVien();
                CapNhatNutKhoaThe();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật trạng thái thẻ:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // TÌM KIẾM & FILTER
        // ══════════════════════════════════════════════════════════════════════
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            _tuKhoa = txtSearch.Text;
            LoadDanhSachThanhVien();
        }

        private void CboFilterHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDanhSachThanhVien();
        }

        // ══════════════════════════════════════════════════════════════════════
        // TIER CARDS MẶC ĐỊNH
        // ══════════════════════════════════════════════════════════════════════
        private void DatTierCardMacDinh()
        {
            // Tier cards đã có sẵn trong Designer với text tĩnh
            // Không cần load từ DB — chỉ cập nhật nếu muốn động
        }

        // ══════════════════════════════════════════════════════════════════════
        // STUB EVENTS TỪ DESIGNER
        // ══════════════════════════════════════════════════════════════════════
        private void pnlDgvThanhVien_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void pnlRight_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void pnlThongKe_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void pnlTienDo_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void lblTitleTK_Click(object sender, EventArgs e) { }
        private void guna2ProgressBar1_ValueChanged(object sender, EventArgs e) { }
        private void dgvBenhNhan_CellContentClick(object sender,
            System.Windows.Forms.DataGridViewCellEventArgs e)
        { }
        private void cboFilterHang_SelectedIndexChanged(object sender, EventArgs e)
            => CboFilterHang_SelectedIndexChanged(sender, e);
    }
}
