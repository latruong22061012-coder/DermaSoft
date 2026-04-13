using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    partial class DashboardBacSiForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Controls
        private System.Windows.Forms.TableLayoutPanel tlpStatCards;
        private Guna2Panel pnlCard1;
        private System.Windows.Forms.Panel pnlAccent1;
        private System.Windows.Forms.Label lblCard1Icon;
        private System.Windows.Forms.Label lblCard1Value;
        private System.Windows.Forms.Label lblCard1Title;
        private Guna2Panel pnlCard2;
        private System.Windows.Forms.Panel pnlAccent2;
        private System.Windows.Forms.Label lblCard2Icon;
        private System.Windows.Forms.Label lblCard2Value;
        private System.Windows.Forms.Label lblCard2Title;
        private Guna2Panel pnlCard3;
        private System.Windows.Forms.Panel pnlAccent3;
        private System.Windows.Forms.Label lblCard3Icon;
        private System.Windows.Forms.Label lblCard3Value;
        private System.Windows.Forms.Label lblCard3Title;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private Guna2Panel pnlQueueBenhNhan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSTT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenBenhNhan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSDT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrieuChung;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChoPhut;
        private System.Windows.Forms.TableLayoutPanel tlpActionButtons;
        private Guna2GradientButton btnBatDauKham;
        private Guna2Button btnXemHoSo;
        private Guna2Panel pnlLichLamViec;
        private System.Windows.Forms.Label lblLichTitle;
        private Guna2DataGridView dgvLich;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgay;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCa;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiemDanh;
        #endregion

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpStatCards = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCard1 = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlAccent1 = new System.Windows.Forms.Panel();
            this.lblCard1Icon = new System.Windows.Forms.Label();
            this.lblCard1Value = new System.Windows.Forms.Label();
            this.lblCard1Title = new System.Windows.Forms.Label();
            this.pnlCard2 = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlAccent2 = new System.Windows.Forms.Panel();
            this.lblCard2Icon = new System.Windows.Forms.Label();
            this.lblCard2Value = new System.Windows.Forms.Label();
            this.lblCard2Title = new System.Windows.Forms.Label();
            this.pnlCard3 = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlAccent3 = new System.Windows.Forms.Panel();
            this.lblCard3Icon = new System.Windows.Forms.Label();
            this.lblCard3Value = new System.Windows.Forms.Label();
            this.lblCard3Title = new System.Windows.Forms.Label();
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            this.pnlQueueBenhNhan = new Guna.UI2.WinForms.Guna2Panel();
            this.tlpActionButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnBatDauKham = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnXemHoSo = new Guna.UI2.WinForms.Guna2Button();
            this.pnlLichLamViec = new Guna.UI2.WinForms.Guna2Panel();
            this.lblLichTitle = new System.Windows.Forms.Label();
            this.dgvLich = new Guna.UI2.WinForms.Guna2DataGridView();
            this.dgvQueue = new Guna.UI2.WinForms.Guna2DataGridView();
            this.lblQueueTitle = new System.Windows.Forms.Label();
            this.tlpStatCards.SuspendLayout();
            this.pnlCard1.SuspendLayout();
            this.pnlCard2.SuspendLayout();
            this.pnlCard3.SuspendLayout();
            this.tlpContent.SuspendLayout();
            this.pnlQueueBenhNhan.SuspendLayout();
            this.tlpActionButtons.SuspendLayout();
            this.pnlLichLamViec.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLich)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpStatCards
            // 
            this.tlpStatCards.BackColor = System.Drawing.Color.Transparent;
            this.tlpStatCards.ColumnCount = 3;
            this.tlpStatCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpStatCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpStatCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tlpStatCards.Controls.Add(this.pnlCard1, 0, 0);
            this.tlpStatCards.Controls.Add(this.pnlCard2, 1, 0);
            this.tlpStatCards.Controls.Add(this.pnlCard3, 2, 0);
            this.tlpStatCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpStatCards.Location = new System.Drawing.Point(25, 25);
            this.tlpStatCards.Margin = new System.Windows.Forms.Padding(0);
            this.tlpStatCards.Name = "tlpStatCards";
            this.tlpStatCards.Padding = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.tlpStatCards.RowCount = 1;
            this.tlpStatCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStatCards.Size = new System.Drawing.Size(1325, 178);
            this.tlpStatCards.TabIndex = 0;
            // 
            // pnlCard1
            // 
            this.pnlCard1.BackColor = System.Drawing.Color.Transparent;
            this.pnlCard1.BorderRadius = 12;
            this.pnlCard1.Controls.Add(this.pnlAccent1);
            this.pnlCard1.Controls.Add(this.lblCard1Icon);
            this.pnlCard1.Controls.Add(this.lblCard1Value);
            this.pnlCard1.Controls.Add(this.lblCard1Title);
            this.pnlCard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCard1.FillColor = System.Drawing.Color.White;
            this.pnlCard1.Location = new System.Drawing.Point(0, 0);
            this.pnlCard1.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pnlCard1.Name = "pnlCard1";
            this.pnlCard1.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlCard1.ShadowDecoration.Depth = 4;
            this.pnlCard1.ShadowDecoration.Enabled = true;
            this.pnlCard1.Size = new System.Drawing.Size(429, 163);
            this.pnlCard1.TabIndex = 0;
            // 
            // pnlAccent1
            // 
            this.pnlAccent1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.pnlAccent1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlAccent1.Location = new System.Drawing.Point(0, 0);
            this.pnlAccent1.Name = "pnlAccent1";
            this.pnlAccent1.Size = new System.Drawing.Size(5, 163);
            this.pnlAccent1.TabIndex = 0;
            // 
            // lblCard1Icon
            // 
            this.lblCard1Icon.AutoSize = true;
            this.lblCard1Icon.BackColor = System.Drawing.Color.Transparent;
            this.lblCard1Icon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.lblCard1Icon.Location = new System.Drawing.Point(25, 18);
            this.lblCard1Icon.Name = "lblCard1Icon";
            this.lblCard1Icon.Size = new System.Drawing.Size(52, 36);
            this.lblCard1Icon.TabIndex = 1;
            this.lblCard1Icon.Text = "👥";
            // 
            // lblCard1Value
            // 
            this.lblCard1Value.AutoSize = true;
            this.lblCard1Value.BackColor = System.Drawing.Color.Transparent;
            this.lblCard1Value.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCard1Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.lblCard1Value.Location = new System.Drawing.Point(25, 58);
            this.lblCard1Value.Name = "lblCard1Value";
            this.lblCard1Value.Size = new System.Drawing.Size(63, 54);
            this.lblCard1Value.TabIndex = 2;
            this.lblCard1Value.Text = "—";
            // 
            // lblCard1Title
            // 
            this.lblCard1Title.AutoSize = true;
            this.lblCard1Title.BackColor = System.Drawing.Color.Transparent;
            this.lblCard1Title.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCard1Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblCard1Title.Location = new System.Drawing.Point(25, 118);
            this.lblCard1Title.Name = "lblCard1Title";
            this.lblCard1Title.Size = new System.Drawing.Size(144, 20);
            this.lblCard1Title.TabIndex = 3;
            this.lblCard1Title.Text = "Bệnh nhân đang chờ";
            // 
            // pnlCard2
            // 
            this.pnlCard2.BackColor = System.Drawing.Color.Transparent;
            this.pnlCard2.BorderRadius = 12;
            this.pnlCard2.Controls.Add(this.pnlAccent2);
            this.pnlCard2.Controls.Add(this.lblCard2Icon);
            this.pnlCard2.Controls.Add(this.lblCard2Value);
            this.pnlCard2.Controls.Add(this.lblCard2Title);
            this.pnlCard2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCard2.FillColor = System.Drawing.Color.White;
            this.pnlCard2.Location = new System.Drawing.Point(441, 0);
            this.pnlCard2.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pnlCard2.Name = "pnlCard2";
            this.pnlCard2.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlCard2.ShadowDecoration.Depth = 4;
            this.pnlCard2.ShadowDecoration.Enabled = true;
            this.pnlCard2.Size = new System.Drawing.Size(429, 163);
            this.pnlCard2.TabIndex = 1;
            // 
            // pnlAccent2
            // 
            this.pnlAccent2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(138)))), ((int)(((byte)(40)))));
            this.pnlAccent2.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlAccent2.Location = new System.Drawing.Point(0, 0);
            this.pnlAccent2.Name = "pnlAccent2";
            this.pnlAccent2.Size = new System.Drawing.Size(5, 163);
            this.pnlAccent2.TabIndex = 0;
            // 
            // lblCard2Icon
            // 
            this.lblCard2Icon.AutoSize = true;
            this.lblCard2Icon.BackColor = System.Drawing.Color.Transparent;
            this.lblCard2Icon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.lblCard2Icon.Location = new System.Drawing.Point(25, 18);
            this.lblCard2Icon.Name = "lblCard2Icon";
            this.lblCard2Icon.Size = new System.Drawing.Size(52, 36);
            this.lblCard2Icon.TabIndex = 1;
            this.lblCard2Icon.Text = "✅";
            // 
            // lblCard2Value
            // 
            this.lblCard2Value.AutoSize = true;
            this.lblCard2Value.BackColor = System.Drawing.Color.Transparent;
            this.lblCard2Value.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCard2Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.lblCard2Value.Location = new System.Drawing.Point(25, 58);
            this.lblCard2Value.Name = "lblCard2Value";
            this.lblCard2Value.Size = new System.Drawing.Size(63, 54);
            this.lblCard2Value.TabIndex = 2;
            this.lblCard2Value.Text = "—";
            // 
            // lblCard2Title
            // 
            this.lblCard2Title.AutoSize = true;
            this.lblCard2Title.BackColor = System.Drawing.Color.Transparent;
            this.lblCard2Title.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCard2Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblCard2Title.Location = new System.Drawing.Point(25, 118);
            this.lblCard2Title.Name = "lblCard2Title";
            this.lblCard2Title.Size = new System.Drawing.Size(129, 20);
            this.lblCard2Title.TabIndex = 3;
            this.lblCard2Title.Text = "Đã khám hôm nay";
            // 
            // pnlCard3
            // 
            this.pnlCard3.BackColor = System.Drawing.Color.Transparent;
            this.pnlCard3.BorderRadius = 12;
            this.pnlCard3.Controls.Add(this.pnlAccent3);
            this.pnlCard3.Controls.Add(this.lblCard3Icon);
            this.pnlCard3.Controls.Add(this.lblCard3Value);
            this.pnlCard3.Controls.Add(this.lblCard3Title);
            this.pnlCard3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCard3.FillColor = System.Drawing.Color.White;
            this.pnlCard3.Location = new System.Drawing.Point(882, 0);
            this.pnlCard3.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCard3.Name = "pnlCard3";
            this.pnlCard3.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlCard3.ShadowDecoration.Depth = 4;
            this.pnlCard3.ShadowDecoration.Enabled = true;
            this.pnlCard3.Size = new System.Drawing.Size(443, 163);
            this.pnlCard3.TabIndex = 2;
            // 
            // pnlAccent3
            // 
            this.pnlAccent3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.pnlAccent3.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlAccent3.Location = new System.Drawing.Point(0, 0);
            this.pnlAccent3.Name = "pnlAccent3";
            this.pnlAccent3.Size = new System.Drawing.Size(5, 163);
            this.pnlAccent3.TabIndex = 0;
            // 
            // lblCard3Icon
            // 
            this.lblCard3Icon.AutoSize = true;
            this.lblCard3Icon.BackColor = System.Drawing.Color.Transparent;
            this.lblCard3Icon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.lblCard3Icon.Location = new System.Drawing.Point(25, 18);
            this.lblCard3Icon.Name = "lblCard3Icon";
            this.lblCard3Icon.Size = new System.Drawing.Size(52, 36);
            this.lblCard3Icon.TabIndex = 1;
            this.lblCard3Icon.Text = "📅";
            // 
            // lblCard3Value
            // 
            this.lblCard3Value.AutoSize = true;
            this.lblCard3Value.BackColor = System.Drawing.Color.Transparent;
            this.lblCard3Value.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCard3Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.lblCard3Value.Location = new System.Drawing.Point(25, 55);
            this.lblCard3Value.Name = "lblCard3Value";
            this.lblCard3Value.Size = new System.Drawing.Size(54, 46);
            this.lblCard3Value.TabIndex = 2;
            this.lblCard3Value.Text = "—";
            // 
            // lblCard3Title
            // 
            this.lblCard3Title.AutoSize = true;
            this.lblCard3Title.BackColor = System.Drawing.Color.Transparent;
            this.lblCard3Title.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCard3Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblCard3Title.Location = new System.Drawing.Point(25, 118);
            this.lblCard3Title.Name = "lblCard3Title";
            this.lblCard3Title.Size = new System.Drawing.Size(138, 20);
            this.lblCard3Title.TabIndex = 3;
            this.lblCard3Title.Text = "Ca làm việc hiện tại";
            // 
            // tlpContent
            // 
            this.tlpContent.BackColor = System.Drawing.Color.Transparent;
            this.tlpContent.ColumnCount = 2;
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpContent.Controls.Add(this.pnlQueueBenhNhan, 0, 0);
            this.tlpContent.Controls.Add(this.pnlLichLamViec, 1, 0);
            this.tlpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContent.Location = new System.Drawing.Point(25, 203);
            this.tlpContent.Margin = new System.Windows.Forms.Padding(0);
            this.tlpContent.Name = "tlpContent";
            this.tlpContent.RowCount = 1;
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.Size = new System.Drawing.Size(1325, 672);
            this.tlpContent.TabIndex = 1;
            // 
            // pnlQueueBenhNhan
            // 
            this.pnlQueueBenhNhan.BackColor = System.Drawing.Color.Transparent;
            this.pnlQueueBenhNhan.BorderRadius = 12;
            this.pnlQueueBenhNhan.Controls.Add(this.dgvQueue);
            this.pnlQueueBenhNhan.Controls.Add(this.tlpActionButtons);
            this.pnlQueueBenhNhan.Controls.Add(this.lblQueueTitle);
            this.pnlQueueBenhNhan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQueueBenhNhan.FillColor = System.Drawing.Color.White;
            this.pnlQueueBenhNhan.Location = new System.Drawing.Point(0, 0);
            this.pnlQueueBenhNhan.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pnlQueueBenhNhan.Name = "pnlQueueBenhNhan";
            this.pnlQueueBenhNhan.Padding = new System.Windows.Forms.Padding(16, 14, 16, 14);
            this.pnlQueueBenhNhan.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlQueueBenhNhan.ShadowDecoration.Depth = 4;
            this.pnlQueueBenhNhan.ShadowDecoration.Enabled = true;
            this.pnlQueueBenhNhan.Size = new System.Drawing.Size(716, 672);
            this.pnlQueueBenhNhan.TabIndex = 0;
            // 
            // tlpActionButtons
            // 
            this.tlpActionButtons.BackColor = System.Drawing.Color.Transparent;
            this.tlpActionButtons.ColumnCount = 2;
            this.tlpActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlpActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpActionButtons.Controls.Add(this.btnBatDauKham, 0, 0);
            this.tlpActionButtons.Controls.Add(this.btnXemHoSo, 1, 0);
            this.tlpActionButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tlpActionButtons.Location = new System.Drawing.Point(16, 608);
            this.tlpActionButtons.Name = "tlpActionButtons";
            this.tlpActionButtons.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.tlpActionButtons.RowCount = 1;
            this.tlpActionButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpActionButtons.Size = new System.Drawing.Size(684, 50);
            this.tlpActionButtons.TabIndex = 3;
            // 
            // btnBatDauKham
            // 
            this.btnBatDauKham.BorderRadius = 10;
            this.btnBatDauKham.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBatDauKham.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBatDauKham.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnBatDauKham.FillColor2 = System.Drawing.Color.SeaGreen;
            this.btnBatDauKham.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnBatDauKham.ForeColor = System.Drawing.Color.White;
            this.btnBatDauKham.Location = new System.Drawing.Point(0, 8);
            this.btnBatDauKham.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnBatDauKham.Name = "btnBatDauKham";
            this.btnBatDauKham.Size = new System.Drawing.Size(370, 42);
            this.btnBatDauKham.TabIndex = 0;
            this.btnBatDauKham.Text = "🩺  Bắt Đầu Khám";
            // 
            // btnXemHoSo
            // 
            this.btnXemHoSo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnXemHoSo.BorderRadius = 10;
            this.btnXemHoSo.BorderThickness = 1;
            this.btnXemHoSo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXemHoSo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnXemHoSo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.btnXemHoSo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnXemHoSo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnXemHoSo.Location = new System.Drawing.Point(382, 8);
            this.btnXemHoSo.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnXemHoSo.Name = "btnXemHoSo";
            this.btnXemHoSo.Size = new System.Drawing.Size(302, 42);
            this.btnXemHoSo.TabIndex = 1;
            this.btnXemHoSo.Text = "📋  Xem Hồ Sơ";
            // 
            // pnlLichLamViec
            // 
            this.pnlLichLamViec.BackColor = System.Drawing.Color.Transparent;
            this.pnlLichLamViec.BorderRadius = 12;
            this.pnlLichLamViec.Controls.Add(this.dgvLich);
            this.pnlLichLamViec.Controls.Add(this.lblLichTitle);
            this.pnlLichLamViec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLichLamViec.FillColor = System.Drawing.Color.White;
            this.pnlLichLamViec.Location = new System.Drawing.Point(728, 0);
            this.pnlLichLamViec.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLichLamViec.Name = "pnlLichLamViec";
            this.pnlLichLamViec.Padding = new System.Windows.Forms.Padding(16, 14, 16, 14);
            this.pnlLichLamViec.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlLichLamViec.ShadowDecoration.Depth = 4;
            this.pnlLichLamViec.ShadowDecoration.Enabled = true;
            this.pnlLichLamViec.Size = new System.Drawing.Size(597, 672);
            this.pnlLichLamViec.TabIndex = 1;
            // 
            // lblLichTitle
            // 
            this.lblLichTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblLichTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLichTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLichTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblLichTitle.Location = new System.Drawing.Point(16, 14);
            this.lblLichTitle.Name = "lblLichTitle";
            this.lblLichTitle.Size = new System.Drawing.Size(565, 40);
            this.lblLichTitle.TabIndex = 0;
            this.lblLichTitle.Text = "📅  Lịch Làm Việc Tuần Này";
            this.lblLichTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvLich
            // 
            this.dgvLich.AllowUserToAddRows = false;
            this.dgvLich.AllowUserToDeleteRows = false;
            this.dgvLich.AllowUserToResizeColumns = false;
            this.dgvLich.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvLich.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLich.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvLich.ColumnHeadersHeight = 40;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLich.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvLich.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLich.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvLich.Location = new System.Drawing.Point(16, 54);
            this.dgvLich.MultiSelect = false;
            this.dgvLich.Name = "dgvLich";
            this.dgvLich.ReadOnly = true;
            this.dgvLich.RowHeadersVisible = false;
            this.dgvLich.RowHeadersWidth = 51;
            this.dgvLich.RowTemplate.Height = 42;
            this.dgvLich.Size = new System.Drawing.Size(565, 604);
            this.dgvLich.TabIndex = 2;
            this.dgvLich.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvLich.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvLich.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvLich.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvLich.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvLich.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvLich.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvLich.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.dgvLich.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvLich.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.dgvLich.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvLich.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLich.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvLich.ThemeStyle.ReadOnly = true;
            this.dgvLich.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvLich.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLich.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvLich.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.dgvLich.ThemeStyle.RowsStyle.Height = 42;
            this.dgvLich.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.dgvLich.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            // 
            // dgvQueue
            // 
            this.dgvQueue.AllowUserToAddRows = false;
            this.dgvQueue.AllowUserToDeleteRows = false;
            this.dgvQueue.AllowUserToResizeColumns = false;
            this.dgvQueue.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvQueue.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvQueue.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvQueue.ColumnHeadersHeight = 40;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvQueue.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvQueue.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvQueue.Location = new System.Drawing.Point(16, 54);
            this.dgvQueue.MultiSelect = false;
            this.dgvQueue.Name = "dgvQueue";
            this.dgvQueue.ReadOnly = true;
            this.dgvQueue.RowHeadersVisible = false;
            this.dgvQueue.RowHeadersWidth = 51;
            this.dgvQueue.RowTemplate.Height = 42;
            this.dgvQueue.Size = new System.Drawing.Size(684, 604);
            this.dgvQueue.TabIndex = 2;
            this.dgvQueue.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvQueue.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvQueue.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvQueue.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvQueue.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvQueue.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvQueue.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvQueue.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.dgvQueue.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvQueue.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.dgvQueue.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvQueue.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvQueue.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvQueue.ThemeStyle.ReadOnly = true;
            this.dgvQueue.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvQueue.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvQueue.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvQueue.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.dgvQueue.ThemeStyle.RowsStyle.Height = 42;
            this.dgvQueue.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.dgvQueue.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            // 
            // lblQueueTitle
            // 
            this.lblQueueTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblQueueTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblQueueTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblQueueTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblQueueTitle.Location = new System.Drawing.Point(16, 14);
            this.lblQueueTitle.Name = "lblQueueTitle";
            this.lblQueueTitle.Size = new System.Drawing.Size(684, 40);
            this.lblQueueTitle.TabIndex = 0;
            this.lblQueueTitle.Text = "🩺  Bệnh Nhân Chờ Khám";
            this.lblQueueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DashboardBacSiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1375, 900);
            this.Controls.Add(this.tlpContent);
            this.Controls.Add(this.tlpStatCards);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DashboardBacSiForm";
            this.Padding = new System.Windows.Forms.Padding(25);
            this.Text = "Dashboard — Bác Sĩ";
            this.tlpStatCards.ResumeLayout(false);
            this.pnlCard1.ResumeLayout(false);
            this.pnlCard1.PerformLayout();
            this.pnlCard2.ResumeLayout(false);
            this.pnlCard2.PerformLayout();
            this.pnlCard3.ResumeLayout(false);
            this.pnlCard3.PerformLayout();
            this.tlpContent.ResumeLayout(false);
            this.pnlQueueBenhNhan.ResumeLayout(false);
            this.tlpActionButtons.ResumeLayout(false);
            this.pnlLichLamViec.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLich)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).EndInit();
            this.ResumeLayout(false);

        }

        private Guna2DataGridView dgvQueue;
        private Label lblQueueTitle;
    }
}
