namespace DermaSoft.Forms
{
    partial class TiepNhanForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
<<<<<<< HEAD
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tlpLeft = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTimKiem = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlTimKiemContent = new System.Windows.Forms.Panel();
            this.pnlBNResult = new Guna.UI2.WinForms.Guna2Panel();
            this.tlpBNInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lblBNTen = new System.Windows.Forms.Label();
            this.lblBNInfo = new System.Windows.Forms.Label();
            this.lblBNTienSu = new System.Windows.Forms.Label();
            this.pnlBNHang = new System.Windows.Forms.Panel();
            this.lblBNHang = new System.Windows.Forms.Label();
            this.pnlBNAvatar = new System.Windows.Forms.Panel();
            this.picAvatarBN = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.pnlSearchBar = new System.Windows.Forms.Panel();
            this.txtTimKiem = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnBNMoi = new Guna.UI2.WinForms.Guna2Button();
            this.btnTim = new Guna.UI2.WinForms.Guna2GradientButton();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTimKiem = new System.Windows.Forms.Label();
            this.pnlQueueBN = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvQueue = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colSTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBenhNhanQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGioTiepNhan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBacSiQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThaiQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblQueueBN = new System.Windows.Forms.Label();
            this.pnlRight = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlRightFields = new System.Windows.Forms.Panel();
            this.pnlFieldTrieuChung = new System.Windows.Forms.Panel();
            this.txtTrieuChung = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlFieldTrieuChun = new System.Windows.Forms.Label();
            this.pnlFieldGhiChu = new System.Windows.Forms.Panel();
            this.txtGhiChu = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.pnlFieldLichHen = new System.Windows.Forms.Panel();
            this.cmbLichHen = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblLichHen = new System.Windows.Forms.Label();
            this.pnlFieldBacSi = new System.Windows.Forms.Panel();
            this.cmbBacSi = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblBacSi = new System.Windows.Forms.Label();
            this.pnlFieldBenhNhan = new System.Windows.Forms.Panel();
            this.txtBenhNhanDisplay = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblBenhNhan = new System.Windows.Forms.Label();
            this.tlpRightButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnTiepNhan = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnHuy = new Guna.UI2.WinForms.Guna2Button();
            this.lblRightHeader = new System.Windows.Forms.Label();
            this.tlpMain.SuspendLayout();
            this.tlpLeft.SuspendLayout();
            this.pnlTimKiem.SuspendLayout();
            this.pnlTimKiemContent.SuspendLayout();
            this.pnlBNResult.SuspendLayout();
            this.tlpBNInfo.SuspendLayout();
            this.pnlBNHang.SuspendLayout();
            this.pnlBNAvatar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatarBN)).BeginInit();
            this.pnlSearchBar.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlQueueBN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.pnlRightFields.SuspendLayout();
            this.pnlFieldTrieuChung.SuspendLayout();
            this.pnlFieldGhiChu.SuspendLayout();
            this.pnlFieldLichHen.SuspendLayout();
            this.pnlFieldBacSi.SuspendLayout();
            this.pnlFieldBenhNhan.SuspendLayout();
            this.tlpRightButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpMain.Controls.Add(this.tlpLeft, 0, 0);
            this.tlpMain.Controls.Add(this.pnlRight, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(20, 20);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(1060, 680);
            this.tlpMain.TabIndex = 0;
            // 
            // tlpLeft
            // 
            this.tlpLeft.ColumnCount = 1;
            this.tlpLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLeft.Controls.Add(this.pnlTimKiem, 0, 0);
            this.tlpLeft.Controls.Add(this.pnlQueueBN, 0, 1);
            this.tlpLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLeft.Location = new System.Drawing.Point(0, 0);
            this.tlpLeft.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.tlpLeft.Name = "tlpLeft";
            this.tlpLeft.RowCount = 2;
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLeft.Size = new System.Drawing.Size(677, 680);
            this.tlpLeft.TabIndex = 0;
            // 
            // pnlTimKiem
            // 
            this.pnlTimKiem.BackColor = System.Drawing.Color.Transparent;
            this.pnlTimKiem.BorderRadius = 12;
            this.pnlTimKiem.Controls.Add(this.pnlTimKiemContent);
            this.pnlTimKiem.Controls.Add(this.pnlSearchBar);
            this.pnlTimKiem.Controls.Add(this.pnlHeader);
            this.pnlTimKiem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTimKiem.FillColor = System.Drawing.Color.White;
            this.pnlTimKiem.Location = new System.Drawing.Point(0, 0);
            this.pnlTimKiem.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.pnlTimKiem.Name = "pnlTimKiem";
            this.pnlTimKiem.Padding = new System.Windows.Forms.Padding(16);
            this.pnlTimKiem.ShadowDecoration.BorderRadius = 12;
            this.pnlTimKiem.ShadowDecoration.Color = System.Drawing.Color.Gray;
            this.pnlTimKiem.ShadowDecoration.Depth = 4;
            this.pnlTimKiem.ShadowDecoration.Enabled = true;
            this.pnlTimKiem.Size = new System.Drawing.Size(677, 288);
            this.pnlTimKiem.TabIndex = 0;
            // 
            // pnlTimKiemContent
            // 
            this.pnlTimKiemContent.Controls.Add(this.pnlBNResult);
            this.pnlTimKiemContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTimKiemContent.Location = new System.Drawing.Point(16, 115);
            this.pnlTimKiemContent.Name = "pnlTimKiemContent";
            this.pnlTimKiemContent.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlTimKiemContent.Size = new System.Drawing.Size(645, 157);
            this.pnlTimKiemContent.TabIndex = 2;
            // 
            // pnlBNResult
            // 
            this.pnlBNResult.BorderRadius = 12;
            this.pnlBNResult.Controls.Add(this.tlpBNInfo);
            this.pnlBNResult.Controls.Add(this.pnlBNHang);
            this.pnlBNResult.Controls.Add(this.pnlBNAvatar);
            this.pnlBNResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBNResult.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.pnlBNResult.Location = new System.Drawing.Point(0, 8);
            this.pnlBNResult.Name = "pnlBNResult";
            this.pnlBNResult.Padding = new System.Windows.Forms.Padding(14, 12, 14, 12);
            this.pnlBNResult.Size = new System.Drawing.Size(645, 149);
            this.pnlBNResult.TabIndex = 0;
            this.pnlBNResult.Visible = false;
            // 
            // tlpBNInfo
            // 
            this.tlpBNInfo.ColumnCount = 1;
            this.tlpBNInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBNInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpBNInfo.Controls.Add(this.lblBNTen, 0, 0);
            this.tlpBNInfo.Controls.Add(this.lblBNInfo, 0, 1);
            this.tlpBNInfo.Controls.Add(this.lblBNTienSu, 0, 2);
            this.tlpBNInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBNInfo.Location = new System.Drawing.Point(139, 12);
            this.tlpBNInfo.Name = "tlpBNInfo";
            this.tlpBNInfo.Padding = new System.Windows.Forms.Padding(5);
            this.tlpBNInfo.RowCount = 3;
            this.tlpBNInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpBNInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpBNInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpBNInfo.Size = new System.Drawing.Size(367, 125);
            this.tlpBNInfo.TabIndex = 2;
            // 
            // lblBNTen
            // 
            this.lblBNTen.AutoSize = true;
            this.lblBNTen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBNTen.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBNTen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblBNTen.Location = new System.Drawing.Point(8, 5);
            this.lblBNTen.Name = "lblBNTen";
            this.lblBNTen.Size = new System.Drawing.Size(351, 46);
            this.lblBNTen.TabIndex = 0;
            this.lblBNTen.Text = "Loading...";
            // 
            // lblBNInfo
            // 
            this.lblBNInfo.AutoSize = true;
            this.lblBNInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBNInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBNInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblBNInfo.Location = new System.Drawing.Point(8, 51);
            this.lblBNInfo.Name = "lblBNInfo";
            this.lblBNInfo.Size = new System.Drawing.Size(351, 34);
            this.lblBNInfo.TabIndex = 1;
            this.lblBNInfo.Text = "📞 SĐT • 📅 Ngày sinh • ♀ Giới tính";
            // 
            // lblBNTienSu
            // 
            this.lblBNTienSu.AutoSize = true;
            this.lblBNTienSu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBNTienSu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBNTienSu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(64)))), ((int)(((byte)(14)))));
            this.lblBNTienSu.Location = new System.Drawing.Point(8, 85);
            this.lblBNTienSu.Name = "lblBNTienSu";
            this.lblBNTienSu.Size = new System.Drawing.Size(351, 35);
            this.lblBNTienSu.TabIndex = 2;
            this.lblBNTienSu.Text = "⚠️ Dị ứng: ...";
            // 
            // pnlBNHang
            // 
            this.pnlBNHang.Controls.Add(this.lblBNHang);
            this.pnlBNHang.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlBNHang.Location = new System.Drawing.Point(506, 12);
            this.pnlBNHang.Name = "pnlBNHang";
            this.pnlBNHang.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.pnlBNHang.Size = new System.Drawing.Size(125, 125);
            this.pnlBNHang.TabIndex = 1;
            // 
            // lblBNHang
            // 
            this.lblBNHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBNHang.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBNHang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblBNHang.Location = new System.Drawing.Point(5, 10);
            this.lblBNHang.Name = "lblBNHang";
            this.lblBNHang.Size = new System.Drawing.Size(115, 105);
            this.lblBNHang.TabIndex = 0;
            this.lblBNHang.Text = "Loading...";
            this.lblBNHang.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlBNAvatar
            // 
            this.pnlBNAvatar.Controls.Add(this.picAvatarBN);
            this.pnlBNAvatar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBNAvatar.Location = new System.Drawing.Point(14, 12);
            this.pnlBNAvatar.Name = "pnlBNAvatar";
            this.pnlBNAvatar.Padding = new System.Windows.Forms.Padding(5, 15, 25, 15);
            this.pnlBNAvatar.Size = new System.Drawing.Size(125, 125);
            this.pnlBNAvatar.TabIndex = 0;
            // 
            // picAvatarBN
            // 
            this.picAvatarBN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picAvatarBN.ImageRotate = 0F;
            this.picAvatarBN.Location = new System.Drawing.Point(5, 15);
            this.picAvatarBN.Name = "picAvatarBN";
            this.picAvatarBN.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.picAvatarBN.Size = new System.Drawing.Size(95, 95);
            this.picAvatarBN.TabIndex = 0;
            this.picAvatarBN.TabStop = false;
            // 
            // pnlSearchBar
            // 
            this.pnlSearchBar.Controls.Add(this.txtTimKiem);
            this.pnlSearchBar.Controls.Add(this.pnlButtons);
            this.pnlSearchBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearchBar.Location = new System.Drawing.Point(16, 50);
            this.pnlSearchBar.Name = "pnlSearchBar";
            this.pnlSearchBar.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.pnlSearchBar.Size = new System.Drawing.Size(645, 65);
            this.pnlSearchBar.TabIndex = 1;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.txtTimKiem.BorderRadius = 20;
            this.txtTimKiem.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimKiem.DefaultText = "";
            this.txtTimKiem.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTimKiem.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTimKiem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTimKiem.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTimKiem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTimKiem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTimKiem.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTimKiem.Location = new System.Drawing.Point(0, 8);
            this.txtTimKiem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "Nhập SĐT...";
            this.txtTimKiem.SelectedText = "";
            this.txtTimKiem.Size = new System.Drawing.Size(384, 49);
            this.txtTimKiem.TabIndex = 1;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnBNMoi);
            this.pnlButtons.Controls.Add(this.btnTim);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Location = new System.Drawing.Point(384, 8);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.pnlButtons.Size = new System.Drawing.Size(261, 49);
            this.pnlButtons.TabIndex = 0;
            // 
            // btnBNMoi
            // 
            this.btnBNMoi.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnBNMoi.BorderRadius = 20;
            this.btnBNMoi.BorderThickness = 1;
            this.btnBNMoi.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBNMoi.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnBNMoi.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnBNMoi.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnBNMoi.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBNMoi.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.btnBNMoi.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBNMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnBNMoi.Location = new System.Drawing.Point(131, 0);
            this.btnBNMoi.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.btnBNMoi.Name = "btnBNMoi";
            this.btnBNMoi.Size = new System.Drawing.Size(110, 49);
            this.btnBNMoi.TabIndex = 1;
            this.btnBNMoi.Text = "BN Mới";
            // 
            // btnTim
            // 
            this.btnTim.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnTim.BorderRadius = 20;
            this.btnTim.BorderThickness = 1;
            this.btnTim.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTim.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTim.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTim.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTim.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTim.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTim.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnTim.FillColor2 = System.Drawing.Color.SeaGreen;
            this.btnTim.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTim.ForeColor = System.Drawing.Color.White;
            this.btnTim.Location = new System.Drawing.Point(20, 0);
            this.btnTim.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.btnTim.Name = "btnTim";
            this.btnTim.Size = new System.Drawing.Size(80, 49);
            this.btnTim.TabIndex = 0;
            this.btnTim.Text = "Tìm";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblTimKiem);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(16, 16);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(645, 34);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTimKiem
            // 
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimKiem.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTimKiem.Location = new System.Drawing.Point(0, 0);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(179, 23);
            this.lblTimKiem.TabIndex = 0;
            this.lblTimKiem.Text = "Tìm Kiếm Bệnh Nhân";
            // 
            // pnlQueueBN
            // 
            this.pnlQueueBN.BackColor = System.Drawing.Color.Transparent;
            this.pnlQueueBN.BorderRadius = 12;
            this.pnlQueueBN.Controls.Add(this.dgvQueue);
            this.pnlQueueBN.Controls.Add(this.lblQueueBN);
            this.pnlQueueBN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQueueBN.FillColor = System.Drawing.Color.White;
            this.pnlQueueBN.Location = new System.Drawing.Point(3, 303);
            this.pnlQueueBN.Name = "pnlQueueBN";
            this.pnlQueueBN.Padding = new System.Windows.Forms.Padding(16);
            this.pnlQueueBN.ShadowDecoration.BorderRadius = 12;
            this.pnlQueueBN.ShadowDecoration.Color = System.Drawing.Color.Gray;
            this.pnlQueueBN.ShadowDecoration.Depth = 4;
            this.pnlQueueBN.ShadowDecoration.Enabled = true;
            this.pnlQueueBN.Size = new System.Drawing.Size(671, 374);
            this.pnlQueueBN.TabIndex = 1;
            // 
            // dgvQueue
            // 
            this.dgvQueue.AllowUserToAddRows = false;
            this.dgvQueue.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvQueue.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvQueue.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvQueue.ColumnHeadersHeight = 42;
            this.dgvQueue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSTT,
            this.colBenhNhanQ,
            this.colGioTiepNhan,
            this.colBacSiQ,
            this.colTrangThaiQ});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvQueue.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvQueue.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvQueue.Location = new System.Drawing.Point(16, 51);
            this.dgvQueue.Name = "dgvQueue";
            this.dgvQueue.ReadOnly = true;
            this.dgvQueue.RowHeadersVisible = false;
            this.dgvQueue.RowHeadersWidth = 51;
            this.dgvQueue.RowTemplate.Height = 42;
            this.dgvQueue.Size = new System.Drawing.Size(639, 307);
            this.dgvQueue.TabIndex = 1;
            this.dgvQueue.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvQueue.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvQueue.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvQueue.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvQueue.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvQueue.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvQueue.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvQueue.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.dgvQueue.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvQueue.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvQueue.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvQueue.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvQueue.ThemeStyle.HeaderStyle.Height = 42;
            this.dgvQueue.ThemeStyle.ReadOnly = true;
            this.dgvQueue.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvQueue.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvQueue.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvQueue.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvQueue.ThemeStyle.RowsStyle.Height = 42;
            this.dgvQueue.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.dgvQueue.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            // 
            // colSTT
            // 
            this.colSTT.DataPropertyName = "STT";
            this.colSTT.FillWeight = 40F;
            this.colSTT.HeaderText = "STT";
            this.colSTT.MinimumWidth = 6;
            this.colSTT.Name = "colSTT";
            this.colSTT.ReadOnly = true;
            // 
            // colBenhNhanQ
            // 
            this.colBenhNhanQ.DataPropertyName = "HoTen";
            this.colBenhNhanQ.FillWeight = 175F;
            this.colBenhNhanQ.HeaderText = "Bệnh nhân";
            this.colBenhNhanQ.MinimumWidth = 6;
            this.colBenhNhanQ.Name = "colBenhNhanQ";
            this.colBenhNhanQ.ReadOnly = true;
            // 
            // colGioTiepNhan
            // 
            this.colGioTiepNhan.DataPropertyName = "GioTiepNhan";
            this.colGioTiepNhan.HeaderText = "Giờ tiếp nhận";
            this.colGioTiepNhan.MinimumWidth = 6;
            this.colGioTiepNhan.Name = "colGioTiepNhan";
            this.colGioTiepNhan.ReadOnly = true;
            // 
            // colBacSiQ
            // 
            this.colBacSiQ.DataPropertyName = "TenBacSi";
            this.colBacSiQ.FillWeight = 130F;
            this.colBacSiQ.HeaderText = "Bác sĩ";
            this.colBacSiQ.MinimumWidth = 6;
            this.colBacSiQ.Name = "colBacSiQ";
            this.colBacSiQ.ReadOnly = true;
            // 
            // colTrangThaiQ
            // 
            this.colTrangThaiQ.DataPropertyName = "TrangThaiText";
            this.colTrangThaiQ.FillWeight = 110F;
            this.colTrangThaiQ.HeaderText = "Trạng thái";
            this.colTrangThaiQ.MinimumWidth = 6;
            this.colTrangThaiQ.Name = "colTrangThaiQ";
            this.colTrangThaiQ.ReadOnly = true;
            // 
            // lblQueueBN
            // 
            this.lblQueueBN.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblQueueBN.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQueueBN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblQueueBN.Location = new System.Drawing.Point(16, 16);
            this.lblQueueBN.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblQueueBN.Name = "lblQueueBN";
            this.lblQueueBN.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblQueueBN.Size = new System.Drawing.Size(639, 35);
            this.lblQueueBN.TabIndex = 0;
            this.lblQueueBN.Text = "📋 Queue Bệnh Nhân Hôm Nay";
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.Transparent;
            this.pnlRight.BorderRadius = 12;
            this.pnlRight.Controls.Add(this.pnlRightFields);
            this.pnlRight.Controls.Add(this.tlpRightButtons);
            this.pnlRight.Controls.Add(this.lblRightHeader);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.FillColor = System.Drawing.Color.White;
            this.pnlRight.Location = new System.Drawing.Point(692, 3);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(15);
            this.pnlRight.ShadowDecoration.BorderRadius = 12;
            this.pnlRight.ShadowDecoration.Color = System.Drawing.Color.Gray;
            this.pnlRight.ShadowDecoration.Depth = 4;
            this.pnlRight.ShadowDecoration.Enabled = true;
            this.pnlRight.Size = new System.Drawing.Size(365, 674);
            this.pnlRight.TabIndex = 1;
            // 
            // pnlRightFields
            // 
            this.pnlRightFields.Controls.Add(this.pnlFieldTrieuChung);
            this.pnlRightFields.Controls.Add(this.pnlFieldGhiChu);
            this.pnlRightFields.Controls.Add(this.pnlFieldLichHen);
            this.pnlRightFields.Controls.Add(this.pnlFieldBacSi);
            this.pnlRightFields.Controls.Add(this.pnlFieldBenhNhan);
            this.pnlRightFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRightFields.Location = new System.Drawing.Point(15, 50);
            this.pnlRightFields.Name = "pnlRightFields";
            this.pnlRightFields.Size = new System.Drawing.Size(335, 471);
            this.pnlRightFields.TabIndex = 2;
            // 
            // pnlFieldTrieuChung
            // 
            this.pnlFieldTrieuChung.Controls.Add(this.txtTrieuChung);
            this.pnlFieldTrieuChung.Controls.Add(this.pnlFieldTrieuChun);
            this.pnlFieldTrieuChung.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFieldTrieuChung.Location = new System.Drawing.Point(0, 240);
            this.pnlFieldTrieuChung.Name = "pnlFieldTrieuChung";
            this.pnlFieldTrieuChung.Padding = new System.Windows.Forms.Padding(5, 5, 5, 10);
            this.pnlFieldTrieuChung.Size = new System.Drawing.Size(335, 128);
            this.pnlFieldTrieuChung.TabIndex = 4;
            // 
            // txtTrieuChung
            // 
            this.txtTrieuChung.BorderColor = System.Drawing.Color.SeaGreen;
            this.txtTrieuChung.BorderRadius = 10;
            this.txtTrieuChung.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTrieuChung.DefaultText = "";
            this.txtTrieuChung.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTrieuChung.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTrieuChung.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTrieuChung.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTrieuChung.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTrieuChung.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTrieuChung.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTrieuChung.ForeColor = System.Drawing.Color.Black;
            this.txtTrieuChung.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTrieuChung.Location = new System.Drawing.Point(5, 30);
            this.txtTrieuChung.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTrieuChung.Multiline = true;
            this.txtTrieuChung.Name = "txtTrieuChung";
            this.txtTrieuChung.PlaceholderText = "";
            this.txtTrieuChung.SelectedText = "";
            this.txtTrieuChung.Size = new System.Drawing.Size(325, 88);
            this.txtTrieuChung.TabIndex = 2;
            // 
            // pnlFieldTrieuChun
            // 
            this.pnlFieldTrieuChun.AutoSize = true;
            this.pnlFieldTrieuChun.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFieldTrieuChun.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlFieldTrieuChun.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.pnlFieldTrieuChun.Location = new System.Drawing.Point(5, 5);
            this.pnlFieldTrieuChun.Name = "pnlFieldTrieuChun";
            this.pnlFieldTrieuChun.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.pnlFieldTrieuChun.Size = new System.Drawing.Size(152, 25);
            this.pnlFieldTrieuChun.TabIndex = 1;
            this.pnlFieldTrieuChun.Text = "Triệu chứng ban đầu";
            this.pnlFieldTrieuChun.Click += new System.EventHandler(this.label5_Click);
            // 
            // pnlFieldGhiChu
            // 
            this.pnlFieldGhiChu.Controls.Add(this.txtGhiChu);
            this.pnlFieldGhiChu.Controls.Add(this.lblGhiChu);
            this.pnlFieldGhiChu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFieldGhiChu.Location = new System.Drawing.Point(0, 368);
            this.pnlFieldGhiChu.Name = "pnlFieldGhiChu";
            this.pnlFieldGhiChu.Padding = new System.Windows.Forms.Padding(5, 5, 5, 10);
            this.pnlFieldGhiChu.Size = new System.Drawing.Size(335, 103);
            this.pnlFieldGhiChu.TabIndex = 3;
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.BorderColor = System.Drawing.Color.SeaGreen;
            this.txtGhiChu.BorderRadius = 10;
            this.txtGhiChu.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGhiChu.DefaultText = "";
            this.txtGhiChu.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtGhiChu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtGhiChu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtGhiChu.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtGhiChu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGhiChu.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtGhiChu.ForeColor = System.Drawing.Color.Black;
            this.txtGhiChu.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtGhiChu.Location = new System.Drawing.Point(5, 30);
            this.txtGhiChu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGhiChu.Multiline = true;
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.PlaceholderText = "";
            this.txtGhiChu.SelectedText = "";
            this.txtGhiChu.Size = new System.Drawing.Size(325, 63);
            this.txtGhiChu.TabIndex = 2;
            // 
            // lblGhiChu
            // 
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGhiChu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblGhiChu.Location = new System.Drawing.Point(5, 5);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblGhiChu.Size = new System.Drawing.Size(103, 25);
            this.lblGhiChu.TabIndex = 1;
            this.lblGhiChu.Text = "Ghi chú thêm";
            this.lblGhiChu.Click += new System.EventHandler(this.label4_Click);
            // 
            // pnlFieldLichHen
            // 
            this.pnlFieldLichHen.Controls.Add(this.cmbLichHen);
            this.pnlFieldLichHen.Controls.Add(this.lblLichHen);
            this.pnlFieldLichHen.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFieldLichHen.Location = new System.Drawing.Point(0, 160);
            this.pnlFieldLichHen.Name = "pnlFieldLichHen";
            this.pnlFieldLichHen.Padding = new System.Windows.Forms.Padding(5, 5, 5, 10);
            this.pnlFieldLichHen.Size = new System.Drawing.Size(335, 80);
            this.pnlFieldLichHen.TabIndex = 2;
            // 
            // cmbLichHen
            // 
            this.cmbLichHen.BackColor = System.Drawing.Color.Transparent;
            this.cmbLichHen.BorderColor = System.Drawing.Color.SeaGreen;
            this.cmbLichHen.BorderRadius = 10;
            this.cmbLichHen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbLichHen.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbLichHen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLichHen.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbLichHen.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbLichHen.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbLichHen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbLichHen.ItemHeight = 30;
            this.cmbLichHen.Location = new System.Drawing.Point(5, 30);
            this.cmbLichHen.Name = "cmbLichHen";
            this.cmbLichHen.Size = new System.Drawing.Size(325, 36);
            this.cmbLichHen.TabIndex = 2;
            // 
            // lblLichHen
            // 
            this.lblLichHen.AutoSize = true;
            this.lblLichHen.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLichHen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLichHen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblLichHen.Location = new System.Drawing.Point(5, 5);
            this.lblLichHen.Name = "lblLichHen";
            this.lblLichHen.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblLichHen.Size = new System.Drawing.Size(122, 25);
            this.lblLichHen.TabIndex = 1;
            this.lblLichHen.Text = "Lịch hẹn liên kết";
            // 
            // pnlFieldBacSi
            // 
            this.pnlFieldBacSi.Controls.Add(this.cmbBacSi);
            this.pnlFieldBacSi.Controls.Add(this.lblBacSi);
            this.pnlFieldBacSi.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFieldBacSi.Location = new System.Drawing.Point(0, 80);
            this.pnlFieldBacSi.Name = "pnlFieldBacSi";
            this.pnlFieldBacSi.Padding = new System.Windows.Forms.Padding(5, 5, 5, 10);
            this.pnlFieldBacSi.Size = new System.Drawing.Size(335, 80);
            this.pnlFieldBacSi.TabIndex = 1;
            // 
            // cmbBacSi
            // 
            this.cmbBacSi.BackColor = System.Drawing.Color.Transparent;
            this.cmbBacSi.BorderColor = System.Drawing.Color.SeaGreen;
            this.cmbBacSi.BorderRadius = 10;
            this.cmbBacSi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBacSi.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbBacSi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBacSi.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbBacSi.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbBacSi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbBacSi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbBacSi.ItemHeight = 30;
            this.cmbBacSi.Location = new System.Drawing.Point(5, 30);
            this.cmbBacSi.Name = "cmbBacSi";
            this.cmbBacSi.Size = new System.Drawing.Size(325, 36);
            this.cmbBacSi.TabIndex = 3;
            // 
            // lblBacSi
            // 
            this.lblBacSi.AutoSize = true;
            this.lblBacSi.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBacSi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBacSi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblBacSi.Location = new System.Drawing.Point(5, 5);
            this.lblBacSi.Name = "lblBacSi";
            this.lblBacSi.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblBacSi.Size = new System.Drawing.Size(120, 25);
            this.lblBacSi.TabIndex = 1;
            this.lblBacSi.Text = "Bác sĩ phụ trách";
            // 
            // pnlFieldBenhNhan
            // 
            this.pnlFieldBenhNhan.Controls.Add(this.txtBenhNhanDisplay);
            this.pnlFieldBenhNhan.Controls.Add(this.lblBenhNhan);
            this.pnlFieldBenhNhan.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFieldBenhNhan.Location = new System.Drawing.Point(0, 0);
            this.pnlFieldBenhNhan.Name = "pnlFieldBenhNhan";
            this.pnlFieldBenhNhan.Padding = new System.Windows.Forms.Padding(5, 5, 5, 10);
            this.pnlFieldBenhNhan.Size = new System.Drawing.Size(335, 80);
            this.pnlFieldBenhNhan.TabIndex = 0;
            // 
            // txtBenhNhanDisplay
            // 
            this.txtBenhNhanDisplay.BorderColor = System.Drawing.Color.SeaGreen;
            this.txtBenhNhanDisplay.BorderRadius = 10;
            this.txtBenhNhanDisplay.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBenhNhanDisplay.DefaultText = "";
            this.txtBenhNhanDisplay.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtBenhNhanDisplay.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtBenhNhanDisplay.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBenhNhanDisplay.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBenhNhanDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBenhNhanDisplay.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBenhNhanDisplay.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtBenhNhanDisplay.ForeColor = System.Drawing.Color.Black;
            this.txtBenhNhanDisplay.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBenhNhanDisplay.Location = new System.Drawing.Point(5, 30);
            this.txtBenhNhanDisplay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBenhNhanDisplay.Name = "txtBenhNhanDisplay";
            this.txtBenhNhanDisplay.PlaceholderText = "";
            this.txtBenhNhanDisplay.ReadOnly = true;
            this.txtBenhNhanDisplay.SelectedText = "";
            this.txtBenhNhanDisplay.Size = new System.Drawing.Size(325, 40);
            this.txtBenhNhanDisplay.TabIndex = 1;
            // 
            // lblBenhNhan
            // 
            this.lblBenhNhan.AutoSize = true;
            this.lblBenhNhan.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBenhNhan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBenhNhan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblBenhNhan.Location = new System.Drawing.Point(5, 5);
            this.lblBenhNhan.Name = "lblBenhNhan";
            this.lblBenhNhan.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblBenhNhan.Size = new System.Drawing.Size(84, 25);
            this.lblBenhNhan.TabIndex = 0;
            this.lblBenhNhan.Text = "Bệnh nhân";
            // 
            // tlpRightButtons
            // 
            this.tlpRightButtons.ColumnCount = 1;
            this.tlpRightButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRightButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRightButtons.Controls.Add(this.btnTiepNhan, 0, 0);
            this.tlpRightButtons.Controls.Add(this.btnHuy, 0, 1);
            this.tlpRightButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tlpRightButtons.Location = new System.Drawing.Point(15, 521);
            this.tlpRightButtons.Name = "tlpRightButtons";
            this.tlpRightButtons.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.tlpRightButtons.RowCount = 2;
            this.tlpRightButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRightButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRightButtons.Size = new System.Drawing.Size(335, 138);
            this.tlpRightButtons.TabIndex = 1;
            this.tlpRightButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // btnTiepNhan
            // 
            this.btnTiepNhan.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnTiepNhan.BorderRadius = 18;
            this.btnTiepNhan.BorderThickness = 1;
            this.btnTiepNhan.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTiepNhan.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTiepNhan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTiepNhan.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTiepNhan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTiepNhan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTiepNhan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnTiepNhan.FillColor2 = System.Drawing.Color.SeaGreen;
            this.btnTiepNhan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTiepNhan.ForeColor = System.Drawing.Color.White;
            this.btnTiepNhan.Location = new System.Drawing.Point(13, 8);
            this.btnTiepNhan.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.btnTiepNhan.Name = "btnTiepNhan";
            this.btnTiepNhan.Size = new System.Drawing.Size(309, 53);
            this.btnTiepNhan.TabIndex = 0;
            this.btnTiepNhan.Text = "Tiếp Nhận và Tạo Phiếu Khám";
            this.btnTiepNhan.Click += new System.EventHandler(this.guna2GradientButton1_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnHuy.BorderRadius = 18;
            this.btnHuy.BorderThickness = 1;
            this.btnHuy.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnHuy.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnHuy.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHuy.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHuy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHuy.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnHuy.Location = new System.Drawing.Point(13, 77);
            this.btnHuy.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(309, 53);
            this.btnHuy.TabIndex = 1;
            this.btnHuy.Text = "Hủy";
            // 
            // lblRightHeader
            // 
            this.lblRightHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRightHeader.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRightHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblRightHeader.Location = new System.Drawing.Point(15, 15);
            this.lblRightHeader.Name = "lblRightHeader";
            this.lblRightHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblRightHeader.Size = new System.Drawing.Size(335, 35);
            this.lblRightHeader.TabIndex = 0;
            this.lblRightHeader.Text = "📋 Tạo Phiếu Khám Mới";
            this.lblRightHeader.Click += new System.EventHandler(this.label2_Click);
            // 
            // TiepNhanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1100, 720);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TiepNhanForm";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.Text = "Tiếp Nhận Bệnh Nhân";
            this.tlpMain.ResumeLayout(false);
            this.tlpLeft.ResumeLayout(false);
            this.pnlTimKiem.ResumeLayout(false);
            this.pnlTimKiemContent.ResumeLayout(false);
            this.pnlBNResult.ResumeLayout(false);
            this.tlpBNInfo.ResumeLayout(false);
            this.tlpBNInfo.PerformLayout();
            this.pnlBNHang.ResumeLayout(false);
            this.pnlBNAvatar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAvatarBN)).EndInit();
            this.pnlSearchBar.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlQueueBN.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.pnlRightFields.ResumeLayout(false);
            this.pnlFieldTrieuChung.ResumeLayout(false);
            this.pnlFieldTrieuChung.PerformLayout();
            this.pnlFieldGhiChu.ResumeLayout(false);
            this.pnlFieldGhiChu.PerformLayout();
            this.pnlFieldLichHen.ResumeLayout(false);
            this.pnlFieldLichHen.PerformLayout();
            this.pnlFieldBacSi.ResumeLayout(false);
            this.pnlFieldBacSi.PerformLayout();
            this.pnlFieldBenhNhan.ResumeLayout(false);
            this.pnlFieldBenhNhan.PerformLayout();
            this.tlpRightButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpLeft;
        private Guna.UI2.WinForms.Guna2Panel pnlTimKiem;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.Panel pnlSearchBar;
        private System.Windows.Forms.Panel pnlTimKiemContent;
        private Guna.UI2.WinForms.Guna2TextBox txtTimKiem;
        private System.Windows.Forms.Panel pnlButtons;
        private Guna.UI2.WinForms.Guna2Button btnBNMoi;
        private Guna.UI2.WinForms.Guna2GradientButton btnTim;
        private Guna.UI2.WinForms.Guna2Panel pnlBNResult;
        private System.Windows.Forms.Panel pnlBNHang;
        private System.Windows.Forms.Panel pnlBNAvatar;
        private System.Windows.Forms.TableLayoutPanel tlpBNInfo;
        private Guna.UI2.WinForms.Guna2CirclePictureBox picAvatarBN;
        private System.Windows.Forms.Label lblBNHang;
        private System.Windows.Forms.Label lblBNTen;
        private System.Windows.Forms.Label lblBNInfo;
        private System.Windows.Forms.Label lblBNTienSu;
        private Guna.UI2.WinForms.Guna2Panel pnlQueueBN;
        private Guna.UI2.WinForms.Guna2DataGridView dgvQueue;
        private System.Windows.Forms.Label lblQueueBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSTT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBenhNhanQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGioTiepNhan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBacSiQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThaiQ;
        private Guna.UI2.WinForms.Guna2Panel pnlRight;
        private System.Windows.Forms.Label lblRightHeader;
        private System.Windows.Forms.TableLayoutPanel tlpRightButtons;
        private System.Windows.Forms.Panel pnlRightFields;
        private System.Windows.Forms.Panel pnlFieldBenhNhan;
        private System.Windows.Forms.Panel pnlFieldLichHen;
        private System.Windows.Forms.Panel pnlFieldBacSi;
        private System.Windows.Forms.Label lblBenhNhan;
        private System.Windows.Forms.Label lblLichHen;
        private System.Windows.Forms.Label lblBacSi;
        private System.Windows.Forms.Panel pnlFieldTrieuChung;
        private System.Windows.Forms.Label pnlFieldTrieuChun;
        private System.Windows.Forms.Panel pnlFieldGhiChu;
        private System.Windows.Forms.Label lblGhiChu;
        private Guna.UI2.WinForms.Guna2TextBox txtBenhNhanDisplay;
        private Guna.UI2.WinForms.Guna2TextBox txtTrieuChung;
        private Guna.UI2.WinForms.Guna2TextBox txtGhiChu;
        private Guna.UI2.WinForms.Guna2ComboBox cmbLichHen;
        private Guna.UI2.WinForms.Guna2ComboBox cmbBacSi;
        private Guna.UI2.WinForms.Guna2GradientButton btnTiepNhan;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
=======
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 720);
            this.Name = "TiepNhanForm";
            this.Text = "Tiếp Nhận Bệnh Nhân";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }
>>>>>>> d2fc9d190a76c0c366e0407bca6067fe95379af1
    }
}
