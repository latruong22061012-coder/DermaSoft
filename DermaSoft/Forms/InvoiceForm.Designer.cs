namespace DermaSoft.Forms
{
    partial class InvoiceForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlWrapperThuoc = new System.Windows.Forms.Panel();
            this.pnlThuoc = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvThuoc = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colThuocTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThuocSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThuocDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThuocThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTitleThuoc = new System.Windows.Forms.Label();
            this.pnlWrapperDichVu = new System.Windows.Forms.Panel();
            this.pnlDichVu = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvDichVu = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colDVTenDV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDVSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDVDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDVThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTitleDV = new System.Windows.Forms.Label();
            this.pnlWrapperBNInfo = new System.Windows.Forms.Panel();
            this.pnlBNInfo = new Guna.UI2.WinForms.Guna2Panel();
            this.tlpBNInfo = new System.Windows.Forms.TableLayoutPanel();
            this.pnlChanDoan = new System.Windows.Forms.Panel();
            this.lblChanDoan = new System.Windows.Forms.Label();
            this.lblLabelCD = new System.Windows.Forms.Label();
            this.pnlBacSi = new System.Windows.Forms.Panel();
            this.lblBacSi = new System.Windows.Forms.Label();
            this.lblLabelBS = new System.Windows.Forms.Label();
            this.pnlNgayKham = new System.Windows.Forms.Panel();
            this.lblNgayKham = new System.Windows.Forms.Label();
            this.lblLabelNgay = new System.Windows.Forms.Label();
            this.pnlBNTen = new System.Windows.Forms.Panel();
            this.lblBNTen = new System.Windows.Forms.Label();
            this.lblLabelBN = new System.Windows.Forms.Label();
            this.pnlPhieuKham = new System.Windows.Forms.Panel();
            this.cmbPhieuKham = new Guna.UI2.WinForms.Guna2ComboBox();
            this.pnlWrapperlblPhieuKham = new System.Windows.Forms.Panel();
            this.lblPhieuKham = new System.Windows.Forms.Label();
            this.pnlButtonTai = new System.Windows.Forms.Panel();
            this.btnTai = new Guna.UI2.WinForms.Guna2Button();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.tlpRightButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnXacNhan = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnInHoaDon = new Guna.UI2.WinForms.Guna2Button();
            this.pnlWrapperPreview = new System.Windows.Forms.Panel();
            this.pnlPreview = new Guna.UI2.WinForms.Guna2Panel();
            this.lblPreviewTongValue = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.lblPreviewTongLabel = new System.Windows.Forms.Label();
            this.guna2Separator2 = new Guna.UI2.WinForms.Guna2Separator();
            this.pnlThuocRow = new System.Windows.Forms.Panel();
            this.lblPreviewThuocValue = new System.Windows.Forms.Label();
            this.lblPreviewThuocLabel = new System.Windows.Forms.Label();
            this.pnlDVRow = new System.Windows.Forms.Panel();
            this.lblPreviewDVValue = new System.Windows.Forms.Label();
            this.lblPreviewDVLabel = new System.Windows.Forms.Label();
            this.pnlBNRow = new System.Windows.Forms.Panel();
            this.lblPreviewNgay = new System.Windows.Forms.Label();
            this.lblPreviewBN = new System.Windows.Forms.Label();
            this.pnlDermaSoftClinic = new System.Windows.Forms.Panel();
            this.guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            this.lblClinicSDT = new System.Windows.Forms.Label();
            this.lblClinicAddress = new System.Windows.Forms.Label();
            this.lblClinicName = new System.Windows.Forms.Label();
            this.pnlWrapperThanhToan = new System.Windows.Forms.Panel();
            this.pnlThanhToan = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlTienThua = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTienThuaLabel = new System.Windows.Forms.Label();
            this.lblTienThua = new System.Windows.Forms.Label();
            this.pnlTienKhach = new System.Windows.Forms.Panel();
            this.txtTienKhach = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTienKhach = new System.Windows.Forms.Label();
            this.pnlPhuongThucTT = new System.Windows.Forms.Panel();
            this.cmbPhuongThuc = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblPhuongThuc = new System.Windows.Forms.Label();
            this.lblTitleTT = new System.Windows.Forms.Label();
            this.pnlWrapperTongKet = new System.Windows.Forms.Panel();
            this.pnlTongKet = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlTong = new System.Windows.Forms.Panel();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.lblTONG = new System.Windows.Forms.Label();
            this.pnlGiamGia = new System.Windows.Forms.Panel();
            this.txtGiamGia = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblGiamGiaLabel = new System.Windows.Forms.Label();
            this.pnlTamTinh = new System.Windows.Forms.Panel();
            this.lblTamTinhValue = new System.Windows.Forms.Label();
            this.lblTamTinhLabel = new System.Windows.Forms.Label();
            this.pnlTongThuoc = new System.Windows.Forms.Panel();
            this.lblTongThuocValue = new System.Windows.Forms.Label();
            this.lblTongThuocLabel = new System.Windows.Forms.Label();
            this.pnlTongDV = new System.Windows.Forms.Panel();
            this.lblTongDVValue = new System.Windows.Forms.Label();
            this.lblTongDVLabel = new System.Windows.Forms.Label();
            this.lblTitleTK = new System.Windows.Forms.Label();
            this.tlpMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlWrapperThuoc.SuspendLayout();
            this.pnlThuoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThuoc)).BeginInit();
            this.pnlWrapperDichVu.SuspendLayout();
            this.pnlDichVu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDichVu)).BeginInit();
            this.pnlWrapperBNInfo.SuspendLayout();
            this.pnlBNInfo.SuspendLayout();
            this.tlpBNInfo.SuspendLayout();
            this.pnlChanDoan.SuspendLayout();
            this.pnlBacSi.SuspendLayout();
            this.pnlNgayKham.SuspendLayout();
            this.pnlBNTen.SuspendLayout();
            this.pnlPhieuKham.SuspendLayout();
            this.pnlWrapperlblPhieuKham.SuspendLayout();
            this.pnlButtonTai.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.tlpRightButtons.SuspendLayout();
            this.pnlWrapperPreview.SuspendLayout();
            this.pnlPreview.SuspendLayout();
            this.lblPreviewTongValue.SuspendLayout();
            this.pnlThuocRow.SuspendLayout();
            this.pnlDVRow.SuspendLayout();
            this.pnlBNRow.SuspendLayout();
            this.pnlDermaSoftClinic.SuspendLayout();
            this.pnlWrapperThanhToan.SuspendLayout();
            this.pnlThanhToan.SuspendLayout();
            this.pnlTienThua.SuspendLayout();
            this.pnlTienKhach.SuspendLayout();
            this.pnlPhuongThucTT.SuspendLayout();
            this.pnlWrapperTongKet.SuspendLayout();
            this.pnlTongKet.SuspendLayout();
            this.pnlTong.SuspendLayout();
            this.pnlGiamGia.SuspendLayout();
            this.pnlTamTinh.SuspendLayout();
            this.pnlTongThuoc.SuspendLayout();
            this.pnlTongDV.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpMain.Controls.Add(this.pnlLeft, 0, 0);
            this.tlpMain.Controls.Add(this.pnlRight, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(20, 10);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(1020, 748);
            this.tlpMain.TabIndex = 0;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.pnlWrapperThuoc);
            this.pnlLeft.Controls.Add(this.pnlWrapperDichVu);
            this.pnlLeft.Controls.Add(this.pnlWrapperBNInfo);
            this.pnlLeft.Controls.Add(this.pnlPhieuKham);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(651, 748);
            this.pnlLeft.TabIndex = 0;
            this.pnlLeft.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlLeft_Paint);
            // 
            // pnlWrapperThuoc
            // 
            this.pnlWrapperThuoc.Controls.Add(this.pnlThuoc);
            this.pnlWrapperThuoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWrapperThuoc.Location = new System.Drawing.Point(0, 485);
            this.pnlWrapperThuoc.Name = "pnlWrapperThuoc";
            this.pnlWrapperThuoc.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlWrapperThuoc.Size = new System.Drawing.Size(651, 263);
            this.pnlWrapperThuoc.TabIndex = 3;
            // 
            // pnlThuoc
            // 
            this.pnlThuoc.BackColor = System.Drawing.Color.Transparent;
            this.pnlThuoc.BorderRadius = 12;
            this.pnlThuoc.Controls.Add(this.dgvThuoc);
            this.pnlThuoc.Controls.Add(this.lblTitleThuoc);
            this.pnlThuoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlThuoc.FillColor = System.Drawing.Color.White;
            this.pnlThuoc.Location = new System.Drawing.Point(0, 10);
            this.pnlThuoc.Name = "pnlThuoc";
            this.pnlThuoc.Padding = new System.Windows.Forms.Padding(10);
            this.pnlThuoc.ShadowDecoration.BorderRadius = 12;
            this.pnlThuoc.ShadowDecoration.Color = System.Drawing.Color.LightGray;
            this.pnlThuoc.ShadowDecoration.Depth = 4;
            this.pnlThuoc.ShadowDecoration.Enabled = true;
            this.pnlThuoc.Size = new System.Drawing.Size(651, 253);
            this.pnlThuoc.TabIndex = 1;
            // 
            // dgvThuoc
            // 
            this.dgvThuoc.AllowUserToAddRows = false;
            this.dgvThuoc.AllowUserToDeleteRows = false;
            this.dgvThuoc.AllowUserToResizeColumns = false;
            this.dgvThuoc.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvThuoc.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvThuoc.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvThuoc.ColumnHeadersHeight = 40;
            this.dgvThuoc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colThuocTen,
            this.colThuocSoLuong,
            this.colThuocDonGia,
            this.colThuocThanhTien});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvThuoc.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvThuoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvThuoc.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvThuoc.Location = new System.Drawing.Point(10, 45);
            this.dgvThuoc.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.dgvThuoc.Name = "dgvThuoc";
            this.dgvThuoc.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvThuoc.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvThuoc.RowHeadersVisible = false;
            this.dgvThuoc.RowHeadersWidth = 51;
            this.dgvThuoc.RowTemplate.Height = 38;
            this.dgvThuoc.Size = new System.Drawing.Size(631, 198);
            this.dgvThuoc.TabIndex = 1;
            this.dgvThuoc.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvThuoc.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvThuoc.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvThuoc.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvThuoc.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvThuoc.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvThuoc.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvThuoc.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.dgvThuoc.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvThuoc.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvThuoc.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvThuoc.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvThuoc.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvThuoc.ThemeStyle.ReadOnly = true;
            this.dgvThuoc.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvThuoc.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvThuoc.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvThuoc.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvThuoc.ThemeStyle.RowsStyle.Height = 38;
            this.dgvThuoc.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.dgvThuoc.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            // 
            // colThuocTen
            // 
            this.colThuocTen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colThuocTen.DataPropertyName = "TenThuoc";
            this.colThuocTen.HeaderText = "Tên thuốc";
            this.colThuocTen.MinimumWidth = 6;
            this.colThuocTen.Name = "colThuocTen";
            this.colThuocTen.ReadOnly = true;
            // 
            // colThuocSoLuong
            // 
            this.colThuocSoLuong.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colThuocSoLuong.DataPropertyName = "SoLuong";
            this.colThuocSoLuong.HeaderText = "Số lượng";
            this.colThuocSoLuong.MinimumWidth = 6;
            this.colThuocSoLuong.Name = "colThuocSoLuong";
            this.colThuocSoLuong.ReadOnly = true;
            this.colThuocSoLuong.Width = 125;
            // 
            // colThuocDonGia
            // 
            this.colThuocDonGia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colThuocDonGia.DataPropertyName = "DonGia";
            this.colThuocDonGia.HeaderText = "Đơn giá";
            this.colThuocDonGia.MinimumWidth = 6;
            this.colThuocDonGia.Name = "colThuocDonGia";
            this.colThuocDonGia.ReadOnly = true;
            this.colThuocDonGia.Width = 120;
            // 
            // colThuocThanhTien
            // 
            this.colThuocThanhTien.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colThuocThanhTien.DataPropertyName = "ThanhTien";
            this.colThuocThanhTien.HeaderText = "Thành tiền";
            this.colThuocThanhTien.MinimumWidth = 6;
            this.colThuocThanhTien.Name = "colThuocThanhTien";
            this.colThuocThanhTien.ReadOnly = true;
            this.colThuocThanhTien.Width = 130;
            // 
            // lblTitleThuoc
            // 
            this.lblTitleThuoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleThuoc.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleThuoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTitleThuoc.Location = new System.Drawing.Point(10, 10);
            this.lblTitleThuoc.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblTitleThuoc.Name = "lblTitleThuoc";
            this.lblTitleThuoc.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblTitleThuoc.Size = new System.Drawing.Size(631, 35);
            this.lblTitleThuoc.TabIndex = 0;
            this.lblTitleThuoc.Text = "💊 Chi Tiết Thuốc";
            // 
            // pnlWrapperDichVu
            // 
            this.pnlWrapperDichVu.Controls.Add(this.pnlDichVu);
            this.pnlWrapperDichVu.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlWrapperDichVu.Location = new System.Drawing.Point(0, 162);
            this.pnlWrapperDichVu.Name = "pnlWrapperDichVu";
            this.pnlWrapperDichVu.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.pnlWrapperDichVu.Size = new System.Drawing.Size(651, 323);
            this.pnlWrapperDichVu.TabIndex = 2;
            // 
            // pnlDichVu
            // 
            this.pnlDichVu.BackColor = System.Drawing.Color.Transparent;
            this.pnlDichVu.BorderRadius = 12;
            this.pnlDichVu.Controls.Add(this.dgvDichVu);
            this.pnlDichVu.Controls.Add(this.lblTitleDV);
            this.pnlDichVu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDichVu.FillColor = System.Drawing.Color.White;
            this.pnlDichVu.Location = new System.Drawing.Point(0, 10);
            this.pnlDichVu.Name = "pnlDichVu";
            this.pnlDichVu.Padding = new System.Windows.Forms.Padding(10);
            this.pnlDichVu.ShadowDecoration.BorderRadius = 12;
            this.pnlDichVu.ShadowDecoration.Color = System.Drawing.Color.LightGray;
            this.pnlDichVu.ShadowDecoration.Depth = 4;
            this.pnlDichVu.ShadowDecoration.Enabled = true;
            this.pnlDichVu.Size = new System.Drawing.Size(651, 303);
            this.pnlDichVu.TabIndex = 0;
            // 
            // dgvDichVu
            // 
            this.dgvDichVu.AllowUserToAddRows = false;
            this.dgvDichVu.AllowUserToDeleteRows = false;
            this.dgvDichVu.AllowUserToResizeColumns = false;
            this.dgvDichVu.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            this.dgvDichVu.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDichVu.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDichVu.ColumnHeadersHeight = 40;
            this.dgvDichVu.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDVTenDV,
            this.colDVSoLuong,
            this.colDVDonGia,
            this.colDVThanhTien});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDichVu.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvDichVu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDichVu.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvDichVu.Location = new System.Drawing.Point(10, 45);
            this.dgvDichVu.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.dgvDichVu.Name = "dgvDichVu";
            this.dgvDichVu.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDichVu.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvDichVu.RowHeadersVisible = false;
            this.dgvDichVu.RowHeadersWidth = 51;
            this.dgvDichVu.RowTemplate.Height = 38;
            this.dgvDichVu.Size = new System.Drawing.Size(631, 248);
            this.dgvDichVu.TabIndex = 1;
            this.dgvDichVu.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDichVu.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvDichVu.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvDichVu.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvDichVu.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvDichVu.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvDichVu.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvDichVu.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.dgvDichVu.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvDichVu.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDichVu.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvDichVu.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDichVu.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvDichVu.ThemeStyle.ReadOnly = true;
            this.dgvDichVu.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDichVu.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDichVu.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDichVu.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvDichVu.ThemeStyle.RowsStyle.Height = 38;
            this.dgvDichVu.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.dgvDichVu.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            // 
            // colDVTenDV
            // 
            this.colDVTenDV.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDVTenDV.DataPropertyName = "TenDichVu";
            this.colDVTenDV.HeaderText = "Dịch vụ";
            this.colDVTenDV.MinimumWidth = 6;
            this.colDVTenDV.Name = "colDVTenDV";
            this.colDVTenDV.ReadOnly = true;
            // 
            // colDVSoLuong
            // 
            this.colDVSoLuong.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colDVSoLuong.DataPropertyName = "SoLuong";
            this.colDVSoLuong.HeaderText = "Số lượng";
            this.colDVSoLuong.MinimumWidth = 6;
            this.colDVSoLuong.Name = "colDVSoLuong";
            this.colDVSoLuong.ReadOnly = true;
            this.colDVSoLuong.Width = 125;
            // 
            // colDVDonGia
            // 
            this.colDVDonGia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colDVDonGia.DataPropertyName = "DonGia";
            this.colDVDonGia.HeaderText = "Đơn giá";
            this.colDVDonGia.MinimumWidth = 6;
            this.colDVDonGia.Name = "colDVDonGia";
            this.colDVDonGia.ReadOnly = true;
            this.colDVDonGia.Width = 120;
            // 
            // colDVThanhTien
            // 
            this.colDVThanhTien.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colDVThanhTien.DataPropertyName = "ThanhTien";
            this.colDVThanhTien.HeaderText = "Thành tiền";
            this.colDVThanhTien.MinimumWidth = 6;
            this.colDVThanhTien.Name = "colDVThanhTien";
            this.colDVThanhTien.ReadOnly = true;
            this.colDVThanhTien.Width = 130;
            // 
            // lblTitleDV
            // 
            this.lblTitleDV.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleDV.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleDV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTitleDV.Location = new System.Drawing.Point(10, 10);
            this.lblTitleDV.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblTitleDV.Name = "lblTitleDV";
            this.lblTitleDV.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblTitleDV.Size = new System.Drawing.Size(631, 35);
            this.lblTitleDV.TabIndex = 0;
            this.lblTitleDV.Text = "✨ Chi Tiết Dịch Vụ";
            // 
            // pnlWrapperBNInfo
            // 
            this.pnlWrapperBNInfo.Controls.Add(this.pnlBNInfo);
            this.pnlWrapperBNInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlWrapperBNInfo.Location = new System.Drawing.Point(0, 55);
            this.pnlWrapperBNInfo.Name = "pnlWrapperBNInfo";
            this.pnlWrapperBNInfo.Padding = new System.Windows.Forms.Padding(0, 15, 0, 10);
            this.pnlWrapperBNInfo.Size = new System.Drawing.Size(651, 107);
            this.pnlWrapperBNInfo.TabIndex = 1;
            // 
            // pnlBNInfo
            // 
            this.pnlBNInfo.BorderRadius = 10;
            this.pnlBNInfo.Controls.Add(this.tlpBNInfo);
            this.pnlBNInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBNInfo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.pnlBNInfo.Location = new System.Drawing.Point(0, 15);
            this.pnlBNInfo.Name = "pnlBNInfo";
            this.pnlBNInfo.Padding = new System.Windows.Forms.Padding(10);
            this.pnlBNInfo.Size = new System.Drawing.Size(651, 82);
            this.pnlBNInfo.TabIndex = 0;
            // 
            // tlpBNInfo
            // 
            this.tlpBNInfo.BackColor = System.Drawing.Color.Transparent;
            this.tlpBNInfo.ColumnCount = 4;
            this.tlpBNInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpBNInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpBNInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.tlpBNInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27F));
            this.tlpBNInfo.Controls.Add(this.pnlChanDoan, 3, 0);
            this.tlpBNInfo.Controls.Add(this.pnlBacSi, 2, 0);
            this.tlpBNInfo.Controls.Add(this.pnlNgayKham, 1, 0);
            this.tlpBNInfo.Controls.Add(this.pnlBNTen, 0, 0);
            this.tlpBNInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBNInfo.Location = new System.Drawing.Point(10, 10);
            this.tlpBNInfo.Name = "tlpBNInfo";
            this.tlpBNInfo.RowCount = 1;
            this.tlpBNInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBNInfo.Size = new System.Drawing.Size(631, 62);
            this.tlpBNInfo.TabIndex = 0;
            this.tlpBNInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.tlpBNInfo_Paint);
            // 
            // pnlChanDoan
            // 
            this.pnlChanDoan.Controls.Add(this.lblChanDoan);
            this.pnlChanDoan.Controls.Add(this.lblLabelCD);
            this.pnlChanDoan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChanDoan.Location = new System.Drawing.Point(462, 3);
            this.pnlChanDoan.Name = "pnlChanDoan";
            this.pnlChanDoan.Size = new System.Drawing.Size(166, 56);
            this.pnlChanDoan.TabIndex = 3;
            this.pnlChanDoan.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlChanDoan_Paint);
            // 
            // lblChanDoan
            // 
            this.lblChanDoan.AutoSize = true;
            this.lblChanDoan.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblChanDoan.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChanDoan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblChanDoan.Location = new System.Drawing.Point(0, 36);
            this.lblChanDoan.Name = "lblChanDoan";
            this.lblChanDoan.Size = new System.Drawing.Size(76, 20);
            this.lblChanDoan.TabIndex = 3;
            this.lblChanDoan.Text = "Loading...";
            // 
            // lblLabelCD
            // 
            this.lblLabelCD.AutoSize = true;
            this.lblLabelCD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLabelCD.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabelCD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblLabelCD.Location = new System.Drawing.Point(0, 0);
            this.lblLabelCD.Name = "lblLabelCD";
            this.lblLabelCD.Size = new System.Drawing.Size(83, 20);
            this.lblLabelCD.TabIndex = 1;
            this.lblLabelCD.Text = "Chẩn đoán";
            // 
            // pnlBacSi
            // 
            this.pnlBacSi.Controls.Add(this.lblBacSi);
            this.pnlBacSi.Controls.Add(this.lblLabelBS);
            this.pnlBacSi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBacSi.Location = new System.Drawing.Point(286, 3);
            this.pnlBacSi.Name = "pnlBacSi";
            this.pnlBacSi.Size = new System.Drawing.Size(170, 56);
            this.pnlBacSi.TabIndex = 2;
            // 
            // lblBacSi
            // 
            this.lblBacSi.AutoSize = true;
            this.lblBacSi.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblBacSi.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBacSi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblBacSi.Location = new System.Drawing.Point(0, 36);
            this.lblBacSi.Name = "lblBacSi";
            this.lblBacSi.Size = new System.Drawing.Size(76, 20);
            this.lblBacSi.TabIndex = 3;
            this.lblBacSi.Text = "Loading...";
            // 
            // lblLabelBS
            // 
            this.lblLabelBS.AutoSize = true;
            this.lblLabelBS.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLabelBS.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabelBS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblLabelBS.Location = new System.Drawing.Point(0, 0);
            this.lblLabelBS.Name = "lblLabelBS";
            this.lblLabelBS.Size = new System.Drawing.Size(47, 20);
            this.lblLabelBS.TabIndex = 1;
            this.lblLabelBS.Text = "Bác sĩ";
            // 
            // pnlNgayKham
            // 
            this.pnlNgayKham.Controls.Add(this.lblNgayKham);
            this.pnlNgayKham.Controls.Add(this.lblLabelNgay);
            this.pnlNgayKham.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNgayKham.Location = new System.Drawing.Point(160, 3);
            this.pnlNgayKham.Name = "pnlNgayKham";
            this.pnlNgayKham.Size = new System.Drawing.Size(120, 56);
            this.pnlNgayKham.TabIndex = 1;
            this.pnlNgayKham.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // lblNgayKham
            // 
            this.lblNgayKham.AutoSize = true;
            this.lblNgayKham.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblNgayKham.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgayKham.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblNgayKham.Location = new System.Drawing.Point(0, 36);
            this.lblNgayKham.Name = "lblNgayKham";
            this.lblNgayKham.Size = new System.Drawing.Size(76, 20);
            this.lblNgayKham.TabIndex = 2;
            this.lblNgayKham.Text = "Loading...";
            // 
            // lblLabelNgay
            // 
            this.lblLabelNgay.AutoSize = true;
            this.lblLabelNgay.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLabelNgay.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabelNgay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblLabelNgay.Location = new System.Drawing.Point(0, 0);
            this.lblLabelNgay.Name = "lblLabelNgay";
            this.lblLabelNgay.Size = new System.Drawing.Size(88, 20);
            this.lblLabelNgay.TabIndex = 1;
            this.lblLabelNgay.Text = "Ngày khám";
            // 
            // pnlBNTen
            // 
            this.pnlBNTen.Controls.Add(this.lblBNTen);
            this.pnlBNTen.Controls.Add(this.lblLabelBN);
            this.pnlBNTen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBNTen.Location = new System.Drawing.Point(3, 3);
            this.pnlBNTen.Name = "pnlBNTen";
            this.pnlBNTen.Size = new System.Drawing.Size(151, 56);
            this.pnlBNTen.TabIndex = 0;
            // 
            // lblBNTen
            // 
            this.lblBNTen.AutoSize = true;
            this.lblBNTen.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblBNTen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBNTen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblBNTen.Location = new System.Drawing.Point(0, 36);
            this.lblBNTen.Name = "lblBNTen";
            this.lblBNTen.Size = new System.Drawing.Size(77, 20);
            this.lblBNTen.TabIndex = 1;
            this.lblBNTen.Text = "Loading...";
            // 
            // lblLabelBN
            // 
            this.lblLabelBN.AutoSize = true;
            this.lblLabelBN.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLabelBN.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabelBN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblLabelBN.Location = new System.Drawing.Point(0, 0);
            this.lblLabelBN.Name = "lblLabelBN";
            this.lblLabelBN.Size = new System.Drawing.Size(83, 20);
            this.lblLabelBN.TabIndex = 0;
            this.lblLabelBN.Text = "Bệnh nhân";
            // 
            // pnlPhieuKham
            // 
            this.pnlPhieuKham.Controls.Add(this.cmbPhieuKham);
            this.pnlPhieuKham.Controls.Add(this.pnlWrapperlblPhieuKham);
            this.pnlPhieuKham.Controls.Add(this.pnlButtonTai);
            this.pnlPhieuKham.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPhieuKham.Location = new System.Drawing.Point(0, 0);
            this.pnlPhieuKham.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.pnlPhieuKham.Name = "pnlPhieuKham";
            this.pnlPhieuKham.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.pnlPhieuKham.Size = new System.Drawing.Size(651, 55);
            this.pnlPhieuKham.TabIndex = 0;
            this.pnlPhieuKham.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlPhieuKham_Paint);
            // 
            // cmbPhieuKham
            // 
            this.cmbPhieuKham.BackColor = System.Drawing.Color.Transparent;
            this.cmbPhieuKham.BorderColor = System.Drawing.Color.SeaGreen;
            this.cmbPhieuKham.BorderRadius = 10;
            this.cmbPhieuKham.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPhieuKham.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPhieuKham.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPhieuKham.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPhieuKham.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPhieuKham.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbPhieuKham.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbPhieuKham.ItemHeight = 30;
            this.cmbPhieuKham.Location = new System.Drawing.Point(114, 8);
            this.cmbPhieuKham.Name = "cmbPhieuKham";
            this.cmbPhieuKham.Size = new System.Drawing.Size(393, 36);
            this.cmbPhieuKham.TabIndex = 3;
            // 
            // pnlWrapperlblPhieuKham
            // 
            this.pnlWrapperlblPhieuKham.Controls.Add(this.lblPhieuKham);
            this.pnlWrapperlblPhieuKham.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlWrapperlblPhieuKham.Location = new System.Drawing.Point(0, 8);
            this.pnlWrapperlblPhieuKham.Name = "pnlWrapperlblPhieuKham";
            this.pnlWrapperlblPhieuKham.Padding = new System.Windows.Forms.Padding(0, 8, 5, 15);
            this.pnlWrapperlblPhieuKham.Size = new System.Drawing.Size(114, 39);
            this.pnlWrapperlblPhieuKham.TabIndex = 2;
            // 
            // lblPhieuKham
            // 
            this.lblPhieuKham.AutoSize = true;
            this.lblPhieuKham.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPhieuKham.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhieuKham.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblPhieuKham.Location = new System.Drawing.Point(0, 8);
            this.lblPhieuKham.Margin = new System.Windows.Forms.Padding(5);
            this.lblPhieuKham.Name = "lblPhieuKham";
            this.lblPhieuKham.Size = new System.Drawing.Size(109, 23);
            this.lblPhieuKham.TabIndex = 0;
            this.lblPhieuKham.Text = "Phiếu khám:";
            // 
            // pnlButtonTai
            // 
            this.pnlButtonTai.Controls.Add(this.btnTai);
            this.pnlButtonTai.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtonTai.Location = new System.Drawing.Point(507, 8);
            this.pnlButtonTai.Name = "pnlButtonTai";
            this.pnlButtonTai.Padding = new System.Windows.Forms.Padding(45, 0, 0, 0);
            this.pnlButtonTai.Size = new System.Drawing.Size(144, 39);
            this.pnlButtonTai.TabIndex = 1;
            // 
            // btnTai
            // 
            this.btnTai.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnTai.BorderRadius = 18;
            this.btnTai.BorderThickness = 1;
            this.btnTai.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTai.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTai.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTai.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTai.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.btnTai.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnTai.Location = new System.Drawing.Point(45, 0);
            this.btnTai.Name = "btnTai";
            this.btnTai.Size = new System.Drawing.Size(99, 39);
            this.btnTai.TabIndex = 0;
            this.btnTai.Text = "Tải";
            // 
            // pnlRight
            // 
            this.pnlRight.AutoScroll = true;
            this.pnlRight.Controls.Add(this.tlpRightButtons);
            this.pnlRight.Controls.Add(this.pnlWrapperPreview);
            this.pnlRight.Controls.Add(this.pnlWrapperThanhToan);
            this.pnlRight.Controls.Add(this.pnlWrapperTongKet);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(666, 3);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(351, 742);
            this.pnlRight.TabIndex = 1;
            // 
            // tlpRightButtons
            // 
            this.tlpRightButtons.ColumnCount = 1;
            this.tlpRightButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRightButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRightButtons.Controls.Add(this.btnXacNhan, 0, 0);
            this.tlpRightButtons.Controls.Add(this.btnInHoaDon, 0, 1);
            this.tlpRightButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRightButtons.Location = new System.Drawing.Point(0, 493);
            this.tlpRightButtons.Name = "tlpRightButtons";
            this.tlpRightButtons.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.tlpRightButtons.RowCount = 2;
            this.tlpRightButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRightButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRightButtons.Size = new System.Drawing.Size(351, 3);
            this.tlpRightButtons.TabIndex = 3;
            // 
            // btnXacNhan
            // 
            this.btnXacNhan.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnXacNhan.BorderRadius = 18;
            this.btnXacNhan.BorderThickness = 1;
            this.btnXacNhan.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnXacNhan.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnXacNhan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnXacNhan.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnXacNhan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnXacNhan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnXacNhan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnXacNhan.FillColor2 = System.Drawing.Color.SeaGreen;
            this.btnXacNhan.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXacNhan.ForeColor = System.Drawing.Color.White;
            this.btnXacNhan.Location = new System.Drawing.Point(13, 3);
            this.btnXacNhan.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.Size = new System.Drawing.Size(325, 1);
            this.btnXacNhan.TabIndex = 0;
            this.btnXacNhan.Text = "Xác Nhận Thanh Toán";
            // 
            // btnInHoaDon
            // 
            this.btnInHoaDon.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnInHoaDon.BorderRadius = 18;
            this.btnInHoaDon.BorderThickness = 1;
            this.btnInHoaDon.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnInHoaDon.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnInHoaDon.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnInHoaDon.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnInHoaDon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInHoaDon.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.btnInHoaDon.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInHoaDon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnInHoaDon.Location = new System.Drawing.Point(13, 6);
            this.btnInHoaDon.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.btnInHoaDon.Name = "btnInHoaDon";
            this.btnInHoaDon.Size = new System.Drawing.Size(325, 1);
            this.btnInHoaDon.TabIndex = 1;
            this.btnInHoaDon.Text = "In Hóa Đơn";
            // 
            // pnlWrapperPreview
            // 
            this.pnlWrapperPreview.Controls.Add(this.pnlPreview);
            this.pnlWrapperPreview.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlWrapperPreview.Location = new System.Drawing.Point(0, 496);
            this.pnlWrapperPreview.Name = "pnlWrapperPreview";
            this.pnlWrapperPreview.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.pnlWrapperPreview.Size = new System.Drawing.Size(351, 246);
            this.pnlWrapperPreview.TabIndex = 2;
            // 
            // pnlPreview
            // 
            this.pnlPreview.BorderRadius = 12;
            this.pnlPreview.Controls.Add(this.lblPreviewTongValue);
            this.pnlPreview.Controls.Add(this.pnlThuocRow);
            this.pnlPreview.Controls.Add(this.pnlDVRow);
            this.pnlPreview.Controls.Add(this.pnlBNRow);
            this.pnlPreview.Controls.Add(this.pnlDermaSoftClinic);
            this.pnlPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPreview.FillColor = System.Drawing.Color.White;
            this.pnlPreview.Location = new System.Drawing.Point(0, 5);
            this.pnlPreview.Name = "pnlPreview";
            this.pnlPreview.Padding = new System.Windows.Forms.Padding(10, 0, 10, 8);
            this.pnlPreview.Size = new System.Drawing.Size(351, 241);
            this.pnlPreview.TabIndex = 1;
            // 
            // lblPreviewTongValue
            // 
            this.lblPreviewTongValue.BackColor = System.Drawing.Color.Transparent;
            this.lblPreviewTongValue.Controls.Add(this.label5);
            this.lblPreviewTongValue.Controls.Add(this.lblPreviewTongLabel);
            this.lblPreviewTongValue.Controls.Add(this.guna2Separator2);
            this.lblPreviewTongValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPreviewTongValue.Location = new System.Drawing.Point(10, 191);
            this.lblPreviewTongValue.Name = "lblPreviewTongValue";
            this.lblPreviewTongValue.Size = new System.Drawing.Size(331, 42);
            this.lblPreviewTongValue.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.label5.Location = new System.Drawing.Point(241, 10);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label5.Size = new System.Drawing.Size(90, 28);
            this.label5.TabIndex = 1;
            this.label5.Text = "Loading...";
            // 
            // lblPreviewTongLabel
            // 
            this.lblPreviewTongLabel.AutoSize = true;
            this.lblPreviewTongLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPreviewTongLabel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreviewTongLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblPreviewTongLabel.Location = new System.Drawing.Point(0, 10);
            this.lblPreviewTongLabel.Name = "lblPreviewTongLabel";
            this.lblPreviewTongLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lblPreviewTongLabel.Size = new System.Drawing.Size(56, 28);
            this.lblPreviewTongLabel.TabIndex = 0;
            this.lblPreviewTongLabel.Text = "Tổng:";
            // 
            // guna2Separator2
            // 
            this.guna2Separator2.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Separator2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.guna2Separator2.FillThickness = 2;
            this.guna2Separator2.Location = new System.Drawing.Point(0, 0);
            this.guna2Separator2.Name = "guna2Separator2";
            this.guna2Separator2.Size = new System.Drawing.Size(331, 10);
            this.guna2Separator2.TabIndex = 7;
            // 
            // pnlThuocRow
            // 
            this.pnlThuocRow.BackColor = System.Drawing.Color.Transparent;
            this.pnlThuocRow.Controls.Add(this.lblPreviewThuocValue);
            this.pnlThuocRow.Controls.Add(this.lblPreviewThuocLabel);
            this.pnlThuocRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlThuocRow.Location = new System.Drawing.Point(10, 159);
            this.pnlThuocRow.Name = "pnlThuocRow";
            this.pnlThuocRow.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.pnlThuocRow.Size = new System.Drawing.Size(331, 32);
            this.pnlThuocRow.TabIndex = 4;
            // 
            // lblPreviewThuocValue
            // 
            this.lblPreviewThuocValue.AutoSize = true;
            this.lblPreviewThuocValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPreviewThuocValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPreviewThuocValue.Location = new System.Drawing.Point(259, 5);
            this.lblPreviewThuocValue.Name = "lblPreviewThuocValue";
            this.lblPreviewThuocValue.Size = new System.Drawing.Size(72, 20);
            this.lblPreviewThuocValue.TabIndex = 1;
            this.lblPreviewThuocValue.Text = "Loading...";
            // 
            // lblPreviewThuocLabel
            // 
            this.lblPreviewThuocLabel.AutoSize = true;
            this.lblPreviewThuocLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPreviewThuocLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreviewThuocLabel.Location = new System.Drawing.Point(0, 5);
            this.lblPreviewThuocLabel.Name = "lblPreviewThuocLabel";
            this.lblPreviewThuocLabel.Size = new System.Drawing.Size(52, 20);
            this.lblPreviewThuocLabel.TabIndex = 0;
            this.lblPreviewThuocLabel.Text = "Thuốc:";
            // 
            // pnlDVRow
            // 
            this.pnlDVRow.BackColor = System.Drawing.Color.Transparent;
            this.pnlDVRow.Controls.Add(this.lblPreviewDVValue);
            this.pnlDVRow.Controls.Add(this.lblPreviewDVLabel);
            this.pnlDVRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDVRow.Location = new System.Drawing.Point(10, 127);
            this.pnlDVRow.Name = "pnlDVRow";
            this.pnlDVRow.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.pnlDVRow.Size = new System.Drawing.Size(331, 32);
            this.pnlDVRow.TabIndex = 3;
            // 
            // lblPreviewDVValue
            // 
            this.lblPreviewDVValue.AutoSize = true;
            this.lblPreviewDVValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPreviewDVValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPreviewDVValue.Location = new System.Drawing.Point(259, 5);
            this.lblPreviewDVValue.Name = "lblPreviewDVValue";
            this.lblPreviewDVValue.Size = new System.Drawing.Size(72, 20);
            this.lblPreviewDVValue.TabIndex = 1;
            this.lblPreviewDVValue.Text = "Loading...";
            // 
            // lblPreviewDVLabel
            // 
            this.lblPreviewDVLabel.AutoSize = true;
            this.lblPreviewDVLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPreviewDVLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreviewDVLabel.Location = new System.Drawing.Point(0, 5);
            this.lblPreviewDVLabel.Name = "lblPreviewDVLabel";
            this.lblPreviewDVLabel.Size = new System.Drawing.Size(61, 20);
            this.lblPreviewDVLabel.TabIndex = 0;
            this.lblPreviewDVLabel.Text = "Dịch vụ:";
            // 
            // pnlBNRow
            // 
            this.pnlBNRow.BackColor = System.Drawing.Color.Transparent;
            this.pnlBNRow.Controls.Add(this.lblPreviewNgay);
            this.pnlBNRow.Controls.Add(this.lblPreviewBN);
            this.pnlBNRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBNRow.Location = new System.Drawing.Point(10, 95);
            this.pnlBNRow.Name = "pnlBNRow";
            this.pnlBNRow.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.pnlBNRow.Size = new System.Drawing.Size(331, 32);
            this.pnlBNRow.TabIndex = 2;
            // 
            // lblPreviewNgay
            // 
            this.lblPreviewNgay.AutoSize = true;
            this.lblPreviewNgay.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPreviewNgay.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPreviewNgay.Location = new System.Drawing.Point(259, 5);
            this.lblPreviewNgay.Name = "lblPreviewNgay";
            this.lblPreviewNgay.Size = new System.Drawing.Size(72, 20);
            this.lblPreviewNgay.TabIndex = 1;
            this.lblPreviewNgay.Text = "Loading...";
            // 
            // lblPreviewBN
            // 
            this.lblPreviewBN.AutoSize = true;
            this.lblPreviewBN.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPreviewBN.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreviewBN.Location = new System.Drawing.Point(0, 5);
            this.lblPreviewBN.Name = "lblPreviewBN";
            this.lblPreviewBN.Size = new System.Drawing.Size(32, 20);
            this.lblPreviewBN.TabIndex = 0;
            this.lblPreviewBN.Text = "BN:";
            // 
            // pnlDermaSoftClinic
            // 
            this.pnlDermaSoftClinic.BackColor = System.Drawing.Color.Transparent;
            this.pnlDermaSoftClinic.Controls.Add(this.guna2Separator1);
            this.pnlDermaSoftClinic.Controls.Add(this.lblClinicSDT);
            this.pnlDermaSoftClinic.Controls.Add(this.lblClinicAddress);
            this.pnlDermaSoftClinic.Controls.Add(this.lblClinicName);
            this.pnlDermaSoftClinic.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDermaSoftClinic.Location = new System.Drawing.Point(10, 0);
            this.pnlDermaSoftClinic.Name = "pnlDermaSoftClinic";
            this.pnlDermaSoftClinic.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlDermaSoftClinic.Size = new System.Drawing.Size(331, 95);
            this.pnlDermaSoftClinic.TabIndex = 0;
            // 
            // guna2Separator1
            // 
            this.guna2Separator1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.guna2Separator1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.guna2Separator1.FillThickness = 2;
            this.guna2Separator1.Location = new System.Drawing.Point(0, 85);
            this.guna2Separator1.Name = "guna2Separator1";
            this.guna2Separator1.Size = new System.Drawing.Size(331, 10);
            this.guna2Separator1.TabIndex = 6;
            // 
            // lblClinicSDT
            // 
            this.lblClinicSDT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClinicSDT.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClinicSDT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblClinicSDT.Location = new System.Drawing.Point(0, 64);
            this.lblClinicSDT.Name = "lblClinicSDT";
            this.lblClinicSDT.Padding = new System.Windows.Forms.Padding(0, 3, 0, 5);
            this.lblClinicSDT.Size = new System.Drawing.Size(331, 31);
            this.lblClinicSDT.TabIndex = 5;
            this.lblClinicSDT.Text = "SĐT: 0909123456";
            this.lblClinicSDT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClinicSDT.Click += new System.EventHandler(this.lblClinicSDT_Click);
            // 
            // lblClinicAddress
            // 
            this.lblClinicAddress.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblClinicAddress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClinicAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblClinicAddress.Location = new System.Drawing.Point(0, 36);
            this.lblClinicAddress.Name = "lblClinicAddress";
            this.lblClinicAddress.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblClinicAddress.Size = new System.Drawing.Size(331, 28);
            this.lblClinicAddress.TabIndex = 4;
            this.lblClinicAddress.Text = "123 Nguyễn Hữu Cảnh, Q.Bình Thạnh";
            this.lblClinicAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblClinicName
            // 
            this.lblClinicName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblClinicName.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClinicName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblClinicName.Location = new System.Drawing.Point(0, 8);
            this.lblClinicName.Name = "lblClinicName";
            this.lblClinicName.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblClinicName.Size = new System.Drawing.Size(331, 28);
            this.lblClinicName.TabIndex = 0;
            this.lblClinicName.Text = "DermaSoft Clinic";
            this.lblClinicName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlWrapperThanhToan
            // 
            this.pnlWrapperThanhToan.Controls.Add(this.pnlThanhToan);
            this.pnlWrapperThanhToan.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlWrapperThanhToan.Location = new System.Drawing.Point(0, 232);
            this.pnlWrapperThanhToan.Name = "pnlWrapperThanhToan";
            this.pnlWrapperThanhToan.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.pnlWrapperThanhToan.Size = new System.Drawing.Size(351, 261);
            this.pnlWrapperThanhToan.TabIndex = 1;
            // 
            // pnlThanhToan
            // 
            this.pnlThanhToan.BorderRadius = 12;
            this.pnlThanhToan.Controls.Add(this.pnlTienThua);
            this.pnlThanhToan.Controls.Add(this.pnlTienKhach);
            this.pnlThanhToan.Controls.Add(this.pnlPhuongThucTT);
            this.pnlThanhToan.Controls.Add(this.lblTitleTT);
            this.pnlThanhToan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlThanhToan.FillColor = System.Drawing.Color.White;
            this.pnlThanhToan.Location = new System.Drawing.Point(0, 5);
            this.pnlThanhToan.Name = "pnlThanhToan";
            this.pnlThanhToan.Padding = new System.Windows.Forms.Padding(10, 0, 10, 8);
            this.pnlThanhToan.Size = new System.Drawing.Size(351, 251);
            this.pnlThanhToan.TabIndex = 1;
            // 
            // pnlTienThua
            // 
            this.pnlTienThua.BorderRadius = 12;
            this.pnlTienThua.Controls.Add(this.lblTienThuaLabel);
            this.pnlTienThua.Controls.Add(this.lblTienThua);
            this.pnlTienThua.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTienThua.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.pnlTienThua.Location = new System.Drawing.Point(10, 184);
            this.pnlTienThua.Name = "pnlTienThua";
            this.pnlTienThua.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.pnlTienThua.ShadowDecoration.Color = System.Drawing.Color.Transparent;
            this.pnlTienThua.Size = new System.Drawing.Size(331, 59);
            this.pnlTienThua.TabIndex = 4;
            // 
            // lblTienThuaLabel
            // 
            this.lblTienThuaLabel.AutoSize = true;
            this.lblTienThuaLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblTienThuaLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTienThuaLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTienThuaLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTienThuaLabel.Location = new System.Drawing.Point(5, 0);
            this.lblTienThuaLabel.Name = "lblTienThuaLabel";
            this.lblTienThuaLabel.Padding = new System.Windows.Forms.Padding(0, 25, 0, 20);
            this.lblTienThuaLabel.Size = new System.Drawing.Size(77, 65);
            this.lblTienThuaLabel.TabIndex = 4;
            this.lblTienThuaLabel.Text = "Tiền thừa:";
            // 
            // lblTienThua
            // 
            this.lblTienThua.AutoSize = true;
            this.lblTienThua.BackColor = System.Drawing.Color.Transparent;
            this.lblTienThua.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTienThua.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblTienThua.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTienThua.Location = new System.Drawing.Point(295, 0);
            this.lblTienThua.Name = "lblTienThua";
            this.lblTienThua.Padding = new System.Windows.Forms.Padding(0, 23, 0, 20);
            this.lblTienThua.Size = new System.Drawing.Size(31, 66);
            this.lblTienThua.TabIndex = 2;
            this.lblTienThua.Text = "0đ";
            // 
            // pnlTienKhach
            // 
            this.pnlTienKhach.BackColor = System.Drawing.Color.Transparent;
            this.pnlTienKhach.Controls.Add(this.txtTienKhach);
            this.pnlTienKhach.Controls.Add(this.lblTienKhach);
            this.pnlTienKhach.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTienKhach.Location = new System.Drawing.Point(10, 108);
            this.pnlTienKhach.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.pnlTienKhach.Name = "pnlTienKhach";
            this.pnlTienKhach.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlTienKhach.Size = new System.Drawing.Size(331, 76);
            this.pnlTienKhach.TabIndex = 3;
            // 
            // txtTienKhach
            // 
            this.txtTienKhach.BorderColor = System.Drawing.Color.SeaGreen;
            this.txtTienKhach.BorderRadius = 8;
            this.txtTienKhach.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTienKhach.DefaultText = "";
            this.txtTienKhach.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTienKhach.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTienKhach.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTienKhach.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTienKhach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTienKhach.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTienKhach.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTienKhach.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTienKhach.Location = new System.Drawing.Point(0, 30);
            this.txtTienKhach.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTienKhach.Name = "txtTienKhach";
            this.txtTienKhach.PlaceholderText = "";
            this.txtTienKhach.SelectedText = "";
            this.txtTienKhach.Size = new System.Drawing.Size(331, 36);
            this.txtTienKhach.TabIndex = 2;
            this.txtTienKhach.TextChanged += new System.EventHandler(this.txtTienKhach_TextChanged);
            // 
            // lblTienKhach
            // 
            this.lblTienKhach.AutoSize = true;
            this.lblTienKhach.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTienKhach.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTienKhach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTienKhach.Location = new System.Drawing.Point(0, 0);
            this.lblTienKhach.Name = "lblTienKhach";
            this.lblTienKhach.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.lblTienKhach.Size = new System.Drawing.Size(108, 30);
            this.lblTienKhach.TabIndex = 0;
            this.lblTienKhach.Text = "Tiền khách trả";
            // 
            // pnlPhuongThucTT
            // 
            this.pnlPhuongThucTT.BackColor = System.Drawing.Color.Transparent;
            this.pnlPhuongThucTT.Controls.Add(this.cmbPhuongThuc);
            this.pnlPhuongThucTT.Controls.Add(this.lblPhuongThuc);
            this.pnlPhuongThucTT.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPhuongThucTT.Location = new System.Drawing.Point(10, 37);
            this.pnlPhuongThucTT.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.pnlPhuongThucTT.Name = "pnlPhuongThucTT";
            this.pnlPhuongThucTT.Padding = new System.Windows.Forms.Padding(0, 5, 0, 10);
            this.pnlPhuongThucTT.Size = new System.Drawing.Size(331, 71);
            this.pnlPhuongThucTT.TabIndex = 2;
            // 
            // cmbPhuongThuc
            // 
            this.cmbPhuongThuc.BackColor = System.Drawing.Color.Transparent;
            this.cmbPhuongThuc.BorderColor = System.Drawing.Color.SeaGreen;
            this.cmbPhuongThuc.BorderRadius = 10;
            this.cmbPhuongThuc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPhuongThuc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPhuongThuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPhuongThuc.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPhuongThuc.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPhuongThuc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbPhuongThuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbPhuongThuc.ItemHeight = 30;
            this.cmbPhuongThuc.Items.AddRange(new object[] {
            "Tiền mặt",
            "Chuyển khoản",
            "Thẻ"});
            this.cmbPhuongThuc.Location = new System.Drawing.Point(0, 30);
            this.cmbPhuongThuc.Name = "cmbPhuongThuc";
            this.cmbPhuongThuc.Size = new System.Drawing.Size(331, 36);
            this.cmbPhuongThuc.TabIndex = 1;
            // 
            // lblPhuongThuc
            // 
            this.lblPhuongThuc.AutoSize = true;
            this.lblPhuongThuc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPhuongThuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhuongThuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblPhuongThuc.Location = new System.Drawing.Point(0, 5);
            this.lblPhuongThuc.Name = "lblPhuongThuc";
            this.lblPhuongThuc.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblPhuongThuc.Size = new System.Drawing.Size(181, 25);
            this.lblPhuongThuc.TabIndex = 0;
            this.lblPhuongThuc.Text = "Phương thức thanh toán";
            // 
            // lblTitleTT
            // 
            this.lblTitleTT.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleTT.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleTT.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleTT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTitleTT.Location = new System.Drawing.Point(10, 0);
            this.lblTitleTT.Name = "lblTitleTT";
            this.lblTitleTT.Padding = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblTitleTT.Size = new System.Drawing.Size(331, 37);
            this.lblTitleTT.TabIndex = 1;
            this.lblTitleTT.Text = "💳 Nhập Tiền Thanh Toán";
            // 
            // pnlWrapperTongKet
            // 
            this.pnlWrapperTongKet.Controls.Add(this.pnlTongKet);
            this.pnlWrapperTongKet.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlWrapperTongKet.Location = new System.Drawing.Point(0, 0);
            this.pnlWrapperTongKet.Name = "pnlWrapperTongKet";
            this.pnlWrapperTongKet.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.pnlWrapperTongKet.Size = new System.Drawing.Size(351, 232);
            this.pnlWrapperTongKet.TabIndex = 0;
            this.pnlWrapperTongKet.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlWrapperTongKet_Paint);
            // 
            // pnlTongKet
            // 
            this.pnlTongKet.BorderRadius = 12;
            this.pnlTongKet.Controls.Add(this.pnlTong);
            this.pnlTongKet.Controls.Add(this.pnlGiamGia);
            this.pnlTongKet.Controls.Add(this.pnlTamTinh);
            this.pnlTongKet.Controls.Add(this.pnlTongThuoc);
            this.pnlTongKet.Controls.Add(this.pnlTongDV);
            this.pnlTongKet.Controls.Add(this.lblTitleTK);
            this.pnlTongKet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTongKet.FillColor = System.Drawing.Color.White;
            this.pnlTongKet.Location = new System.Drawing.Point(0, 0);
            this.pnlTongKet.Name = "pnlTongKet";
            this.pnlTongKet.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.pnlTongKet.Size = new System.Drawing.Size(351, 227);
            this.pnlTongKet.TabIndex = 0;
            this.pnlTongKet.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTongKet_Paint);
            // 
            // pnlTong
            // 
            this.pnlTong.BackColor = System.Drawing.Color.Transparent;
            this.pnlTong.Controls.Add(this.lblTongTien);
            this.pnlTong.Controls.Add(this.lblTONG);
            this.pnlTong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTong.Location = new System.Drawing.Point(10, 182);
            this.pnlTong.Name = "pnlTong";
            this.pnlTong.Padding = new System.Windows.Forms.Padding(5, 8, 5, 3);
            this.pnlTong.Size = new System.Drawing.Size(331, 45);
            this.pnlTong.TabIndex = 5;
            // 
            // lblTongTien
            // 
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTongTien.Location = new System.Drawing.Point(236, 8);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(90, 23);
            this.lblTongTien.TabIndex = 1;
            this.lblTongTien.Text = "Loading...";
            // 
            // lblTONG
            // 
            this.lblTONG.AutoSize = true;
            this.lblTONG.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTONG.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTONG.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTONG.Location = new System.Drawing.Point(5, 8);
            this.lblTONG.Name = "lblTONG";
            this.lblTONG.Size = new System.Drawing.Size(63, 23);
            this.lblTONG.TabIndex = 0;
            this.lblTONG.Text = "TỔNG:";
            // 
            // pnlGiamGia
            // 
            this.pnlGiamGia.BackColor = System.Drawing.Color.Transparent;
            this.pnlGiamGia.Controls.Add(this.txtGiamGia);
            this.pnlGiamGia.Controls.Add(this.lblGiamGiaLabel);
            this.pnlGiamGia.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGiamGia.Location = new System.Drawing.Point(10, 142);
            this.pnlGiamGia.Name = "pnlGiamGia";
            this.pnlGiamGia.Padding = new System.Windows.Forms.Padding(5);
            this.pnlGiamGia.Size = new System.Drawing.Size(331, 40);
            this.pnlGiamGia.TabIndex = 4;
            // 
            // txtGiamGia
            // 
            this.txtGiamGia.BorderColor = System.Drawing.Color.SeaGreen;
            this.txtGiamGia.BorderRadius = 8;
            this.txtGiamGia.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGiamGia.DefaultText = "";
            this.txtGiamGia.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtGiamGia.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtGiamGia.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtGiamGia.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtGiamGia.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtGiamGia.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtGiamGia.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtGiamGia.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtGiamGia.Location = new System.Drawing.Point(186, 5);
            this.txtGiamGia.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGiamGia.Name = "txtGiamGia";
            this.txtGiamGia.PlaceholderText = "";
            this.txtGiamGia.SelectedText = "";
            this.txtGiamGia.Size = new System.Drawing.Size(140, 30);
            this.txtGiamGia.TabIndex = 1;
            // 
            // lblGiamGiaLabel
            // 
            this.lblGiamGiaLabel.AutoSize = true;
            this.lblGiamGiaLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblGiamGiaLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGiamGiaLabel.Location = new System.Drawing.Point(5, 5);
            this.lblGiamGiaLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblGiamGiaLabel.Name = "lblGiamGiaLabel";
            this.lblGiamGiaLabel.Size = new System.Drawing.Size(73, 20);
            this.lblGiamGiaLabel.TabIndex = 0;
            this.lblGiamGiaLabel.Text = "Giảm giá:";
            this.lblGiamGiaLabel.Click += new System.EventHandler(this.lblGiamGiaLabel_Click);
            // 
            // pnlTamTinh
            // 
            this.pnlTamTinh.BackColor = System.Drawing.Color.Transparent;
            this.pnlTamTinh.Controls.Add(this.lblTamTinhValue);
            this.pnlTamTinh.Controls.Add(this.lblTamTinhLabel);
            this.pnlTamTinh.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTamTinh.Location = new System.Drawing.Point(10, 107);
            this.pnlTamTinh.Name = "pnlTamTinh";
            this.pnlTamTinh.Padding = new System.Windows.Forms.Padding(5);
            this.pnlTamTinh.Size = new System.Drawing.Size(331, 35);
            this.pnlTamTinh.TabIndex = 3;
            // 
            // lblTamTinhValue
            // 
            this.lblTamTinhValue.AutoSize = true;
            this.lblTamTinhValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTamTinhValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTamTinhValue.Location = new System.Drawing.Point(249, 5);
            this.lblTamTinhValue.Name = "lblTamTinhValue";
            this.lblTamTinhValue.Size = new System.Drawing.Size(77, 20);
            this.lblTamTinhValue.TabIndex = 1;
            this.lblTamTinhValue.Text = "Loading...";
            // 
            // lblTamTinhLabel
            // 
            this.lblTamTinhLabel.AutoSize = true;
            this.lblTamTinhLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTamTinhLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTamTinhLabel.Location = new System.Drawing.Point(5, 5);
            this.lblTamTinhLabel.Name = "lblTamTinhLabel";
            this.lblTamTinhLabel.Size = new System.Drawing.Size(73, 20);
            this.lblTamTinhLabel.TabIndex = 0;
            this.lblTamTinhLabel.Text = "Tạm tính:";
            // 
            // pnlTongThuoc
            // 
            this.pnlTongThuoc.BackColor = System.Drawing.Color.Transparent;
            this.pnlTongThuoc.Controls.Add(this.lblTongThuocValue);
            this.pnlTongThuoc.Controls.Add(this.lblTongThuocLabel);
            this.pnlTongThuoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTongThuoc.Location = new System.Drawing.Point(10, 72);
            this.pnlTongThuoc.Name = "pnlTongThuoc";
            this.pnlTongThuoc.Padding = new System.Windows.Forms.Padding(5);
            this.pnlTongThuoc.Size = new System.Drawing.Size(331, 35);
            this.pnlTongThuoc.TabIndex = 2;
            // 
            // lblTongThuocValue
            // 
            this.lblTongThuocValue.AutoSize = true;
            this.lblTongThuocValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTongThuocValue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongThuocValue.Location = new System.Drawing.Point(250, 5);
            this.lblTongThuocValue.Name = "lblTongThuocValue";
            this.lblTongThuocValue.Size = new System.Drawing.Size(76, 20);
            this.lblTongThuocValue.TabIndex = 1;
            this.lblTongThuocValue.Text = "Loading...";
            // 
            // lblTongThuocLabel
            // 
            this.lblTongThuocLabel.AutoSize = true;
            this.lblTongThuocLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTongThuocLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongThuocLabel.Location = new System.Drawing.Point(5, 5);
            this.lblTongThuocLabel.Name = "lblTongThuocLabel";
            this.lblTongThuocLabel.Size = new System.Drawing.Size(91, 20);
            this.lblTongThuocLabel.TabIndex = 0;
            this.lblTongThuocLabel.Text = "Tổng thuốc:";
            // 
            // pnlTongDV
            // 
            this.pnlTongDV.BackColor = System.Drawing.Color.Transparent;
            this.pnlTongDV.Controls.Add(this.lblTongDVValue);
            this.pnlTongDV.Controls.Add(this.lblTongDVLabel);
            this.pnlTongDV.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTongDV.Location = new System.Drawing.Point(10, 37);
            this.pnlTongDV.Name = "pnlTongDV";
            this.pnlTongDV.Padding = new System.Windows.Forms.Padding(5);
            this.pnlTongDV.Size = new System.Drawing.Size(331, 35);
            this.pnlTongDV.TabIndex = 1;
            this.pnlTongDV.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTongDV_Paint);
            // 
            // lblTongDVValue
            // 
            this.lblTongDVValue.AutoSize = true;
            this.lblTongDVValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTongDVValue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongDVValue.Location = new System.Drawing.Point(250, 5);
            this.lblTongDVValue.Name = "lblTongDVValue";
            this.lblTongDVValue.Size = new System.Drawing.Size(76, 20);
            this.lblTongDVValue.TabIndex = 1;
            this.lblTongDVValue.Text = "Loading...";
            // 
            // lblTongDVLabel
            // 
            this.lblTongDVLabel.AutoSize = true;
            this.lblTongDVLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTongDVLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongDVLabel.Location = new System.Drawing.Point(5, 5);
            this.lblTongDVLabel.Name = "lblTongDVLabel";
            this.lblTongDVLabel.Size = new System.Drawing.Size(102, 20);
            this.lblTongDVLabel.TabIndex = 0;
            this.lblTongDVLabel.Text = "Tổng dịch vụ:";
            // 
            // lblTitleTK
            // 
            this.lblTitleTK.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleTK.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleTK.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleTK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTitleTK.Location = new System.Drawing.Point(10, 0);
            this.lblTitleTK.Name = "lblTitleTK";
            this.lblTitleTK.Padding = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblTitleTK.Size = new System.Drawing.Size(331, 37);
            this.lblTitleTK.TabIndex = 0;
            this.lblTitleTK.Text = "🧾 Tổng Kết Hóa Đơn";
            // 
            // InvoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1060, 768);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "InvoiceForm";
            this.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.Text = "Quản lý hóa đơn";
            this.tlpMain.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlWrapperThuoc.ResumeLayout(false);
            this.pnlThuoc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThuoc)).EndInit();
            this.pnlWrapperDichVu.ResumeLayout(false);
            this.pnlDichVu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDichVu)).EndInit();
            this.pnlWrapperBNInfo.ResumeLayout(false);
            this.pnlBNInfo.ResumeLayout(false);
            this.tlpBNInfo.ResumeLayout(false);
            this.pnlChanDoan.ResumeLayout(false);
            this.pnlChanDoan.PerformLayout();
            this.pnlBacSi.ResumeLayout(false);
            this.pnlBacSi.PerformLayout();
            this.pnlNgayKham.ResumeLayout(false);
            this.pnlNgayKham.PerformLayout();
            this.pnlBNTen.ResumeLayout(false);
            this.pnlBNTen.PerformLayout();
            this.pnlPhieuKham.ResumeLayout(false);
            this.pnlWrapperlblPhieuKham.ResumeLayout(false);
            this.pnlWrapperlblPhieuKham.PerformLayout();
            this.pnlButtonTai.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.tlpRightButtons.ResumeLayout(false);
            this.pnlWrapperPreview.ResumeLayout(false);
            this.pnlPreview.ResumeLayout(false);
            this.lblPreviewTongValue.ResumeLayout(false);
            this.lblPreviewTongValue.PerformLayout();
            this.pnlThuocRow.ResumeLayout(false);
            this.pnlThuocRow.PerformLayout();
            this.pnlDVRow.ResumeLayout(false);
            this.pnlDVRow.PerformLayout();
            this.pnlBNRow.ResumeLayout(false);
            this.pnlBNRow.PerformLayout();
            this.pnlDermaSoftClinic.ResumeLayout(false);
            this.pnlWrapperThanhToan.ResumeLayout(false);
            this.pnlThanhToan.ResumeLayout(false);
            this.pnlTienThua.ResumeLayout(false);
            this.pnlTienThua.PerformLayout();
            this.pnlTienKhach.ResumeLayout(false);
            this.pnlTienKhach.PerformLayout();
            this.pnlPhuongThucTT.ResumeLayout(false);
            this.pnlPhuongThucTT.PerformLayout();
            this.pnlWrapperTongKet.ResumeLayout(false);
            this.pnlTongKet.ResumeLayout(false);
            this.pnlTong.ResumeLayout(false);
            this.pnlTong.PerformLayout();
            this.pnlGiamGia.ResumeLayout(false);
            this.pnlGiamGia.PerformLayout();
            this.pnlTamTinh.ResumeLayout(false);
            this.pnlTamTinh.PerformLayout();
            this.pnlTongThuoc.ResumeLayout(false);
            this.pnlTongThuoc.PerformLayout();
            this.pnlTongDV.ResumeLayout(false);
            this.pnlTongDV.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlWrapperBNInfo;
        private System.Windows.Forms.Panel pnlPhieuKham;
        private System.Windows.Forms.Panel pnlRight;
        private Guna.UI2.WinForms.Guna2Panel pnlBNInfo;
        private System.Windows.Forms.Panel pnlWrapperThuoc;
        private System.Windows.Forms.Panel pnlWrapperDichVu;
        private Guna.UI2.WinForms.Guna2Panel pnlDichVu;
        private System.Windows.Forms.Panel pnlButtonTai;
        private Guna.UI2.WinForms.Guna2Button btnTai;
        private Guna.UI2.WinForms.Guna2ComboBox cmbPhieuKham;
        private System.Windows.Forms.Panel pnlWrapperlblPhieuKham;
        private System.Windows.Forms.Label lblPhieuKham;
        private System.Windows.Forms.TableLayoutPanel tlpBNInfo;
        private System.Windows.Forms.Panel pnlBNTen;
        private System.Windows.Forms.Panel pnlChanDoan;
        private System.Windows.Forms.Panel pnlBacSi;
        private System.Windows.Forms.Panel pnlNgayKham;
        private System.Windows.Forms.Label lblLabelBN;
        private System.Windows.Forms.Label lblBNTen;
        private System.Windows.Forms.Label lblLabelBS;
        private System.Windows.Forms.Label lblLabelNgay;
        private System.Windows.Forms.Label lblChanDoan;
        private System.Windows.Forms.Label lblLabelCD;
        private System.Windows.Forms.Label lblBacSi;
        private System.Windows.Forms.Label lblNgayKham;
        private System.Windows.Forms.Label lblTitleDV;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDichVu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDVTenDV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDVSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDVDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDVThanhTien;
        private Guna.UI2.WinForms.Guna2Panel pnlThuoc;
        private Guna.UI2.WinForms.Guna2DataGridView dgvThuoc;
        private System.Windows.Forms.Label lblTitleThuoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThuocTen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThuocSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThuocDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThuocThanhTien;
        private System.Windows.Forms.Panel pnlWrapperPreview;
        private System.Windows.Forms.Panel pnlWrapperThanhToan;
        private System.Windows.Forms.Panel pnlWrapperTongKet;
        private System.Windows.Forms.TableLayoutPanel tlpRightButtons;
        private Guna.UI2.WinForms.Guna2GradientButton btnXacNhan;
        private Guna.UI2.WinForms.Guna2Button btnInHoaDon;
        private Guna.UI2.WinForms.Guna2Panel pnlPreview;
        private Guna.UI2.WinForms.Guna2Panel pnlThanhToan;
        private Guna.UI2.WinForms.Guna2Panel pnlTongKet;
        private System.Windows.Forms.Panel pnlTongDV;
        private System.Windows.Forms.Label lblTongDVLabel;
        private System.Windows.Forms.Label lblTitleTK;
        private System.Windows.Forms.Panel pnlGiamGia;
        private Guna.UI2.WinForms.Guna2TextBox txtGiamGia;
        private System.Windows.Forms.Label lblGiamGiaLabel;
        private System.Windows.Forms.Panel pnlTamTinh;
        private System.Windows.Forms.Label lblTamTinhValue;
        private System.Windows.Forms.Label lblTamTinhLabel;
        private System.Windows.Forms.Panel pnlTongThuoc;
        private System.Windows.Forms.Label lblTongThuocValue;
        private System.Windows.Forms.Label lblTongThuocLabel;
        private System.Windows.Forms.Label lblTongDVValue;
        private System.Windows.Forms.Panel pnlTong;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Label lblTONG;
        private System.Windows.Forms.Label lblTitleTT;
        private System.Windows.Forms.Panel pnlPhuongThucTT;
        private Guna.UI2.WinForms.Guna2ComboBox cmbPhuongThuc;
        private System.Windows.Forms.Label lblPhuongThuc;
        private Guna.UI2.WinForms.Guna2Panel pnlTienThua;
        private System.Windows.Forms.Panel pnlTienKhach;
        private Guna.UI2.WinForms.Guna2TextBox txtTienKhach;
        private System.Windows.Forms.Label lblTienKhach;
        private System.Windows.Forms.Label lblTienThuaLabel;
        private System.Windows.Forms.Label lblTienThua;
        private System.Windows.Forms.Panel pnlDermaSoftClinic;
        private System.Windows.Forms.Label lblClinicName;
        private System.Windows.Forms.Label lblClinicAddress;
        private System.Windows.Forms.Label lblClinicSDT;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private System.Windows.Forms.Panel pnlBNRow;
        private System.Windows.Forms.Label lblPreviewNgay;
        private System.Windows.Forms.Label lblPreviewBN;
        private System.Windows.Forms.Panel pnlThuocRow;
        private System.Windows.Forms.Label lblPreviewThuocValue;
        private System.Windows.Forms.Label lblPreviewThuocLabel;
        private System.Windows.Forms.Panel pnlDVRow;
        private System.Windows.Forms.Label lblPreviewDVValue;
        private System.Windows.Forms.Label lblPreviewDVLabel;
        private System.Windows.Forms.Panel lblPreviewTongValue;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator2;
        private System.Windows.Forms.Label lblPreviewTongLabel;
    }
}
