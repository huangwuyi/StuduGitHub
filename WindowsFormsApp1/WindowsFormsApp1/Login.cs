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
        Service_User ser_User = new Service_User();
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
            String UserName = textBox1.Text.Trim();
            String Password = textBox2.Text.Trim();

            if (!ser_User.Exists(UserName))
            {
                MessageUtil.MessageError("账户不存在！");
                return;
            }

            Service_User.Table_UserInfo tui = new Service_User.Table_UserInfo();
            tui = ser_User.GetModelByKey(UserName);

            if (!(tui.Password == Password))
            {
                MessageUtil.MessageError("密码不正确！");
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
