<<<<<<< HEAD
using System.Windows.Forms;
using Guna.UI2.WinForms;

=======
>>>>>>> d2fc9d190a76c0c366e0407bca6067fe95379af1
namespace DermaSoft.Forms
{
    partial class PatientForm
    {
        private System.ComponentModel.IContainer components = null;
<<<<<<< HEAD

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLeft = new Guna.UI2.WinForms.Guna2Panel();
            this.tlpLeftInner = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.txtTimKiem = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlBtnGroup = new System.Windows.Forms.Panel();
            this.btnLamMoi = new Guna.UI2.WinForms.Guna2Button();
            this.btnThemMoi = new Guna.UI2.WinForms.Guna2GradientButton();
            this.pnlFilterBar = new System.Windows.Forms.Panel();
            this.lblTongSo = new System.Windows.Forms.Label();
            this.cmbLoc = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblLocLabel = new System.Windows.Forms.Label();
            this.dgvBenhNhan = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colMaBN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHoTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgaySinh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSDT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGioiTinh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhVien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThaoTac = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlRight = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlFormFields = new System.Windows.Forms.Panel();
            this.pnlTienSu = new System.Windows.Forms.Panel();
            this.txtTienSu = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTienSu = new System.Windows.Forms.Label();
            this.pnlSDT = new System.Windows.Forms.Panel();
            this.txtSDT = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblSDT = new System.Windows.Forms.Label();
            this.pnlNgaySinhGioiTinh = new System.Windows.Forms.Panel();
            this.tlpNSGT = new System.Windows.Forms.TableLayoutPanel();
            this.pnlGioiTinh = new System.Windows.Forms.Panel();
            this.cmbGioiTinh = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblGioiTinh = new System.Windows.Forms.Label();
            this.pnlNgaySinh = new System.Windows.Forms.Panel();
            this.dtpNgaySinh = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblNgaySinh = new System.Windows.Forms.Label();
            this.pnlHoTen = new System.Windows.Forms.Panel();
            this.txtHoTen = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblHoTen = new System.Windows.Forms.Label();
            this.pnlThanhVienCard = new Guna.UI2.WinForms.Guna2Panel();
            this.lblThanhVienInfo = new System.Windows.Forms.Label();
            this.lblThanhVienTitle = new System.Windows.Forms.Label();
            this.pnlDetailButtons = new System.Windows.Forms.Panel();
            this.tlpDetailButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnXoa = new Guna.UI2.WinForms.Guna2Button();
            this.btnLuuThayDoi = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnXemLichSuKham = new Guna.UI2.WinForms.Guna2Button();
            this.pnlDetailHeader = new System.Windows.Forms.Panel();
            this.lblTitleDetail = new System.Windows.Forms.Label();
            this.tlpMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.tlpLeftInner.SuspendLayout();
            this.pnlTopBar.SuspendLayout();
            this.pnlBtnGroup.SuspendLayout();
            this.pnlFilterBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBenhNhan)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.pnlFormFields.SuspendLayout();
            this.pnlTienSu.SuspendLayout();
            this.pnlSDT.SuspendLayout();
            this.pnlNgaySinhGioiTinh.SuspendLayout();
            this.tlpNSGT.SuspendLayout();
            this.pnlGioiTinh.SuspendLayout();
            this.pnlNgaySinh.SuspendLayout();
            this.pnlHoTen.SuspendLayout();
            this.pnlThanhVienCard.SuspendLayout();
            this.pnlDetailButtons.SuspendLayout();
            this.tlpDetailButtons.SuspendLayout();
            this.pnlDetailHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpMain.Controls.Add(this.pnlLeft, 0, 0);
            this.tlpMain.Controls.Add(this.pnlRight, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(25, 25);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(1550, 950);
            this.tlpMain.TabIndex = 0;
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.Transparent;
            this.pnlLeft.BorderRadius = 12;
            this.pnlLeft.Controls.Add(this.tlpLeftInner);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.FillColor = System.Drawing.Color.White;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(16);
            this.pnlLeft.ShadowDecoration.Color = System.Drawing.Color.Gainsboro;
            this.pnlLeft.ShadowDecoration.Depth = 4;
            this.pnlLeft.ShadowDecoration.Enabled = true;
            this.pnlLeft.Size = new System.Drawing.Size(1073, 950);
            this.pnlLeft.TabIndex = 0;
            this.pnlLeft.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlLeft_Paint);
            // 
            // tlpLeftInner
            // 
            this.tlpLeftInner.ColumnCount = 1;
            this.tlpLeftInner.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLeftInner.Controls.Add(this.pnlTopBar, 0, 0);
            this.tlpLeftInner.Controls.Add(this.pnlFilterBar, 0, 1);
            this.tlpLeftInner.Controls.Add(this.dgvBenhNhan, 0, 2);
            this.tlpLeftInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLeftInner.Location = new System.Drawing.Point(16, 16);
            this.tlpLeftInner.Name = "tlpLeftInner";
            this.tlpLeftInner.RowCount = 3;
            this.tlpLeftInner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpLeftInner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tlpLeftInner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLeftInner.Size = new System.Drawing.Size(1041, 918);
            this.tlpLeftInner.TabIndex = 0;
            // 
            // pnlTopBar
            // 
            this.pnlTopBar.Controls.Add(this.txtTimKiem);
            this.pnlTopBar.Controls.Add(this.pnlBtnGroup);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopBar.Location = new System.Drawing.Point(3, 3);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.pnlTopBar.Size = new System.Drawing.Size(1035, 54);
            this.pnlTopBar.TabIndex = 0;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.BorderRadius = 15;
            this.txtTimKiem.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimKiem.DefaultText = "";
            this.txtTimKiem.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTimKiem.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTimKiem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTimKiem.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTimKiem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTimKiem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTimKiem.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtTimKiem.Location = new System.Drawing.Point(0, 8);
            this.txtTimKiem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "🔍 Tìm kiếm theo tên, SĐT, mã bệnh nhân...";
            this.txtTimKiem.SelectedText = "";
            this.txtTimKiem.Size = new System.Drawing.Size(685, 38);
            this.txtTimKiem.TabIndex = 2;
            // 
            // pnlBtnGroup
            // 
            this.pnlBtnGroup.Controls.Add(this.btnLamMoi);
            this.pnlBtnGroup.Controls.Add(this.btnThemMoi);
            this.pnlBtnGroup.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlBtnGroup.Location = new System.Drawing.Point(685, 8);
            this.pnlBtnGroup.Name = "pnlBtnGroup";
            this.pnlBtnGroup.Padding = new System.Windows.Forms.Padding(50, 0, 50, 0);
            this.pnlBtnGroup.Size = new System.Drawing.Size(350, 38);
            this.pnlBtnGroup.TabIndex = 1;
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(230)))), ((int)(((byte)(207)))));
            this.btnLamMoi.BorderRadius = 15;
            this.btnLamMoi.BorderThickness = 1;
            this.btnLamMoi.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLamMoi.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLamMoi.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLamMoi.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLamMoi.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLamMoi.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLamMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnLamMoi.Location = new System.Drawing.Point(190, 0);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(110, 38);
            this.btnLamMoi.TabIndex = 1;
            this.btnLamMoi.Text = "Làm Mới";
            // 
            // btnThemMoi
            // 
            this.btnThemMoi.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnThemMoi.BorderRadius = 15;
            this.btnThemMoi.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnThemMoi.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnThemMoi.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnThemMoi.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnThemMoi.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnThemMoi.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnThemMoi.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnThemMoi.FillColor2 = System.Drawing.Color.SeaGreen;
            this.btnThemMoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemMoi.ForeColor = System.Drawing.Color.White;
            this.btnThemMoi.Location = new System.Drawing.Point(50, 0);
            this.btnThemMoi.Name = "btnThemMoi";
            this.btnThemMoi.Size = new System.Drawing.Size(110, 38);
            this.btnThemMoi.TabIndex = 0;
            this.btnThemMoi.Text = "Thêm Mới";
            // 
            // pnlFilterBar
            // 
            this.pnlFilterBar.Controls.Add(this.lblTongSo);
            this.pnlFilterBar.Controls.Add(this.cmbLoc);
            this.pnlFilterBar.Controls.Add(this.lblLocLabel);
            this.pnlFilterBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFilterBar.Location = new System.Drawing.Point(3, 63);
            this.pnlFilterBar.Name = "pnlFilterBar";
            this.pnlFilterBar.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.pnlFilterBar.Size = new System.Drawing.Size(1035, 53);
            this.pnlFilterBar.TabIndex = 1;
            // 
            // lblTongSo
            // 
            this.lblTongSo.AutoSize = true;
            this.lblTongSo.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTongSo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongSo.Location = new System.Drawing.Point(869, 4);
            this.lblTongSo.Name = "lblTongSo";
            this.lblTongSo.Padding = new System.Windows.Forms.Padding(20, 6, 6, 6);
            this.lblTongSo.Size = new System.Drawing.Size(166, 32);
            this.lblTongSo.TabIndex = 2;
            this.lblTongSo.Text = "Tổng: 0 bệnh nhân";
            this.lblTongSo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbLoc
            // 
            this.cmbLoc.BackColor = System.Drawing.Color.Transparent;
            this.cmbLoc.BorderRadius = 10;
            this.cmbLoc.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbLoc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbLoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoc.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.cmbLoc.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.cmbLoc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbLoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbLoc.ItemHeight = 30;
            this.cmbLoc.Items.AddRange(new object[] {
            "Tất cả bệnh nhân",
            "Có thẻ thành viên",
            "Chưa có thẻ"});
            this.cmbLoc.Location = new System.Drawing.Point(43, 4);
            this.cmbLoc.Name = "cmbLoc";
            this.cmbLoc.Size = new System.Drawing.Size(200, 36);
            this.cmbLoc.TabIndex = 1;
            // 
            // lblLocLabel
            // 
            this.lblLocLabel.AutoSize = true;
            this.lblLocLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblLocLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocLabel.Location = new System.Drawing.Point(0, 4);
            this.lblLocLabel.Margin = new System.Windows.Forms.Padding(0);
            this.lblLocLabel.Name = "lblLocLabel";
            this.lblLocLabel.Padding = new System.Windows.Forms.Padding(0, 6, 6, 6);
            this.lblLocLabel.Size = new System.Drawing.Size(43, 32);
            this.lblLocLabel.TabIndex = 0;
            this.lblLocLabel.Text = "Lọc:";
            this.lblLocLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvBenhNhan
            // 
            this.dgvBenhNhan.AllowUserToAddRows = false;
            this.dgvBenhNhan.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvBenhNhan.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBenhNhan.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBenhNhan.ColumnHeadersHeight = 42;
            this.dgvBenhNhan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaBN,
            this.colHoTen,
            this.colNgaySinh,
            this.colSDT,
            this.colGioiTinh,
            this.colThanhVien,
            this.colThaoTac});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBenhNhan.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvBenhNhan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBenhNhan.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvBenhNhan.Location = new System.Drawing.Point(3, 122);
            this.dgvBenhNhan.Name = "dgvBenhNhan";
            this.dgvBenhNhan.ReadOnly = true;
            this.dgvBenhNhan.RowHeadersVisible = false;
            this.dgvBenhNhan.RowHeadersWidth = 51;
            this.dgvBenhNhan.RowTemplate.Height = 42;
            this.dgvBenhNhan.Size = new System.Drawing.Size(1035, 793);
            this.dgvBenhNhan.TabIndex = 2;
            this.dgvBenhNhan.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvBenhNhan.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvBenhNhan.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvBenhNhan.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvBenhNhan.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvBenhNhan.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvBenhNhan.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvBenhNhan.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.dgvBenhNhan.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvBenhNhan.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvBenhNhan.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvBenhNhan.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvBenhNhan.ThemeStyle.HeaderStyle.Height = 42;
            this.dgvBenhNhan.ThemeStyle.ReadOnly = true;
            this.dgvBenhNhan.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvBenhNhan.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvBenhNhan.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvBenhNhan.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvBenhNhan.ThemeStyle.RowsStyle.Height = 42;
            this.dgvBenhNhan.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.dgvBenhNhan.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            // 
            // colMaBN
            // 
            this.colMaBN.DataPropertyName = "MaBNText";
            this.colMaBN.FillWeight = 65F;
            this.colMaBN.HeaderText = "Mã BN";
            this.colMaBN.MinimumWidth = 6;
            this.colMaBN.Name = "colMaBN";
            this.colMaBN.ReadOnly = true;
            // 
            // colHoTen
            // 
            this.colHoTen.DataPropertyName = "HoTen";
            this.colHoTen.FillWeight = 165F;
            this.colHoTen.HeaderText = "Họ tên";
            this.colHoTen.MinimumWidth = 6;
            this.colHoTen.Name = "colHoTen";
            this.colHoTen.ReadOnly = true;
            // 
            // colNgaySinh
            // 
            this.colNgaySinh.DataPropertyName = "NgaySinhText";
            this.colNgaySinh.HeaderText = "Ngày sinh";
            this.colNgaySinh.MinimumWidth = 6;
            this.colNgaySinh.Name = "colNgaySinh";
            this.colNgaySinh.ReadOnly = true;
            // 
            // colSDT
            // 
            this.colSDT.DataPropertyName = "SoDienThoai";
            this.colSDT.FillWeight = 110F;
            this.colSDT.HeaderText = "SĐT";
            this.colSDT.MinimumWidth = 6;
            this.colSDT.Name = "colSDT";
            this.colSDT.ReadOnly = true;
            // 
            // colGioiTinh
            // 
            this.colGioiTinh.DataPropertyName = "GioiTinhText";
            this.colGioiTinh.FillWeight = 75F;
            this.colGioiTinh.HeaderText = "Giới tính";
            this.colGioiTinh.MinimumWidth = 6;
            this.colGioiTinh.Name = "colGioiTinh";
            this.colGioiTinh.ReadOnly = true;
            // 
            // colThanhVien
            // 
            this.colThanhVien.DataPropertyName = "HangThanhVien";
            this.colThanhVien.FillWeight = 110F;
            this.colThanhVien.HeaderText = "Thành viên";
            this.colThanhVien.MinimumWidth = 6;
            this.colThanhVien.Name = "colThanhVien";
            this.colThanhVien.ReadOnly = true;
            // 
            // colThaoTac
            // 
            this.colThaoTac.DataPropertyName = "ThaoTacText";
            this.colThaoTac.FillWeight = 80F;
            this.colThaoTac.HeaderText = "Thao tác";
            this.colThaoTac.MinimumWidth = 6;
            this.colThaoTac.Name = "colThaoTac";
            this.colThaoTac.ReadOnly = true;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.Transparent;
            this.pnlRight.BorderRadius = 12;
            this.pnlRight.Controls.Add(this.pnlFormFields);
            this.pnlRight.Controls.Add(this.pnlThanhVienCard);
            this.pnlRight.Controls.Add(this.pnlDetailButtons);
            this.pnlRight.Controls.Add(this.pnlDetailHeader);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.FillColor = System.Drawing.Color.White;
            this.pnlRight.Location = new System.Drawing.Point(1088, 3);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(20);
            this.pnlRight.ShadowDecoration.Color = System.Drawing.Color.Gainsboro;
            this.pnlRight.ShadowDecoration.Depth = 4;
            this.pnlRight.ShadowDecoration.Enabled = true;
            this.pnlRight.Size = new System.Drawing.Size(459, 944);
            this.pnlRight.TabIndex = 1;
            // 
            // pnlFormFields
            // 
            this.pnlFormFields.Controls.Add(this.pnlTienSu);
            this.pnlFormFields.Controls.Add(this.pnlSDT);
            this.pnlFormFields.Controls.Add(this.pnlNgaySinhGioiTinh);
            this.pnlFormFields.Controls.Add(this.pnlHoTen);
            this.pnlFormFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFormFields.Location = new System.Drawing.Point(20, 80);
            this.pnlFormFields.Name = "pnlFormFields";
            this.pnlFormFields.Size = new System.Drawing.Size(419, 493);
            this.pnlFormFields.TabIndex = 3;
            // 
            // pnlTienSu
            // 
            this.pnlTienSu.Controls.Add(this.txtTienSu);
            this.pnlTienSu.Controls.Add(this.lblTienSu);
            this.pnlTienSu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTienSu.Location = new System.Drawing.Point(0, 245);
            this.pnlTienSu.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.pnlTienSu.Name = "pnlTienSu";
            this.pnlTienSu.Padding = new System.Windows.Forms.Padding(10, 10, 10, 20);
            this.pnlTienSu.Size = new System.Drawing.Size(419, 248);
            this.pnlTienSu.TabIndex = 3;
            // 
            // txtTienSu
            // 
            this.txtTienSu.BorderRadius = 10;
            this.txtTienSu.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTienSu.DefaultText = "";
            this.txtTienSu.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTienSu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTienSu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTienSu.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTienSu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTienSu.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtTienSu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTienSu.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtTienSu.Location = new System.Drawing.Point(10, 35);
            this.txtTienSu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTienSu.Multiline = true;
            this.txtTienSu.Name = "txtTienSu";
            this.txtTienSu.PlaceholderText = "Dị ứng, bệnh mãn tính...";
            this.txtTienSu.SelectedText = "";
            this.txtTienSu.Size = new System.Drawing.Size(399, 193);
            this.txtTienSu.TabIndex = 1;
            // 
            // lblTienSu
            // 
            this.lblTienSu.AutoSize = true;
            this.lblTienSu.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTienSu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTienSu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTienSu.Location = new System.Drawing.Point(10, 10);
            this.lblTienSu.Name = "lblTienSu";
            this.lblTienSu.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblTienSu.Size = new System.Drawing.Size(115, 25);
            this.lblTienSu.TabIndex = 0;
            this.lblTienSu.Text = "Tiền sử bệnh lý";
            // 
            // pnlSDT
            // 
            this.pnlSDT.Controls.Add(this.txtSDT);
            this.pnlSDT.Controls.Add(this.lblSDT);
            this.pnlSDT.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSDT.Location = new System.Drawing.Point(0, 165);
            this.pnlSDT.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.pnlSDT.Name = "pnlSDT";
            this.pnlSDT.Padding = new System.Windows.Forms.Padding(5, 5, 5, 10);
            this.pnlSDT.Size = new System.Drawing.Size(419, 80);
            this.pnlSDT.TabIndex = 2;
            // 
            // txtSDT
            // 
            this.txtSDT.BorderRadius = 10;
            this.txtSDT.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSDT.DefaultText = "";
            this.txtSDT.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSDT.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSDT.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSDT.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSDT.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtSDT.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSDT.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtSDT.Location = new System.Drawing.Point(5, 32);
            this.txtSDT.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.PlaceholderText = "Nhập SDT";
            this.txtSDT.SelectedText = "";
            this.txtSDT.Size = new System.Drawing.Size(409, 38);
            this.txtSDT.TabIndex = 1;
            // 
            // lblSDT
            // 
            this.lblSDT.AutoSize = true;
            this.lblSDT.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSDT.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSDT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblSDT.Location = new System.Drawing.Point(5, 5);
            this.lblSDT.Name = "lblSDT";
            this.lblSDT.Size = new System.Drawing.Size(111, 20);
            this.lblSDT.TabIndex = 0;
            this.lblSDT.Text = "Số điện thoại *";
            // 
            // pnlNgaySinhGioiTinh
            // 
            this.pnlNgaySinhGioiTinh.Controls.Add(this.tlpNSGT);
            this.pnlNgaySinhGioiTinh.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNgaySinhGioiTinh.Location = new System.Drawing.Point(0, 80);
            this.pnlNgaySinhGioiTinh.Name = "pnlNgaySinhGioiTinh";
            this.pnlNgaySinhGioiTinh.Padding = new System.Windows.Forms.Padding(5, 5, 5, 10);
            this.pnlNgaySinhGioiTinh.Size = new System.Drawing.Size(419, 85);
            this.pnlNgaySinhGioiTinh.TabIndex = 1;
            // 
            // tlpNSGT
            // 
            this.tlpNSGT.ColumnCount = 2;
            this.tlpNSGT.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.05624F));
            this.tlpNSGT.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.94376F));
            this.tlpNSGT.Controls.Add(this.pnlGioiTinh, 1, 0);
            this.tlpNSGT.Controls.Add(this.pnlNgaySinh, 0, 0);
            this.tlpNSGT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpNSGT.Location = new System.Drawing.Point(5, 5);
            this.tlpNSGT.Name = "tlpNSGT";
            this.tlpNSGT.RowCount = 1;
            this.tlpNSGT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpNSGT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpNSGT.Size = new System.Drawing.Size(409, 70);
            this.tlpNSGT.TabIndex = 0;
            // 
            // pnlGioiTinh
            // 
            this.pnlGioiTinh.Controls.Add(this.cmbGioiTinh);
            this.pnlGioiTinh.Controls.Add(this.lblGioiTinh);
            this.pnlGioiTinh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGioiTinh.Location = new System.Drawing.Point(225, 3);
            this.pnlGioiTinh.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.pnlGioiTinh.Name = "pnlGioiTinh";
            this.pnlGioiTinh.Size = new System.Drawing.Size(181, 64);
            this.pnlGioiTinh.TabIndex = 1;
            // 
            // cmbGioiTinh
            // 
            this.cmbGioiTinh.BackColor = System.Drawing.Color.Transparent;
            this.cmbGioiTinh.BorderRadius = 10;
            this.cmbGioiTinh.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmbGioiTinh.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGioiTinh.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.cmbGioiTinh.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.cmbGioiTinh.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbGioiTinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbGioiTinh.ItemHeight = 30;
            this.cmbGioiTinh.Items.AddRange(new object[] {
            "Nữ",
            "Nam"});
            this.cmbGioiTinh.Location = new System.Drawing.Point(0, 28);
            this.cmbGioiTinh.Name = "cmbGioiTinh";
            this.cmbGioiTinh.Size = new System.Drawing.Size(181, 36);
            this.cmbGioiTinh.TabIndex = 2;
            // 
            // lblGioiTinh
            // 
            this.lblGioiTinh.AutoSize = true;
            this.lblGioiTinh.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGioiTinh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGioiTinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblGioiTinh.Location = new System.Drawing.Point(0, 0);
            this.lblGioiTinh.Name = "lblGioiTinh";
            this.lblGioiTinh.Size = new System.Drawing.Size(80, 20);
            this.lblGioiTinh.TabIndex = 1;
            this.lblGioiTinh.Text = "Giới tính *";
            // 
            // pnlNgaySinh
            // 
            this.pnlNgaySinh.Controls.Add(this.dtpNgaySinh);
            this.pnlNgaySinh.Controls.Add(this.lblNgaySinh);
            this.pnlNgaySinh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNgaySinh.Location = new System.Drawing.Point(3, 3);
            this.pnlNgaySinh.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
            this.pnlNgaySinh.Name = "pnlNgaySinh";
            this.pnlNgaySinh.Size = new System.Drawing.Size(209, 64);
            this.pnlNgaySinh.TabIndex = 0;
            // 
            // dtpNgaySinh
            // 
            this.dtpNgaySinh.BorderRadius = 10;
            this.dtpNgaySinh.Checked = true;
            this.dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            this.dtpNgaySinh.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dtpNgaySinh.FillColor = System.Drawing.Color.White;
            this.dtpNgaySinh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpNgaySinh.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgaySinh.Location = new System.Drawing.Point(0, 28);
            this.dtpNgaySinh.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpNgaySinh.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpNgaySinh.Name = "dtpNgaySinh";
            this.dtpNgaySinh.Size = new System.Drawing.Size(209, 36);
            this.dtpNgaySinh.TabIndex = 2;
            this.dtpNgaySinh.Value = new System.DateTime(2026, 4, 5, 5, 44, 50, 994);
            // 
            // lblNgaySinh
            // 
            this.lblNgaySinh.AutoSize = true;
            this.lblNgaySinh.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNgaySinh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgaySinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblNgaySinh.Location = new System.Drawing.Point(0, 0);
            this.lblNgaySinh.Name = "lblNgaySinh";
            this.lblNgaySinh.Size = new System.Drawing.Size(90, 20);
            this.lblNgaySinh.TabIndex = 1;
            this.lblNgaySinh.Text = "Ngày sinh *";
            // 
            // pnlHoTen
            // 
            this.pnlHoTen.Controls.Add(this.txtHoTen);
            this.pnlHoTen.Controls.Add(this.lblHoTen);
            this.pnlHoTen.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHoTen.Location = new System.Drawing.Point(0, 0);
            this.pnlHoTen.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.pnlHoTen.Name = "pnlHoTen";
            this.pnlHoTen.Padding = new System.Windows.Forms.Padding(5, 5, 5, 10);
            this.pnlHoTen.Size = new System.Drawing.Size(419, 80);
            this.pnlHoTen.TabIndex = 0;
            // 
            // txtHoTen
            // 
            this.txtHoTen.BorderRadius = 10;
            this.txtHoTen.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtHoTen.DefaultText = "";
            this.txtHoTen.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtHoTen.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtHoTen.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtHoTen.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtHoTen.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtHoTen.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtHoTen.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtHoTen.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtHoTen.Location = new System.Drawing.Point(5, 32);
            this.txtHoTen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.PlaceholderText = "Nhập họ và tên...";
            this.txtHoTen.SelectedText = "";
            this.txtHoTen.Size = new System.Drawing.Size(409, 38);
            this.txtHoTen.TabIndex = 1;
            // 
            // lblHoTen
            // 
            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHoTen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoTen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblHoTen.Location = new System.Drawing.Point(5, 5);
            this.lblHoTen.Name = "lblHoTen";
            this.lblHoTen.Size = new System.Drawing.Size(87, 20);
            this.lblHoTen.TabIndex = 0;
            this.lblHoTen.Text = "Họ và tên *";
            // 
            // pnlThanhVienCard
            // 
            this.pnlThanhVienCard.BorderRadius = 10;
            this.pnlThanhVienCard.Controls.Add(this.lblThanhVienInfo);
            this.pnlThanhVienCard.Controls.Add(this.lblThanhVienTitle);
            this.pnlThanhVienCard.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlThanhVienCard.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.pnlThanhVienCard.Location = new System.Drawing.Point(20, 573);
            this.pnlThanhVienCard.Margin = new System.Windows.Forms.Padding(3, 20, 3, 20);
            this.pnlThanhVienCard.Name = "pnlThanhVienCard";
            this.pnlThanhVienCard.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.pnlThanhVienCard.Size = new System.Drawing.Size(419, 150);
            this.pnlThanhVienCard.TabIndex = 2;
            // 
            // lblThanhVienInfo
            // 
            this.lblThanhVienInfo.AutoSize = true;
            this.lblThanhVienInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblThanhVienInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThanhVienInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(80)))), ((int)(((byte)(55)))));
            this.lblThanhVienInfo.Location = new System.Drawing.Point(14, 38);
            this.lblThanhVienInfo.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblThanhVienInfo.Name = "lblThanhVienInfo";
            this.lblThanhVienInfo.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblThanhVienInfo.Size = new System.Drawing.Size(160, 30);
            this.lblThanhVienInfo.TabIndex = 1;
            this.lblThanhVienInfo.Text = "Chưa có thẻ thành viên";
            // 
            // lblThanhVienTitle
            // 
            this.lblThanhVienTitle.AutoSize = true;
            this.lblThanhVienTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblThanhVienTitle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThanhVienTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblThanhVienTitle.Location = new System.Drawing.Point(14, 10);
            this.lblThanhVienTitle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblThanhVienTitle.Name = "lblThanhVienTitle";
            this.lblThanhVienTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblThanhVienTitle.Size = new System.Drawing.Size(162, 28);
            this.lblThanhVienTitle.TabIndex = 0;
            this.lblThanhVienTitle.Text = "💎 Thẻ Thành Viên";
            // 
            // pnlDetailButtons
            // 
            this.pnlDetailButtons.Controls.Add(this.tlpDetailButtons);
            this.pnlDetailButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailButtons.Location = new System.Drawing.Point(20, 723);
            this.pnlDetailButtons.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.pnlDetailButtons.Name = "pnlDetailButtons";
            this.pnlDetailButtons.Size = new System.Drawing.Size(419, 201);
            this.pnlDetailButtons.TabIndex = 1;
            this.pnlDetailButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDetailButtons_Paint);
            // 
            // tlpDetailButtons
            // 
            this.tlpDetailButtons.ColumnCount = 1;
            this.tlpDetailButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDetailButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpDetailButtons.Controls.Add(this.btnXoa, 0, 2);
            this.tlpDetailButtons.Controls.Add(this.btnLuuThayDoi, 0, 0);
            this.tlpDetailButtons.Controls.Add(this.btnXemLichSuKham, 0, 1);
            this.tlpDetailButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDetailButtons.Location = new System.Drawing.Point(0, 0);
            this.tlpDetailButtons.Name = "tlpDetailButtons";
            this.tlpDetailButtons.Padding = new System.Windows.Forms.Padding(10, 0, 10, 8);
            this.tlpDetailButtons.RowCount = 3;
            this.tlpDetailButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpDetailButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpDetailButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tlpDetailButtons.Size = new System.Drawing.Size(419, 201);
            this.tlpDetailButtons.TabIndex = 0;
            // 
            // btnXoa
            // 
            this.btnXoa.BorderRadius = 18;
            this.btnXoa.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnXoa.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnXoa.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnXoa.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnXoa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnXoa.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(10, 136);
            this.btnXoa.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(399, 57);
            this.btnXoa.TabIndex = 2;
            this.btnXoa.Text = "Xóa (Soft Delete)";
            // 
            // btnLuuThayDoi
            // 
            this.btnLuuThayDoi.BorderRadius = 18;
            this.btnLuuThayDoi.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLuuThayDoi.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLuuThayDoi.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLuuThayDoi.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLuuThayDoi.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLuuThayDoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLuuThayDoi.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnLuuThayDoi.FillColor2 = System.Drawing.Color.SeaGreen;
            this.btnLuuThayDoi.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuuThayDoi.ForeColor = System.Drawing.Color.White;
            this.btnLuuThayDoi.Location = new System.Drawing.Point(10, 8);
            this.btnLuuThayDoi.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.btnLuuThayDoi.Name = "btnLuuThayDoi";
            this.btnLuuThayDoi.Size = new System.Drawing.Size(399, 56);
            this.btnLuuThayDoi.TabIndex = 0;
            this.btnLuuThayDoi.Text = "Lưu Thay Đổi";
            // 
            // btnXemLichSuKham
            // 
            this.btnXemLichSuKham.BorderRadius = 18;
            this.btnXemLichSuKham.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnXemLichSuKham.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnXemLichSuKham.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnXemLichSuKham.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnXemLichSuKham.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnXemLichSuKham.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.btnXemLichSuKham.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXemLichSuKham.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnXemLichSuKham.Location = new System.Drawing.Point(10, 72);
            this.btnXemLichSuKham.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.btnXemLichSuKham.Name = "btnXemLichSuKham";
            this.btnXemLichSuKham.Size = new System.Drawing.Size(399, 56);
            this.btnXemLichSuKham.TabIndex = 1;
            this.btnXemLichSuKham.Text = " Xem Lịch Sử Khám";
            // 
            // pnlDetailHeader
            // 
            this.pnlDetailHeader.Controls.Add(this.lblTitleDetail);
            this.pnlDetailHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDetailHeader.Location = new System.Drawing.Point(20, 20);
            this.pnlDetailHeader.Name = "pnlDetailHeader";
            this.pnlDetailHeader.Size = new System.Drawing.Size(419, 60);
            this.pnlDetailHeader.TabIndex = 0;
            // 
            // lblTitleDetail
            // 
            this.lblTitleDetail.AutoSize = true;
            this.lblTitleDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitleDetail.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTitleDetail.Location = new System.Drawing.Point(0, 0);
            this.lblTitleDetail.Name = "lblTitleDetail";
            this.lblTitleDetail.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblTitleDetail.Size = new System.Drawing.Size(211, 25);
            this.lblTitleDetail.TabIndex = 0;
            this.lblTitleDetail.Text = "Thông Tin Bệnh Nhân";
            this.lblTitleDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PatientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1600, 1000);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PatientForm";
            this.Padding = new System.Windows.Forms.Padding(25);
            this.Text = "Quản Lý Bệnh Nhân";
            this.tlpMain.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.tlpLeftInner.ResumeLayout(false);
            this.pnlTopBar.ResumeLayout(false);
            this.pnlBtnGroup.ResumeLayout(false);
            this.pnlFilterBar.ResumeLayout(false);
            this.pnlFilterBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBenhNhan)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.pnlFormFields.ResumeLayout(false);
            this.pnlTienSu.ResumeLayout(false);
            this.pnlTienSu.PerformLayout();
            this.pnlSDT.ResumeLayout(false);
            this.pnlSDT.PerformLayout();
            this.pnlNgaySinhGioiTinh.ResumeLayout(false);
            this.tlpNSGT.ResumeLayout(false);
            this.pnlGioiTinh.ResumeLayout(false);
            this.pnlGioiTinh.PerformLayout();
            this.pnlNgaySinh.ResumeLayout(false);
            this.pnlNgaySinh.PerformLayout();
            this.pnlHoTen.ResumeLayout(false);
            this.pnlHoTen.PerformLayout();
            this.pnlThanhVienCard.ResumeLayout(false);
            this.pnlThanhVienCard.PerformLayout();
            this.pnlDetailButtons.ResumeLayout(false);
            this.tlpDetailButtons.ResumeLayout(false);
            this.pnlDetailHeader.ResumeLayout(false);
            this.pnlDetailHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        private TableLayoutPanel tlpMain;
        private Guna2Panel pnlLeft;
        private TableLayoutPanel tlpLeftInner;
        private Panel pnlTopBar;
        private Panel pnlBtnGroup;
        private Guna2TextBox txtTimKiem;
        private Guna2Button btnLamMoi;
        private Guna2GradientButton btnThemMoi;
        private Panel pnlFilterBar;
        private Guna2ComboBox cmbLoc;
        private Label lblLocLabel;
        private Label lblTongSo;
        private Guna2DataGridView dgvBenhNhan;
        private DataGridViewTextBoxColumn colMaBN;
        private DataGridViewTextBoxColumn colHoTen;
        private DataGridViewTextBoxColumn colNgaySinh;
        private DataGridViewTextBoxColumn colSDT;
        private DataGridViewTextBoxColumn colGioiTinh;
        private DataGridViewTextBoxColumn colThanhVien;
        private DataGridViewTextBoxColumn colThaoTac;
        private Guna2Panel pnlRight;
        private Panel pnlDetailHeader;
        private Guna2Panel pnlThanhVienCard;
        private Panel pnlDetailButtons;
        private Panel pnlFormFields;
        private Label lblTitleDetail;
        private TableLayoutPanel tlpDetailButtons;
        private Guna2GradientButton btnLuuThayDoi;
        private Guna2Button btnXemLichSuKham;
        private Label lblThanhVienInfo;
        private Label lblThanhVienTitle;
        private Guna2Button btnXoa;
        private Panel pnlHoTen;
        private Guna2TextBox txtHoTen;
        private Label lblHoTen;
        private Panel pnlNgaySinhGioiTinh;
        private TableLayoutPanel tlpNSGT;
        private Panel pnlGioiTinh;
        private Label lblGioiTinh;
        private Panel pnlNgaySinh;
        private Label lblNgaySinh;
        private Panel pnlSDT;
        private Guna2TextBox txtSDT;
        private Label lblSDT;
        private Guna2ComboBox cmbGioiTinh;
        private Guna2DateTimePicker dtpNgaySinh;
        private Panel pnlTienSu;
        private Guna2TextBox txtTienSu;
        private Label lblTienSu;
    }
}
=======
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 768);
            this.Name = "PatientForm";
            this.Text = "Quản lý bệnh nhân";
            this.ResumeLayout(false);
        }
    }
}
>>>>>>> d2fc9d190a76c0c366e0407bca6067fe95379af1
