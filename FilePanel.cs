using Qiniu.RS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenNiu
{
    public partial class FilePanel : UserControl
    {
        public ToolTip toolTip;


        public FilePanel(ToolTip toolTip, FileDesc fileDesc)
        {

            InitializeComponent();
            this.Tag = fileDesc;
            this.toolTip = toolTip;
            Console.WriteLine();
            for (int i = 0; i < this.Controls.Count; i++)
            {
                this.Controls[i].MouseEnter += this.FilePanel_MouseEnter;
                this.Controls[i].MouseLeave += this.FilePanel_MouseLeave;
                this.Controls[i].MouseHover += this.FilePanel_MouseHover;
                toolTip.SetToolTip(this.Controls[i], getTip());
               
            }
            if (fileDesc != null)
            {
                this.lblName.MouseDoubleClick +=delegate {
                    Clipboard.SetText(this.lblName.Text);
                    this.toolTip.SetToolTip(this.lblName,"复制成功");
                };
            }

        }

        private void FilePanel_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.AliceBlue;
        }

        private void FilePanel_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Transparent;
        }

        private void FilePanel_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(this, getTip());
        }
   

        public string getTip() {
            FileDesc fileDesc = (FileDesc)this.Tag;
            if (fileDesc==null)
            {
                return this.lblName.Text;
            }
            string txt = "";
            txt += "文件名称：" + fileDesc.Key;
            txt += "\n文件大小：" + getFileSize(fileDesc.Fsize);
            txt += "\n文件类型：" + fileDesc.MimeType;
            long unixTimeStamp = fileDesc.PutTime/10000000;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(unixTimeStamp);
            txt += "\n创建时间：" + dt.ToString("yyyy-MM-dd HH:mm:ss");
            return txt;
        }

        public string getFileSize(long data) {
            if (data < 1200)
            {
                return data.ToString("#.##") + " B";
            }
            else if (data < 1200 * 1024)
            {
                return (data/1024).ToString("#.##") + " KB";
            }
            else if (data < 1200 * 1024 * 1024)
            {
                return (data / 1024/1024).ToString("#.##") + " MB";
            }
            else if (data < 1099511627776)
            {
                return (data / 1024 / 1024 /1024).ToString("#.##") + " GB";
            }
            else
            {
                return (data / 1024 / 1024/1024/1024).ToString("#.##") + " TB";
            }
        }

   
    }
}
