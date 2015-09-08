using Shadowsocks.Controller;
using Shadowsocks.Properties;
using Shadowsocks.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Shadowsocks.Hotkey
{
    class RegHotkey
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotkey(
            IntPtr hWnd,                //要定义热键的窗口的句柄
            int id,                     //定义热键ID（不能与其它ID重复）           
            KeyModifiers fsModifiers,   //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效
            Keys vk                     //定义热键的内容
            );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotkey(
            IntPtr hWnd,
            int id
            );
        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            WindowsKey = 8,
            Shift = 4
        }
    }
}
namespace Shadowsocks
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Util.Utils.ReleaseMemory();
            using (Mutex mutex = new Mutex(false, "Global\\" + "71981632-A427-497F-AB91-241CD227EC1F"))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                if (!mutex.WaitOne(0, false))
                {
                    Process[] oldProcesses = Process.GetProcessesByName("Shadowsocks");
                    if (oldProcesses.Length > 0)
                    {
                        Process oldProcess = oldProcesses[0];
                    }
                    MessageBox.Show("Shadowsocks is already running.\n\nFind Shadowsocks icon in your notify tray.");
                    return;
                }
      
                Directory.SetCurrentDirectory(Application.StartupPath);
#if !DEBUG
                Logging.OpenLogFile();
#endif
                ShadowsocksController controller = new ShadowsocksController();

                MenuViewController viewController = new MenuViewController(controller);
                viewController.initialize(viewController);
                try
                {
                    if (!File.Exists("config.ini"))
                    {
                        File.Create("config.ini");
                    }
                    FileStream file = File.Open("config.ini", FileMode.Open);
                    using (StreamReader sr = new StreamReader(file))
                    {
                        String str = sr.ReadLine();
                        str = str == null ? "":str;
                        str.Trim();
                        if (str.StartsWith("T")) 
                        {
                            viewController.changeCheckboxState(true);
                          //  sr.Close();
                        }
                        else
                        {
                            file.SetLength(0);
                            StreamWriter sw = new StreamWriter(file);
                            sw.WriteLine("False");
                            sw.Flush();
                            //sw.Close();
                        }
                       file.Close();
                    }
                }
                catch (IOException err)
                {
                    Console.Error.WriteLine(err);
                }
                controller.Start();
                Application.Run();
            }
        }
    }
}
