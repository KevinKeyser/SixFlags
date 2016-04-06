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
        private Dictionary<string, Department> departments;
        private string shift;

        public ShiftTracker(string shift)
        {
            this.shift = shift;
            InitializeComponent();
            departments = new Dictionary<string, Department>();

            XmlDocument document = new XmlDocument();
            document.Load(SixFlagsTracker.xmlFile);
            int x = 0;
            int y = 0;
            foreach (Department depart in SixFlagsTracker.Departments)
            {
                departments.Add(depart.Name, new Department(depart.Name));
                Button departmentButton = new Button();
                departmentButton.Text = depart.Name;
                departmentButton.Location = new Point(x, y);
                departmentButton.Size = new Size(100, 100);
                departmentButton.Click += department_Click;
                Controls.Add(departmentButton);
                x += 100;
                if (x == 500)
                {
                    y += 100;
                    x = 0;
                }
            }

            foreach (XmlElement timeSheets in document.GetElementsByTagName("TimeSheets"))
            {
                if (timeSheets.Attributes["shift"].Value != shift)
                {
                    continue;
                }
                foreach (XmlElement department in timeSheets.GetElementsByTagName("Department"))
                {
                    foreach (XmlElement timeSheet in department.GetElementsByTagName("TimeSheet"))
                    {
                        departments[department.Attributes["name"].Value].timeSheets.Add(
                            new TimeSheet(
                                timeSheet.Attributes["name"].Value,
                                DateTime.Parse(timeSheet.Attributes["timeIn"].Value),
                                DateTime.Parse(timeSheet.Attributes["timeOut"].Value)));
                    }
                }
            }
        }

        private void department_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            TimeSheetAdder timeSheetAdder = new TimeSheetAdder(button.Text);
            timeSheetAdder.ShowDialog();

            if (timeSheetAdder.DialogResult == DialogResult.Yes)
            {
                departments[button.Text].timeSheets.Add(timeSheetAdder.TimeSheet);

                XmlDocument document = new XmlDocument();
                document.Load(SixFlagsTracker.xmlFile);
                XmlElement timeSheet = document.CreateElement("TimeSheet");
                timeSheet.SetAttribute("name", timeSheetAdder.TimeSheet.Name);
                timeSheet.SetAttribute("timeIn", timeSheetAdder.TimeSheet.TimeIn.ToString());
                timeSheet.SetAttribute("timeOut", timeSheetAdder.TimeSheet.TimeOut.ToString());
                foreach (XmlElement timeSheets in document.GetElementsByTagName("TimeSheets"))
                {
                    if (timeSheets.Attributes["shift"].Value != shift.ToLower())
                    {
                        continue;
                    }
                    foreach (XmlElement department in timeSheets.GetElementsByTagName("Department"))
                    {
                        if (department.Attributes["name"].Value == button.Text)
                        {
                            department.AppendChild(timeSheet);
                        }
                    }
                }
                document.Save(SixFlagsTracker.xmlFile);
            }
        }
    }
}
