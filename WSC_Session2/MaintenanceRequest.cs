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
    public partial class MaintenanceRequest : Form
    {
        int id;
        public MaintenanceRequest(int id)
        {
            this.id = id;
            InitializeComponent();
        }

        void LoadData()
        {
            cbPriority.ValueMember = "ID";
            cbPriority.DisplayMember = "Name";
            var priorities = Global.db.Priorities
                .Select(x => new
                {
                    x.ID,
                    x.Name
                }).ToArray();
            cbPriority.DataSource = priorities;

            var data = Global.db.Assets.FirstOrDefault(x => x.ID == this.id);

            if (data == null)
            {
                Global.ErrorAlert("Invalid data");
                this.Close();
            } else
            {
                txtAssetName.Text = data.AssetName;
                txtAssetSN.Text = data.AssetSN;
                txtDepartmentName.Text = data.DepartmentLocation.Department.Name;
            }
        }
        private void MaintenanceRequest_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtDescription.Text == "" || txtConsiderations.Text == "")
            {
                Global.WarningAlert("Please input all required fields");
            } else
            {
                var request = new EmergencyMaintenance
                {
                    AssetID = id,
                    PriorityID = Convert.ToInt32(cbPriority.SelectedValue),
                    DescriptionEmergency = txtDescription.Text,
                    OtherConsiderations = txtConsiderations.Text,
                    EMReportDate = DateTime.Now.Date
                };
                Global.db.EmergencyMaintenances.Add(request);
                Global.db.SaveChanges();
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
