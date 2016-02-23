using System;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.Net.Sockets;
// Add a reference to NetworkingBasics.dll: classes used - BufferChunk
// Add a reference to LSTCommon.dll: classes used -  UnzhandledExceptionHandler
//using MSR.LST;              

// Add a reference to MSR.LST.Net.Rtp.dll
// Classes used - RtpSession, RtpSender, RtpParticipant, RtpStream
using MSR.LST.Net.Rtp;

// Add a reference to OxyPlot
using OxyPlot.WindowsForms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace PMF
{
    public class PMF: System.Windows.Forms.Form
    {
        // Form variables
        public PictureBox pictureBox_Receive;
       
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 操作ToolStripMenuItem;
        private ToolStripMenuItem joinToolStripMenuItem;
        private ToolStripMenuItem leaveToolStripMenuItem;
        private ComboBox comboBox1;
        private Label label_p;
        private TextBox textBox_p;
        private Label label_iv;
        private TextBox textBox_curval;
        private ListView listViewControl;
        private Label label1;
       

        // variables for surface plot part
        ChartStyle cst;
        ChartStyle2D cs2d;
        DataSeries ds;
        DrawChart dc;
        ChartFunctions cf;
        private Panel panel2;
        private PlotView plot1;
        ColorMap cm;

        AutoSizeFormClass asc = new AutoSizeFormClass();
        int iHeight = 320;
        int iWidth = 400;

        #region Create Thread and Delegate 
        /********创建线程及委托*******/

        // 绘制采集波形线程
        public Thread drawThread;
        public delegate void DrawWave();
        public DrawWave drawWave;

        // 绘制errormap线程
        public Thread drawContourThread;
        public delegate void DrawContourWave();
        public DrawContourWave drawContourWave;

        // 绘制errormap3d线程
        public Thread drawSurfaceThread;
        public delegate void DrawSurface();
        public DrawSurface drawSurface;

        public Thread searchCableThread;
        #endregion


        #region Communication data

        int channels = 8;                     // 通道数目
        int bufSize = 1024;                   // 每通道数据数目
        static int bulks = 10;
        static int ynum = 35, znum = 35;
        double ymin = -2, ymax = 2, zmin = -2, zmax = 0;

        double[,] res = new double[8, bulks*1024];  // 存储各个通道的数据
        
        double[] timelen = new double[bulks*1024];  // 时间轴数据

        double[] freqlen = new double[bulks * 1024];

        double[,] sig = new double[8, bulks * 1024];

        double[,] freq = new double[8, bulks * 1024];
       
        bool VALUE_GET = false;               // 当前是否获取波形

        double []current=new double[3];       // 预测电流大小
        double []position=new double[3];      // 预测的位置
        double[] e = new double[8];           // 8个线圈的电动势
        double[] X = new double[ynum];
        double[] Y = new double[znum];
        double[,] errorMap = new double[ynum, znum];
        bool GET_CABLE_POS = false;
        bool PREDICT_VALUE_GET = false;
        bool paint_once = true;

        
        #endregion

        #region RTP session members

        /// <summary>
        /// Create a Rtp session
        /// </summary>
       
        public Panel PlotPanel;
        private Label label_IP;
        private TextBox textBox_IP;
        private Label label_port;
        private TextBox textBox_Port;

        /// <summary>
        /// Sends the data across the network
        /// </summary>

        Rtp rtp;
        SearchCable sc;

        #endregion members

        public PMF()
        {
            InitializeComponent();
//             PlotPanel.Paint +=
//                 new PaintEventHandler(PlotPanelPaint);
            // 默认显示线圈1的波形
            comboBox1.SelectedIndex = 1;

            // 在初始界面小时线圈、电动势表格
            listViewControl.View = View.Details;
            listViewControl.BeginUpdate();

            ColumnHeader coil = new ColumnHeader();
            coil.Text = "线圈";
            coil.Width = listViewControl.Size.Width / 3;

            ColumnHeader volts = new ColumnHeader();
            volts.Text = "电动势";
            volts.Width = listViewControl.Size.Width / 3*2;

            this.listViewControl.Columns.Add(coil);
            this.listViewControl.Columns.Add(volts);

            for (int i = 0; i < 8; ++i)
            {
                listViewControl.Items.Add("coil" + i.ToString(), "e" + i.ToString(), 0);
                listViewControl.Items["coil" + i.ToString()].SubItems.Add(" ");
            }
            listViewControl.EndUpdate();

            // rtp socket
           // ep = new IPEndPoint(IPAddress.Parse("192.168.1.6"), 5001);
            rtp = new Rtp(iWidth, iHeight, this);

            for (int i = 0; i < ynum;++i )
                X[i] = (ymax-ymin) / ynum * i;
            for (int i = 0; i < znum; ++i)
                Y[i] = (zmax-zmin) / znum * i;


            drawWave = new DrawWave(run_plot_primitive);
          //  drawContourWave=new DrawContourWave(run_plot);
            drawSurface=new DrawSurface(run_surface_plot);

            cst = new ChartStyle(this);
            cs2d = new ChartStyle2D(this);
            ds = new DataSeries();
            dc = new DrawChart(this);
            cf = new ChartFunctions();
            cm = new ColorMap();



            cst.GridStyle.LineColor = Color.LightGray;
            cst.GridStyle.Pattern = DashStyle.Dash;
            cst.Title = "ErrorMap3D";
            cst.IsColorBar = true;

            cs2d.ChartBackColor = Color.White;
            cs2d.ChartBorderColor = Color.Black;
            

            dc.ChartType = DrawChart.ChartTypeEnum.FillContour;
            dc.IsColorMap = true;
            dc.IsHiddenLine = false;
            ds.LineStyle.IsVisible = false;
            // 无网格模式时颜色的平滑
            dc.IsInterp = true;
            dc.NumberInterp = 2;
            dc.CMap = cm.Jet();
            
        }


        // Required method for Designer support - do not modify
        // the contents of this method with the code editor.
        private void InitializeComponent()
        {
            this.pictureBox_Receive = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.plot1 = new OxyPlot.WindowsForms.PlotView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label_p = new System.Windows.Forms.Label();
            this.textBox_p = new System.Windows.Forms.TextBox();
            this.label_iv = new System.Windows.Forms.Label();
            this.textBox_curval = new System.Windows.Forms.TextBox();
            this.listViewControl = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.PlotPanel = new System.Windows.Forms.Panel();
            this.label_IP = new System.Windows.Forms.Label();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.label_port = new System.Windows.Forms.Label();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Receive)).BeginInit();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox_Receive
            // 
            this.pictureBox_Receive.Image = global::PMF.Properties.Resources.ip;
            this.pictureBox_Receive.Location = new System.Drawing.Point(12, 291);
            this.pictureBox_Receive.Name = "pictureBox_Receive";
            this.pictureBox_Receive.Size = new System.Drawing.Size(341, 210);
            this.pictureBox_Receive.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Receive.TabIndex = 6;
            this.pictureBox_Receive.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.plot1);
            this.panel2.Location = new System.Drawing.Point(380, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(341, 255);
            this.panel2.TabIndex = 14;
            // 
            // plot1
            // 
            this.plot1.Location = new System.Drawing.Point(3, 0);
            this.plot1.Name = "plot1";
            this.plot1.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plot1.Size = new System.Drawing.Size(338, 252);
            this.plot1.TabIndex = 0;
            this.plot1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plot1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plot1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.操作ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(747, 25);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 操作ToolStripMenuItem
            // 
            this.操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.joinToolStripMenuItem,
            this.leaveToolStripMenuItem});
            this.操作ToolStripMenuItem.Name = "操作ToolStripMenuItem";
            this.操作ToolStripMenuItem.Size = new System.Drawing.Size(48, 21);
            this.操作ToolStripMenuItem.Text = " 操作";
            // 
            // joinToolStripMenuItem
            // 
            this.joinToolStripMenuItem.Name = "joinToolStripMenuItem";
            this.joinToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.joinToolStripMenuItem.Text = "Join";
            this.joinToolStripMenuItem.Click += new System.EventHandler(this.joinToolStripMenuItem_Click);
            // 
            // leaveToolStripMenuItem
            // 
            this.leaveToolStripMenuItem.Name = "leaveToolStripMenuItem";
            this.leaveToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.leaveToolStripMenuItem.Text = "Leave";
            this.leaveToolStripMenuItem.Click += new System.EventHandler(this.leaveToolStripMenuItem_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "多通道",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.comboBox1.Location = new System.Drawing.Point(90, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(90, 20);
            this.comboBox1.TabIndex = 16;
            // 
            // label_p
            // 
            this.label_p.AutoSize = true;
            this.label_p.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_p.Location = new System.Drawing.Point(378, 365);
            this.label_p.Name = "label_p";
            this.label_p.Size = new System.Drawing.Size(53, 12);
            this.label_p.TabIndex = 19;
            this.label_p.Text = "电缆位置";
            // 
            // textBox_p
            // 
            this.textBox_p.Location = new System.Drawing.Point(437, 362);
            this.textBox_p.Name = "textBox_p";
            this.textBox_p.Size = new System.Drawing.Size(77, 21);
            this.textBox_p.TabIndex = 20;
            // 
            // label_iv
            // 
            this.label_iv.AutoSize = true;
            this.label_iv.Location = new System.Drawing.Point(378, 441);
            this.label_iv.Name = "label_iv";
            this.label_iv.Size = new System.Drawing.Size(53, 12);
            this.label_iv.TabIndex = 23;
            this.label_iv.Text = "电流大小";
            // 
            // textBox_curval
            // 
            this.textBox_curval.Location = new System.Drawing.Point(438, 438);
            this.textBox_curval.Name = "textBox_curval";
            this.textBox_curval.Size = new System.Drawing.Size(77, 21);
            this.textBox_curval.TabIndex = 24;
            // 
            // listViewControl
            // 
            this.listViewControl.GridLines = true;
            this.listViewControl.Location = new System.Drawing.Point(548, 326);
            this.listViewControl.Name = "listViewControl";
            this.listViewControl.Scrollable = false;
            this.listViewControl.Size = new System.Drawing.Size(136, 175);
            this.listViewControl.TabIndex = 25;
            this.listViewControl.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 26;
            this.label1.Text = "通道";
            // 
            // PlotPanel
            // 
            this.PlotPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlotPanel.Location = new System.Drawing.Point(12, 32);
            this.PlotPanel.Name = "PlotPanel";
            this.PlotPanel.Size = new System.Drawing.Size(341, 252);
            this.PlotPanel.TabIndex = 27;
            // 
            // label_IP
            // 
            this.label_IP.AutoSize = true;
            this.label_IP.Location = new System.Drawing.Point(212, 8);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(47, 12);
            this.label_IP.TabIndex = 28;
            this.label_IP.Text = "IP 地址";
            // 
            // textBox_IP
            // 
            this.textBox_IP.Location = new System.Drawing.Point(279, 3);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(123, 21);
            this.textBox_IP.TabIndex = 29;
            this.textBox_IP.Text = "127.0.0.1";
            // 
            // label_port
            // 
            this.label_port.AutoSize = true;
            this.label_port.Location = new System.Drawing.Point(435, 7);
            this.label_port.Name = "label_port";
            this.label_port.Size = new System.Drawing.Size(29, 12);
            this.label_port.TabIndex = 30;
            this.label_port.Text = "端口";
            // 
            // textBox_Port
            // 
            this.textBox_Port.Location = new System.Drawing.Point(479, 4);
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(63, 21);
            this.textBox_Port.TabIndex = 31;
            this.textBox_Port.Text = "8001";
            // 
            // PMF
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(747, 531);
            this.Controls.Add(this.textBox_Port);
            this.Controls.Add(this.label_port);
            this.Controls.Add(this.textBox_IP);
            this.Controls.Add(this.label_IP);
            this.Controls.Add(this.PlotPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewControl);
            this.Controls.Add(this.textBox_curval);
            this.Controls.Add(this.label_iv);
            this.Controls.Add(this.textBox_p);
            this.Controls.Add(this.label_p);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pictureBox_Receive);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PMF";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "被动磁场采集显示";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmChat_Closing);
            this.Load += new System.EventHandler(this.frmChat_Load);
            this.SizeChanged += new System.EventHandler(this.frmChat_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Receive)).EndInit();
            this.panel2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        [STAThread]
        static void Main() 
        {
            Application.EnableVisualStyles();
            // Make sure no exceptions escape unnoticed
            //UnhandledExceptionHandler.Register();
            Application.Run(new PMF());
        
        }
        
       

        private void frmChat_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                
                rtp.Cleanup();
            }
            catch (Exception){}
        }       
    

        private void frmChat_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
        }


        private void frmChat_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
            rtp.Width  =  pictureBox_Receive.Width;
            rtp.Height =  pictureBox_Receive.Height;
            rtp.SetwndPos(rtp.hHwnd, 1 ,0, 0, rtp.Width, rtp.Height, (2 | 1));
        }

        #region Update the controls and control its sleeping time

        /**************更新各个控件，控制休眠时间**********/
        void run()
        {
            while (true)
            {
                plot1.Invoke(drawWave, new object[] { });
                Thread.Sleep(2000);
            }
        }

        void run_contour()
        {
            while (true)
            {
                plot1.Invoke(drawContourWave, new object[] { });
                Thread.Sleep(2000);
            }
        }

        void run_surface()
        {
            while (true)
            {
                PlotPanel.Invoke(drawSurface, new object[] { });
                Thread.Sleep(2000);
            }
        }
        #endregion

       
           
        public void tcp_get()
        {
            TcpGet tcp = new TcpGet(8001);
            while (true)
            {
                tcp.getData(bufSize, channels, timelen, res, errorMap, X, Y, e, current, position, ref VALUE_GET, ref GET_CABLE_POS);
                if (paint_once)
                    PlotPanel.Paint += new PaintEventHandler(PlotPanelPaint);
                paint_once = false;

            }
        }

        // 绘制某个线圈的波形
        private void draw_channels(int channel_num)
        {
            var s = new LineSeries { Title = "线圈"+(channel_num+1).ToString(), StrokeThickness = 1 };

            for (int k = 0; k < 1024; k++)
            {
                s.Points.Add(new DataPoint(timelen[k],sig[channel_num,k]));
            }

            // add Series and Axis to plot1 model
            plot1.Model.Series.Add(s);

        }

        private void draw_single_channels(int channel_num)
        {
            plot1.Model = new PlotModel();
            plot1.Dock = DockStyle.Fill;
            // this.Controls.Add(plot1);

            plot1.Model.PlotType = PlotType.XY;
            plot1.Model.Background = OxyColor.FromRgb(255, 255, 255);
            plot1.Model.TextColor = OxyColor.FromRgb(0, 0, 0);
            plot1.Model.Title = "collection waves";

            draw_channels(channel_num);
            plot1.InvalidatePlot(true);

        }

        // 绘制多个线圈的波形
        private void draw_multi_channels() 
        {
            plot1.Model = new PlotModel();
            plot1.Dock = DockStyle.Fill;
            // this.Controls.Add(plot1);

            plot1.Model.PlotType = PlotType.XY;
            plot1.Model.Background = OxyColor.FromRgb(255, 255, 255);
            plot1.Model.TextColor = OxyColor.FromRgb(0, 0, 0);
            plot1.Model.Title = "collection waves";
            for (int i = 0; i < 8; ++i) draw_channels(i);
            plot1.InvalidatePlot(true);
           
        }

        private void run_plot_primitive()
        {

            // 当接收到数据后才开始绘制
            if (VALUE_GET)
            {
                int cb_idx = comboBox1.SelectedIndex;
                if (cb_idx == 0) { draw_multi_channels(); }
                else
                    draw_single_channels(cb_idx - 1);
            }
        }


        private void run_plot()
        {
            //Random rnd = new Random();
            //randa = rnd.Next(10) + 2;


            if (GET_CABLE_POS)
            {
                var model = new PlotModel { Title = "Errormap" };
                var cs = new ContourSeries
                {

                    ColumnCoordinates = X,
                    RowCoordinates = Y,
                    ContourColors = new[] { OxyColors.SeaGreen, OxyColors.RoyalBlue, OxyColors.IndianRed }
                };
                cs.Data = errorMap;
                model.Series.Add(cs);
                plot1.Model = model;
                plot1.InvalidatePlot(true);
            }
            else
            {

                textBox_curval.Text = "计算中";
                textBox_p.Text = "计算中";
                listViewControl.BeginUpdate();
                ColumnHeader coil = new ColumnHeader();
                coil.Text = "线圈";
                coil.Width = listViewControl.Size.Width / 3;

                ColumnHeader volts = new ColumnHeader();
                volts.Text = "电动势";
                volts.Width = listViewControl.Size.Width / 3 * 2;

                this.listViewControl.Columns.Add(coil);
                this.listViewControl.Columns.Add(volts);

                for (int i = 0; i < 8; ++i)
                {
                    ListViewItem lvi = new ListViewItem();

                    listViewControl.Items.Add("coil" + i.ToString(), "e" + i.ToString(), 0);
                    listViewControl.Items["coil" + i.ToString()].SubItems.Add(" ");
                }
                listViewControl.EndUpdate();

                //   randb = rnd.Next(10) + 2;
                Func<double, double, double> peaks = (x, y) =>
              3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1))
              - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y)
              - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);

            }

        

        }


        #region SURFACE PLOT PART
        bool interp = true;
        private void run_surface_plot()
        {
            if (GET_CABLE_POS)
            {

                listViewControl.Clear();
                listViewControl.BeginUpdate();

                ColumnHeader coil = new ColumnHeader();
                coil.Text = "线圈";
                coil.Width = listViewControl.Size.Width / 3;

                ColumnHeader volts = new ColumnHeader();
                volts.Text = "电动势";
                volts.Width = listViewControl.Size.Width / 3 * 2;

                this.listViewControl.Columns.Add(coil);
                this.listViewControl.Columns.Add(volts);

                for (int i = 0; i < 8; ++i)
                {
                    listViewControl.Items.Add("coil" + i.ToString(), "e" + i.ToString(), 0);
                    listViewControl.Items["coil" + i.ToString()].SubItems.Add(e[i].ToString("G4"));
                }
                // 
                listViewControl.EndUpdate();

                textBox_p.Text = position[0].ToString() + "," + position[1].ToString() + " " + position[2].ToString();
                textBox_curval.Text = current[0].ToString() + "," + current[1].ToString() + "," + current[2].ToString();

                this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.OptimizedDoubleBuffer, true);
                this.SetStyle(ControlStyles.ResizeRedraw, true);
                this.BackColor = Color.White;
               
                // Subscribing to a paint eventhandler to drawingPanel: 
                

                PlotPanel.Invalidate();
            }
            else
            {

                textBox_curval.Text = "计算中";
                textBox_p.Text = "计算中";
                listViewControl.BeginUpdate();
                ColumnHeader coil = new ColumnHeader();
                coil.Text = "线圈";
                coil.Width = listViewControl.Size.Width / 3;

                ColumnHeader volts = new ColumnHeader();
                volts.Text = "电动势";
                volts.Width = listViewControl.Size.Width / 3 * 2;

                this.listViewControl.Columns.Add(coil);
                this.listViewControl.Columns.Add(volts);

                for (int i = 0; i < 8; ++i)
                {
                    ListViewItem lvi = new ListViewItem();

                    listViewControl.Items.Add("coil" + i.ToString(), "e" + i.ToString(), 0);
                    listViewControl.Items["coil" + i.ToString()].SubItems.Add(" ");
                }
                listViewControl.EndUpdate();
            }
           
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (GET_CABLE_POS)
            {
                Graphics g = e.Graphics;

                if (dc.ChartType == DrawChart.ChartTypeEnum.XYColor ||
                    dc.ChartType == DrawChart.ChartTypeEnum.Contour ||
                    dc.ChartType == DrawChart.ChartTypeEnum.FillContour)
                {
                    Rectangle rect = this.ClientRectangle;
                    //cs2d.ChartArea = new Rectangle(PlotPanel.Left, PlotPanel.Top,
                    //  PlotPanel.Width, PlotPanel.Height);
                    cs2d.ChartArea = new Rectangle(rect.X + rect.X / 20, rect.Y + rect.Height/13,
                        12*rect.Width / 25, 11 * rect.Height / 25);
                   // cf.Peak3D(ds, cst);
                   cf.myPeak3D(ds, cst, X, Y, errorMap);
                    cs2d.SetPlotArea(g, cst);
                    dc.AddColorBar(g, ds, cst, cs2d);
                }
            }
        }



        public void PlotPanelPaint(object sender, PaintEventArgs e)
        {
            


            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (dc.ChartType == DrawChart.ChartTypeEnum.XYColor ||
                dc.ChartType == DrawChart.ChartTypeEnum.Contour ||
                dc.ChartType == DrawChart.ChartTypeEnum.FillContour)
            {
                //cf.Peak3D(ds, cst);
               // cf.myPeak3D(ds, cst, X, Y, errorMap);
                cs2d.AddChartStyle2D(g, cst);
                dc.AddChart(g, ds, cst, cs2d);
            }
            else
            {
                //cst.Elevation =95;
                //cst.Azimuth = -37;
                cf.myPeak3D(ds, cst,X,Y,errorMap);
              //  cf.Peak3D(ds, cst);
                cst.AddChartStyle(g);
                dc.AddChart(g, ds, cst, cs2d);
            }
        }

       
        public void SearchCables()
        {
            while (true)
            {
                sc = new SearchCable();
                sc.SearchCables();
                VALUE_GET = sc.Value_get;

                double timeLength = sc.TimeLen;
                double interval = timeLength / (bulks * 1024);
                for (int i = 0; i < bulks * 1024; ++i)
                {
                    timelen[i] = interval * i;
                    //freqlen[i] = (double)i / timeLength;
                }

                double[] ptr = sc.Map;
                int cnt = 0;

                for (int i = 0; i < ynum; i++, cnt += znum)
                {
                    for (int j = 0; j < znum; ++j)
                    {
                        errorMap[i, j] = ptr[cnt + j];
                    }

                }

                position[0] = 0.0; position[1] = sc.Y; position[2] = sc.Z;
                current[0] = sc.Current; current[1] = 0; current[2] = 0;
                GET_CABLE_POS = sc.Status;
                if (paint_once)
                    PlotPanel.Paint +=new PaintEventHandler(PlotPanelPaint);
                paint_once = false;

                ptr = sc.Sig;
                int bufLen = bulks * 1024;
                cnt = 0;
                for (int i = 0; i < 8; i++, cnt += bufLen)
                {
                    for (int j = 0; j < bufLen; j++)
                    {
                        sig[i, j] = ptr[cnt + j];
                    }

                }

                e = sc.Volt;
                ptr = sc.Freq;
                cnt = 0;
                for (int i = 0; i < 8; i++, cnt += bufLen)
                {
                    for (int j = 0; j < bufLen; j++)
                    {
                        freq[i, j] = ptr[cnt + j];
                    }
                }
              //  Thread.Sleep(500);
            }
        }
       
        void genData()
        {
            while(true)
            {

                double timeLength =10.0;
                double interval = timeLength / (bulks * 1024);
                for (int i = 0; i < bulks * 1024; ++i)
                {
                    timelen[i] = interval * i;
                    //freqlen[i] = (double)i / timeLength;
                }

                GenData gd = new GenData();
                gd.randomdata(errorMap, sig,e, bulks, bufSize, ynum, znum);
                VALUE_GET = true;
                GET_CABLE_POS = true;

                if (paint_once)
                    PlotPanel.Paint += new PaintEventHandler(PlotPanelPaint);
                paint_once = false;
                Thread.Sleep(500);
            }
        }

        #endregion SURFACE PLOT PART

        /**********启动各个线程************/
        private void joinToolStripMenuItem_Click(object sender, EventArgs e)
        {

//             Thread wave_get_thread = new Thread(tcp_get);
//             wave_get_thread.Start();
//             wave_get_thread.IsBackground = true;

            //Thread rndDataThread = new Thread(genData);
            //rndDataThread.Start();
            //rndDataThread.IsBackground = true;

            searchCableThread = new Thread(SearchCables);
            searchCableThread.Start();
            searchCableThread.IsBackground = true;

            rtp.iDevice = 0;
            rtp.OpenPreviewWindow();
            drawThread = new Thread(new ThreadStart(run));
            drawThread.IsBackground = true;
            drawThread.Start();

            //drawContourThread = new Thread(run_contour);
            //drawContourThread.IsBackground = true;
            //drawContourThread.Start();


            drawSurfaceThread = new Thread(run_surface);
            drawSurfaceThread.IsBackground = true;
            drawSurfaceThread.Start();
            
        }

        /**************关闭各个线程*************/
        private void leaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtp.Cleanup();
            searchCableThread.Abort();
            drawThread.Abort();
          //  drawContourThread.Abort();
            drawSurfaceThread.Abort();
        }

      

    }
}
