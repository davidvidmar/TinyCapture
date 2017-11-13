using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TinyCapture
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.whatIsThisMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createDefaultSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuLine = new System.Windows.Forms.ToolStripSeparator();
            this.closeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "...";
            this.notifyIcon.BalloonTipTitle = "TinyCapture";
            this.notifyIcon.ContextMenuStrip = this.contextMenu;
            this.notifyIcon.Text = "TinyCapture";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.whatIsThisMenuItem,
            this.createDefaultSettingsMenuItem,
            this.toolStripMenuLine,
            this.closeMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(212, 76);
            this.contextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenu_ItemClicked);
            // 
            // whatIsThisMenuItem
            // 
            this.whatIsThisMenuItem.Name = "whatIsThisMenuItem";
            this.whatIsThisMenuItem.Size = new System.Drawing.Size(211, 22);
            this.whatIsThisMenuItem.Text = "What is this?";
            // 
            // createDefaultSettingsMenuItem
            // 
            this.createDefaultSettingsMenuItem.Name = "createDefaultSettingsMenuItem";
            this.createDefaultSettingsMenuItem.Size = new System.Drawing.Size(211, 22);
            this.createDefaultSettingsMenuItem.Text = "Create default settings file";
            // 
            // toolStripMenuLine
            // 
            this.toolStripMenuLine.Name = "toolStripMenuLine";
            this.toolStripMenuLine.Size = new System.Drawing.Size(208, 6);
            // 
            // closeMenuItem
            // 
            this.closeMenuItem.Name = "closeMenuItem";
            this.closeMenuItem.Size = new System.Drawing.Size(211, 22);
            this.closeMenuItem.Text = "Close TinyCapture";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(120, 23);
            this.ControlBox = false;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TinyCapture";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem closeMenuItem;
        private ToolStripMenuItem whatIsThisMenuItem;
        private ToolStripMenuItem createDefaultSettingsMenuItem;
        private ToolStripSeparator toolStripMenuLine;
    }
}

