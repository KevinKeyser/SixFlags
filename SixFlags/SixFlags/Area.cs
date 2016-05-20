using System.Collections.Generic;

namespace SixFlags
{
    public class Area
    {
        public List<TimeSheet> timeSheets;

        public List<TimeSheet> EndedTimeSheets;

        public Area(string name)
        {
            timeSheets = new List<TimeSheet>();
            EndedTimeSheets = new List<TimeSheet>();
            Name = name;
        }

        public string Name { get; set; }
    }
}