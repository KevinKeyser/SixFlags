using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Office.Interop.Excel;
using Button = System.Windows.Forms.Button;

namespace SixFlags
{
    public partial class SixFlagsTracker : Form
    {
        public static string TimeSheetsFile = DateTime.Now.ToShortDateString().Replace("/", "-") + "-TimeSheets.xml";
        public static string SavedDataFile = "SavedData.xml";
        public static List<Area> Areas;
        public static DateTime currentSelectedDate;

        public SixFlagsTracker()
        {
            Areas = new List<Area>();
            XDocument document;
            currentSelectedDate = DateTime.Now;

            #region Create SavedData

            if (!File.Exists(Environment.CurrentDirectory + "/" + SavedDataFile))
            {
                document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("SavedData",
                        new XElement("Areas"),
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
                    XElement areaElement in SavedDocument.XPathSelectElements("SavedData/Areas/Area"))
                {
                    foreach (XElement timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                    {
                        timeSheets.Add(areaElement);
                    }
                }
                document.Save(TimeSheetsFile);
            }

            #endregion

            //Add All Areas to Reference List
            document = XDocument.Load(SavedDataFile);
            document.XPathSelectElements("SavedData/Areas/Area")
                .ToList()
                .ForEach(element =>
                {
                    Areas.Add(new Area(element.Attribute("name").Value));
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
                            XElement areaElement in SavedDocument.XPathSelectElements("SavedData/Areas/Area"))
                        {
                            foreach (XElement timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                            {
                                timeSheets.Add(areaElement);
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

                        tracker.Areas.Keys.ToList()
                        .ForEach(depart => tracker.Areas[depart].timeSheets.Clear());


                        List<XElement> timeSheets = document.XPathSelectElements("SixFlags/TimeSheets")
                                                    .ToList()
                                                    .Find(element =>
                                                        (String.Compare(element.Attribute("shift").Value, tracker.shift,
                                                            StringComparison.CurrentCultureIgnoreCase) == 0))
                                                    .Elements()
                                                    .ToList();
                        foreach (XElement timeSheet in timeSheets.Elements())
                        {
                            tracker.Areas[timeSheet.Parent.Attribute("name").Value].timeSheets.Add(
                                new TimeSheet(
                                    timeSheet.Attribute("name").Value,
                                    DateTime.Parse(timeSheet.Attribute("timeIn").Value),
                                    DateTime.Parse(timeSheet.Attribute("timeOut").Value)));
                            foreach (XElement Lunch in timeSheet.XPathSelectElements("Lunches/Lunch"))
                            {
                                tracker.Areas[timeSheet.Parent.Attribute("name").Value].timeSheets.Last().SentLunch.Add(DateTime.Parse(Lunch.Attribute("time").Value));
                            }
                            foreach (XElement Break in timeSheet.XPathSelectElements("Breaks/Break"))
                            {
                                tracker.Areas[timeSheet.Parent.Attribute("name").Value].timeSheets.Last().SentLunch.Add(DateTime.Parse(Break.Attribute("time").Value));
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
            var add = new AreaAdd();
            add.ShowDialog();
            if (add.DialogResult == DialogResult.Yes)
            {
                //Add Area to SavedData
                var document = XDocument.Load(SavedDataFile);
                var departElement = new XElement("Area");
                departElement.SetAttributeValue("name", add.Area);
                document.XPathSelectElement("SavedData/Areas")
                    .Add(departElement);
                document.Save(SavedDataFile);

                //Add Area to Current TimeSheets
                document = XDocument.Load(TimeSheetsFile);
                document.XPathSelectElements("SixFlags/TimeSheets")
                    .ToList()
                    .ForEach(element =>
                            element.Add(departElement));
                document.Save(TimeSheetsFile);

                //Add Area to Reference List
                Areas.Add(new Area(add.Area));

                //Add Button to Tabs
                foreach (TabPage tabPage in shiftTimes.TabPages)
                {
                    ShiftTracker tracker = (ShiftTracker)tabPage.Controls[0];
                    tracker.AddArea(add.Area);
                }
            }
        }

        private void EditTool_Click(object sender, EventArgs e)
        {
            var editor = new AreaEditor();
            editor.ShowDialog();
            if (editor.DialogResult == DialogResult.Yes)
            {
                //Edit Areas in SavedData
                var document = XDocument.Load(SavedDataFile);
                document.XPathSelectElements("SavedData/Areas/Area")
                    .ToList()
                    .FindAll(element =>
                        String.Compare(element.Attribute("name").Value, editor.Area,
                            StringComparison.CurrentCultureIgnoreCase) == 0)
                    .ForEach(element =>
                    {
                        element.SetAttributeValue("name", editor.newName);
                    });
                document.Save(SavedDataFile);

                //Edit Area in Current TimeSheets
                document = XDocument.Load(TimeSheetsFile);
                document.XPathSelectElements("SixFlags/TimeSheets/Area")
                    .ToList()
                    .FindAll(element =>
                        String.Compare(element.Attribute("name").Value, editor.Area,
                            StringComparison.CurrentCultureIgnoreCase) == 0)
                    .ForEach(element =>
                    {
                        element.SetAttributeValue("name", editor.newName);
                    });
                document.Save(TimeSheetsFile);

                //Edit Area in Reference List
                Areas.Find(area => String.Compare(area.Name, editor.Area,
                    StringComparison.CurrentCultureIgnoreCase) == 0).Name = editor.newName;

                //Edit Button from Tabs
                foreach (TabPage tabPage in shiftTimes.TabPages)
                {
                    ShiftTracker tracker = (ShiftTracker)tabPage.Controls[0];
                    Area dept = tracker.Areas[editor.Area];
                    tracker.Areas.Remove(editor.Area);
                    tracker.Areas.Add(editor.newName, dept);
                    List<Button> buttons = tracker.Controls.OfType<Button>().ToList();
                    foreach (Button button in buttons)
                    {
                        if (String.Compare(button.Text, editor.Area, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            button.Text = editor.newName;
                        }
                    }
                }
            }
        }

        private void RemoveTool_Click(object sender, EventArgs e)
        {
            var removal = new AreaRemoval();
            removal.ShowDialog();
            if (removal.DialogResult == DialogResult.Yes)
            {
                var areaDelete = removal.Area;

                //Remove Area From Saved Data
                var document = XDocument.Load(SavedDataFile);
                document.XPathSelectElements("SavedData/Areas/Area")
                    .ToList()
                    .FindAll(element => String.Compare(element.Attribute("name").Value, areaDelete,
                        StringComparison.CurrentCultureIgnoreCase) == 0)
                    .ForEach(element =>
                {
                    element.Remove();
                });
                document.Save(SavedDataFile);

                //Remove Area from Current TimeSheets
                document = XDocument.Load(TimeSheetsFile);
                document.XPathSelectElements("SixFlags/TimeSheets/Area")
                    .ToList()
                    .FindAll(element => String.Compare(element.Attribute("name").Value, areaDelete,
                        StringComparison.CurrentCultureIgnoreCase) == 0)
                    .ForEach(element =>
                    {
                        element.Remove();
                    });

                document.Save(TimeSheetsFile);

                //Remove Area from ReferenceList
                Areas.Remove(
                    Areas.Find(area => String.Compare(area.Name, areaDelete,
                        StringComparison.CurrentCultureIgnoreCase) == 0));

                //Remove Button from Tabs
                foreach (TabPage tabPage in shiftTimes.TabPages)
                {
                    ShiftTracker tracker = (ShiftTracker)tabPage.Controls[0];
                    List<Button> buttons = tracker.Controls.OfType<Button>().ToList();
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        if (String.Compare(buttons[i].Text, areaDelete, StringComparison.CurrentCultureIgnoreCase) == 0)
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
                Areas.ForEach(area =>
                {
                    area.timeSheets.Clear();
                });

                //Remove All TimeSheets from Tab Reference
                foreach (TabPage tabPage in shiftTimes.TabPages)
                {
                    ShiftTracker shiftTracker = (ShiftTracker)tabPage.Controls[0];
                    foreach (string key in shiftTracker.Areas.Keys)
                    {
                        shiftTracker.Areas[key].timeSheets.Clear();
                    }
                }
            }
        }

        private void fileSaveExcelTool_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application xlApplication = new Microsoft.Office.Interop.Excel.Application();


            if (xlApplication == null)
            {
                CenteredMessageBox.Show(
                    "EXCEL could not be started. Check that your office installation and project references are correct.",
                    "Error", MessageBoxButtons.OK);
                return;
            }

            Microsoft.Office.Interop.Excel.Workbook workbook = xlApplication.Workbooks.Open(Environment.CurrentDirectory + @"\SixFlagsTemplate.xlsx");

            Microsoft.Office.Interop.Excel.Worksheet excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlApplication.Sheets.get_Item("Security Day Shift Plan");


            Microsoft.Office.Interop.Excel.Range range = xlApplication.Range["A1", "R68"];
            foreach (TabPage tabPage in shiftTimes.TabPages)
            {
                ShiftTracker tracker = (ShiftTracker)tabPage.Controls[0];
                foreach (string area in tracker.Areas.Keys)
                {
                    Microsoft.Office.Interop.Excel.Range curentCell = null;

                    bool findByIndex = true;
                    curentCell = range.Find(area);
                    if (curentCell != null)
                    {
                        findByIndex = false;
                    }
                    Microsoft.Office.Interop.Excel.Range original = curentCell;

                    for (int i = 0, j = 0; i < tracker.Areas[area].timeSheets.Count; i++)
                    {
                        TimeSheet timeSheet = tracker.Areas[area].timeSheets[i];
                        if (findByIndex)
                        {
                            curentCell = range.Find($"{area} {i + 1}");
                        }
                        else
                        {
                            if (i > 0)
                            {
                                curentCell = range.FindNext(original);
                                original = curentCell;
                            }
                        }
                        if (curentCell != null)
                        {
                            curentCell = curentCell.Next;
                            curentCell.Value2 = timeSheet.Name;
                            curentCell = curentCell.Next;
                            curentCell = curentCell.Next;
                            for (int ii = 0; ii < 2; ii++)
                            {
                                if (timeSheet.SentBreak.Count > ii)
                                {
                                    curentCell.NumberFormat = "hh:mm:ss";
                                    curentCell.Value2 = timeSheet.SentBreak[ii].TimeOfDay.ToString();
                                }

                                curentCell = curentCell.Next;
                                if (timeSheet.SentLunch.Count > ii)
                                {
                                    curentCell.NumberFormat = "hh:mm:ss";
                                    curentCell.Value2 = timeSheet.SentLunch[ii].TimeOfDay.ToString();
                                }
                                curentCell = curentCell.Next;
                            }
                            if (timeSheet.SentBreak.Count > 2)
                            {
                                curentCell.NumberFormat = "hh:mm:ss";
                                curentCell.Value2 = timeSheet.SentBreak[2].TimeOfDay.ToString();
                            }
                        }
                    }
                }
            }
            xlApplication.DisplayAlerts = false;
            workbook.SaveAs(Environment.CurrentDirectory + $@"/SixFlags{currentSelectedDate.Date.ToShortDateString().Replace("/", "_")}.xlsx");
            workbook.Close();
        }
    }
}