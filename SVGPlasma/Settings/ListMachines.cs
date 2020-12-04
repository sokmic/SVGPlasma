using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVGPlasma
{
    public partial class ListMachines : Form
    {
        public ListMachines()
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = Program.settings.machines;       
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            EditMachine em = new EditMachine(new GCodeMachineSettings());
            em.ShowDialog();
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = Program.settings.machines;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            EditMachine em = new EditMachine((GCodeMachineSettings)dataGridView1.CurrentRow.DataBoundItem);
            em.ShowDialog();
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = Program.settings.machines;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this machine?","Confirm",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                GCodeMachineSettings gc = (GCodeMachineSettings)dataGridView1.CurrentRow.DataBoundItem;
                Program.settings.machines.Remove(gc);
                Program.settings.Save();
                this.dataGridView1.DataSource = null;
                this.dataGridView1.DataSource = Program.settings.machines;
            }
        }
    }
}
