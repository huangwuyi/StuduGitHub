using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MainFormHome : Form
    {
        Service_Machine ser_Machine = new Service_Machine();
        int on_line = 0, off_line = 0;
        public MainFormHome()
        {
            InitializeComponent();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
            Application.Exit();
        }
        DataTable dt = new DataTable();
        DataTable dt_OnLine = new DataTable();
        DataTable dt_OffLine = new DataTable();
        WaitingForm waitingForm = new WaitingForm();
        int global_i = 0;
        private Object lockSomeThing = new object();
        Stopwatch sw = new Stopwatch();

        private void MainForm_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = Global.t_UserInfo.UerName;
            bindHome(); comboBox1.SelectedIndex = 0;
        }

        private void initialized2222()
        {
            //bindHome();
            Func<String> func = new Func<string>(()=> {
                return comboBox1.SelectedValue.ToString().Trim(); });
            dt = ser_Machine.GetList(" ip in (select ip from table_home_machine where home='"
                + Invoke(func).ToString().Trim()+ "')").Tables[0];
            dt.Columns.Add("State");

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("请维护机房机器列表！");
                MaintainForm mf = new MaintainForm();
                mf.Show();
                return;
                dt.Columns.Add();
                dt.Columns.Add("Ip");
                dt.Columns.Add();
                dt.Columns.Add();
                for (int i = 0; i < 10; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = i;
                    //dr[1] = "192.168.1." + i.ToString();
                    dr[1] = "10.2.42." + i.ToString();
                    dr[2] = i.ToString() + "设备";
                    dr[3] = i.ToString() + "设备";
                    dt.Rows.Add(dr);
                }
            }
            Console.WriteLine(dt.Rows.Count);
            dt_OffLine.PrimaryKey = null;
            dt_OnLine.PrimaryKey = null;
            dt_OffLine.Columns.Clear();
            dt_OnLine.Columns.Clear();
            dt_OffLine.Rows.Clear();
            dt_OnLine.Rows.Clear();

            dt_OnLine.Columns.Add("序号");
            dt_OnLine.Columns.Add("Ip");
            dt_OnLine.Columns.Add("机器名称");
            dt_OnLine.Columns.Add("机器备注");
            dt_OnLine.PrimaryKey = new DataColumn[] { dt_OnLine.Columns["Ip"] };

            dt_OffLine.Columns.Add("序号");
            dt_OffLine.Columns.Add("Ip");
            dt_OffLine.Columns.Add("机器名称");
            dt_OffLine.Columns.Add("机器备注");
            dt_OffLine.PrimaryKey = new DataColumn[] { dt_OffLine.Columns["Ip"] };
        }
        private void bindHome()
        {
            Service_Home service_Home = new Service_Home();
            DataTable dt = service_Home.GetList("").Tables[0];
            comboBox1.DataSource = dt;
            comboBox1.ValueMember = dt.Columns[1].ToString().Trim();
            comboBox1.DisplayMember = dt.Columns[1].ToString().Trim();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            refresh_MachineState();
        }

        private void refresh_MachineState()
        {
            initialized2222();
            global_i = 0;

            dt_OnLine.Rows.Clear();
            dt_OffLine.Rows.Clear();

            int core_count = Environment.ProcessorCount;
            core_count = 4;
            int fourthAverage = (int)Math.Ceiling((decimal)((decimal)dt.Rows.Count / (decimal)core_count));

            //for (int i = 0; i < core_count; i++)
            //{
            //    Thread thread = new Thread(new ParameterizedThreadStart(ThreadStartFun));
            //    thread.Name = "thread"+ i;
            //    thread.Start(i * fourthAverage);

            //    Task task = Task.Factory.StartNew(new Action<Object>(ThreadStartFun), null);
            //    task.Start();
            //}
            Thread thread0 = new Thread(new ParameterizedThreadStart(ThreadStartFun));
            thread0.Name = "thread0";
            thread0.Start(0 * fourthAverage);

            Thread thread1 = new Thread(new ParameterizedThreadStart(ThreadStartFun));
            thread1.Name = "thread1";
            thread1.Start(1 * fourthAverage);

            Thread thread2 = new Thread(new ParameterizedThreadStart(ThreadStartFun));
            thread2.Name = "thread2";
            thread2.Start(2 * fourthAverage);

            Thread thread3 = new Thread(new ParameterizedThreadStart(ThreadStartFun));
            thread3.Name = "thread3";
            thread3.Start(3 * fourthAverage);

            thread0.Join();
            thread1.Join();
            thread2.Join();
            thread3.Join();

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    String IP = dt.Rows[i]["Ip"].ToString().Trim();
            //    if (PingGo(IP))
            //    {
            //        DataRow dr = dt_OnLine.NewRow();
            //        dr[0] = dt.Rows[i][0];
            //        dr[1] = dt.Rows[i][1];
            //        dr[2] = dt.Rows[i][2];
            //        dr[3] = dt.Rows[i][3];
            //        dt_OnLine.Rows.Add(dr);
            //    }
            //    else
            //    {
            //        DataRow dr = dt_OffLine.NewRow();
            //        dr[0] = dt.Rows[i][0];
            //        dr[1] = dt.Rows[i][1];
            //        dr[2] = dt.Rows[i][2];
            //        dr[3] = dt.Rows[i][3];
            //        dt_OffLine.Rows.Add(dr);
            //    }
            //    backgroundWorker1.ReportProgress(i * 100 / dt.Rows.Count, i.ToString() + "/" + dt.Rows.Count);
            //}
        }

        private void ThreadStartFun(Object p)
        {
            int fourthAverage = (int)Math.Ceiling((decimal)((decimal)dt.Rows.Count / (decimal)4));
            //dt.Rows.Count / 4;
            for (int i = (int)p; i < Math.Min(dt.Rows.Count, (int)p + fourthAverage); i++)
            {
                String IP = dt.Rows[i]["Ip"].ToString().Trim();
                if (PingGo(IP))
                {
                    DataRow dr = dt_OnLine.NewRow();
                    dr[0] = dt.Rows[i][0];
                    dr[1] = dt.Rows[i][1];
                    dr[2] = dt.Rows[i][2];
                    dr[3] = dt.Rows[i][3];
                    lock (dt_OnLine)
                    {
                        if (dt_OnLine.Rows.Contains(dr[1])) { }
                        else
                        {
                            dt_OnLine.Rows.Add(dr);
                        }
                    }
                    dt.Rows[i]["State"] = 1;
                }
                else
                {
                    DataRow dr = dt_OffLine.NewRow();
                    dr[0] = dt.Rows[i][0];
                    dr[1] = dt.Rows[i][1];
                    dr[2] = dt.Rows[i][2];
                    dr[3] = dt.Rows[i][3];
                    lock (dt_OffLine)
                    {
                        if (dt_OffLine.Rows.Contains(dr[1])) { }
                        else
                        {
                            dt_OffLine.Rows.Add(dr);
                        }
                    }
                    dt.Rows[i]["State"] = 0;
                }
                //Mutex mutex = new Mutex(true,"progress_i");

                lock (lockSomeThing)
                {
                    global_i++;
                    Console.WriteLine("当前线程是：" + Thread.CurrentThread.Name + ";当前的global_i:"
                        + global_i.ToString() + "当前的i:" + i);
                }

                backgroundWorker1.ReportProgress(global_i * 100 / dt.Rows.Count, "正在扫描设备，当前进度" + global_i.ToString() + "/" + dt.Rows.Count);
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
            //MessageUtil.MessageInfo("刷新成功！");
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt_OnLine;
            label3.Text = dt_OnLine.Rows.Count.ToString();
            dataGridView1.Refresh();
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = dt_OffLine;
            label4.Text = dt_OffLine.Rows.Count.ToString();
            dataGridView2.Refresh();
            //sw.Stop();
            //MessageBox.Show(sw.Elapsed.ToString());
            listView1.BeginUpdate();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = dt.Rows[i][1].ToString().Trim();
                lvi.ImageIndex = int.Parse(dt.Rows[i][4].ToString().Trim());
                listView1.Items.Add(lvi);
            }
            listView1.EndUpdate();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            //DataTable dt = ser_Machine.GetList("").Tables[0];
            backgroundWorker1.RunWorkerAsync();
            waitingForm.ShowDialog();
        }

        private void 机房设备维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaintainForm maintainForm = new MaintainForm();
            maintainForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            waitingForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainFormAuto mainFormAuto = new MainFormAuto();
            mainFormAuto.ShowDialog();
        }

        private void MainFormHome_DoubleClick(object sender, EventArgs e)
        {
            if (this.listView1.FocusedItem != null)

            {
                if (this.listView1.SelectedItems != null)
                {
                    foreach (ListViewItem item in this.listView1.SelectedItems)
                    {
                        //MessageBox.Show(item.SubItems[0].ToString());
                        item.ImageIndex = Math.Abs(item.ImageIndex + (-1));
                    }
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            if (info.Item != null)
            {
                info.Item.ImageIndex = Math.Abs(info.Item.ImageIndex + (-1));
            }
        }

        private bool PingGo(String IP)
        {
            Ping ping = new Ping();
            PingOptions pingOptions = new PingOptions();
            pingOptions.Ttl = 3;
            PingReply pingReply = ping.Send(IP);
            return pingReply.Status == IPStatus.Success;
        }

    }
}
