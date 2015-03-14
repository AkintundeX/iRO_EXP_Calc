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

        private const int RECHECK_CLIENTS = 20;

        private bool autoScreen;
        private int loopCount;

        private bool isBusy;
        public bool IsBusy { get { return isBusy; } }

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
            int processCount = ProcessMemory.ProcessCount(Settings.ClassicProcess) + ProcessMemory.ProcessCount(Settings.RenewalProcess);
            if (processCount != characters.Count)
            {
                DetectNewClients();
                loopCount = 0;
            }
            else
            {
                loopCount++;
                if (loopCount > RECHECK_CLIENTS)
                {
                    DetectNewClients();
                    loopCount = 0;
                }
            }
            for (int i = 0; i < characters.Count; i++)
            {
                if (this[i].Memory == null)
                    throw new ProcessNotFoundException("Never attached");
                if (!this[i].Memory.CheckProcess())
                    throw new ProcessNotFoundException("Process not found.");

                bool ignoreUpdates = String.IsNullOrEmpty(this[i].Name);
                string curName = this[i].Name;

                this[i].ReadBasicStats();
                if (this[i].Name != curName)
                {
                    this[i].Reset();
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
                    this[i].Reset();
                    continue;
                }

                if (this[i].IsStopped && (this[i].CurrentExp != curExp || this[i].CurrentJob != curJob || this[i].BaseLevel != curBaseLvl || this[i].JobLevel != curJobLvl))
                {
                    this[i].Reset();
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
                        this[i].UpdateKillTime();

                    this[i].UpdateToNext();
                    this[i].UpdatePerHour(false);

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

        public void StopAll()
        {
            for (int i = 0; i < characters.Count; i++)
                this[i].Stop();
        }

        public void ResetAll()
        {
            for (int i = 0; i < characters.Count; i++)
                this[i].Reset();
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

        public void DetectNewClients()
        {
            isBusy = true;
            int processCount = ProcessMemory.ProcessCount(Settings.ClassicProcess);
            List<Character> newchars = new List<Character>();
            for (int i = 0; i < processCount; i++)
                newchars.Add(new Character(i, Client.Classic));
            processCount = ProcessMemory.ProcessCount(Settings.RenewalProcess);
            for (int i = 0; i < processCount; i++)
                newchars.Add(new Character(i, Client.Renewal));

            foreach (Character c in newchars)
                c.ReadBasicStats();

            processCount = ProcessMemory.ProcessCount(Settings.ClassicProcess) + ProcessMemory.ProcessCount(Settings.RenewalProcess);

            if (processCount != characters.Count)
            {
                List<Character> kept = new List<Character>();
                foreach (Character c in characters)
                {
                    foreach (Character n in newchars)
                    {
                        if (c.Equals(n))
                        {
                            kept.Add(c);
                        }
                    }
                }
                characters = kept;
                if (characters.Count != newchars.Count)
                {
                    // New clients
                    foreach (Character c in characters)
                    {
                        newchars.Remove(c);
                    }
                    foreach (Character c in newchars)
                    {
                        characters.Add(c);
                    }
                }
            }
            else
            {
                int matches = 0;
                foreach (Character c in characters)
                {
                    foreach (Character n in newchars)
                    {
                        if (c.Equals(n))
                        {
                            matches++;
                            break;
                        }
                    }
                }

                if (matches != characters.Count)
                {
                    for (int i = 0; i < characters.Count; i++)
                    {
                        bool matched = false;
                        foreach (Character c in newchars)
                        {
                            if (c.Equals(characters[i]))
                            {
                                matched = true;
                                break;
                            }
                        }
                        if (!matched)
                        {
                            characters[i].Dispose();
                            characters[i] = null;
                        }
                    }
                    while (characters.Remove(null)) { }
                }
            }

            foreach (Character c in characters)
            {
                c.ReadBasicStats();
            }

            isBusy = false;
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
