using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Windows.Forms;

namespace Shadowsocks.View
{
    class HotkeyManagement
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt32 RegisterHotKey(
            IntPtr hWnd,                
            int id,                     
            KeyModifiers fsModifiers,   
            Keys vk                     
            );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
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

        internal static void register(IntPtr handle)
        {
            RegisterHotKey(handle, 508, KeyModifiers.Ctrl, Keys.F12);
        }
        internal static void unregister(IntPtr handle)
        {
            UnregisterHotKey(handle, 508);
        }
    }
}