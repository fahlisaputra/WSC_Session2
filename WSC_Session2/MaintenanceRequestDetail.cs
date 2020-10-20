using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSC_Session2
{
    public partial class MaintenanceRequestDetail : Form
    {
        int id;
        bool changed = false;
        List<ChangedPart> changedParts = new List<ChangedPart>();

        List<ChangedPart> removeParts = new List<ChangedPart>();
        public MaintenanceRequestDetail(int id)
        {
            this.id = id;
            InitializeComponent();
        }

        void LoadData()
        {
            cbParts.DisplayMember = "Name";
            cbParts.ValueMember = "ID";
            var partname = Global.db.Parts
                .Select(x => new
                {
                    x.ID,
                    x.Name
                }).ToArray();
            cbParts.DataSource = partname;

            var data = Global.db.EmergencyMaintenances
                .FirstOrDefault(x => x.ID == this.id);

            if (data == null)
            {
                Global.ErrorAlert("Invalid data");
                this.Close();
            }
            else
            {
                txtAssetSN.Text = data.Asset.AssetSN;
                txtAssetName.Text = data.Asset.AssetName;
                txtDepartmentName.Text = data.Asset.DepartmentLocation.Department.Name;

                if (data.EMStartDate != null)
                {
                    startDate.Value = data.EMStartDate.Value;
                }

                if (data.EMEndDate != null)
                {
                    completeDate.Value = data.EMEndDate.Value;
                }

                if (data.EMTechnicianNote != null)
                {
                    txtNote.Text = data.EMTechnicianNote;
                }

                var parts = Global.db.ChangedParts
                    .ToArray()
                    .Where(x => x.EmergencyMaintenanceID == this.id)
                    .Select(i => new
                    {
                        i.ID,
                        i.Part.Name,
                        i.Amount
                    }).ToArray();  

                foreach(var item in parts)
                {
                    var changed = Global.db.ChangedParts.Find(item.ID);
                    changedParts.Add(changed);
                }    
             
                dgv.DataSource = parts;
                //dgv.Columns[0].Visible = false;
                dgv.Columns[1].HeaderText = "Part Name";
                dgv.Columns[2].HeaderText = "Amount";

                var btn = new DataGridViewButtonColumn
                {
                    Name = "Remove",
                    UseColumnTextForButtonValue = true,
                    HeaderText = "Action",
                    Text = "Remove"
                };

                dgv.Columns.Add(btn);

            }
        }

        void LoadChangedPart()
        {
            dgv.Columns.Clear();
            dgv.DataSource = null;
            var parts = changedParts
                 .Select(x => new
                 {
                     x.ID,
                     x.Part.Name,
                     x.Amount
                 }).ToArray();

            dgv.DataSource = parts;
            //dgv.Columns[0].Visible = true;
            dgv.Columns[1].HeaderText = "Part Name";
            dgv.Columns[2].HeaderText = "Amount";

            var btn = new DataGridViewButtonColumn
            {
                Name = "Remove",
                UseColumnTextForButtonValue = true,
                Text = "Remove",
                HeaderText = "Action"
            };

            dgv.Columns.AddRange(btn);
        }
        private void MaintenanceRequestDetail_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int amount = Convert.ToInt32(txtAmount.Text);
                int partID = Convert.ToInt32(cbParts.SelectedValue);
                var exist = changedParts.FirstOrDefault(x => x.PartID == partID);
                var part = Global.db.Parts.Find(partID);

                if (exist == null)
                {
                    exist = new ChangedPart();
                    exist.EmergencyMaintenanceID = this.id;
                    exist.PartID = partID;
                    exist.Part = part;
                    exist.Amount = amount;
                    changedParts.Add(exist);

                }
                else
                {
                    int index = changedParts.IndexOf(exist);
                    changedParts[index].Amount = exist.Amount + amount;
                }

                LoadChangedPart();
            }
            catch(Exception ex)
            {
                Global.WarningAlert("Please input a valid amount");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var data = Global.db.EmergencyMaintenances.Find(this.id);
            if (data == null)
            {
                Global.WarningAlert("Invalid data");
                this.Close();
            } else
            {
                data.EMStartDate = startDate.Value;
                data.EMTechnicianNote = txtNote.Text;
                if (changed)
                {
                    data.EMEndDate = completeDate.Value;
                }
                Global.db.SaveChanges();

                foreach(var item in changedParts)
                {
                    var exist = Global.db.ChangedParts.FirstOrDefault(x => x.ID == item.ID);
                    if (exist == null)
                    {
                        Global.db.ChangedParts.Add(item);
                        Global.db.SaveChanges();
                    }        
                }

                foreach(var item in removeParts)
                {
                    var exist = Global.db.ChangedParts.FirstOrDefault(x => x.ID == item.ID);
                    if (exist != null)
                    {
                        Global.db.ChangedParts.Remove(exist);
                        Global.db.SaveChanges();
                    }
                }
                this.Close();
            }
        }

        private void completeDate_ValueChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (dgv.Columns[e.ColumnIndex].Name == "Remove")
            {

                int id = int.Parse(dgv[0, e.RowIndex].Value.ToString());
                var item = changedParts.FirstOrDefault(x => x.ID == id);
                changedParts.Remove(item);
                dgv.DataSource = null;
                removeParts.Add(item);
                LoadChangedPart();

            }
        }
    }
}
