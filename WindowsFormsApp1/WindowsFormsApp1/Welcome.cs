using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private void Welcome_Load(object sender, EventArgs e)
        {

        }

        private void Welcome_Shown(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(2000);

                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        Login login = new Login();
                        login.Show();
                        this.TopMost = true;
                        this.Hide();
                        //this.Close();
                    }));
                }
            }));
            thread.Start();
        }
    }
}
