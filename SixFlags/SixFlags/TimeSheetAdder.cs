using System;
using System.Windows.Forms;

namespace SixFlags
{
    public partial class TimeSheetAdder : Form
    {
        public string area;
        public TimeSheet TimeSheet;
        private ShiftTracker tracker;

        public TimeSheetAdder(ShiftTracker tracker, string area)
        {
            InitializeComponent();
            this.area = area;
            this.tracker = tracker;
            TimeSheet = null;
            foreach (var depart in SixFlagsTracker.Areas)
            {
                areaComboBox.Items.Add(depart.Name);
            }

            areaComboBox.SelectedItem = area;

            timeInPicker.Text = DateTime.Now.TimeOfDay.ToString();
            timeOutPicker.Text = DateTime.Now.AddHours(8.75f).TimeOfDay.ToString();

            dateTimePicker.Items.Add(DateTime.Today.AddDays(-1));
            dateTimePicker.Items.Add(DateTime.Today);
            dateTimePicker.Items.Add(DateTime.Today.AddDays(1));
            dateTimePicker.SelectedIndex = 1;
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text.Trim() == "")
            {
                CenteredMessageBox.Show("Enter a name", "Error", MessageBoxButtons.OK);
                return;
            }
            foreach (TimeSheet timeSheet in tracker.Areas[area].timeSheets)
            {
                if (String.Compare(timeSheet.Name.Trim(), nameTextBox.Text.Trim(),
                        StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    CenteredMessageBox.Show("Name is already in area", "Error", MessageBoxButtons.OK);
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
            area = areaComboBox.Text;
            TimeSheet = new TimeSheet(
                nameTextBox.Text,
                dateIn,
                dateOut);
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }
    }
}