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
        public DialogResult Result;
        public string Department;
        public TimeSheet TimeSheet;

        public TimeSheetAdder(string department)
        {
            InitializeComponent();
            Result = DialogResult.None;
            Department = department;
            TimeSheet = null;
            departmentComboBox.Text = department;
            foreach (string depart in SixFlagsTracker.departments.Keys)
            {
                departmentComboBox.Items.Add(depart);
            }
            timeInMaskedTextBox.Text = DateTime.Now.TimeOfDay.ToString();
            timeOutMaskedTextBox.Text = DateTime.Now.AddHours(8.75f).ToString();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Yes;
            Department = departmentComboBox.Text;
            TimeSheet = new TimeSheet(
                nameTextBox.Text, 
                TimeSpan.Parse(timeInMaskedTextBox.Text),
                TimeSpan.Parse(timeOutMaskedTextBox.Text));
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Result = DialogResult.No;
            Close();
        }
    }
}
