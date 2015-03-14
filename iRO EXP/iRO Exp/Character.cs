using System;
using System.Collections.Generic;
using System.Text;
using ReadWriteMemory;
using System.IO;

namespace iROClassicExp
{
    public class Character
    {
        public readonly int HP;
        public readonly int SP;
        public readonly int MAX_HP;
        public readonly int MAX_SP;
        public readonly int NAME;
        public readonly int MAP_NAME;
        public readonly int WEIGHT;
        public readonly int MAX_WEIGHT;
        public readonly int EXP;
        public readonly int EXP_TO_NEXT;
        public readonly int BASE_LEVEL;
        public readonly int JOB_LEVEL;
        public readonly int JOB_EXP;
        public readonly int JOB_EXP_TO_NEXT;

        private const int JOB_99_EXP = 999999999;
        private const int BASE_99_150_EXP = 99999999;

        private const int MAP_NAME_SIZE = 10;
        private const int MAX_LEN = 24;
        private const int MAX_MAP_NAME_LEN = 10;

        private int currentHp;
        private int currentSp;
        private int maxHp;
        private int maxSp;
        private string name;
        private int currentWeight;
        private int maxWeight;
        private int currentExp;
        private int expToNext;
        private int baseLevel;
        private int jobLevel;
        private int currentJob;
        private int jobToNext;
        private DateTime firstKill = DateTime.MinValue;
        private DateTime lastKill;
        private TimeSpan time;
        private int totalExp;
        private int totalJob;
        private int expPerHour;
        private int jobPerHour;
        private TimeSpan baseEstimate;
        private TimeSpan jobEstimate;

        private float hpPercent;
        private float spPercent;
        private float weightPercent;
        private float expPercent;
        private float jobPercent;

        private bool isStopped;
        private string mapName;

        private bool autoScreen;
        private int levelUpScreens;
        private bool leveledUp;
        private Client client;

        private ProcessMemory memory;

        public int CurrentHp { get { return currentHp; } set { currentHp = value; } }
        public int CurrentSp { get { return currentSp; } set { currentSp = value; } }
        public int MaxHp { get { return maxHp; } set { maxHp = value; } }
        public int MaxSp { get { return maxSp; } set { maxSp = value; } }
        public string Name { get { return name; } set { name = value; } }
        public int CurrentWeight { get { return currentWeight; } set { currentWeight = value; } }
        public int MaxWeight { get { return maxWeight; } set { maxWeight = value; } }
        public int CurrentExp { get { return currentExp; } set { currentExp = value; } }
        public int ExpToNext { get { return expToNext; } set { expToNext = value; } }
        public int BaseLevel { get { return baseLevel; } set { baseLevel = value; } }
        public int JobLevel { get { return jobLevel; } set { jobLevel = value; } }
        public int CurrentJob { get { return currentJob; } set { currentJob = value; } }
        public int JobToNext { get { return jobToNext; } set { jobToNext = value; } }
        public float HpPercent { get { return hpPercent; } set { hpPercent = value; } }
        public float SpPercent { get { return spPercent; } set { spPercent = value; } }
        public float WeightPercent { get { return weightPercent; } set { weightPercent = value; } }
        public float ExpPercent { get { return expPercent; } set { expPercent = value; } }
        public float JobPercent { get { return jobPercent; } set { jobPercent = value; } }
        public TimeSpan Time { get { return time; } set { time = value; } }
        public int TotalExp { get { return totalExp; } set { totalExp = value; } }
        public int ExpPerHour { get { return expPerHour; } set { expPerHour = value; } }
        public int TotalJob { get { return totalJob; } set { totalJob = value; } }
        public int JobPerHour { get { return jobPerHour; } set { jobPerHour = value; } }
        public TimeSpan BaseEstimate { get { return baseEstimate; } set { baseEstimate = value; } }
        public TimeSpan JobEstimate { get { return jobEstimate; } set { jobEstimate = value; } }
        public bool AutoScreen { get { return autoScreen; } set { autoScreen = value; } }
        public bool IsStopped { get { return isStopped; } set { isStopped = value; } }
        public string MapName { get { return mapName; } set { mapName = value; } }
        public Client Client { get { return client; } set { client = value; } }
        public DateTime FirstKill { get { return firstKill; } set { firstKill = value; } }
        public DateTime LastKill { get { return lastKill; } set { lastKill = value; } }
        public bool LeveledUp { get { return leveledUp; } set { leveledUp = value; } }
        public int LevelUpScreens { get { return levelUpScreens; } set { levelUpScreens = value; } }
        public ProcessMemory Memory { get { return memory; } }

        public Character(int index, Client client)
        {
            this.client = client;
            this.memory = new ProcessMemory(client == Client.Classic ? @"clragexe" : @"ragexe", index);
            memory.StartProcess();
            String filename = client == Client.Classic ? @"memoryaddress.dat" : @"renewalmemory.dat";
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    String line = reader.ReadLine();
                    HP = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    line = reader.ReadLine();
                    SP = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    line = reader.ReadLine();
                    MAX_HP = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    line = reader.ReadLine();
                    MAX_SP = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    line = reader.ReadLine();
                    NAME = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    line = reader.ReadLine();
                    MAP_NAME = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    line = reader.ReadLine();
                    WEIGHT = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    line = reader.ReadLine();
                    MAX_WEIGHT = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    line = reader.ReadLine();
                    EXP = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    line = reader.ReadLine();
                    EXP_TO_NEXT = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    line = reader.ReadLine();
                    BASE_LEVEL = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    line = reader.ReadLine();
                    JOB_LEVEL = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    line = reader.ReadLine();
                    JOB_EXP = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    line = reader.ReadLine();
                    JOB_EXP_TO_NEXT = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception)
            {
                if (client == Client.Classic)
                {
                    HP = 0x008b78dc;
                    SP = 0x008b78e4;
                    MAX_HP = 0x008b78e0;
                    MAX_SP = 0x008b78e8;
                    NAME = 0x008b7bf8;
                    MAP_NAME = 0x008b57a4;
                    WEIGHT = 0x008b64d8;
                    MAX_WEIGHT = 0x008b64cc;
                    EXP = 0x008b6418;
                    EXP_TO_NEXT = 0x008b6424;
                    BASE_LEVEL = 0x008b641c;
                    JOB_LEVEL = 0x008b6428;
                    JOB_EXP = 0x008b64d4;
                    JOB_EXP_TO_NEXT = 0x008b64d0;
                }
                else
                {
                    HP = 0x99DAC4;
                    SP = 0x99DACC;
                    MAX_HP = 0x99DAC8;
                    MAX_SP = 0x99DAD0;
                    NAME = 0x99e000;
                    MAP_NAME = 0x99aa0c;
                    WEIGHT = 0x99b74c;
                    MAX_WEIGHT = 0x99b740;
                    EXP = 0x99b68c;
                    EXP_TO_NEXT = 0x99b698;
                    BASE_LEVEL = 0x99b690;
                    JOB_LEVEL = 0x99b69c;
                    JOB_EXP = 0x99b748;
                    JOB_EXP_TO_NEXT = 0x99b744;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Character))
            {
                return false;
            }
            if (((obj as Character).Client == this.Client) && ((obj as Character).Name == this.Name))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void Dispose()
        {
            ReadWriteMemory.ProcessMemory.CloseHandle(memory.ProcessHandle);
            memory = null;
        }

        public void ReadBasicStats()
        {
            Name = Memory.ReadStringAscii(NAME, MAX_LEN);
            CurrentHp = Memory.ReadInt(HP);
            MaxHp = Memory.ReadInt(MAX_HP);
            CurrentSp = Memory.ReadInt(SP);
            MaxSp = Memory.ReadInt(MAX_SP);
            CurrentWeight = Memory.ReadInt(WEIGHT);
            MaxWeight = Memory.ReadInt(MAX_WEIGHT);
            MapName = Memory.ReadStringAscii(MAP_NAME, MAX_MAP_NAME_LEN);

            HpPercent = MaxHp == 0 ? 0 : (float)CurrentHp * 100.0f / (float)MaxHp;
            SpPercent = MaxSp == 0 ? 0 : (float)CurrentSp * 100.0f / (float)MaxSp;
            WeightPercent = MaxWeight == 0 ? 0 : (float)CurrentWeight * 100.0f / (float)MaxWeight;
        }

        public void UpdateToNext()
        {
            ExpToNext = Memory.ReadInt(EXP_TO_NEXT);
            JobToNext = Memory.ReadInt(JOB_EXP_TO_NEXT);
        }

        public void UpdatePerHour(bool useLastKill)
        {
            if (FirstKill != DateTime.MinValue)
            {
                Time = (useLastKill ? LastKill : DateTime.Now) - FirstKill;
                if (Time.TotalSeconds <= 5)
                {
                    ExpPerHour = 0;
                    JobPerHour = 0;
                }
                else
                {
                    ExpPerHour = (int)((double)TotalExp / Time.TotalHours);
                    JobPerHour = (int)((double)TotalJob / Time.TotalHours);
                    UpdateEstimate();
                }
            }
        }

        public void UpdateEstimate()
        {
            if (ExpPerHour > 0)
            {
                if (ExpToNext == BASE_99_150_EXP)
                {
                    BaseEstimate = TimeSpan.Zero;
                }
                else
                {
                    double expEst = ((double)(ExpToNext - CurrentExp)) / (double)ExpPerHour;
                    BaseEstimate = TimeSpan.FromHours(expEst);
                }
            }
            else
            {
                BaseEstimate = TimeSpan.Zero;
            }
            if (JobPerHour > 0)
            {
                if (JobToNext == JOB_99_EXP)
                {
                    JobEstimate = TimeSpan.Zero;
                }
                else
                {
                    double expEst = ((double)(JobToNext - CurrentJob)) / (double)JobPerHour;
                    JobEstimate = TimeSpan.FromHours(expEst);
                }
            }
            else
            {
                JobEstimate = TimeSpan.Zero;
            }
        }

        public void Reset()
        {
            FirstKill = DateTime.MinValue;
            LastKill = DateTime.MinValue;
            TotalExp = 0;
            ExpPerHour = 0;
            TotalJob = 0;
            JobPerHour = 0;
            IsStopped = false;
            Time = TimeSpan.Zero;
            BaseEstimate = TimeSpan.Zero;
            JobEstimate = TimeSpan.Zero;
        }

        public void UpdateKillTime()
        {
            if (IsStopped)
            {
                Reset();
            }
            if (FirstKill == DateTime.MinValue)
            {
                FirstKill = DateTime.Now;
            }
            LastKill = DateTime.Now;
        }

        public void Stop()
        {            
            IsStopped = true;
            if (FirstKill == DateTime.MinValue || LastKill == DateTime.MinValue)
            {
                Reset();
            }
            else
            {
                Time = LastKill - FirstKill;
                FirstKill = DateTime.MinValue;
                LastKill = DateTime.MinValue;
                UpdatePerHour(true);
                UpdateEstimate();
            }
        }
    }
}
