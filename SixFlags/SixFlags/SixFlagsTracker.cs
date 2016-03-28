using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SixFlags
{
    public partial class SixFlagsTracker : Form
    {
        public static Dictionary<string, List<TimeSheet>> departments;
        public static string xmlFile = "TimeSheets.xml";

        public SixFlagsTracker()
        {
            InitializeComponent();
            departments = new Dictionary<string, List<TimeSheet>>();
            XmlDocument document = new XmlDocument();
            document.Load(xmlFile);
            foreach (XmlElement department in document.GetElementsByTagName("Department"))
            {
                departments.Add(department.Attributes["name"].Value, new List<TimeSheet>());
            }
            foreach (XmlElement timeSheet in document.GetElementsByTagName("TimeSheet"))
            {
                departments[timeSheet.Attributes["department"].Value].Add(
                    new TimeSheet(
                        timeSheet.Attributes["name"].Value,
                        TimeSpan.Parse(timeSheet.Attributes["timeIn"].Value),
                        TimeSpan.Parse(timeSheet.Attributes["timeOut"].Value)));
            }
        }
    }
}
