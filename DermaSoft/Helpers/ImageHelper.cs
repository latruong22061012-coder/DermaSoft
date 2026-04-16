using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DermaSoft.Helpers
{
    /// <summary>
    /// Helper an toàn cho việc load và giải phóng Image trong WinForms.
    /// - Dùng MemoryStream để tránh khóa file (file lock).
    /// - Dispose ảnh cũ trước khi gán ảnh mới vào PictureBox.
    /// </summary>
    internal static class ImageHelper
    {
        /// <summary>
        /// Load ảnh từ file mà không giữ file lock.
        /// Trả về null nếu file không tồn tại hoặc không đọc được.
        /// </summary>
        public static Image LoadImageSafe(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;

            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                var ms = new MemoryStream(bytes);
                return Image.FromStream(ms);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gán ảnh mới vào PictureBox, tự động dispose ảnh cũ.
        /// </summary>
        public static void SetImage(PictureBox pic, Image newImage)
        {
            if (pic == null) return;
            Image old = pic.Image;
            pic.Image = newImage;
            old?.Dispose();
        }

        /// <summary>
        /// Dispose tất cả Image bên trong các control con (PictureBox)
        /// trước khi gọi Controls.Clear().
        /// </summary>
        public static void DisposeControlImages(Control parent)
        {
            if (parent == null) return;
            foreach (Control ctrl in parent.Controls)
            {
                // Dispose hình ảnh trong PictureBox lồng nhau
                DisposeControlImages(ctrl);
                if (ctrl is PictureBox pic && pic.Image != null)
                {
                    pic.Image.Dispose();
                    pic.Image = null;
                }
            }
        }
    }
}
