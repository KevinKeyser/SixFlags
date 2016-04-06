using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixFlags
{
    public class Department
    {
        public List<TimeSheet> timeSheets;

        public string Name { get; set; }

        public Department(string name)
        {
            timeSheets = new List<TimeSheet>();
            Name = name;
        }
    }
}
