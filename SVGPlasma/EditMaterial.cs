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
    public partial class EditMaterial : Form
    {
        GCodeMaterialSettings materialSettings;
        public EditMaterial()
        {
            InitializeComponent();
        }

        public EditMaterial(GCodeMaterialSettings gCodeMaterial) : this()
        {
            materialSettings = gCodeMaterial;
            LoadSettings();
        }

        public void LoadSettings()
        {
            txtName.Text = materialSettings.MaterialName;
            txtPierceTime.Value = materialSettings.PierceTime;
            txtFeedRate.Value = materialSettings.FeedRate;            
        }

        public void SaveSettings()
        {
            materialSettings.MaterialName = txtName.Text;
            materialSettings.PierceTime = txtPierceTime.Value;
            materialSettings.FeedRate = txtFeedRate.Value;            
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            int i = Program.settings.materials.IndexOf(materialSettings);
            SaveSettings();
            if (i >= 0)
            {
                Program.settings.materials[i] = materialSettings;
            }
            else
            {
                Program.settings.materials.Add(materialSettings);
            }
            Program.settings.Save();
            this.Close();
        }
    }
}
