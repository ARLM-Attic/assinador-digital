using System;
using System.Windows.Forms;

namespace AssinadorDigital
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            System.Diagnostics.Debug.Assert(true);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormHidden(args));
        }
    }
}