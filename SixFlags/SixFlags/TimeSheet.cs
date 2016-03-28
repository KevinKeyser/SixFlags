using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SixFlags
{
    public class TimeSheet
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private TimeSpan timeIn;

        public TimeSpan TimeIn
        {
            get { return timeIn; }
            set { timeIn = value; }
        }

        private TimeSpan timeOut;

        public TimeSpan TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }

        public TimeSheet(string name, TimeSpan timeIn, TimeSpan timeOut)
        {
            this.name = name;
            this.timeIn = timeIn;
            this.timeOut = timeOut;
        }
            
    }
}
