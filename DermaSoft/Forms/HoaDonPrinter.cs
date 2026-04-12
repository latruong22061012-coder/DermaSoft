using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using DermaSoft.Data;

namespace DermaSoft.Forms
{
    /// <summary>
    /// In hóa đơn bằng GDI+ — sắc nét, không phụ thuộc vào UserControl.
    /// Xuất PDF: chọn "Microsoft Print to PDF" trong PrintPreviewDialog.
    /// </summary>
    internal class HoaDonPrinter
    {
        // ── Dữ liệu ───────────────────────────────────────────────────────────
        private readonly int _maPK;
        private readonly string _tenBN;
        private readonly string _ngayKham;
        private readonly string _tenBacSi;
        private readonly string _chanDoan;
        private readonly decimal _tongDV;
        private readonly decimal _tongThuoc;
        private readonly decimal _giamGia;
        private readonly decimal _tongTien;
        private readonly decimal _tienKhach;
        private readonly decimal _tienThua;
        private readonly string _phuongThuc;
        private DataTable _dtDV;
        private DataTable _dtThuoc;

        // ── Màu sắc ───────────────────────────────────────────────────────────
        private static readonly Color CXanh = Color.FromArgb(15, 92, 77);
        private static readonly Color CVang = Color.FromArgb(184, 138, 40);
        private static readonly Color CXanhNhat = Color.FromArgb(221, 245, 229);
        private static readonly Color CNenTK = Color.FromArgb(248, 252, 249);
        private static readonly Color CXam = Color.FromArgb(107, 114, 128);
        private static readonly Color CXamDam = Color.FromArgb(55, 65, 81);
        private static readonly Color CKeBang = Color.FromArgb(220, 220, 220);

        public HoaDonPrinter(
            int maPK, string tenBN, string ngayKham,
            string tenBacSi, string chanDoan,
            decimal tongDV, decimal tongThuoc, decimal giamGia,
            decimal tongTien, decimal tienKhach, decimal tienThua,
            string phuongThuc)
        {
            _maPK = maPK;
            _tenBN = tenBN;
            _ngayKham = ngayKham;
            _tenBacSi = tenBacSi;
            _chanDoan = chanDoan;
            _tongDV = tongDV;
            _tongThuoc = tongThuoc;
            _giamGia = giamGia;
            _tongTien = tongTien;
            _tienKhach = tienKhach;
            _tienThua = tienThua;
            _phuongThuc = phuongThuc;
        }

        // ══════════════════════════════════════════════════════════════════════
        // PUBLIC
        // ══════════════════════════════════════════════════════════════════════

        public void MoXemTruoc(IWin32Window owner)
        {
            TaiChiTiet();
            var ppd = new PrintPreviewDialog
            {
                Document = TaoDoc(),
                Width = 820,
                Height = 1100,
                Text = $"Xem Trước Hóa Đơn — {_tenBN}",
                StartPosition = FormStartPosition.CenterParent
            };
            ppd.ShowDialog(owner);
        }

        public void In(IWin32Window owner)
        {
            TaiChiTiet();
            var pd = TaoDoc();
            using (var dlg = new PrintDialog { Document = pd, UseEXDialog = true })
                if (dlg.ShowDialog() == DialogResult.OK)
                    pd.Print();
        }

        // ══════════════════════════════════════════════════════════════════════
        // LOAD DỮ LIỆU
        // ══════════════════════════════════════════════════════════════════════

        private void TaiChiTiet()
        {
            _dtDV = DatabaseConnection.ExecuteQuery(@"
                SELECT dv.TenDichVu, ctdv.SoLuong,
                       dv.DonGia      AS DonGiaRaw,
                       ctdv.ThanhTien AS ThanhTienRaw
                FROM ChiTietDichVu ctdv
                JOIN DichVu dv ON ctdv.MaDichVu = dv.MaDichVu
                WHERE ctdv.MaPhieuKham = @MaPK",
                p => p.AddWithValue("@MaPK", _maPK));

            _dtThuoc = DatabaseConnection.ExecuteQuery(@"
                SELECT t.TenThuoc, cdt.SoLuong,
                       t.DonGia              AS DonGiaRaw,
                       cdt.SoLuong * t.DonGia AS ThanhTienRaw
                FROM ChiTietDonThuoc cdt
                JOIN Thuoc t ON cdt.MaThuoc = t.MaThuoc
                WHERE cdt.MaPhieuKham = @MaPK",
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
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            int L = e.MarginBounds.Left;    // Left margin
            int R = e.MarginBounds.Right;   // Right margin
            int W = e.MarginBounds.Width;   // Nội dung rộng
            int y = e.MarginBounds.Top;

            y = VeHeader(g, L, R, W, y);
            y = VeSepVang(g, L, W, y);
            y = VeTieuDe(g, L, R, W, y);
            y = VeBNInfo(g, L, W, y);
            y += 14;
            y = VeBang(g, L, W, y, "✦  Chi Tiết Dịch Vụ", _dtDV, CXanh);
            y += 10;
            if (_dtThuoc != null && _dtThuoc.Rows.Count > 0)
            {
                y = VeBang(g, L, W, y, "✦  Chi Tiết Thuốc", _dtThuoc, CVang);
                y += 10;
            }
            y = VeTongKet(g, L, W, y);
            VeFooter(g, L, W, e.MarginBounds.Bottom);

            e.HasMorePages = false;
        }

        // ── 1. Header xanh ────────────────────────────────────────────────────
        private int VeHeader(Graphics g, int L, int R, int W, int y)
        {
            int h = 72;
            using (var br = new SolidBrush(CXanh))
                g.FillRectangle(br, L, y, W, h);

            // Tên clinic
            using (var f = new Font("Segoe UI", 14f, FontStyle.Bold))
            using (var br = new SolidBrush(Color.White))
            {
                string t = "DermaSoft Clinic";
                var sz = g.MeasureString(t, f);
                g.DrawString(t, f, br, L + (W - sz.Width) / 2f, y + 8);
            }

            // Địa chỉ
            using (var f = new Font("Segoe UI", 8.5f))
            using (var br = new SolidBrush(Color.FromArgb(192, 217, 207)))
            {
                string t = "123 Nguyễn Hữu Cảnh, Q.Bình Thạnh  |  SĐT: 0909123456";
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

            // "HÓA ĐƠN THANH TOÁN"
            using (var f = new Font("Segoe UI", 17f, FontStyle.Bold))
            using (var br = new SolidBrush(CXanh))
            {
                string t = "HÓA ĐƠN THANH TOÁN";
                var sz = g.MeasureString(t, f);
                g.DrawString(t, f, br, L + (W - sz.Width) / 2f, y);
                y += (int)sz.Height + 4;
            }

            // Số PK + ngày
            using (var f = new Font("Segoe UI", 8.5f))
            using (var br = new SolidBrush(CXam))
            {
                string t = $"Số PK: #{_maPK:D4}   |   Ngày: {DateTime.Now:dd/MM/yyyy HH:mm}";
                var sz = g.MeasureString(t, f);
                g.DrawString(t, f, br, L + (W - sz.Width) / 2f, y);
                y += (int)sz.Height + 14;
            }

            return y;
        }

        // ── 4. Thông tin BN ───────────────────────────────────────────────────
        private int VeBNInfo(Graphics g, int L, int W, int y)
        {
            int h = 64;

            // Nền xanh nhạt
            using (var br = new SolidBrush(CXanhNhat))
                g.FillRectangle(br, L, y, W, h);
            using (var pen = new Pen(Color.FromArgb(180, 220, 200)))
                g.DrawRectangle(pen, L, y, W, h);

            // 4 cột: 27% | 18% | 18% | 37%
            float[] ratios = { 0.27f, 0.18f, 0.18f, 0.37f };
            string[] labels = { "Bệnh nhân", "Ngày khám", "Bác sĩ", "Chẩn đoán" };
            string[] values = { _tenBN, _ngayKham, _tenBacSi, _chanDoan ?? "—" };

            float xPos = L + 10;
            using (var fLabel = new Font("Segoe UI", 8f, FontStyle.Bold))
            using (var fValue = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (var brLabel = new SolidBrush(CXanh))
            using (var brValue = new SolidBrush(Color.FromArgb(20, 55, 38)))
            {
                for (int i = 0; i < 4; i++)
                {
                    float colW = W * ratios[i];

                    // Label nhỏ màu xanh
                    g.DrawString(labels[i], fLabel, brLabel, xPos, y + 8);

                    // Value đậm
                    string val = values[i] ?? "—";
                    // Cắt ngắn nếu quá dài (đặc biệt cột Chẩn đoán)
                    val = TruncateString(g, val, fValue, colW - 8);
                    g.DrawString(val, fValue, brValue, xPos, y + 32);

                    xPos += colW;
                }
            }

            return y + h + 4;
        }

        // ── 5. Bảng dịch vụ / thuốc ──────────────────────────────────────────
        private int VeBang(Graphics g, int L, int W, int y,
                           string tieuDe, DataTable dt, Color mauAccent)
        {
            // Tiêu đề mục
            using (var accent = new SolidBrush(mauAccent))
                g.FillRectangle(accent, L, y, 4, 26);

            using (var f = new Font("Segoe UI", 9.5f, FontStyle.Bold))
            using (var br = new SolidBrush(mauAccent))
                g.DrawString(tieuDe, f, br, L + 10, y + 3);

            y += 30;

            if (dt == null || dt.Rows.Count == 0)
            {
                using (var f = new Font("Segoe UI", 8.5f, FontStyle.Italic))
                using (var br = new SolidBrush(CXam))
                    g.DrawString("  (Không có dữ liệu)", f, br, L, y);
                return y + 20;
            }

            // Độ rộng 4 cột: Tên(Fill) | SL(50) | Đơn giá(110) | Thành tiền(120)
            int cSL = 50;
            int cDG = 115;
            int cTT = 120;
            int cTen = W - cSL - cDG - cTT;

            int rowH = 28;

            // Header
            using (var br = new SolidBrush(CXanh))
                g.FillRectangle(br, L, y, W, rowH);

            using (var f = new Font("Segoe UI", 8.5f, FontStyle.Bold))
            using (var brW = new SolidBrush(Color.White))
            {
                g.DrawString("Tên", f, brW, L + 8, y + 6);
                g.DrawString("SL", f, brW, L + cTen + 10, y + 6);
                VeTextRight(g, "Đơn giá", f, brW, L + cTen + cSL + cDG - 4, y + 6);
                VeTextRight(g, "Thành tiền", f, brW, L + W - 4, y + 6);
            }
            y += rowH;

            // Rows
            bool alt = false;
            using (var fRow = new Font("Segoe UI", 8.5f))
            {
                foreach (DataRow row in dt.Rows)
                {
                    using (var br = new SolidBrush(alt ? Color.FromArgb(245, 250, 247) : Color.White))
                        g.FillRectangle(br, L, y, W, rowH);

                    string ten = TruncateString(g, row[0]?.ToString() ?? "", fRow, cTen - 12);
                    string sl = row[1]?.ToString() ?? "";
                    decimal dg = Convert.ToDecimal(row[2]);
                    decimal tt = Convert.ToDecimal(row[3]);

                    using (var br = new SolidBrush(Color.FromArgb(33, 33, 33)))
                    {
                        g.DrawString(ten, fRow, br, L + 8, y + 6);
                        g.DrawString(sl, fRow, br, L + cTen + 10, y + 6);
                        VeTextRight(g, FT(dg), fRow, br, L + cTen + cSL + cDG - 4, y + 6);
                        VeTextRight(g, FT(tt), fRow, br, L + W - 4, y + 6);
                    }

                    // Kẻ ngang
                    using (var pen = new Pen(CKeBang, 0.5f))
                        g.DrawLine(pen, L, y + rowH, L + W, y + rowH);

                    y += rowH;
                    alt = !alt;
                }
            }

            // Viền bảng
            using (var pen = new Pen(Color.FromArgb(180, 220, 200)))
                g.DrawRectangle(pen, L, y - rowH * (dt.Rows.Count + 1),
                    W, rowH * (dt.Rows.Count + 1));

            return y;
        }

        // ── 6. Tổng kết ───────────────────────────────────────────────────────
        private int VeTongKet(Graphics g, int L, int W, int y)
        {
            int boxH = 160;
            using (var br = new SolidBrush(CNenTK))
                g.FillRectangle(br, L, y, W, boxH);
            using (var pen = new Pen(Color.FromArgb(180, 220, 200)))
                g.DrawRectangle(pen, L, y, W, boxH);

            y += 10;
            int xL = L + 14;
            int xR = L + W - 14;

            using (var f = new Font("Segoe UI", 9f))
            using (var fBold = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (var brN = new SolidBrush(CXamDam))
            using (var brX = new SolidBrush(CXanh))
            {
                VeDoiLabel(g, f, brN, brN, xL, xR, y, "Tổng dịch vụ:", FT(_tongDV)); y += 24;
                VeDoiLabel(g, f, brN, brN, xL, xR, y, "Tổng thuốc:", FT(_tongThuoc)); y += 24;
                VeDoiLabel(g, f, brN, brN, xL, xR, y, "Tạm tính:", FT(_tongDV + _tongThuoc)); y += 24;
                VeDoiLabel(g, f, brN, brN, xL, xR, y, "Giảm giá:", FT(_giamGia)); y += 26;

                // Separator đậm
                using (var pen = new Pen(CXanh, 1.5f))
                    g.DrawLine(pen, xL, y, xR, y);
                y += 8;

                // TỔNG CỘNG — to nhất
                using (var fTong = new Font("Segoe UI", 12f, FontStyle.Bold))
                {
                    g.DrawString("TỔNG CỘNG:", fTong, brX, xL, y);
                    VeTextRight(g, FT(_tongTien), fTong, brX, xR, y);
                }
                y += 28;

                // Thanh toán
                using (var fSm = new Font("Segoe UI", 8f))
                using (var brG = new SolidBrush(CXam))
                {
                    g.DrawString($"Phương thức: {_phuongThuc}", fSm, brG, xL, y);
                    VeTextRight(g, $"Khách trả: {FT(_tienKhach)}   |   Tiền thừa: {FT(_tienThua)}",
                        fSm, brG, xR, y);
                }
            }

            return y + 26;
        }

        // ── 7. Footer ─────────────────────────────────────────────────────────
        private void VeFooter(Graphics g, int L, int W, int bottom)
        {
            int yF = bottom - 28;
            using (var pen = new Pen(CKeBang, 0.5f))
                g.DrawLine(pen, L, yF, L + W, yF);
            yF += 6;

            string t = $"Cảm ơn quý khách đã tin tưởng DermaSoft Clinic!  —  Xuất ngày {DateTime.Now:dd/MM/yyyy HH:mm}";
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

        private void VeDoiLabel(Graphics g, Font f,
            Brush brLabel, Brush brValue,
            int xL, int xR, int y, string label, string value)
        {
            g.DrawString(label, f, brLabel, xL, y);
            VeTextRight(g, value, f, brValue, xR, y);
        }

        private void VeTextRight(Graphics g, string text, Font f, Brush br, int xRight, int y)
        {
            var sz = g.MeasureString(text, f);
            g.DrawString(text, f, br, xRight - sz.Width, y);
        }

        private string TruncateString(Graphics g, string text, Font f, float maxW)
        {
            if (g.MeasureString(text, f).Width <= maxW) return text;
            while (text.Length > 1 && g.MeasureString(text + "...", f).Width > maxW)
                text = text.Substring(0, text.Length - 1);
            return text + "...";
        }

        private string FT(decimal v) => v.ToString("#,##0") + "đ";
    }
}