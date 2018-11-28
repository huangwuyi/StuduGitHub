using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class UserControl_Machine : UserControl
    {
        Service_Machine service_Machine = new Service_Machine();
        public UserControl_Machine()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Service_Machine.Table_Machine t_Machine = new Service_Machine.Table_Machine();
            if (!(textBox1.Text.Trim().Length == 0))
            {
                t_Machine = service_Machine.GetModel(int.Parse(textBox1.Text.Trim()));
            }
            t_Machine.Ip = textBox2.Text.Trim();
            t_Machine.MachineName = textBox3.Text.Trim();
            t_Machine.MachineRemark = textBox4.Text.Trim();
            if (service_Machine.Exists(int.Parse(textBox1.Text.Trim())))
            {
                service_Machine.Update(t_Machine);
            }
            else
            {
                service_Machine.Add(t_Machine);
            }
            MessageUtil.MessageInfo("保存成功！");
        }

        public void ShowValue(Service_Machine.Table_Machine t_Machin)
        {
            label1.Visible = true;
            textBox1.Visible = true;
            textBox1.Text = t_Machin.Lsno.ToString();
            textBox2.Text = t_Machin.Ip;
            textBox3.Text = t_Machin.MachineName;
            textBox4.Text = t_Machin.MachineRemark;
        }
    }
}
