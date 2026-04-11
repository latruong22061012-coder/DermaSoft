using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace DermaSoft.Forms
{
    partial class DashboardLeTanForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Controls
        // ── KPI stat cards row ────────────────────────────────────────────
        private System.Windows.Forms.TableLayoutPanel tlpStatCards;

        private Guna2Panel pnlCard1;
        private System.Windows.Forms.Panel pnlAccent1;    // Thanh màu trái card 1
        private System.Windows.Forms.Label lblCard1Icon;  // Emoji icon
        private System.Windows.Forms.Label lblCard1Value; // Số lớn
        private System.Windows.Forms.Label lblCard1Title; // Nhãn nhỏ

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

        private Guna2Panel pnlCard4;
        private System.Windows.Forms.Panel pnlAccent4;
        private System.Windows.Forms.Label lblCard4Icon;
        private System.Windows.Forms.Label lblCard4Value;
        private System.Windows.Forms.Label lblCard4Title;

        // ── Vùng nội dung chính: 2 cột ───────────────────────────────────
        private System.Windows.Forms.TableLayoutPanel tlpContent;

        // Cột trái — Lịch hẹn hôm nay
        private Guna2Panel pnlLichHen;
        private System.Windows.Forms.Label lblTitleLichHen;
        private Guna2DataGridView dgvQueue;
        private DataGridViewTextBoxColumn colGio;
        private DataGridViewTextBoxColumn colBenhNhan;
        private DataGridViewTextBoxColumn colSDT;
        private DataGridViewTextBoxColumn colBacSi;
        private DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.TableLayoutPanel tlpActionButtons;
        private Guna2GradientButton btnTiepNhan;
        private Guna2Button btnTaoLich;
        private Guna2Button btnXacNhan;

        // Cột phải — Hóa đơn cần thu
        private Guna2Panel pnlHoaDon;
        private System.Windows.Forms.Label lblTitleHoaDon;
        private System.Windows.Forms.FlowLayoutPanel flpHoaDonList;
        #endregion

        private void InitializeComponent()
        {
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
            this.pnlCard4 = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlAccent4 = new System.Windows.Forms.Panel();
            this.lblCard4Icon = new System.Windows.Forms.Label();
            this.lblCard4Value = new System.Windows.Forms.Label();
            this.lblCard4Title = new System.Windows.Forms.Label();
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLichHen = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvQueue = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colGio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBenhNhan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSDT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBacSi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpActionButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnTiepNhan = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnTaoLich = new Guna.UI2.WinForms.Guna2Button();
            this.btnXacNhan = new Guna.UI2.WinForms.Guna2Button();
            this.lblTitleLichHen = new System.Windows.Forms.Label();
            this.pnlHoaDon = new Guna.UI2.WinForms.Guna2Panel();
            this.flpHoaDonList = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTitleHoaDon = new System.Windows.Forms.Label();
            this.tlpStatCards.SuspendLayout();
            this.pnlCard1.SuspendLayout();
            this.pnlCard2.SuspendLayout();
            this.pnlCard3.SuspendLayout();
            this.pnlCard4.SuspendLayout();
            this.tlpContent.SuspendLayout();
            this.pnlLichHen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).BeginInit();
            this.tlpActionButtons.SuspendLayout();
            this.pnlHoaDon.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpStatCards
            // 
            this.tlpStatCards.BackColor = System.Drawing.Color.Transparent;
            this.tlpStatCards.ColumnCount = 4;
            this.tlpStatCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpStatCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpStatCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpStatCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpStatCards.Controls.Add(this.pnlCard1, 0, 0);
            this.tlpStatCards.Controls.Add(this.pnlCard2, 1, 0);
            this.tlpStatCards.Controls.Add(this.pnlCard3, 2, 0);
            this.tlpStatCards.Controls.Add(this.pnlCard4, 3, 0);
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
            this.pnlCard1.Size = new System.Drawing.Size(319, 163);
            this.pnlCard1.TabIndex = 0;
            // 
            // pnlAccent1
            // 
            this.pnlAccent1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.pnlAccent1.Location = new System.Drawing.Point(0, 0);
            this.pnlAccent1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlAccent1.Name = "pnlAccent1";
            this.pnlAccent1.Size = new System.Drawing.Size(5, 162);
            this.pnlAccent1.TabIndex = 0;
            // 
            // lblCard1Icon
            // 
            this.lblCard1Icon.AutoSize = true;
            this.lblCard1Icon.BackColor = System.Drawing.Color.Transparent;
            this.lblCard1Icon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.lblCard1Icon.Location = new System.Drawing.Point(25, 18);
            this.lblCard1Icon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCard1Icon.Name = "lblCard1Icon";
            this.lblCard1Icon.Size = new System.Drawing.Size(52, 36);
            this.lblCard1Icon.TabIndex = 1;
            this.lblCard1Icon.Text = "📅";
            // 
            // lblCard1Value
            // 
            this.lblCard1Value.AutoSize = true;
            this.lblCard1Value.BackColor = System.Drawing.Color.Transparent;
            this.lblCard1Value.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCard1Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.lblCard1Value.Location = new System.Drawing.Point(25, 55);
            this.lblCard1Value.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCard1Value.Name = "lblCard1Value";
            this.lblCard1Value.Size = new System.Drawing.Size(45, 54);
            this.lblCard1Value.TabIndex = 2;
            this.lblCard1Value.Text = "..";
            // 
            // lblCard1Title
            // 
            this.lblCard1Title.AutoSize = true;
            this.lblCard1Title.BackColor = System.Drawing.Color.Transparent;
            this.lblCard1Title.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCard1Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblCard1Title.Location = new System.Drawing.Point(25, 128);
            this.lblCard1Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCard1Title.Name = "lblCard1Title";
            this.lblCard1Title.Size = new System.Drawing.Size(139, 20);
            this.lblCard1Title.TabIndex = 3;
            this.lblCard1Title.Text = "Lịch Hẹn Hôm Nay";
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
            this.pnlCard2.Location = new System.Drawing.Point(331, 0);
            this.pnlCard2.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pnlCard2.Name = "pnlCard2";
            this.pnlCard2.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlCard2.ShadowDecoration.Depth = 4;
            this.pnlCard2.ShadowDecoration.Enabled = true;
            this.pnlCard2.Size = new System.Drawing.Size(319, 163);
            this.pnlCard2.TabIndex = 1;
            // 
            // pnlAccent2
            // 
            this.pnlAccent2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.pnlAccent2.Location = new System.Drawing.Point(0, 0);
            this.pnlAccent2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlAccent2.Name = "pnlAccent2";
            this.pnlAccent2.Size = new System.Drawing.Size(5, 162);
            this.pnlAccent2.TabIndex = 0;
            // 
            // lblCard2Icon
            // 
            this.lblCard2Icon.AutoSize = true;
            this.lblCard2Icon.BackColor = System.Drawing.Color.Transparent;
            this.lblCard2Icon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.lblCard2Icon.Location = new System.Drawing.Point(25, 18);
            this.lblCard2Icon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCard2Icon.Name = "lblCard2Icon";
            this.lblCard2Icon.Size = new System.Drawing.Size(52, 36);
            this.lblCard2Icon.TabIndex = 1;
            this.lblCard2Icon.Text = "⏳";
            // 
            // lblCard2Value
            // 
            this.lblCard2Value.AutoSize = true;
            this.lblCard2Value.BackColor = System.Drawing.Color.Transparent;
            this.lblCard2Value.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCard2Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.lblCard2Value.Location = new System.Drawing.Point(25, 55);
            this.lblCard2Value.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCard2Value.Name = "lblCard2Value";
            this.lblCard2Value.Size = new System.Drawing.Size(45, 54);
            this.lblCard2Value.TabIndex = 2;
            this.lblCard2Value.Text = "..";
            // 
            // lblCard2Title
            // 
            this.lblCard2Title.AutoSize = true;
            this.lblCard2Title.BackColor = System.Drawing.Color.Transparent;
            this.lblCard2Title.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCard2Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblCard2Title.Location = new System.Drawing.Point(25, 128);
            this.lblCard2Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCard2Title.Name = "lblCard2Title";
            this.lblCard2Title.Size = new System.Drawing.Size(159, 20);
            this.lblCard2Title.TabIndex = 3;
            this.lblCard2Title.Text = "Bệnh Nhân Đang Chờ";
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
            this.pnlCard3.Location = new System.Drawing.Point(662, 0);
            this.pnlCard3.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pnlCard3.Name = "pnlCard3";
            this.pnlCard3.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlCard3.ShadowDecoration.Depth = 4;
            this.pnlCard3.ShadowDecoration.Enabled = true;
            this.pnlCard3.Size = new System.Drawing.Size(319, 163);
            this.pnlCard3.TabIndex = 2;
            // 
            // pnlAccent3
            // 
            this.pnlAccent3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.pnlAccent3.Location = new System.Drawing.Point(0, 0);
            this.pnlAccent3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlAccent3.Name = "pnlAccent3";
            this.pnlAccent3.Size = new System.Drawing.Size(5, 162);
            this.pnlAccent3.TabIndex = 0;
            // 
            // lblCard3Icon
            // 
            this.lblCard3Icon.AutoSize = true;
            this.lblCard3Icon.BackColor = System.Drawing.Color.Transparent;
            this.lblCard3Icon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.lblCard3Icon.Location = new System.Drawing.Point(25, 18);
            this.lblCard3Icon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCard3Icon.Name = "lblCard3Icon";
            this.lblCard3Icon.Size = new System.Drawing.Size(52, 36);
            this.lblCard3Icon.TabIndex = 1;
            this.lblCard3Icon.Text = "💳";
            // 
            // lblCard3Value
            // 
            this.lblCard3Value.AutoSize = true;
            this.lblCard3Value.BackColor = System.Drawing.Color.Transparent;
            this.lblCard3Value.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCard3Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.lblCard3Value.Location = new System.Drawing.Point(25, 55);
            this.lblCard3Value.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCard3Value.Name = "lblCard3Value";
            this.lblCard3Value.Size = new System.Drawing.Size(45, 54);
            this.lblCard3Value.TabIndex = 2;
            this.lblCard3Value.Text = "..";
            // 
            // lblCard3Title
            // 
            this.lblCard3Title.AutoSize = true;
            this.lblCard3Title.BackColor = System.Drawing.Color.Transparent;
            this.lblCard3Title.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCard3Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblCard3Title.Location = new System.Drawing.Point(25, 128);
            this.lblCard3Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCard3Title.Name = "lblCard3Title";
            this.lblCard3Title.Size = new System.Drawing.Size(186, 20);
            this.lblCard3Title.TabIndex = 3;
            this.lblCard3Title.Text = "Hóa Đơn Cần Thanh Toán";
            // 
            // pnlCard4
            // 
            this.pnlCard4.BackColor = System.Drawing.Color.Transparent;
            this.pnlCard4.BorderRadius = 12;
            this.pnlCard4.Controls.Add(this.pnlAccent4);
            this.pnlCard4.Controls.Add(this.lblCard4Icon);
            this.pnlCard4.Controls.Add(this.lblCard4Value);
            this.pnlCard4.Controls.Add(this.lblCard4Title);
            this.pnlCard4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCard4.FillColor = System.Drawing.Color.White;
            this.pnlCard4.Location = new System.Drawing.Point(993, 0);
            this.pnlCard4.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCard4.Name = "pnlCard4";
            this.pnlCard4.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlCard4.ShadowDecoration.Depth = 4;
            this.pnlCard4.ShadowDecoration.Enabled = true;
            this.pnlCard4.Size = new System.Drawing.Size(332, 163);
            this.pnlCard4.TabIndex = 3;
            // 
            // pnlAccent4
            // 
            this.pnlAccent4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.pnlAccent4.Location = new System.Drawing.Point(0, 0);
            this.pnlAccent4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlAccent4.Name = "pnlAccent4";
            this.pnlAccent4.Size = new System.Drawing.Size(5, 162);
            this.pnlAccent4.TabIndex = 0;
            // 
            // lblCard4Icon
            // 
            this.lblCard4Icon.AutoSize = true;
            this.lblCard4Icon.BackColor = System.Drawing.Color.Transparent;
            this.lblCard4Icon.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.lblCard4Icon.Location = new System.Drawing.Point(25, 18);
            this.lblCard4Icon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCard4Icon.Name = "lblCard4Icon";
            this.lblCard4Icon.Size = new System.Drawing.Size(52, 36);
            this.lblCard4Icon.TabIndex = 1;
            this.lblCard4Icon.Text = "✅";
            // 
            // lblCard4Value
            // 
            this.lblCard4Value.AutoSize = true;
            this.lblCard4Value.BackColor = System.Drawing.Color.Transparent;
            this.lblCard4Value.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblCard4Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(46)))), ((int)(((byte)(37)))));
            this.lblCard4Value.Location = new System.Drawing.Point(25, 55);
            this.lblCard4Value.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCard4Value.Name = "lblCard4Value";
            this.lblCard4Value.Size = new System.Drawing.Size(45, 54);
            this.lblCard4Value.TabIndex = 2;
            this.lblCard4Value.Text = "..";
            // 
            // lblCard4Title
            // 
            this.lblCard4Title.AutoSize = true;
            this.lblCard4Title.BackColor = System.Drawing.Color.Transparent;
            this.lblCard4Title.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCard4Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblCard4Title.Location = new System.Drawing.Point(25, 128);
            this.lblCard4Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCard4Title.Name = "lblCard4Title";
            this.lblCard4Title.Size = new System.Drawing.Size(174, 20);
            this.lblCard4Title.TabIndex = 3;
            this.lblCard4Title.Text = "Đã Tiếp Nhận Hôm Nay";
            // 
            // tlpContent
            // 
            this.tlpContent.ColumnCount = 2;
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpContent.Controls.Add(this.pnlLichHen, 0, 0);
            this.tlpContent.Controls.Add(this.pnlHoaDon, 1, 0);
            this.tlpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContent.Location = new System.Drawing.Point(25, 203);
            this.tlpContent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tlpContent.Name = "tlpContent";
            this.tlpContent.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.tlpContent.RowCount = 1;
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.Size = new System.Drawing.Size(1325, 672);
            this.tlpContent.TabIndex = 1;
            // 
            // pnlLichHen
            // 
            this.pnlLichHen.BackColor = System.Drawing.Color.Transparent;
            this.pnlLichHen.BorderRadius = 12;
            this.pnlLichHen.Controls.Add(this.dgvQueue);
            this.pnlLichHen.Controls.Add(this.tlpActionButtons);
            this.pnlLichHen.Controls.Add(this.lblTitleLichHen);
            this.pnlLichHen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLichHen.FillColor = System.Drawing.Color.White;
            this.pnlLichHen.Location = new System.Drawing.Point(0, 10);
            this.pnlLichHen.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pnlLichHen.Name = "pnlLichHen";
            this.pnlLichHen.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            this.pnlLichHen.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlLichHen.ShadowDecoration.Depth = 4;
            this.pnlLichHen.ShadowDecoration.Enabled = true;
            this.pnlLichHen.Size = new System.Drawing.Size(716, 662);
            this.pnlLichHen.TabIndex = 0;
            // 
            // dgvQueue
            // 
            this.dgvQueue.AllowUserToAddRows = false;
            this.dgvQueue.AllowUserToDeleteRows = false;
            this.dgvQueue.AllowUserToResizeColumns = false;
            this.dgvQueue.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(247)))));
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
            this.dgvQueue.ColumnHeadersHeight = 38;
            this.dgvQueue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGio,
            this.colBenhNhan,
            this.colSDT,
            this.colBacSi,
            this.colTrangThai});
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
            this.dgvQueue.Location = new System.Drawing.Point(20, 53);
            this.dgvQueue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvQueue.MultiSelect = false;
            this.dgvQueue.Name = "dgvQueue";
            this.dgvQueue.ReadOnly = true;
            this.dgvQueue.RowHeadersVisible = false;
            this.dgvQueue.RowHeadersWidth = 51;
            this.dgvQueue.RowTemplate.Height = 40;
            this.dgvQueue.Size = new System.Drawing.Size(676, 526);
            this.dgvQueue.TabIndex = 2;
            this.dgvQueue.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(251)))), ((int)(((byte)(247)))));
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
            this.dgvQueue.ThemeStyle.HeaderStyle.Height = 38;
            this.dgvQueue.ThemeStyle.ReadOnly = true;
            this.dgvQueue.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvQueue.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvQueue.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvQueue.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.dgvQueue.ThemeStyle.RowsStyle.Height = 40;
            this.dgvQueue.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.dgvQueue.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            // 
            // colGio
            // 
            this.colGio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colGio.DataPropertyName = "ThoiGianHen";
            this.colGio.HeaderText = "Giờ";
            this.colGio.MinimumWidth = 6;
            this.colGio.Name = "colGio";
            this.colGio.ReadOnly = true;
            this.colGio.Width = 65;
            // 
            // colBenhNhan
            // 
            this.colBenhNhan.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBenhNhan.DataPropertyName = "HoTen";
            this.colBenhNhan.HeaderText = "Bệnh Nhân";
            this.colBenhNhan.MinimumWidth = 6;
            this.colBenhNhan.Name = "colBenhNhan";
            this.colBenhNhan.ReadOnly = true;
            // 
            // colSDT
            // 
            this.colSDT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSDT.DataPropertyName = "SoDienThoai";
            this.colSDT.HeaderText = "SĐT";
            this.colSDT.MinimumWidth = 6;
            this.colSDT.Name = "colSDT";
            this.colSDT.ReadOnly = true;
            // 
            // colBacSi
            // 
            this.colBacSi.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colBacSi.DataPropertyName = "TenBacSi";
            this.colBacSi.HeaderText = "Bác Sĩ";
            this.colBacSi.MinimumWidth = 6;
            this.colBacSi.Name = "colBacSi";
            this.colBacSi.ReadOnly = true;
            this.colBacSi.Width = 120;
            // 
            // colTrangThai
            // 
            this.colTrangThai.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTrangThai.DataPropertyName = "TrangThaiText";
            this.colTrangThai.HeaderText = "Trạng Thái";
            this.colTrangThai.MinimumWidth = 6;
            this.colTrangThai.Name = "colTrangThai";
            this.colTrangThai.ReadOnly = true;
            this.colTrangThai.Width = 110;
            // 
            // tlpActionButtons
            // 
            this.tlpActionButtons.BackColor = System.Drawing.Color.Transparent;
            this.tlpActionButtons.ColumnCount = 3;
            this.tlpActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tlpActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpActionButtons.Controls.Add(this.btnTiepNhan, 0, 0);
            this.tlpActionButtons.Controls.Add(this.btnTaoLich, 1, 0);
            this.tlpActionButtons.Controls.Add(this.btnXacNhan, 2, 0);
            this.tlpActionButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tlpActionButtons.Location = new System.Drawing.Point(20, 579);
            this.tlpActionButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tlpActionButtons.Name = "tlpActionButtons";
            this.tlpActionButtons.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.tlpActionButtons.RowCount = 1;
            this.tlpActionButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpActionButtons.Size = new System.Drawing.Size(676, 65);
            this.tlpActionButtons.TabIndex = 1;
            // 
            // btnTiepNhan
            // 
            this.btnTiepNhan.Animated = true;
            this.btnTiepNhan.BorderRadius = 18;
            this.btnTiepNhan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTiepNhan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTiepNhan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnTiepNhan.FillColor2 = System.Drawing.Color.SeaGreen;
            this.btnTiepNhan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTiepNhan.ForeColor = System.Drawing.Color.White;
            this.btnTiepNhan.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(64)))), ((int)(((byte)(53)))));
            this.btnTiepNhan.HoverState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(64)))), ((int)(((byte)(53)))));
            this.btnTiepNhan.Location = new System.Drawing.Point(0, 12);
            this.btnTiepNhan.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnTiepNhan.Name = "btnTiepNhan";
            this.btnTiepNhan.Size = new System.Drawing.Size(215, 53);
            this.btnTiepNhan.TabIndex = 0;
            this.btnTiepNhan.Text = "📋  Tiếp Nhận";
            // 
            // btnTaoLich
            // 
            this.btnTaoLich.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnTaoLich.BorderRadius = 18;
            this.btnTaoLich.BorderThickness = 1;
            this.btnTaoLich.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTaoLich.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTaoLich.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.btnTaoLich.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTaoLich.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnTaoLich.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(230)))), ((int)(((byte)(207)))));
            this.btnTaoLich.Location = new System.Drawing.Point(231, 12);
            this.btnTaoLich.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.btnTaoLich.Name = "btnTaoLich";
            this.btnTaoLich.Size = new System.Drawing.Size(213, 53);
            this.btnTaoLich.TabIndex = 1;
            this.btnTaoLich.Text = "➕  Tạo Lịch Mới";
            // 
            // btnXacNhan
            // 
            this.btnXacNhan.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnXacNhan.BorderRadius = 18;
            this.btnXacNhan.BorderThickness = 1;
            this.btnXacNhan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXacNhan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnXacNhan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(229)))));
            this.btnXacNhan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnXacNhan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.btnXacNhan.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(230)))), ((int)(((byte)(207)))));
            this.btnXacNhan.Location = new System.Drawing.Point(460, 12);
            this.btnXacNhan.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.Size = new System.Drawing.Size(216, 53);
            this.btnXacNhan.TabIndex = 2;
            this.btnXacNhan.Text = "✅  Xác Nhận";
            // 
            // lblTitleLichHen
            // 
            this.lblTitleLichHen.AutoSize = false;
            this.lblTitleLichHen.Size = new System.Drawing.Size(500, 35);
            this.lblTitleLichHen.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleLichHen.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleLichHen.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitleLichHen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTitleLichHen.Location = new System.Drawing.Point(20, 18);
            this.lblTitleLichHen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitleLichHen.Name = "lblTitleLichHen";
            this.lblTitleLichHen.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblTitleLichHen.Size = new System.Drawing.Size(210, 35);
            this.lblTitleLichHen.TabIndex = 0;
            this.lblTitleLichHen.Text = "📅  Lịch Hẹn Hôm Nay";
            this.lblTitleLichHen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlHoaDon
            // 
            this.pnlHoaDon.BackColor = System.Drawing.Color.Transparent;
            this.pnlHoaDon.BorderRadius = 12;
            this.pnlHoaDon.Controls.Add(this.flpHoaDonList);
            this.pnlHoaDon.Controls.Add(this.lblTitleHoaDon);
            this.pnlHoaDon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHoaDon.FillColor = System.Drawing.Color.White;
            this.pnlHoaDon.Location = new System.Drawing.Point(740, 10);
            this.pnlHoaDon.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.pnlHoaDon.Name = "pnlHoaDon";
            this.pnlHoaDon.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            this.pnlHoaDon.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlHoaDon.ShadowDecoration.Depth = 4;
            this.pnlHoaDon.ShadowDecoration.Enabled = true;
            this.pnlHoaDon.Size = new System.Drawing.Size(585, 662);
            this.pnlHoaDon.TabIndex = 1;
            // 
            // flpHoaDonList
            // 
            this.flpHoaDonList.AutoScroll = true;
            this.flpHoaDonList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpHoaDonList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpHoaDonList.Location = new System.Drawing.Point(20, 53);
            this.flpHoaDonList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flpHoaDonList.Name = "flpHoaDonList";
            this.flpHoaDonList.Size = new System.Drawing.Size(545, 591);
            this.flpHoaDonList.TabIndex = 1;
            this.flpHoaDonList.WrapContents = false;
            // 
            // lblTitleHoaDon
            // 
            this.lblTitleHoaDon.AutoSize = false;
            this.lblTitleHoaDon.Size = new System.Drawing.Size(500, 35);
            this.lblTitleHoaDon.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleHoaDon.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleHoaDon.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitleHoaDon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(92)))), ((int)(((byte)(77)))));
            this.lblTitleHoaDon.Location = new System.Drawing.Point(20, 18);
            this.lblTitleHoaDon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitleHoaDon.Name = "lblTitleHoaDon";
            this.lblTitleHoaDon.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblTitleHoaDon.Size = new System.Drawing.Size(202, 35);
            this.lblTitleHoaDon.TabIndex = 0;
            this.lblTitleHoaDon.Text = "💳  Hóa Đơn Cần Thu";
            this.lblTitleHoaDon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DashboardLeTanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1375, 900);
            this.Controls.Add(this.tlpContent);
            this.Controls.Add(this.tlpStatCards);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DashboardLeTanForm";
            this.Padding = new System.Windows.Forms.Padding(25, 25, 25, 25);
            this.Text = "Dashboard — Lễ Tân";
            this.tlpStatCards.ResumeLayout(false);
            this.pnlCard1.ResumeLayout(false);
            this.pnlCard1.PerformLayout();
            this.pnlCard2.ResumeLayout(false);
            this.pnlCard2.PerformLayout();
            this.pnlCard3.ResumeLayout(false);
            this.pnlCard3.PerformLayout();
            this.pnlCard4.ResumeLayout(false);
            this.pnlCard4.PerformLayout();
            this.tlpContent.ResumeLayout(false);
            this.pnlLichHen.ResumeLayout(false);
            this.pnlLichHen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).EndInit();
            this.tlpActionButtons.ResumeLayout(false);
            this.pnlHoaDon.ResumeLayout(false);
            this.pnlHoaDon.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}