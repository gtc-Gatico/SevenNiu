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
    public partial class frmBack : Form
    {
        public frmBack()
        {
            InitializeComponent();
        }

        private void frmBack_Load(object sender, EventArgs e)
        {
            this.pbBack.Image = Image.FromFile(AppContext.BaseDirectory+ "res/background.jpg");
        }

        
    }
}
