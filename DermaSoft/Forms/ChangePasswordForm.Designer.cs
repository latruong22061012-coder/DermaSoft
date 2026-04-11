using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    partial class ChangePasswordForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Controls
        private Guna2GradientPanel pnlTitleBar;
        private Guna2Panel         pnlTitleLogo;
        private Label              lblTitleText;
        private Guna2Panel         pnlWarning;
        private Label              lblWarningIcon;
        private Label              lblWarningText;
        private Label              lblMatKhauHienTai;
        private Guna2TextBox       txtMatKhauHienTai;
        private Label              lblMatKhauMoi;
        private Guna2TextBox       txtMatKhauMoi;
        private Label              lblDoManh;
        private Label              lblDoManhGiaTri;
        private Guna2ProgressBar   barDoManh;
        private Label              lblYeuCau;
        private Label              lblXacNhan;
        private Guna2TextBox       txtXacNhan;
        private Guna2Button        btnLuuMatKhau;
        private Guna2Button        btnHuy;
        private Guna2Panel         pnlError;
        private Label              lblError;
        private Guna2Elipse        elipse;
        private Guna2DragControl   dragControl;
        #endregion

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlTitleBar = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.pnlTitleLogo = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTitleText = new System.Windows.Forms.Label();
            this.pnlWarning = new Guna.UI2.WinForms.Guna2Panel();
            this.lblWarningIcon = new System.Windows.Forms.Label();
            this.lblWarningText = new System.Windows.Forms.Label();
            this.lblMatKhauHienTai = new System.Windows.Forms.Label();
            this.txtMatKhauHienTai = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblMatKhauMoi = new System.Windows.Forms.Label();
            this.txtMatKhauMoi = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblDoManh = new System.Windows.Forms.Label();
            this.lblDoManhGiaTri = new System.Windows.Forms.Label();
            this.barDoManh = new Guna.UI2.WinForms.Guna2ProgressBar();
            this.lblYeuCau = new System.Windows.Forms.Label();
            this.lblXacNhan = new System.Windows.Forms.Label();
            this.txtXacNhan = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnLuuMatKhau = new Guna.UI2.WinForms.Guna2Button();
            this.btnHuy = new Guna.UI2.WinForms.Guna2Button();
            this.pnlError = new Guna.UI2.WinForms.Guna2Panel();
            this.lblError = new System.Windows.Forms.Label();
            this.elipse = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.dragControl = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.pnlTitleBar.SuspendLayout();
            this.pnlWarning.SuspendLayout();
            this.pnlError.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTitleBar
            // 
            this.pnlTitleBar.Controls.Add(this.pnlTitleLogo);
            this.pnlTitleBar.Controls.Add(this.lblTitleText);
            this.pnlTitleBar.CustomizableEdges.BottomLeft = false;
            this.pnlTitleBar.CustomizableEdges.BottomRight = false;
            this.pnlTitleBar.CustomizableEdges.TopLeft = false;
            this.pnlTitleBar.CustomizableEdges.TopRight = false;
            this.pnlTitleBar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.pnlTitleBar.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(138)))), ((int)(((byte)(40)))));
            this.pnlTitleBar.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Size = new System.Drawing.Size(533, 61);
            this.pnlTitleBar.TabIndex = 0;
            // 
            // pnlTitleLogo
            // 
            this.pnlTitleLogo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlTitleLogo.BackgroundImage = global::DermaSoft.Properties.Resources.logoRound;
            this.pnlTitleLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlTitleLogo.Location = new System.Drawing.Point(12, 12);
            this.pnlTitleLogo.Name = "pnlTitleLogo";
            this.pnlTitleLogo.Size = new System.Drawing.Size(44, 43);
            this.pnlTitleLogo.TabIndex = 0;
            this.pnlTitleLogo.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTitleLogo_Paint);
            // 
            // lblTitleText
            // 
            this.lblTitleText.AutoSize = true;
            this.lblTitleText.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleText.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.lblTitleText.ForeColor = System.Drawing.Color.White;
            this.lblTitleText.Location = new System.Drawing.Point(62, 23);
            this.lblTitleText.Name = "lblTitleText";
            this.lblTitleText.Size = new System.Drawing.Size(238, 23);
            this.lblTitleText.TabIndex = 1;
            this.lblTitleText.Text = "🔒 Đổi Mật Khẩu — Bắt Buộc";
            // 
            // pnlWarning
            // 
            this.pnlWarning.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(224)))), ((int)(((byte)(71)))));
            this.pnlWarning.BorderRadius = 10;
            this.pnlWarning.Controls.Add(this.lblWarningIcon);
            this.pnlWarning.Controls.Add(this.lblWarningText);
            this.pnlWarning.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(251)))), ((int)(((byte)(235)))));
            this.pnlWarning.Location = new System.Drawing.Point(38, 82);
            this.pnlWarning.Name = "pnlWarning";
            this.pnlWarning.Size = new System.Drawing.Size(450, 57);
            this.pnlWarning.TabIndex = 1;
            // 
            // lblWarningIcon
            // 
            this.lblWarningIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 14F);
            this.lblWarningIcon.Location = new System.Drawing.Point(11, 4);
            this.lblWarningIcon.Name = "lblWarningIcon";
            this.lblWarningIcon.Size = new System.Drawing.Size(30, 50);
            this.lblWarningIcon.TabIndex = 0;
            this.lblWarningIcon.Text = "⚠️";
            this.lblWarningIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWarningText
            // 
            this.lblWarningText.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblWarningText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(64)))), ((int)(((byte)(14)))));
            this.lblWarningText.Location = new System.Drawing.Point(47, 4);
            this.lblWarningText.Name = "lblWarningText";
            this.lblWarningText.Size = new System.Drawing.Size(395, 50);
            this.lblWarningText.TabIndex = 1;
            this.lblWarningText.Text = "Lần đăng nhập đầu tiên! Bạn bắt buộc phải đổi mật khẩu trước khi sử dụng hệ thống" +
    ".";
            this.lblWarningText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMatKhauHienTai
            // 
            this.lblMatKhauHienTai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMatKhauHienTai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.lblMatKhauHienTai.Location = new System.Drawing.Point(48, 151);
            this.lblMatKhauHienTai.Name = "lblMatKhauHienTai";
            this.lblMatKhauHienTai.Size = new System.Drawing.Size(432, 20);
            this.lblMatKhauHienTai.TabIndex = 2;
            this.lblMatKhauHienTai.Text = "Mật khẩu hiện tại  *";
            // 
            // txtMatKhauHienTai
            // 
            this.txtMatKhauHienTai.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.txtMatKhauHienTai.BorderRadius = 10;
            this.txtMatKhauHienTai.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMatKhauHienTai.DefaultText = "";
            this.txtMatKhauHienTai.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(247)))));
            this.txtMatKhauHienTai.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtMatKhauHienTai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMatKhauHienTai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.txtMatKhauHienTai.Location = new System.Drawing.Point(38, 185);
            this.txtMatKhauHienTai.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtMatKhauHienTai.MaxLength = 100;
            this.txtMatKhauHienTai.Name = "txtMatKhauHienTai";
            this.txtMatKhauHienTai.PlaceholderText = "Nhập mật khẩu hiện tại";
            this.txtMatKhauHienTai.SelectedText = "";
            this.txtMatKhauHienTai.Size = new System.Drawing.Size(450, 53);
            this.txtMatKhauHienTai.TabIndex = 0;
            this.txtMatKhauHienTai.UseSystemPasswordChar = true;
            this.txtMatKhauHienTai.TextChanged += new System.EventHandler(this.txtMatKhauHienTai_TextChanged);
            // 
            // lblMatKhauMoi
            // 
            this.lblMatKhauMoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMatKhauMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.lblMatKhauMoi.Location = new System.Drawing.Point(49, 252);
            this.lblMatKhauMoi.Name = "lblMatKhauMoi";
            this.lblMatKhauMoi.Size = new System.Drawing.Size(432, 20);
            this.lblMatKhauMoi.TabIndex = 3;
            this.lblMatKhauMoi.Text = "Mật khẩu mới  *";
            // 
            // txtMatKhauMoi
            // 
            this.txtMatKhauMoi.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.txtMatKhauMoi.BorderRadius = 10;
            this.txtMatKhauMoi.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMatKhauMoi.DefaultText = "";
            this.txtMatKhauMoi.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(247)))));
            this.txtMatKhauMoi.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtMatKhauMoi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMatKhauMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.txtMatKhauMoi.Location = new System.Drawing.Point(38, 290);
            this.txtMatKhauMoi.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtMatKhauMoi.MaxLength = 100;
            this.txtMatKhauMoi.Name = "txtMatKhauMoi";
            this.txtMatKhauMoi.PlaceholderText = "Tối thiểu 8 ký tự, có chữ hoa + số";
            this.txtMatKhauMoi.SelectedText = "";
            this.txtMatKhauMoi.Size = new System.Drawing.Size(450, 53);
            this.txtMatKhauMoi.TabIndex = 1;
            this.txtMatKhauMoi.UseSystemPasswordChar = true;
            // 
            // lblDoManh
            // 
            this.lblDoManh.AutoSize = true;
            this.lblDoManh.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblDoManh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblDoManh.Location = new System.Drawing.Point(56, 379);
            this.lblDoManh.Name = "lblDoManh";
            this.lblDoManh.Size = new System.Drawing.Size(128, 19);
            this.lblDoManh.TabIndex = 4;
            this.lblDoManh.Text = "Độ mạnh mật khẩu";
            // 
            // lblDoManhGiaTri
            // 
            this.lblDoManhGiaTri.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDoManhGiaTri.AutoSize = true;
            this.lblDoManhGiaTri.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblDoManhGiaTri.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.lblDoManhGiaTri.Location = new System.Drawing.Point(426, 296);
            this.lblDoManhGiaTri.Name = "lblDoManhGiaTri";
            this.lblDoManhGiaTri.Size = new System.Drawing.Size(0, 19);
            this.lblDoManhGiaTri.TabIndex = 5;
            // 
            // barDoManh
            // 
            this.barDoManh.BorderRadius = 4;
            this.barDoManh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.barDoManh.Location = new System.Drawing.Point(38, 401);
            this.barDoManh.Name = "barDoManh";
            this.barDoManh.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.barDoManh.ProgressColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.barDoManh.Size = new System.Drawing.Size(450, 8);
            this.barDoManh.TabIndex = 6;
            this.barDoManh.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.barDoManh.ValueChanged += new System.EventHandler(this.barDoManh_ValueChanged);
            // 
            // lblYeuCau
            // 
            this.lblYeuCau.AutoSize = true;
            this.lblYeuCau.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblYeuCau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.lblYeuCau.Location = new System.Drawing.Point(56, 348);
            this.lblYeuCau.Name = "lblYeuCau";
            this.lblYeuCau.Size = new System.Drawing.Size(217, 19);
            this.lblYeuCau.TabIndex = 7;
            this.lblYeuCau.Text = "Yêu cầu: ≥8 ký tự, 1 chữ hoa, 1 số";
            // 
            // lblXacNhan
            // 
            this.lblXacNhan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblXacNhan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.lblXacNhan.Location = new System.Drawing.Point(48, 422);
            this.lblXacNhan.Name = "lblXacNhan";
            this.lblXacNhan.Size = new System.Drawing.Size(432, 20);
            this.lblXacNhan.TabIndex = 8;
            this.lblXacNhan.Text = "Xác nhận mật khẩu mới  *";
            // 
            // txtXacNhan
            // 
            this.txtXacNhan.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.txtXacNhan.BorderRadius = 10;
            this.txtXacNhan.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtXacNhan.DefaultText = "";
            this.txtXacNhan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(247)))));
            this.txtXacNhan.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtXacNhan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtXacNhan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.txtXacNhan.Location = new System.Drawing.Point(38, 456);
            this.txtXacNhan.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtXacNhan.MaxLength = 100;
            this.txtXacNhan.Name = "txtXacNhan";
            this.txtXacNhan.PlaceholderText = "Nhập lại mật khẩu mới";
            this.txtXacNhan.SelectedText = "";
            this.txtXacNhan.Size = new System.Drawing.Size(450, 53);
            this.txtXacNhan.TabIndex = 2;
            this.txtXacNhan.UseSystemPasswordChar = true;
            // 
            // btnLuuMatKhau
            // 
            this.btnLuuMatKhau.BorderRadius = 20;
            this.btnLuuMatKhau.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuuMatKhau.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnLuuMatKhau.FocusedColor = System.Drawing.Color.White;
            this.btnLuuMatKhau.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnLuuMatKhau.ForeColor = System.Drawing.Color.White;
            this.btnLuuMatKhau.HoverState.FillColor = System.Drawing.Color.DarkGreen;
            this.btnLuuMatKhau.Location = new System.Drawing.Point(38, 554);
            this.btnLuuMatKhau.Name = "btnLuuMatKhau";
            this.btnLuuMatKhau.Size = new System.Drawing.Size(330, 46);
            this.btnLuuMatKhau.TabIndex = 3;
            this.btnLuuMatKhau.Text = "💾  Lưu Mật Khẩu Mới";
            // 
            // btnHuy
            // 
            this.btnHuy.BorderRadius = 20;
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnHuy.HoverState.FillColor = System.Drawing.Color.Silver;
            this.btnHuy.Location = new System.Drawing.Point(394, 554);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(94, 46);
            this.btnHuy.TabIndex = 4;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click_1);
            // 
            // pnlError
            // 
            this.pnlError.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.pnlError.BorderRadius = 8;
            this.pnlError.Controls.Add(this.lblError);
            this.pnlError.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.pnlError.Location = new System.Drawing.Point(24, 498);
            this.pnlError.Name = "pnlError";
            this.pnlError.Size = new System.Drawing.Size(432, 40);
            this.pnlError.TabIndex = 9;
            this.pnlError.Visible = false;
            // 
            // lblError
            // 
            this.lblError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblError.Location = new System.Drawing.Point(0, 0);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(432, 40);
            this.lblError.TabIndex = 0;
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // elipse
            // 
            this.elipse.BorderRadius = 14;
            this.elipse.TargetControl = this;
            // 
            // dragControl
            // 
            this.dragControl.DockIndicatorTransparencyValue = 0.6D;
            this.dragControl.TargetControl = this.pnlTitleBar;
            this.dragControl.UseTransparentDrag = true;
            // 
            // ChangePasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(526, 684);
            this.Controls.Add(this.pnlTitleBar);
            this.Controls.Add(this.pnlWarning);
            this.Controls.Add(this.lblMatKhauHienTai);
            this.Controls.Add(this.txtMatKhauHienTai);
            this.Controls.Add(this.lblMatKhauMoi);
            this.Controls.Add(this.txtMatKhauMoi);
            this.Controls.Add(this.lblDoManh);
            this.Controls.Add(this.lblDoManhGiaTri);
            this.Controls.Add(this.barDoManh);
            this.Controls.Add(this.lblYeuCau);
            this.Controls.Add(this.lblXacNhan);
            this.Controls.Add(this.txtXacNhan);
            this.Controls.Add(this.btnLuuMatKhau);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.pnlError);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangePasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DermaSoft — Đổi Mật Khẩu";
            this.pnlTitleBar.ResumeLayout(false);
            this.pnlTitleBar.PerformLayout();
            this.pnlWarning.ResumeLayout(false);
            this.pnlError.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
