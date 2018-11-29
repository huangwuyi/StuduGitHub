using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MachineModiForm : Form
    {
        public MachineModiForm()
        {
            InitializeComponent();
        }

        public Service_Machine.Table_Machine t_Machine;

        private void MachineModiForm_Load(object sender, EventArgs e)
        {
            userControl_Machine1.ShowValue(t_Machine);
        }

        private void MachineModiForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
