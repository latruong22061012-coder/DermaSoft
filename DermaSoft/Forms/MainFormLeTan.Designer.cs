using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    partial class MainFormLeTan
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Controls
        // ── Sidebar ──────────────────────────────────────────────────────────
        private Panel pnlSidebar;
        private Panel pnlSidebarLogo;
        private Guna2Panel pnlLogoIcon;
        private Label lblLogoText;
        private Label lblLogoSub;
        private Panel pnlSidebarUser;
        private Label lblUserName;
        private Label lblUserRole;
        private Panel pnlSidebarNav;          // Menu items xây bằng code
        private Guna2Button btnDangXuat;
        // ── Topbar ───────────────────────────────────────────────────────────
        private Panel pnlTopbar;
        private Label lblBreadcrumb;
        private Label lblTopbarTitle;
        private Label lblNgayGio;
        private Label lblTopbarAvatar;
        private Guna2Button btnThongBao;
        // ── Content area ─────────────────────────────────────────────────────
        private Panel pnlMdiArea;
        // ── Custom title bar (FormBorderStyle.None) ───────────────────────────
        private Guna2GradientPanel pnlTitleBar;
        private Label lblTitleText;
        private Guna2Button btnMinimize;
        private Guna2Button btnMaximize;
        private Guna2Button btnClose;
        private Guna2DragControl dragControl;
        #endregion

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ── Khởi tạo controls ──────────────────────────────────────────
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlSidebarNav = new System.Windows.Forms.Panel();
            this.btnDangXuat = new Guna.UI2.WinForms.Guna2Button();
            this.pnlSidebarUser = new System.Windows.Forms.Panel();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.pnlSidebarLogo = new System.Windows.Forms.Panel();
            this.pnlLogoIcon = new Guna.UI2.WinForms.Guna2Panel();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.lblLogoSub = new System.Windows.Forms.Label();
            this.pnlTopbar = new System.Windows.Forms.Panel();
            this.lblBreadcrumb = new System.Windows.Forms.Label();
            this.lblTopbarTitle = new System.Windows.Forms.Label();
            this.btnThongBao = new Guna.UI2.WinForms.Guna2Button();
            this.lblNgayGio = new System.Windows.Forms.Label();
            this.lblTopbarAvatar = new System.Windows.Forms.Label();
            this.pnlMdiArea = new System.Windows.Forms.Panel();
            this.pnlTitleBar = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.lblTitleText = new System.Windows.Forms.Label();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2Button();
            this.btnMaximize = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.dragControl = new Guna.UI2.WinForms.Guna2DragControl(this.components);

            this.pnlSidebar.SuspendLayout();
            this.pnlSidebarUser.SuspendLayout();
            this.pnlSidebarLogo.SuspendLayout();
            this.pnlTopbar.SuspendLayout();
            this.pnlTitleBar.SuspendLayout();
            this.SuspendLayout();

            // ══════════════════════════════════════════════════════════════════
            // pnlSidebar  —  Cột trái, toàn bộ chiều cao bên dưới TitleBar
            // ══════════════════════════════════════════════════════════════════
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.pnlSidebar.Controls.Add(this.pnlSidebarNav);
            this.pnlSidebar.Controls.Add(this.btnDangXuat);
            this.pnlSidebar.Controls.Add(this.pnlSidebarUser);
            this.pnlSidebar.Controls.Add(this.pnlSidebarLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 78);
            this.pnlSidebar.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(338, 882);
            this.pnlSidebar.TabIndex = 2;

            // ══════════════════════════════════════════════════════════════════
            // pnlSidebarNav  —  Khu vực menu (Fill, xây bằng code trong .cs)
            // ══════════════════════════════════════════════════════════════════
            this.pnlSidebarNav.AutoScroll = true;
            this.pnlSidebarNav.BackColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.pnlSidebarNav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSidebarNav.Location = new System.Drawing.Point(0, 169);
            this.pnlSidebarNav.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSidebarNav.Name = "pnlSidebarNav";
            this.pnlSidebarNav.Size = new System.Drawing.Size(338, 658);
            this.pnlSidebarNav.TabIndex = 1;

            // ══════════════════════════════════════════════════════════════════
            // btnDangXuat  —  Nút đăng xuất, luôn nằm dưới cùng sidebar
            // ══════════════════════════════════════════════════════════════════
            this.btnDangXuat.BorderColor = System.Drawing.Color.White;
            this.btnDangXuat.BorderThickness = 1;
            this.btnDangXuat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDangXuat.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDangXuat.FillColor = System.Drawing.Color.FromArgb(12, 72, 60);
            this.btnDangXuat.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDangXuat.ForeColor = System.Drawing.Color.White;
            this.btnDangXuat.HoverState.FillColor = System.Drawing.Color.MediumSeaGreen;
            this.btnDangXuat.Location = new System.Drawing.Point(0, 827);
            this.btnDangXuat.Margin = new System.Windows.Forms.Padding(4);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Size = new System.Drawing.Size(338, 55);
            this.btnDangXuat.TabIndex = 0;
            this.btnDangXuat.Text = "🚪  Đăng Xuất";

            // ══════════════════════════════════════════════════════════════════
            // pnlSidebarUser  —  Khu vực hiển thị tên & vai trò người dùng
            // ══════════════════════════════════════════════════════════════════
            this.pnlSidebarUser.BackColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.pnlSidebarUser.Controls.Add(this.lblUserName);
            this.pnlSidebarUser.Controls.Add(this.lblUserRole);
            this.pnlSidebarUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSidebarUser.Location = new System.Drawing.Point(0, 75);
            this.pnlSidebarUser.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSidebarUser.Name = "pnlSidebarUser";
            this.pnlSidebarUser.Size = new System.Drawing.Size(338, 94);
            this.pnlSidebarUser.TabIndex = 2;

            // lblUserName  —  Tên người dùng (set từ LoginForm.NguoiDungHienTai)
            this.lblUserName.AutoSize = true;
            this.lblUserName.BackColor = System.Drawing.Color.Transparent;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.White;
            this.lblUserName.Location = new System.Drawing.Point(72, 21);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(136, 20);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "Lễ Tân";

            // lblUserRole  —  Vai trò (set động trong code)
            this.lblUserRole.AutoSize = true;
            this.lblUserRole.BackColor = System.Drawing.Color.Transparent;
            this.lblUserRole.Font = new System.Drawing.Font("Segoe UI Emoji", 7.2F);
            this.lblUserRole.ForeColor = System.Drawing.Color.FromArgb(168, 213, 197);
            this.lblUserRole.Location = new System.Drawing.Point(73, 54);
            this.lblUserRole.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(148, 16);
            this.lblUserRole.TabIndex = 2;
            this.lblUserRole.Text = "📋 Lễ Tân";

            // ══════════════════════════════════════════════════════════════════
            // pnlSidebarLogo  —  Logo + tên ứng dụng (trên cùng sidebar)
            // ══════════════════════════════════════════════════════════════════
            this.pnlSidebarLogo.BackColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.pnlSidebarLogo.Controls.Add(this.pnlLogoIcon);
            this.pnlSidebarLogo.Controls.Add(this.lblLogoText);
            this.pnlSidebarLogo.Controls.Add(this.lblLogoSub);
            this.pnlSidebarLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSidebarLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebarLogo.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSidebarLogo.Name = "pnlSidebarLogo";
            this.pnlSidebarLogo.Size = new System.Drawing.Size(338, 75);
            this.pnlSidebarLogo.TabIndex = 3;

            // pnlLogoIcon  —  Ô chứa logo (BackgroundImage từ Resources)
            this.pnlLogoIcon.BackgroundImage = global::DermaSoft.Properties.Resources.logo1;
            this.pnlLogoIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlLogoIcon.BorderRadius = 6;
            this.pnlLogoIcon.Location = new System.Drawing.Point(30, 12);
            this.pnlLogoIcon.Margin = new System.Windows.Forms.Padding(4);
            this.pnlLogoIcon.Name = "pnlLogoIcon";
            this.pnlLogoIcon.Size = new System.Drawing.Size(51, 50);
            this.pnlLogoIcon.TabIndex = 0;

            // lblLogoText  —  "DermaSoft"
            this.lblLogoText.AutoSize = true;
            this.lblLogoText.BackColor = System.Drawing.Color.Transparent;
            this.lblLogoText.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLogoText.ForeColor = System.Drawing.Color.White;
            this.lblLogoText.Location = new System.Drawing.Point(100, 18);
            this.lblLogoText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(108, 25);
            this.lblLogoText.TabIndex = 1;
            this.lblLogoText.Text = "DermaSoft";

            // lblLogoSub  —  "Phòng Khám Da Liễu"
            this.lblLogoSub.AutoSize = true;
            this.lblLogoSub.BackColor = System.Drawing.Color.Transparent;
            this.lblLogoSub.Font = new System.Drawing.Font("Segoe UI Emoji", 7.2F);
            this.lblLogoSub.ForeColor = System.Drawing.Color.FromArgb(168, 213, 197);
            this.lblLogoSub.Location = new System.Drawing.Point(89, 46);
            this.lblLogoSub.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogoSub.Name = "lblLogoSub";
            this.lblLogoSub.Size = new System.Drawing.Size(119, 16);
            this.lblLogoSub.TabIndex = 2;
            this.lblLogoSub.Text = "Phòng Khám Da Liễu";

            // ══════════════════════════════════════════════════════════════════
            // pnlTopbar  —  Thanh ngang bên trên khu vực nội dung
            // ══════════════════════════════════════════════════════════════════
            this.pnlTopbar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlTopbar.Controls.Add(this.lblBreadcrumb);
            this.pnlTopbar.Controls.Add(this.lblTopbarTitle);
            this.pnlTopbar.Controls.Add(this.btnThongBao);
            this.pnlTopbar.Controls.Add(this.lblNgayGio);
            this.pnlTopbar.Controls.Add(this.lblTopbarAvatar);
            this.pnlTopbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopbar.Location = new System.Drawing.Point(338, 78);
            this.pnlTopbar.Margin = new System.Windows.Forms.Padding(4);
            this.pnlTopbar.Name = "pnlTopbar";
            this.pnlTopbar.Size = new System.Drawing.Size(1370, 75);
            this.pnlTopbar.TabIndex = 1;

            // lblBreadcrumb  —  "DermaSoft › Dashboard"
            this.lblBreadcrumb.AutoSize = true;
            this.lblBreadcrumb.BackColor = System.Drawing.Color.Transparent;
            this.lblBreadcrumb.Font = new System.Drawing.Font("Segoe UI", 7.8F);
            this.lblBreadcrumb.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.lblBreadcrumb.Location = new System.Drawing.Point(25, 33);
            this.lblBreadcrumb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBreadcrumb.Name = "lblBreadcrumb";
            this.lblBreadcrumb.Size = new System.Drawing.Size(147, 17);
            this.lblBreadcrumb.TabIndex = 0;
            this.lblBreadcrumb.Text = "DermaSoft › Dashboard";

            // lblTopbarTitle  —  Tiêu đề trang hiện tại (cập nhật khi đổi menu)
            this.lblTopbarTitle.AutoSize = true;
            this.lblTopbarTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTopbarTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTopbarTitle.ForeColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.lblTopbarTitle.Location = new System.Drawing.Point(233, 27);
            this.lblTopbarTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTopbarTitle.Name = "lblTopbarTitle";
            this.lblTopbarTitle.Size = new System.Drawing.Size(135, 25);
            this.lblTopbarTitle.TabIndex = 1;
            this.lblTopbarTitle.Text = "Tiếp Đón";

            // btnThongBao  —  Nút chuông thông báo (Anchor Right)
            this.btnThongBao.Anchor = System.Windows.Forms.AnchorStyles.Top
                                    | System.Windows.Forms.AnchorStyles.Right;
            this.btnThongBao.BorderRadius = 8;
            this.btnThongBao.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThongBao.FillColor = System.Drawing.Color.Gainsboro;
            this.btnThongBao.Font = new System.Drawing.Font("Segoe UI Emoji", 11F);
            this.btnThongBao.ForeColor = System.Drawing.Color.Black;
            this.btnThongBao.Location = new System.Drawing.Point(1055, 19);
            this.btnThongBao.Margin = new System.Windows.Forms.Padding(4);
            this.btnThongBao.Name = "btnThongBao";
            this.btnThongBao.Size = new System.Drawing.Size(42, 42);
            this.btnThongBao.TabIndex = 2;
            this.btnThongBao.Text = "🔔";

            // lblNgayGio  —  Ngày giờ hiện tại (Anchor Right, cập nhật bằng Timer)
            this.lblNgayGio.Anchor = System.Windows.Forms.AnchorStyles.Top
                                   | System.Windows.Forms.AnchorStyles.Right;
            this.lblNgayGio.AutoSize = true;
            this.lblNgayGio.BackColor = System.Drawing.Color.Transparent;
            this.lblNgayGio.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblNgayGio.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblNgayGio.Location = new System.Drawing.Point(1127, 31);
            this.lblNgayGio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNgayGio.Name = "lblNgayGio";
            this.lblNgayGio.Size = new System.Drawing.Size(0, 19);
            this.lblNgayGio.TabIndex = 3;

            // lblTopbarAvatar  —  Chữ cái đầu tên người dùng, vẽ bo tròn bằng Paint
            this.lblTopbarAvatar.Anchor = System.Windows.Forms.AnchorStyles.Top
                                        | System.Windows.Forms.AnchorStyles.Right;
            this.lblTopbarAvatar.BackColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.lblTopbarAvatar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTopbarAvatar.ForeColor = System.Drawing.Color.White;
            this.lblTopbarAvatar.Location = new System.Drawing.Point(1315, 18);
            this.lblTopbarAvatar.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTopbarAvatar.Name = "lblTopbarAvatar";
            this.lblTopbarAvatar.Size = new System.Drawing.Size(40, 40);
            this.lblTopbarAvatar.TabIndex = 4;
            this.lblTopbarAvatar.Text = "L";
            this.lblTopbarAvatar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ══════════════════════════════════════════════════════════════════
            // pnlMdiArea  —  Khu vực hiển thị các form con (thay thế IsMdiContainer)
            // ══════════════════════════════════════════════════════════════════
            this.pnlMdiArea.BackColor = System.Drawing.Color.FromArgb(246, 249, 247);
            this.pnlMdiArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMdiArea.Location = new System.Drawing.Point(338, 153);
            this.pnlMdiArea.Margin = new System.Windows.Forms.Padding(4);
            this.pnlMdiArea.Name = "pnlMdiArea";
            this.pnlMdiArea.Padding = new System.Windows.Forms.Padding(20);
            this.pnlMdiArea.Size = new System.Drawing.Size(1370, 807);
            this.pnlMdiArea.TabIndex = 0;

            // ══════════════════════════════════════════════════════════════════
            // pnlTitleBar  —  Custom title bar (gradient xanh → vàng)
            //                 Chứa tên app + nút Minimize / Maximize / Close
            // ══════════════════════════════════════════════════════════════════
            this.pnlTitleBar.Controls.Add(this.lblTitleText);
            this.pnlTitleBar.Controls.Add(this.btnMinimize);
            this.pnlTitleBar.Controls.Add(this.btnMaximize);
            this.pnlTitleBar.Controls.Add(this.btnClose);
            this.pnlTitleBar.CustomizableEdges.BottomLeft = false;
            this.pnlTitleBar.CustomizableEdges.BottomRight = false;
            this.pnlTitleBar.CustomizableEdges.TopLeft = false;
            this.pnlTitleBar.CustomizableEdges.TopRight = false;
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.FillColor = System.Drawing.Color.FromArgb(15, 92, 77);   // Xanh đậm
            this.pnlTitleBar.FillColor2 = System.Drawing.Color.FromArgb(184, 138, 40); // Vàng Gold
            this.pnlTitleBar.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleBar.Margin = new System.Windows.Forms.Padding(4);
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Size = new System.Drawing.Size(1708, 78);
            this.pnlTitleBar.TabIndex = 3;

            // lblTitleText  —  Tên ứng dụng trên title bar
            this.lblTitleText.AutoSize = true;
            this.lblTitleText.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitleText.ForeColor = System.Drawing.Color.White;
            this.lblTitleText.Location = new System.Drawing.Point(64, 22);
            this.lblTitleText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitleText.Name = "lblTitleText";
            this.lblTitleText.Size = new System.Drawing.Size(523, 28);
            this.lblTitleText.TabIndex = 4;
            this.lblTitleText.Text = "DermaSoft — Hệ Thống Quản Lý Phòng Khám Da Liễu";

            // btnMinimize  —  Nút thu nhỏ (Anchor TopRight)
            this.btnMinimize.Anchor = System.Windows.Forms.AnchorStyles.Top
                                    | System.Windows.Forms.AnchorStyles.Right;
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimize.FillColor = System.Drawing.Color.Transparent;
            this.btnMinimize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(1516, 15);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(4);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(58, 40);
            this.btnMinimize.TabIndex = 2;
            this.btnMinimize.Text = "—";
            this.btnMinimize.UseTransparentBackground = true;
            this.btnMinimize.Click += new System.EventHandler(this.BtnMinimize_Click);

            // btnMaximize  —  Nút phóng to / thu nhỏ (Anchor TopRight)
            this.btnMaximize.Anchor = System.Windows.Forms.AnchorStyles.Top
                                    | System.Windows.Forms.AnchorStyles.Right;
            this.btnMaximize.BackColor = System.Drawing.Color.Transparent;
            this.btnMaximize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMaximize.FillColor = System.Drawing.Color.Transparent;
            this.btnMaximize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnMaximize.ForeColor = System.Drawing.Color.White;
            this.btnMaximize.Location = new System.Drawing.Point(1581, 15);
            this.btnMaximize.Margin = new System.Windows.Forms.Padding(4);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(58, 40);
            this.btnMaximize.TabIndex = 1;
            this.btnMaximize.Text = "□";
            this.btnMaximize.UseTransparentBackground = true;
            this.btnMaximize.Click += new System.EventHandler(this.BtnMaximize_Click);

            // btnClose  —  Nút đóng (Anchor TopRight, hover đỏ)
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top
                                  | System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FillColor = System.Drawing.Color.Transparent;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.HoverState.FillColor = System.Drawing.Color.FromArgb(232, 17, 35);
            this.btnClose.Location = new System.Drawing.Point(1646, 15);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(58, 40);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "✕";
            this.btnClose.UseTransparentBackground = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);

            // ══════════════════════════════════════════════════════════════════
            // dragControl  —  Cho phép kéo form qua pnlTitleBar
            // ══════════════════════════════════════════════════════════════════
            this.dragControl.DockIndicatorTransparencyValue = 0.6D;
            this.dragControl.TargetControl = this.pnlTitleBar;
            this.dragControl.UseTransparentDrag = true;

            // ══════════════════════════════════════════════════════════════════
            // MainFormLeTan  —  Form chính
            // ══════════════════════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(246, 249, 247);
            this.ClientSize = new System.Drawing.Size(1708, 960);
            // Thứ tự add QUAN TRỌNG: control add sau sẽ nằm trên cùng (z-order)
            // Fill (pnlMdiArea) phải add TRƯỚC Sidebar/TitleBar để dock đúng
            this.Controls.Add(this.pnlMdiArea);
            this.Controls.Add(this.pnlTopbar);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlTitleBar);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;  // Custom TitleBar
            this.IsMdiContainer = false;   // Dùng pnlMdiArea thay vì MDI native
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainFormLeTan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DermaSoft — Hệ Thống Quản Lý Phòng Khám Da Liễu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            // ── Resume layout ──────────────────────────────────────────────
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebarUser.ResumeLayout(false);
            this.pnlSidebarUser.PerformLayout();
            this.pnlSidebarLogo.ResumeLayout(false);
            this.pnlSidebarLogo.PerformLayout();
            this.pnlTopbar.ResumeLayout(false);
            this.pnlTopbar.PerformLayout();
            this.pnlTitleBar.ResumeLayout(false);
            this.pnlTitleBar.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}