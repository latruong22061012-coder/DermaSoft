using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using DermaSoft.Data;

namespace DermaSoft.Forms
{
    /// <summary>
    /// In đơn thuốc bằng GDI+ — sắc nét, không phụ thuộc UserControl.
    /// Xuất PDF: chọn "Microsoft Print to PDF" trong PrintPreviewDialog.
    /// Lấy thông tin phòng khám ĐỘNG từ bảng ThongTinPhongKham.
    /// </summary>
    internal class DonThuocPrinter
    {
        // ── Dữ liệu ───────────────────────────────────────────────────────────
        private readonly int _maPK;
        private DataTable _dtThuoc;

        // Thông tin phòng khám (lấy từ DB)
        private string _tenPhongKham;
        private string _diaChiPK;
        private string _hotlinePK;

        // Thông tin bệnh nhân & bác sĩ (lấy từ DB)
        private string _tenBN;
        private string _sdtBN;
        private string _tuoiGioiTinh;
        private string _tenBacSi;
        private string _chanDoan;
        private string _ngayKham;

        // ── Màu sắc ───────────────────────────────────────────────────────────
        private static readonly Color CXanh = Color.FromArgb(15, 92, 77);
        private static readonly Color CVang = Color.FromArgb(184, 138, 40);
        private static readonly Color CXanhNhat = Color.FromArgb(221, 245, 229);
        private static readonly Color CXam = Color.FromArgb(107, 114, 128);
        private static readonly Color CXamDam = Color.FromArgb(55, 65, 81);
        private static readonly Color CKeBang = Color.FromArgb(220, 220, 220);

        public DonThuocPrinter(int maPhieuKham)
        {
            _maPK = maPhieuKham;
        }

        // ══════════════════════════════════════════════════════════════════════
        // PUBLIC
        // ══════════════════════════════════════════════════════════════════════

        public void MoXemTruoc(IWin32Window owner)
        {
            TaiDuLieu();
            var ppd = new PrintPreviewDialog
            {
                Document = TaoDoc(),
                Width = 820,
                Height = 1100,
                Text = $"Xem Trước Đơn Thuốc — {_tenBN}",
                StartPosition = FormStartPosition.CenterParent
            };
            ppd.ShowDialog(owner);
        }

        public void In(IWin32Window owner)
        {
            TaiDuLieu();
            var pd = TaoDoc();
            using (var dlg = new PrintDialog { Document = pd, UseEXDialog = true })
                if (dlg.ShowDialog() == DialogResult.OK)
                    pd.Print();
        }

        // ══════════════════════════════════════════════════════════════════════
        // LOAD DỮ LIỆU
        // ══════════════════════════════════════════════════════════════════════

        private void TaiDuLieu()
        {
            // 1. Thông tin phòng khám
            _tenPhongKham = "DermaSoft Clinic";
            _diaChiPK = "";
            _hotlinePK = "";
            try
            {
                DataTable dtPK = DatabaseConnection.ExecuteQuery(
                    "SELECT TOP 1 TenPhongKham, DiaChi, SoDienThoai " +
                    "FROM ThongTinPhongKham ORDER BY MaThongTin DESC");
                if (dtPK != null && dtPK.Rows.Count > 0)
                {
                    _tenPhongKham = dtPK.Rows[0]["TenPhongKham"]?.ToString() ?? _tenPhongKham;
                    _diaChiPK = dtPK.Rows[0]["DiaChi"]?.ToString() ?? "";
                    _hotlinePK = dtPK.Rows[0]["SoDienThoai"]?.ToString() ?? "";
                }
            }
            catch { }

            // 2. Thông tin bệnh nhân, bác sĩ, chẩn đoán
            _tenBN = "—";
            _sdtBN = "";
            _tuoiGioiTinh = "";
            _tenBacSi = "—";
            _chanDoan = "";
            _ngayKham = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            try
            {
                DataTable dtInfo = DatabaseConnection.ExecuteQuery(@"
                    SELECT bn.HoTen AS TenBN, bn.SoDienThoai AS SDT,
                           bn.NgaySinh, bn.GioiTinh,
                           nd.HoTen AS TenBS, pk.ChanDoan, pk.NgayKham
                    FROM PhieuKham pk
                    LEFT JOIN BenhNhan bn ON pk.MaBenhNhan = bn.MaBenhNhan
                    LEFT JOIN NguoiDung nd ON pk.MaBacSi = nd.MaNguoiDung
                    WHERE pk.MaPhieuKham = @MaPK",
                    p => p.AddWithValue("@MaPK", _maPK));

                if (dtInfo != null && dtInfo.Rows.Count > 0)
                {
                    DataRow r = dtInfo.Rows[0];
                    _tenBN = r["TenBN"]?.ToString() ?? "—";
                    _sdtBN = r["SDT"]?.ToString() ?? "";
                    _tenBacSi = r["TenBS"]?.ToString() ?? "—";
                    _chanDoan = r["ChanDoan"]?.ToString() ?? "";

                    if (r["NgayKham"] != DBNull.Value)
                        _ngayKham = Convert.ToDateTime(r["NgayKham"]).ToString("dd/MM/yyyy HH:mm");

                    // Tính tuổi + giới tính
                    string tuoi = "";
                    if (r["NgaySinh"] != DBNull.Value)
                    {
                        var ns = Convert.ToDateTime(r["NgaySinh"]);
                        int age = DateTime.Today.Year - ns.Year;
                        if (DateTime.Today < ns.AddYears(age)) age--;
                        tuoi = age + " tuổi";
                    }
                    string gt = r["GioiTinh"] != DBNull.Value
                        ? (Convert.ToBoolean(r["GioiTinh"]) ? "Nam" : "Nữ") : "";
                    _tuoiGioiTinh = string.Join(" · ",
                        new[] { gt, tuoi }).Trim(' ', '·');
                }
            }
            catch { }

            // 3. Danh sách thuốc
            _dtThuoc = DatabaseConnection.ExecuteQuery(@"
                SELECT t.TenThuoc, t.DonViTinh, cdt.SoLuong,
                       t.DonGia,
                       (cdt.SoLuong * t.DonGia) AS ThanhTien,
                       ISNULL(cdt.LieuDung, N'') AS LieuDung
                FROM ChiTietDonThuoc cdt
                JOIN Thuoc t ON cdt.MaThuoc = t.MaThuoc
                WHERE cdt.MaPhieuKham = @MaPK
                ORDER BY t.TenThuoc",
                p => p.AddWithValue("@MaPK", _maPK));
        }

        // ══════════════════════════════════════════════════════════════════════
        // PRINT DOCUMENT
        // ══════════════════════════════════════════════════════════════════════

        private PrintDocument TaoDoc()
        {
            var pd = new PrintDocument();
            pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
            pd.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
            pd.PrintPage += VeTrang;
            return pd;
        }

        // ══════════════════════════════════════════════════════════════════════
        // VẼ TOÀN BỘ TRANG
        // ══════════════════════════════════════════════════════════════════════

        private void VeTrang(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int L = e.MarginBounds.Left;
            int R = e.MarginBounds.Right;
            int W = e.MarginBounds.Width;
            int y = e.MarginBounds.Top;

            y = VeHeader(g, L, R, W, y);
            y = VeSepVang(g, L, W, y);
            y = VeTieuDe(g, L, R, W, y);
            y = VeBNInfo(g, L, W, y);
            y += 14;
            y = VeBangThuoc(g, L, W, y);
            y += 10;
            y = VeTongTien(g, L, W, y);
            y += 20;
            y = VeLuuY(g, L, W, y);
            VeChuKy(g, L, W, y);
            VeFooter(g, L, W, e.MarginBounds.Bottom);

            e.HasMorePages = false;
        }

        // ── 1. Header xanh ────────────────────────────────────────────────────
        private int VeHeader(Graphics g, int L, int R, int W, int y)
        {
            int h = 72;
            using (var br = new SolidBrush(CXanh))
                g.FillRectangle(br, L, y, W, h);

            using (var f = new Font("Segoe UI", 14f, FontStyle.Bold))
            using (var br = new SolidBrush(Color.White))
            {
                string t = _tenPhongKham;
                var sz = g.MeasureString(t, f);
                g.DrawString(t, f, br, L + (W - sz.Width) / 2f, y + 8);
            }

            using (var f = new Font("Segoe UI", 8.5f))
            using (var br = new SolidBrush(Color.FromArgb(192, 217, 207)))
            {
                string dc = _diaChiPK;
                string hl = string.IsNullOrWhiteSpace(_hotlinePK) ? "" : $"SĐT: {_hotlinePK}";
                string t = string.Join("  |  ",
                    new[] { dc, hl }).Trim(' ', '|');
                if (string.IsNullOrWhiteSpace(t)) t = "Phòng khám chuyên da liễu";
                var sz = g.MeasureString(t, f);
                g.DrawString(t, f, br, L + (W - sz.Width) / 2f, y + 40);
            }

            return y + h;
        }

        // ── 2. Separator vàng ─────────────────────────────────────────────────
        private int VeSepVang(Graphics g, int L, int W, int y)
        {
            using (var br = new SolidBrush(CVang))
                g.FillRectangle(br, L, y, W, 3);
            return y + 3;
        }

        // ── 3. Tiêu đề ────────────────────────────────────────────────────────
        private int VeTieuDe(Graphics g, int L, int R, int W, int y)
        {
            y += 14;

            using (var f = new Font("Segoe UI", 17f, FontStyle.Bold))
            using (var br = new SolidBrush(CXanh))
            {
                string t = "ĐƠN THUỐC";
                var sz = g.MeasureString(t, f);
                g.DrawString(t, f, br, L + (W - sz.Width) / 2f, y);
                y += (int)sz.Height + 4;
            }

            using (var f = new Font("Segoe UI", 8.5f))
            using (var br = new SolidBrush(CXam))
            {
                string t = $"Số PK: #{_maPK:D4}   |   Ngày kê: {DateTime.Now:dd/MM/yyyy HH:mm}";
                var sz = g.MeasureString(t, f);
                g.DrawString(t, f, br, L + (W - sz.Width) / 2f, y);
                y += (int)sz.Height + 14;
            }

            return y;
        }

        // ── 4. Thông tin BN ───────────────────────────────────────────────────
        private int VeBNInfo(Graphics g, int L, int W, int y)
        {
            int h = 72;

            using (var br = new SolidBrush(CXanhNhat))
                g.FillRectangle(br, L, y, W, h);
            using (var pen = new Pen(Color.FromArgb(180, 220, 200)))
                g.DrawRectangle(pen, L, y, W, h);

            // Hàng 1: Bệnh nhân | SĐT | Tuổi/GT
            // Hàng 2: Bác sĩ | Ngày khám | Chẩn đoán
            float[] ratios = { 0.35f, 0.22f, 0.43f };
            string[] labels1 = { "Bệnh nhân", "SĐT", "Giới tính / Tuổi" };
            string[] values1 = { _tenBN, string.IsNullOrWhiteSpace(_sdtBN) ? "—" : _sdtBN,
                                  string.IsNullOrWhiteSpace(_tuoiGioiTinh) ? "—" : _tuoiGioiTinh };

            string[] labels2 = { "Bác sĩ", "Ngày khám", "Chẩn đoán" };
            string[] values2 = { "BS. " + _tenBacSi, _ngayKham,
                                  string.IsNullOrWhiteSpace(_chanDoan) ? "—" : _chanDoan };

            using (var fLabel = new Font("Segoe UI", 7.5f, FontStyle.Bold))
            using (var fValue = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (var brLabel = new SolidBrush(CXanh))
            using (var brValue = new SolidBrush(Color.FromArgb(20, 55, 38)))
            {
                // Hàng 1
                float xPos = L + 10;
                for (int i = 0; i < 3; i++)
                {
                    float colW = W * ratios[i];
                    g.DrawString(labels1[i], fLabel, brLabel, xPos, y + 4);
                    string val = TruncateString(g, values1[i] ?? "—", fValue, colW - 8);
                    g.DrawString(val, fValue, brValue, xPos, y + 18);
                    xPos += colW;
                }

                // Hàng 2
                xPos = L + 10;
                for (int i = 0; i < 3; i++)
                {
                    float colW = W * ratios[i];
                    g.DrawString(labels2[i], fLabel, brLabel, xPos, y + 40);
                    string val = TruncateString(g, values2[i] ?? "—", fValue, colW - 8);
                    g.DrawString(val, fValue, brValue, xPos, y + 54);
                    xPos += colW;
                }
            }

            return y + h + 4;
        }

        // ── 5. Bảng thuốc ─────────────────────────────────────────────────────
        private int VeBangThuoc(Graphics g, int L, int W, int y)
        {
            // Tiêu đề mục
            using (var accent = new SolidBrush(CVang))
                g.FillRectangle(accent, L, y, 4, 26);

            using (var f = new Font("Segoe UI", 9.5f, FontStyle.Bold))
            using (var br = new SolidBrush(CVang))
                g.DrawString("✦  Chi Tiết Thuốc", f, br, L + 10, y + 3);

            y += 30;

            if (_dtThuoc == null || _dtThuoc.Rows.Count == 0)
            {
                using (var f = new Font("Segoe UI", 8.5f, FontStyle.Italic))
                using (var br = new SolidBrush(CXam))
                    g.DrawString("  (Chưa kê thuốc)", f, br, L, y);
                return y + 20;
            }

            // Cột: STT(35) | Tên+Liều(Fill) | SL(45) | Đơn giá(100) | Thành tiền(110)
            int cSTT = 35;
            int cSL = 45;
            int cDG = 100;
            int cTT = 110;
            int cTen = W - cSTT - cSL - cDG - cTT;

            int rowHeader = 28;

            // Header
            using (var br = new SolidBrush(CXanh))
                g.FillRectangle(br, L, y, W, rowHeader);

            using (var f = new Font("Segoe UI", 8.5f, FontStyle.Bold))
            using (var brW = new SolidBrush(Color.White))
            {
                VeTextCenter(g, "STT", f, brW, L + cSTT / 2f, y + 7);
                g.DrawString("Tên thuốc / Liều dùng", f, brW, L + cSTT + 8, y + 7);
                VeTextCenter(g, "SL", f, brW, L + cSTT + cTen + cSL / 2f, y + 7);
                VeTextRight(g, "Đơn giá", f, brW, L + cSTT + cTen + cSL + cDG - 4, y + 7);
                VeTextRight(g, "Thành tiền", f, brW, L + W - 4, y + 7);
            }
            y += rowHeader;

            // Rows
            int yStart = y;
            bool alt = false;
            decimal tong = 0;
            int stt = 1;

            using (var fRow = new Font("Segoe UI", 9f))
            using (var fLieu = new Font("Segoe UI", 8f, FontStyle.Italic))
            {
                foreach (DataRow row in _dtThuoc.Rows)
                {
                    string ten = row["TenThuoc"]?.ToString() ?? "";
                    string donVi = row["DonViTinh"]?.ToString() ?? "";
                    int sl = Convert.ToInt32(row["SoLuong"] ?? 0);
                    decimal dg = Convert.ToDecimal(row["DonGia"] ?? 0);
                    decimal tt = Convert.ToDecimal(row["ThanhTien"] ?? 0);
                    string lieu = row["LieuDung"]?.ToString() ?? "";

                    // Dynamic row height dựa trên có liều dùng hay không
                    int rowH = string.IsNullOrWhiteSpace(lieu) ? 28 : 44;

                    using (var br = new SolidBrush(alt ? Color.FromArgb(245, 250, 247) : Color.White))
                        g.FillRectangle(br, L, y, W, rowH);

                    using (var br = new SolidBrush(Color.FromArgb(33, 33, 33)))
                    {
                        // STT
                        VeTextCenter(g, stt.ToString(), fRow, br, L + cSTT / 2f, y + 6);

                        // Tên thuốc (đậm) + đơn vị
                        string tenHienThi = string.IsNullOrWhiteSpace(donVi)
                            ? ten : $"{ten}  ({donVi})";
                        tenHienThi = TruncateString(g, tenHienThi, fRow, cTen - 12);
                        using (var fBold = new Font("Segoe UI", 9f, FontStyle.Bold))
                            g.DrawString(tenHienThi, fBold, br, L + cSTT + 8, y + 5);

                        // Liều dùng (dòng 2)
                        if (!string.IsNullOrWhiteSpace(lieu))
                        {
                            string lieuHT = TruncateString(g, "► " + lieu, fLieu, cTen - 12);
                            using (var brL = new SolidBrush(Color.FromArgb(107, 76, 20)))
                                g.DrawString(lieuHT, fLieu, brL, L + cSTT + 8, y + 22);
                        }

                        // SL
                        VeTextCenter(g, sl.ToString(), fRow, br, L + cSTT + cTen + cSL / 2f, y + 6);

                        // Đơn giá
                        VeTextRight(g, FT(dg), fRow, br, L + cSTT + cTen + cSL + cDG - 4, y + 6);

                        // Thành tiền (đậm + vàng)
                        using (var fBold = new Font("Segoe UI", 9f, FontStyle.Bold))
                        using (var brGold = new SolidBrush(CVang))
                            VeTextRight(g, FT(tt), fBold, brGold, L + W - 4, y + 6);
                    }

                    // Kẻ ngang
                    using (var pen = new Pen(CKeBang, 0.5f))
                        g.DrawLine(pen, L, y + rowH, L + W, y + rowH);

                    y += rowH;
                    alt = !alt;
                    tong += tt;
                    stt++;
                }
            }

            // Viền bảng
            using (var pen = new Pen(Color.FromArgb(180, 220, 200)))
                g.DrawRectangle(pen, L, yStart - rowHeader, W, y - yStart + rowHeader);

            return y;
        }

        // ── 6. Tổng tiền ──────────────────────────────────────────────────────
        private int VeTongTien(Graphics g, int L, int W, int y)
        {
            if (_dtThuoc == null || _dtThuoc.Rows.Count == 0) return y;

            decimal tong = 0;
            foreach (DataRow r in _dtThuoc.Rows)
                tong += Convert.ToDecimal(r["ThanhTien"] ?? 0);

            int boxH = 46;
            using (var br = new SolidBrush(Color.FromArgb(248, 252, 249)))
                g.FillRectangle(br, L, y, W, boxH);
            using (var pen = new Pen(Color.FromArgb(180, 220, 200)))
                g.DrawRectangle(pen, L, y, W, boxH);

            using (var f = new Font("Segoe UI", 13f, FontStyle.Bold))
            using (var brX = new SolidBrush(CXanh))
            {
                g.DrawString("TỔNG TIỀN THUỐC:", f, brX, L + 14, y + 12);
                VeTextRight(g, FT(tong), f, brX, L + W - 14, y + 12);
            }

            return y + boxH;
        }

        // ── 7. Lưu ý ──────────────────────────────────────────────────────────
        private int VeLuuY(Graphics g, int L, int W, int y)
        {
            int h = 90;
            using (var br = new SolidBrush(Color.FromArgb(255, 251, 235)))
                g.FillRectangle(br, L, y, W, h);
            using (var pen = new Pen(Color.FromArgb(234, 179, 8)))
                g.DrawRectangle(pen, L, y, W, h);

            using (var f = new Font("Segoe UI", 8.5f, FontStyle.Bold))
            using (var br = new SolidBrush(Color.FromArgb(146, 64, 14)))
                g.DrawString("⚠ LƯU Ý", f, br, L + 10, y + 8);

            using (var f = new Font("Segoe UI", 8.5f))
            using (var br = new SolidBrush(CXamDam))
            {
                string[] luuY = new[]
                {
                    "• Uống thuốc đúng liều, đúng giờ theo chỉ dẫn của bác sĩ",
                    "• Tái khám ngay nếu có triệu chứng bất thường",
                    "• Bảo quản thuốc ở nơi khô ráo, tránh ánh nắng trực tiếp",
                    string.IsNullOrWhiteSpace(_hotlinePK)
                        ? "• Liên hệ phòng khám nếu cần tư vấn"
                        : $"• Liên hệ hotline {_hotlinePK} nếu cần tư vấn",
                };
                int yL = y + 26;
                foreach (string s in luuY)
                {
                    g.DrawString(s, f, br, L + 14, yL);
                    yL += 15;
                }
            }

            return y + h;
        }

        // ── 8. Chữ ký bác sĩ ──────────────────────────────────────────────────
        private void VeChuKy(Graphics g, int L, int W, int y)
        {
            y += 20;
            int xSig = L + W - 220;

            using (var f = new Font("Segoe UI", 8.5f, FontStyle.Italic))
            using (var br = new SolidBrush(CXam))
                g.DrawString($"Ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}",
                    f, br, xSig, y);

            using (var f = new Font("Segoe UI", 9.5f, FontStyle.Bold))
            using (var br = new SolidBrush(CXanh))
                g.DrawString("BÁC SĨ ĐIỀU TRỊ", f, br, xSig + 30, y + 18);

            using (var f = new Font("Segoe UI", 8f, FontStyle.Italic))
            using (var br = new SolidBrush(CXam))
                g.DrawString("(Ký, ghi rõ họ tên)", f, br, xSig + 40, y + 34);

            using (var f = new Font("Segoe UI", 10f, FontStyle.Bold))
            using (var br = new SolidBrush(Color.FromArgb(20, 55, 38)))
                g.DrawString("BS. " + _tenBacSi, f, br, xSig + 30, y + 80);
        }

        // ── 9. Footer ─────────────────────────────────────────────────────────
        private void VeFooter(Graphics g, int L, int W, int bottom)
        {
            int yF = bottom - 28;
            using (var pen = new Pen(CKeBang, 0.5f))
                g.DrawLine(pen, L, yF, L + W, yF);
            yF += 6;

            string t = $"Cảm ơn quý khách đã tin tưởng {_tenPhongKham}!  —  Xuất ngày {DateTime.Now:dd/MM/yyyy HH:mm}";
            using (var f = new Font("Segoe UI", 7.5f, FontStyle.Italic))
            using (var br = new SolidBrush(CXam))
            {
                var sz = g.MeasureString(t, f);
                g.DrawString(t, f, br, L + (W - sz.Width) / 2f, yF);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // HELPERS
        // ══════════════════════════════════════════════════════════════════════

        private void VeTextRight(Graphics g, string text, Font f, Brush br, float xRight, float y)
        {
            var sz = g.MeasureString(text, f);
            g.DrawString(text, f, br, xRight - sz.Width, y);
        }

        private void VeTextCenter(Graphics g, string text, Font f, Brush br, float xCenter, float y)
        {
            var sz = g.MeasureString(text, f);
            g.DrawString(text, f, br, xCenter - sz.Width / 2f, y);
        }

        private string TruncateString(Graphics g, string text, Font f, float maxW)
        {
            if (string.IsNullOrEmpty(text)) return text;
            if (g.MeasureString(text, f).Width <= maxW) return text;
            while (text.Length > 1 && g.MeasureString(text + "...", f).Width > maxW)
                text = text.Substring(0, text.Length - 1);
            return text + "...";
        }

        private string FT(decimal v) => v.ToString("#,##0") + "đ";
    }
}
