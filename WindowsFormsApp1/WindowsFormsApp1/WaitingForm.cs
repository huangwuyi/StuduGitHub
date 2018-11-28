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
    public partial class WaitingForm : Form
    {
        public WaitingForm()
        {
            InitializeComponent();
        }

        public void setProgressBar(int value)
        {
            progressBar1.Value = value;
        }

        public void setProgressLabel(string text)
        {
            label1.Text = text;
        }

        private void WaitingForm_Load(object sender, EventArgs e)
        {
            label1.Parent = pictureBox1;
        }
    }
}
