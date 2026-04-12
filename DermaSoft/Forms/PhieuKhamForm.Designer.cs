namespace DermaSoft.Forms
{
    partial class PhieuKhamForm
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlBenhNhanInfo = new System.Windows.Forms.Panel();
            this.pnlBNText = new System.Windows.Forms.Panel();
            this.lblDiUng = new System.Windows.Forms.Label();
            this.lblThongTinBN = new System.Windows.Forms.Label();
            this.lblTenBN = new System.Windows.Forms.Label();
            this.lblAvatar = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnHoanThanhKham = new Guna.UI2.WinForms.Guna2GradientButton();
            this.pnlSpace = new System.Windows.Forms.Panel();
            this.btnLuuNhap = new Guna.UI2.WinForms.Guna2Button();
            this.pnlTabs = new System.Windows.Forms.Panel();
            this.btnTabGhiChu = new Guna.UI2.WinForms.Guna2Button();
            this.btnTabHinhAnh = new Guna.UI2.WinForms.Guna2Button();
            this.btnTabKeDonThuoc = new Guna.UI2.WinForms.Guna2Button();
            this.btnTabDichVu = new Guna.UI2.WinForms.Guna2Button();
            this.btnTabChanDoan = new Guna.UI2.WinForms.Guna2Button();
            this.pnlDonThuoc = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvDonThuoc = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colTenThuoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonVi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLieuDung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHSD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colXoa = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlAddThuoc = new System.Windows.Forms.Panel();
            this.cboThuoc = new Guna.UI2.WinForms.Guna2ComboBox();
            this.pnlSpace3 = new System.Windows.Forms.Panel();
            this.txtSoLuong = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlSpace2 = new System.Windows.Forms.Panel();
            this.txtLieuDung = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlSpace1 = new System.Windows.Forms.Panel();
            this.btnThemThuoc = new Guna.UI2.WinForms.Guna2GradientButton();
            this.lblTitleDonThuoc = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlDichVu = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlDichVuList = new System.Windows.Forms.Panel();
            this.pnlTongDichVu = new System.Windows.Forms.Panel();
            this.lblTongDichVu = new System.Windows.Forms.Label();
            this.lblTongDVLabel = new System.Windows.Forms.Label();
            this.lblTitleDichVu = new System.Windows.Forms.Label();
            this.pnlThongTinKham = new Guna.UI2.WinForms.Guna2Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboTrangThai = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblTrangThaiLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpNgayTaiKham = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblNgayTaiKham = new System.Windows.Forms.Label();
            this.pnlChanDoan = new System.Windows.Forms.Panel();
            this.txtChanDoan = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblChanDoan = new System.Windows.Forms.Label();
            this.pnlTrieuChung = new System.Windows.Forms.Panel();
            this.txtTrieuChung = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTrieuChung = new System.Windows.Forms.Label();
            this.lblTitleThongTin = new System.Windows.Forms.Label();
            this.pnlSpace5 = new System.Windows.Forms.Panel();
            this.pnlSpace4 = new System.Windows.Forms.Panel();
            this.pnlTop.SuspendLayout();
            this.pnlBenhNhanInfo.SuspendLayout();
            this.pnlBNText.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.pnlTabs.SuspendLayout();
            this.pnlDonThuoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDonThuoc)).BeginInit();
            this.pnlAddThuoc.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.pnlDichVu.SuspendLayout();
            this.pnlTongDichVu.SuspendLayout();
            this.pnlThongTinKham.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlChanDoan.SuspendLayout();
            this.pnlTrieuChung.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.pnlBenhNhanInfo);
            this.pnlTop.Controls.Add(this.pnlButtons);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(10, 10);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.pnlTop.Size = new System.Drawing.Size(1080, 62);
            this.pnlTop.TabIndex = 0;
            // 
            // pnlBenhNhanInfo
            // 
            this.pnlBenhNhanInfo.BackColor = System.Drawing.Color.Transparent;
            this.pnlBenhNhanInfo.Controls.Add(this.pnlBNText);
            this.pnlBenhNhanInfo.Controls.Add(this.lblAvatar);
            this.pnlBenhNhanInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBenhNhanInfo.Location = new System.Drawing.Point(14, 0);
            this.pnlBenhNhanInfo.Name = "pnlBenhNhanInfo";
            this.pnlBenhNhanInfo.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.pnlBenhNhanInfo.Size = new System.Drawing.Size(703, 62);
            this.pnlBenhNhanInfo.TabIndex = 1;
            // 
            // pnlBNText
            // 
            this.pnlBNText.Controls.Add(this.lblDiUng);
            this.pnlBNText.Controls.Add(this.lblThongTinBN);
            this.pnlBNText.Controls.Add(this.lblTenBN);
            this.pnlBNText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBNText.Location = new System.Drawing.Point(44, 9);
            this.pnlBNText.Name = "pnlBNText";
            this.pnlBNText.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.pnlBNText.Size = new System.Drawing.Size(659, 44);
            this.pnlBNText.TabIndex = 1;
            // 
            // lblDiUng
            // 
            this.lblDiUng.AutoSize = true;
            this.lblDiUng.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiUng.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.lblDiUng.Location = new System.Drawing.Point(210, 23);
            this.lblDiUng.Name = "lblDiUng";
            this.lblDiUng.Size = new System.Drawing.Size(250, 20);
            this.lblDiUng.TabIndex = 2;
            this.lblDiUng.Text = "⚠️ Không có tiền sử dị ứng đặc biệt";
            // 
            // lblThongTinBN
            // 
            this.lblThongTinBN.AutoSize = true;
            this.lblThongTinBN.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThongTinBN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblThongTinBN.Location = new System.Drawing.Point(0, 23);
            this.lblThongTinBN.Name = "lblThongTinBN";
            this.lblThongTinBN.Size = new System.Drawing.Size(189, 20);
            this.lblThongTinBN.TabIndex = 1;
            this.lblThongTinBN.Text = "0912345678 · Nam · 40 tuổi";
            // 
            // lblTenBN
            // 
            this.lblTenBN.AutoSize = true;
            this.lblTenBN.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenBN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.lblTenBN.Location = new System.Drawing.Point(0, 2);
            this.lblTenBN.Name = "lblTenBN";
            this.lblTenBN.Size = new System.Drawing.Size(189, 23);
            this.lblTenBN.TabIndex = 0;
            this.lblTenBN.Text = "Trần Văn Bình · BN002";
            // 
            // lblAvatar
            // 
            this.lblAvatar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblAvatar.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblAvatar.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvatar.ForeColor = System.Drawing.Color.White;
            this.lblAvatar.Location = new System.Drawing.Point(0, 9);
            this.lblAvatar.Name = "lblAvatar";
            this.lblAvatar.Size = new System.Drawing.Size(44, 44);
            this.lblAvatar.TabIndex = 0;
            this.lblAvatar.Text = "B";
            this.lblAvatar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
            this.pnlButtons.Controls.Add(this.btnHoanThanhKham);
            this.pnlButtons.Controls.Add(this.pnlSpace);
            this.pnlButtons.Controls.Add(this.btnLuuNhap);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Location = new System.Drawing.Point(717, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(0, 11, 0, 11);
            this.pnlButtons.Size = new System.Drawing.Size(349, 62);
            this.pnlButtons.TabIndex = 0;
            // 
            // btnHoanThanhKham
            // 
            this.btnHoanThanhKham.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnHoanThanhKham.BorderRadius = 15;
            this.btnHoanThanhKham.BorderThickness = 1;
            this.btnHoanThanhKham.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnHoanThanhKham.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnHoanThanhKham.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHoanThanhKham.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHoanThanhKham.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHoanThanhKham.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHoanThanhKham.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnHoanThanhKham.FillColor2 = System.Drawing.Color.SeaGreen;
            this.btnHoanThanhKham.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnHoanThanhKham.ForeColor = System.Drawing.Color.White;
            this.btnHoanThanhKham.Location = new System.Drawing.Point(0, 11);
            this.btnHoanThanhKham.Name = "btnHoanThanhKham";
            this.btnHoanThanhKham.Size = new System.Drawing.Size(212, 40);
            this.btnHoanThanhKham.TabIndex = 2;
            this.btnHoanThanhKham.Text = "Hoàn Thành Khám";
            // 
            // pnlSpace
            // 
            this.pnlSpace.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSpace.Location = new System.Drawing.Point(212, 11);
            this.pnlSpace.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.pnlSpace.Name = "pnlSpace";
            this.pnlSpace.Size = new System.Drawing.Size(24, 40);
            this.pnlSpace.TabIndex = 1;
            this.pnlSpace.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSpace_Paint);
            // 
            // btnLuuNhap
            // 
            this.btnLuuNhap.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnLuuNhap.BorderRadius = 15;
            this.btnLuuNhap.BorderThickness = 1;
            this.btnLuuNhap.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLuuNhap.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLuuNhap.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLuuNhap.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLuuNhap.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLuuNhap.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.btnLuuNhap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuuNhap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnLuuNhap.Location = new System.Drawing.Point(236, 11);
            this.btnLuuNhap.Name = "btnLuuNhap";
            this.btnLuuNhap.Size = new System.Drawing.Size(113, 40);
            this.btnLuuNhap.TabIndex = 0;
            this.btnLuuNhap.Text = "Lưu Nháp";
            // 
            // pnlTabs
            // 
            this.pnlTabs.BackColor = System.Drawing.Color.White;
            this.pnlTabs.Controls.Add(this.btnTabGhiChu);
            this.pnlTabs.Controls.Add(this.btnTabHinhAnh);
            this.pnlTabs.Controls.Add(this.btnTabKeDonThuoc);
            this.pnlTabs.Controls.Add(this.btnTabDichVu);
            this.pnlTabs.Controls.Add(this.btnTabChanDoan);
            this.pnlTabs.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTabs.Location = new System.Drawing.Point(10, 72);
            this.pnlTabs.Name = "pnlTabs";
            this.pnlTabs.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.pnlTabs.Size = new System.Drawing.Size(1080, 46);
            this.pnlTabs.TabIndex = 1;
            this.pnlTabs.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTabs_Paint);
            // 
            // btnTabGhiChu
            // 
            this.btnTabGhiChu.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTabGhiChu.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTabGhiChu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTabGhiChu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTabGhiChu.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTabGhiChu.FillColor = System.Drawing.Color.Transparent;
            this.btnTabGhiChu.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTabGhiChu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnTabGhiChu.Location = new System.Drawing.Point(544, 0);
            this.btnTabGhiChu.Name = "btnTabGhiChu";
            this.btnTabGhiChu.Size = new System.Drawing.Size(110, 46);
            this.btnTabGhiChu.TabIndex = 4;
            this.btnTabGhiChu.Text = "📝 Ghi Chú";
            // 
            // btnTabHinhAnh
            // 
            this.btnTabHinhAnh.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTabHinhAnh.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTabHinhAnh.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTabHinhAnh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTabHinhAnh.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTabHinhAnh.FillColor = System.Drawing.Color.Transparent;
            this.btnTabHinhAnh.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTabHinhAnh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnTabHinhAnh.Location = new System.Drawing.Point(424, 0);
            this.btnTabHinhAnh.Name = "btnTabHinhAnh";
            this.btnTabHinhAnh.Size = new System.Drawing.Size(120, 46);
            this.btnTabHinhAnh.TabIndex = 3;
            this.btnTabHinhAnh.Text = "🖼️ Hình Ảnh";
            // 
            // btnTabKeDonThuoc
            // 
            this.btnTabKeDonThuoc.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTabKeDonThuoc.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTabKeDonThuoc.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTabKeDonThuoc.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTabKeDonThuoc.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTabKeDonThuoc.FillColor = System.Drawing.Color.Transparent;
            this.btnTabKeDonThuoc.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTabKeDonThuoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnTabKeDonThuoc.Location = new System.Drawing.Point(264, 0);
            this.btnTabKeDonThuoc.Name = "btnTabKeDonThuoc";
            this.btnTabKeDonThuoc.Size = new System.Drawing.Size(160, 46);
            this.btnTabKeDonThuoc.TabIndex = 2;
            this.btnTabKeDonThuoc.Text = "💊 Kê Đơn Thuốc";
            // 
            // btnTabDichVu
            // 
            this.btnTabDichVu.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTabDichVu.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTabDichVu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTabDichVu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTabDichVu.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTabDichVu.FillColor = System.Drawing.Color.Transparent;
            this.btnTabDichVu.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTabDichVu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnTabDichVu.Location = new System.Drawing.Point(154, 0);
            this.btnTabDichVu.Name = "btnTabDichVu";
            this.btnTabDichVu.Size = new System.Drawing.Size(110, 46);
            this.btnTabDichVu.TabIndex = 1;
            this.btnTabDichVu.Text = "✨ Dịch Vụ";
            // 
            // btnTabChanDoan
            // 
            this.btnTabChanDoan.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTabChanDoan.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTabChanDoan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTabChanDoan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTabChanDoan.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTabChanDoan.FillColor = System.Drawing.Color.Transparent;
            this.btnTabChanDoan.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTabChanDoan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnTabChanDoan.Location = new System.Drawing.Point(14, 0);
            this.btnTabChanDoan.Name = "btnTabChanDoan";
            this.btnTabChanDoan.Size = new System.Drawing.Size(140, 46);
            this.btnTabChanDoan.TabIndex = 0;
            this.btnTabChanDoan.Text = "🩺 Chẩn Đoán";
            // 
            // pnlDonThuoc
            // 
            this.pnlDonThuoc.BorderRadius = 12;
            this.pnlDonThuoc.Controls.Add(this.dgvDonThuoc);
            this.pnlDonThuoc.Controls.Add(this.pnlAddThuoc);
            this.pnlDonThuoc.Controls.Add(this.lblTitleDonThuoc);
            this.pnlDonThuoc.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDonThuoc.FillColor = System.Drawing.Color.White;
            this.pnlDonThuoc.Location = new System.Drawing.Point(0, 392);
            this.pnlDonThuoc.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlDonThuoc.Name = "pnlDonThuoc";
            this.pnlDonThuoc.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.pnlDonThuoc.Size = new System.Drawing.Size(1080, 240);
            this.pnlDonThuoc.TabIndex = 0;
            // 
            // dgvDonThuoc
            // 
            this.dgvDonThuoc.AllowUserToAddRows = false;
            this.dgvDonThuoc.AllowUserToDeleteRows = false;
            this.dgvDonThuoc.AllowUserToResizeColumns = false;
            this.dgvDonThuoc.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvDonThuoc.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDonThuoc.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDonThuoc.ColumnHeadersHeight = 38;
            this.dgvDonThuoc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTenThuoc,
            this.colDonVi,
            this.colSoLuong,
            this.colLieuDung,
            this.colHSD,
            this.colXoa});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDonThuoc.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDonThuoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDonThuoc.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(246)))), ((int)(((byte)(241)))));
            this.dgvDonThuoc.Location = new System.Drawing.Point(14, 94);
            this.dgvDonThuoc.Name = "dgvDonThuoc";
            this.dgvDonThuoc.RowHeadersVisible = false;
            this.dgvDonThuoc.RowHeadersWidth = 51;
            this.dgvDonThuoc.RowTemplate.Height = 36;
            this.dgvDonThuoc.Size = new System.Drawing.Size(1052, 136);
            this.dgvDonThuoc.TabIndex = 2;
            this.dgvDonThuoc.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDonThuoc.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvDonThuoc.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvDonThuoc.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvDonThuoc.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvDonThuoc.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvDonThuoc.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(246)))), ((int)(((byte)(241)))));
            this.dgvDonThuoc.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.dgvDonThuoc.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvDonThuoc.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDonThuoc.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvDonThuoc.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDonThuoc.ThemeStyle.HeaderStyle.Height = 38;
            this.dgvDonThuoc.ThemeStyle.ReadOnly = false;
            this.dgvDonThuoc.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDonThuoc.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDonThuoc.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDonThuoc.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvDonThuoc.ThemeStyle.RowsStyle.Height = 36;
            this.dgvDonThuoc.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDonThuoc.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // colTenThuoc
            // 
            this.colTenThuoc.DataPropertyName = "TenThuoc";
            this.colTenThuoc.FillWeight = 200F;
            this.colTenThuoc.HeaderText = "Tên thuốc";
            this.colTenThuoc.MinimumWidth = 6;
            this.colTenThuoc.Name = "colTenThuoc";
            this.colTenThuoc.ReadOnly = true;
            // 
            // colDonVi
            // 
            this.colDonVi.DataPropertyName = "DonViTinh";
            this.colDonVi.FillWeight = 75F;
            this.colDonVi.HeaderText = "Đơn vị";
            this.colDonVi.MinimumWidth = 6;
            this.colDonVi.Name = "colDonVi";
            this.colDonVi.ReadOnly = true;
            // 
            // colSoLuong
            // 
            this.colSoLuong.DataPropertyName = "SoLuong";
            this.colSoLuong.FillWeight = 75F;
            this.colSoLuong.HeaderText = "Số lượng";
            this.colSoLuong.MinimumWidth = 6;
            this.colSoLuong.Name = "colSoLuong";
            // 
            // colLieuDung
            // 
            this.colLieuDung.DataPropertyName = "LieuDung";
            this.colLieuDung.FillWeight = 200F;
            this.colLieuDung.HeaderText = "Liều dùng";
            this.colLieuDung.MinimumWidth = 6;
            this.colLieuDung.Name = "colLieuDung";
            // 
            // colHSD
            // 
            this.colHSD.DataPropertyName = "HanSuDung";
            this.colHSD.FillWeight = 85F;
            this.colHSD.HeaderText = "HSD";
            this.colHSD.MinimumWidth = 6;
            this.colHSD.Name = "colHSD";
            this.colHSD.ReadOnly = true;
            // 
            // colXoa
            // 
            this.colXoa.FillWeight = 38F;
            this.colXoa.HeaderText = "";
            this.colXoa.MinimumWidth = 6;
            this.colXoa.Name = "colXoa";
            this.colXoa.Text = "✕";
            this.colXoa.UseColumnTextForButtonValue = true;
            // 
            // pnlAddThuoc
            // 
            this.pnlAddThuoc.BackColor = System.Drawing.Color.Transparent;
            this.pnlAddThuoc.Controls.Add(this.cboThuoc);
            this.pnlAddThuoc.Controls.Add(this.pnlSpace3);
            this.pnlAddThuoc.Controls.Add(this.txtSoLuong);
            this.pnlAddThuoc.Controls.Add(this.pnlSpace2);
            this.pnlAddThuoc.Controls.Add(this.txtLieuDung);
            this.pnlAddThuoc.Controls.Add(this.pnlSpace1);
            this.pnlAddThuoc.Controls.Add(this.btnThemThuoc);
            this.pnlAddThuoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAddThuoc.Location = new System.Drawing.Point(14, 44);
            this.pnlAddThuoc.Name = "pnlAddThuoc";
            this.pnlAddThuoc.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.pnlAddThuoc.Size = new System.Drawing.Size(1052, 50);
            this.pnlAddThuoc.TabIndex = 1;
            // 
            // cboThuoc
            // 
            this.cboThuoc.BackColor = System.Drawing.Color.Transparent;
            this.cboThuoc.BorderColor = System.Drawing.Color.SeaGreen;
            this.cboThuoc.BorderRadius = 10;
            this.cboThuoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboThuoc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboThuoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboThuoc.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboThuoc.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboThuoc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboThuoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboThuoc.ItemHeight = 30;
            this.cboThuoc.Location = new System.Drawing.Point(0, 5);
            this.cboThuoc.Name = "cboThuoc";
            this.cboThuoc.Size = new System.Drawing.Size(370, 36);
            this.cboThuoc.TabIndex = 7;
            // 
            // pnlSpace3
            // 
            this.pnlSpace3.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSpace3.Location = new System.Drawing.Point(370, 5);
            this.pnlSpace3.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.pnlSpace3.Name = "pnlSpace3";
            this.pnlSpace3.Size = new System.Drawing.Size(24, 40);
            this.pnlSpace3.TabIndex = 6;
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.BorderColor = System.Drawing.Color.SeaGreen;
            this.txtSoLuong.BorderRadius = 10;
            this.txtSoLuong.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSoLuong.DefaultText = "";
            this.txtSoLuong.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSoLuong.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSoLuong.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSoLuong.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSoLuong.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtSoLuong.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSoLuong.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSoLuong.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSoLuong.Location = new System.Drawing.Point(394, 5);
            this.txtSoLuong.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.PlaceholderText = "SL";
            this.txtSoLuong.SelectedText = "";
            this.txtSoLuong.Size = new System.Drawing.Size(141, 40);
            this.txtSoLuong.TabIndex = 5;
            // 
            // pnlSpace2
            // 
            this.pnlSpace2.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSpace2.Location = new System.Drawing.Point(535, 5);
            this.pnlSpace2.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.pnlSpace2.Name = "pnlSpace2";
            this.pnlSpace2.Size = new System.Drawing.Size(24, 40);
            this.pnlSpace2.TabIndex = 4;
            // 
            // txtLieuDung
            // 
            this.txtLieuDung.BorderColor = System.Drawing.Color.SeaGreen;
            this.txtLieuDung.BorderRadius = 10;
            this.txtLieuDung.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLieuDung.DefaultText = "";
            this.txtLieuDung.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtLieuDung.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtLieuDung.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtLieuDung.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtLieuDung.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtLieuDung.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtLieuDung.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtLieuDung.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtLieuDung.Location = new System.Drawing.Point(559, 5);
            this.txtLieuDung.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLieuDung.Name = "txtLieuDung";
            this.txtLieuDung.PlaceholderText = "Liều dùng";
            this.txtLieuDung.SelectedText = "";
            this.txtLieuDung.Size = new System.Drawing.Size(349, 40);
            this.txtLieuDung.TabIndex = 3;
            // 
            // pnlSpace1
            // 
            this.pnlSpace1.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSpace1.Location = new System.Drawing.Point(908, 5);
            this.pnlSpace1.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.pnlSpace1.Name = "pnlSpace1";
            this.pnlSpace1.Size = new System.Drawing.Size(24, 40);
            this.pnlSpace1.TabIndex = 2;
            // 
            // btnThemThuoc
            // 
            this.btnThemThuoc.BorderRadius = 10;
            this.btnThemThuoc.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnThemThuoc.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnThemThuoc.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnThemThuoc.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnThemThuoc.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnThemThuoc.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnThemThuoc.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnThemThuoc.FillColor2 = System.Drawing.Color.SeaGreen;
            this.btnThemThuoc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemThuoc.ForeColor = System.Drawing.Color.White;
            this.btnThemThuoc.Location = new System.Drawing.Point(932, 5);
            this.btnThemThuoc.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnThemThuoc.Name = "btnThemThuoc";
            this.btnThemThuoc.Size = new System.Drawing.Size(120, 40);
            this.btnThemThuoc.TabIndex = 0;
            this.btnThemThuoc.Text = "Thêm";
            // 
            // lblTitleDonThuoc
            // 
            this.lblTitleDonThuoc.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleDonThuoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleDonThuoc.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleDonThuoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTitleDonThuoc.Location = new System.Drawing.Point(14, 10);
            this.lblTitleDonThuoc.Name = "lblTitleDonThuoc";
            this.lblTitleDonThuoc.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.lblTitleDonThuoc.Size = new System.Drawing.Size(1052, 34);
            this.lblTitleDonThuoc.TabIndex = 0;
            this.lblTitleDonThuoc.Text = "💊 Đơn Thuốc (VW_TonKhoTheoLo — FEFO)";
            // 
            // pnlContent
            // 
            this.pnlContent.AutoScroll = true;
            this.pnlContent.Controls.Add(this.tlpMain);
            this.pnlContent.Controls.Add(this.pnlSpace5);
            this.pnlContent.Controls.Add(this.pnlSpace4);
            this.pnlContent.Controls.Add(this.pnlDonThuoc);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(10, 118);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1080, 632);
            this.pnlContent.TabIndex = 2;
            this.pnlContent.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlContent_Paint);
            // 
            // tlpMain
            // 
            this.tlpMain.BackColor = System.Drawing.Color.Transparent;
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpMain.Controls.Add(this.pnlDichVu, 1, 0);
            this.tlpMain.Controls.Add(this.pnlThongTinKham, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 10);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 372F));
            this.tlpMain.Size = new System.Drawing.Size(1080, 380);
            this.tlpMain.TabIndex = 3;
            // 
            // pnlDichVu
            // 
            this.pnlDichVu.BorderRadius = 12;
            this.pnlDichVu.Controls.Add(this.pnlDichVuList);
            this.pnlDichVu.Controls.Add(this.pnlTongDichVu);
            this.pnlDichVu.Controls.Add(this.lblTitleDichVu);
            this.pnlDichVu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDichVu.FillColor = System.Drawing.Color.White;
            this.pnlDichVu.Location = new System.Drawing.Point(600, 0);
            this.pnlDichVu.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.pnlDichVu.Name = "pnlDichVu";
            this.pnlDichVu.Padding = new System.Windows.Forms.Padding(16, 12, 16, 14);
            this.pnlDichVu.Size = new System.Drawing.Size(480, 372);
            this.pnlDichVu.TabIndex = 1;
            // 
            // pnlDichVuList
            // 
            this.pnlDichVuList.AutoScroll = true;
            this.pnlDichVuList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDichVuList.Location = new System.Drawing.Point(16, 48);
            this.pnlDichVuList.Name = "pnlDichVuList";
            this.pnlDichVuList.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.pnlDichVuList.Size = new System.Drawing.Size(448, 260);
            this.pnlDichVuList.TabIndex = 4;
            // 
            // pnlTongDichVu
            // 
            this.pnlTongDichVu.Controls.Add(this.lblTongDichVu);
            this.pnlTongDichVu.Controls.Add(this.lblTongDVLabel);
            this.pnlTongDichVu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTongDichVu.Location = new System.Drawing.Point(16, 308);
            this.pnlTongDichVu.Name = "pnlTongDichVu";
            this.pnlTongDichVu.Padding = new System.Windows.Forms.Padding(2, 8, 2, 0);
            this.pnlTongDichVu.Size = new System.Drawing.Size(448, 50);
            this.pnlTongDichVu.TabIndex = 3;
            // 
            // lblTongDichVu
            // 
            this.lblTongDichVu.AutoSize = true;
            this.lblTongDichVu.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTongDichVu.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongDichVu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTongDichVu.Location = new System.Drawing.Point(415, 8);
            this.lblTongDichVu.Name = "lblTongDichVu";
            this.lblTongDichVu.Size = new System.Drawing.Size(31, 23);
            this.lblTongDichVu.TabIndex = 1;
            this.lblTongDichVu.Text = "0đ";
            // 
            // lblTongDVLabel
            // 
            this.lblTongDVLabel.AutoSize = true;
            this.lblTongDVLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTongDVLabel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongDVLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTongDVLabel.Location = new System.Drawing.Point(2, 8);
            this.lblTongDVLabel.Name = "lblTongDVLabel";
            this.lblTongDVLabel.Size = new System.Drawing.Size(119, 23);
            this.lblTongDVLabel.TabIndex = 0;
            this.lblTongDVLabel.Text = "Tổng dịch vụ:";
            // 
            // lblTitleDichVu
            // 
            this.lblTitleDichVu.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleDichVu.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleDichVu.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleDichVu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTitleDichVu.Location = new System.Drawing.Point(16, 12);
            this.lblTitleDichVu.Name = "lblTitleDichVu";
            this.lblTitleDichVu.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.lblTitleDichVu.Size = new System.Drawing.Size(448, 36);
            this.lblTitleDichVu.TabIndex = 2;
            this.lblTitleDichVu.Text = "✨  Dịch Vụ Đã Thực Hiện";
            // 
            // pnlThongTinKham
            // 
            this.pnlThongTinKham.BorderRadius = 12;
            this.pnlThongTinKham.Controls.Add(this.tableLayoutPanel1);
            this.pnlThongTinKham.Controls.Add(this.pnlChanDoan);
            this.pnlThongTinKham.Controls.Add(this.pnlTrieuChung);
            this.pnlThongTinKham.Controls.Add(this.lblTitleThongTin);
            this.pnlThongTinKham.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlThongTinKham.FillColor = System.Drawing.Color.White;
            this.pnlThongTinKham.Location = new System.Drawing.Point(0, 0);
            this.pnlThongTinKham.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.pnlThongTinKham.Name = "pnlThongTinKham";
            this.pnlThongTinKham.Padding = new System.Windows.Forms.Padding(14, 12, 16, 14);
            this.pnlThongTinKham.Size = new System.Drawing.Size(588, 372);
            this.pnlThongTinKham.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(14, 248);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(558, 110);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cboTrangThai);
            this.panel2.Controls.Add(this.lblTrangThaiLabel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(282, 3);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.panel2.Size = new System.Drawing.Size(273, 104);
            this.panel2.TabIndex = 1;
            // 
            // cboTrangThai
            // 
            this.cboTrangThai.BackColor = System.Drawing.Color.Transparent;
            this.cboTrangThai.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.cboTrangThai.BorderRadius = 10;
            this.cboTrangThai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboTrangThai.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboTrangThai.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboTrangThai.ItemHeight = 30;
            this.cboTrangThai.Location = new System.Drawing.Point(8, 44);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(265, 36);
            this.cboTrangThai.TabIndex = 2;
            // 
            // lblTrangThaiLabel
            // 
            this.lblTrangThaiLabel.AutoSize = true;
            this.lblTrangThaiLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTrangThaiLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrangThaiLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTrangThaiLabel.Location = new System.Drawing.Point(8, 8);
            this.lblTrangThaiLabel.Name = "lblTrangThaiLabel";
            this.lblTrangThaiLabel.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.lblTrangThaiLabel.Size = new System.Drawing.Size(78, 36);
            this.lblTrangThaiLabel.TabIndex = 1;
            this.lblTrangThaiLabel.Text = "Trạng thái";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtpNgayTaiKham);
            this.panel1.Controls.Add(this.lblNgayTaiKham);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 8, 8, 24);
            this.panel1.Size = new System.Drawing.Size(273, 104);
            this.panel1.TabIndex = 0;
            // 
            // dtpNgayTaiKham
            // 
            this.dtpNgayTaiKham.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.dtpNgayTaiKham.BorderRadius = 10;
            this.dtpNgayTaiKham.BorderThickness = 1;
            this.dtpNgayTaiKham.Checked = true;
            this.dtpNgayTaiKham.Dock = System.Windows.Forms.DockStyle.Top;
            this.dtpNgayTaiKham.FillColor = System.Drawing.Color.White;
            this.dtpNgayTaiKham.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpNgayTaiKham.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgayTaiKham.Location = new System.Drawing.Point(0, 44);
            this.dtpNgayTaiKham.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpNgayTaiKham.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpNgayTaiKham.Name = "dtpNgayTaiKham";
            this.dtpNgayTaiKham.Size = new System.Drawing.Size(265, 36);
            this.dtpNgayTaiKham.TabIndex = 2;
            this.dtpNgayTaiKham.Value = new System.DateTime(2026, 4, 8, 4, 13, 31, 357);
            // 
            // lblNgayTaiKham
            // 
            this.lblNgayTaiKham.AutoSize = true;
            this.lblNgayTaiKham.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNgayTaiKham.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgayTaiKham.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblNgayTaiKham.Location = new System.Drawing.Point(0, 8);
            this.lblNgayTaiKham.Name = "lblNgayTaiKham";
            this.lblNgayTaiKham.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.lblNgayTaiKham.Size = new System.Drawing.Size(109, 36);
            this.lblNgayTaiKham.TabIndex = 1;
            this.lblNgayTaiKham.Text = "Ngày tái khám";
            // 
            // pnlChanDoan
            // 
            this.pnlChanDoan.Controls.Add(this.txtChanDoan);
            this.pnlChanDoan.Controls.Add(this.lblChanDoan);
            this.pnlChanDoan.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChanDoan.Location = new System.Drawing.Point(14, 148);
            this.pnlChanDoan.Name = "pnlChanDoan";
            this.pnlChanDoan.Size = new System.Drawing.Size(558, 100);
            this.pnlChanDoan.TabIndex = 3;
            // 
            // txtChanDoan
            // 
            this.txtChanDoan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.txtChanDoan.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.txtChanDoan.BorderRadius = 10;
            this.txtChanDoan.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtChanDoan.DefaultText = "";
            this.txtChanDoan.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtChanDoan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtChanDoan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtChanDoan.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtChanDoan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChanDoan.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtChanDoan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtChanDoan.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtChanDoan.Location = new System.Drawing.Point(0, 36);
            this.txtChanDoan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtChanDoan.Name = "txtChanDoan";
            this.txtChanDoan.PlaceholderText = "";
            this.txtChanDoan.SelectedText = "";
            this.txtChanDoan.Size = new System.Drawing.Size(558, 64);
            this.txtChanDoan.TabIndex = 1;
            // 
            // lblChanDoan
            // 
            this.lblChanDoan.AutoSize = true;
            this.lblChanDoan.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChanDoan.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChanDoan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblChanDoan.Location = new System.Drawing.Point(0, 0);
            this.lblChanDoan.Name = "lblChanDoan";
            this.lblChanDoan.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.lblChanDoan.Size = new System.Drawing.Size(83, 36);
            this.lblChanDoan.TabIndex = 0;
            this.lblChanDoan.Text = "Chẩn đoán";
            // 
            // pnlTrieuChung
            // 
            this.pnlTrieuChung.Controls.Add(this.txtTrieuChung);
            this.pnlTrieuChung.Controls.Add(this.lblTrieuChung);
            this.pnlTrieuChung.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTrieuChung.Location = new System.Drawing.Point(14, 48);
            this.pnlTrieuChung.Name = "pnlTrieuChung";
            this.pnlTrieuChung.Size = new System.Drawing.Size(558, 100);
            this.pnlTrieuChung.TabIndex = 2;
            // 
            // txtTrieuChung
            // 
            this.txtTrieuChung.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.txtTrieuChung.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
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
            this.txtTrieuChung.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTrieuChung.Location = new System.Drawing.Point(0, 36);
            this.txtTrieuChung.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTrieuChung.Name = "txtTrieuChung";
            this.txtTrieuChung.PlaceholderText = "";
            this.txtTrieuChung.SelectedText = "";
            this.txtTrieuChung.Size = new System.Drawing.Size(558, 64);
            this.txtTrieuChung.TabIndex = 1;
            // 
            // lblTrieuChung
            // 
            this.lblTrieuChung.AutoSize = true;
            this.lblTrieuChung.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTrieuChung.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrieuChung.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTrieuChung.Location = new System.Drawing.Point(0, 0);
            this.lblTrieuChung.Name = "lblTrieuChung";
            this.lblTrieuChung.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.lblTrieuChung.Size = new System.Drawing.Size(90, 36);
            this.lblTrieuChung.TabIndex = 0;
            this.lblTrieuChung.Text = "Triệu chứng";
            // 
            // lblTitleThongTin
            // 
            this.lblTitleThongTin.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleThongTin.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleThongTin.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleThongTin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTitleThongTin.Location = new System.Drawing.Point(14, 12);
            this.lblTitleThongTin.Name = "lblTitleThongTin";
            this.lblTitleThongTin.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.lblTitleThongTin.Size = new System.Drawing.Size(558, 36);
            this.lblTitleThongTin.TabIndex = 1;
            this.lblTitleThongTin.Text = "🩺  Thông Tin Khám";
            // 
            // pnlSpace5
            // 
            this.pnlSpace5.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpace5.Location = new System.Drawing.Point(0, 0);
            this.pnlSpace5.Name = "pnlSpace5";
            this.pnlSpace5.Size = new System.Drawing.Size(1080, 10);
            this.pnlSpace5.TabIndex = 2;
            // 
            // pnlSpace4
            // 
            this.pnlSpace4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSpace4.Location = new System.Drawing.Point(0, 390);
            this.pnlSpace4.Name = "pnlSpace4";
            this.pnlSpace4.Size = new System.Drawing.Size(1080, 2);
            this.pnlSpace4.TabIndex = 1;
            // 
            // PhieuKhamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(246)))));
            this.ClientSize = new System.Drawing.Size(1100, 760);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTabs);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PhieuKhamForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Phiếu Khám";
            this.pnlTop.ResumeLayout(false);
            this.pnlBenhNhanInfo.ResumeLayout(false);
            this.pnlBNText.ResumeLayout(false);
            this.pnlBNText.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlTabs.ResumeLayout(false);
            this.pnlDonThuoc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDonThuoc)).EndInit();
            this.pnlAddThuoc.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.pnlDichVu.ResumeLayout(false);
            this.pnlTongDichVu.ResumeLayout(false);
            this.pnlTongDichVu.PerformLayout();
            this.pnlThongTinKham.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlChanDoan.ResumeLayout(false);
            this.pnlChanDoan.PerformLayout();
            this.pnlTrieuChung.ResumeLayout(false);
            this.pnlTrieuChung.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlButtons;
        private Guna.UI2.WinForms.Guna2Button btnLuuNhap;
        private Guna.UI2.WinForms.Guna2GradientButton btnHoanThanhKham;
        private System.Windows.Forms.Panel pnlSpace;
        private System.Windows.Forms.Panel pnlBenhNhanInfo;
        private System.Windows.Forms.Panel pnlBNText;
        private System.Windows.Forms.Label lblAvatar;
        private System.Windows.Forms.Label lblDiUng;
        private System.Windows.Forms.Label lblThongTinBN;
        private System.Windows.Forms.Label lblTenBN;
        private System.Windows.Forms.Panel pnlTabs;
        private Guna.UI2.WinForms.Guna2Button btnTabChanDoan;
        private Guna.UI2.WinForms.Guna2Button btnTabGhiChu;
        private Guna.UI2.WinForms.Guna2Button btnTabHinhAnh;
        private Guna.UI2.WinForms.Guna2Button btnTabKeDonThuoc;
        private Guna.UI2.WinForms.Guna2Button btnTabDichVu;
        private Guna.UI2.WinForms.Guna2Panel pnlDonThuoc;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDonThuoc;
        private System.Windows.Forms.Panel pnlAddThuoc;
        private Guna.UI2.WinForms.Guna2ComboBox cboThuoc;
        private System.Windows.Forms.Panel pnlSpace3;
        private Guna.UI2.WinForms.Guna2TextBox txtSoLuong;
        private System.Windows.Forms.Panel pnlSpace2;
        private Guna.UI2.WinForms.Guna2TextBox txtLieuDung;
        private System.Windows.Forms.Panel pnlSpace1;
        private Guna.UI2.WinForms.Guna2GradientButton btnThemThuoc;
        private System.Windows.Forms.Label lblTitleDonThuoc;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlSpace4;
        private System.Windows.Forms.Panel pnlSpace5;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private Guna.UI2.WinForms.Guna2Panel pnlDichVu;
        private Guna.UI2.WinForms.Guna2Panel pnlThongTinKham;
        private System.Windows.Forms.Label lblTitleThongTin;
        private System.Windows.Forms.Panel pnlTrieuChung;
        private System.Windows.Forms.Label lblTrieuChung;
        private System.Windows.Forms.Panel pnlChanDoan;
        private Guna.UI2.WinForms.Guna2TextBox txtChanDoan;
        private System.Windows.Forms.Label lblChanDoan;
        private Guna.UI2.WinForms.Guna2TextBox txtTrieuChung;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private Guna.UI2.WinForms.Guna2ComboBox cboTrangThai;
        private System.Windows.Forms.Label lblTrangThaiLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblNgayTaiKham;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpNgayTaiKham;
        private System.Windows.Forms.Label lblTitleDichVu;
        private System.Windows.Forms.Panel pnlTongDichVu;
        private System.Windows.Forms.Label lblTongDichVu;
        private System.Windows.Forms.Label lblTongDVLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenThuoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonVi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLieuDung;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHSD;
        private System.Windows.Forms.DataGridViewButtonColumn colXoa;
        private System.Windows.Forms.Panel pnlDichVuList;
=======
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 760);
            this.Name = "PhieuKhamForm";
            this.Text = "Phiếu Khám";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }
>>>>>>> d2fc9d190a76c0c366e0407bca6067fe95379af1
    }
}
