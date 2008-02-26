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
            System.Diagnostics.Debug.Assert(true);

            if (args.Length > 0)
            {
                if (args[0] == "/v")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    string[] paths = args[1].Split('|');
                    if ((paths.Length <= 1)
                        && System.IO.Path.HasExtension(paths[0]))
                        Application.Run(new frmManageDigitalSignature(args[1].Split('|'),false));
                    else
                        Application.Run(new frmIncludeSubFolders(args[1].Split('|'),args[0]));
                    
                }
                else if (args[0] == "/r")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    string[] paths = args[1].Split('|');
                    if ((paths.Length <= 1)
                        && System.IO.Path.HasExtension(paths[0]))
                        Application.Run(new frmSelectDigitalSignatureToRemove(args[1].Split('|'), false));
                    else
                        Application.Run(new frmIncludeSubFolders(args[1].Split('|'), args[0]));
                }
                else if (args[0] == "/a")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmAddDigitalSignature(args[1].Split('|'), true));
                }
            }
        }
    }
}