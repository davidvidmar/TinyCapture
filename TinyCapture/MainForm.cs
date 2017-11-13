using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TinyCapture.Helper;

namespace TinyCapture
{
    public partial class MainForm : Form
    {
        private GlobalHotkeys _hotkeys;

        private int _captureWindowKey;
        private int _captureScreenKey;

        private static readonly string HelpText =
            string.Format(
                "Minimalistic screen capture program.\n\n" +
                "Pressing {0} + {1} will capture current window.\nPressing {0} + {2} win capture entire screen.\n\n" +
                "Images will be saved to {3}.",
                Settings.KeyModifier, Settings.CaptureWindowKey, Settings.CaptureScreenKey, Settings.SavePath);

        public MainForm()
        {
            InitializeComponent();

            notifyIcon.BalloonTipText = HelpText;            
        }

        protected override void OnLoad(EventArgs e)
        {
            _hotkeys = new GlobalHotkeys();
            base.OnLoad(e);

            _captureWindowKey = _hotkeys.RegisterGlobalHotKey((int)Settings.CaptureWindowKey, (int)Settings.KeyModifier, Handle);
            Debug.WriteLine("Registered hotkey: " + _captureWindowKey);

            _captureScreenKey = _hotkeys.RegisterGlobalHotKey((int)Settings.CaptureScreenKey, (int)Settings.KeyModifier, Handle);
            Debug.WriteLine("Registered hotkey: " + _captureScreenKey);

            Visible = false;

            if (_captureWindowKey == 0 || _captureScreenKey == 0)
            {
                MessageBox.Show(
                    string.Format("Sorry, could not register hotkeys.\nProbably another software has {2} + {0} or {2} + {1} hooked.",
                    Settings.CaptureWindowKey, Settings.CaptureScreenKey, Settings.KeyModifier),
                    "What a shame...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg != GlobalHotkeys.WM_HOTKEY) return;

            Debug.WriteLine("Hotkey caught.");

            if ((int)m.WParam == _captureWindowKey)
            {
                try
                {                    
                    var filename = ScreenCapture.CaptureCurrentWindowToFile();
                    if (Settings.ShowTooltip)
                    {
                        notifyIcon.BalloonTipText = string.Format("Captured window to {0}", Path.GetFileName(filename));
                        notifyIcon.ShowBalloonTip(10);
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Something went wrong: {0}", ex.Message), "Ups!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if ((int)m.WParam == _captureScreenKey)
                try
                {
                    var filename = ScreenCapture.CaptureScreenToFile();
                    if (Settings.ShowTooltip)
                    {
                        notifyIcon.BalloonTipText = string.Format("Captured screen to {0}", Path.GetFileName(filename));
                        notifyIcon.ShowBalloonTip(10);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Something went wrong: {0}", ex.Message), "Ups!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (e.Cancel) return;
            _hotkeys.UnregisterGlobalHotKey();            
        }

        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == closeMenuItem) Close();
            if (e.ClickedItem == createDefaultSettingsMenuItem)
            {
                if (!Settings.Write())
                {
                    MessageBox.Show("Could not create settings file. Is folder writable?", "Ups!", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (e.ClickedItem == whatIsThisMenuItem)
            {
                MessageBox.Show(HelpText, "This is it!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            if (e is MouseEventArgs args && args.Button == MouseButtons.Left)
                notifyIcon.ShowBalloonTip(100);
        }

        private void startWithWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (AutoRunHelper.Get())
            {
                AutoRunHelper.Clear();
                menu.Checked = false;
            }
            else
            {
                AutoRunHelper.Set();
                menu.Checked = true;
            }
        }
    }
}
