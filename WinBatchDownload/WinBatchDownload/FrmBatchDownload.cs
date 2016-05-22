using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.Collections;



namespace WinBatchDownload
{
    public partial class FrmBatchDownload : Form
    {
       

        //存放下载列表
        List<SynFileInfo> m_SynFileInfoList;

       

        public FrmBatchDownload()
        {
            InitializeComponent();
            m_SynFileInfoList = new List<SynFileInfo>();
        }

     

        private void FrmBatchDownload_Load(object sender, EventArgs e)
        {
            //初始化DataGridView相关属性
            InitDataGridView(dgvDownLoad);
            //添加DataGridView相关列信息
            AddGridViewColumns(dgvDownLoad);
            //新建任务
            AddBatchDownload();
        }

      
        //添加GridView列
      
        void AddGridViewColumns(DataGridView dgv)
        {
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "DocID",
                HeaderText = "文件ID",
                Visible = false,
                Name = "DocID"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DataPropertyName = "DocName",
                HeaderText = "文件名",
                Name = "DocName",
                Width = 300
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "FileSize",
                HeaderText = "大小",
                Name = "FileSize",
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SynSpeed",
                HeaderText = "速度",
                Name = "SynSpeed"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SynProgress",
                HeaderText = "进度",
                Name = "SynProgress"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "DownPath",
                HeaderText = "下载地址",
                Visible = false,
                Name = "DownPath"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SavePath",
                HeaderText = "保存地址",
                Visible = false,
                Name = "SavePath"
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Async",
                HeaderText = "是否异步",
                Visible = false,
                Name = "Async"
            });
        }

   

        //添加下载任务并显示到列表中

        void AddBatchDownload()
        {
            dgvDownLoad.Rows.Clear();
            List<ArrayList> arrayListList = new List<ArrayList>();
            arrayListList.Add(new ArrayList(){
                "0",//文件id
                "周杰伦-青花瓷.avi",//文件名称
                "21.2 MB",//文件大小
                "0 KB/S",//下载速度
                "0%",//下载进度
                "http://o7b09w3mn.bkt.clouddn.com/%E5%91%A8%E6%9D%B0%E4%BC%A6-%E9%9D%92%E8%8A%B1%E7%93%B7.avi",//远程服务器下载地址
                "E:/学习/c#/data/周杰伦-青花瓷.avi",//本地保存地址
                true//是否异步
            });
            arrayListList.Add(new ArrayList(){
                "1",
                "周杰伦-不能说的秘密.mp4",
                "14.3 MB",
                "0 KB/S",
                "0%",
                "http://o7b09w3mn.bkt.clouddn.com/%E5%91%A8%E6%9D%B0%E4%BC%A6-%E4%B8%8D%E8%83%BD%E8%AF%B4%E7%9A%84%E7%A7%98%E5%AF%86.mp4",
                "E:/学习/c#/data/周杰伦-不能说的秘密.mp4",
                true
            });
           
            foreach (ArrayList arrayList in arrayListList)
            {
                int rowIndex = dgvDownLoad.Rows.Add(arrayList.ToArray());
                arrayList[2] = 0;
                arrayList.Add(dgvDownLoad.Rows[rowIndex]);
                m_SynFileInfoList.Add(new SynFileInfo(arrayList.ToArray()));
            }
        }

       

        //开始下载按钮单机事件

        private void btnStartDownLoad_Click(object sender, EventArgs e)
        {
            //判断网络连接是否正常
            if (isConnected())
            {
                //设置不可用
                btnStartDownLoad.Enabled = false;
                //设置最大活动线程数以及可等待线程数
                ThreadPool.SetMaxThreads(3, 3);
                //判断是否还存在任务
                if (m_SynFileInfoList.Count <= 0) AddBatchDownload();
                foreach (SynFileInfo m_SynFileInfo in m_SynFileInfoList)
                {
                    StartDownLoad(m_SynFileInfo);
                }
            }
            else
            {
                MessageBox.Show("无法连接网络!");
            }
        }

  

        

        //检测网络状态
        [DllImport("wininet.dll")]
        extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);
       
        //检测网络状态
       
        bool isConnected()
        {
            int I = 0;
            bool state = InternetGetConnectedState(out I, 0);
            return state;
        }

       

 
        // HTTP下载远程文件并保存本地的函数
        
        void StartDownLoad(object o)
        {
            SynFileInfo m_SynFileInfo = (SynFileInfo)o;
            m_SynFileInfo.LastTime = DateTime.Now;
            //再次new 避免WebClient不能I/O并发 
            WebClient client = new WebClient();
            if (m_SynFileInfo.Async)
            {
                //异步下载
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                client.DownloadFileAsync(new Uri(m_SynFileInfo.DownPath), m_SynFileInfo.SavePath, m_SynFileInfo);
            }
            else client.DownloadFile(new Uri(m_SynFileInfo.DownPath), m_SynFileInfo.SavePath);
        }

      
        // 下载进度条
       
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            SynFileInfo m_SynFileInfo = (SynFileInfo)e.UserState;
            m_SynFileInfo.SynProgress = e.ProgressPercentage + "%";
            double secondCount = (DateTime.Now - m_SynFileInfo.LastTime).TotalSeconds;
            m_SynFileInfo.SynSpeed = FileOperate.GetAutoSizeString(Convert.ToDouble(e.BytesReceived / secondCount), 2) + "/s";
            //更新DataGridView中相应数据显示下载进度
            m_SynFileInfo.RowObject.Cells["SynProgress"].Value = m_SynFileInfo.SynProgress;
            //更新DataGridView中相应数据显示下载速度(总进度的平均速度)
            m_SynFileInfo.RowObject.Cells["SynSpeed"].Value = m_SynFileInfo.SynSpeed;
        }

        
        //下载完成调用
       
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //到此则一个文件下载完毕
            SynFileInfo m_SynFileInfo = (SynFileInfo)e.UserState;
            m_SynFileInfoList.Remove(m_SynFileInfo);
            if (m_SynFileInfoList.Count <= 0)
            {
                //此时所有文件下载完毕
                btnStartDownLoad.Enabled = true;
            }
        }

       

       

       

        //初始化GridView

        void InitDataGridView(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = false;//是否自动创建列
            dgv.AllowUserToAddRows = false;//是否允许添加行(默认：true)
            dgv.AllowUserToDeleteRows = false;//是否允许删除行(默认：true)
            dgv.AllowUserToResizeColumns = false;//是否允许调整大小(默认：true)
            dgv.AllowUserToResizeRows = false;//是否允许调整行大小(默认：true)
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;//列宽模式(当前填充)(默认：DataGridViewAutoSizeColumnsMode.None)
            dgv.BackgroundColor = System.Drawing.Color.White;//背景色(默认：ControlDark)
            dgv.BorderStyle = BorderStyle.Fixed3D;//边框样式(默认：BorderStyle.FixedSingle)
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;//单元格边框样式(默认：DataGridViewCellBorderStyle.Single)
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;//列表头样式(默认：DataGridViewHeaderBorderStyle.Single)
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;//是否允许调整列大小(默认：DataGridViewColumnHeadersHeightSizeMode.EnableResizing)
            dgv.ColumnHeadersHeight = 30;//列表头高度(默认：20)
            dgv.MultiSelect = false;//是否支持多选(默认：true)
            dgv.ReadOnly = true;//是否只读(默认：false)
            dgv.RowHeadersVisible = false;//行头是否显示(默认：true)
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//选择模式(默认：DataGridViewSelectionMode.CellSelect)
        }

       

       
    }
}