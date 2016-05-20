using System;
using System.Collections.Generic;

namespace SixFlags
{
    public class TimeSheet
    {
        public TimeSheet(string name, DateTime timeIn, DateTime timeOut)
        {
            Name = name;
            TimeIn = timeIn;
            TimeOut = timeOut;
            SentLunch = new List<DateTime>();
            SentBreak = new List<DateTime>();
        }

        public string Name { get; set; }

        public DateTime TimeIn { get; set; }

        public DateTime TimeOut { get; set; }

        public List<DateTime> SentLunch;

        public List<DateTime> SentBreak;

        public TimeSpan getNextLunch()
        {
            return TimeSpan.FromHours(5 * (SentLunch.Count + 1) + .75f * SentLunch.Count);
        }

        public TimeSpan getNextBreak()
        {
            return (SentBreak.Count <= SentLunch.Count
                ? TimeSpan.FromHours(5.75f * SentLunch.Count + 3.75f)
                : TimeSpan.FromHours((SentLunch.Count + 1) * 3.75f));
        }
    }
}