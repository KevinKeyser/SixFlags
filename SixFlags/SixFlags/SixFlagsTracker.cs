using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SixFlags
{
    public partial class SixFlagsTracker : Form
    {
        public static string xmlFile = "TimeSheets.xml";
        public static List<Department> Departments;

        public SixFlagsTracker()
        {
            Departments = new List<Department>();
            
            var document = XDocument.Load(xmlFile);
            foreach (var department in document.XPathSelectElements("SixFlags/SavedData/Departments/Department"))
            {
                Departments.Add(new Department(department.Attribute("name").Value));
            }

            InitializeComponent();
            foreach (TabPage TabPage in shiftTimes.TabPages)
            {
                var tracker = new ShiftTracker(TabPage.Text);
                tracker.Dock = DockStyle.Fill;
                TabPage.Controls.Add(tracker);
            }
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var add = new DepartmentAdd();
            add.ShowDialog();
            if (add.DialogResult == DialogResult.Yes)
            {
                var departmentAdd = add.Department;
                var document = XDocument.Load(xmlFile);
                var departElement = new XElement("Department");
                departElement.SetAttributeValue("name", departmentAdd);
                document.XPathSelectElement("SixFlags/SavedData/Departments").Add(departElement);
                //departElement = document.CreateElement("Department");
                //departElement.SetAttribute("name", departmentAdd);
                document.XPathSelectElements("SixFlags/TimeSheets")
                    .ToList()
                    .ForEach(element => element.Add(departElement));

                document.Save(xmlFile);
                Departments.Add(new Department(departmentAdd));
                foreach (TabPage tabPage in shiftTimes.TabPages)
                {
                    ShiftTracker tracker = (ShiftTracker) tabPage.Controls[0];
                    tracker.AddDepartment(departmentAdd);
                }
            }
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = new DepartmentEditor();
            editor.ShowDialog();
            if (editor.DialogResult == DialogResult.Yes)
            {
                var departmentAdd = editor.Department;
                var document = XDocument.Load(xmlFile);
                var departElement = new XElement("Department");
                departElement.SetAttributeValue("name", departmentAdd);
                document.XPathSelectElements("SixFlags/SavedData/Departments/Department")
                    .ToList()
                    .ForEach(element =>
                    {
                        if (element.Attribute("name").Value == editor.Department)
                            element.SetAttributeValue("name", editor.newName);
                    });

                document.XPathSelectElements("SixFlags/TimeSheets/Department")
                    .ToList()
                    .ForEach(element =>
                    {
                        if (element.Attribute("name").Value == editor.Department)
                            element.SetAttributeValue("name", editor.newName);
                    });
                document.Save(xmlFile);
                foreach (Department department in Departments)
                {
                    if (String.Compare(department.Name, editor.Department, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        department.Name = editor.newName;
                    }
                }
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

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var removal = new DepartmentRemoval();
            removal.ShowDialog();
            if (removal.DialogResult == DialogResult.Yes)
            {
                var departmentDelete = removal.Department;
                var document = XDocument.Load(xmlFile);
                var depratmentElement = document.XPathSelectElements("SixFlags/SavedData/Departments/Department");
                for (var i = 0; i < depratmentElement.Count(); i++)
                {
                    var deptElement = depratmentElement.ElementAt(i);
                    if (String.Compare(deptElement.Attribute("name").Value, departmentDelete, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        deptElement.Remove();
                        break;
                    }
                }
                document.XPathSelectElements("SixFlags/TimeSheets/Department")
                    .ToList()
                    .FindAll(element => String.Compare(element.Attribute("name").Value, departmentDelete,
                        StringComparison.CurrentCultureIgnoreCase) == 0)
                        .ForEach(element => element.Remove());

                document.Save(xmlFile);
                for (var i = 0; i < Departments.Count; i++)
                {
                    if (Departments[i].Name == departmentDelete)
                    {
                        Departments.RemoveAt(i);
                        break;
                    }
                }
                foreach (TabPage tabPage in shiftTimes.TabPages)
                {
                    ShiftTracker tracker = (ShiftTracker)tabPage.Controls[0];
                    List<Button> buttons = tracker.Controls.OfType<Button>().ToList();
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        if (String.Compare(buttons[i].Text, departmentDelete, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            tracker.Controls.RemoveAt(i);
                            break;
                        }
                    }
                    tracker.UpdatePositioning();
                }
            }
        }

        private void clearTimeSheetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CenteredMessageBox.Show("Are you sure you want to delete TimeSheet data?", "TimeSheet Delete",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var document = XDocument.Load(xmlFile);

                var timeSheetElement = document.XPathSelectElements("SixFlags/TimeSheets");
                foreach (var departmentElement in timeSheetElement.Elements())
                {
                    departmentElement.RemoveNodes();
                }
                document.Save(xmlFile);
                for (int i = 0; i < Departments.Count; i++)
                {
                    Departments[i].timeSheets.Clear();
                }
                foreach (TabPage tabPage in shiftTimes.TabPages)
                {
                    ShiftTracker shiftTracker = (ShiftTracker) tabPage.Controls[0];
                    foreach (string key in shiftTracker.departments.Keys)
                    {
                        shiftTracker.departments[key].timeSheets.Clear();
                    }
                }
            }
        }
    }
}