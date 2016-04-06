using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            foreach (Department depart in SixFlagsTracker.Departments)
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
            TimeSpan timeIn = TimeSpan.Parse(timeInPicker.Text);
            TimeSpan timeOut = TimeSpan.Parse(timeOutPicker.Text);
            DateTime dateIn = DateTime.Parse(dateTimePicker.Text).Subtract(-timeIn);
            DateTime dateOut = DateTime.Parse(dateTimePicker.Text).Subtract(TimeSpan.FromDays(timeIn > timeOut ? -1 : 0)).Subtract(-timeOut);
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
