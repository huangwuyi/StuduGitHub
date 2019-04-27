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
    public partial class MainFormAuto : Form
    {
        Service_Machine ser_Machine = new Service_Machine();
        int on_line = 0, off_line = 0;
        public MainFormAuto()
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
            bindHome();
            tabControl1.SelectedIndex = 1;
        }

        private void initialized()
        {
            dt = ser_Machine.GetList("").Tables[0];

            dt.PrimaryKey = null;            
            dt.Columns.Clear();
            dt.Rows.Clear();
            dt_OnLine.PrimaryKey = new DataColumn[] { dt_OnLine.Columns["Ip"] };

            dt.Columns.Add("序号");
            dt.Columns.Add("Ip");
            dt.Columns.Add("机器名称");
            dt.Columns.Add("机器备注");
            //
            string gateway = GetGateway();
            MessageBox.Show("当前网关是"+gateway+";点击确定自动扫描当前网关；");
            string gatewayleft = gateway.Substring(0, gateway.Length - 1);
            for (int i = 0; i < 20; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr["ip"] = gatewayleft + i.ToString();
                dr[2] = "自动扫描ip" + i.ToString();
                dr[3] = "";
                dt.Rows.Add(dr);
            }

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

        private string GetGateway()
        {
            //网关地址
            string strGateway = "";
            //获取所有网卡
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            //遍历数组
            foreach (var netWork in nics)
            {
                //单个网卡的IP对象
                IPInterfaceProperties ip = netWork.GetIPProperties();
                //获取该IP对象的网关
                GatewayIPAddressInformationCollection gateways = ip.GatewayAddresses;
                foreach (var gateWay in gateways)
                {
                    //如果能够Ping通网关
                    if (IsPingIP(gateWay.Address.ToString()))
                    {
                        //得到网关地址
                        strGateway = gateWay.Address.ToString();
                        //跳出循环
                        break;
                    }
                }

                //如果已经得到网关地址
                if (strGateway.Length > 0)
                {
                    //跳出循环
                    break;
                }
            }

            //返回网关地址
            return strGateway;
        }

        /// <summary>
        /// 尝试Ping指定IP是否能够Ping通
        /// </summary>
        /// <param name="strIP">指定IP</param>
        /// <returns>true 是 false 否</returns>
        public static bool IsPingIP(string strIP)
        {
            try
            {
                //创建Ping对象
                Ping ping = new Ping();
                //接受Ping返回值
                PingReply reply = ping.Send(strIP, 1000);
                //Ping通
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                //Ping失败
                return false;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            refresh_MachineState();
        }

        private void refresh_MachineState()
        {
            initialized();
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
                    if (dt_OnLine.Rows.Contains(dr[1])) { }
                    else
                    {
                        dt_OnLine.Rows.Add(dr);
                    }
                }
                else
                {
                    DataRow dr = dt_OffLine.NewRow();
                    dr[0] = dt.Rows[i][0];
                    dr[1] = dt.Rows[i][1];
                    dr[2] = dt.Rows[i][2];
                    dr[3] = dt.Rows[i][3];
                    if (dt_OffLine.Rows.Contains(dr[1])) { }
                    else
                    {
                        dt_OffLine.Rows.Add(dr);
                    }
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
            //
            try
            {
                string lsno = comboBox1.SelectedValue.ToString().Trim();
                if (!new Service_Home().Exists(lsno))
                {
                    MessageBox.Show("先选择机房");
                    return;
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    Service_Machine.Table_Machine table_Machine = new Service_Machine.Table_Machine();
                    table_Machine.Ip = dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                    table_Machine.MachineName = dataGridView1.Rows[i].Cells[2].Value.ToString().Trim();
                    table_Machine.MachineRemark = lsno;// dataGridView1.Rows[i].Cells[3].Value.ToString().Trim();
                    Service_Machine service_Machine = new Service_Machine();
                    if (!service_Machine.Exists(table_Machine.Ip))
                    {
                        service_Machine.Add(table_Machine);
                    }
                    Service_Home_Machine.Table_Home_Machine table_Home_Machine = new Service_Home_Machine.Table_Home_Machine();
                    Service_Home_Machine service_Home_Machine = new Service_Home_Machine();
                    table_Home_Machine.Ip = table_Machine.Ip;
                    table_Home_Machine.Home = lsno;
                    if (!service_Home_Machine.Exists(table_Home_Machine.Home, table_Home_Machine.Ip))
                    {
                        service_Home_Machine.Add(table_Home_Machine);
                    }
                }
                MessageBox.Show("添加成功！");
            }
            catch
            {
                MessageBox.Show("添加失败！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string lsno = comboBox1.SelectedValue.ToString().Trim();
                if (!new Service_Home().Exists(lsno))
                {
                    MessageBox.Show("先选择机房");
                    return;
                }
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    Service_Machine.Table_Machine table_Machine = new Service_Machine.Table_Machine();
                    table_Machine.Ip = dataGridView2.Rows[i].Cells[1].Value.ToString().Trim();
                    table_Machine.MachineName = dataGridView2.Rows[i].Cells[2].Value.ToString().Trim();
                    table_Machine.MachineRemark = lsno;// dataGridView2.Rows[i].Cells[3].Value.ToString().Trim();
                    Service_Machine service_Machine = new Service_Machine();
                    if (!service_Machine.Exists(table_Machine.Ip))
                    {
                        service_Machine.Add(table_Machine);
                    }
                    Service_Home_Machine.Table_Home_Machine table_Home_Machine = new Service_Home_Machine.Table_Home_Machine();
                    Service_Home_Machine service_Home_Machine = new Service_Home_Machine();
                    table_Home_Machine.Ip = table_Machine.Ip;
                    table_Home_Machine.Home = lsno;
                    if (!service_Home_Machine.Exists(table_Home_Machine.Home, table_Home_Machine.Ip))
                    {
                        service_Home_Machine.Add(table_Home_Machine);
                    }
                }
                MessageBox.Show("添加成功！");
            }
            catch
            {
                MessageBox.Show("添加失败！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HomeMaintain homeMaintain = new HomeMaintain();
            homeMaintain.ShowDialog();
            bindHome();
        }

        private void bindHome()
        {
            Service_Home service_Home = new Service_Home();
            DataTable dt = service_Home.GetList("").Tables[0];
            comboBox1.DataSource = dt;
            comboBox1.ValueMember = dt.Columns[1].ToString().Trim();
            comboBox1.DisplayMember = dt.Columns[1].ToString().Trim();
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
