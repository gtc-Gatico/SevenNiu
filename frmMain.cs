

using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Qiniu.RS.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SevenNiu
{
    public partial class frmMain : Form
    {
        Image fileImage;
        Image folderImage;
        frmBack frmBack = new frmBack();
        string bucket = "";
        string marker = "";
        string prefix = "";
        Stack<string> level = new Stack<string>();
        public frmMain()
        {

            InitializeComponent();



        }

        #region 改变窗体大小
        const int WM_NCHITTEST = 0x0084;
        const int HTLEFT = 10;    //左边界
        const int HTRIGHT = 11;   //右边界
        const int HTTOP = 12; //上边界
        const int HTTOPLEFT = 13; //左上角
        const int HTTOPRIGHT = 14;    //右上角
        const int HTBOTTOM = 15;  //下边界
        const int HTBOTTOMLEFT = 0x10;    //左下角
        const int HTBOTTOMRIGHT = 17; //右下角

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    {
                        Point vPoint = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);
                        vPoint = PointToClient(vPoint);
                        //判断：仅当当前窗体状态不是最大化时，相关鼠标事件生效
                        if (this.WindowState != FormWindowState.Maximized)
                        {
                            if (vPoint.X < 10)
                            {
                                if (vPoint.Y < 10)
                                {
                                    m.Result = (IntPtr)HTTOPLEFT;
                                }
                                else if (vPoint.Y > this.Height - 10)
                                {
                                    m.Result = (IntPtr)HTBOTTOMLEFT;
                                }
                                else
                                {
                                    m.Result = (IntPtr)HTLEFT;
                                }
                            }
                            else if (vPoint.X > this.Width - 10)
                            {
                                if (vPoint.Y < 10)
                                {
                                    m.Result = (IntPtr)HTTOPRIGHT;
                                }
                                else if (vPoint.Y > this.Height - 10)
                                {
                                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                                }
                                else
                                {
                                    m.Result = (IntPtr)HTRIGHT;
                                }
                            }
                            else if (10 < vPoint.X && vPoint.X < this.Width - 10)
                            {
                                if (vPoint.Y < 10)
                                {
                                    m.Result = (IntPtr)HTTOP;
                                }
                                else if (vPoint.Y > this.Height - 10)
                                {
                                    m.Result = (IntPtr)HTBOTTOM;
                                }
                            }
                        }
                        break;
                    }
            }
        }
        #endregion
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;
                CreateParams cp = base.CreateParams;
                cp.Style = cp.Style | WS_MINIMIZEBOX; // 允许最小化操作 
                return cp;
            }
        }

        //需添加using System.Runtime.InteropServices;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        //常量
        public const int WM_SYSCOMMAND = 0x0112;

        //窗体移动
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        //改变窗体大小
        public const int WMSZ_LEFT = 0xF001;
        public const int WMSZ_RIGHT = 0xF002;
        public const int WMSZ_TOP = 0xF003;
        public const int WMSZ_TOPLEFT = 0xF004;
        public const int WMSZ_TOPRIGHT = 0xF005;
        public const int WMSZ_BOTTOM = 0xF006;
        public const int WMSZ_BOTTOMLEFT = 0xF007;
        public const int WMSZ_BOTTOMRIGHT = 0xF008;
        private void plTitle_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        public void init()
        {
            this.flMain.Controls.Clear();
            BucketsResult bucketsResult = Api.ListZone();
            string[] zone = JsonConvert.DeserializeObject<string[]>(bucketsResult.Text);

            foreach (var item in zone)
            {
                FilePanel filePanel = new FilePanel(this.toolTip1, null);
                Console.WriteLine(item);
                Label lablel = (Label)filePanel.Controls.Find("lblName", true)[0];
                lablel.Text = item;
                PictureBox pictureBox = (PictureBox)filePanel.Controls.Find("pbImg", true)[0];
                pictureBox.Image = folderImage;
                filePanel.MouseDoubleClick += delegate
                {
                    FileDesc fileDesc = (FileDesc)this.Tag;
                    if (fileDesc == null)
                    {
                        Label lableTmp = (Label)filePanel.Controls.Find("lblName", true)[0];
                        this.bucket = lableTmp.Text;
                       
                        loadData();
                        setTitle();
                        level.Push(prefix);
                    }
                };
                pictureBox.MouseDoubleClick += delegate
                {
                    FileDesc fileDesc = (FileDesc)this.Tag;
                    if (fileDesc == null)
                    {
                        Label lableTmp = (Label)filePanel.Controls.Find("lblName", true)[0];
                        this.bucket = lableTmp.Text;
                        loadData();
                        setTitle();
                        level.Push(prefix);
                    }
                };
                this.flMain.Controls.Add(filePanel);
                level.Clear();
                //loadData(item, "", "");
            }

            frmBack.Location = this.Location;
            frmBack.Size = this.Size;
            this.Owner = frmBack;
            this.TopLevel = true;
            if (Settings.config.ContainsKey("opacity"))
            {
                frmBack.Opacity = Convert.ToDouble(Settings.config["opacity"]) /100;
            }
            frmBack.Show();
            if (Settings.config.ContainsKey("background"))
            {
                frmBack.pbBack.Image = Image.FromFile(Settings.config["background"]);
            }

            MenuItem menuItem = new MenuItem("新建文件夹");
            menuItem.Click += delegate {
                if (bucket.Length==0) {
                    MessageBox.Show("文件块请在官网创建，或者在块文件夹内创建", "提示");
                    return;
                }
                string str = Interaction.InputBox("请输入文件夹名称", "提示信息", "", -1, -1);
                char[] arr = new char[] { '/','\\',':','*','?','"','<','>','|' };
                while (str.IndexOfAny(arr)>0) {
                    MessageBox.Show("文件名不能包含下列字符(/,\\,:,*,?,\", <, >,| )", "提示");
                    str = Interaction.InputBox("提示信息", "请输入文件夹名称", "", -1, -1);
                }
                FileStream file = File.Create(Path.GetTempPath()+"说明.txt");
                byte[] data = System.Text.Encoding.Default.GetBytes("本文件是创建文件夹的时候默认创建的，如果不需要，请在删除文件之前上传一个文件到此目录，否则此目录也将会删除。");
                file.Write(data, 0, data.Length);
                file.Flush();
                file.Close();
                Api.UploadFile(bucket,prefix+str+ "/说明.txt", Path.GetTempPath() + "说明.txt");
                loadData();

            };
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(menuItem);
            flMain.ContextMenu = contextMenu;
            
        }

        

        private void frmMain_Load(object sender, EventArgs e)
        {
            fileImage = Image.FromFile(AppContext.BaseDirectory + "res/file.png");
            folderImage = Image.FromFile(AppContext.BaseDirectory + "res/folder.png");
            init();


            this.lblTitle.Text = this.Text;
            //四周边框




        }

        public void loadData()
        {
            this.flMain.Controls.Clear();
            List<ListResult> listResults = Api.ListFiles(bucket, marker, prefix);
            listResults.ForEach(data =>
            {
                Console.WriteLine(data.Result.CommonPrefixes);
                if (data.Result.Items != null)
                {
                    data.Result.Items.ForEach(item =>
                    {
                        FilePanel filePanel = new FilePanel(this.toolTip1, item);
                        Console.WriteLine(item);
                        Label lablel = (Label)filePanel.Controls.Find("lblName", true)[0];
                        if (item.Key.IndexOf("/") > 0)
                        {
                            lablel.Text = item.Key.Substring(item.Key.LastIndexOf("/")+1);
                        }
                        else
                        {
                            lablel.Text = item.Key;
                        }
                        PictureBox pictureBox = (PictureBox)filePanel.Controls.Find("pbImg", true)[0];
                        pictureBox.Image = fileImage;

                        MenuItem menuItem = new MenuItem("下载");
                        menuItem.Tag = item;
                        menuItem.Click += MenuItem_Click;
                        MenuItem deleteMenuItem = new MenuItem("删除");
                        deleteMenuItem.Tag = item;
                        deleteMenuItem.Click += delegate {
                            DialogResult res =  MessageBox.Show("确定要删除{" + lablel.Text + "}吗？", "提示", MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
                            if (DialogResult.OK == res) {
                                string result = Api.DeleteFile(bucket,item.Key);
                                if (result==null) {
                                    toolTip1.SetToolTip(lablel, "删除成功");
                                    loadData();
                                }
                                toolTip1.SetToolTip(lablel, "删除失败"+result);
                            }                       
                        };
                        
                        ContextMenu contextMenu = new ContextMenu();
                        contextMenu.MenuItems.Add(menuItem);
                        contextMenu.MenuItems.Add(deleteMenuItem);
                        filePanel.ContextMenu= contextMenu;
                        //filePanel.DoDragDrop(item, DragDropEffects.Copy);
                        //filePanel.DragLeave += delegate{
                        //    Console.WriteLine(123);
                        //    Console.WriteLine(dropFile.Key);
                        //};
                        //filePanel.DragEnter += delegate {
                        //    Console.WriteLine(234);
                        //    Console.WriteLine(dropFile.Key);
                        //};
                        this.flMain.Controls.Add(filePanel);
                    });
                }
                if (data.Result.CommonPrefixes != null)
                {
                    data.Result.CommonPrefixes.ForEach(item =>
                    {
                        FilePanel filePanel = new FilePanel(this.toolTip1, null);
                        Console.WriteLine(item);
                        Label lablel = (Label)filePanel.Controls.Find("lblName", true)[0];
                        lablel.Text = item.Replace("/", "");
                        PictureBox pictureBox = (PictureBox)filePanel.Controls.Find("pbImg", true)[0];
                        pictureBox.Image = folderImage;
                        filePanel.MouseDoubleClick += delegate
                          {
                              Label tmp = (Label)filePanel.Controls.Find("lblName", true)[0];
                              prefix += (tmp.Text) + "/";
                              loadData();
                              setTitle();
                              level.Push(item);
                          };
                        pictureBox.MouseDoubleClick += delegate
                        {
                            Label tmp = (Label)filePanel.Controls.Find("lblName", true)[0];
                            prefix += (tmp.Text) + "/";
                            loadData();
                            setTitle();
                            level.Push(item);
                        };
                        this.flMain.Controls.Add(filePanel);
                    });
                }

            });

        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (Settings.config.ContainsKey("defaultPath")) {
                dialog.SelectedPath = Settings.config["defaultPath"];
            }
            
            //dialog.RootFolder = Environment.SpecialFolder.Programs;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                Settings.add("defaultPath", foldPath);
                MenuItem menu = (MenuItem)sender;
                FileDesc fileDesc = (FileDesc)menu.Tag;
                string domain = Settings.getDomain(bucket);
                Console.WriteLine("http://" + domain + "/" + fileDesc.Key);
                string fileName = fileDesc.Key;
                if (fileName.IndexOf("/")>0) {
                    fileName = fileName.Substring(fileName.LastIndexOf("/"));
                }
                Api.DownloadFile("http://" + domain + "/" + fileDesc.Key, foldPath+"/" + fileName);
            }
           
            
        }

        public void setTitle()
        {
            if (bucket.Equals("")) {
                this.lblPath.Text = "";
            }
            else
            {
            this.lblPath.Text = bucket + ":" + prefix;
            }
        }
        private void frmMain_Move(object sender, EventArgs e)
        {
            frmBack.Location = this.Location;
            frmBack.Size = this.Size;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            frmBack.Location = this.Location;
            frmBack.Size = this.Size;
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            frmBack.Location = this.Location;
            frmBack.Size = this.Size;
        }

        private void frmMain_Deactivate(object sender, EventArgs e)
        {
        }


        private void lblMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void lblMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }

        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblClose_MouseEnter(object sender, EventArgs e)
        {
            this.lblClose.BackColor = Color.RoyalBlue;
        }

        private void lblClose_MouseLeave(object sender, EventArgs e)
        {
            this.lblClose.BackColor = Color.Transparent;
        }

        private void frmMain_TextChanged(object sender, EventArgs e)
        {
            this.lblTitle.Text = this.Text;
        }

        private void lblMin_MouseEnter(object sender, EventArgs e)
        {
            this.lblMin.BackColor = Color.AliceBlue;
        }

        private void lblMin_MouseLeave(object sender, EventArgs e)
        {
            this.lblMin.BackColor = Color.Transparent;
        }

        private void lblMax_MouseEnter(object sender, EventArgs e)
        {
            this.lblMax.BackColor = Color.AliceBlue;
        }

        private void lblMax_MouseLeave(object sender, EventArgs e)
        {
            this.lblMax.BackColor = Color.Transparent;
        }

        private void lblCannel_MouseEnter(object sender, EventArgs e)
        {
            this.lblCannel.BackColor = Color.AliceBlue;
        }

        private void lblCannel_MouseLeave(object sender, EventArgs e)
        {
            this.lblCannel.BackColor = Color.Transparent;
        }

        private void lblCannel_Click(object sender, EventArgs e)
        {
            if (level.Count > 1)
            {
                level.Pop();
                prefix = level.Peek();
                loadData();
                setTitle();
            }
            else
            {
                prefix = "";
                bucket = "";
                init();
                setTitle();
            }
        }



        int flag = 0;//0无操作1上传2下载
        //上传
        private void flMain_DragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine(e.ToString());

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
            flag = 1;

        }


        private void flMain_DragDrop(object sender, DragEventArgs e)
        {
            if (flag == 1) {
                if (bucket.Equals(""))
                {

                    return;
                }
                string fileName = (e.Data.GetData(DataFormats.FileDrop, false) as String[])[0];
                Console.WriteLine(fileName);
                Api.UploadFile(bucket, prefix+fileName.Substring(fileName.LastIndexOf("\\")+1), fileName);
                loadData();
               
            }
            flag = 0;
        }

        /// <summary>
        ///下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flMain_DragLeave(object sender, EventArgs e)
        {
            
            Console.WriteLine(e.ToString());
            flag = 2;
            Console.WriteLine(flag);
        }

        private void flMain_MouseDown(object sender, MouseEventArgs e)
        {
           
        }


        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
            { //Esc
                frmSetting frmSetting = new frmSetting();
                frmSetting.frmBack = frmBack;
                frmSetting.Show();
            }


        }
    }
}
