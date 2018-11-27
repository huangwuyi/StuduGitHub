using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
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

        DataTable dt_OnLine = new DataTable();

        

            DataTable dt_OffLine = new DataTable();

        WaitingForm waitingForm = new WaitingForm();

        private void MainForm_Load(object sender, EventArgs e)
        {
            //DataTable dt = ser_Machine.GetList("").Tables[0];
            backgroundWorker1.RunWorkerAsync();
            waitingForm.ShowDialog();            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add("Ip");
            dt.Columns.Add();
            dt.Columns.Add();

            if (dt.Rows.Count == 0)
            {
                for (int i =100; i < 110; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = i;
                    dr[1] = "192.168.1." + i.ToString();
                    dr[2] = i.ToString() + "设备";
                    dr[3] = i.ToString() + "设备";
                    dt.Rows.Add(dr);
                }
            }

            dt_OnLine.Columns.Add();
            dt_OnLine.Columns.Add("Ip");
            dt_OnLine.Columns.Add();
            dt_OnLine.Columns.Add();

            dt_OffLine.Columns.Add();
            dt_OffLine.Columns.Add("Ip");
            dt_OffLine.Columns.Add();
            dt_OffLine.Columns.Add();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String IP = dt.Rows[i]["Ip"].ToString().Trim();
                if (PingGo(IP))
                {
                    DataRow dr = dt_OnLine.NewRow();
                    dr[0] = dt.Rows[i][0];
                    dr[1] = dt.Rows[i][1];
                    dr[2] = dt.Rows[i][2];
                    dr[3] = dt.Rows[i][3];
                    dt_OnLine.Rows.Add(dr);
                }
                else
                {
                    DataRow dr = dt_OffLine.NewRow();
                    dr[0] = dt.Rows[i][0];
                    dr[1] = dt.Rows[i][1];
                    dr[2] = dt.Rows[i][2];
                    dr[3] = dt.Rows[i][3];
                    dt_OffLine.Rows.Add(dr);
                }
                backgroundWorker1.ReportProgress(i * 100 / dt.Rows.Count, i.ToString() + "/" + dt.Rows.Count);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            waitingForm.setProgressBar(e.ProgressPercentage);
            waitingForm.setProgressLabel(e.UserState.ToString());
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            waitingForm.Close();
            dataGridView1.DataSource = dt_OnLine;
            label3.Text = dt_OnLine.Rows.Count.ToString();
            dataGridView2.DataSource = dt_OffLine;
            label4.Text = dt_OffLine.Rows.Count.ToString();
        }

        private bool PingGo(String IP)
        {
            Ping ping = new Ping();
            PingOptions pingOptions = new PingOptions();
            PingReply pingReply = ping.Send(IP);
            return pingReply.Status == IPStatus.Success;
        }

    }
}
