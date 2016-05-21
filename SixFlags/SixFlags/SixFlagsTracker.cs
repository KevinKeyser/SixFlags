using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;


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
                        new XElement("SaveLocation", new XAttribute("value", Environment.CurrentDirectory)),
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
            SpreadsheetDocument document = SpreadsheetDocument.Open( "SixFlagsTemplate.xlsx", true);

            foreach (TabPage tabPage in shiftTimes.TabPages)
            {
                ShiftTracker tracker = (ShiftTracker)tabPage.Controls[0];
                string title = tabPage.Text == "Mid" ? "Night" : "Day";
                WorksheetPart worksheet = GetWorksheetPartByName(document, $"Security {title} Shift Plan");
                SheetData sheetData = worksheet.Worksheet.Elements<SheetData>().First();
                foreach (string area in tracker.Areas.Keys)
                {

                    bool findByIndex = true;
                    Cell currentCell = FindCellByValue(document, sheetData, area);
                    if (currentCell != null)
                    {
                        findByIndex = false;
                    }

                    for (int i = 0, j = 0; i < tracker.Areas[area].timeSheets.Count; i++)
                    {
                        TimeSheet timeSheet = tracker.Areas[area].timeSheets[i];
                        if (findByIndex)
                        {
                            currentCell = FindCellByValue(document, sheetData, $"{area} {i + 1}");
                        }
                        else
                        {
                            if (i > 0)
                            {
                                string row = Regex.Match(currentCell.CellReference, @"\d+").Value;
                                string column = Regex.Match(currentCell.CellReference, @"\D+").Value;

                                currentCell = GetCell(sheetData, column.Remove(column.Length - 1, 1) + (char)(column[column.Length - 1] + 1) + row);
                            }
                        }
                        if (currentCell != null)
                        {
                            currentCell = nextColumn(sheetData, currentCell);
                            currentCell.CellValue = new CellValue(
                                InsertSharedStringItem(timeSheet.Name, document.WorkbookPart.SharedStringTablePart).ToString());
                            currentCell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                            currentCell = nextColumn(sheetData, currentCell);
                            currentCell = nextColumn(sheetData, currentCell);
                            for (int ii = 0; ii < 2; ii++)
                            {
                                if (timeSheet.SentBreak.Count > ii)
                                {
                                    currentCell.CellValue = new CellValue(timeSheet.SentBreak[ii].ToString("HH:mm:ss"));
                                    currentCell.DataType = new EnumValue<CellValues>(CellValues.String);
                                }

                                currentCell = nextColumn(sheetData, currentCell);
                                if (timeSheet.SentLunch.Count > ii)
                                {
                                    currentCell.CellValue = new CellValue(timeSheet.SentLunch[ii].ToString("HH:mm:ss"));
                                    currentCell.DataType = new EnumValue<CellValues>(CellValues.String);
                                }
                                currentCell = nextColumn(sheetData, currentCell);
                            }
                            if (timeSheet.SentBreak.Count > 2)
                            {
                                currentCell.CellValue = new CellValue(timeSheet.SentBreak[2].ToString("HH:mm:ss"));
                                currentCell.DataType = new EnumValue<CellValues>(CellValues.String);
                            }
                        }
                    }
                    for (int i = 0, j = 0; i < tracker.Areas[area].EndedTimeSheets.Count; i++)
                    {
                        TimeSheet timeSheet = tracker.Areas[area].EndedTimeSheets[i];
                        if (findByIndex)
                        {
                            currentCell = FindCellByValue(document, sheetData, $"{area} {i + 1}");
                        }
                        else
                        {
                            if (i > 0)
                            {
                                string row = Regex.Match(currentCell.CellReference, @"\d+").Value;
                                string column = Regex.Match(currentCell.CellReference, @"\D+").Value;

                                currentCell = GetCell(sheetData, column.Remove(column.Length - 1, 1) + (char)(column[column.Length - 1] + 1) + row);
                            }
                        }
                        if (currentCell != null)
                        {
                            currentCell = nextColumn(sheetData, currentCell);
                            currentCell.CellValue = new CellValue(
                                InsertSharedStringItem(timeSheet.Name, document.WorkbookPart.SharedStringTablePart).ToString());
                            currentCell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                            currentCell = nextColumn(sheetData, currentCell);
                            currentCell = nextColumn(sheetData, currentCell);
                            for (int ii = 0; ii < 2; ii++)
                            {
                                if (timeSheet.SentBreak.Count > ii)
                                {
                                    currentCell.CellValue = new CellValue(timeSheet.SentBreak[ii].ToString("HH:mm:ss"));
                                    currentCell.DataType = new EnumValue<CellValues>(CellValues.String);
                                }

                                currentCell = nextColumn(sheetData, currentCell);
                                if (timeSheet.SentLunch.Count > ii)
                                {
                                    currentCell.CellValue = new CellValue(timeSheet.SentLunch[ii].ToString("HH:mm:ss"));
                                    currentCell.DataType = new EnumValue<CellValues>(CellValues.String);
                                }
                                currentCell = nextColumn(sheetData, currentCell);
                            }
                            if (timeSheet.SentBreak.Count > 2)
                            {
                                currentCell.CellValue = new CellValue(timeSheet.SentBreak[2].ToString("HH:mm:ss"));
                                currentCell.DataType = new EnumValue<CellValues>(CellValues.String);
                            }
                        }
                    }
                }
                worksheet.Worksheet.Save();
            }


            var xmlDocument = XDocument.Load(SavedDataFile);

            string path = xmlDocument.XPathSelectElements("SavedData/SaveLocation").Attributes("value").First().Value;
            path = path == "" ? Environment.CurrentDirectory : path;
            SpreadsheetDocument newDoc = SpreadsheetDocument.Create($"{path}/SixFlags_{currentSelectedDate.Date.ToShortDateString().Replace("/", "_")}.xlsx", SpreadsheetDocumentType.Workbook);
            
            newDoc.DeleteParts<OpenXmlPart>(newDoc.GetPartsOfType<OpenXmlPart>());

            foreach (OpenXmlPart part in document.GetPartsOfType<OpenXmlPart>())
            {
                OpenXmlPart newPart = newDoc.AddPart<OpenXmlPart>(part);
            }

            newDoc.Save();
            newDoc.Close();
            document.Close();
            

        }

        private Cell nextColumn(SheetData sheetData, Cell cell)
        {
            string row = Regex.Match(cell.CellReference, @"\d+").Value;
            string column = Regex.Match(cell.CellReference, @"\D+").Value;

            return GetCell(sheetData, column.Remove(column.Length - 1, 1) + (char)(column[column.Length - 1] + 1) + row);
        }

        private Cell nextRow(SheetData sheetData, Cell cell)
        {
            string row = Regex.Match(cell.CellReference, @"\d+").Value;
            string column = Regex.Match(cell.CellReference, @"\D+").Value;

            return GetCell(sheetData, column + (row + 1));
        }

        private Cell GetCell(SheetData sheetData, string cellAddress)
        {
            uint rowIndex = uint.Parse(Regex.Match(cellAddress, @"[0-9]+").Value);
            return sheetData.Descendants<Row>().FirstOrDefault(p => p.RowIndex == rowIndex).Descendants<Cell>().FirstOrDefault(p => p.CellReference == cellAddress);
        }

        private int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }

        private WorksheetPart GetWorksheetPartByName(SpreadsheetDocument document, string sheetName)
        {
            IEnumerable<Sheet> sheets =
               document.WorkbookPart.Workbook.GetFirstChild<Sheets>().
               Elements<Sheet>().Where(s => s.Name == sheetName);
            
            
            if (sheets.Count() == 0)
            {
                // The specified worksheet does not exist.

                return null;
            }

            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)
                 document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart;

        }

        private Cell FindCellByValue(SpreadsheetDocument document, SheetData sheetData, string value)
        {
            var values = document.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ToArray();


            foreach (Row row in sheetData.Elements<Row>())
            {
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.DataType != null)
                    {
                        switch (cell.DataType.Value)
                        {
                            case CellValues.String:
                                if (cell.CellValue.Text == value)
                                {
                                    return cell;
                                }
                                break;
                            case CellValues.Boolean:
                                if (cell.CellValue.Text == value)
                                {
                                    return cell;
                                }
                                break;
                            case CellValues.Date:
                                if (cell.CellValue.Text == value)
                                {
                                    return cell;
                                }
                                break;
                            case CellValues.SharedString:
                                if (values[int.Parse(cell.CellValue.Text)].InnerText == value)
                                {
                                    return cell;
                                }
                                break;
                            case CellValues.InlineString:
                                if (cell.CellValue.Text == value)
                                {
                                    return cell;
                                }
                                break;
                            case CellValues.Number:
                                if (cell.CellValue.Text == value)
                                {
                                    return cell;
                                }
                                break;
                            case CellValues.Error:
                                if (cell.CellValue.Text == value)
                                {
                                    return cell;
                                }
                                break;
                        }
                    }
                }
            }
            return null;
        }

        private void editSettingsExcel_Click(object sender, EventArgs e)
        {
            var document = XDocument.Load(SavedDataFile);

            string path = document.XPathSelectElements("SavedData/SaveLocation").Attributes("value").First().Value;

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = path;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                document.XPathSelectElements("SavedData/SaveLocation").Attributes("value").First().Value = dialog.SelectedPath;
                document.Save(SavedDataFile);
            }
        }
    }
}