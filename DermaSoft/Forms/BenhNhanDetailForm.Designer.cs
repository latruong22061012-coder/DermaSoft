namespace DermaSoft.Forms
{
    partial class BenhNhanDetailForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("PK#024 — 24/03");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("PK#018 — 12/02");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("PK#005 — 05/01");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("2026 (3 lần)", new System.Windows.Forms.TreeNode[] { treeNode1, treeNode2, treeNode3 });
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("PK#099 — 15/12");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("PK#088 — 10/11");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("PK#077 — 05/10");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("PK#066 — 20/09");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("PK#055 — 01/08");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("2025 (5 lần)", new System.Windows.Forms.TreeNode[] { treeNode5, treeNode6, treeNode7, treeNode8, treeNode9 });

            // Shared DGV styles — SelectionBackColor bắt buộc để header đồng màu (15,92,77)
            System.Windows.Forms.DataGridViewCellStyle dgvHeaderStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvAltStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvDefaultStyle = new System.Windows.Forms.DataGridViewCellStyle();

            dgvHeaderStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvHeaderStyle.BackColor = System.Drawing.Color.FromArgb(15, 92, 77);
            dgvHeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            dgvHeaderStyle.ForeColor = System.Drawing.Color.White;
            dgvHeaderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(15, 92, 77); // [FIX] đồng màu khi focus
            dgvHeaderStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvHeaderStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            dgvAltStyle.BackColor = System.Drawing.Color.FromArgb(240, 250, 245);

            dgvDefaultStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvDefaultStyle.BackColor = System.Drawing.Color.White;
            dgvDefaultStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dgvDefaultStyle.ForeColor = System.Drawing.Color.FromArgb(26, 46, 37);
            dgvDefaultStyle.SelectionBackColor = System.Drawing.Color.FromArgb(221, 245, 229);
            dgvDefaultStyle.SelectionForeColor = System.Drawing.Color.FromArgb(15, 92, 77);

            // ── Khai báo controls ─────────────────────────────────────────
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlAvatarSection = new System.Windows.Forms.Panel();
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.lblTenBN = new System.Windows.Forms.Label();
            this.lblThongTinCo = new System.Windows.Forms.Label();
            this.pnlBadge = new Guna.UI2.WinForms.Guna2Panel();
            this.lblBadgeText = new System.Windows.Forms.Label();
            this.pnlTienSu = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTienSuTitle = new System.Windows.Forms.Label();
            this.lblTienSuND = new System.Windows.Forms.Label();
            this.pnlLichSuHeader = new System.Windows.Forms.Panel();
            this.lblLichSuTitle = new System.Windows.Forms.Label();
            this.trvLichSu = new System.Windows.Forms.TreeView();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlTabs = new System.Windows.Forms.Panel();
            this.btnTabThongTin = new Guna.UI2.WinForms.Guna2Button();
            this.btnTabDonThuoc = new Guna.UI2.WinForms.Guna2Button();
            this.btnTabHinhAnh = new Guna.UI2.WinForms.Guna2Button();
            this.btnTabGhiChu = new Guna.UI2.WinForms.Guna2Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            // TAB 1
            this.pnlThongTinFull = new System.Windows.Forms.Panel();
            this.tblContent = new System.Windows.Forms.TableLayoutPanel();
            this.tblTop = new System.Windows.Forms.TableLayoutPanel();
            this.cardExamInfo = new Guna.UI2.WinForms.Guna2Panel();
            this.tblExamInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lblExamTitle = new System.Windows.Forms.Label();
            this.lblDoctor = new System.Windows.Forms.Label();
            this.lblSymptom = new System.Windows.Forms.Label();
            this.lblDiagnosis = new System.Windows.Forms.Label();
            this.lblReExam = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.grbPrescription = new Guna.UI2.WinForms.Guna2GroupBox();
            this.dgvPrescription = new System.Windows.Forms.DataGridView();
            this.colThuoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLieuDung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tblStats = new System.Windows.Forms.TableLayoutPanel();
            this.cardStat1 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblStatValue1 = new System.Windows.Forms.Label();
            this.lblStatLabel1 = new System.Windows.Forms.Label();
            this.cardStat2 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblStatValue2 = new System.Windows.Forms.Label();
            this.lblStatLabel2 = new System.Windows.Forms.Label();
            this.cardStat3 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblStatValue3 = new System.Windows.Forms.Label();
            this.lblStatLabel3 = new System.Windows.Forms.Label();
            this.cardStat4 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblStatValue4 = new System.Windows.Forms.Label();
            this.lblStatLabel4 = new System.Windows.Forms.Label();
            // TAB 2
            this.pnlDonThuocFull = new System.Windows.Forms.Panel();
            this.dgvDonThuocFull = new System.Windows.Forms.DataGridView();
            this.colDTF_NgayKham = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDTF_TenThuoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDTF_SoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDTF_LieuDung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            // TAB 3
            this.pnlHinhAnhFull = new System.Windows.Forms.Panel();
            this.flpHinhAnh = new System.Windows.Forms.FlowLayoutPanel();
            this.lblHinhAnhEmpty = new System.Windows.Forms.Label();
            // TAB 4
            this.pnlGhiChuFull = new System.Windows.Forms.Panel();
            this.dgvGhiChu = new System.Windows.Forms.DataGridView();
            this.colGC_NgayKham = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGC_GhiChu = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.tlpMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlAvatarSection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
            this.pnlBadge.SuspendLayout();
            this.pnlTienSu.SuspendLayout();
            this.pnlLichSuHeader.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlTabs.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlThongTinFull.SuspendLayout();
            this.tblContent.SuspendLayout();
            this.tblTop.SuspendLayout();
            this.cardExamInfo.SuspendLayout();
            this.tblExamInfo.SuspendLayout();
            this.grbPrescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrescription)).BeginInit();
            this.tblStats.SuspendLayout();
            this.cardStat1.SuspendLayout();
            this.cardStat2.SuspendLayout();
            this.cardStat3.SuspendLayout();
            this.cardStat4.SuspendLayout();
            this.pnlDonThuocFull.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDonThuocFull)).BeginInit();
            this.pnlHinhAnhFull.SuspendLayout();
            this.pnlGhiChuFull.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGhiChu)).BeginInit();
            this.SuspendLayout();

            // ══ tlpMain ══════════════════════════════════════════════════════
            this.tlpMain.BackColor = System.Drawing.Color.Transparent;
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tlpMain.Controls.Add(this.pnlLeft, 0, 0);
            this.tlpMain.Controls.Add(this.pnlRight, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(1100, 760);
            this.tlpMain.TabIndex = 0;

            // ══ pnlLeft ═══════════════════════════════════════════════════════
            this.pnlLeft.Controls.Add(this.trvLichSu);
            this.pnlLeft.Controls.Add(this.pnlLichSuHeader);
            this.pnlLeft.Controls.Add(this.pnlTienSu);
            this.pnlLeft.Controls.Add(this.pnlAvatarSection);
            this.pnlLeft.BackColor = System.Drawing.Color.White;
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.TabIndex = 0;

            // ── pnlAvatarSection ──────────────────────────────────────────
            this.pnlAvatarSection.Controls.Add(this.picAvatar);
            this.pnlAvatarSection.Controls.Add(this.lblTenBN);
            this.pnlAvatarSection.Controls.Add(this.lblThongTinCo);
            this.pnlAvatarSection.Controls.Add(this.pnlBadge);
            this.pnlAvatarSection.BackColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.pnlAvatarSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAvatarSection.Height = 200;
            this.pnlAvatarSection.Name = "pnlAvatarSection";
            this.pnlAvatarSection.Padding = new System.Windows.Forms.Padding(16, 20, 16, 12);
            this.pnlAvatarSection.TabIndex = 0;

            this.picAvatar.BackColor = System.Drawing.Color.Transparent;
            this.picAvatar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAvatar.TabIndex = 2;
            this.picAvatar.TabStop = false;

            this.lblTenBN.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTenBN.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTenBN.ForeColor = System.Drawing.Color.White;
            this.lblTenBN.Height = 28;
            this.lblTenBN.Name = "lblTenBN";
            this.lblTenBN.Text = "Nguyễn Thị Anh";
            this.lblTenBN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTenBN.TabIndex = 1;

            this.lblThongTinCo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblThongTinCo.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblThongTinCo.ForeColor = System.Drawing.Color.FromArgb(168, 213, 197);
            this.lblThongTinCo.Height = 22;
            this.lblThongTinCo.Name = "lblThongTinCo";
            this.lblThongTinCo.Text = "0901234567 · ♀ · 35 tuổi";
            this.lblThongTinCo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblThongTinCo.TabIndex = 0;

            this.pnlBadge.Controls.Add(this.lblBadgeText);
            this.pnlBadge.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBadge.BorderRadius = 14;
            this.pnlBadge.FillColor = System.Drawing.Color.FromArgb(184, 138, 40);
            this.pnlBadge.Height = 28;
            this.pnlBadge.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.pnlBadge.Name = "pnlBadge";
            this.pnlBadge.TabIndex = 0;

            this.lblBadgeText.AutoSize = false;
            this.lblBadgeText.BackColor = System.Drawing.Color.Transparent;
            this.lblBadgeText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBadgeText.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblBadgeText.ForeColor = System.Drawing.Color.FromArgb(255, 248, 225);
            this.lblBadgeText.Name = "lblBadgeText";
            this.lblBadgeText.Text = "⭐ TV Vàng · 1,250 điểm";
            this.lblBadgeText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblBadgeText.TabIndex = 0;

            // ── pnlTienSu ─────────────────────────────────────────────────
            this.pnlTienSu.Controls.Add(this.lblTienSuND);
            this.pnlTienSu.Controls.Add(this.lblTienSuTitle);
            this.pnlTienSu.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTienSu.FillColor = System.Drawing.Color.White;
            this.pnlTienSu.Height = 80;
            this.pnlTienSu.Name = "pnlTienSu";
            this.pnlTienSu.Padding = new System.Windows.Forms.Padding(14, 8, 14, 8);
            this.pnlTienSu.TabIndex = 3;

            this.lblTienSuTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTienSuTitle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblTienSuTitle.ForeColor = System.Drawing.Color.FromArgb(252, 211, 77);
            this.lblTienSuTitle.Height = 22;
            this.lblTienSuTitle.Name = "lblTienSuTitle";
            this.lblTienSuTitle.Text = "⚠  Tiền sử bệnh lý:";
            this.lblTienSuTitle.TabIndex = 0;

            this.lblTienSuND.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTienSuND.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblTienSuND.ForeColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.lblTienSuND.Name = "lblTienSuND";
            this.lblTienSuND.Text = "Dị ứng Benzoyl Peroxide, Viêm da mãn tính";
            this.lblTienSuND.TabIndex = 1;

            // ── pnlLichSuHeader ───────────────────────────────────────────
            this.pnlLichSuHeader.Controls.Add(this.lblLichSuTitle);
            this.pnlLichSuHeader.BackColor = System.Drawing.Color.FromArgb(221, 245, 229);
            this.pnlLichSuHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLichSuHeader.Height = 36;
            this.pnlLichSuHeader.Name = "pnlLichSuHeader";
            this.pnlLichSuHeader.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.pnlLichSuHeader.TabIndex = 4;

            this.lblLichSuTitle.AutoSize = false;
            this.lblLichSuTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblLichSuTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLichSuTitle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblLichSuTitle.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblLichSuTitle.Name = "lblLichSuTitle";
            this.lblLichSuTitle.Text = "LỊCH SỬ KHÁM";
            this.lblLichSuTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLichSuTitle.TabIndex = 0;

            // ── trvLichSu — [FIX] OwnerDrawText để tô màu node được chọn ─
            this.trvLichSu.BackColor = System.Drawing.Color.White;
            this.trvLichSu.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trvLichSu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvLichSu.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.trvLichSu.ForeColor = System.Drawing.Color.FromArgb(26, 46, 37);
            this.trvLichSu.FullRowSelect = true;
            this.trvLichSu.HideSelection = false;
            this.trvLichSu.Indent = 20;
            this.trvLichSu.ItemHeight = 32;
            this.trvLichSu.Name = "trvLichSu";
            this.trvLichSu.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText; // [FIX]
            treeNode1.Text = "PK#024 — 24/03";
            treeNode2.Text = "PK#018 — 12/02";
            treeNode3.Text = "PK#005 — 05/01";
            treeNode4.Text = "2026 (3 lần)";
            treeNode5.Text = "PK#099 — 15/12";
            treeNode6.Text = "PK#088 — 10/11";
            treeNode7.Text = "PK#077 — 05/10";
            treeNode8.Text = "PK#066 — 20/09";
            treeNode9.Text = "PK#055 — 01/08";
            treeNode10.Text = "2025 (5 lần)";
            this.trvLichSu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] { treeNode4, treeNode10 });
            this.trvLichSu.ShowLines = false;
            this.trvLichSu.ShowRootLines = false;
            this.trvLichSu.TabIndex = 5;
            this.trvLichSu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvLichSu_AfterSelect);
            this.trvLichSu.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.trvLichSu_DrawNode); // [FIX]

            // ══ pnlRight ══════════════════════════════════════════════════════
            this.pnlRight.Controls.Add(this.pnlContent);
            this.pnlRight.Controls.Add(this.pnlTabs);
            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(246, 249, 247);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(16, 12, 16, 16);
            this.pnlRight.TabIndex = 1;

            // ── pnlTabs — [FIX] width lớn hơn để chữ in đậm không bị cắt ─
            this.pnlTabs.Controls.Add(this.btnTabThongTin);
            this.pnlTabs.Controls.Add(this.btnTabDonThuoc);
            this.pnlTabs.Controls.Add(this.btnTabHinhAnh);
            this.pnlTabs.Controls.Add(this.btnTabGhiChu);
            this.pnlTabs.BackColor = System.Drawing.Color.Transparent;
            this.pnlTabs.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTabs.Height = 50;
            this.pnlTabs.Name = "pnlTabs";
            this.pnlTabs.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.pnlTabs.TabIndex = 0;

            // btnTabThongTin — [FIX] Width 185
            this.btnTabThongTin.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTabThongTin.Width = 185;
            this.btnTabThongTin.Text = "📋  Thông Tin Khám";
            this.btnTabThongTin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTabThongTin.FillColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.btnTabThongTin.ForeColor = System.Drawing.Color.White;
            this.btnTabThongTin.BorderRadius = 8;
            this.btnTabThongTin.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnTabThongTin.Name = "btnTabThongTin";
            this.btnTabThongTin.TabIndex = 0;
            this.btnTabThongTin.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnTabThongTin.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnTabThongTin.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTabThongTin.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTabThongTin.Click += new System.EventHandler(this.btnTabThongTin_Click);

            // btnTabDonThuoc — [FIX] Width 155
            this.btnTabDonThuoc.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTabDonThuoc.Width = 155;
            this.btnTabDonThuoc.Text = "💊  Đơn Thuốc";
            this.btnTabDonThuoc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTabDonThuoc.FillColor = System.Drawing.Color.Transparent;
            this.btnTabDonThuoc.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.btnTabDonThuoc.BorderRadius = 8;
            this.btnTabDonThuoc.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnTabDonThuoc.Name = "btnTabDonThuoc";
            this.btnTabDonThuoc.TabIndex = 1;
            this.btnTabDonThuoc.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnTabDonThuoc.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnTabDonThuoc.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTabDonThuoc.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTabDonThuoc.Click += new System.EventHandler(this.btnTabDonThuoc_Click);

            // btnTabHinhAnh — [FIX] Width 195
            this.btnTabHinhAnh.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTabHinhAnh.Width = 195;
            this.btnTabHinhAnh.Text = "🖼  Hình Ảnh Bệnh Lý";
            this.btnTabHinhAnh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTabHinhAnh.FillColor = System.Drawing.Color.Transparent;
            this.btnTabHinhAnh.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.btnTabHinhAnh.BorderRadius = 8;
            this.btnTabHinhAnh.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnTabHinhAnh.Name = "btnTabHinhAnh";
            this.btnTabHinhAnh.TabIndex = 2;
            this.btnTabHinhAnh.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnTabHinhAnh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnTabHinhAnh.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTabHinhAnh.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTabHinhAnh.Click += new System.EventHandler(this.btnTabHinhAnh_Click);

            // btnTabGhiChu — [FIX] Width 135
            this.btnTabGhiChu.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTabGhiChu.Width = 135;
            this.btnTabGhiChu.Text = "📝  Ghi Chú";
            this.btnTabGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTabGhiChu.FillColor = System.Drawing.Color.Transparent;
            this.btnTabGhiChu.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.btnTabGhiChu.BorderRadius = 8;
            this.btnTabGhiChu.Margin = new System.Windows.Forms.Padding(0);
            this.btnTabGhiChu.Name = "btnTabGhiChu";
            this.btnTabGhiChu.TabIndex = 3;
            this.btnTabGhiChu.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnTabGhiChu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnTabGhiChu.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTabGhiChu.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTabGhiChu.Click += new System.EventHandler(this.btnTabGhiChu_Click);

            // ── pnlContent ────────────────────────────────────────────────
            this.pnlContent.Controls.Add(this.pnlGhiChuFull);
            this.pnlContent.Controls.Add(this.pnlHinhAnhFull);
            this.pnlContent.Controls.Add(this.pnlDonThuocFull);
            this.pnlContent.Controls.Add(this.pnlThongTinFull);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.BackColor = System.Drawing.Color.Transparent;
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.TabIndex = 1;

            // ══ TAB 1 — pnlThongTinFull ══════════════════════════════════════
            this.pnlThongTinFull.Controls.Add(this.tblContent);
            this.pnlThongTinFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlThongTinFull.BackColor = System.Drawing.Color.Transparent;
            this.pnlThongTinFull.Name = "pnlThongTinFull";
            this.pnlThongTinFull.Visible = true;
            this.pnlThongTinFull.TabIndex = 0;

            // tblContent — 2 rows
            this.tblContent.ColumnCount = 1;
            this.tblContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblContent.Controls.Add(this.tblTop, 0, 0);
            this.tblContent.Controls.Add(this.tblStats, 0, 1);
            this.tblContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblContent.Name = "tblContent";
            this.tblContent.RowCount = 2;
            this.tblContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.tblContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tblContent.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.tblContent.TabIndex = 0;

            // ── tblTop — [FIX] 46% cardExamInfo | 54% grbPrescription ────
            this.tblTop.ColumnCount = 2;
            this.tblTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46F));
            this.tblTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54F));
            this.tblTop.Controls.Add(this.cardExamInfo, 0, 0);
            this.tblTop.Controls.Add(this.grbPrescription, 1, 0);
            this.tblTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblTop.Name = "tblTop";
            this.tblTop.RowCount = 1;
            this.tblTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblTop.TabIndex = 0;

            // ── cardExamInfo ──────────────────────────────────────────────
            this.cardExamInfo.Controls.Add(this.tblExamInfo);
            this.cardExamInfo.FillColor = System.Drawing.Color.White;
            this.cardExamInfo.BorderColor = System.Drawing.Color.FromArgb(168, 230, 207);
            this.cardExamInfo.BorderRadius = 12;
            this.cardExamInfo.CustomBorderThickness = new System.Windows.Forms.Padding(1);
            this.cardExamInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardExamInfo.Margin = new System.Windows.Forms.Padding(0, 0, 8, 8);
            this.cardExamInfo.Name = "cardExamInfo";
            this.cardExamInfo.Padding = new System.Windows.Forms.Padding(16, 14, 16, 14);
            this.cardExamInfo.TabIndex = 0;

            // tblExamInfo — 6 rows
            this.tblExamInfo.ColumnCount = 1;
            this.tblExamInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblExamInfo.Controls.Add(this.lblExamTitle, 0, 0);
            this.tblExamInfo.Controls.Add(this.lblDoctor, 0, 1);
            this.tblExamInfo.Controls.Add(this.lblSymptom, 0, 2);
            this.tblExamInfo.Controls.Add(this.lblDiagnosis, 0, 3);
            this.tblExamInfo.Controls.Add(this.lblReExam, 0, 4);
            this.tblExamInfo.Controls.Add(this.lblStatus, 0, 5);
            this.tblExamInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblExamInfo.Name = "tblExamInfo";
            this.tblExamInfo.RowCount = 6;
            this.tblExamInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblExamInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblExamInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblExamInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblExamInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblExamInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblExamInfo.TabIndex = 0;

            this.lblExamTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExamTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblExamTitle.ForeColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.lblExamTitle.Name = "lblExamTitle";
            this.lblExamTitle.Text = "📋 Phiếu Khám PK#018 — 12/02/2026";
            this.lblExamTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblExamTitle.TabIndex = 0;

            this.lblDoctor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDoctor.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDoctor.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblDoctor.Name = "lblDoctor";
            this.lblDoctor.Text = "Bác sĩ:       BS. Nguyễn Văn Nam";
            this.lblDoctor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDoctor.TabIndex = 1;

            this.lblSymptom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSymptom.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSymptom.ForeColor = System.Drawing.Color.FromArgb(26, 46, 37);
            this.lblSymptom.Name = "lblSymptom";
            this.lblSymptom.Text = "Triệu chứng:  Mụn đỏ vùng trán, ngứa";
            this.lblSymptom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSymptom.TabIndex = 2;

            this.lblDiagnosis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDiagnosis.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDiagnosis.ForeColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.lblDiagnosis.Name = "lblDiagnosis";
            this.lblDiagnosis.Text = "Chẩn đoán:  Viêm da dầu nhẹ";
            this.lblDiagnosis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDiagnosis.TabIndex = 3;

            this.lblReExam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReExam.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblReExam.ForeColor = System.Drawing.Color.FromArgb(26, 46, 37);
            this.lblReExam.Name = "lblReExam";
            this.lblReExam.Text = "Tái khám:    12/03/2026";
            this.lblReExam.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblReExam.TabIndex = 4;

            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Text = "Trạng thái:  ✅ Đã thanh toán";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.TabIndex = 5;

            // ── grbPrescription ──
            this.grbPrescription.Controls.Add(this.dgvPrescription);
            this.grbPrescription.ForeColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.grbPrescription.BorderColor = System.Drawing.Color.White;
            this.grbPrescription.CustomBorderColor = System.Drawing.Color.White;
            this.grbPrescription.BorderRadius = 12;
            this.grbPrescription.BackColor = System.Drawing.Color.White;
            this.grbPrescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbPrescription.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.grbPrescription.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.grbPrescription.Name = "grbPrescription";
            this.grbPrescription.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.grbPrescription.TabIndex = 1;
            this.grbPrescription.Text = "Đơn Thuốc Lần Này";

            // ── dgvPrescription ───────────
            this.dgvPrescription.AlternatingRowsDefaultCellStyle = dgvAltStyle;
            this.dgvPrescription.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvPrescription.DefaultCellStyle = dgvDefaultStyle;
            this.dgvPrescription.AllowUserToAddRows = false;
            this.dgvPrescription.AllowUserToDeleteRows = false;
            this.dgvPrescription.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPrescription.BackgroundColor = System.Drawing.Color.White;
            this.dgvPrescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPrescription.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPrescription.ColumnHeadersHeight = 38;
            this.dgvPrescription.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPrescription.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colThuoc, this.colSoLuong, this.colLieuDung });
            this.dgvPrescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPrescription.EnableHeadersVisualStyles = false;
            this.dgvPrescription.GridColor = System.Drawing.Color.FromArgb(226, 237, 232);
            this.dgvPrescription.Name = "dgvPrescription";
            this.dgvPrescription.ReadOnly = true;
            this.dgvPrescription.RowHeadersVisible = false;
            this.dgvPrescription.RowTemplate.Height = 36;
            this.dgvPrescription.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPrescription.TabIndex = 0;

            this.colThuoc.DataPropertyName = "TenThuoc";
            this.colThuoc.HeaderText = "Thuốc";
            this.colThuoc.Name = "colThuoc";
            this.colThuoc.ReadOnly = true;
            this.colThuoc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colThuoc.FillWeight = 50F;

            this.colSoLuong.DataPropertyName = "SoLuong";
            this.colSoLuong.HeaderText = "SL";
            this.colSoLuong.Name = "colSoLuong";
            this.colSoLuong.ReadOnly = true;
            this.colSoLuong.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSoLuong.FillWeight = 15F;

            this.colLieuDung.DataPropertyName = "LieuDung";
            this.colLieuDung.HeaderText = "Liều dùng";
            this.colLieuDung.Name = "colLieuDung";
            this.colLieuDung.ReadOnly = true;
            this.colLieuDung.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLieuDung.FillWeight = 35F;

            // ── tblStats — [FIX] Padding(0,0,0,30) để label nhích lên ────
            this.tblStats.ColumnCount = 4;
            this.tblStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblStats.Controls.Add(this.cardStat1, 0, 0);
            this.tblStats.Controls.Add(this.cardStat2, 1, 0);
            this.tblStats.Controls.Add(this.cardStat3, 2, 0);
            this.tblStats.Controls.Add(this.cardStat4, 3, 0);
            this.tblStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblStats.Name = "tblStats";
            this.tblStats.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.tblStats.RowCount = 1;
            this.tblStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblStats.TabIndex = 1;

            // cardStat1 — [FIX] Padding bottom 30 → label nhích lên ~30px
            this.cardStat1.Controls.Add(this.lblStatValue1);
            this.cardStat1.Controls.Add(this.lblStatLabel1);
            this.cardStat1.FillColor = System.Drawing.Color.FromArgb(221, 245, 229);
            this.cardStat1.BorderRadius = 12;
            this.cardStat1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardStat1.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.cardStat1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30); // [FIX]
            this.cardStat1.Name = "cardStat1";
            this.cardStat1.TabIndex = 0;

            this.lblStatValue1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatValue1.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblStatValue1.ForeColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.lblStatValue1.BackColor = System.Drawing.Color.Transparent;
            this.lblStatValue1.Name = "lblStatValue1";
            this.lblStatValue1.Text = "8";
            this.lblStatValue1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatValue1.TabIndex = 0;

            this.lblStatLabel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatLabel1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblStatLabel1.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblStatLabel1.BackColor = System.Drawing.Color.Transparent;
            this.lblStatLabel1.Height = 32;
            this.lblStatLabel1.Name = "lblStatLabel1";
            this.lblStatLabel1.Text = "Tổng lần khám";
            this.lblStatLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatLabel1.TabIndex = 1;

            // cardStat2
            this.cardStat2.Controls.Add(this.lblStatValue2);
            this.cardStat2.Controls.Add(this.lblStatLabel2);
            this.cardStat2.FillColor = System.Drawing.Color.FromArgb(221, 245, 229);
            this.cardStat2.BorderRadius = 12;
            this.cardStat2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardStat2.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.cardStat2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30); // [FIX]
            this.cardStat2.Name = "cardStat2";
            this.cardStat2.TabIndex = 1;

            this.lblStatValue2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatValue2.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblStatValue2.ForeColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.lblStatValue2.BackColor = System.Drawing.Color.Transparent;
            this.lblStatValue2.Name = "lblStatValue2";
            this.lblStatValue2.Text = "3";
            this.lblStatValue2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatValue2.TabIndex = 0;

            this.lblStatLabel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatLabel2.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblStatLabel2.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblStatLabel2.BackColor = System.Drawing.Color.Transparent;
            this.lblStatLabel2.Height = 32;
            this.lblStatLabel2.Name = "lblStatLabel2";
            this.lblStatLabel2.Text = "DV hay dùng";
            this.lblStatLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatLabel2.TabIndex = 1;

            // cardStat3
            this.cardStat3.Controls.Add(this.lblStatValue3);
            this.cardStat3.Controls.Add(this.lblStatLabel3);
            this.cardStat3.FillColor = System.Drawing.Color.FromArgb(221, 245, 229);
            this.cardStat3.BorderRadius = 12;
            this.cardStat3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardStat3.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.cardStat3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30); // [FIX]
            this.cardStat3.Name = "cardStat3";
            this.cardStat3.TabIndex = 2;

            this.lblStatValue3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatValue3.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblStatValue3.ForeColor = System.Drawing.Color.FromArgb(15, 92, 77);
            this.lblStatValue3.BackColor = System.Drawing.Color.Transparent;
            this.lblStatValue3.Name = "lblStatValue3";
            this.lblStatValue3.Text = "5";
            this.lblStatValue3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatValue3.TabIndex = 0;

            this.lblStatLabel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatLabel3.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblStatLabel3.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblStatLabel3.BackColor = System.Drawing.Color.Transparent;
            this.lblStatLabel3.Height = 32;
            this.lblStatLabel3.Name = "lblStatLabel3";
            this.lblStatLabel3.Text = "Loại thuốc";
            this.lblStatLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatLabel3.TabIndex = 1;

            // cardStat4
            this.cardStat4.Controls.Add(this.lblStatValue4);
            this.cardStat4.Controls.Add(this.lblStatLabel4);
            this.cardStat4.FillColor = System.Drawing.Color.FromArgb(221, 245, 229);
            this.cardStat4.BorderRadius = 12;
            this.cardStat4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardStat4.Margin = new System.Windows.Forms.Padding(0);
            this.cardStat4.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30); // [FIX]
            this.cardStat4.Name = "cardStat4";
            this.cardStat4.TabIndex = 3;

            this.lblStatValue4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatValue4.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblStatValue4.ForeColor = System.Drawing.Color.FromArgb(184, 138, 40);
            this.lblStatValue4.BackColor = System.Drawing.Color.Transparent;
            this.lblStatValue4.Name = "lblStatValue4";
            this.lblStatValue4.Text = "95%";
            this.lblStatValue4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatValue4.TabIndex = 0;

            this.lblStatLabel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatLabel4.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblStatLabel4.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblStatLabel4.BackColor = System.Drawing.Color.Transparent;
            this.lblStatLabel4.Height = 32;
            this.lblStatLabel4.Name = "lblStatLabel4";
            this.lblStatLabel4.Text = "Hài lòng";
            this.lblStatLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatLabel4.TabIndex = 1;

            // ══ TAB 2 — pnlDonThuocFull ══════════════════════════════════════
            this.pnlDonThuocFull.Controls.Add(this.dgvDonThuocFull);
            this.pnlDonThuocFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDonThuocFull.BackColor = System.Drawing.Color.Transparent;
            this.pnlDonThuocFull.Name = "pnlDonThuocFull";
            this.pnlDonThuocFull.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlDonThuocFull.Visible = false;
            this.pnlDonThuocFull.TabIndex = 1;

            // dgvDonThuocFull — [FIX] header (15,92,77) chữ trắng, SelectionBackColor đồng màu
            this.dgvDonThuocFull.AlternatingRowsDefaultCellStyle = dgvAltStyle;
            this.dgvDonThuocFull.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvDonThuocFull.DefaultCellStyle = dgvDefaultStyle;
            this.dgvDonThuocFull.AllowUserToAddRows = false;
            this.dgvDonThuocFull.AllowUserToDeleteRows = false;
            this.dgvDonThuocFull.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDonThuocFull.BackgroundColor = System.Drawing.Color.White;
            this.dgvDonThuocFull.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDonThuocFull.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDonThuocFull.ColumnHeadersHeight = 38;
            this.dgvDonThuocFull.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDonThuocFull.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colDTF_NgayKham, this.colDTF_TenThuoc, this.colDTF_SoLuong, this.colDTF_LieuDung });
            this.dgvDonThuocFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDonThuocFull.EnableHeadersVisualStyles = false;
            this.dgvDonThuocFull.GridColor = System.Drawing.Color.FromArgb(226, 237, 232);
            this.dgvDonThuocFull.Name = "dgvDonThuocFull";
            this.dgvDonThuocFull.ReadOnly = true;
            this.dgvDonThuocFull.RowHeadersVisible = false;
            this.dgvDonThuocFull.RowTemplate.Height = 36;
            this.dgvDonThuocFull.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDonThuocFull.TabIndex = 0;

            this.colDTF_NgayKham.DataPropertyName = "NgayKham";
            this.colDTF_NgayKham.HeaderText = "Ngày Khám";
            this.colDTF_NgayKham.Name = "colDTF_NgayKham";
            this.colDTF_NgayKham.ReadOnly = true;
            this.colDTF_NgayKham.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDTF_NgayKham.FillWeight = 20F;

            this.colDTF_TenThuoc.DataPropertyName = "TenThuoc";
            this.colDTF_TenThuoc.HeaderText = "Thuốc";
            this.colDTF_TenThuoc.Name = "colDTF_TenThuoc";
            this.colDTF_TenThuoc.ReadOnly = true;
            this.colDTF_TenThuoc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDTF_TenThuoc.FillWeight = 40F;

            this.colDTF_SoLuong.DataPropertyName = "SoLuong";
            this.colDTF_SoLuong.HeaderText = "SL";
            this.colDTF_SoLuong.Name = "colDTF_SoLuong";
            this.colDTF_SoLuong.ReadOnly = true;
            this.colDTF_SoLuong.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDTF_SoLuong.FillWeight = 10F;

            this.colDTF_LieuDung.DataPropertyName = "LieuDung";
            this.colDTF_LieuDung.HeaderText = "Liều dùng";
            this.colDTF_LieuDung.Name = "colDTF_LieuDung";
            this.colDTF_LieuDung.ReadOnly = true;
            this.colDTF_LieuDung.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDTF_LieuDung.FillWeight = 30F;

            // ══ TAB 3 — pnlHinhAnhFull ═══════════════════════════════════════
            this.pnlHinhAnhFull.Controls.Add(this.flpHinhAnh);
            this.pnlHinhAnhFull.Controls.Add(this.lblHinhAnhEmpty);
            this.pnlHinhAnhFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHinhAnhFull.BackColor = System.Drawing.Color.Transparent;
            this.pnlHinhAnhFull.Name = "pnlHinhAnhFull";
            this.pnlHinhAnhFull.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlHinhAnhFull.Visible = false;
            this.pnlHinhAnhFull.TabIndex = 2;

            this.flpHinhAnh.AutoScroll = true;
            this.flpHinhAnh.BackColor = System.Drawing.Color.FromArgb(246, 249, 247);
            this.flpHinhAnh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpHinhAnh.Name = "flpHinhAnh";
            this.flpHinhAnh.Padding = new System.Windows.Forms.Padding(8);
            this.flpHinhAnh.TabIndex = 0;
            this.flpHinhAnh.WrapContents = true;

            this.lblHinhAnhEmpty.BackColor = System.Drawing.Color.Transparent;
            this.lblHinhAnhEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHinhAnhEmpty.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblHinhAnhEmpty.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.lblHinhAnhEmpty.Name = "lblHinhAnhEmpty";
            this.lblHinhAnhEmpty.Text = "🖼  Chưa có hình ảnh bệnh lý";
            this.lblHinhAnhEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHinhAnhEmpty.Visible = false;
            this.lblHinhAnhEmpty.TabIndex = 1;

            // ══ TAB 4 — pnlGhiChuFull ════════════════════════════════════════
            this.pnlGhiChuFull.Controls.Add(this.dgvGhiChu);
            this.pnlGhiChuFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGhiChuFull.BackColor = System.Drawing.Color.Transparent;
            this.pnlGhiChuFull.Name = "pnlGhiChuFull";
            this.pnlGhiChuFull.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlGhiChuFull.Visible = false;
            this.pnlGhiChuFull.TabIndex = 3;

            this.dgvGhiChu.AlternatingRowsDefaultCellStyle = dgvAltStyle;
            this.dgvGhiChu.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvGhiChu.DefaultCellStyle = dgvDefaultStyle;
            this.dgvGhiChu.AllowUserToAddRows = false;
            this.dgvGhiChu.AllowUserToDeleteRows = false;
            this.dgvGhiChu.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGhiChu.BackgroundColor = System.Drawing.Color.White;
            this.dgvGhiChu.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGhiChu.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvGhiChu.ColumnHeadersHeight = 38;
            this.dgvGhiChu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGhiChu.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colGC_NgayKham, this.colGC_GhiChu });
            this.dgvGhiChu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGhiChu.EnableHeadersVisualStyles = false;
            this.dgvGhiChu.GridColor = System.Drawing.Color.FromArgb(226, 237, 232);
            this.dgvGhiChu.Name = "dgvGhiChu";
            this.dgvGhiChu.ReadOnly = true;
            this.dgvGhiChu.RowHeadersVisible = false;
            this.dgvGhiChu.RowTemplate.Height = 36;
            this.dgvGhiChu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGhiChu.TabIndex = 0;

            this.colGC_NgayKham.DataPropertyName = "NgayKham";
            this.colGC_NgayKham.HeaderText = "Ngày Khám";
            this.colGC_NgayKham.Name = "colGC_NgayKham";
            this.colGC_NgayKham.ReadOnly = true;
            this.colGC_NgayKham.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGC_NgayKham.FillWeight = 25F;

            this.colGC_GhiChu.DataPropertyName = "GhiChu";
            this.colGC_GhiChu.HeaderText = "Ghi Chú";
            this.colGC_GhiChu.Name = "colGC_GhiChu";
            this.colGC_GhiChu.ReadOnly = true;
            this.colGC_GhiChu.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGC_GhiChu.FillWeight = 75F;

            // ══ BenhNhanDetailForm ════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(246, 249, 247);
            this.ClientSize = new System.Drawing.Size(1100, 760);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "BenhNhanDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hồ Sơ Bệnh Nhân";

            this.tlpMain.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlAvatarSection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();
            this.pnlBadge.ResumeLayout(false);
            this.pnlTienSu.ResumeLayout(false);
            this.pnlLichSuHeader.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlTabs.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlThongTinFull.ResumeLayout(false);
            this.tblContent.ResumeLayout(false);
            this.tblTop.ResumeLayout(false);
            this.cardExamInfo.ResumeLayout(false);
            this.tblExamInfo.ResumeLayout(false);
            this.grbPrescription.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrescription)).EndInit();
            this.tblStats.ResumeLayout(false);
            this.cardStat1.ResumeLayout(false);
            this.cardStat2.ResumeLayout(false);
            this.cardStat3.ResumeLayout(false);
            this.cardStat4.ResumeLayout(false);
            this.pnlDonThuocFull.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDonThuocFull)).EndInit();
            this.pnlHinhAnhFull.ResumeLayout(false);
            this.pnlGhiChuFull.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGhiChu)).EndInit();
            this.ResumeLayout(false);
        }

        // ── Field declarations ────────────────────────────────────────────────
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlAvatarSection;
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.Label lblTenBN;
        private System.Windows.Forms.Label lblThongTinCo;
        private Guna.UI2.WinForms.Guna2Panel pnlBadge;
        private System.Windows.Forms.Label lblBadgeText;
        private Guna.UI2.WinForms.Guna2Panel pnlTienSu;
        private System.Windows.Forms.Label lblTienSuTitle;
        private System.Windows.Forms.Label lblTienSuND;
        private System.Windows.Forms.Panel pnlLichSuHeader;
        private System.Windows.Forms.Label lblLichSuTitle;
        private System.Windows.Forms.TreeView trvLichSu;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlTabs;
        private Guna.UI2.WinForms.Guna2Button btnTabThongTin;
        private Guna.UI2.WinForms.Guna2Button btnTabDonThuoc;
        private Guna.UI2.WinForms.Guna2Button btnTabHinhAnh;
        private Guna.UI2.WinForms.Guna2Button btnTabGhiChu;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlThongTinFull;
        private System.Windows.Forms.TableLayoutPanel tblContent;
        private System.Windows.Forms.TableLayoutPanel tblTop;
        private Guna.UI2.WinForms.Guna2Panel cardExamInfo;
        private System.Windows.Forms.TableLayoutPanel tblExamInfo;
        private System.Windows.Forms.Label lblExamTitle;
        private System.Windows.Forms.Label lblDoctor;
        private System.Windows.Forms.Label lblSymptom;
        private System.Windows.Forms.Label lblDiagnosis;
        private System.Windows.Forms.Label lblReExam;
        private System.Windows.Forms.Label lblStatus;
        private Guna.UI2.WinForms.Guna2GroupBox grbPrescription;
        private System.Windows.Forms.DataGridView dgvPrescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThuoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLieuDung;
        private System.Windows.Forms.TableLayoutPanel tblStats;
        private Guna.UI2.WinForms.Guna2Panel cardStat1;
        private System.Windows.Forms.Label lblStatValue1;
        private System.Windows.Forms.Label lblStatLabel1;
        private Guna.UI2.WinForms.Guna2Panel cardStat2;
        private System.Windows.Forms.Label lblStatValue2;
        private System.Windows.Forms.Label lblStatLabel2;
        private Guna.UI2.WinForms.Guna2Panel cardStat3;
        private System.Windows.Forms.Label lblStatValue3;
        private System.Windows.Forms.Label lblStatLabel3;
        private Guna.UI2.WinForms.Guna2Panel cardStat4;
        private System.Windows.Forms.Label lblStatValue4;
        private System.Windows.Forms.Label lblStatLabel4;
        private System.Windows.Forms.Panel pnlDonThuocFull;
        private System.Windows.Forms.DataGridView dgvDonThuocFull;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDTF_NgayKham;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDTF_TenThuoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDTF_SoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDTF_LieuDung;
        private System.Windows.Forms.Panel pnlHinhAnhFull;
        private System.Windows.Forms.FlowLayoutPanel flpHinhAnh;
        private System.Windows.Forms.Label lblHinhAnhEmpty;
        private System.Windows.Forms.Panel pnlGhiChuFull;
        private System.Windows.Forms.DataGridView dgvGhiChu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGC_NgayKham;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGC_GhiChu;
    }
}
