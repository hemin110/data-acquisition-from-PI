using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using PISDK;
using PISDKCommon;
using PITimeServer;
using System.IO;

namespace PiUtinity
{
    public partial class opeart : Form
    {

        Server server = null;
        PISDK.PISDK pisdk = null;
        string piConnectionString = null;
        string uid = null;
        string pwd = null;
        int piPort;
        string hostName = null;
        string destFile;

        public opeart()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text = open.FileName;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                textBox6.Text = folder.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            destFile = textBox6.Text;
            String sourFile = textBox5.Text;
            uid = textBox2.Text;
            pwd = textBox3.Text;
            hostName = textBox1.Text;
            piPort =int.Parse(textBox4.Text);

            if (destFile == "" || sourFile == "" || uid == "" || hostName == "" || piPort == null)
            {
                MessageBox.Show("请将信息填写完整", "提示");
            }
            else
            {
                textBox7.AppendText("采集参数设置完毕\n");
                textBox7.AppendText("主机名：" + hostName + "\n");
                textBox7.AppendText("端口：" + piPort + "\n");
                textBox7.AppendText("用户名：" + uid + "\n");
                textBox7.AppendText("目标文件夹" + destFile + "\n");
                textBox7.AppendText("测点文件名" + sourFile + "\n");
                textBox7.AppendText(dateTimePicker1.Text + "-" + dateTimePicker2.Text+"\n");

                //开始写程序了
                opeart pih = new opeart(hostName, uid, "", piPort);  //初始化数据库连接
                string strReadFilePath = @sourFile;
                textBox7.AppendText("数据库连接成功！\n");
                StreamReader srReadFile = new StreamReader(strReadFilePath);
                textBox7.AppendText("测点表读取成功\n");
                while (!srReadFile.EndOfStream)
                {
                    DateTime thisDate1 = new DateTime(int.Parse(dateTimePicker1.Value.Year.ToString()), int.Parse(dateTimePicker1.Value.Month.ToString()), int.Parse(dateTimePicker1.Value.Day.ToString()));  //起始日期；
                    DateTime thisDate2 = new DateTime(int.Parse(dateTimePicker2.Value.Year.ToString()), int.Parse(dateTimePicker2.Value.Month.ToString()), int.Parse(dateTimePicker2.Value.Day.ToString()));  // 结束日期；
                    string strReadLine = srReadFile.ReadLine(); //读取每行数据   从每行获取测点字段  

                    if (strReadLine != null && strReadLine != "")
                    {
                        string start = strReadLine.Trim();    // "70BFP02VE";  
                        textBox7.AppendText(start+"\n");
                        while (DateTime.Compare(thisDate1, thisDate2) != 0)
                        {
                            DateTime Midtime = thisDate1.AddDays(1);
                            System.Console.WriteLine(start + thisDate1.ToString() + " ----- " + Midtime.ToString());
                            textBox7.AppendText(start+":" + thisDate1.ToString() + " ----- " + Midtime.ToString()+"\n");
                            DataTable dt = pih.GetHistoryDataFromPI(start, thisDate1, Midtime, "");

                                textBox7.AppendText("测点"+start+"读取失败！\n");

                                if (start.Contains("/") || start.Contains(":"))
                                {
                                    string fix = null;
                                    fix = start.Replace('/', '_');
                                    fix = fix.Replace(':', '_');
                                    pih.WriteTxt(dt, fix , destFile);
                                }
                                else
                                {
                                    pih.WriteTxt(dt, start , destFile);
                                }

                                thisDate1 = Midtime;
                      
                            /*
                            将datatable写入到txt中
                             */
                            
                        }
                    }
                }
                textBox7.AppendText("读取完毕");

            }

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }


        public opeart(string hostName, string uid, string pwd, int piPort)
        {
            this.hostName = hostName;
            this.uid = uid;
            this.pwd = pwd;
            this.piPort = piPort;
            piConnectionString = string.Format("UID={0};PWD={1};port={2};Host={3};", uid, pwd, piPort, hostName);
            try
            {
                pisdk = new PISDK.PISDK();
                //.PISDKClass();
                foreach (Server server in pisdk.Servers)
                {
                    if (server.Name.Equals(hostName))
                    {
                        this.server = server;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("初始化PIServer发生错误:" + ex.Message);
            }
            System.Console.WriteLine("初始化成功! ");

        }
        ~opeart()
        {
            if (this.server != null)
            {
                this.server.Close();
            }
        }

        protected void Dispose(bool disposing)
        {
            //释放非托管资源
            if (this.server != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(this.server);
            }
            if (this.pisdk != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(this.pisdk);
            }
            GC.Collect();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.ReRegisterForFinalize(this);
        }

        public virtual DataTable GetHistoryDataFromPI(string tagName, DateTime startTime, DateTime endTime, string filter)
        {
            if (this.server == null)
            {
                throw new Exception("server未初始化。");
            }
            PIValues pivalues = null;
            //PointList pLst = null;
            //StringBuilder sb = new StringBuilder();
            //由于历史数据可能过多，所以选择用datatable返回
            DataTable dt = null;
            try
            {
                if (!this.server.Connected)
                {
                    this.server.Open(this.piConnectionString);
                    System.Console.WriteLine("连接成功! ");  
                }
                //pLst = this.server.GetPoints(tagName, null);

                PIPoint pipoint = this.server.PIPoints[tagName];

                if (pipoint.Equals(null))
                {
                    System.Console.WriteLine("测点不存在");

                }
                else
                {
                    startTime = DateTime.Parse(startTime.Year.ToString() + "-" + startTime.Month.ToString() + "-" + startTime.Day.ToString() + " 00:00:00");
                    endTime = DateTime.Parse(endTime.Year.ToString() + "-" + endTime.Month.ToString() + "-" + endTime.Day.ToString() + " 00:00:00");
                    PITime ptStart = new PITime(); //PITimeClass();
                    ptStart.LocalDate = startTime;
                    PITime ptEnd = new PITime();
                    ptEnd.LocalDate = endTime;
                    dt = new DataTable();
                    // dt.Columns.Add("tagName", typeof(string));
                    dt.Columns.Add("timeSpan", typeof(string));
                    dt.Columns.Add("value", typeof(string));
                    pivalues = pipoint.Data.RecordedValues(ptStart, ptEnd, BoundaryTypeConstants.btInside, filter);//取历史数据
                    foreach (PIValue p in pivalues)
                    {
                        DataRow row = dt.NewRow();
                        // row[0] = pipoint.Name;
                        row[0] = p.TimeStamp.LocalDate.ToString();
                        row[1] = p.Value;
                        dt.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }


            System.Console.WriteLine("获取数据成功! ");

            return dt;
        }

        private void WriteTxt(DataTable tb, string fileName ,string destFile)
        {
            StreamWriter sr;

            if (File.Exists(destFile + "\\"+fileName + ".txt"))   //如果文件存在,则创建File.AppendText对象   
            {
                //MessageBox.Show(destFile ,"hah");
                //MessageBox.Show(destFile + "\\" + fileName + ".txt", "提示");
                sr = File.AppendText(destFile + "\\" + fileName + ".txt");


            }
            else   //如果文件不存在,则创建File.CreateText对象   
            {
                sr = File.CreateText(destFile + "\\" + fileName + ".txt");

            }
            StringBuilder sb = new StringBuilder();

            ///////////////////////////////
            if (tb == null)
            {
                sr.WriteLine("ERROE");
                sr.Close();
            }
            else
            {
                foreach (DataRow dr in tb.Rows)
                {
                    sr.WriteLine(dr[0].ToString() + "\t" + dr[1].ToString() + "\r\n");

                }
                sr.Close();
            }
        }

    }
}
