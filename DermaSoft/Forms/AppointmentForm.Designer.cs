using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    partial class AppointmentForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Controls
        // ── Layout chính ──────────────────────────────────────────────────
        private System.Windows.Forms.TableLayoutPanel tlpMain;

        // ── Cột trái: Calendar + Form tạo lịch ───────────────────────────
        private System.Windows.Forms.TableLayoutPanel tlpLeft;

        private Guna2Panel pnlCalendar;
        private System.Windows.Forms.MonthCalendar mcLichHen;

        private Guna2Panel pnlTaoLich;
        private System.Windows.Forms.Panel pnlTitleAccentBar;  // Thanh xanh dọc trước tiêu đề
        private System.Windows.Forms.Label lblTitleTaoLich;
        private System.Windows.Forms.TableLayoutPanel tlpFormTaoLich;

        private System.Windows.Forms.Panel pnlBenhNhan;
        private System.Windows.Forms.Label lblBenhNhan;
        private Guna2TextBox txtBenhNhan;

        private System.Windows.Forms.Panel pnlThoiGian;
        private System.Windows.Forms.Label lblThoiGian;
        private Guna2DateTimePicker dtpThoiGian;
        private Guna.UI2.WinForms.Guna2ComboBox cmbGioHen;


        private System.Windows.Forms.Panel pnlBacSi;
        private System.Windows.Forms.Label lblBacSi;
        private Guna2ComboBox cmbBacSi;

        private System.Windows.Forms.Panel pnlGhiChu;
        private System.Windows.Forms.Label lblGhiChu;
        private Guna2TextBox txtGhiChu;

        private Guna2GradientButton btnTaoLich;

        // ── Cột phải: Search + Label ngày + DataGridView ─────────────────
        private Guna2Panel pnlRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        private System.Windows.Forms.Panel pnlSearch;
        private Guna2TextBox txtSearch;
        private System.Windows.Forms.Panel pnlSearchSpacer;
        private Guna2ComboBox cmbFilter;

        private System.Windows.Forms.Label lblNgayHien;

        private Guna2DataGridView dgvLichHen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBenhNhan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSDT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBacSi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGhiChu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThaoTac;
        #endregion

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tlpLeft = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCalendar = new Guna.UI2.WinForms.Guna2Panel();
            this.mcLichHen = new System.Windows.Forms.MonthCalendar();
            this.pnlTaoLich = new Guna.UI2.WinForms.Guna2Panel();
            this.tlpFormTaoLich = new System.Windows.Forms.TableLayoutPanel();
            this.pnlBenhNhan = new System.Windows.Forms.Panel();
            this.txtBenhNhan = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblBenhNhan = new System.Windows.Forms.Label();
            this.pnlThoiGian = new System.Windows.Forms.Panel();
            this.dtpThoiGian = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.cmbGioHen = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblThoiGian = new System.Windows.Forms.Label();
            this.pnlBacSi = new System.Windows.Forms.Panel();
            this.cmbBacSi = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblBacSi = new System.Windows.Forms.Label();
            this.pnlGhiChu = new System.Windows.Forms.Panel();
            this.txtGhiChu = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.btnTaoLich = new Guna.UI2.WinForms.Guna2GradientButton();
            this.pnlTitleAccentBar = new System.Windows.Forms.Panel();
            this.lblTitleTaoLich = new System.Windows.Forms.Label();
            this.pnlRight = new Guna.UI2.WinForms.Guna2Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlSearchSpacer = new System.Windows.Forms.Panel();
            this.cmbFilter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblNgayHien = new System.Windows.Forms.Label();
            this.dgvLichHen = new Guna.UI2.WinForms.Guna2DataGridView();
            this.tlpMain.SuspendLayout();
            this.tlpLeft.SuspendLayout();
            this.pnlCalendar.SuspendLayout();
            this.pnlTaoLich.SuspendLayout();
            this.tlpFormTaoLich.SuspendLayout();
            this.pnlBenhNhan.SuspendLayout();
            this.pnlThoiGian.SuspendLayout();
            this.pnlBacSi.SuspendLayout();
            this.pnlGhiChu.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichHen)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 412F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.tlpLeft, 0, 0);
            this.tlpMain.Controls.Add(this.pnlRight, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(25, 25);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(1275, 910);
            this.tlpMain.TabIndex = 0;
            // 
            // tlpLeft
            // 
            this.tlpLeft.ColumnCount = 1;
            this.tlpLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLeft.Controls.Add(this.pnlCalendar, 0, 0);
            this.tlpLeft.Controls.Add(this.pnlTaoLich, 0, 1);
            this.tlpLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLeft.Location = new System.Drawing.Point(0, 0);
            this.tlpLeft.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.tlpLeft.Name = "tlpLeft";
            this.tlpLeft.RowCount = 2;
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 340F));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLeft.Size = new System.Drawing.Size(397, 910);
            this.tlpLeft.TabIndex = 0;
            // 
            // pnlCalendar
            // 
            this.pnlCalendar.BackColor = System.Drawing.Color.Transparent;
            this.pnlCalendar.BorderRadius = 12;
            this.pnlCalendar.Controls.Add(this.mcLichHen);
            this.pnlCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCalendar.FillColor = System.Drawing.Color.White;
            this.pnlCalendar.Location = new System.Drawing.Point(0, 0);
            this.pnlCalendar.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.pnlCalendar.Name = "pnlCalendar";
            this.pnlCalendar.Padding = new System.Windows.Forms.Padding(10);
            this.pnlCalendar.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.pnlCalendar.ShadowDecoration.Depth = 4;
            this.pnlCalendar.ShadowDecoration.Enabled = true;
            this.pnlCalendar.Size = new System.Drawing.Size(397, 325);
            this.pnlCalendar.TabIndex = 0;
            // 
            // mcLichHen
            // 
            this.mcLichHen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mcLichHen.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mcLichHen.Location = new System.Drawing.Point(10, 10);
            this.mcLichHen.Margin = new System.Windows.Forms.Padding(11);
            this.mcLichHen.MaxSelectionCount = 1;
            this.mcLichHen.Name = "mcLichHen";
            this.mcLichHen.TabIndex = 0;
            this.mcLichHen.TitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.mcLichHen.TitleForeColor = System.Drawing.Color.White;
            this.mcLichHen.TrailingForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            // 
            // pnlTaoLich
            // 
            this.pnlTaoLich.BackColor = System.Drawing.Color.Transparent;
            this.pnlTaoLich.BorderRadius = 12;
            this.pnlTaoLich.Controls.Add(this.tlpFormTaoLich);
            this.pnlTaoLich.Controls.Add(this.pnlTitleAccentBar);
            this.pnlTaoLich.Controls.Add(this.lblTitleTaoLich);
            this.pnlTaoLich.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTaoLich.FillColor = System.Drawing.Color.White;
            this.pnlTaoLich.Location = new System.Drawing.Point(0, 340);
            this.pnlTaoLich.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTaoLich.Name = "pnlTaoLich";
            this.pnlTaoLich.Padding = new System.Windows.Forms.Padding(20);
            this.pnlTaoLich.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.pnlTaoLich.ShadowDecoration.Depth = 4;
            this.pnlTaoLich.ShadowDecoration.Enabled = true;
            this.pnlTaoLich.Size = new System.Drawing.Size(397, 570);
            this.pnlTaoLich.TabIndex = 1;
            // 
            // tlpFormTaoLich
            // 
            this.tlpFormTaoLich.ColumnCount = 1;
            this.tlpFormTaoLich.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFormTaoLich.Controls.Add(this.pnlBenhNhan, 0, 0);
            this.tlpFormTaoLich.Controls.Add(this.pnlThoiGian, 0, 1);
            this.tlpFormTaoLich.Controls.Add(this.pnlBacSi, 0, 2);
            this.tlpFormTaoLich.Controls.Add(this.pnlGhiChu, 0, 3);
            this.tlpFormTaoLich.Controls.Add(this.btnTaoLich, 0, 4);
            this.tlpFormTaoLich.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFormTaoLich.Location = new System.Drawing.Point(20, 64);
            this.tlpFormTaoLich.Margin = new System.Windows.Forms.Padding(4);
            this.tlpFormTaoLich.Name = "tlpFormTaoLich";
            this.tlpFormTaoLich.RowCount = 5;
            this.tlpFormTaoLich.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tlpFormTaoLich.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 124F));
            this.tlpFormTaoLich.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tlpFormTaoLich.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFormTaoLich.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tlpFormTaoLich.Size = new System.Drawing.Size(357, 486);
            this.tlpFormTaoLich.TabIndex = 1;
            // 
            // pnlBenhNhan
            // 
            this.pnlBenhNhan.Controls.Add(this.txtBenhNhan);
            this.pnlBenhNhan.Controls.Add(this.lblBenhNhan);
            this.pnlBenhNhan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBenhNhan.Location = new System.Drawing.Point(4, 4);
            this.pnlBenhNhan.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBenhNhan.Name = "pnlBenhNhan";
            this.pnlBenhNhan.Size = new System.Drawing.Size(349, 63);
            this.pnlBenhNhan.TabIndex = 0;
            // 
            // txtBenhNhan
            // 
            this.txtBenhNhan.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.txtBenhNhan.BorderRadius = 8;
            this.txtBenhNhan.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBenhNhan.DefaultText = "";
            this.txtBenhNhan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBenhNhan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtBenhNhan.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtBenhNhan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtBenhNhan.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtBenhNhan.Location = new System.Drawing.Point(0, 28);
            this.txtBenhNhan.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBenhNhan.Name = "txtBenhNhan";
            this.txtBenhNhan.PlaceholderText = "Nhập tên / SĐT...";
            this.txtBenhNhan.SelectedText = "";
            this.txtBenhNhan.Size = new System.Drawing.Size(349, 35);
            this.txtBenhNhan.TabIndex = 1;
            // 
            // lblBenhNhan
            // 
            this.lblBenhNhan.AutoSize = true;
            this.lblBenhNhan.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBenhNhan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBenhNhan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblBenhNhan.Location = new System.Drawing.Point(0, 0);
            this.lblBenhNhan.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBenhNhan.Name = "lblBenhNhan";
            this.lblBenhNhan.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblBenhNhan.Size = new System.Drawing.Size(84, 28);
            this.lblBenhNhan.TabIndex = 0;
            this.lblBenhNhan.Text = "Bệnh nhân";
            // 
            // pnlThoiGian
            // 
            this.pnlThoiGian.Controls.Add(this.dtpThoiGian);
            this.pnlThoiGian.Controls.Add(this.cmbGioHen);
            this.pnlThoiGian.Controls.Add(this.lblThoiGian);
            this.pnlThoiGian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlThoiGian.Location = new System.Drawing.Point(4, 75);
            this.pnlThoiGian.Margin = new System.Windows.Forms.Padding(4);
            this.pnlThoiGian.Name = "pnlThoiGian";
            this.pnlThoiGian.Size = new System.Drawing.Size(349, 116);
            this.pnlThoiGian.TabIndex = 1;
            // 
            // dtpThoiGian
            // 
            this.dtpThoiGian.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.dtpThoiGian.BorderRadius = 8;
            this.dtpThoiGian.Checked = true;
            this.dtpThoiGian.CustomFormat = "dd/MM/yyyy";
            this.dtpThoiGian.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dtpThoiGian.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.dtpThoiGian.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpThoiGian.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpThoiGian.Location = new System.Drawing.Point(0, 78);
            this.dtpThoiGian.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.dtpThoiGian.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpThoiGian.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpThoiGian.Name = "dtpThoiGian";
            this.dtpThoiGian.Size = new System.Drawing.Size(349, 38);
            this.dtpThoiGian.TabIndex = 1;
            this.dtpThoiGian.Value = new System.DateTime(2026, 4, 5, 1, 10, 44, 781);
            // 
            // cmbGioHen
            // 
            this.cmbGioHen.BackColor = System.Drawing.Color.Transparent;
            this.cmbGioHen.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.cmbGioHen.BorderRadius = 8;
            this.cmbGioHen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbGioHen.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbGioHen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGioHen.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbGioHen.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.cmbGioHen.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.cmbGioHen.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbGioHen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(75)))), ((int)(((byte)(66)))));
            this.cmbGioHen.ItemHeight = 30;
            this.cmbGioHen.Location = new System.Drawing.Point(0, 28);
            this.cmbGioHen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 10);
            this.cmbGioHen.Name = "cmbGioHen";
            this.cmbGioHen.Size = new System.Drawing.Size(349, 36);
            this.cmbGioHen.TabIndex = 2;
            // 
            // lblThoiGian
            // 
            this.lblThoiGian.AutoSize = true;
            this.lblThoiGian.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblThoiGian.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblThoiGian.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblThoiGian.Location = new System.Drawing.Point(0, 0);
            this.lblThoiGian.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThoiGian.Name = "lblThoiGian";
            this.lblThoiGian.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblThoiGian.Size = new System.Drawing.Size(104, 28);
            this.lblThoiGian.TabIndex = 0;
            this.lblThoiGian.Text = "Thời gian hẹn";
            // 
            // pnlBacSi
            // 
            this.pnlBacSi.Controls.Add(this.cmbBacSi);
            this.pnlBacSi.Controls.Add(this.lblBacSi);
            this.pnlBacSi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBacSi.Location = new System.Drawing.Point(4, 199);
            this.pnlBacSi.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBacSi.Name = "pnlBacSi";
            this.pnlBacSi.Size = new System.Drawing.Size(349, 69);
            this.pnlBacSi.TabIndex = 2;
            // 
            // cmbBacSi
            // 
            this.cmbBacSi.BackColor = System.Drawing.Color.Transparent;
            this.cmbBacSi.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.cmbBacSi.BorderRadius = 8;
            this.cmbBacSi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBacSi.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbBacSi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBacSi.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbBacSi.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.cmbBacSi.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.cmbBacSi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbBacSi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(75)))), ((int)(((byte)(66)))));
            this.cmbBacSi.ItemHeight = 30;
            this.cmbBacSi.Location = new System.Drawing.Point(0, 28);
            this.cmbBacSi.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBacSi.Name = "cmbBacSi";
            this.cmbBacSi.Size = new System.Drawing.Size(349, 36);
            this.cmbBacSi.TabIndex = 1;
            // 
            // lblBacSi
            // 
            this.lblBacSi.AutoSize = true;
            this.lblBacSi.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBacSi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBacSi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblBacSi.Location = new System.Drawing.Point(0, 0);
            this.lblBacSi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBacSi.Name = "lblBacSi";
            this.lblBacSi.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblBacSi.Size = new System.Drawing.Size(120, 28);
            this.lblBacSi.TabIndex = 0;
            this.lblBacSi.Text = "Bác sĩ phụ trách";
            // 
            // pnlGhiChu
            // 
            this.pnlGhiChu.Controls.Add(this.txtGhiChu);
            this.pnlGhiChu.Controls.Add(this.lblGhiChu);
            this.pnlGhiChu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGhiChu.Location = new System.Drawing.Point(4, 276);
            this.pnlGhiChu.Margin = new System.Windows.Forms.Padding(4);
            this.pnlGhiChu.Name = "pnlGhiChu";
            this.pnlGhiChu.Size = new System.Drawing.Size(349, 151);
            this.pnlGhiChu.TabIndex = 3;
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.txtGhiChu.BorderRadius = 8;
            this.txtGhiChu.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGhiChu.DefaultText = "";
            this.txtGhiChu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGhiChu.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtGhiChu.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtGhiChu.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtGhiChu.Location = new System.Drawing.Point(0, 28);
            this.txtGhiChu.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGhiChu.Multiline = true;
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.PlaceholderText = "Nhập ghi chú...";
            this.txtGhiChu.SelectedText = "";
            this.txtGhiChu.Size = new System.Drawing.Size(349, 123);
            this.txtGhiChu.TabIndex = 1;
            // 
            // lblGhiChu
            // 
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblGhiChu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblGhiChu.Location = new System.Drawing.Point(0, 0);
            this.lblGhiChu.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblGhiChu.Size = new System.Drawing.Size(62, 28);
            this.lblGhiChu.TabIndex = 0;
            this.lblGhiChu.Text = "Ghi chú";
            // 
            // btnTaoLich
            // 
            this.btnTaoLich.Animated = true;
            this.btnTaoLich.BorderRadius = 20;
            this.btnTaoLich.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTaoLich.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTaoLich.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnTaoLich.FillColor2 = System.Drawing.Color.SeaGreen;
            this.btnTaoLich.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTaoLich.ForeColor = System.Drawing.Color.White;
            this.btnTaoLich.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(64)))), ((int)(((byte)(53)))));
            this.btnTaoLich.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(64)))), ((int)(((byte)(53)))));
            this.btnTaoLich.Location = new System.Drawing.Point(4, 435);
            this.btnTaoLich.Margin = new System.Windows.Forms.Padding(4);
            this.btnTaoLich.Name = "btnTaoLich";
            this.btnTaoLich.Size = new System.Drawing.Size(349, 47);
            this.btnTaoLich.TabIndex = 4;
            this.btnTaoLich.Text = "📅  Tạo Lịch";
            // 
            // pnlTitleAccentBar
            // 
            this.pnlTitleAccentBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.pnlTitleAccentBar.Location = new System.Drawing.Point(20, 22);
            this.pnlTitleAccentBar.Margin = new System.Windows.Forms.Padding(4);
            this.pnlTitleAccentBar.Name = "pnlTitleAccentBar";
            this.pnlTitleAccentBar.Size = new System.Drawing.Size(5, 28);
            this.pnlTitleAccentBar.TabIndex = 3;
            // 
            // lblTitleTaoLich
            // 
            this.lblTitleTaoLich.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleTaoLich.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitleTaoLich.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTitleTaoLich.Location = new System.Drawing.Point(20, 20);
            this.lblTitleTaoLich.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitleTaoLich.Name = "lblTitleTaoLich";
            this.lblTitleTaoLich.Padding = new System.Windows.Forms.Padding(12, 0, 0, 12);
            this.lblTitleTaoLich.Size = new System.Drawing.Size(357, 44);
            this.lblTitleTaoLich.TabIndex = 0;
            this.lblTitleTaoLich.Text = "➕  Tạo Lịch Hẹn Nhanh";
            this.lblTitleTaoLich.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.Transparent;
            this.pnlRight.BorderRadius = 12;
            this.pnlRight.Controls.Add(this.tableLayoutPanel1);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.FillColor = System.Drawing.Color.White;
            this.pnlRight.Location = new System.Drawing.Point(412, 0);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(20);
            this.pnlRight.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(237)))), ((int)(((byte)(232)))));
            this.pnlRight.ShadowDecoration.Depth = 4;
            this.pnlRight.ShadowDecoration.Enabled = true;
            this.pnlRight.Size = new System.Drawing.Size(863, 910);
            this.pnlRight.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pnlSearch, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblNgayHien, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgvLichHen, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(20, 20);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(823, 870);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.pnlSearchSpacer);
            this.pnlSearch.Controls.Add(this.cmbFilter);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(4, 4);
            this.pnlSearch.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(0, 7, 8, 7);
            this.pnlSearch.Size = new System.Drawing.Size(815, 54);
            this.pnlSearch.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(230)))), ((int)(((byte)(207)))));
            this.txtSearch.BorderRadius = 20;
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.DefaultText = "";
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.txtSearch.Location = new System.Drawing.Point(0, 7);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.txtSearch.PlaceholderText = "🔍  Tìm kiếm lịch hẹn...";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(604, 40);
            this.txtSearch.TabIndex = 0;
            // 
            // pnlSearchSpacer
            // 
            this.pnlSearchSpacer.BackColor = System.Drawing.Color.Transparent;
            this.pnlSearchSpacer.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSearchSpacer.Location = new System.Drawing.Point(604, 7);
            this.pnlSearchSpacer.Name = "pnlSearchSpacer";
            this.pnlSearchSpacer.Size = new System.Drawing.Size(10, 40);
            this.pnlSearchSpacer.TabIndex = 2;
            // 
            // cmbFilter
            // 
            this.cmbFilter.BackColor = System.Drawing.Color.Transparent;
            this.cmbFilter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(230)))), ((int)(((byte)(207)))));
            this.cmbFilter.BorderRadius = 8;
            this.cmbFilter.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmbFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbFilter.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.cmbFilter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.cmbFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(75)))), ((int)(((byte)(66)))));
            this.cmbFilter.ItemHeight = 30;
            this.cmbFilter.Items.AddRange(new object[] {
            "Tất cả",
            "Chờ xác nhận",
            "Đã xác nhận",
            "Hoàn thành",
            "Đã hủy"});
            this.cmbFilter.Location = new System.Drawing.Point(614, 7);
            this.cmbFilter.Margin = new System.Windows.Forms.Padding(4);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(193, 36);
            this.cmbFilter.TabIndex = 1;
            // 
            // lblNgayHien
            // 
            this.lblNgayHien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNgayHien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNgayHien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblNgayHien.Location = new System.Drawing.Point(4, 62);
            this.lblNgayHien.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNgayHien.Name = "lblNgayHien";
            this.lblNgayHien.Size = new System.Drawing.Size(815, 48);
            this.lblNgayHien.TabIndex = 1;
            this.lblNgayHien.Text = "📅  Đang tải...";
            this.lblNgayHien.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvLichHen
            // 
            this.dgvLichHen.AllowUserToAddRows = false;
            this.dgvLichHen.AllowUserToDeleteRows = false;
            this.dgvLichHen.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(247)))));
            this.dgvLichHen.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLichHen.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLichHen.ColumnHeadersHeight = 42;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLichHen.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvLichHen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLichHen.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.dgvLichHen.Location = new System.Drawing.Point(4, 114);
            this.dgvLichHen.Margin = new System.Windows.Forms.Padding(4);
            this.dgvLichHen.MultiSelect = false;
            this.dgvLichHen.Name = "dgvLichHen";
            this.dgvLichHen.ReadOnly = true;
            this.dgvLichHen.RowHeadersVisible = false;
            this.dgvLichHen.RowHeadersWidth = 51;
            this.dgvLichHen.RowTemplate.Height = 42;
            this.dgvLichHen.Size = new System.Drawing.Size(815, 752);
            this.dgvLichHen.TabIndex = 2;
            this.dgvLichHen.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(247)))));
            this.dgvLichHen.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvLichHen.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvLichHen.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvLichHen.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvLichHen.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvLichHen.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.dgvLichHen.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.dgvLichHen.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvLichHen.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.dgvLichHen.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvLichHen.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLichHen.ThemeStyle.HeaderStyle.Height = 42;
            this.dgvLichHen.ThemeStyle.ReadOnly = true;
            this.dgvLichHen.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvLichHen.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLichHen.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvLichHen.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.dgvLichHen.ThemeStyle.RowsStyle.Height = 42;
            this.dgvLichHen.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.dgvLichHen.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            // 
            // AppointmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1325, 960);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AppointmentForm";
            this.Padding = new System.Windows.Forms.Padding(25);
            this.Text = "Quản Lý Lịch Hẹn";
            this.Load += new System.EventHandler(this.AppointmentForm_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpLeft.ResumeLayout(false);
            this.pnlCalendar.ResumeLayout(false);
            this.pnlTaoLich.ResumeLayout(false);
            this.tlpFormTaoLich.ResumeLayout(false);
            this.pnlBenhNhan.ResumeLayout(false);
            this.pnlBenhNhan.PerformLayout();
            this.pnlThoiGian.ResumeLayout(false);
            this.pnlThoiGian.PerformLayout();
            this.pnlBacSi.ResumeLayout(false);
            this.pnlBacSi.PerformLayout();
            this.pnlGhiChu.ResumeLayout(false);
            this.pnlGhiChu.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichHen)).EndInit();
            this.ResumeLayout(false);

        }
    }
}