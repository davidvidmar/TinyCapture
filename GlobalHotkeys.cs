using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace TinyCapture
{
    /// <summary> This class allows you to manage a hotkey </summary>
    public class GlobalHotkeys : IDisposable
    {
        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hwnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32", SetLastError = true)]
        public static extern int UnregisterHotKey(IntPtr hwnd, int id);
        [DllImport("kernel32", SetLastError = true)]
        public static extern short GlobalAddAtom(string lpString);
        [DllImport("kernel32", SetLastError = true)]
        public static extern short GlobalDeleteAtom(short nAtom);
  
       
        public static int WM_HOTKEY = 0x312;       

        public GlobalHotkeys()
        {
            Handle = Process.GetCurrentProcess().Handle;
        }

        /// <summary>Handle of the current process</summary>
        public IntPtr Handle;

        /// <summary>The ID for the hotkey</summary>
        public List<short> HotkeyIDs = new List<short>();

        /// <summary>Register the hotkey</summary>
        public int RegisterGlobalHotKey(int hotkey, int modifiers, IntPtr handle)
        {            
            Handle = handle;
            return RegisterGlobalHotKey(hotkey, modifiers);
        }

        /// <summary>Register the hotkey</summary>
        public int RegisterGlobalHotKey(int hotkey, int modifiers)
        {
            try
            {
                // use the GlobalAddAtom API to get a unique ID (as suggested by MSDN)
                var atomName = Thread.CurrentThread.ManagedThreadId.ToString("X8") + GetType().FullName;
                var hotkeyID = GlobalAddAtom(atomName + hotkey + modifiers);
                if (hotkeyID == 0)
                    throw new Exception("Unable to generate unique hotkey ID. Error: " + Marshal.GetLastWin32Error());

                // register the hotkey, throw if any error
                if (!RegisterHotKey(Handle, hotkeyID, (uint)modifiers, (uint)hotkey))
                    throw new Exception("Unable to register hotkey. Error: " + Marshal.GetLastWin32Error());

                HotkeyIDs.Add(hotkeyID);
                Debug.WriteLine("ID = " + hotkeyID);
                return hotkeyID;
            }
            catch (Exception ex)
            {
                // clean up if hotkey registration failed
                Dispose();
                Console.WriteLine(ex);
            }
            return 0;
        }

        /// <summary>Unregister the hotkey</summary>
        public void UnregisterGlobalHotKey()
        {
            foreach (var hotkeyID in HotkeyIDs)
            {
                if (hotkeyID == 0) continue;
                UnregisterHotKey(Handle, hotkeyID);
                // clean up the atom list
                GlobalDeleteAtom(hotkeyID);
            }
            HotkeyIDs.Clear();
        }

        public void Dispose()
        {
            UnregisterGlobalHotKey();
        }
    }
}