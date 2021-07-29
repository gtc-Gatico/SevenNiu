using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenNiu
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Settings.init();
            if (Settings.isInit())
            {
                Application.Run(new frmMain());
            }
            else
            {
                Application.Run(new frmSetting());
                ShortcutCreator.CreateShortcutOnDesktop("七牛云客户端", Application.ExecutablePath, "七牛云客户端，仅供参考学习", Application.ExecutablePath);
            }

        }
    }
}
