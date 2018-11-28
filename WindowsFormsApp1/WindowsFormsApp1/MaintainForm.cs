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
    public partial class MaintainForm : Form
    {
        Service_Machine service_Machine = new Service_Machine();
        public MaintainForm()
        {
            InitializeComponent();
        }

        private void MaintainForm_Load(object sender, EventArgs e)
        {
            refresh_data();
        }

        private void refresh_data()
        {
            DataTable dataTable = service_Machine.GetList("").Tables[0];
            if (dataTable.Rows.Count == 0)
            {
                for (int i = 0; i < 30; i++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = i;
                    dataRow[1] = "192.168.1." + i;
                    dataRow[2] = "设备" + i;
                    dataRow[3] = "设备" + i;
                    dataTable.Rows.Add(dataRow);
                }
            }
            dataGridView1.DataSource = dataTable;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("确定要删除选中的设备？","确认",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)==DialogResult.OK)
            {
                String ip = dataGridView1.CurrentRow.Cells[1].Value.ToString().Trim();
                service_Machine.Delete(ip);
                MessageUtil.MessageInfo("删除成功！");
                refresh_data();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}
