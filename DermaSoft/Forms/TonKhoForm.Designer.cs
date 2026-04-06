using System.Drawing;
using System.Windows.Forms;

namespace DermaSoft.Forms
{
    partial class TonKhoForm
    {
        private System.ComponentModel.IContainer components = null;

        protected /*override*/ void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            // base.Dispose(disposing); // Removed to fix CS0117
        }

        private void InitializeComponent()
        {
            // Set form size using Size property
            this.Size = new Size(1100, 680);
            this.Name = "TonKhoForm";
            this.Text = "Tồn Kho Theo Lô (FEFO)";
            this.StartPosition = FormStartPosition.CenterParent;
        }
    }
}
