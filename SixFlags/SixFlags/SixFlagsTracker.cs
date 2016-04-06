using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        public static string xmlFile = "TimeSheets.xml";
        public static List<Department> Departments;

        public SixFlagsTracker()
        {
            Departments = new List<Department>();

            XmlDocument document = new XmlDocument();
            document.Load(xmlFile);
            foreach (XmlElement department in ((XmlElement)document.GetElementsByTagName("Departments")[0]).GetElementsByTagName("Department"))
            {
                Departments.Add(new Department(department.Attributes["name"].Value));
            }

            InitializeComponent();
            foreach (TabPage TabPage in shiftTimes.TabPages)
            {
                ShiftTracker tracker = new ShiftTracker(TabPage.Text);
                tracker.Dock = DockStyle.Fill;
                TabPage.Controls.Add(tracker);
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DepartmentRemoval removal = new DepartmentRemoval();
            removal.ShowDialog();
            if (removal.DialogResult == DialogResult.Yes)
            {
                string departmentDelete = removal.Department;
                XmlDocument document = new XmlDocument();
                document.Load(xmlFile);
                XmlElement depratmentElement = document.GetElementsByTagName("Departments")[0] as XmlElement;
                for (int i = 0; i < depratmentElement.GetElementsByTagName("Department").Count; i++)
                {
                    XmlElement deptElement = depratmentElement.GetElementsByTagName("Department")[i] as XmlElement;
                    if (deptElement.Attributes["name"].Value == departmentDelete)
                    {
                        depratmentElement.RemoveChild(deptElement);
                        break;
                    }
                }
                foreach (XmlElement departmentElement in document.GetElementsByTagName("TimeSheets"))
                {
                    for (int i = 0; i < depratmentElement.GetElementsByTagName("Department").Count; i++)
                    {
                        XmlElement deptElement = depratmentElement.GetElementsByTagName("Department")[i] as XmlElement;
                        if (deptElement.Attributes["name"].Value == departmentDelete)
                        {
                            depratmentElement.RemoveChild(deptElement);
                            break;
                        }
                    }
                }
                document.Save(xmlFile);
                for (int i = 0; i < Departments.Count; i++)
                {
                    if (Departments[i].Name == departmentDelete)
                    {
                        Departments.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void AddEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DepartmentAdd add = new DepartmentAdd();
            add.ShowDialog();
            if (add.DialogResult == DialogResult.Yes)
            {
                string departmentAdd = add.Department;
                XmlDocument document = new XmlDocument();
                document.Load(xmlFile);
                XmlElement departElement = document.CreateElement("Department");
                departElement.SetAttribute("name", departmentAdd);
                document.GetElementsByTagName("Departments")[0].AppendChild(departElement);
                departElement = document.CreateElement("Department");
                departElement.SetAttribute("name", departmentAdd);
                document.GetElementsByTagName("TimeSheets")[0].AppendChild(departElement);
                document.Save(xmlFile);
                Departments.Add(new Department(departmentAdd));
            }
        }

        private void clearTimeSheetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CenteredMessageBox.Show("Are you sure you want to delete TimeSheet data?", "TimeSheet Delete",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                XmlDocument document = new XmlDocument();
                document.Load(xmlFile);

                foreach (XmlElement departmentElement in document.GetElementsByTagName("TimeSheets"))
                {
                    for (int i = 0; i < departmentElement.GetElementsByTagName("Department").Count; i++)
                    {
                        XmlElement deptElement = departmentElement.GetElementsByTagName("Department")[i] as XmlElement;
                        XmlAttribute xmlAttribute = deptElement.Attributes["name"];
                        deptElement.RemoveAll();
                        deptElement.Attributes.Append(xmlAttribute);
                    }
                }
                document.Save(xmlFile);
            }
        }
    }
}
