using System;

namespace SixFlags
{
    public class TimeSheet
    {
        public TimeSheet(string name, DateTime timeIn, DateTime timeOut)
        {
            Name = name;
            TimeIn = timeIn;
            TimeOut = timeOut;
        }

        public string Name { get; set; }

        public DateTime TimeIn { get; set; }

        public DateTime TimeOut { get; set; }

        public int SentLunch = 0;

        public int SentBreak = 0;
    }
}