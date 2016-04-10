using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SixFlags
{
    public partial class ShiftTracker : UserControl
    {
        private Dictionary<string, Department> departments;
        private string shift;
        private List<TimeSheet> currentStatus = new List<TimeSheet>();
        ToolTip tip = new ToolTip();
        Label titleLabel = new Label();

        public ShiftTracker(string shift)
        {
            InitializeComponent();

            BackColor = Color.Black;
            this.shift = shift;
            departments = new Dictionary<string, Department>();
            var document = XDocument.Load(SixFlagsTracker.xmlFile);
            var x = 0;
            var y = 50;
            tip.OwnerDraw = true;
            tip.Draw += Tip_Draw;
            tip.Popup += Tip_Popup;
            tip.ShowAlways = true;
            foreach (var depart in SixFlagsTracker.Departments)
            {
                departments.Add(depart.Name, new Department(depart.Name));
                var departmentButton = new Button();
                departmentButton.Text = depart.Name;
                departmentButton.Location = new Point(x, y);
                departmentButton.Size = new Size(100, 100);
                departmentButton.Click += department_Click;
                departmentButton.BackColor = Color.White;
                departmentButton.MouseEnter += DepartmentButtonOnMouseEnter;
                Controls.Add(departmentButton);
            }

            foreach (var timeSheets in document.XPathSelectElements("SixFlags/TimeSheets"))
            {
                if (timeSheets.Attribute("shift").Value != shift.ToLower())
                {
                    continue;
                }
                foreach (var timeSheet in timeSheets.XPathSelectElements("Department/TimeSheets"))
                {
                    departments[timeSheet.Parent.Attribute("name").Value].timeSheets.Add(
                        new TimeSheet(
                            timeSheet.Attribute("name").Value,
                            DateTime.Parse(timeSheet.Attribute("timeIn").Value),
                            DateTime.Parse(timeSheet.Attribute("timeOut").Value)));
                }
            }
            titleLabel.Font = new Font("Arial", 18, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.TextAlign = ContentAlignment.BottomCenter;
            titleLabel.Text = "Base Tracker";
            titleLabel.AutoSize = false;
            titleLabel.Size = titleLabel.PreferredSize;
            titleLabel.Location = new Point(Width/2 - titleLabel.Width/2, 0);
            Controls.Add(titleLabel);
            this.Resize += ShiftTracker_Resize;
        }

        private void ShiftTracker_Resize(object sender, EventArgs e)
        {
            titleLabel.Location = new Point(Width / 2 - titleLabel.Width / 2, 0);
            List<Button> buttons = Controls.OfType<Button>().ToList();
            if (buttons.Count == 0)
            {
                return;
            }
            int y = titleLabel.Height;
            int buttonsPerRow = Width / (buttons[0].Size.Width + 50);
            float x = Width/2f -
                      (buttonsPerRow > buttons.Count ? buttons.Count : buttonsPerRow)/2f*(buttons[0].Width + 25 * 1.5f);

            for(int i = 0; i < buttons.Count; i++)
            {
                Button button = buttons[i];
                if (i != 0 && i % buttonsPerRow == 0)
                {
                    x = Width / 2f - (buttonsPerRow > (buttons.Count - i) ? (buttons.Count - i) : buttonsPerRow) / 2f * (buttons[0].Width + 25 * 1.5f);
                    y += button.Height + 25;
                }
                button.Location = new Point((int)x, y);
                x += button.Width + 25;
            }
        }

        private void DepartmentButtonOnMouseEnter(object sender, EventArgs eventArgs)
        {
            currentStatus.Clear();

            foreach (TimeSheet timeSheet in departments[((Button)sender).Text].timeSheets)
            {
                TimeSpan timePast = DateTime.Now - timeSheet.TimeIn;
                if (timePast.TotalHours < 0)
                {
                    timePast = timePast.Subtract(-TimeSpan.FromDays(1));
                }
                if (timeSheet.SentLunch == 0)
                {
                    if (timePast >= TimeSpan.FromHours(4))
                    {
                        currentStatus.Add(timeSheet);
                    }
                }
            }
            tip.SetToolTip((Button)sender, "show");
        }

        private void Tip_Popup(object sender, PopupEventArgs e)
        {
            e.ToolTipSize = new Size(100, currentStatus.Count * 25);
        }

        private void Tip_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            Font font = new Font("Arial", 8);
            int y = 0;
            foreach (TimeSheet timeSheet in currentStatus)
            {
                Brush brush;
                TimeSpan timePast = DateTime.Now - timeSheet.TimeIn;
                if (timePast.TotalHours < 0)
                {
                    timePast = timePast.Subtract(-TimeSpan.FromDays(1));
                }

                if (timePast >= TimeSpan.FromHours(5))
                {
                    brush = Brushes.Red;
                }
                else
                {
                    brush = Brushes.Yellow;
                }
                e.Graphics.FillRectangle(brush, 0, y, e.Bounds.Width - 1, 25);
                e.Graphics.DrawString(timeSheet.Name, font, Brushes.Black, 0, y);
                e.Graphics.DrawRectangle(Pens.Black, 0, y, e.Bounds.Width - 1, 24);
                y += 25;
            }
        }

        private void department_Click(object sender, EventArgs e)
        {
            var button = (Button) sender;

            DepartmentEditor editor = new DepartmentEditor(shift, departments[button.Text]);
            editor.ShowDialog();

        }

        private void timer_Tick(object sender, EventArgs e)
        {
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
                    if (timePast.TotalHours < 0)
                    {
                        timePast = timePast.Subtract(-TimeSpan.FromDays(1));
                    }
                    if (timeSheet.SentLunch == 0)
                    {
                        if (timePast >= TimeSpan.FromHours(5))
                        {
                            departmentButton.BackColor = Color.Red;
                        }
                        else if (timePast >= TimeSpan.FromHours(4))
                        {
                            departmentButton.BackColor = Color.Yellow;
                        }
                    }
                    else
                    {
                        departmentButton.BackColor = Color.White;
                    }
                }
            }
        }
    }
}