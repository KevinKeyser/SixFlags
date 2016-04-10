using System;
using System.Windows.Forms;

namespace SixFlags
{
    public partial class TimeSheetAdder : Form
    {
        public string Department;
        public TimeSheet TimeSheet;

        public TimeSheetAdder(string department)
        {
            InitializeComponent();
            Department = department;
            TimeSheet = null;
            foreach (var depart in SixFlagsTracker.Departments)
            {
                departmentComboBox.Items.Add(depart.Name);
            }

            departmentComboBox.SelectedItem = department;

            timeInPicker.Text = DateTime.Now.TimeOfDay.ToString();
            timeOutPicker.Text = DateTime.Now.AddHours(8.75f).TimeOfDay.ToString();

            dateTimePicker.Items.Add(DateTime.Today.AddDays(-1));
            dateTimePicker.Items.Add(DateTime.Today);
            dateTimePicker.Items.Add(DateTime.Today.AddDays(1));
            dateTimePicker.SelectedIndex = 1;
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
            Department = departmentComboBox.Text;
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