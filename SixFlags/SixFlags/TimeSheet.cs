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
    }
}