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
    public partial class frmSetting : Form
    {
        public frmBack frmBack;
        Image oldImg = null;
        double opacity = 0;
        public frmSetting( )
        {

             InitializeComponent();

        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            txtAccessKey.Text = Settings.AccessKey;
            txtSecretKey.Text = Settings.SecretKey;
            if (Settings.config.ContainsKey("background")) {
                this.pbBack.Image = Image.FromFile(Settings.config["background"]);
                this.pbBack.Tag = Settings.config["background"];
                oldImg = this.pbBack.Image;
            }
            if (Settings.config.ContainsKey("opacity")) {
                this.tbOpacity.Value = Convert.ToInt32(Settings.config["opacity"]);
                opacity = Convert.ToDouble(Settings.config["opacity"]);
            }
            
        }

        Boolean isOk = false;
        private void btnOk_Click(object sender, EventArgs e)
        {
           
            if (txtAccessKey.TextLength > 0)
            {

                Settings.add("AccessKey", txtAccessKey.Text);
            }
            if (txtSecretKey.TextLength > 0)
            {
                Settings.add("SecretKey", txtSecretKey.Text);
            }
            if (this.pbBack.Image!=null) {
                Settings.add("background", this.pbBack.Tag.ToString());
            }
            Settings.add("opacity", this.tbOpacity.Value.ToString());
           


            Console.WriteLine(Settings.AccessKey);
            Console.WriteLine(Settings.SecretKey);
            MessageBox.Show("设置成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isOk = true;
            if (!Settings.isInit()) {
                Application.Restart();
            }
            
        }
        
        private void pbBack_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Title = "请选择背景图片";
            file.Filter = "图片文件(*.jpg,*.gif,*.bmp)|*.jpg;*.gif;*.bmp";
            if (file.ShowDialog() == DialogResult.OK)
            {
                string path = file.FileName;
                this.pbBack.Image = Image.FromFile(path);
                this.pbBack.Tag = path;
                if (frmBack != null) {
                   
                    frmBack.pbBack.Image = this.pbBack.Image;
                }
            }
        }

        private void frmSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine(opacity);
            Console.WriteLine(oldImg);
            if (frmBack != null && !isOk)
            {
                frmBack.pbBack.Image = oldImg;
                frmBack.Opacity = Convert.ToDouble(opacity)  / 100;
            }
        }

        private void tbOpacity_Scroll(object sender, EventArgs e)
        {
           
            if (frmBack != null)
            {
               
                frmBack.Opacity = Convert.ToDouble(this.tbOpacity.Value) / 100;
            }
        }
    }
}
