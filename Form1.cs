using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReStartRemoateComputer
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Windos登陆辅助
        /// </summary>
        internal static class WinLogonHelper
        {
            /// <summary>
            /// 模拟windows登录域
            /// </summary>
            [DllImport("advapi32.DLL", SetLastError = true)]
            public static extern int LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        }
            /// <summary>
            /// 远程重启电脑
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void 重启电脑_Click(object sender, EventArgs e)
           {

            //192.168.199.203
            //10.163.65.118
            //定义连接远程计算机的一些选项
            ConnectionOptions options = new ConnectionOptions();
            options.Username = "Administrator";
            options.Password = "123";
            ManagementScope scope = new ManagementScope("\\\\192.168.199.170\\root\\cimv2", options);
            try
            {
                //用给定管理者用户名和口令连接远程的计算机
                scope.Connect();
                System.Management.ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher query1 = new ManagementObjectSearcher(scope, oq);
                //得到WMI控制
                ManagementObjectCollection queryCollection1 = query1.Get();
                foreach (ManagementObject mo in queryCollection1)
                {
                    string[] ss = { "" };
                    //重启远程计算机
                    mo.InvokeMethod("Reboot", ss);
                }
                IntPtr admin_token = default(IntPtr);
                WindowsIdentity wid_admin = null;
                WindowsImpersonationContext wic = null;

                //在程序中模拟域帐户登录
                if (WinLogonHelper.LogonUser("JV913MS", "admin","Daliangss847", 9, 0, ref admin_token) != 0)
                {
                    using (wid_admin = new WindowsIdentity(admin_token))
                    {
                        using (wic = wid_admin.Impersonate())
                        {
                            
                        }
                    }
                }
            }
            //报错
            catch (Exception ee)
            {
                MessageBox.Show("连接" + textBox1.Text + "出错，出错信息为：" + ee.Message);
                Console.WriteLine("错误信息为{0}",ee.Message);
            }
        }
    }
}
