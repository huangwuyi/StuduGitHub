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
    public partial class MainForm : Form
    {
        Service_Machine ser_Machine = new Service_Machine();
        int on_line = 0, off_line = 0;
        public MainForm()
        {
            InitializeComponent();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DataTable dt = ser_Machine.GetList("").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {

            }
        }


    }
}
