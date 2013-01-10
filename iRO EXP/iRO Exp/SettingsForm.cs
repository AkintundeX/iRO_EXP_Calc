﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace iROClassicExp
{
    public partial class SettingsForm : Form
    {
        public static readonly Keys[] Keymap = { Keys.PrintScreen, Keys.Scroll, Keys.Pause, Keys.Insert, Keys.Delete,
                                               Keys.Home, Keys.End, Keys.PageUp, Keys.PageDown};

        public delegate void SettingsCallBack();
        private SettingsCallBack callback;

        public SettingsForm(SettingsCallBack callback)
        {
            InitializeComponent();
            this.comboPause.SelectedIndex = Array.IndexOf(Keymap, Settings.PauseKey);
            this.comboReset.SelectedIndex = Array.IndexOf(Keymap, Settings.ResetKey);
            this.comboScreenshotKey.SelectedIndex = Array.IndexOf(Keymap, Settings.ScreenshotKey);
            this.comboStop.SelectedIndex = Array.IndexOf(Keymap, Settings.StopKey);
            this.textWindowTitle.Text = Settings.WindowTitle;
            this.textClassicProcess.Text = Settings.ClassicProcess;
            this.textRenewalProcess.Text = Settings.RenewalProcess;
            this.callback = callback;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Settings.ScreenshotKey = Keymap[this.comboScreenshotKey.SelectedIndex];
            Settings.PauseKey = Keymap[this.comboPause.SelectedIndex];
            Settings.StopKey = Keymap[this.comboStop.SelectedIndex];
            Settings.ResetKey = Keymap[this.comboReset.SelectedIndex];
            Settings.WindowTitle = textWindowTitle.Text;
            Settings.ClassicProcess = this.textClassicProcess.Text;
            Settings.RenewalProcess = this.textRenewalProcess.Text;
            callback();
            //this.Close();
        }
    }
}
