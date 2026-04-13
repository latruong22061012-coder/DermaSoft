using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DermaSoft.Data;

namespace DermaSoft.Forms
{
    public partial class HinhAnhForm : Form
    {
        private int _maPhieuKham = -1;          // FIX 1: context bắt buộc
        private int _maHinhAnhDangChon = -1;    // key hàng đang chọn trong DGV

        // ── Constructor từ PhieuKhamForm (có context) ─────────────────────
        public HinhAnhForm(int maPhieuKham)
        {
            InitializeComponent();
            _maPhieuKham = maPhieuKham;
            this.Load += HinhAnhForm_Load;
        }

        // ── Constructor từ sidebar (không có context — hiện picker) ────────
        public HinhAnhForm()
        {
            InitializeComponent();
            this.Load += HinhAnhForm_Load;
        }

        private void HinhAnhForm_Load(object sender, EventArgs e)
        {
            WireEvents();
            CauHinhDgv();

            if (_maPhieuKham > 0)
                LoadDanhSachAnh();
            else
                HienThiChonPhieuKham();
        }

        // ══════════════════════════════════════════════════════════════════
        // WIRE EVENTS
        // ══════════════════════════════════════════════════════════════════
        private void WireEvents()
        {
            btnUploadAnh.Click += BtnUploadAnh_Click;
            btnChonNhieu.Click += BtnChonNhieu_Click;
            btnLuuGhiChu.Click += BtnLuuGhiChu_Click;
            btnPhongTo.Click += BtnPhongTo_Click;
            btnXoaThumb1.Click += (s, e) => XoaThumb(1);
            btnXoaThumb2.Click += (s, e) => XoaThumb(2);  // FIX 6: dùng đúng tên
            dgvHinhAnh.CellClick += DgvHinhAnh_CellClick;
            dgvHinhAnh.SelectionChanged += DgvHinhAnh_SelectionChanged;
        }

        // ══════════════════════════════════════════════════════════════════
        // CẤU HÌNH DGV — DataPropertyName (FIX 3 + 4)
        // ══════════════════════════════════════════════════════════════════
        private void CauHinhDgv()
        {
            // Cột ẩn — key để UPDATE/DELETE
            colMaHinhAnh.DataPropertyName = "MaHinhAnh";   // FIX 4
            colDuongDan.DataPropertyName = "DuongDanAnh"; // FIX 4

            // Cột hiện
            colTenFile.DataPropertyName = "TenFile";     // FIX 3 (alias trong SQL)
            colGhiChu.DataPropertyName = "GhiChu";      // FIX 3
            colNgayChup.DataPropertyName = "NgayChup";    // FIX 3
        }

        // ══════════════════════════════════════════════════════════════════
        // LOAD DANH SÁCH ẢNH
        // map: HinhAnhBenhLy WHERE MaPhieuKham = @MaPK
        // ══════════════════════════════════════════════════════════════════
        private void LoadDanhSachAnh()
        {
            try
            {
                const string sql = @"
                    SELECT
                        ha.MaHinhAnh,
                        ha.DuongDanAnh,
                        -- TenFile lấy từ đường dẫn (alias cho colTenFile)
                        RIGHT(ha.DuongDanAnh,
                            CHARINDEX('/', REVERSE(ha.DuongDanAnh)) - 1) AS TenFile,
                        ha.GhiChu,
                        CONVERT(VARCHAR, ha.NgayChup, 103) + ' '
                        + CONVERT(VARCHAR, ha.NgayChup, 108)             AS NgayChup
                    FROM HinhAnhBenhLy ha
                    WHERE ha.MaPhieuKham = @MaPK
                    ORDER BY ha.NgayChup DESC";

                DataTable dt = DatabaseConnection.ExecuteQuery(sql,
                    p => p.AddWithValue("@MaPK", _maPhieuKham));

                dgvHinhAnh.DataSource = dt;

                // Cập nhật thumbnails (2 ảnh đầu tiên)
                CapNhatThumbnails(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách ảnh: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CapNhatThumbnails(DataTable dt)
        {
            // Reset
            picThumb1.Image = null;
            picThumb2.Image = null;

            if (dt == null || dt.Rows.Count == 0) return;

            // Thumb 1
            string path1 = dt.Rows[0]["DuongDanAnh"]?.ToString();
            if (!string.IsNullOrEmpty(path1) && File.Exists(path1))
                try { picThumb1.Image = Image.FromFile(path1); } catch { }

            if (dt.Rows.Count < 2) return;

            // Thumb 2
            string path2 = dt.Rows[1]["DuongDanAnh"]?.ToString();
            if (!string.IsNullOrEmpty(path2) && File.Exists(path2))
                try { picThumb2.Image = Image.FromFile(path2); } catch { }
        }

        // ══════════════════════════════════════════════════════════════════
        // UPLOAD ẢNH — map: INSERT HinhAnhBenhLy
        // ══════════════════════════════════════════════════════════════════
        private void BtnUploadAnh_Click(object sender, EventArgs e)
        {
            if (_maPhieuKham < 0)
            {
                MessageBox.Show("Chưa chọn phiếu khám.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var ofd = new OpenFileDialog
            {
                Title = "Chọn ảnh bệnh lý",
                Filter = "Ảnh|*.jpg;*.jpeg;*.png;*.bmp",
                Multiselect = false
            })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                UploadFile(ofd.FileName);
            }
        }

        private void BtnChonNhieu_Click(object sender, EventArgs e)
        {
            if (_maPhieuKham < 0)
            {
                MessageBox.Show("Chưa chọn phiếu khám.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var ofd = new OpenFileDialog
            {
                Title = "Chọn nhiều ảnh bệnh lý",
                Filter = "Ảnh|*.jpg;*.jpeg;*.png;*.bmp",
                Multiselect = true
            })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                foreach (string file in ofd.FileNames)
                    UploadFile(file);
            }
        }

        private void UploadFile(string filePath)
        {
            try
            {
                // Kiểm tra dung lượng ≤ 10MB
                var info = new FileInfo(filePath);
                if (info.Length > 10 * 1024 * 1024)
                {
                    MessageBox.Show($"File '{info.Name}' vượt quá 10MB.",
                        "Quá dung lượng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // INSERT HinhAnhBenhLy
                // DuongDanAnh: lưu đường dẫn tuyệt đối
                // GhiChu: NULL — bác sĩ nhập sau qua txtGhiChuAnh
                // NgayChup: GETDATE() (DB tự điền)
                DatabaseConnection.ExecuteNonQuery(@"
                    INSERT INTO HinhAnhBenhLy (MaPhieuKham, DuongDanAnh, NgayChup)
                    VALUES (@MaPK, @DuongDan, GETDATE())",
                    p =>
                    {
                        p.AddWithValue("@MaPK", _maPhieuKham);
                        p.AddWithValue("@DuongDan", filePath);
                    });

                LoadDanhSachAnh(); // Reload
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi upload ảnh: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // LƯU GHI CHÚ — map: UPDATE HinhAnhBenhLy SET GhiChu WHERE MaHinhAnh
        // ══════════════════════════════════════════════════════════════════
        private void BtnLuuGhiChu_Click(object sender, EventArgs e)
        {
            if (_maHinhAnhDangChon < 0)
            {
                MessageBox.Show("Vui lòng chọn ảnh trong danh sách trước.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // map: HinhAnhBenhLy.GhiChu (nullable NVARCHAR(200))
                DatabaseConnection.ExecuteNonQuery(@"
                    UPDATE HinhAnhBenhLy
                    SET GhiChu = @GhiChu
                    WHERE MaHinhAnh = @MaHA",
                    p =>
                    {
                        string ghiChu = txtGhiChuAnh.Text.Trim();
                        p.Add("@GhiChu", System.Data.SqlDbType.NVarChar, 200).Value =
                            string.IsNullOrEmpty(ghiChu) ? (object)System.DBNull.Value : ghiChu;
                        p.AddWithValue("@MaHA", _maHinhAnhDangChon);
                    });

                MessageBox.Show("Đã lưu ghi chú.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachAnh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu ghi chú: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // XOÁ ẢNH — map: DELETE HinhAnhBenhLy WHERE MaHinhAnh
        // ══════════════════════════════════════════════════════════════════
        private void XoaThumb(int slot)
        {
            DataTable dt = dgvHinhAnh.DataSource as DataTable;
            if (dt == null || dt.Rows.Count < slot) return;

            int maHA = Convert.ToInt32(dt.Rows[slot - 1]["MaHinhAnh"]);
            var confirm = MessageBox.Show("Xóa ảnh này khỏi phiếu khám?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                DatabaseConnection.ExecuteNonQuery(
                    "DELETE FROM HinhAnhBenhLy WHERE MaHinhAnh = @MaHA",
                    p => p.AddWithValue("@MaHA", maHA));
                LoadDanhSachAnh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa ảnh: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // DGV EVENTS — chọn hàng → load preview + ghi chú
        // ══════════════════════════════════════════════════════════════════
        private void DgvHinhAnh_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHinhAnh.CurrentRow == null) return;
            DataRow r = ((DataRowView)dgvHinhAnh.CurrentRow.DataBoundItem).Row;

            _maHinhAnhDangChon = Convert.ToInt32(r["MaHinhAnh"]);

            // Load preview
            string path = r["DuongDanAnh"]?.ToString();
            picPreview.Image = null;
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
                try { picPreview.Image = Image.FromFile(path); } catch { }

            // Load ghi chú vào txtGhiChuAnh
            txtGhiChuAnh.Text = r["GhiChu"] != DBNull.Value
                ? r["GhiChu"].ToString() : "";
        }

        // CellClick xử lý nút Xem / X trong cột Thao tác
        private void DgvHinhAnh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
        }

        // [FIX] Method này được Designer wire qua CellContentClick
        private void dgvHinhAnh_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            // Xử lý chung với CellClick — delegate sang cùng logic
            DgvHinhAnh_CellClick(sender, e);
        }

        // Event stub từ Designer (lblFileHint.Click)
        private void lblFileHint_Click(object sender, EventArgs e) { }

        // ══════════════════════════════════════════════════════════════════
        // PHÓNG TO
        // ══════════════════════════════════════════════════════════════════
        private void BtnPhongTo_Click(object sender, EventArgs e)
        {
            if (picPreview.Image == null) return;

            var frm = new Form
            {
                Text = "Hình Ảnh Bệnh Lý",
                StartPosition = FormStartPosition.CenterScreen,
                Size = new Size(900, 700),
                BackColor = Color.Black
            };
            var pic = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = picPreview.Image,
                BackColor = Color.Black
            };
            frm.Controls.Add(pic);
            frm.ShowDialog(this);
        }

        // ══════════════════════════════════════════════════════════════════
        // PICKER — khi mở từ sidebar (không có maPhieuKham)
        // ══════════════════════════════════════════════════════════════════
        private void HienThiChonPhieuKham()
        {
            // Thông báo bác sĩ cần mở từ PhieuKhamForm
            MessageBox.Show(
                "Để xem/thêm hình ảnh bệnh lý, vui lòng mở từ tab\n" +
                "\"Hình Ảnh\" bên trong Phiếu Khám Bệnh.",
                "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
