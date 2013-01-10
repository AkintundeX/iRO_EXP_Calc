using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Utilities;
using System.IO;

namespace iROClassicExp
{
    public partial class ExpForm : Form
    {
        private Logic logic;
        System.Windows.Forms.Timer logicTimer;


        public ExpForm()
        {
            KeyboardHook.Hook();
            KeyboardHook.KeyUp += new KeyEventHandler(KeyUp);
            KeyboardHook.KeyDown += new KeyEventHandler(KeyDown);
            try
            {
                using (StreamReader sr = new StreamReader("settings.dat"))
                {
                    Settings.ScreenshotKey = (Keys)Int32.Parse((sr.ReadLine()));
                    Settings.ResetKey = (Keys)Int32.Parse((sr.ReadLine()));
                    Settings.StopKey = (Keys)Int32.Parse((sr.ReadLine()));
                    Settings.PauseKey = (Keys)Int32.Parse((sr.ReadLine()));
                    Settings.WindowTitle = sr.ReadLine();
                    Settings.ClassicProcess = sr.ReadLine();
                    Settings.RenewalProcess = sr.ReadLine();
                }
            }
            catch (Exception)
            {
                Settings.ScreenshotKey = Keys.PrintScreen;
                Settings.ResetKey = Keys.Delete;
                Settings.StopKey = Keys.End;
                Settings.PauseKey = Keys.Pause;
                Settings.WindowTitle = "Ragnarok";
                Settings.ClassicProcess = "clragexe";
                Settings.RenewalProcess = "ragexe";
            }

            InitializeComponent();
            this.FormClosing += CloseWindow;

            ResetHotkeys();

            logicTimer = new System.Windows.Forms.Timer();
            logicTimer.Tick += Looper;
            logicTimer.Interval = (int)((1.0 / 60.0) * 1000.0);
            logicTimer.Start();
        }

        public new void KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        public new void KeyDown(object sender, KeyEventArgs e)
        {
            String title = WindowsAPI.GetActiveWindowTitle();
            // This should technically never get hit if they open RO after the fact
            // So no need to check if a process is open

            if (title != Settings.WindowTitle)
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.PrintScreen:
                    e.Handled = true;
                    logic.Screenshot(0, 0, false);
                    break;
                case Keys.LWin:
                    e.Handled = true;
                    break;
                case Keys.RWin:
                    e.Handled = true;
                    break;
            }
        }

        private void Looper(object sender, EventArgs e)
        {
            try
            {
                logic.Update();
                ResetUi(true);
            }
            catch (NullReferenceException)
            {
                ResetUi(false);
                logic = new Logic();
                int count = logic.CharacterCount;
                logic.Update();
                tabCharacter.TabPages.Clear();
                for (int i = 0; i < count; i++)
                {
                    tabCharacter.TabPages.Add(new TabPage(logic[i].Name));
                }
            }
            catch (ProcessNotFoundException)
            {
                ResetUi(false);
                logic.Dispose();
                logic = null;
            }
            catch (Exception ex)
            {
                if (!this.suppressExceptionsToolStripMenuItem.Checked)
                {
                    logicTimer.Stop();
                    MessageBox.Show("An error has occured\n" + ex.Message);
                    MessageBox.Show(ex.StackTrace);
                }
                logic.Dispose();
                ResetUi(false);
                return;
            }
        }

        private void CloseWindow()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("settings.dat"))
                {
                    sw.WriteLine((int)Settings.ScreenshotKey);
                    sw.WriteLine((int)Settings.ResetKey);
                    sw.WriteLine((int)Settings.StopKey);
                    sw.WriteLine((int)Settings.PauseKey);
                    sw.WriteLine(Settings.WindowTitle);
                    sw.WriteLine(Settings.ClassicProcess);
                    sw.WriteLine(Settings.RenewalProcess);
                }
            }
            catch (Exception)
            { }
            logic.Dispose();
            KeyboardHook.Unhook();
        }

        private void CloseWindow(object s, EventArgs e)
        {
            CloseWindow();
        }

        private void ResetUi(bool actual)
        {
            if (!actual)
            {
                this.textName.Text = "Name";
                this.textHp.Text = "-/-";
                this.textSp.Text = "-/-";
                this.textLevel.Text = "-/-";
                this.textExp.Text = "-/-";
                this.textExpPerHour.Text = "0";
                this.textTime.Text = "00:00:00";
                this.textTotalExp.Text = "0";
                this.textWeight.Text = "-/-";
                this.textJobExp.Text = "-/-";
                this.textTotalJob.Text = "0";
                this.textJobPerHour.Text = "0";
                this.textHpPercent.Text = "(0.00%)";
                this.textSpPercent.Text = "(0.00%)";
                this.textWeightPercent.Text = "(0.00%)";
                this.textExpPercent.Text = "(0.00%)";
                this.textJobPercent.Text = "(0.00%)";
                this.Text = String.Format("iRO EXP - Disengaged");
            }
            else
            {
                this.textName.Text = logic[tabCharacter.SelectedIndex].Name;
                this.textHp.Text = String.Format("{0:n0}/{1:n0}", logic[tabCharacter.SelectedIndex].CurrentHp, logic[tabCharacter.SelectedIndex].MaxHp);
                this.textSp.Text = String.Format("{0:n0}/{1:n0}", logic[tabCharacter.SelectedIndex].CurrentSp, logic[tabCharacter.SelectedIndex].MaxSp);
                this.textLevel.Text = String.Format("{0:n0}/{1:n0}", logic[tabCharacter.SelectedIndex].BaseLevel, logic[tabCharacter.SelectedIndex].JobLevel);
                this.textExp.Text = String.Format("{0:n0}/{1:n0}", logic[tabCharacter.SelectedIndex].CurrentExp, logic[tabCharacter.SelectedIndex].ExpToNext);
                this.textExpPerHour.Text = String.Format("{0:n0}/hr", logic[tabCharacter.SelectedIndex].ExpPerHour);
                this.textTime.Text = String.Format("{0:n0}:{1}:{2}", (int)logic[tabCharacter.SelectedIndex].Time.TotalHours, logic[tabCharacter.SelectedIndex].Time.Minutes.ToString("00"), logic[tabCharacter.SelectedIndex].Time.Seconds.ToString("00"));
                this.textTotalExp.Text = String.Format("{0:n0}", logic[tabCharacter.SelectedIndex].TotalExp);
                this.textWeight.Text = String.Format("{0:n0}/{1:n0}", logic[tabCharacter.SelectedIndex].CurrentWeight, logic[tabCharacter.SelectedIndex].MaxWeight);
                this.textJobExp.Text = String.Format("{0:n0}/{1:n0}", logic[tabCharacter.SelectedIndex].CurrentJob, logic[tabCharacter.SelectedIndex].JobToNext);
                this.textTotalJob.Text = String.Format("{0:n0}", logic[tabCharacter.SelectedIndex].TotalJob);
                this.textJobPerHour.Text = String.Format("{0:n0}/hr", logic[tabCharacter.SelectedIndex].JobPerHour);
                this.textHpPercent.Text = String.Format("({0:n2}%)", logic[tabCharacter.SelectedIndex].HpPercent);
                this.textSpPercent.Text = String.Format("({0:n2}%)", logic[tabCharacter.SelectedIndex].SpPercent);
                this.textWeightPercent.Text = String.Format("({0:n2}%)", logic[tabCharacter.SelectedIndex].WeightPercent);
                this.textExpPercent.Text = String.Format("({0:n2}%)", logic[tabCharacter.SelectedIndex].ExpPercent);
                this.textJobPercent.Text = String.Format("({0:n2}%)", logic[tabCharacter.SelectedIndex].JobPercent);
                this.Text = String.Format("iRO {1} EXP - Engaged - {0}", logic[tabCharacter.SelectedIndex].MapName, logic[tabCharacter.SelectedIndex].Client.ToString());
                this.textBaseEstimate.Text = String.Format("{0:n0}:{1}:{2}", (int)logic[tabCharacter.SelectedIndex].BaseEstimate.TotalHours, logic[tabCharacter.SelectedIndex].BaseEstimate.Minutes.ToString("00"), logic[tabCharacter.SelectedIndex].BaseEstimate.Seconds.ToString("00"));
                this.textJobEstimate.Text = String.Format("{0:n0}:{1}:{2}", (int)logic[tabCharacter.SelectedIndex].JobEstimate.TotalHours, logic[tabCharacter.SelectedIndex].JobEstimate.Minutes.ToString("00"), logic[tabCharacter.SelectedIndex].JobEstimate.Seconds.ToString("00"));
            }
        }
        private void screenOnLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.screenOnLevelToolStripMenuItem.Checked = !this.screenOnLevelToolStripMenuItem.Checked;
            logic.AutoScreen = this.screenOnLevelToolStripMenuItem.Checked;
        }

        private void suppressExceptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.suppressExceptionsToolStripMenuItem.Checked = !this.suppressExceptionsToolStripMenuItem.Checked;
        }

        private void profileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.profileToolStripMenuItem.Checked = !this.profileToolStripMenuItem.Checked;
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logic.Reset(tabCharacter.SelectedIndex);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logic.Stop(tabCharacter.SelectedIndex);
        }

        private void enableScreenshotterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableScreenshotterToolStripMenuItem.Checked = !enableScreenshotterToolStripMenuItem.Checked;
            ResetHotkeys();
        }

        private void disableWindowsKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableWindowsKeysToolStripMenuItem.Checked = !disableWindowsKeysToolStripMenuItem.Checked;
            ChangeWindowsKey(disableWindowsKeysToolStripMenuItem.Checked);
            ResetHotkeys();
        }

        public void ChangeWindowsKey(bool enable)
        {
            if (enable)
            {
                if (!KeyboardHook.HookedKeys.Contains(Keys.LWin))
                    KeyboardHook.HookedKeys.Add(Keys.LWin);
                if (!KeyboardHook.HookedKeys.Contains(Keys.RWin))
                    KeyboardHook.HookedKeys.Add(Keys.RWin);
            }
            else
            {
                KeyboardHook.HookedKeys.Remove(Keys.LWin);
                KeyboardHook.HookedKeys.Remove(Keys.RWin);
            }
        }

        private void resetAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logic.ResetAll();
        }

        private void stopAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logic.StopAll();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EXPAboutBox about = new EXPAboutBox();
            about.ShowDialog();
        }

        private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm form = new SettingsForm(ResetHotkeys);
            form.ShowDialog();
        }

        private void enableHotkeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.enableHotkeysToolStripMenuItem.Checked = !this.enableHotkeysToolStripMenuItem.Checked;
            ResetHotkeys();
        }

        private void ResetHotkeys()
        {
            KeyboardHook.HookedKeys.Clear();
            if (enableScreenshotterToolStripMenuItem.Checked)
            {
                KeyboardHook.HookedKeys.Add(Settings.ScreenshotKey);
            }
            if (enableHotkeysToolStripMenuItem.Checked)
            {
                KeyboardHook.HookedKeys.Add(Settings.StopKey);
                KeyboardHook.HookedKeys.Add(Settings.ResetKey);
                KeyboardHook.HookedKeys.Add(Settings.PauseKey);
            }
            if (disableWindowsKeysToolStripMenuItem.Checked)
            {
                KeyboardHook.HookedKeys.Add(Keys.LWin);
                KeyboardHook.HookedKeys.Add(Keys.RWin);
            }
        }
    }
}
