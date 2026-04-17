using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace DermaSoft.Forms
{
    /// <summary>
    /// In phiếu lương bằng GDI+ — thiết kế đồng bộ với HoaDonPrinter.
    /// Xuất PDF: chọn "Microsoft Print to PDF" trong PrintPreviewDialog.
    /// </summary>
    internal class PhieuLuongPrinter
    {
        // ── Dữ liệu ───────────────────────────────────────────────────────────
        private readonly string _tenPK, _diaChiPK, _sdtPK;
        private readonly string _hoTen, _sdt, _vaiTro;
        private readonly DateTime _thangNam;
        private readonly string _loai;
        private readonly decimal _donGia, _heSoTC;
        private readonly int _soBN, _soBNTC;
        private readonly decimal _soGio, _soGioTC;
        private readonly decimal _luongChinh, _luongTC;
        private readonly decimal _thuong, _khauTru, _tongLuong;
        private readonly string _ghiChu;

        // ── Màu sắc — đồng bộ HoaDonPrinter ──────────────────────────────────
        private static readonly Color CXanh = Color.FromArgb(15, 92, 77);
        private static readonly Color CVang = Color.FromArgb(184, 138, 40);
        private static readonly Color CXanhNhat = Color.FromArgb(221, 245, 229);
        private static readonly Color CNenTK = Color.FromArgb(248, 252, 249);
        private static readonly Color CXam = Color.FromArgb(107, 114, 128);
        private static readonly Color CXamDam = Color.FromArgb(55, 65, 81);
        private static readonly Color CKeBang = Color.FromArgb(220, 220, 220);
        private static readonly Color CSuccess = Color.FromArgb(21, 128, 61);
        private static readonly Color CDanger = Color.FromArgb(220, 38, 38);

        public PhieuLuongPrinter(
            string tenPK, string diaChiPK, string sdtPK,
            string hoTen, string sdt, string vaiTro,
            DateTime thangNam, string loai,
            decimal donGia, decimal heSoTC,
            int soBN, int soBNTC,
            decimal soGio, decimal soGioTC,
            decimal luongChinh, decimal luongTC,
            decimal thuong, decimal khauTru, decimal tongLuong,
            string ghiChu)
        {
            _tenPK = tenPK; _diaChiPK = diaChiPK; _sdtPK = sdtPK;
            _hoTen = hoTen; _sdt = sdt; _vaiTro = vaiTro;
            _thangNam = thangNam; _loai = loai;
            _donGia = donGia; _heSoTC = heSoTC;
            _soBN = soBN; _soBNTC = soBNTC;
            _soGio = soGio; _soGioTC = soGioTC;
            _luongChinh = luongChinh; _luongTC = luongTC;
            _thuong = thuong; _khauTru = khauTru; _tongLuong = tongLuong;
            _ghiChu = ghiChu;
        }

        // ══════════════════════════════════════════════════════════════════════
        // PUBLIC
        // ══════════════════════════════════════════════════════════════════════

        public void MoXemTruoc(IWin32Window owner)
        {
            var ppd = new PrintPreviewDialog
            {
                Document = TaoDoc(),
                Width = 620,
                Height = 950,
                Text = $"Phiếu lương — {_hoTen} — {_thangNam:MM/yyyy}",
                StartPosition = FormStartPosition.CenterParent
            };
            ppd.ShowDialog(owner);
        }

        // ══════════════════════════════════════════════════════════════════════
        // PRINT DOCUMENT
        // ══════════════════════════════════════════════════════════════════════

        private PrintDocument TaoDoc()
        {
            var pd = new PrintDocument();
            pd.DefaultPageSettings.PaperSize = new PaperSize("A5", 583, 827);
            pd.DefaultPageSettings.Margins = new Margins(40, 40, 30, 30);
            pd.PrintPage += VeTrang;
            return pd;
        }

        // ══════════════════════════════════════════════════════════════════════
        // VẼ TOÀN BỘ TRANG
        // ══════════════════════════════════════════════════════════════════════

        private void VeTrang(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int L = e.MarginBounds.Left;
            int R = e.MarginBounds.Right;
            int W = e.MarginBounds.Width;
            int y = e.MarginBounds.Top;

            y = VeHeader(g, L, W, y);
            y = VeSepVang(g, L, W, y);
            y = VeTieuDe(g, L, W, y);
            y = VeThongTinNV(g, L, W, y);
            y += 10;
            y = VeChiTietLuong(g, L, W, y);
            y += 10;
            y = VeTongKet(g, L, W, y);
            VeFooter(g, L, W, e.MarginBounds.Bottom);

            e.HasMorePages = false;
        }

        // ── 1. Header xanh ────────────────────────────────────────────────────
        private int VeHeader(Graphics g, int L, int W, int y)
        {
            int h = 62;
            using (var br = new SolidBrush(CXanh))
                g.FillRectangle(br, L, y, W, h);

            using (var f = new Font("Segoe UI", 13f, FontStyle.Bold))
            using (var br = new SolidBrush(Color.White))
            {
                string t = _tenPK.ToUpper();
                var sz = g.MeasureString(t, f);
                g.DrawString(t, f, br, L + (W - sz.Width) / 2f, y + 6);
            }

            using (var f = new Font("Segoe UI", 7.5f))
            using (var br = new SolidBrush(Color.FromArgb(192, 217, 207)))
            {
                string t = $"{_diaChiPK}  |  ĐT: {_sdtPK}";
                var sz = g.MeasureString(t, f);
                g.DrawString(t, f, br, L + (W - sz.Width) / 2f, y + 34);
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
        private int VeTieuDe(Graphics g, int L, int W, int y)
        {
            y += 12;

            using (var f = new Font("Segoe UI", 15f, FontStyle.Bold))
            using (var br = new SolidBrush(CVang))
            {
                string t = $"PHIẾU LƯƠNG THÁNG {_thangNam:MM/yyyy}";
                var sz = g.MeasureString(t, f);
                g.DrawString(t, f, br, L + (W - sz.Width) / 2f, y);
                y += (int)sz.Height + 2;
            }

            using (var f = new Font("Segoe UI", 8f))
            using (var br = new SolidBrush(CXam))
            {
                string t = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";
                var sz = g.MeasureString(t, f);
                g.DrawString(t, f, br, L + (W - sz.Width) / 2f, y);
                y += (int)sz.Height + 10;
            }

            return y;
        }

        // ── 4. Thông tin nhân viên ────────────────────────────────────────────
        private int VeThongTinNV(Graphics g, int L, int W, int y)
        {
            int h = 56;
            using (var br = new SolidBrush(CXanhNhat))
                g.FillRectangle(br, L, y, W, h);
            using (var pen = new Pen(Color.FromArgb(180, 220, 200)))
                g.DrawRectangle(pen, L, y, W, h);

            string loaiText = _loai == "THEO_BN" ? "Theo bệnh nhân"
                            : _loai == "THEO_GIO" ? "Theo giờ" : "Cố định/tháng";

            float[] ratios = { 0.35f, 0.25f, 0.40f };
            string[] labels = { "Nhân viên", "Vai trò", "Loại tính lương" };
            string[] values = { _hoTen, _vaiTro, loaiText };

            float xPos = L + 10;
            using (var fL = new Font("Segoe UI", 7.5f, FontStyle.Bold))
            using (var fV = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (var brL = new SolidBrush(CXanh))
            using (var brV = new SolidBrush(Color.FromArgb(20, 55, 38)))
            {
                for (int i = 0; i < 3; i++)
                {
                    float colW = W * ratios[i];
                    g.DrawString(labels[i], fL, brL, xPos, y + 6);
                    g.DrawString(values[i], fV, brV, xPos, y + 26);
                    xPos += colW;
                }
            }

            return y + h + 4;
        }

        // ── 5. Chi tiết lương — bảng ──────────────────────────────────────────
        private int VeChiTietLuong(Graphics g, int L, int W, int y)
        {
            // Tiêu đề section
            using (var accent = new SolidBrush(CVang))
                g.FillRectangle(accent, L, y, 4, 24);
            using (var f = new Font("Segoe UI", 9.5f, FontStyle.Bold))
            using (var br = new SolidBrush(CVang))
                g.DrawString("✦  Chi Tiết Tính Lương", f, br, L + 10, y + 3);
            y += 30;

            // Header bảng
            int rowH = 28;
            int cMuc = W / 2;
            int cSoTien = W - cMuc;

            using (var br = new SolidBrush(CXanh))
                g.FillRectangle(br, L, y, W, rowH);
            using (var f = new Font("Segoe UI", 8.5f, FontStyle.Bold))
            using (var brW = new SolidBrush(Color.White))
            {
                g.DrawString("Mục", f, brW, L + 8, y + 6);
                VeTextRight(g, "Số tiền", f, brW, L + W - 8, y + 6);
            }
            y += rowH;

            // Rows
            int rowIdx = 0;

            if (_loai == "THEO_BN")
            {
                y = VeDongBang(g, L, W, rowH, y, ref rowIdx,
                    $"Lương BN trong ca ({_soBN} BN × {FT(_donGia)})", FT(_luongChinh), null);
                y = VeDongBang(g, L, W, rowH, y, ref rowIdx,
                    $"Lương BN tăng ca ({_soBNTC} BN × {FT(_donGia)} × {_heSoTC:0.#})", FT(_luongTC), null);
            }
            else if (_loai == "THEO_GIO")
            {
                y = VeDongBang(g, L, W, rowH, y, ref rowIdx,
                    $"Lương giờ thường ({_soGio:#,##0.#}h × {FT(_donGia)})", FT(_luongChinh), null);
                y = VeDongBang(g, L, W, rowH, y, ref rowIdx,
                    $"Lương giờ tăng ca ({_soGioTC:#,##0.#}h × {FT(_donGia)} × {_heSoTC:0.#})", FT(_luongTC), null);
            }
            else
            {
                y = VeDongBang(g, L, W, rowH, y, ref rowIdx,
                    "Lương cố định/tháng", FT(_luongChinh), null);
            }

            // Separator
            using (var pen = new Pen(CKeBang))
                g.DrawLine(pen, L + 8, y + 2, L + W - 8, y + 2);
            y += 6;

            y = VeDongBang(g, L, W, rowH, y, ref rowIdx,
                "Lương chính", FT(_luongChinh), null);
            y = VeDongBang(g, L, W, rowH, y, ref rowIdx,
                "Lương tăng ca", FT(_luongTC), null);
            y = VeDongBang(g, L, W, rowH, y, ref rowIdx,
                "Thưởng thêm", "+" + FT(_thuong), CSuccess);
            y = VeDongBang(g, L, W, rowH, y, ref rowIdx,
                "Khấu trừ", "-" + FT(_khauTru), CDanger);

            // Viền bảng
            int totalH = y - (rowH + 30) - (y - rowH * rowIdx - 6); // approximate
            using (var pen = new Pen(Color.FromArgb(180, 220, 200)))
                g.DrawRectangle(pen, L, y - rowH * rowIdx - 6 - rowH, W, rowH * rowIdx + 6 + rowH);

            return y;
        }

        private int VeDongBang(Graphics g, int L, int W, int rowH, int y,
            ref int idx, string label, string value, Color? valueColor)
        {
            bool alt = idx % 2 == 1;
            using (var br = new SolidBrush(alt ? Color.FromArgb(245, 250, 247) : Color.White))
                g.FillRectangle(br, L, y, W, rowH);

            Color cVal = valueColor ?? CXamDam;

            using (var f = new Font("Segoe UI", 8.5f))
            using (var brL = new SolidBrush(CXamDam))
            using (var brV = new SolidBrush(cVal))
            {
                g.DrawString(label, f, brL, L + 8, y + 6);
                VeTextRight(g, value, f, brV, L + W - 8, y + 6);
            }

            using (var pen = new Pen(CKeBang, 0.5f))
                g.DrawLine(pen, L, y + rowH, L + W, y + rowH);

            idx++;
            return y + rowH;
        }

        // ── 6. Tổng kết ───────────────────────────────────────────────────────
        private int VeTongKet(Graphics g, int L, int W, int y)
        {
            int boxH = 80;
            using (var br = new SolidBrush(CNenTK))
                g.FillRectangle(br, L, y, W, boxH);
            using (var pen = new Pen(Color.FromArgb(180, 220, 200)))
                g.DrawRectangle(pen, L, y, W, boxH);

            int xL = L + 14;
            int xR = L + W - 14;
            int iy = y + 10;

            // Separator đậm
            using (var pen = new Pen(CVang, 2f))
                g.DrawLine(pen, xL, iy, xR, iy);
            iy += 10;

            // TỔNG LƯƠNG
            using (var f = new Font("Segoe UI", 14f, FontStyle.Bold))
            using (var br = new SolidBrush(CVang))
            {
                g.DrawString("TỔNG LƯƠNG:", f, br, xL, iy);
                VeTextRight(g, FT(_tongLuong), f, br, xR, iy);
            }
            iy += 30;

            // Ghi chú
            if (!string.IsNullOrWhiteSpace(_ghiChu))
            {
                using (var f = new Font("Segoe UI", 7.5f, FontStyle.Italic))
                using (var br = new SolidBrush(CXam))
                    g.DrawString($"Ghi chú: {_ghiChu}", f, br, xL, iy);
            }

            return y + boxH + 14;
        }

        // ── 7. Footer + chữ ký ────────────────────────────────────────────────
        private void VeFooter(Graphics g, int L, int W, int bottom)
        {
            int yF = bottom - 70;

            // Ngày tháng — căn phải
            using (var f = new Font("Segoe UI", 8f))
            using (var br = new SolidBrush(CXam))
            {
                string ngay = $"Ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}";
                VeTextRight(g, ngay, f, br, L + W, yF);
            }
            yF += 20;

            // Chữ ký
            using (var f = new Font("Segoe UI", 8.5f, FontStyle.Bold))
            using (var br = new SolidBrush(CXamDam))
            {
                g.DrawString("Người lập", f, br, L + 40, yF);
                VeTextRight(g, "Người nhận", f, br, L + W - 40, yF);
            }

            // Footer line + text
            yF = bottom - 14;
            using (var pen = new Pen(CKeBang, 0.5f))
                g.DrawLine(pen, L, yF - 4, L + W, yF - 4);

            using (var f = new Font("Segoe UI", 7f, FontStyle.Italic))
            using (var br = new SolidBrush(CXam))
            {
                string t = $"Phiếu lương được xuất tự động bởi DermaSoft  —  {DateTime.Now:dd/MM/yyyy HH:mm}";
                var sz = g.MeasureString(t, f);
                g.DrawString(t, f, br, L + (W - sz.Width) / 2f, yF);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // HELPERS
        // ══════════════════════════════════════════════════════════════════════

        private void VeTextRight(Graphics g, string text, Font f, Brush br, int xRight, int y)
        {
            var sz = g.MeasureString(text, f);
            g.DrawString(text, f, br, xRight - sz.Width, y);
        }

        private string FT(decimal v) => v.ToString("#,##0") + "đ";
    }
}
