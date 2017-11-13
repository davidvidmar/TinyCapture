using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TinyCapture
{
    public class ScreenCapture
    {
        private const string Extension = ".png";

        public struct RECT
        {
            public int Left;      
            public int Top;       
            public int Right;     
            public int Bottom;    
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWINFO
        {
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public WINDOWINFO(Boolean? filler)
                : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
            {
                cbSize = (UInt32)(Marshal.SizeOf(typeof(WINDOWINFO)));
            }

        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public static Bitmap CaptureScreen()
        {
            var bounds = Screen.GetBounds(Point.Empty);
            var bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }
            return bitmap;
        }

        public static string CaptureScreenToFile(string filename)
        {
            Debug.WriteLine("Capturing screen.");
            var bitmap = CaptureScreen();
            if (filename == null)
            {
                filename = Environment.MachineName;
            }
            Debug.WriteLine("Filename = " + filename);
            filename = GetUniqueFilename(Settings.SavePath, filename, Extension);
            Debug.WriteLine("Unique Filename = " + filename);            
            bitmap.Save(filename, ImageFormat.Png);
            return filename;
        }

        public static string CaptureScreenToFile()
        {                                   
            return CaptureScreenToFile(null);
        }

        public static Bitmap CaptureWindow(IntPtr handle)
        {
            Debug.WriteLine("Capturing window.");
            var info = new WINDOWINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);

            GetWindowInfo(handle, ref info);

            var bitmap = new Bitmap(info.rcWindow.Right - info.rcWindow.Left, info.rcWindow.Bottom - info.rcWindow.Top);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(info.rcWindow.Left, info.rcWindow.Top, 0, 0, new Size(info.rcWindow.Right - info.rcWindow.Left, info.rcWindow.Bottom - info.rcWindow.Top));
            }
            return bitmap;
        }

        public static string CaptureWindowToFile(IntPtr handle, string filename)
        {
            var bitmap = CaptureWindow(handle);
            if (filename == null)
            {
                filename = GetWindowText(handle);
            }
            Debug.WriteLine("Filename = " + filename);
            filename = GetUniqueFilename(Settings.SavePath, filename, Extension);
            Debug.WriteLine("Unique Filename = " + filename);
            bitmap.Save(filename, ImageFormat.Png);
            return filename;
        }

        public static string CaptureWindowToFile(IntPtr handle)
        {
            return CaptureWindowToFile(handle, null);
        }

        public static string CaptureCurrentWindowToFile(string filename)
        {
            return CaptureWindowToFile(GetForegroundWindow(), filename);
        }

        public static string CaptureCurrentWindowToFile()
        {            
            return CaptureWindowToFile(GetForegroundWindow(), null);
        }

        private static string GetWindowText(IntPtr handle)
        {
            var stringBuilder = new StringBuilder(100);
            try
            {
                GetWindowText(handle, stringBuilder, 100);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return stringBuilder.Length > 0 ? stringBuilder.ToString() : "Window";
        }

        private static string GetUniqueFilename(string path, string filename, string extension)
        {
            var counter = 0;
            string fullPath;

            filename = CleanFileName(filename);

            do
            {
                fullPath = Path.Combine(path, filename + (counter == 0 ? "" : "_" + counter) + extension);
                counter++;
            } while (File.Exists(fullPath));

            return fullPath;
        }

        private static string CleanFileName(string filename)
        {
            return filename.Replace(':', '-');
        }
    }
}
