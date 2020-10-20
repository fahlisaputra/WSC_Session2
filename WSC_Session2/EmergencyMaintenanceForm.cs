using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSC_Session2
{
    public partial class EmergencyMaintenanceForm : Form
    {
        Employee employee;
        public EmergencyMaintenanceForm(Employee employee)
        {
            this.employee = employee;
            InitializeComponent();
        }

        void LoadData()
        {
            dgv.DataSource = null;
            if (employee.isAdmin == true)
            {
                // ADMIN SECTION
                label2.Text = "List of Assets Requesting EM :";
                button1.Text = "Manage Request";

                var assets = Global.db.EmergencyMaintenances
                    .ToArray()
                    .Select(x => new
                    {
                        x.ID,
                        x.Asset.AssetSN,
                        x.Asset.AssetName,
                        x.EMReportDate,
                        name = x.Asset.Employee.FirstName + " " + x.Asset.Employee.LastName,
                        x.Asset.DepartmentLocation.Department.Name
                    }).ToArray();

                dgv.DataSource = assets;

                dgv.Columns[0].Visible = false;
                dgv.Columns[1].HeaderText = "Asset SN";
                dgv.Columns[2].HeaderText = "Asset Name";
                dgv.Columns[3].HeaderText = "Request Date";
                dgv.Columns[4].HeaderText = "Employee Full Name";
                dgv.Columns[5].HeaderText = "Department";

            } else
            {
                // ACCOUNTABLE PARTY SECTION
                label2.Text = "Available Assets :";
                button1.Text = "Send Emergency Maintenance Request";

                var assets = Global.db.Assets
                    .ToArray()
                    .Select(a => new
                    {
                        a.ID,
                        a.AssetSN,
                        a.AssetName,
                        date = a.EmergencyMaintenances.Where(j => j.AssetID == a.ID).Select(o => o.EMEndDate).DefaultIfEmpty().Last(),
                        count = a.EmergencyMaintenances.Count(i => i.AssetID == a.ID)
                    }).ToArray();

                dgv.DataSource = assets;

                dgv.Columns[0].Visible = false;
                dgv.Columns[1].HeaderText = "Asset SN";
                dgv.Columns[2].HeaderText = "Asset Name";
                dgv.Columns[3].HeaderText = "Last Closed EM";
                dgv.Columns[4].HeaderText = "Number of EMs";

                foreach(DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells[3].Value == null)
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                        row.DefaultCellStyle.ForeColor = Color.White;
                    }
                }
            }
        }
        private void EmergencyMaintenanceForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (employee.isAdmin == false)
            {
                foreach(DataGridViewRow row in dgv.SelectedRows)
                {
                    MaintenanceRequest form = new MaintenanceRequest(Convert.ToInt32(row.Cells[0].Value.ToString()));
                    form.ShowDialog();
                    LoadData();
                }
            } else
            {
                foreach(DataGridViewRow row in dgv.SelectedRows)
                {
                    MaintenanceRequestDetail form = new MaintenanceRequestDetail(Convert.ToInt32(row.Cells[0].Value.ToString()));
                    form.ShowDialog();
                    LoadData();
                }
            }
        }

        private void EmergencyMaintenanceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
