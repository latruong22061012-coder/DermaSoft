using System.Drawing;

namespace DermaSoft.Theme
{
    internal static class ColorScheme
    {
        // Màu xanh chủ đạo
        public static readonly Color PrimaryDark    = ColorTranslator.FromHtml("#0F5C4D");
        public static readonly Color Primary        = ColorTranslator.FromHtml("#2E8B57");
        public static readonly Color PrimaryMid     = ColorTranslator.FromHtml("#3A9E68");
        public static readonly Color PrimaryLight   = ColorTranslator.FromHtml("#A8E6CF");
        public static readonly Color PrimaryPale    = ColorTranslator.FromHtml("#DDF5EA");

        // Màu vàng gold nhấn
        public static readonly Color Gold           = ColorTranslator.FromHtml("#B88A28");
        public static readonly Color GoldSoft       = ColorTranslator.FromHtml("#D7B66F");
        public static readonly Color GoldChampagne  = ColorTranslator.FromHtml("#F2E4C3");

        // Màu trung tính
        public static readonly Color White          = Color.White;
        public static readonly Color Background     = ColorTranslator.FromHtml("#F6F9F7");
        public static readonly Color Surface        = ColorTranslator.FromHtml("#FFFFFF");
        public static readonly Color Border         = ColorTranslator.FromHtml("#E2EDE8");

        // Màu chữ
        public static readonly Color TextDark       = ColorTranslator.FromHtml("#1A2E25");
        public static readonly Color TextMid        = ColorTranslator.FromHtml("#374B42");
        public static readonly Color TextGray       = ColorTranslator.FromHtml("#6B7280");
        public static readonly Color TextLight      = ColorTranslator.FromHtml("#9CA3AF");

        // Màu trạng thái
        public static readonly Color Success        = ColorTranslator.FromHtml("#22C55E");
        public static readonly Color Warning        = ColorTranslator.FromHtml("#F59E0B");
        public static readonly Color Danger         = ColorTranslator.FromHtml("#EF4444");
        public static readonly Color Info           = ColorTranslator.FromHtml("#3B82F6");

        // Màu thanh điều hướng bên
        public static readonly Color SidebarBg     = ColorTranslator.FromHtml("#0F5C4D");
        public static readonly Color SidebarActive  = ColorTranslator.FromHtml("#1A7A66");
        public static readonly Color SidebarHover   = ColorTranslator.FromHtml("#155C4A");
        public static readonly Color SidebarText    = Color.White;
        public static readonly Color SidebarSubText = ColorTranslator.FromHtml("#A8D5C5");
    }
}
