using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SixFlags
{
    public partial class ShiftTracker : UserControl
    {
        public ShiftTracker()
        {
            InitializeComponent();
        }

        private void mobileButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            TimeSheetAdder timeSheetAdder = new TimeSheetAdder(button.Text);
            timeSheetAdder.ShowDialog();

            if (timeSheetAdder.Result == DialogResult.Yes)
            {
                if (!SixFlagsTracker.departments.ContainsKey(button.Text))
                {
                    SixFlagsTracker.departments.Add(button.Text, new List<TimeSheet>());
                }
                SixFlagsTracker.departments[button.Text].Add(timeSheetAdder.TimeSheet);

                XmlDocument document = new XmlDocument();
                document.Load(SixFlagsTracker.xmlFile);
                XmlElement timeSheet = document.CreateElement("TimeSheet");
                timeSheet.SetAttribute("department", timeSheetAdder.Department);
                timeSheet.SetAttribute("name", timeSheetAdder.TimeSheet.Name);
                timeSheet.SetAttribute("timeIn", timeSheetAdder.TimeSheet.TimeIn.ToString());
                timeSheet.SetAttribute("timeOut", timeSheetAdder.TimeSheet.TimeOut.ToString());
                document.GetElementsByTagName("TimeSheets")[0].AppendChild(timeSheet);
                document.Save(SixFlagsTracker.xmlFile);
            }
        }

    }
}
