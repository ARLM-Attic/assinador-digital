using System;
using System.Collections.Generic;
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
            if (args.Length > 0)
            {
                if (args[0] == "/v")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmManageDigitalSignature(args[1].Split('|')));
                }
                else if (args[0] == "/r")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmRemoveDigitalSignature(args[1].Split('|')));
                }
                else if (args[0] == "/a")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmAddDigitalSignature(args[1].Split('|')));
                }
            }
        }
    }
}