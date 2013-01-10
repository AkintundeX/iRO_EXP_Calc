using System;
using System.Collections.Generic;
using System.Text;
using ReadWriteMemory;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace iROClassicExp
{
    class Logic
    {

        private const int JOB_99_EXP = 999999999;

        private const int MAP_NAME_SIZE = 10;
        private const int MAX_LEN = 24;
        private const int MAX_MAP_NAME_LEN = 10;

        private bool autoScreen;

        private List<Character> characters;

        public Character this[int i]
        {
            get { return characters[i]; }
        }

        public bool AutoScreen
        {
            get { return autoScreen; }
            set { autoScreen = value; }
        }

        public Logic()
        {
            int processCount = ProcessMemory.ProcessCount(Settings.ClassicProcess);
            characters = new List<Character>();
            for (int i = 0; i < processCount; i++)
                characters.Add(new Character(i, Client.Classic));
            processCount = ProcessMemory.ProcessCount(Settings.RenewalProcess);
            for (int i = 0; i < processCount; i++)
                characters.Add(new Character(i, Client.Renewal));
        }

        public int CharacterCount
        {
            get { return characters.Count; }
        }

        public void Update()
        {
            if (characters == null || characters.Count == 0)
                throw new ProcessNotFoundException("lol, no.");
            for (int i = 0; i < characters.Count; i++)
            {
                if (this[i].Memory == null)
                    throw new ProcessNotFoundException("Never attached");
                if (!this[i].Memory.CheckProcess())
                    throw new ProcessNotFoundException("Process not found.");

                bool ignoreUpdates = String.IsNullOrEmpty(this[i].Name);
                string curName = this[i].Name;

                ReadBasicStats(i);
                if (this[i].Name != curName)
                {
                    this.Reset(i);
                    continue;
                }
                // Make temporary holders
                int curExp = this[i].CurrentExp;
                int curJob = this[i].CurrentJob;
                int curBaseLvl = this[i].BaseLevel;
                int curJobLvl = this[i].JobLevel;

                this[i].BaseLevel = this[i].Memory.ReadInt(this[i].BASE_LEVEL);
                this[i].JobLevel = this[i].Memory.ReadInt(this[i].JOB_LEVEL);
                this[i].CurrentExp = this[i].Memory.ReadInt(this[i].EXP);
                this[i].CurrentJob = this[i].Memory.ReadInt(this[i].JOB_EXP);
                bool updateKillTime = false;

                if (this[i].BaseLevel - curBaseLvl > 1 || this[i].JobLevel - curJobLvl > 1)
                {
                    this.Reset(i);
                    continue;
                }

                if (this[i].IsStopped && (this[i].CurrentExp != curExp || this[i].CurrentJob != curJob || this[i].BaseLevel != curBaseLvl || this[i].JobLevel != curJobLvl))
                {
                    Reset(i);
                    continue;
                }

                if (!ignoreUpdates || !this[i].IsStopped)
                {
                    if (curBaseLvl != this[i].BaseLevel && curBaseLvl > 0)
                    {
                        this[i].TotalExp += (this[i].ExpToNext - curExp) + this[i].CurrentExp;
                        updateKillTime = true;
                        this[i].LeveledUp = true;
                    }
                    else if (this[i].CurrentExp != curExp)
                    {
                        this[i].TotalExp += (this[i].CurrentExp - curExp);
                        updateKillTime = true;
                    }
                    if (curJobLvl != this[i].JobLevel)
                    {
                        this[i].TotalJob += (this[i].JobToNext - curJob) + this[i].CurrentJob;
                        updateKillTime = true;
                    }
                    else if (this[i].CurrentJob != curJob)
                    {
                        this[i].TotalJob += (this[i].CurrentJob - curJob);
                        updateKillTime = true;
                    }

                    if (updateKillTime)
                        UpdateKillTime(i);

                    UpdateToNext(i);
                    UpdatePerHour(i, false);

                    if (this[i].LeveledUp)
                    {
                        if (autoScreen)
                        {
                            this[i].LevelUpScreens++;
                            try
                            {
                                if (this[i].LevelUpScreens % 3 == 0)
                                {
                                    Screenshot(this[i].LevelUpScreens, i, true);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                    if (this[i].LevelUpScreens > 30)
                    {
                        this[i].LeveledUp = false;
                        this[i].LevelUpScreens = 0;
                    }
                }

                this[i].ExpPercent = this[i].ExpToNext == 0 ? 0 : (float)this[i].CurrentExp * 100.0f / (float)this[i].ExpToNext;
                this[i].JobPercent = this[i].JobToNext == 0 ? 0 : (float)this[i].CurrentJob * 100.0f / (float)this[i].JobToNext;
            }
        }

        public void ReadBasicStats(int i)
        {
            this[i].Name = this[i].Memory.ReadStringAscii(this[i].NAME, MAX_LEN);
            this[i].CurrentHp = this[i].Memory.ReadInt(this[i].HP);
            this[i].MaxHp = this[i].Memory.ReadInt(this[i].MAX_HP);
            this[i].CurrentSp = this[i].Memory.ReadInt(this[i].SP);
            this[i].MaxSp = this[i].Memory.ReadInt(this[i].MAX_SP);
            this[i].CurrentWeight = this[i].Memory.ReadInt(this[i].WEIGHT);
            this[i].MaxWeight = this[i].Memory.ReadInt(this[i].MAX_WEIGHT);
            this[i].MapName = this[i].Memory.ReadStringAscii(this[i].MAP_NAME, MAX_MAP_NAME_LEN);

            this[i].HpPercent = this[i].MaxHp == 0 ? 0 : (float)this[i].CurrentHp * 100.0f / (float)this[i].MaxHp;
            this[i].SpPercent = this[i].MaxSp == 0 ? 0 : (float)this[i].CurrentSp * 100.0f / (float)this[i].MaxSp;
            this[i].WeightPercent = this[i].MaxWeight == 0 ? 0 : (float)this[i].CurrentWeight * 100.0f / (float)this[i].MaxWeight;
        }

        public void UpdateToNext(int i)
        {
            this[i].ExpToNext = this[i].Memory.ReadInt(this[i].EXP_TO_NEXT);
            this[i].JobToNext = this[i].Memory.ReadInt(this[i].JOB_EXP_TO_NEXT);
        }

        public void UpdateKillTime(int i)
        {
            if (this[i].IsStopped)
            {
                Reset(i);
            }
            if (this[i].FirstKill == DateTime.MinValue)
            {
                this[i].FirstKill = DateTime.Now;
            }
            this[i].LastKill = DateTime.Now;
        }

        public void UpdatePerHour(int i, bool useLastKill)
        {
            if (this[i].FirstKill != DateTime.MinValue)
            {
                this[i].Time = (useLastKill ? this[i].LastKill : DateTime.Now) - this[i].FirstKill;
                if (this[i].Time.TotalSeconds <= 5)
                {
                    this[i].ExpPerHour = 0;
                    this[i].JobPerHour = 0;
                }
                else
                {
                    this[i].ExpPerHour = (int)((double)this[i].TotalExp / this[i].Time.TotalHours);
                    this[i].JobPerHour = (int)((double)this[i].TotalJob / this[i].Time.TotalHours);
                    UpdateEstimate(i);
                }
            }
        }

        public void UpdateEstimate(int i)
        {
            if (this[i].ExpPerHour > 0)
            {
                if (this[i].BaseLevel == 99)
                {
                    this[i].BaseEstimate = TimeSpan.Zero;
                }
                else
                {
                    double expEst = ((double)(this[i].ExpToNext - this[i].CurrentExp)) / (double)this[i].ExpPerHour;
                    this[i].BaseEstimate = TimeSpan.FromHours(expEst);
                }
            }
            else
            {
                this[i].BaseEstimate = TimeSpan.Zero;
            }
            if (this[i].JobPerHour > 0)
            {
                if (this[i].JobToNext == JOB_99_EXP)
                {
                    this[i].JobEstimate = TimeSpan.Zero;
                }
                else
                {
                    double expEst = ((double)(this[i].JobToNext - this[i].CurrentJob)) / (double)this[i].JobPerHour;
                    this[i].JobEstimate = TimeSpan.FromHours(expEst);
                }
            }
            else
            {
                this[i].JobEstimate = TimeSpan.Zero;
            }
        }

        public void Reset(int i)
        {
            this[i].FirstKill = DateTime.MinValue;
            this[i].LastKill = DateTime.MinValue;
            this[i].TotalExp = 0;
            this[i].ExpPerHour = 0;
            this[i].TotalJob = 0;
            this[i].JobPerHour = 0;
            this[i].IsStopped = false;
            this[i].Time = TimeSpan.Zero;
            this[i].BaseEstimate = TimeSpan.Zero;
            this[i].JobEstimate = TimeSpan.Zero;
        }

        public void Stop(int i)
        {
            this[i].IsStopped = true;
            if (this[i].FirstKill == DateTime.MinValue || this[i].LastKill == DateTime.MinValue)
            {
                Reset(i);
            }
            else
            {
                this[i].Time = this[i].LastKill - this[i].FirstKill;
                this[i].FirstKill = DateTime.MinValue;
                this[i].LastKill = DateTime.MinValue;
                UpdatePerHour(i, true);
                UpdateEstimate(i);
            }
        }

        public void StopAll()
        {
            for (int i = 0; i < characters.Count; i++)
                Stop(i);
        }

        public void ResetAll()
        {
            for (int i = 0; i < characters.Count; i++)
                Reset(i);
        }

        public void Dispose()
        {
            if (characters == null)
                return;
            for (int i = 0; i < characters.Count; i++)
            {
                try
                {
                    this[i].Dispose();
                }
                catch { }
            }
            characters = null;
        }

        public void Screenshot(int screenNumber, int i, bool automatic)
        {
            bool allow = false;
            IntPtr foreground = WindowsAPI.GetForegroundWindow();
            if (automatic)
            {
                allow = foreground == this[i].Memory.Process.MainWindowHandle;
            }
            else
            {
                for (int j = 0; j < characters.Count; j++)
                {

                    if (characters[j].Memory.Process.MainWindowHandle == foreground)
                    {
                        i = j;
                        allow = true;
                        break;
                    }
                }
            }
            if (allow)
            {
                Process process = this[i].Memory.Process;
                WindowsAPI.RECT srcRect;
                if (!process.MainWindowHandle.Equals(IntPtr.Zero))
                {
                    if (WindowsAPI.GetWindowRect(process.MainWindowHandle, out srcRect))
                    {
                        int width = srcRect.Right - srcRect.Left;
                        int height = srcRect.Bottom - srcRect.Top;

                        Bitmap bmp = new Bitmap(width, height);
                        Graphics screen = Graphics.FromImage(bmp);

                        try
                        {
                            DateTime now = DateTime.Now;
                            screen.CopyFromScreen(srcRect.Left, srcRect.Top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

                            string path = Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), this[i].Name);
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            if (automatic)
                            {
                                path = Path.Combine(path, String.Format("{0} - Level {1} - {2}.png", this[i].Name, this[i].BaseLevel, screenNumber / 3));
                            }
                            else
                            {
                                path = Path.Combine(path, String.Format("{0} - {1}-{2}-{3}-{4}.png", this[i].Name, DateTime.Now.Hour.ToString("00"), DateTime.Now.Minute.ToString("00"), DateTime.Now.Second.ToString("00"), DateTime.Now.Millisecond.ToString("00")));
                            }
                            bmp.Save(path, ImageFormat.Png);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            screen.Dispose();
                            bmp.Dispose();
                        }
                    }
                }

            }
        }
    }

    public class ProcessNotFoundException : Exception
    {
        public ProcessNotFoundException() : base() { }
        public ProcessNotFoundException(string message) : base(message) { }
    }

    public enum Client
    {
        None,
        Classic,
        Renewal
    }
}
