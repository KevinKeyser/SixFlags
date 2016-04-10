using System;
using System.Windows.Forms;

namespace SixFlags
{
    public partial class DepartmentRemoval : Form
    {
        public string Department;

        public DepartmentRemoval()
        {
            InitializeComponent();
            foreach (var depart in SixFlagsTracker.Departments)
            {
                departmentComboBox.Items.Add(depart.Name);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (departmentComboBox.Text != "")
            {
                if (CenteredMessageBox.Show(
                    string.Format($"Are you sure you want to delete {departmentComboBox.Text}?"),
                    "Deletion Dialog", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Department = departmentComboBox.Text;
                    DialogResult = DialogResult.Yes;
                    Close();
                }
            }
            else
            {
                CenteredMessageBox.Show("Please select a department to delete.", "Error", MessageBoxButtons.OK);
            }
        }
    }
}