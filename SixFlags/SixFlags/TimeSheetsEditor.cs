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
        private Area _area;
        private ShiftTracker tracker;

        public TimeSheetsEditor(ShiftTracker tracker, string shift, Area _area)
        {
            this.tracker = tracker;
            InitializeComponent();
            this.shift = shift;
            this._area = _area;
            foreach (TimeSheet timeSheet in this._area.timeSheets)
            {
                timeSheetListBox.Items.Add(timeSheet.Name);
            }
            timeSheetListBox.DrawItem += TimeSheetListBox_DrawItem;
            timeSheetListBox.SelectedIndexChanged += (sender, args) => { timeSheetListBox.Refresh(); };
            timeSheetListBox.DrawMode = DrawMode.OwnerDrawFixed;
        }

        private void TimeSheetListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index == -1)
            {
                return;
            }
            TimeSpan timePast = DateTime.Now - _area.timeSheets[e.Index].TimeIn;
            Brush brush = Brushes.White;
            if (DateTime.Now > _area.timeSheets[e.Index].TimeOut)
            {
                brush = Brushes.LightGray;
            }
            else if (timePast >= _area.timeSheets[e.Index].getNextLunch() - TimeSpan.FromHours(1 / 6f))
            {
                brush = Brushes.Red;
            }
            else if (timePast >= _area.timeSheets[e.Index].getNextBreak() - TimeSpan.FromHours(1 / 6f))
            {
                brush = Brushes.Red;
            }
            else if (timePast >= _area.timeSheets[e.Index].getNextLunch() - TimeSpan.FromHours(1 / 3f))
            {
                brush = Brushes.Yellow;
            }
            else if (timePast >= _area.timeSheets[e.Index].getNextBreak() - TimeSpan.FromHours(1 / 3f))
            {
                brush = Brushes.Yellow;
            }
            if (e.Index == timeSheetListBox.SelectedIndex)
            {
                brush = Brushes.LightBlue;
            }
            e.Graphics.FillRectangle(brush, e.Bounds);
            e.Graphics.DrawString(_area.timeSheets[e.Index].Name, e.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var timeSheetAdder = new TimeSheetAdder(tracker, _area.Name);
            timeSheetAdder.ShowDialog();

            if (timeSheetAdder.DialogResult == DialogResult.Yes)
            {
                tracker.Areas[timeSheetAdder.area].timeSheets.Add(timeSheetAdder.TimeSheet);
                if (String.Compare(timeSheetAdder.area, _area.Name, StringComparison.CurrentCultureIgnoreCase) ==
                    0)
                {
                    timeSheetListBox.Items.Add(timeSheetAdder.TimeSheet.Name);
                }

                var document = XDocument.Load(SixFlagsTracker.TimeSheetsFile);
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

                    foreach (XElement xArea in timeSheets.Elements())
                    {
                        if (xArea.Attribute("name").Value == tracker.Areas[timeSheetAdder.area].Name)
                        {
                            xArea.Add(timeSheet);
                        }
                    }
                }
                document.Save(SixFlagsTracker.TimeSheetsFile);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            int index = timeSheetListBox.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            TimeSheetEditor editor = new TimeSheetEditor(_area, _area.timeSheets[index]);
            editor.ShowDialog();
            if (editor.DialogResult == DialogResult.Yes)
            {
                XDocument document = XDocument.Load(SixFlagsTracker.TimeSheetsFile);
                List<XElement> areaElements =
                    document.XPathSelectElements("SixFlags/TimeSheets").ToList().Find(
                        element =>
                        String.Compare(element.Attribute("shift").Value, shift,
                            StringComparison.CurrentCultureIgnoreCase) == 0)
                            .XPathSelectElements("Area").ToList();
                foreach (XElement areaElement in areaElements)
                {
                    if (String.Compare(areaElement.Attribute("name").Value, _area.Name, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        areaElement.XPathSelectElements("TimeSheet").ToList().ForEach(
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
                _area.timeSheets[index] = editor.newTimeSheet;
                timeSheetListBox.Items[index] = editor.newTimeSheet.Name;
                document.Save(SixFlagsTracker.TimeSheetsFile);
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
                var document = XDocument.Load(SixFlagsTracker.TimeSheetsFile);
                var timeSheetsElements = document.XPathSelectElements("SixFlags/TimeSheets");
                foreach (XElement timeSheetsElement in timeSheetsElements)
                {
                    if (timeSheetsElement.Attribute("shift").Value.ToLower() == shift.ToLower())
                    {
                        var areaElements = timeSheetsElement.XPathSelectElements("Area");

                        foreach (XElement areaElement in areaElements)
                        {
                            if (areaElement.Attribute("name").Value.ToLower() == _area.Name.ToLower())
                            {
                                var timeSheetElements = areaElement.XPathSelectElements("TimeSheet");
                                foreach (XElement timeSheetElement in timeSheetElements)
                                {
                                    if (String.Equals(  timeSheetElement.Attribute("name").Value, 
                                                        timeSheetListBox.Items[index].ToString(), 
                                                        StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        timeSheetElement.Remove();
                                    }
                                }
                            }
                        }
                    }
                }

                document.Save(SixFlagsTracker.TimeSheetsFile);
                _area.timeSheets.RemoveAt(index);
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
            XDocument document = XDocument.Load(SixFlagsTracker.TimeSheetsFile);
            if (sendLunchButton.Text == "Undo Lunch")
            {
                _area.timeSheets[index].SentLunch.RemoveAt(_area.timeSheets[index].SentLunch.Count - 1);
                foreach (var timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                {
                    if (timeSheets.Attribute("shift").Value != shift.ToLower())
                    {
                        continue;
                    }

                    foreach (XElement xArea in timeSheets.Elements())
                    {
                        if (xArea.Attribute("name").Value == _area.Name)
                        {
                            xArea.XPathSelectElements("TimeSheet").ToList().ForEach(element =>
                            {
                                if (String.Compare(element.Attribute("name").Value, _area.timeSheets[index].Name,
                                    StringComparison.CurrentCultureIgnoreCase) == 0)
                                {
                                    element.XPathSelectElements("Lunches/Lunch").Last().Remove();
                                }
                            });
                        }
                    }
                }
                document.Save(SixFlagsTracker.TimeSheetsFile);
                return;
            }
            else
            {
                _area.timeSheets[index].SentLunch.Add(DateTime.Now);
                foreach (var timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                {
                    if (timeSheets.Attribute("shift").Value != shift.ToLower())
                    {
                        continue;
                    }

                    foreach (XElement xArea in timeSheets.Elements())
                    {
                        if (xArea.Attribute("name").Value == _area.Name)
                        {
                            xArea.XPathSelectElements("TimeSheet").ToList().ForEach(element =>
                            {
                                if (String.Compare(element.Attribute("name").Value, _area.timeSheets[index].Name,
                                    StringComparison.CurrentCultureIgnoreCase) == 0)
                                {
                                    XElement lunchElement = new XElement("Lunch");
                                    lunchElement.SetAttributeValue("time",
                                        _area.timeSheets[index].SentLunch.Last().ToString());
                                    element.XPathSelectElement("Lunches").Add(lunchElement);
                                }
                            });
                        }
                    }
                }
            }
            document.Save(SixFlagsTracker.TimeSheetsFile);
        }

        private void sendBreakButton_Click(object sender, EventArgs e)
        {
            int index = timeSheetListBox.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            XDocument document = XDocument.Load(SixFlagsTracker.TimeSheetsFile);
            if (sendBreakButton.Text == "Undo Break")
            {
                _area.timeSheets[index].SentBreak.RemoveAt(_area.timeSheets[index].SentBreak.Count - 1);
                foreach (var timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                {
                    if (timeSheets.Attribute("shift").Value != shift.ToLower())
                    {
                        continue;
                    }

                    foreach (XElement xArea in timeSheets.Elements())
                    {
                        if (xArea.Attribute("name").Value == _area.Name)
                        {
                            xArea.XPathSelectElements("TimeSheet").ToList().ForEach(element =>
                            {
                                if (String.Compare(element.Attribute("name").Value, _area.timeSheets[index].Name,
                                    StringComparison.CurrentCultureIgnoreCase) == 0)
                                {
                                    element.XPathSelectElements("Breaks/Break").Last().Remove();
                                }
                            });
                        }
                    }
                }
                document.Save(SixFlagsTracker.TimeSheetsFile);
                return;
            }
            else
            {
                _area.timeSheets[index].SentBreak.Add(DateTime.Now);
                foreach (var timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
                {
                    if (timeSheets.Attribute("shift").Value != shift.ToLower())
                    {
                        continue;
                    }

                    foreach (XElement xArea in timeSheets.Elements())
                    {
                        if (xArea.Attribute("name").Value == _area.Name)
                        {
                            xArea.XPathSelectElements("TimeSheet").ToList().ForEach(element =>
                            {
                                if (String.Compare(element.Attribute("name").Value, _area.timeSheets[index].Name,
                                    StringComparison.CurrentCultureIgnoreCase) == 0)
                                {
                                    XElement breakElement = new XElement("Break");
                                    breakElement.SetAttributeValue("time",
                                        _area.timeSheets[index].SentBreak.Last().ToString());
                                    element.XPathSelectElement("Breaks").Add(breakElement);
                                }
                            });
                        }
                    }
                }
            }
            document.Save(SixFlagsTracker.TimeSheetsFile);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (timeSheetListBox.SelectedIndex == -1 || _area.timeSheets.Count <= timeSheetListBox.SelectedIndex)
            {
                return;
            }
            TimeSheet timeSheet = _area.timeSheets[timeSheetListBox.SelectedIndex];
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

        private void endShiftButton_Click(object sender, EventArgs e)
        {
            int index = timeSheetListBox.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            _area.timeSheets[index].TimeOut = DateTime.Now;
            var document = XDocument.Load(SixFlagsTracker.TimeSheetsFile);
            foreach (var timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
            {
                if (timeSheets.Attribute("shift").Value != shift.ToLower())
                {
                    continue;
                }

                foreach (XElement xArea in timeSheets.Elements())
                {
                    if (xArea.Attribute("name").Value == _area.Name)
                    {
                        xArea.XPathSelectElements("TimeSheet").ToList().ForEach(element =>
                        {
                            if (String.Compare(element.Attribute("name").Value, _area.timeSheets[index].Name,
                                StringComparison.CurrentCultureIgnoreCase) == 0)
                            {
                                element.SetAttributeValue("timeOut", _area.timeSheets[index].TimeOut.ToString());
                            }
                        });
                    }
                }
            }
            _area.EndedTimeSheets.Add(_area.timeSheets[index]);
            _area.timeSheets.RemoveAt(index);
            timeSheetListBox.Items.RemoveAt(index);
            document.Save(SixFlagsTracker.TimeSheetsFile);
        }
    }
}
