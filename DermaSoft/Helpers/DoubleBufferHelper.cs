using System.Reflection;
using System.Windows.Forms;

namespace DermaSoft.Helpers
{
    /// <summary>
    /// Bật DoubleBuffered cho bất kỳ Control nào (kể cả Panel, Form)
    /// thông qua Reflection — vì property DoubleBuffered là protected.
    /// </summary>
    internal static class DoubleBufferHelper
    {
        /// <summary>
        /// Bật DoubleBuffered cho control và tất cả control con (đệ quy).
        /// </summary>
        public static void BatDoubleBuffered(Control control)
        {
            if (control == null) return;

            typeof(Control)
                .GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(control, true, null);

            // Đệ quy cho control con
            foreach (Control child in control.Controls)
                BatDoubleBuffered(child);
        }

        /// <summary>
        /// Nhúng một Form con vào panel container (pnlMdiArea) một cách mượt mà:
        ///   1. SuspendLayout container
        ///   2. Dispose form cũ đúng cách
        ///   3. Ẩn form con trước khi add → tránh flash trắng
        ///   4. Bật DoubleBuffered cho form con
        ///   5. Show + ResumeLayout
        /// </summary>
        public static void NhungFormCon(Panel container, Form frm)
        {
            container.SuspendLayout();

            // Dispose form cũ đúng cách
            while (container.Controls.Count > 0)
            {
                var old = container.Controls[0];
                container.Controls.RemoveAt(0);
                old.Dispose();
            }

            if (frm != null)
            {
                frm.TopLevel = false;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Dock = DockStyle.Fill;
                frm.Visible = false;    // Ẩn trước khi add → tránh flash
                BatDoubleBuffered(frm);
                container.Controls.Add(frm);
                frm.Visible = true;     // Hiện sau khi đã add + layout xong
            }

            container.ResumeLayout(true);
        }
    }
}
