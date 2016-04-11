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
    public partial class TimeSheetsEditor : Form
    {

        //undo break 10 mins max time
        private string shift;
        private Department department;
        public TimeSheetsEditor(string shift, Department department)
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
                timeSheet.Add(new XElement("Lunches"));
                timeSheet.Add(new XElement("Breaks"));

                foreach (var timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                {
                    if (timeSheets.Attribute("shift").Value != shift.ToLower())
                    {
                        continue;
                    }

                    foreach (XElement xDepartment in timeSheets.Elements())
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
            TimeSheetEditor editor = new TimeSheetEditor(department.timeSheets[index]);
            editor.ShowDialog();
            if (editor.DialogResult == DialogResult.Yes)
            {
                XDocument document = XDocument.Load(SixFlagsTracker.xmlFile);
                List<XElement> departmentElements =
                    document.XPathSelectElements("SixFlags/TimeSheets").ToList().Find(
                        element =>
                        String.Compare(element.Attribute("shift").Value, shift,
                            StringComparison.CurrentCultureIgnoreCase) == 0)
                            .XPathSelectElements("Department").ToList();
                foreach (XElement departmentElement in departmentElements)
                {
                    if (String.Compare(departmentElement.Attribute("name").Value, department.Name, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        departmentElement.XPathSelectElements("TimeSheet").ToList().ForEach(
                            element =>
                        {
                            if (element.Attribute("name").Value == editor.oldName)
                            {
                                element.SetAttributeValue("name", editor.newTimeSheet.Name);
                                element.SetAttributeValue("timeIn", editor.newTimeSheet.TimeIn.ToString());
                                element.SetAttributeValue("timeOut", editor.newTimeSheet.TimeOut.ToString());
                            }
                        });
                    }
                }
                department.timeSheets[index] = editor.newTimeSheet;
                timeSheetListBox.Items[index] = editor.newTimeSheet.Name;
                document.Save(SixFlagsTracker.xmlFile);
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
            XDocument document = XDocument.Load(SixFlagsTracker.xmlFile);
            if (sendLunchButton.Text == "Undo Lunch")
            {
                department.timeSheets[index].SentLunch.RemoveAt(department.timeSheets[index].SentLunch.Count - 1);
                foreach (var timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                {
                    if (timeSheets.Attribute("shift").Value != shift.ToLower())
                    {
                        continue;
                    }

                    foreach (XElement xDepartment in timeSheets.Elements())
                    {
                        if (xDepartment.Attribute("name").Value == department.Name)
                        {
                            xDepartment.XPathSelectElements("TimeSheet").ToList().ForEach(element =>
                            {
                                if (String.Compare(element.Attribute("name").Value, department.timeSheets[index].Name,
                                    StringComparison.CurrentCultureIgnoreCase) == 0)
                                {
                                    element.XPathSelectElements("Lunches/Lunch").Last().Remove();
                                }
                            });
                        }
                    }
                }
                document.Save(SixFlagsTracker.xmlFile);
                return;
            }
            else
            {
                department.timeSheets[index].SentLunch.Add(DateTime.Now);
                foreach (var timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                {
                    if (timeSheets.Attribute("shift").Value != shift.ToLower())
                    {
                        continue;
                    }

                    foreach (XElement xDepartment in timeSheets.Elements())
                    {
                        if (xDepartment.Attribute("name").Value == department.Name)
                        {
                            xDepartment.XPathSelectElements("TimeSheet").ToList().ForEach(element =>
                            {
                                if (String.Compare(element.Attribute("name").Value, department.timeSheets[index].Name,
                                    StringComparison.CurrentCultureIgnoreCase) == 0)
                                {
                                    XElement lunchElement = new XElement("Lunch");
                                    lunchElement.SetAttributeValue("time",
                                        department.timeSheets[index].SentLunch.Last().ToString());
                                    element.XPathSelectElement("Lunches").Add(lunchElement);
                                }
                            });
                        }
                    }
                }
            }
            document.Save(SixFlagsTracker.xmlFile);
        }

        private void sendBreakButton_Click(object sender, EventArgs e)
        {
            int index = timeSheetListBox.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            XDocument document = XDocument.Load(SixFlagsTracker.xmlFile);
            if (sendBreakButton.Text == "Undo Break")
            {
                department.timeSheets[index].SentBreak.RemoveAt(department.timeSheets[index].SentBreak.Count - 1);
                foreach (var timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                {
                    if (timeSheets.Attribute("shift").Value != shift.ToLower())
                    {
                        continue;
                    }

                    foreach (XElement xDepartment in timeSheets.Elements())
                    {
                        if (xDepartment.Attribute("name").Value == department.Name)
                        {
                            xDepartment.XPathSelectElements("TimeSheet").ToList().ForEach(element =>
                            {
                                if (String.Compare(element.Attribute("name").Value, department.timeSheets[index].Name,
                                    StringComparison.CurrentCultureIgnoreCase) == 0)
                                {
                                    element.XPathSelectElements("Breaks/Break").Last().Remove();
                                }
                            });
                        }
                    }
                }
                document.Save(SixFlagsTracker.xmlFile);
                return;
            }
            else
            {
                department.timeSheets[index].SentBreak.Add(DateTime.Now);
                foreach (var timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                {
                    if (timeSheets.Attribute("shift").Value != shift.ToLower())
                    {
                        continue;
                    }

                    foreach (XElement xDepartment in timeSheets.Elements())
                    {
                        if (xDepartment.Attribute("name").Value == department.Name)
                        {
                            xDepartment.XPathSelectElements("TimeSheet").ToList().ForEach(element =>
                            {
                                if (String.Compare(element.Attribute("name").Value, department.timeSheets[index].Name,
                                    StringComparison.CurrentCultureIgnoreCase) == 0)
                                {
                                    XElement breakElement = new XElement("Break");
                                    breakElement.SetAttributeValue("time",
                                        department.timeSheets[index].SentBreak.Last().ToString());
                                    element.XPathSelectElement("Breaks").Add(breakElement);
                                }
                            });
                        }
                    }
                }
            }
            document.Save(SixFlagsTracker.xmlFile);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (timeSheetListBox.SelectedIndex == -1)
            {
                return;
            }
            TimeSheet timeSheet = department.timeSheets[timeSheetListBox.SelectedIndex];
            if (timeSheet.SentBreak.Count > 0 && DateTime.Now - timeSheet.SentBreak.Last() <= TimeSpan.FromMinutes(10))
            {
                sendBreakButton.Text = "Undo Break";
            }
            else
            {
                sendBreakButton.Text = "Send On Break";
            }
            if (timeSheet.SentLunch.Count > 0 && DateTime.Now - timeSheet.SentLunch.Last() <= TimeSpan.FromMinutes(10))
            {
                sendLunchButton.Text = "Undo Lunch";
            }
            else
            {
                sendLunchButton.Text = "Send On Lunch";
            }
        }
    }
}
