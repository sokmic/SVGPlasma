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
    public partial class ListMaterials : Form
    {
        public ListMaterials()
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = Program.settings.materials;       
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            EditMaterial em = new EditMaterial(new GCodeMaterialSettings());
            em.ShowDialog();
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = Program.settings.materials;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            EditMaterial em = new EditMaterial((GCodeMaterialSettings)dataGridView1.CurrentRow.DataBoundItem);
            em.ShowDialog();
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = Program.settings.materials;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this material?", "Confirm",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                GCodeMaterialSettings gc = (GCodeMaterialSettings)dataGridView1.CurrentRow.DataBoundItem;
                Program.settings.materials.Remove(gc);
                Program.settings.Save();
                this.dataGridView1.DataSource = null;
                this.dataGridView1.DataSource = Program.settings.materials;
            }
        }
    }
}
