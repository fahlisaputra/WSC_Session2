using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSC_Session2
{
    class Global
    {
        public static Session2Entities db = new Session2Entities();
        public static void InfoAlert(string message)
        {
            MessageBox.Show(message, "EM Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void WarningAlert(string message)
        {
            MessageBox.Show(message, "EM Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static void ErrorAlert(string message)
        {
            MessageBox.Show(message, "EM Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static bool ChoiceAlert(string message)
        {
            if (MessageBox.Show(message, "EM Management", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }
    }
}
