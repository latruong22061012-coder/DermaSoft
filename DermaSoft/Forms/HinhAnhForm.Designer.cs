namespace DermaSoft.Forms
{
    partial class HinhAnhForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvHinhAnh = new System.Windows.Forms.DataGridView();
            this.colTenFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGhiChu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayChup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThaoTac = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlThumbnails = new Guna.UI2.WinForms.Guna2Panel();
            this.tlpThumbs = new System.Windows.Forms.TableLayoutPanel();
            this.pnlThumb4 = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlSpace6 = new System.Windows.Forms.Panel();
            this.lblUploadText4 = new System.Windows.Forms.Label();
            this.lblPlus4 = new System.Windows.Forms.Label();
            this.pnlThumb3 = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlSpace5 = new System.Windows.Forms.Panel();
            this.lblUploadText3 = new System.Windows.Forms.Label();
            this.lblPlus3 = new System.Windows.Forms.Label();
            this.pnlThumb2 = new Guna.UI2.WinForms.Guna2Panel();
            this.picThumb2 = new System.Windows.Forms.PictureBox();
            this.pnlSpace2 = new System.Windows.Forms.Panel();
            this.btnXoaThumb2 = new Guna.UI2.WinForms.Guna2Button();
            this.pnlThumb1 = new Guna.UI2.WinForms.Guna2Panel();
            this.picThumb1 = new System.Windows.Forms.PictureBox();
            this.pnlSpace1 = new System.Windows.Forms.Panel();
            this.btnXoaThumb1 = new Guna.UI2.WinForms.Guna2Button();
            this.pnlToolbar = new Guna.UI2.WinForms.Guna2Panel();
            this.lblFileHint = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnChonNhieu = new Guna.UI2.WinForms.Guna2Button();
            this.pnlSpace = new System.Windows.Forms.Panel();
            this.btnUploadAnh = new Guna.UI2.WinForms.Guna2GradientButton();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.txtGhiChuAnh = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblGhiChuTitle = new System.Windows.Forms.Label();
            this.btnLuuGhiChu = new Guna.UI2.WinForms.Guna2GradientButton();
            this.pnlPreview = new Guna.UI2.WinForms.Guna2Panel();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.btnPhongTo = new Guna.UI2.WinForms.Guna2Button();
            this.tlpMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHinhAnh)).BeginInit();
            this.pnlThumbnails.SuspendLayout();
            this.tlpThumbs.SuspendLayout();
            this.pnlThumb4.SuspendLayout();
            this.pnlSpace6.SuspendLayout();
            this.pnlThumb3.SuspendLayout();
            this.pnlSpace5.SuspendLayout();
            this.pnlThumb2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picThumb2)).BeginInit();
            this.pnlSpace2.SuspendLayout();
            this.pnlThumb1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picThumb1)).BeginInit();
            this.pnlSpace1.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.BackColor = System.Drawing.Color.Transparent;
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.42857F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.57143F));
            this.tlpMain.Controls.Add(this.pnlLeft, 0, 0);
            this.tlpMain.Controls.Add(this.pnlRight, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(16, 16);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(968, 648);
            this.tlpMain.TabIndex = 0;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.pnlGrid);
            this.pnlLeft.Controls.Add(this.pnlThumbnails);
            this.pnlLeft.Controls.Add(this.pnlToolbar);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(3, 3);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.pnlLeft.Size = new System.Drawing.Size(685, 642);
            this.pnlLeft.TabIndex = 0;
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.dgvHinhAnh);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 252);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(677, 390);
            this.pnlGrid.TabIndex = 2;
            // 
            // dgvHinhAnh
            // 
            this.dgvHinhAnh.AllowUserToAddRows = false;
            this.dgvHinhAnh.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(250)))), ((int)(((byte)(245)))));
            this.dgvHinhAnh.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvHinhAnh.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHinhAnh.BackgroundColor = System.Drawing.Color.White;
            this.dgvHinhAnh.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvHinhAnh.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(15, 92, 77);
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(15, 92, 77);
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHinhAnh.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvHinhAnh.ColumnHeadersHeight = 44;
            this.dgvHinhAnh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.colMaHinhAnh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDuongDan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaHinhAnh.Name = "colMaHinhAnh";
            this.colMaHinhAnh.Visible = false;
            this.colDuongDan.Name = "colDuongDan";
            this.colDuongDan.Visible = false;
            this.dgvHinhAnh.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaHinhAnh, this.colDuongDan,
            this.colTenFile, this.colGhiChu, this.colNgayChup, this.colThaoTac});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(221, 245, 229);
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(15, 92, 77);
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvHinhAnh.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvHinhAnh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHinhAnh.EnableHeadersVisualStyles = false;
            this.dgvHinhAnh.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.dgvHinhAnh.Location = new System.Drawing.Point(0, 0);
            this.dgvHinhAnh.Name = "dgvHinhAnh";
            this.dgvHinhAnh.ReadOnly = true;
            this.dgvHinhAnh.RowHeadersVisible = false;
            this.dgvHinhAnh.RowHeadersWidth = 51;
            this.dgvHinhAnh.RowTemplate.Height = 44;
            this.dgvHinhAnh.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHinhAnh.Size = new System.Drawing.Size(677, 390);
            this.dgvHinhAnh.TabIndex = 0;
            this.dgvHinhAnh.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHinhAnh_CellContentClick);
            // 
            // colTenFile
            // 
            this.colTenFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTenFile.FillWeight = 361.4973F;
            this.colTenFile.HeaderText = "Tên file";
            this.colTenFile.MinimumWidth = 6;
            this.colTenFile.Name = "colTenFile";
            this.colTenFile.ReadOnly = true;
            this.colTenFile.Width = 230;
            // 
            // colGhiChu
            // 
            this.colGhiChu.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGhiChu.FillWeight = 12.83423F;
            this.colGhiChu.HeaderText = "Ghi chú";
            this.colGhiChu.MinimumWidth = 6;
            this.colGhiChu.Name = "colGhiChu";
            this.colGhiChu.ReadOnly = true;
            // 
            // colNgayChup
            // 
            this.colNgayChup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colNgayChup.FillWeight = 12.83423F;
            this.colNgayChup.HeaderText = "Ngày chụp";
            this.colNgayChup.MinimumWidth = 6;
            this.colNgayChup.Name = "colNgayChup";
            this.colNgayChup.ReadOnly = true;
            this.colNgayChup.Width = 160;
            // 
            // colThaoTac
            // 
            this.colThaoTac.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colThaoTac.FillWeight = 12.83423F;
            this.colThaoTac.HeaderText = "Thao tác";
            this.colThaoTac.MinimumWidth = 6;
            this.colThaoTac.Name = "colThaoTac";
            this.colThaoTac.ReadOnly = true;
            this.colThaoTac.Width = 120;
            // 
            // pnlThumbnails
            // 
            this.pnlThumbnails.Controls.Add(this.tlpThumbs);
            this.pnlThumbnails.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlThumbnails.FillColor = System.Drawing.Color.Transparent;
            this.pnlThumbnails.Location = new System.Drawing.Point(0, 52);
            this.pnlThumbnails.Name = "pnlThumbnails";
            this.pnlThumbnails.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlThumbnails.Size = new System.Drawing.Size(677, 200);
            this.pnlThumbnails.TabIndex = 1;
            // 
            // tlpThumbs
            // 
            this.tlpThumbs.ColumnCount = 4;
            this.tlpThumbs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpThumbs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpThumbs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpThumbs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpThumbs.Controls.Add(this.pnlThumb4, 3, 0);
            this.tlpThumbs.Controls.Add(this.pnlThumb3, 2, 0);
            this.tlpThumbs.Controls.Add(this.pnlThumb2, 1, 0);
            this.tlpThumbs.Controls.Add(this.pnlThumb1, 0, 0);
            this.tlpThumbs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpThumbs.Location = new System.Drawing.Point(0, 0);
            this.tlpThumbs.Name = "tlpThumbs";
            this.tlpThumbs.RowCount = 1;
            this.tlpThumbs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpThumbs.Size = new System.Drawing.Size(677, 190);
            this.tlpThumbs.TabIndex = 0;
            // 
            // pnlThumb4
            // 
            this.pnlThumb4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(230)))), ((int)(((byte)(212)))));
            this.pnlThumb4.BorderRadius = 12;
            this.pnlThumb4.BorderThickness = 1;
            this.pnlThumb4.Controls.Add(this.pnlSpace6);
            this.pnlThumb4.Controls.Add(this.lblPlus4);
            this.pnlThumb4.CustomBorderThickness = new System.Windows.Forms.Padding(1);
            this.pnlThumb4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlThumb4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(238)))));
            this.pnlThumb4.Location = new System.Drawing.Point(507, 0);
            this.pnlThumb4.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.pnlThumb4.Name = "pnlThumb4";
            this.pnlThumb4.Size = new System.Drawing.Size(162, 190);
            this.pnlThumb4.TabIndex = 3;
            // 
            // pnlSpace6
            // 
            this.pnlSpace6.Controls.Add(this.lblUploadText4);
            this.pnlSpace6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSpace6.Location = new System.Drawing.Point(0, 100);
            this.pnlSpace6.Name = "pnlSpace6";
            this.pnlSpace6.Size = new System.Drawing.Size(162, 90);
            this.pnlSpace6.TabIndex = 3;
            // 
            // lblUploadText4
            // 
            this.lblUploadText4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblUploadText4.AutoSize = true;
            this.lblUploadText4.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUploadText4.Location = new System.Drawing.Point(59, 27);
            this.lblUploadText4.Name = "lblUploadText4";
            this.lblUploadText4.Size = new System.Drawing.Size(51, 17);
            this.lblUploadText4.TabIndex = 2;
            this.lblUploadText4.Text = "Upload";
            this.lblUploadText4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPlus4
            // 
            this.lblPlus4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblPlus4.AutoSize = true;
            this.lblPlus4.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlus4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(213)))), ((int)(((byte)(197)))));
            this.lblPlus4.Location = new System.Drawing.Point(60, 43);
            this.lblPlus4.Name = "lblPlus4";
            this.lblPlus4.Size = new System.Drawing.Size(50, 54);
            this.lblPlus4.TabIndex = 1;
            this.lblPlus4.Text = "+";
            this.lblPlus4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlThumb3
            // 
            this.pnlThumb3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(230)))), ((int)(((byte)(212)))));
            this.pnlThumb3.BorderRadius = 12;
            this.pnlThumb3.BorderThickness = 1;
            this.pnlThumb3.Controls.Add(this.pnlSpace5);
            this.pnlThumb3.Controls.Add(this.lblPlus3);
            this.pnlThumb3.CustomBorderThickness = new System.Windows.Forms.Padding(1);
            this.pnlThumb3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlThumb3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(238)))));
            this.pnlThumb3.Location = new System.Drawing.Point(338, 0);
            this.pnlThumb3.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.pnlThumb3.Name = "pnlThumb3";
            this.pnlThumb3.Size = new System.Drawing.Size(161, 190);
            this.pnlThumb3.TabIndex = 2;
            // 
            // pnlSpace5
            // 
            this.pnlSpace5.Controls.Add(this.lblUploadText3);
            this.pnlSpace5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSpace5.Location = new System.Drawing.Point(0, 100);
            this.pnlSpace5.Name = "pnlSpace5";
            this.pnlSpace5.Size = new System.Drawing.Size(161, 90);
            this.pnlSpace5.TabIndex = 2;
            // 
            // lblUploadText3
            // 
            this.lblUploadText3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblUploadText3.AutoSize = true;
            this.lblUploadText3.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUploadText3.Location = new System.Drawing.Point(53, 27);
            this.lblUploadText3.Name = "lblUploadText3";
            this.lblUploadText3.Size = new System.Drawing.Size(51, 17);
            this.lblUploadText3.TabIndex = 1;
            this.lblUploadText3.Text = "Upload";
            this.lblUploadText3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPlus3
            // 
            this.lblPlus3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblPlus3.AutoSize = true;
            this.lblPlus3.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlus3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(213)))), ((int)(((byte)(197)))));
            this.lblPlus3.Location = new System.Drawing.Point(54, 43);
            this.lblPlus3.Name = "lblPlus3";
            this.lblPlus3.Size = new System.Drawing.Size(50, 54);
            this.lblPlus3.TabIndex = 0;
            this.lblPlus3.Text = "+";
            this.lblPlus3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlThumb2
            // 
            this.pnlThumb2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(230)))), ((int)(((byte)(212)))));
            this.pnlThumb2.BorderRadius = 12;
            this.pnlThumb2.Controls.Add(this.picThumb2);
            this.pnlThumb2.Controls.Add(this.pnlSpace2);
            this.pnlThumb2.CustomBorderThickness = new System.Windows.Forms.Padding(1);
            this.pnlThumb2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlThumb2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(238)))));
            this.pnlThumb2.Location = new System.Drawing.Point(169, 0);
            this.pnlThumb2.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.pnlThumb2.Name = "pnlThumb2";
            this.pnlThumb2.Size = new System.Drawing.Size(161, 190);
            this.pnlThumb2.TabIndex = 1;
            // 
            // picThumb2
            // 
            this.picThumb2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picThumb2.Location = new System.Drawing.Point(0, 31);
            this.picThumb2.Name = "picThumb2";
            this.picThumb2.Size = new System.Drawing.Size(161, 159);
            this.picThumb2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picThumb2.TabIndex = 3;
            this.picThumb2.TabStop = false;
            // 
            // pnlSpace2
            // 
            this.pnlSpace2.Controls.Add(this.btnXoaThumb2);
            this.pnlSpace2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpace2.Location = new System.Drawing.Point(0, 0);
            this.pnlSpace2.Name = "pnlSpace2";
            this.pnlSpace2.Size = new System.Drawing.Size(161, 31);
            this.pnlSpace2.TabIndex = 1;
            // 
            // btnXoaThumb2
            // 
            this.btnXoaThumb2.BorderRadius = 2;
            this.btnXoaThumb2.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnXoaThumb2.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnXoaThumb2.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnXoaThumb2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnXoaThumb2.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnXoaThumb2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnXoaThumb2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoaThumb2.ForeColor = System.Drawing.Color.White;
            this.btnXoaThumb2.Location = new System.Drawing.Point(121, 0);
            this.btnXoaThumb2.Name = "btnXoaThumb2";
            this.btnXoaThumb2.Size = new System.Drawing.Size(40, 31);
            this.btnXoaThumb2.TabIndex = 0;
            this.btnXoaThumb2.Text = "X";
            this.btnXoaThumb2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // pnlThumb1
            // 
            this.pnlThumb1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(230)))), ((int)(((byte)(212)))));
            this.pnlThumb1.BorderRadius = 12;
            this.pnlThumb1.Controls.Add(this.picThumb1);
            this.pnlThumb1.Controls.Add(this.pnlSpace1);
            this.pnlThumb1.CustomBorderThickness = new System.Windows.Forms.Padding(1);
            this.pnlThumb1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlThumb1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(238)))));
            this.pnlThumb1.Location = new System.Drawing.Point(0, 0);
            this.pnlThumb1.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.pnlThumb1.Name = "pnlThumb1";
            this.pnlThumb1.Size = new System.Drawing.Size(161, 190);
            this.pnlThumb1.TabIndex = 0;
            // 
            // picThumb1
            // 
            this.picThumb1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picThumb1.Location = new System.Drawing.Point(0, 30);
            this.picThumb1.Name = "picThumb1";
            this.picThumb1.Size = new System.Drawing.Size(161, 160);
            this.picThumb1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picThumb1.TabIndex = 2;
            this.picThumb1.TabStop = false;
            // 
            // pnlSpace1
            // 
            this.pnlSpace1.Controls.Add(this.btnXoaThumb1);
            this.pnlSpace1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpace1.Location = new System.Drawing.Point(0, 0);
            this.pnlSpace1.Name = "pnlSpace1";
            this.pnlSpace1.Size = new System.Drawing.Size(161, 30);
            this.pnlSpace1.TabIndex = 0;
            // 
            // btnXoaThumb1
            // 
            this.btnXoaThumb1.BorderRadius = 2;
            this.btnXoaThumb1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnXoaThumb1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnXoaThumb1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnXoaThumb1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnXoaThumb1.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnXoaThumb1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnXoaThumb1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoaThumb1.ForeColor = System.Drawing.Color.White;
            this.btnXoaThumb1.Location = new System.Drawing.Point(123, 0);
            this.btnXoaThumb1.Name = "btnXoaThumb1";
            this.btnXoaThumb1.Size = new System.Drawing.Size(38, 30);
            this.btnXoaThumb1.TabIndex = 0;
            this.btnXoaThumb1.Text = "X";
            this.btnXoaThumb1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.Controls.Add(this.lblFileHint);
            this.pnlToolbar.Controls.Add(this.panel1);
            this.pnlToolbar.Controls.Add(this.btnChonNhieu);
            this.pnlToolbar.Controls.Add(this.pnlSpace);
            this.pnlToolbar.Controls.Add(this.btnUploadAnh);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.FillColor = System.Drawing.Color.Transparent;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 0);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlToolbar.Size = new System.Drawing.Size(677, 52);
            this.pnlToolbar.TabIndex = 0;
            // 
            // lblFileHint
            // 
            this.lblFileHint.AutoSize = true;
            this.lblFileHint.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.lblFileHint.Location = new System.Drawing.Point(286, 13);
            this.lblFileHint.Name = "lblFileHint";
            this.lblFileHint.Size = new System.Drawing.Size(230, 20);
            this.lblFileHint.TabIndex = 5;
            this.lblFileHint.Text = "JPG, PNG, BMP · Tối đa 10MB/ảnh";
            this.lblFileHint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblFileHint.Click += new System.EventHandler(this.lblFileHint_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(265, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(15, 42);
            this.panel1.TabIndex = 4;
            // 
            // btnChonNhieu
            // 
            this.btnChonNhieu.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(230)))), ((int)(((byte)(207)))));
            this.btnChonNhieu.BorderRadius = 20;
            this.btnChonNhieu.BorderThickness = 1;
            this.btnChonNhieu.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnChonNhieu.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnChonNhieu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnChonNhieu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnChonNhieu.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnChonNhieu.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.btnChonNhieu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChonNhieu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnChonNhieu.Location = new System.Drawing.Point(145, 0);
            this.btnChonNhieu.Name = "btnChonNhieu";
            this.btnChonNhieu.Size = new System.Drawing.Size(120, 42);
            this.btnChonNhieu.TabIndex = 3;
            this.btnChonNhieu.Text = "Chọn Nhiều";
            // 
            // pnlSpace
            // 
            this.pnlSpace.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSpace.Location = new System.Drawing.Point(130, 0);
            this.pnlSpace.Name = "pnlSpace";
            this.pnlSpace.Size = new System.Drawing.Size(15, 42);
            this.pnlSpace.TabIndex = 2;
            // 
            // btnUploadAnh
            // 
            this.btnUploadAnh.BorderRadius = 20;
            this.btnUploadAnh.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUploadAnh.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnUploadAnh.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnUploadAnh.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnUploadAnh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnUploadAnh.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnUploadAnh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnUploadAnh.FillColor2 = System.Drawing.Color.SeaGreen;
            this.btnUploadAnh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadAnh.ForeColor = System.Drawing.Color.White;
            this.btnUploadAnh.Location = new System.Drawing.Point(0, 0);
            this.btnUploadAnh.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnUploadAnh.Name = "btnUploadAnh";
            this.btnUploadAnh.Size = new System.Drawing.Size(130, 42);
            this.btnUploadAnh.TabIndex = 0;
            this.btnUploadAnh.Text = "Upload Ảnh";
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.Transparent;
            this.pnlRight.Controls.Add(this.txtGhiChuAnh);
            this.pnlRight.Controls.Add(this.lblGhiChuTitle);
            this.pnlRight.Controls.Add(this.btnLuuGhiChu);
            this.pnlRight.Controls.Add(this.pnlPreview);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(694, 3);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(271, 642);
            this.pnlRight.TabIndex = 1;
            // 
            // txtGhiChuAnh
            // 
            this.txtGhiChuAnh.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(220)))));
            this.txtGhiChuAnh.BorderRadius = 8;
            this.txtGhiChuAnh.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGhiChuAnh.DefaultText = "";
            this.txtGhiChuAnh.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtGhiChuAnh.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtGhiChuAnh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtGhiChuAnh.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtGhiChuAnh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGhiChuAnh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(252)))), ((int)(((byte)(250)))));
            this.txtGhiChuAnh.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtGhiChuAnh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtGhiChuAnh.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtGhiChuAnh.Location = new System.Drawing.Point(0, 252);
            this.txtGhiChuAnh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGhiChuAnh.Multiline = true;
            this.txtGhiChuAnh.Name = "txtGhiChuAnh";
            this.txtGhiChuAnh.Padding = new System.Windows.Forms.Padding(8);
            this.txtGhiChuAnh.PlaceholderText = "Vùng trán nổi mụn đỏ, có dịch...";
            this.txtGhiChuAnh.SelectedText = "";
            this.txtGhiChuAnh.Size = new System.Drawing.Size(271, 348);
            this.txtGhiChuAnh.TabIndex = 3;
            // 
            // lblGhiChuTitle
            // 
            this.lblGhiChuTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGhiChuTitle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGhiChuTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblGhiChuTitle.Location = new System.Drawing.Point(0, 220);
            this.lblGhiChuTitle.Name = "lblGhiChuTitle";
            this.lblGhiChuTitle.Size = new System.Drawing.Size(271, 32);
            this.lblGhiChuTitle.TabIndex = 2;
            this.lblGhiChuTitle.Text = "Ghi Chú Ảnh";
            this.lblGhiChuTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLuuGhiChu
            // 
            this.btnLuuGhiChu.BorderRadius = 20;
            this.btnLuuGhiChu.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLuuGhiChu.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLuuGhiChu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLuuGhiChu.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLuuGhiChu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLuuGhiChu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLuuGhiChu.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnLuuGhiChu.FillColor2 = System.Drawing.Color.SeaGreen;
            this.btnLuuGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuuGhiChu.ForeColor = System.Drawing.Color.White;
            this.btnLuuGhiChu.Location = new System.Drawing.Point(0, 600);
            this.btnLuuGhiChu.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.btnLuuGhiChu.Name = "btnLuuGhiChu";
            this.btnLuuGhiChu.Size = new System.Drawing.Size(271, 42);
            this.btnLuuGhiChu.TabIndex = 1;
            this.btnLuuGhiChu.Text = "Lưu Ghi Chú";
            // 
            // pnlPreview
            // 
            this.pnlPreview.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(230)))), ((int)(((byte)(212)))));
            this.pnlPreview.BorderRadius = 12;
            this.pnlPreview.BorderThickness = 1;
            this.pnlPreview.Controls.Add(this.picPreview);
            this.pnlPreview.Controls.Add(this.btnPhongTo);
            this.pnlPreview.CustomBorderThickness = new System.Windows.Forms.Padding(1);
            this.pnlPreview.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPreview.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(238)))));
            this.pnlPreview.Location = new System.Drawing.Point(0, 0);
            this.pnlPreview.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlPreview.Name = "pnlPreview";
            this.pnlPreview.Padding = new System.Windows.Forms.Padding(10);
            this.pnlPreview.Size = new System.Drawing.Size(271, 220);
            this.pnlPreview.TabIndex = 0;
            // 
            // picPreview
            // 
            this.picPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPreview.Location = new System.Drawing.Point(10, 10);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(251, 169);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 1;
            this.picPreview.TabStop = false;
            // 
            // btnPhongTo
            // 
            this.btnPhongTo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(230)))), ((int)(((byte)(212)))));
            this.btnPhongTo.BorderRadius = 8;
            this.btnPhongTo.BorderThickness = 1;
            this.btnPhongTo.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPhongTo.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPhongTo.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPhongTo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPhongTo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnPhongTo.FillColor = System.Drawing.Color.Transparent;
            this.btnPhongTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnPhongTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnPhongTo.Location = new System.Drawing.Point(10, 179);
            this.btnPhongTo.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.btnPhongTo.Name = "btnPhongTo";
            this.btnPhongTo.Size = new System.Drawing.Size(251, 31);
            this.btnPhongTo.TabIndex = 0;
            this.btnPhongTo.Text = "Phóng To";
            // 
            // HinhAnhForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1000, 680);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HinhAnhForm";
            this.Padding = new System.Windows.Forms.Padding(16);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hình Ảnh Bệnh Lý";
            this.tlpMain.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHinhAnh)).EndInit();
            this.pnlThumbnails.ResumeLayout(false);
            this.tlpThumbs.ResumeLayout(false);
            this.pnlThumb4.ResumeLayout(false);
            this.pnlThumb4.PerformLayout();
            this.pnlSpace6.ResumeLayout(false);
            this.pnlSpace6.PerformLayout();
            this.pnlThumb3.ResumeLayout(false);
            this.pnlThumb3.PerformLayout();
            this.pnlSpace5.ResumeLayout(false);
            this.pnlSpace5.PerformLayout();
            this.pnlThumb2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picThumb2)).EndInit();
            this.pnlSpace2.ResumeLayout(false);
            this.pnlThumb1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picThumb1)).EndInit();
            this.pnlSpace1.ResumeLayout(false);
            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.pnlPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlLeft;
        private Guna.UI2.WinForms.Guna2Panel pnlToolbar;
        private Guna.UI2.WinForms.Guna2GradientButton btnUploadAnh;
        private Guna.UI2.WinForms.Guna2Button btnChonNhieu;
        private System.Windows.Forms.Panel pnlSpace;
        private System.Windows.Forms.Label lblFileHint;
        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2Panel pnlThumbnails;
        private System.Windows.Forms.TableLayoutPanel tlpThumbs;
        private Guna.UI2.WinForms.Guna2Panel pnlThumb4;
        private Guna.UI2.WinForms.Guna2Panel pnlThumb3;
        private Guna.UI2.WinForms.Guna2Panel pnlThumb2;
        private Guna.UI2.WinForms.Guna2Panel pnlThumb1;
        private Guna.UI2.WinForms.Guna2Button btnXoaThumb1;
        private System.Windows.Forms.Panel pnlSpace1;
        private System.Windows.Forms.Panel pnlSpace2;
        private Guna.UI2.WinForms.Guna2Button btnXoaThumb2;
        private System.Windows.Forms.PictureBox picThumb1;
        private System.Windows.Forms.Label lblPlus3;
        private System.Windows.Forms.PictureBox picThumb2;
        private System.Windows.Forms.Label lblUploadText4;
        private System.Windows.Forms.Label lblPlus4;
        private System.Windows.Forms.Label lblUploadText3;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvHinhAnh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGhiChu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayChup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThaoTac;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaHinhAnh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDuongDan;
        private System.Windows.Forms.Panel pnlRight;
        private Guna.UI2.WinForms.Guna2Panel pnlPreview;
        private Guna.UI2.WinForms.Guna2Button btnPhongTo;
        private Guna.UI2.WinForms.Guna2GradientButton btnLuuGhiChu;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Label lblGhiChuTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtGhiChuAnh;
        private System.Windows.Forms.Panel pnlSpace6;
        private System.Windows.Forms.Panel pnlSpace5;
=======
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 680);
            this.Name = "HinhAnhForm";
            this.Text = "Hình Ảnh Bệnh Lý";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }
>>>>>>> d2fc9d190a76c0c366e0407bca6067fe95379af1
    }
}
