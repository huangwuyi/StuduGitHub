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
    public partial class HomeMaintain : Form
    {
        public HomeMaintain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //
            if (textBox1.Text.Trim().Length == 0)
            {
                MessageBox.Show("机房名称不能为空!");
                return;
            }
            Service_Home.Table_Home table_Home = new Service_Home.Table_Home();
            table_Home.name = textBox1.Text.Trim();
            Service_Home service_Home = new Service_Home();
            if (service_Home.Exists(table_Home.name))
            {
                MessageBox.Show("此机房已存在!");
                return;
            }
            service_Home.Add(table_Home);
            MessageBox.Show("添加机房成功!");
        }
    }
}
