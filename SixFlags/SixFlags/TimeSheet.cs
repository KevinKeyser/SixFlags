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

        private DateTime timeIn;

        public DateTime TimeIn
        {
            get { return timeIn; }
            set { timeIn = value; }
        }

        private DateTime timeOut;

        public DateTime TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }

        public TimeSheet(string name, DateTime timeIn, DateTime timeOut)
        {
            this.name = name;
            this.timeIn = timeIn;
            this.timeOut = timeOut;
        }
            
    }
}
