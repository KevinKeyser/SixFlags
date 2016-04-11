using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SixFlags
{
    public partial class TimeSheetEditor : Form
    {
        public string oldName;
        public TimeSheet newTimeSheet;
        private List<DateTime> lunches;
        private List<DateTime> breaks;

        public TimeSheetEditor(TimeSheet timeSheet)
        {
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
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }
    }
}