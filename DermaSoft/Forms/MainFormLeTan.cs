using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DermaSoft.Helpers;
using DermaSoft.Theme;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    /// <summary>
    /// Form chính dành cho vai trò Lễ Tân.
    /// Cấu trúc giống hệt MainForm — sidebar, topbar, pnlMdiArea.
    /// Menu sidebar được xây bằng code (TaoMenuSidebar), không hardcode trong Designer.
    /// </summary>
    public partial class MainFormLeTan : Form
    {
        // Theo dõi menu item đang được chọn (để highlight active)
        // Dùng Guna2Button vì MainForm dùng pattern này — chỉ lưu Parent + Tag
        private Guna2Button _menuHienTai = null;

        /// <summary>Cờ đánh dấu đang đăng xuất (true) hay đóng ứng dụng (false).</summary>
        internal bool DangXuat { get; private set; } = false;

        // Bật WS_EX_COMPOSITED: OS tự composite toàn bộ cửa sổ → loại bỏ flicker
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // CONSTRUCTOR
        // ══════════════════════════════════════════════════════════════════════

        public MainFormLeTan()
        {
            InitializeComponent();

            // Bỏ qua logic khi đang chạy trong Designer của Visual Studio
            if (this.DesignMode ||
                System.ComponentModel.LicenseManager.UsageMode ==
                System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            DoubleBufferHelper.BatDoubleBuffered(pnlMdiArea);

            CaiDatThongTinNguoiDung(); // Hiển thị tên + vai trò lên sidebar & topbar
            TaoMenuSidebar();          // Vẽ menu Lễ Tân lên pnlSidebarNav
            CapNhatNgayGio();          // Khởi động Timer cập nhật giờ mỗi phút
            HienThiMdiMacDinh();       // Mở Dashboard khi vừa đăng nhập xong
        }

        // ══════════════════════════════════════════════════════════════════════
        // KHỞI TẠO — Cài đặt thông tin người dùng đang đăng nhập
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Lấy thông tin từ LoginForm.NguoiDungHienTai và hiển thị lên:
        ///   - lblTopbarAvatar : chữ cái đầu của tên (avatar tròn)
        ///   - lblUserName     : họ tên đầy đủ trong sidebar
        ///   - lblUserRole     : icon + tên vai trò trong sidebar
        /// </summary>
        private void CaiDatThongTinNguoiDung()
        {
            var nd = LoginForm.NguoiDungHienTai;
            if (nd == null) return;

            // Avatar = chữ cái đầu của họ tên, sẽ được vẽ bo tròn trong OnLoad
            string chuCaiDau = string.IsNullOrEmpty(nd.HoTen)
                ? "L"
                : nd.HoTen.Substring(0, 1).ToUpper();

            lblTopbarAvatar.Text = chuCaiDau;
            lblUserName.Text = nd.HoTen;

            // Lễ Tân luôn hiển thị icon 📋
            lblUserRole.Text = "📋 Lễ Tân";

            // Gắn sự kiện đăng xuất cho nút btnDangXuat (đã khai báo trong Designer)
            btnDangXuat.Click += BtnDangXuat_Click;
        }

        // ══════════════════════════════════════════════════════════════════════
        // TIMER — Cập nhật ngày giờ mỗi 60 giây
        // ══════════════════════════════════════════════════════════════════════

        private void CapNhatNgayGio()
        {
            // Cập nhật ngay lập tức khi form load
            lblNgayGio.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");

            // Timer tự động cập nhật mỗi phút
            var timer = new Timer { Interval = 60_000 };
            timer.Tick += (s, e) =>
            {
                lblNgayGio.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");
            };
            timer.Start();
        }

        // ══════════════════════════════════════════════════════════════════════
        // SIDEBAR MENU — Xây menu bằng code (không dùng Designer)
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Tạo toàn bộ menu sidebar cho Lễ Tân.
        /// Mỗi mục gồm: section label (tiêu đề nhóm) + menu item (icon + tên).
        /// Biến y theo dõi vị trí dọc, tăng dần sau mỗi control.
        /// </summary>
        private void TaoMenuSidebar()
        {
            pnlSidebarNav.Controls.Clear();
            int y = 4;

            // ── TỔNG QUAN ─────────────────────────────────────────────────────
            y = ThemSectionLabel("TỔNG QUAN", y);
            y = ThemMenuItem("📊", "Dashboard", y, active: true);  // Mặc định active khi load

            // ── TIẾP ĐÓN ─────────────────────────────────────────────────────
            y = ThemSectionLabel("TIẾP ĐÓN", y);
            y = ThemMenuItem("📅", "Quản Lý Lịch Hẹn", y);
            y = ThemMenuItem("👥", "Quản Lý Bệnh Nhân", y);
            y = ThemMenuItem("🏥", "Tiếp Nhận Bệnh Nhân", y);

            // ── THANH TOÁN ───────────────────────────────────────────────────
            y = ThemSectionLabel("THANH TOÁN", y);
            y = ThemMenuItem("💳", "Thanh Toán Hóa Đơn", y);
            y = ThemMenuItem("🎖️", "Thẻ Thành Viên", y);

            // ── HỆ THỐNG ─────────────────────────────────────────────────────
            y = ThemSectionLabel("HỆ THỐNG", y);
            y = ThemMenuItem("👤", "Hồ Sơ Cá Nhân", y);
        }

        /// <summary>
        /// Thêm một label tiêu đề nhóm menu (VD: "TIẾP ĐÓN") vào sidebar.
        /// Trả về vị trí y tiếp theo.
        /// </summary>
        private int ThemSectionLabel(string text, int y)
        {
            var lbl = new Label
            {
                Text = text.ToUpper(),
                Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 168, 230, 207), // Xanh mờ — giống MainForm
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(310, 18),
                Location = new Point(18, y + 8),
            };
            pnlSidebarNav.Controls.Add(lbl);
            return y + 26; // Tiêu đề nhóm cao 26px
        }

        /// <summary>
        /// Thêm một menu item vào sidebar.
        ///   icon   : emoji (📋, 📅, ...)
        ///   text   : tên menu (dùng làm Tag để nhận biết khi click)
        ///   active : true = đang được chọn → highlight xanh lá
        /// Trả về vị trí y tiếp theo.
        /// </summary>
        private int ThemMenuItem(string icon, string text, int y, bool active = false)
        {
            // Panel bao ngoài = toàn bộ hàng menu
            var pnl = new Panel
            {
                Size = new Size(330, 38),
                Location = new Point(0, y),
                BackColor = active ? Color.FromArgb(38, 168, 230, 207) : Color.Transparent,
                Cursor = Cursors.Hand,
                Tag = text,    // Dùng để biết menu nào đang được click
            };

            // Thanh xanh bên trái — chỉ hiện khi active
            var borderLeft = new Panel
            {
                Size = new Size(3, 38),
                Location = new Point(0, 0),
                BackColor = active ? Color.FromArgb(168, 230, 207) : Color.Transparent,
            };
            pnl.Controls.Add(borderLeft);

            // Label chứa icon + tên menu
            var lbl = new Label
            {
                Text = "  " + icon + "   " + text,
                Font = new Font("Segoe UI", 9f, active ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = active ? Color.White : Color.FromArgb(180, 255, 255, 255),
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(310, 38),
                Location = new Point(8, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
            };
            pnl.Controls.Add(lbl);

            // ── Hiệu ứng hover (di chuột vào/ra) ────────────────────────────
            Action<bool> setHover = (hover) =>
            {
                // Không đổi màu nếu đây đang là menu active
                if (_menuHienTai?.Parent == pnl) return;

                pnl.BackColor = hover ? Color.FromArgb(20, 168, 230, 207) : Color.Transparent;
                borderLeft.BackColor = hover ? Color.FromArgb(100, 168, 230, 207) : Color.Transparent;
                lbl.ForeColor = hover ? Color.White : Color.FromArgb(180, 255, 255, 255);
            };

            pnl.MouseEnter += (s, e) => setHover(true);
            pnl.MouseLeave += (s, e) => setHover(false);
            lbl.MouseEnter += (s, e) => setHover(true);
            lbl.MouseLeave += (s, e) => setHover(false);

            // ── Sự kiện click ────────────────────────────────────────────────
            EventHandler onClick = (s, e) => MenuItem_Click(pnl, borderLeft, lbl);
            pnl.Click += onClick;
            lbl.Click += onClick;

            pnlSidebarNav.Controls.Add(pnl);

            // Đánh dấu menu đang active — dùng Guna2Button ẩn để lưu reference
            // (Giống pattern trong MainForm — không ảnh hưởng UI, chỉ lưu Parent)
            if (active)
                _menuHienTai = new Guna2Button { Parent = pnl, Visible = false, Tag = text };

            return y + 40; // Mỗi menu item cao 40px (rộng hơn MainForm 2px cho dễ nhìn)
        }

        // ══════════════════════════════════════════════════════════════════════
        // SỰ KIỆN MENU — Khi click một mục trong sidebar
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Xử lý khi người dùng click vào một menu item:
        ///   1. Bỏ highlight menu cũ
        ///   2. Highlight menu mới
        ///   3. Cập nhật breadcrumb + tiêu đề topbar
        ///   4. Mở form con tương ứng vào pnlMdiArea
        /// </summary>
        // SĐT tạm lưu khi điều hướng từ Dashboard → Tiếp Nhận
        private string _sdtChoTiepNhan = null;

        private void MenuItem_Click(Panel pnl, Panel borderLeft, Label lbl)
        {
            // ── Bỏ highlight menu cũ ─────────────────────────────────────────
            if (_menuHienTai?.Parent != null)
            {
                var oldPnl = _menuHienTai.Parent;
                oldPnl.BackColor = Color.Transparent;

                foreach (Control c in oldPnl.Controls)
                {
                    if (c is Panel p && p.Width == 3)      // Panel borderLeft
                        p.BackColor = Color.Transparent;
                    if (c is Label l)
                    {
                        l.ForeColor = Color.FromArgb(180, 255, 255, 255);
                        l.Font = new Font("Segoe UI", 9f);
                    }
                }
            }

            // ── Highlight menu mới ───────────────────────────────────────────
            pnl.BackColor = Color.FromArgb(38, 168, 230, 207);
            borderLeft.BackColor = Color.FromArgb(168, 230, 207);
            lbl.ForeColor = Color.White;
            lbl.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            // Lưu reference menu đang active
            _menuHienTai = new Guna2Button { Parent = pnl, Visible = false, Tag = pnl.Tag };

            // ── Cập nhật topbar ──────────────────────────────────────────────
            string tenMenu = pnl.Tag?.ToString() ?? "";
            lblTopbarTitle.Text = tenMenu;
            lblBreadcrumb.Text = "DermaSoft › " + tenMenu;

            // Đẩy lblTopbarTitle sang phải một chút sau breadcrumb
            lblTopbarTitle.Location = new Point(lblBreadcrumb.Right + 10,
                                                lblTopbarTitle.Location.Y);

            // ── Mở form con ──────────────────────────────────────────────────
            MoFormCon(tenMenu, _sdtChoTiepNhan);
        }

        // ══════════════════════════════════════════════════════════════════════
        // MDI AREA — Quản lý form con hiển thị trong pnlMdiArea
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Mở Dashboard ngay khi form load lần đầu.
        /// </summary>
        private void HienThiMdiMacDinh()
        {
            MoFormCon("Dashboard");
        }

        /// <summary>
        /// Cho phép form con (VD: DashboardLeTanForm) điều hướng sang menu khác.
        /// Tìm panel menu theo Tag trong sidebar → gọi MenuItem_Click để
        /// sidebar highlight + topbar + form con đều cập nhật đồng bộ.
        /// </summary>
        internal void ChuyenMenu(string tenMenu)
        {
            ChuyenMenu(tenMenu, null);
        }

        // MaPhieuKham tạm lưu khi điều hướng từ Dashboard → Thanh Toán
        private int _maPhieuKhamChoThanhToan = -1;

        /// <summary>
        /// Chuyển menu Thanh Toán Hóa Đơn và tự động mở phiếu khám.
        /// </summary>
        internal void ChuyenMenuThanhToan(int maPhieuKham)
        {
            _maPhieuKhamChoThanhToan = maPhieuKham;
            ChuyenMenu("Thanh Toán Hóa Đơn");
            _maPhieuKhamChoThanhToan = -1;
        }

        /// <summary>
        /// Chuyển menu và truyền SĐT bệnh nhân (dùng khi tiếp nhận từ Dashboard).
        /// </summary>
        internal void ChuyenMenu(string tenMenu, string soDienThoaiBN)
        {
            // Lưu SĐT tạm để MoFormCon sử dụng
            _sdtChoTiepNhan = soDienThoaiBN;

            foreach (Control c in pnlSidebarNav.Controls)
            {
                if (c is Panel pnl && pnl.Tag?.ToString() == tenMenu)
                {
                    Panel borderLeft = null;
                    Label lbl = null;
                    foreach (Control child in pnl.Controls)
                    {
                        if (child is Panel p && p.Width == 3) borderLeft = p;
                        if (child is Label l) lbl = l;
                    }
                    if (borderLeft != null && lbl != null)
                        MenuItem_Click(pnl, borderLeft, lbl);
                    _sdtChoTiepNhan = null;
                    return;
                }
            }

            // Fallback
            MoFormCon(tenMenu, soDienThoaiBN);
            _sdtChoTiepNhan = null;
        }

        /// <summary>
        /// Mở form con tương ứng với tên menu được click.
        /// Form con được nhúng trực tiếp vào pnlMdiArea (TopLevel = false, Dock = Fill).
        /// Mỗi lần chuyển menu sẽ xóa form cũ và tạo form mới.
        /// </summary>
        private void MoFormCon(string tenMenu)
        {
            MoFormCon(tenMenu, null);
        }

        private void MoFormCon(string tenMenu, string soDienThoaiBN)
        {
            Form frm = null;

            switch (tenMenu)
            {
                case "Dashboard":           frm = new DashboardLeTanForm(); break;
                case "Quản Lý Lịch Hẹn":    frm = new AppointmentForm();    break;
                case "Quản Lý Bệnh Nhân":   frm = new PatientForm();        break;
                case "Tiếp Nhận Bệnh Nhân":
                    frm = !string.IsNullOrEmpty(soDienThoaiBN)
                        ? new TiepNhanForm(soDienThoaiBN)
                        : new TiepNhanForm();
                    break;
                case "Thanh Toán Hóa Đơn":
                    frm = _maPhieuKhamChoThanhToan > 0
                        ? new InvoiceForm(_maPhieuKhamChoThanhToan)
                        : new InvoiceForm();
                    break;
                case "Thẻ Thành Viên":      frm = new MemberForm();         break;
                case "Hồ Sơ Cá Nhân":       frm = new ProfileForm();        break;
            }

            DoubleBufferHelper.NhungFormCon(pnlMdiArea, frm);
        }

        // ══════════════════════════════════════════════════════════════════════
        // PAINT — Bo tròn avatar chữ cái trong topbar
        // ══════════════════════════════════════════════════════════════════════

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.DesignMode) return;

            // Vẽ label avatar dạng hình tròn
            BoTronLabel(lblTopbarAvatar);
        }

        /// <summary>
        /// Ghi đè sự kiện Paint của label để vẽ hình tròn màu xanh
        /// chứa chữ cái đầu của tên người dùng.
        /// </summary>
        private void BoTronLabel(Label lbl)
        {
            lbl.Paint += (s, pe) =>
            {
                var g = pe.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                int d = Math.Min(lbl.Width, lbl.Height);

                using (var brush = new SolidBrush(lbl.BackColor))
                using (var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                })
                {
                    g.Clear(lbl.Parent.BackColor);                          // Xóa nền
                    g.FillEllipse(brush, 0, 0, d - 1, d - 1);              // Vẽ hình tròn
                    g.DrawString(lbl.Text, lbl.Font, Brushes.White,        // Vẽ chữ
                                 new RectangleF(0, 0, d, d), sf);
                }
            };
            lbl.Invalidate(); // Kích hoạt vẽ lại ngay
        }

        // ══════════════════════════════════════════════════════════════════════
        // EVENT HANDLERS — Các nút điều khiển cửa sổ
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Nút ✕ — Đóng form. Vì LoginForm đã hook FormClosed,
        /// ứng dụng sẽ tự thoát sau khi MainFormLeTan đóng.
        /// </summary>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Nút — — Thu nhỏ form xuống taskbar.
        /// </summary>
        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Nút □ — Toggle giữa Maximized và Normal.
        /// </summary>
        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = this.WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;
        }

        /// <summary>
        /// Nút Đăng Xuất — Xác nhận rồi xóa session và đóng form.
        /// LoginForm sẽ hiện lại vì hook FormClosed trong LoginForm.
        /// </summary>
        private void BtnDangXuat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoginForm.NguoiDungHienTai = null;
                DangXuat = true;
                this.Close();
            }
        }
    }
}