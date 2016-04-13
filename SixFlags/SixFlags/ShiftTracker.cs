using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SixFlags
{
    public enum BreakType
    {
        Lunch,
        Break
    }

    public partial class ShiftTracker : UserControl
    {
        public Dictionary<string, Department> departments;
        public string shift;
        private List<KeyValuePair<TimeSheet, BreakType[]>> currentStatus = new List<KeyValuePair<TimeSheet, BreakType[]>>();
        ToolTip tip = new ToolTip();
        Label titleLabel = new Label();
        private int timerCount = 0;

        public ShiftTracker(string shift)
        {
            InitializeComponent();

            BackColor = Color.Black;
            this.shift = shift;
            departments = new Dictionary<string, Department>();
            var document = XDocument.Load(SixFlagsTracker.TimeSheetsFile);

            tip.OwnerDraw = true;
            tip.Draw += Tip_Draw;
            tip.Popup += Tip_Popup;
            tip.ShowAlways = true;

            titleLabel.Font = new Font("Arial", 18, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.TextAlign = ContentAlignment.BottomCenter;
            titleLabel.Text = "Base Tracker";
            titleLabel.AutoSize = false;
            titleLabel.Size = titleLabel.PreferredSize;
            titleLabel.Location = new Point(Width / 2 - titleLabel.Width / 2, 0);
            Controls.Add(titleLabel);

            foreach (var depart in SixFlagsTracker.Departments)
            {
                AddDepartment(depart.Name);
            }
            List<XElement> timeSheets = document.XPathSelectElements("SixFlags/TimeSheets")
                .ToList()
                .Find(element =>
                    (String.Compare(element.Attribute("shift").Value, shift,
                        StringComparison.CurrentCultureIgnoreCase) == 0))
                .Elements()
                .ToList();
            foreach (XElement timeSheet in timeSheets.Elements())
            {
                departments[timeSheet.Parent.Attribute("name").Value].timeSheets.Add(
                    new TimeSheet(
                        timeSheet.Attribute("name").Value,
                        DateTime.Parse(timeSheet.Attribute("timeIn").Value),
                        DateTime.Parse(timeSheet.Attribute("timeOut").Value)));
                foreach (XElement Lunch in timeSheet.XPathSelectElements("Lunches/Lunch"))
                {
                    departments[timeSheet.Parent.Attribute("name").Value].timeSheets.Last().SentLunch.Add(DateTime.Parse(Lunch.Attribute("time").Value));
                }
                foreach (XElement Break in timeSheet.XPathSelectElements("Breaks/Break"))
                {
                    departments[timeSheet.Parent.Attribute("name").Value].timeSheets.Last().SentLunch.Add(DateTime.Parse(Break.Attribute("time").Value));
                }

            }
            this.Resize += ShiftTracker_Resize;
        }

        public void AddDepartment(string department)
        {
            departments.Add(department, new Department(department));
            var departmentButton = new Button();
            departmentButton.Text = department;
            departmentButton.Size = new Size(100, 100);
            departmentButton.Click += department_Click;
            departmentButton.BackColor = Color.White;
            departmentButton.MouseEnter += DepartmentButtonOnMouseEnter;
            Controls.Add(departmentButton);
            UpdatePositioning();
        }

        private void ShiftTracker_Resize(object sender, EventArgs e)
        {
            UpdatePositioning();
        }

        private void DepartmentButtonOnMouseEnter(object sender, EventArgs eventArgs)
        {
            currentStatus.Clear();
            foreach (TimeSheet timeSheet in departments[((Button)sender).Text].timeSheets)
            {
                TimeSpan timePast = DateTime.Now - timeSheet.TimeIn;

                List<BreakType> breakTypes = new List<BreakType>();
                if (timePast >=
                    TimeSpan.FromHours(5 * (timeSheet.SentLunch.Count + 1) - .75f * timeSheet.SentLunch.Count - 1))
                {
                    breakTypes.Add(BreakType.Lunch);
                }
                if (timePast >=
                    TimeSpan.FromHours(3.75f * (timeSheet.SentBreak.Count + 1) - .75f * timeSheet.SentLunch.Count - 1))
                {
                    breakTypes.Add(BreakType.Break);
                }
                if (breakTypes.Count == 0)
                {
                    continue;
                }
                currentStatus.Add(new KeyValuePair<TimeSheet, BreakType[]>(timeSheet, breakTypes.ToArray()));
            }
            tip.SetToolTip((Button)sender, "show");
        }

        public void UpdatePositioning()
        {
            if (Size.Width < 200)
            {
                return;
            }
            titleLabel.Location = new Point(Width / 2 - titleLabel.Width / 2, 0);
            List<Button> buttons = Controls.OfType<Button>().ToList();
            if (buttons.Count == 0)
            {
                return;
            }
            int y = titleLabel.Height;
            int buttonsPerRow = Width / (buttons[0].Size.Width + 50);
            float x = Width / 2f -
                      (buttonsPerRow > buttons.Count ? buttons.Count : buttonsPerRow) / 2f * (buttons[0].Width + 25) + 12.5f;

            for (int i = 0; i < buttons.Count; i++)
            {
                Button button = buttons[i];
                if (i != 0 && i % buttonsPerRow == 0)
                {
                    x = Width / 2f - (buttonsPerRow > (buttons.Count - i) ? (buttons.Count - i) : buttonsPerRow) / 2f * (buttons[0].Width + 25) + 12.5f;
                    y += button.Height + 25;
                }
                button.Location = new Point((int)x, y);
                x += button.Width + 25;
            }
        }

        private void Tip_Popup(object sender, PopupEventArgs e)
        {
            e.ToolTipSize = new Size(150, currentStatus.Count * 25);
        }

        private void Tip_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            Font font = new Font("Arial", 8);
            int y = 0;
            foreach (KeyValuePair<TimeSheet, BreakType[]> timeSheet in currentStatus)
            {
                Brush brush;
                TimeSpan timePast = DateTime.Now - timeSheet.Key.TimeIn;
                if (timePast >=
                    TimeSpan.FromHours(5 * (timeSheet.Key.SentLunch.Count + 1) - .75f * timeSheet.Key.SentLunch.Count - 15 / 60f))
                {
                    brush = Brushes.Red;
                }
                else
                {
                    if (timePast >= TimeSpan.FromHours(3.75f * (timeSheet.Key.SentBreak.Count + 1) - .75f * timeSheet.Key.SentLunch.Count - 15 / 60f))
                    {
                        brush = Brushes.Red;
                    }
                    else
                    {
                        brush = Brushes.Yellow;
                    }
                }
                string breakString = timeSheet.Value[0].ToString();

                for (int i = 1; i < timeSheet.Value.Length; i++)
                {
                    breakString += "/" + timeSheet.Value[i].ToString();
                }

                e.Graphics.FillRectangle(brush, 0, y, e.Bounds.Width - 1, 25);
                e.Graphics.DrawString(breakString + ": " + timeSheet.Key.Name, font, Brushes.Black, 0, y);
                e.Graphics.DrawRectangle(Pens.Black, 0, y, e.Bounds.Width - 1, 24);
                y += 25;
            }
        }

        private void department_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            TimeSheetsEditor editor = new TimeSheetsEditor(this, shift, departments[button.Text]);
            editor.ShowDialog();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timerCount++;
            if (timerCount % 2 == 0)
            {
                foreach (Button button in Controls.OfType<Button>())
                {
                    button.BackColor = Color.White;
                }
                return;
            }
            foreach (Department department in departments.Values)
            {
                Button departmentButton = new Button();
                foreach (Button button in Controls.OfType<Button>())
                {
                    if (String.Compare(button.Text, department.Name, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        departmentButton = button;
                        break;
                    }
                }
                foreach (TimeSheet timeSheet in department.timeSheets)
                {
                    TimeSpan timePast = DateTime.Now - timeSheet.TimeIn;
                    if (timePast >=
                        TimeSpan.FromHours(5 * (timeSheet.SentLunch.Count + 1) - .75f * timeSheet.SentLunch.Count -
                                           15 / 60f))
                    {
                        if (timePast >=
                            TimeSpan.FromHours(5 * (timeSheet.SentLunch.Count + 1) - .75f * timeSheet.SentLunch.Count -
                                               1 / 6f))
                        {
                            //More Annoying Alarm
                        }
                        else
                        {
                            //Annoying Alarm
                        }
                        departmentButton.BackColor = Color.Red;
                        break;
                    }
                    else if (timePast >=
                             TimeSpan.FromHours(3.75f * (timeSheet.SentBreak.Count + 1) -
                                                .75f * timeSheet.SentLunch.Count - 15 / 60f))
                    {
                        if (timePast >=
                            TimeSpan.FromHours(3.75f * (timeSheet.SentBreak.Count + 1) -
                                               .75f * timeSheet.SentLunch.Count - 1 / 6f))
                        {
                            //More Annoying Alarm
                        }
                        else
                        {
                            //Annoying Alarm
                        }
                        departmentButton.BackColor = Color.Red;
                        break;
                    }
                    else if (timePast >=
                             TimeSpan.FromHours(5 * (timeSheet.SentLunch.Count + 1) -
                                                .75f * timeSheet.SentLunch.Count - 1))
                    {
                        departmentButton.BackColor = Color.Yellow;
                    }
                    else if (timePast >=
                             TimeSpan.FromHours(3.75f * (timeSheet.SentBreak.Count + 1) -
                                                .75f * timeSheet.SentLunch.Count - 1))
                    {
                        departmentButton.BackColor = Color.Yellow;
                    }
                }
            }
        }
    }
}