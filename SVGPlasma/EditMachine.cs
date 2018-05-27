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
    public partial class EditMachine : Form
    {
        GCodeMachineSettings machineSettings;
        public EditMachine()
        {
            InitializeComponent();
        }

        public EditMachine(GCodeMachineSettings gCodeMachine) : this()
        {
            machineSettings = gCodeMachine;
            LoadSettings();
        }

        public void LoadSettings()
        {
            txtName.Text = machineSettings.MachineName;
            txtBeginCode.Text = machineSettings.BeginCode;
            txtEndCode.Text = machineSettings.EndCode;
            txtPlasmaOnCode.Text = machineSettings.SpindleOnCode;
            txtPlasmaOffCode.Text = machineSettings.SpindleOffCode;
            txtCutWidth.Value = machineSettings.CutWidth;
        }

        public void SaveSettings()
        {
            machineSettings.MachineName = txtName.Text;
            machineSettings.BeginCode = txtBeginCode.Text;
            machineSettings.EndCode = txtEndCode.Text;
            machineSettings.SpindleOnCode = txtPlasmaOnCode.Text;
            machineSettings.SpindleOffCode = txtPlasmaOffCode.Text;
            machineSettings.CutWidth = txtCutWidth.Value;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            int i = Program.settings.machines.IndexOf(machineSettings);
            SaveSettings();
            if (i >= 0)
            {
                Program.settings.machines[i] = machineSettings;
            }
            else
            {
                Program.settings.machines.Add(machineSettings);
            }
            Program.settings.Save();
            this.Close();
        }
    }
}
