using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Controls khai báo
        private Guna2GradientPanel pnlHeader;
        private Label              lblAppName;
        private Label              lblTagline;
        private Label              lblVersion;
        private Guna2Panel         pnlForm;
        private Label              lblTenDangNhap;
        private Guna2TextBox       txtTenDangNhap;
        private Label              lblMatKhau;
        private Guna2TextBox       txtMatKhau;
        private Guna2Button        btnShowPassword;
        private Guna2CheckBox      chkNhoDangNhap;
        private LinkLabel          lnkQuenMatKhau;
        private Guna2Button        btnDangNhap;
        private Guna2Panel         pnlError;
        private Label              lblError;
        private Guna2Panel         pnlFooter;
        private Guna2Elipse        elipse;
        private Guna2DragControl   dragControl;
        #endregion

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlHeader = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.lblAppName = new System.Windows.Forms.Label();
            this.lblTagline = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlForm = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTenDangNhap = new System.Windows.Forms.Label();
            this.txtTenDangNhap = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblMatKhau = new System.Windows.Forms.Label();
            this.txtMatKhau = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnShowPassword = new Guna.UI2.WinForms.Guna2Button();
            this.chkNhoDangNhap = new Guna.UI2.WinForms.Guna2CheckBox();
            this.lnkQuenMatKhau = new System.Windows.Forms.LinkLabel();
            this.btnDangNhap = new Guna.UI2.WinForms.Guna2Button();
            this.pnlError = new Guna.UI2.WinForms.Guna2Panel();
            this.lblError = new System.Windows.Forms.Label();
            this.pnlFooter = new Guna.UI2.WinForms.Guna2Panel();
            this.lblFooter = new System.Windows.Forms.Label();
            this.elipse = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.dragControl = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.guna2GradientPanel1 = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlForm.SuspendLayout();
            this.pnlError.SuspendLayout();
            this.guna2GradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblAppName);
            this.pnlHeader.Controls.Add(this.lblTagline);
            this.pnlHeader.Controls.Add(this.lblVersion);
            this.pnlHeader.Controls.Add(this.guna2Panel2);
            this.pnlHeader.CustomizableEdges.BottomLeft = false;
            this.pnlHeader.CustomizableEdges.BottomRight = false;
            this.pnlHeader.CustomizableEdges.TopLeft = false;
            this.pnlHeader.CustomizableEdges.TopRight = false;
            this.pnlHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.pnlHeader.FillColor2 = System.Drawing.Color.MediumSeaGreen;
            this.pnlHeader.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.pnlHeader.Location = new System.Drawing.Point(0, 52);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(531, 233);
            this.pnlHeader.TabIndex = 0;
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);
            // 
            // lblAppName
            // 
            this.lblAppName.BackColor = System.Drawing.Color.Transparent;
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblAppName.ForeColor = System.Drawing.Color.White;
            this.lblAppName.Location = new System.Drawing.Point(0, 111);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(526, 56);
            this.lblAppName.TabIndex = 1;
            this.lblAppName.Text = "DermaSoft";
            this.lblAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAppName.Click += new System.EventHandler(this.lblAppName_Click);
            // 
            // lblTagline
            // 
            this.lblTagline.BackColor = System.Drawing.Color.Transparent;
            this.lblTagline.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTagline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(245)))), ((int)(((byte)(230)))));
            this.lblTagline.Location = new System.Drawing.Point(0, 167);
            this.lblTagline.Name = "lblTagline";
            this.lblTagline.Size = new System.Drawing.Size(526, 23);
            this.lblTagline.TabIndex = 2;
            this.lblTagline.Text = "Hệ Thống Quản Lý Phòng Khám Da Liễu";
            this.lblTagline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(214)))), ((int)(((byte)(197)))));
            this.lblVersion.Location = new System.Drawing.Point(-4, 190);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(526, 19);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "DermaSoft Clinic · Phiên bản 1.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel2.BackgroundImage = global::DermaSoft.Properties.Resources.logoMain;
            this.guna2Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.guna2Panel2.BorderRadius = 12;
            this.guna2Panel2.BorderStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.guna2Panel2.BorderThickness = 12;
            this.guna2Panel2.CustomBorderThickness = new System.Windows.Forms.Padding(8);
            this.guna2Panel2.Location = new System.Drawing.Point(204, 22);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.ShadowDecoration.BorderRadius = 12;
            this.guna2Panel2.ShadowDecoration.Color = System.Drawing.Color.DarkGray;
            this.guna2Panel2.Size = new System.Drawing.Size(128, 86);
            this.guna2Panel2.TabIndex = 4;
            // 
            // pnlForm
            // 
            this.pnlForm.BorderColor = System.Drawing.Color.Black;
            this.pnlForm.BorderStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            this.pnlForm.Controls.Add(this.lblTenDangNhap);
            this.pnlForm.Controls.Add(this.txtTenDangNhap);
            this.pnlForm.Controls.Add(this.lblMatKhau);
            this.pnlForm.Controls.Add(this.txtMatKhau);
            this.pnlForm.Controls.Add(this.btnShowPassword);
            this.pnlForm.Controls.Add(this.chkNhoDangNhap);
            this.pnlForm.Controls.Add(this.lnkQuenMatKhau);
            this.pnlForm.Controls.Add(this.btnDangNhap);
            this.pnlForm.Controls.Add(this.pnlError);
            this.pnlForm.FillColor = System.Drawing.Color.White;
            this.pnlForm.Location = new System.Drawing.Point(0, 291);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(531, 343);
            this.pnlForm.TabIndex = 1;
            // 
            // lblTenDangNhap
            // 
            this.lblTenDangNhap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTenDangNhap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.lblTenDangNhap.Location = new System.Drawing.Point(37, 25);
            this.lblTenDangNhap.Name = "lblTenDangNhap";
            this.lblTenDangNhap.Size = new System.Drawing.Size(453, 21);
            this.lblTenDangNhap.TabIndex = 0;
            this.lblTenDangNhap.Text = "Tên đăng nhập  *";
            // 
            // txtTenDangNhap
            // 
            this.txtTenDangNhap.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.txtTenDangNhap.BorderRadius = 10;
            this.txtTenDangNhap.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenDangNhap.DefaultText = "";
            this.txtTenDangNhap.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(247)))));
            this.txtTenDangNhap.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtTenDangNhap.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTenDangNhap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.txtTenDangNhap.Location = new System.Drawing.Point(36, 51);
            this.txtTenDangNhap.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtTenDangNhap.MaxLength = 50;
            this.txtTenDangNhap.Name = "txtTenDangNhap";
            this.txtTenDangNhap.PlaceholderText = "Nhập tên đăng nhập (5 ký tự)";
            this.txtTenDangNhap.SelectedText = "";
            this.txtTenDangNhap.Size = new System.Drawing.Size(464, 51);
            this.txtTenDangNhap.TabIndex = 0;
            // 
            // lblMatKhau
            // 
            this.lblMatKhau.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMatKhau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.lblMatKhau.Location = new System.Drawing.Point(37, 109);
            this.lblMatKhau.Name = "lblMatKhau";
            this.lblMatKhau.Size = new System.Drawing.Size(453, 21);
            this.lblMatKhau.TabIndex = 1;
            this.lblMatKhau.Text = "Mật khẩu  *";
            // 
            // txtMatKhau
            // 
            this.txtMatKhau.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.txtMatKhau.BorderRadius = 10;
            this.txtMatKhau.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMatKhau.DefaultText = "";
            this.txtMatKhau.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(247)))));
            this.txtMatKhau.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtMatKhau.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMatKhau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.txtMatKhau.Location = new System.Drawing.Point(36, 143);
            this.txtMatKhau.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtMatKhau.MaxLength = 100;
            this.txtMatKhau.Name = "txtMatKhau";
            this.txtMatKhau.PlaceholderText = "Nhập mật khẩu";
            this.txtMatKhau.SelectedText = "";
            this.txtMatKhau.Size = new System.Drawing.Size(400, 51);
            this.txtMatKhau.TabIndex = 1;
            this.txtMatKhau.UseSystemPasswordChar = true;
            // 
            // btnShowPassword
            // 
            this.btnShowPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.btnShowPassword.BorderRadius = 10;
            this.btnShowPassword.BorderThickness = 1;
            this.btnShowPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShowPassword.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(247)))));
            this.btnShowPassword.Font = new System.Drawing.Font("Segoe UI Emoji", 12F);
            this.btnShowPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnShowPassword.Location = new System.Drawing.Point(442, 143);
            this.btnShowPassword.Name = "btnShowPassword";
            this.btnShowPassword.Size = new System.Drawing.Size(58, 51);
            this.btnShowPassword.TabIndex = 2;
            this.btnShowPassword.TabStop = false;
            this.btnShowPassword.Text = "👁";
            // 
            // chkNhoDangNhap
            // 
            this.chkNhoDangNhap.AutoSize = true;
            this.chkNhoDangNhap.CheckedState.BorderRadius = 0;
            this.chkNhoDangNhap.CheckedState.BorderThickness = 0;
            this.chkNhoDangNhap.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkNhoDangNhap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.chkNhoDangNhap.Location = new System.Drawing.Point(41, 202);
            this.chkNhoDangNhap.Name = "chkNhoDangNhap";
            this.chkNhoDangNhap.Size = new System.Drawing.Size(134, 24);
            this.chkNhoDangNhap.TabIndex = 2;
            this.chkNhoDangNhap.Text = "Nhớ đăng nhập";
            this.chkNhoDangNhap.UncheckedState.BorderRadius = 0;
            this.chkNhoDangNhap.UncheckedState.BorderThickness = 0;
            // 
            // lnkQuenMatKhau
            // 
            this.lnkQuenMatKhau.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lnkQuenMatKhau.AutoSize = true;
            this.lnkQuenMatKhau.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkQuenMatKhau.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkQuenMatKhau.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.lnkQuenMatKhau.Location = new System.Drawing.Point(351, 206);
            this.lnkQuenMatKhau.Name = "lnkQuenMatKhau";
            this.lnkQuenMatKhau.Size = new System.Drawing.Size(116, 20);
            this.lnkQuenMatKhau.TabIndex = 3;
            this.lnkQuenMatKhau.TabStop = true;
            this.lnkQuenMatKhau.Text = "Quên mật khẩu?";
            this.lnkQuenMatKhau.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkQuenMatKhau_LinkClicked_1);
            // 
            // btnDangNhap
            // 
            this.btnDangNhap.BorderRadius = 20;
            this.btnDangNhap.BorderStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.btnDangNhap.BorderThickness = 1;
            this.btnDangNhap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDangNhap.CustomBorderColor = System.Drawing.Color.Black;
            this.btnDangNhap.CustomBorderThickness = new System.Windows.Forms.Padding(1);
            this.btnDangNhap.DisabledState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.btnDangNhap.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnDangNhap.FocusedColor = System.Drawing.Color.Black;
            this.btnDangNhap.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnDangNhap.ForeColor = System.Drawing.Color.White;
            this.btnDangNhap.HoverState.FillColor = System.Drawing.Color.Green;
            this.btnDangNhap.Location = new System.Drawing.Point(37, 240);
            this.btnDangNhap.Name = "btnDangNhap";
            this.btnDangNhap.Size = new System.Drawing.Size(453, 51);
            this.btnDangNhap.TabIndex = 3;
            this.btnDangNhap.Text = "🔑   Đăng Nhập";
            // 
            // pnlError
            // 
            this.pnlError.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.pnlError.BorderRadius = 8;
            this.pnlError.Controls.Add(this.lblError);
            this.pnlError.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.pnlError.Location = new System.Drawing.Point(37, 297);
            this.pnlError.Name = "pnlError";
            this.pnlError.Size = new System.Drawing.Size(453, 43);
            this.pnlError.TabIndex = 4;
            this.pnlError.Visible = false;
            // 
            // lblError
            // 
            this.lblError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblError.Location = new System.Drawing.Point(0, 0);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(453, 43);
            this.lblError.TabIndex = 0;
            this.lblError.Text = "⚠️  Tên đăng nhập hoặc mật khẩu không đúng!";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(244)))), ((int)(((byte)(238)))));
            this.pnlFooter.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(250)))), ((int)(((byte)(245)))));
            this.pnlFooter.Location = new System.Drawing.Point(0, 637);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(526, 50);
            this.pnlFooter.TabIndex = 2;
            // 
            // lblFooter
            // 
            this.lblFooter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFooter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblFooter.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblFooter.ForeColor = System.Drawing.Color.Gray;
            this.lblFooter.Location = new System.Drawing.Point(0, 634);
            this.lblFooter.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(526, 53);
            this.lblFooter.TabIndex = 0;
            this.lblFooter.Text = "© 2026 DermaSoft Clinic · Phần mềm quản lý phòng khám da liễu";
            this.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblFooter.Click += new System.EventHandler(this.lblFooter_Click);
            // 
            // elipse
            // 
            this.elipse.BorderRadius = 16;
            this.elipse.TargetControl = this;
            // 
            // dragControl
            // 
            this.dragControl.DockIndicatorTransparencyValue = 0.6D;
            this.dragControl.TargetControl = this.pnlHeader;
            this.dragControl.UseTransparentDrag = true;
            // 
            // guna2GradientPanel1
            // 
            this.guna2GradientPanel1.Controls.Add(this.guna2Button1);
            this.guna2GradientPanel1.Controls.Add(this.guna2Panel1);
            this.guna2GradientPanel1.Controls.Add(this.label1);
            this.guna2GradientPanel1.CustomizableEdges.BottomLeft = false;
            this.guna2GradientPanel1.CustomizableEdges.BottomRight = false;
            this.guna2GradientPanel1.CustomizableEdges.TopLeft = false;
            this.guna2GradientPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.guna2GradientPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(138)))), ((int)(((byte)(40)))));
            this.guna2GradientPanel1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.guna2GradientPanel1.Location = new System.Drawing.Point(-16, -7);
            this.guna2GradientPanel1.Name = "guna2GradientPanel1";
            this.guna2GradientPanel1.Size = new System.Drawing.Size(547, 60);
            this.guna2GradientPanel1.TabIndex = 3;
            // 
            // guna2Button1
            // 
            this.guna2Button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Button1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Button1.BorderColor = System.Drawing.Color.Transparent;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.Transparent;
            this.guna2Button1.HoverState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2Button1.HoverState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2Button1.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button1.HoverState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2Button1.HoverState.Image = global::DermaSoft.Properties.Resources.closeIcon50;
            this.guna2Button1.ImageSize = new System.Drawing.Size(25, 25);
            this.guna2Button1.Location = new System.Drawing.Point(490, 19);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.PressedColor = System.Drawing.Color.Transparent;
            this.guna2Button1.Size = new System.Drawing.Size(40, 34);
            this.guna2Button1.TabIndex = 7;
            this.guna2Button1.UseTransparentBackground = true;
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackgroundImage = global::DermaSoft.Properties.Resources.logoRound;
            this.guna2Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.guna2Panel1.Location = new System.Drawing.Point(28, 19);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(38, 34);
            this.guna2Panel1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(68, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(276, 23);
            this.label1.TabIndex = 5;
            this.label1.Text = "DermaSoft - Đăng Nhập Hệ Thống";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(526, 684);
            this.Controls.Add(this.lblFooter);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.guna2GradientPanel1);
            this.Controls.Add(this.pnlForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "   ";
            this.pnlHeader.ResumeLayout(false);
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            this.pnlError.ResumeLayout(false);
            this.guna2GradientPanel1.ResumeLayout(false);
            this.guna2GradientPanel1.PerformLayout();
            this.ResumeLayout(false);

        }
        private Guna2Panel guna2Panel2;
        private Guna2GradientPanel guna2GradientPanel1;
        private Label lblFooter;
        protected Label label1;
        private Guna2Panel guna2Panel1;
        private Guna2Button guna2Button1;
    }
}
