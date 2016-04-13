using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SixFlags
{
    public partial class SixFlagsTracker : Form
    {
        public static string TimeSheetsFile = DateTime.Now.ToShortDateString().Replace("/", "-") + "-TimeSheets.xml";
        public static string SavedDataFile = "SavedData.xml";
        public static List<Department> Departments;
        public static DateTime currentSelectedDate;

        public SixFlagsTracker()
        {
            Departments = new List<Department>();
            XDocument document;
            currentSelectedDate = DateTime.Now;

            #region Create SavedData

            if (!File.Exists(Environment.CurrentDirectory + "/" + SavedDataFile))
            {
                document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("SavedData",
                        new XElement("Departments"),
                        new XElement("Names")));
                document.Save(SavedDataFile);
            }

            #endregion

            #region Create TimeSheetFile

            if (!File.Exists(Environment.CurrentDirectory + "/" + TimeSheetsFile))
            {
                document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("SixFlags",
                        new XElement("TimeSheets", new XAttribute("shift", "day")),
                        new XElement("TimeSheets", new XAttribute("shift", "mid"))));
                var SavedDocument = XDocument.Load(SavedDataFile);
                foreach (
                    XElement departmentElement in SavedDocument.XPathSelectElements("SavedData/Departments/Department"))
                {
                    foreach (XElement timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                    {
                        timeSheets.Add(departmentElement);
                    }
                }
                document.Save(TimeSheetsFile);
            }

            #endregion

            //Add All Departments to Reference List
            document = XDocument.Load(SavedDataFile);
            document.XPathSelectElements("SavedData/Departments/Department")
                .ToList()
                .ForEach(element =>
                {
                    Departments.Add(new Department(element.Attribute("name").Value));
                });

            InitializeComponent();

            //Add add Custom Tracker Component to Each Tab
            foreach (TabPage TabPage in shiftTimes.TabPages)
            {
                var tracker = new ShiftTracker(TabPage.Text);
                tracker.Dock = DockStyle.Fill;
                TabPage.Controls.Add(tracker);
            }


            //Add Date Selector to MenuBar
            DateTimePicker datePicker = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Size = new Size(100, 50),
                MinDate = DateTime.MinValue,
                MaxDate = DateTime.Now,
                DropDownAlign = LeftRightAlignment.Right
            };
            datePicker.ValueChanged += (sender, args) =>
            {
                datePicker.Size = new Size(100, 50);
                if (datePicker.Value == currentSelectedDate)
                {
                    return;
                }
                if (CenteredMessageBox.Show("Are you sure you want to load a different Date", "Warning",
                    MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                    currentSelectedDate = datePicker.Value;
                    TimeSheetsFile = currentSelectedDate.ToShortDateString().Replace("/", "-") + "-TimeSheets.xml";
                    #region Create TimeSheetFile

                    if (!File.Exists(Environment.CurrentDirectory + "/" + TimeSheetsFile))
                    {
                        document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                            new XElement("SixFlags",
                                new XElement("TimeSheets", new XAttribute("shift", "day")),
                                new XElement("TimeSheets", new XAttribute("shift", "mid"))));
                        var SavedDocument = XDocument.Load(SavedDataFile);
                        foreach (
                            XElement departmentElement in SavedDocument.XPathSelectElements("SavedData/Departments/Department"))
                        {
                            foreach (XElement timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                            {
                                timeSheets.Add(departmentElement);
                            }
                        }
                        document.Save(TimeSheetsFile);
                    }
                    #endregion
                    document = XDocument.Load(TimeSheetsFile);
                    //Load new Date TimeSheets
                    foreach (TabPage tabPage in shiftTimes.TabPages)
                    {
                        ShiftTracker tracker = (ShiftTracker)tabPage.Controls[0];

                        tracker.departments.Keys.ToList()
                        .ForEach(depart => tracker.departments[depart].timeSheets.Clear());


                        List<XElement> timeSheets = document.XPathSelectElements("SixFlags/TimeSheets")
                                                    .ToList()
                                                    .Find(element =>
                                                        (String.Compare(element.Attribute("shift").Value, tracker.shift,
                                                            StringComparison.CurrentCultureIgnoreCase) == 0))
                                                    .Elements()
                                                    .ToList();
                        foreach (XElement timeSheet in timeSheets.Elements())
                        {
                            tracker.departments[timeSheet.Parent.Attribute("name").Value].timeSheets.Add(
                                new TimeSheet(
                                    timeSheet.Attribute("name").Value,
                                    DateTime.Parse(timeSheet.Attribute("timeIn").Value),
                                    DateTime.Parse(timeSheet.Attribute("timeOut").Value)));
                            foreach (XElement Lunch in timeSheet.XPathSelectElements("Lunches/Lunch"))
                            {
                                tracker.departments[timeSheet.Parent.Attribute("name").Value].timeSheets.Last().SentLunch.Add(DateTime.Parse(Lunch.Attribute("time").Value));
                            }
                            foreach (XElement Break in timeSheet.XPathSelectElements("Breaks/Break"))
                            {
                                tracker.departments[timeSheet.Parent.Attribute("name").Value].timeSheets.Last().SentLunch.Add(DateTime.Parse(Break.Attribute("time").Value));
                            }

                        }
                    }
                }
                else
                {
                    datePicker.Value = currentSelectedDate;
                }
            };
            ToolStripControlHost host = new ToolStripControlHost(datePicker, "dateTool")
            {
                Padding = new Padding(10, 0, 10, 0),
                Alignment = ToolStripItemAlignment.Right
            };
            toolStrip1.Size = new Size(toolStripContainer1.Width, toolStrip1.Size.Height);
            toolStrip1.Items.Add(host);
        }

        private void AddTool_Click(object sender, EventArgs e)
        {
            var add = new DepartmentAdd();
            add.ShowDialog();
            if (add.DialogResult == DialogResult.Yes)
            {
                //Add Department to SavedData
                var document = XDocument.Load(SavedDataFile);
                var departElement = new XElement("Department");
                departElement.SetAttributeValue("name", add.Department);
                document.XPathSelectElement("SavedData/Departments")
                    .Add(departElement);
                document.Save(SavedDataFile);

                //Add Department to Current TimeSheets
                document = XDocument.Load(TimeSheetsFile);
                document.XPathSelectElements("SixFlags/TimeSheets")
                    .ToList()
                    .ForEach(element =>
                            element.Add(departElement));
                document.Save(TimeSheetsFile);

                //Add Department to Reference List
                Departments.Add(new Department(add.Department));

                //Add Button to Tabs
                foreach (TabPage tabPage in shiftTimes.TabPages)
                {
                    ShiftTracker tracker = (ShiftTracker)tabPage.Controls[0];
                    tracker.AddDepartment(add.Department);
                }
            }
        }

        private void EditTool_Click(object sender, EventArgs e)
        {
            var editor = new DepartmentEditor();
            editor.ShowDialog();
            if (editor.DialogResult == DialogResult.Yes)
            {
                //Edit Departments in SavedData
                var document = XDocument.Load(SavedDataFile);
                document.XPathSelectElements("SavedData/Departments/Department")
                    .ToList()
                    .FindAll(element =>
                        String.Compare(element.Attribute("name").Value, editor.Department,
                            StringComparison.CurrentCultureIgnoreCase) == 0)
                    .ForEach(element =>
                    {
                        element.SetAttributeValue("name", editor.newName);
                    });
                document.Save(SavedDataFile);

                //Edit Department in Current TimeSheets
                document = XDocument.Load(TimeSheetsFile);
                document.XPathSelectElements("SixFlags/TimeSheets/Department")
                    .ToList()
                    .FindAll(element =>
                        String.Compare(element.Attribute("name").Value, editor.Department,
                            StringComparison.CurrentCultureIgnoreCase) == 0)
                    .ForEach(element =>
                    {
                        element.SetAttributeValue("name", editor.newName);
                    });
                document.Save(TimeSheetsFile);

                //Edit Department in Reference List
                Departments.Find(department => String.Compare(department.Name, editor.Department,
                    StringComparison.CurrentCultureIgnoreCase) == 0).Name = editor.newName;

                //Edit Button from Tabs
                foreach (TabPage tabPage in shiftTimes.TabPages)
                {
                    ShiftTracker tracker = (ShiftTracker)tabPage.Controls[0];
                    Department dept = tracker.departments[editor.Department];
                    tracker.departments.Remove(editor.Department);
                    tracker.departments.Add(editor.newName, dept);
                    List<Button> buttons = tracker.Controls.OfType<Button>().ToList();
                    foreach (Button button in buttons)
                    {
                        if (String.Compare(button.Text, editor.Department, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            button.Text = editor.newName;
                        }
                    }
                }
            }
        }

        private void RemoveTool_Click(object sender, EventArgs e)
        {
            var removal = new DepartmentRemoval();
            removal.ShowDialog();
            if (removal.DialogResult == DialogResult.Yes)
            {
                var departmentDelete = removal.Department;

                //Remove Department From Saved Data
                var document = XDocument.Load(SavedDataFile);
                document.XPathSelectElements("SavedData/Departments/Department")
                    .ToList()
                    .FindAll(element => String.Compare(element.Attribute("name").Value, departmentDelete,
                        StringComparison.CurrentCultureIgnoreCase) == 0)
                    .ForEach(element =>
                {
                    element.Remove();
                });
                document.Save(SavedDataFile);

                //Remove Department from Current TimeSheets
                document = XDocument.Load(TimeSheetsFile);
                document.XPathSelectElements("SixFlags/TimeSheets/Department")
                    .ToList()
                    .FindAll(element => String.Compare(element.Attribute("name").Value, departmentDelete,
                        StringComparison.CurrentCultureIgnoreCase) == 0)
                    .ForEach(element =>
                    {
                        element.Remove();
                    });

                document.Save(TimeSheetsFile);

                //Remove Department from ReferenceList
                Departments.Remove(
                    Departments.Find(department => String.Compare(department.Name, departmentDelete,
                        StringComparison.CurrentCultureIgnoreCase) == 0));

                //Remove Button from Tabs
                foreach (TabPage tabPage in shiftTimes.TabPages)
                {
                    ShiftTracker tracker = (ShiftTracker)tabPage.Controls[0];
                    List<Button> buttons = tracker.Controls.OfType<Button>().ToList();
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        if (String.Compare(buttons[i].Text, departmentDelete, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            tracker.Controls.Remove(buttons[i]);
                            break;
                        }
                    }
                    tracker.UpdatePositioning();
                }
            }
        }

        private void ClearTimeSheets_Click(object sender, EventArgs e)
        {
            if (CenteredMessageBox.Show("Are you sure you want to delete TimeSheet data?", "TimeSheet Delete",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var document = XDocument.Load(TimeSheetsFile);

                //Remove All TimeSheets
                document.XPathSelectElements("SixFlags/TimeSheets/TimeSheet")
                    .Elements()
                    .Remove();
                document.Save(TimeSheetsFile);

                //Remove All TimeSheet References
                Departments.ForEach(department =>
                {
                    department.timeSheets.Clear();
                });

                //Remove All TimeSheets from Tab Reference
                foreach (TabPage tabPage in shiftTimes.TabPages)
                {
                    ShiftTracker shiftTracker = (ShiftTracker)tabPage.Controls[0];
                    foreach (string key in shiftTracker.departments.Keys)
                    {
                        shiftTracker.departments[key].timeSheets.Clear();
                    }
                }
            }
        }
    }
}