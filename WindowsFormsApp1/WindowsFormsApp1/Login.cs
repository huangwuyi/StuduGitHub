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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length == 0 || textBox2.Text.Trim().Length == 0)
            {
                MessageUtil.MessageError("账户和密码都不能为空！");
                return;
            }
            ExitProgram = false;
            this.Close();
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        private Boolean ExitProgram = true;

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(ExitProgram)
                Application.Exit();
        }
    }
}
