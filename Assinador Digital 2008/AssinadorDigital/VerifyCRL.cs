using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AssinadorDigital
{
    public static class VerifyCRL
    {
        public static void VerifyConsultCRL()
        { 
            RegistryKey ConsultCRL = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\LTIA\Assinador Digital", true);
            if (!(Convert.ToBoolean(ConsultCRL.GetValue("ConsultCRL"))))
                if (MessageBox.Show("A Verificação de Certificados Revogados está desabilitada!\n\n                            " + 
                    "Deseja ativar agora?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    ConsultCRL.SetValue("ConsultCRL", true);
                }
        }
    }
}
