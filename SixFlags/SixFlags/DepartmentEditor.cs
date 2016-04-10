using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SixFlags
{
    public partial class DepartmentEditor : Form
    {
        private string shift;
        private Department department;
        public DepartmentEditor(string shift, Department department)
        {
            InitializeComponent();
            this.shift = shift;
            this.department = department;
            foreach (TimeSheet timeSheet in this.department.timeSheets)
            {
                timeSheetListBox.Items.Add(timeSheet.Name);
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var timeSheetAdder = new TimeSheetAdder(department.Name);
            timeSheetAdder.ShowDialog();
            
            if (timeSheetAdder.DialogResult == DialogResult.Yes)
            {
                department.timeSheets.Add(timeSheetAdder.TimeSheet);
                timeSheetListBox.Items.Add(timeSheetAdder.TimeSheet.Name);
                var document = XDocument.Load(SixFlagsTracker.xmlFile);
                var timeSheet = new XElement("TimeSheet");
                timeSheet.SetAttributeValue("name", timeSheetAdder.TimeSheet.Name);
                timeSheet.SetAttributeValue("timeIn", timeSheetAdder.TimeSheet.TimeIn.ToString());
                timeSheet.SetAttributeValue("timeOut", timeSheetAdder.TimeSheet.TimeOut.ToString());

                foreach (var timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                {
                    if (timeSheets.Attribute("shift").Value != shift.ToLower())
                    {
                        continue;
                    }

                    foreach (XElement xDepartment in timeSheets.Nodes())
                    {
                        if (xDepartment.Attribute("name").Value == department.Name)
                        {
                            xDepartment.Add(timeSheet);
                        }
                    }
                }
                document.Save(SixFlagsTracker.xmlFile);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            int index = timeSheetListBox.SelectedIndex;
            if (index == -1)
            {
                return;
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            int index = timeSheetListBox.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            DialogResult results = CenteredMessageBox.Show("Are you sure you want to remove " + timeSheetListBox.Items[index], "Remove",
                MessageBoxButtons.YesNo);
            if (results == DialogResult.Yes)
            {
                var document = XDocument.Load(SixFlagsTracker.xmlFile);
                var timeSheetsElements = document.XPathSelectElements("SixFlags/TimeSheets");
                foreach (XElement timeSheetsElement in timeSheetsElements)
                {
                    if (timeSheetsElement.Attribute("shift").Value.ToLower() == shift.ToLower())
                    {
                        var departmentElements = timeSheetsElement.XPathSelectElements("Department");

                        foreach (XElement departmentElement in departmentElements)
                        {
                            if (departmentElement.Attribute("name").Value.ToLower() == department.Name.ToLower())
                            {
                                var timeSheetElements = departmentElement.XPathSelectElements("TimeSheet");
                                foreach (XElement timeSheetElement in timeSheetElements)
                                {
                                    if (timeSheetElement.Attribute("name").Value.ToLower() ==
                                        timeSheetListBox.Items[index].ToString().ToLower())
                                    {
                                        timeSheetElement.Remove();
                                    }
                                }
                            }
                        }
                    }
                }

                document.Save(SixFlagsTracker.xmlFile);
                department.timeSheets.RemoveAt(index);
                timeSheetListBox.Items.RemoveAt(index);
            }
        }

        private void sendLunchButton_Click(object sender, EventArgs e)
        {
            int index = timeSheetListBox.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            department.timeSheets[index].SentLunch++;
        }

        private void sendBreakButton_Click(object sender, EventArgs e)
        {
            int index = timeSheetListBox.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            department.timeSheets[index].SentBreak++;
        }
    }
}
