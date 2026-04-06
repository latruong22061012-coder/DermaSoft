using System.Drawing;

namespace DermaSoft.Theme
{
    internal static class AppFonts
    {
        // Tiêu đề
        public static readonly Font H1       = new Font("Segoe UI", 22f, FontStyle.Bold);
        public static readonly Font H2       = new Font("Segoe UI", 16f, FontStyle.Bold);
        public static readonly Font H3       = new Font("Segoe UI", 13f, FontStyle.Bold);
        public static readonly Font H4       = new Font("Segoe UI", 11f, FontStyle.Bold);

        // Nội dung
        public static readonly Font Body     = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        public static readonly Font BodyBold = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        public static readonly Font Small    = new Font("Segoe UI", 8.5f, FontStyle.Regular);
        public static readonly Font Tiny     = new Font("Segoe UI", 8f,   FontStyle.Regular);

        // Thanh điều hướng
        public static readonly Font NavItem  = new Font("Segoe UI", 10f, FontStyle.Regular);
        public static readonly Font NavBold  = new Font("Segoe UI", 10f, FontStyle.Bold);

        // Nhãn & huy hiệu
        public static readonly Font Badge    = new Font("Segoe UI", 8f, FontStyle.Bold);
        public static readonly Font Caption  = new Font("Segoe UI", 8.5f, FontStyle.Regular);
    }
}
