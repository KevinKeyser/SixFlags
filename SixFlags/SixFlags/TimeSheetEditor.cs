using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SixFlags
{
    public partial class TimeSheetEditor : Form
    {
        public string oldName;
        private Department department;
        public TimeSheet newTimeSheet;
        private List<DateTime> lunches;
        private List<DateTime> breaks;
        private TimeSheet oldTimeSheet;

        public TimeSheetEditor(Department department, TimeSheet timeSheet)
        {
            this.department = department;
            oldTimeSheet = timeSheet;
            InitializeComponent();
            oldName = timeSheet.Name;
            nameTextBox.Text = oldName;
            newTimeSheet = new TimeSheet(timeSheet.Name, timeSheet.TimeIn, timeSheet.TimeOut);
            lunches = timeSheet.SentLunch;
            breaks = timeSheet.SentBreak;

            timeInPicker.Text = timeSheet.TimeIn.ToString();
            timeOutPicker.Text = timeSheet.TimeOut.ToString();

            dateTimePicker.Items.Add(timeSheet.TimeIn.Date);
            dateTimePicker.Items.Add(DateTime.Today.AddDays(-1));
            dateTimePicker.Items.Add(DateTime.Today);
            dateTimePicker.Items.Add(DateTime.Today.AddDays(1));
            dateTimePicker.SelectedIndex = 0;

        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text.Trim() == "")
            {
                CenteredMessageBox.Show("Enter a name", "Error", MessageBoxButtons.OK);
                return;
            }
            foreach (TimeSheet timeSheet in department.timeSheets)
            {
                if (timeSheet == oldTimeSheet)
                {
                    continue;
                }
                if (String.Compare(timeSheet.Name.Trim(), nameTextBox.Text.Trim(),
                        StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    CenteredMessageBox.Show("Name is already in department", "Error", MessageBoxButtons.OK);
                    return;
                }
            }
            var timeIn = TimeSpan.Parse(timeInPicker.Text);
            var timeOut = TimeSpan.Parse(timeOutPicker.Text);
            var dateIn = DateTime.Parse(dateTimePicker.Text).Subtract(-timeIn);
            var dateOut =
                DateTime.Parse(dateTimePicker.Text)
                    .Subtract(TimeSpan.FromDays(timeIn > timeOut ? -1 : 0))
                    .Subtract(-timeOut);
            DialogResult = DialogResult.Yes;
            newTimeSheet.Name = nameTextBox.Text;
            newTimeSheet.TimeIn = dateIn;
            newTimeSheet.TimeOut = dateOut;
            newTimeSheet.SentBreak = oldTimeSheet.SentBreak;
            newTimeSheet.SentLunch = oldTimeSheet.SentLunch;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }
    }
}