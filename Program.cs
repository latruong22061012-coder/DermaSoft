using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DermaSoft.Forms;
using DermaSoft.Theme;

namespace DermaSoft
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppSettings.Load();

            Application.Run(new LoginForm());
        }
    }
}
