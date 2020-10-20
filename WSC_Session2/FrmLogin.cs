using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSC_Session2
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                Global.WarningAlert("Please input username or password");
            } else
            {
                var data = Global.db.Employees
                    .FirstOrDefault(x => x.Username == txtUsername.Text && x.Password == txtPassword.Text);

                if (data == null)
                {
                    Global.ErrorAlert("Incorrect username or password");
                } else
                {
                    EmergencyMaintenanceForm form = new EmergencyMaintenanceForm(data);
                    this.Hide();
                    form.ShowDialog();
                    this.Show();
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                }
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
