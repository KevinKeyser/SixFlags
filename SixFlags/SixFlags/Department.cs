using System.Collections.Generic;

namespace SixFlags
{
    public class Department
    {
        public List<TimeSheet> timeSheets;

        public Department(string name)
        {
            timeSheets = new List<TimeSheet>();
            Name = name;
        }

        public string Name { get; set; }
    }
}