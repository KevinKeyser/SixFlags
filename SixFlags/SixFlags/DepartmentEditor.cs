using System;
using System.Windows.Forms;

namespace SixFlags
{
    public partial class DepartmentEditor : Form
    {
        public string Department;
        public string newName;

        public DepartmentEditor()
        {
            InitializeComponent();
            foreach (Department department in SixFlagsTracker.Departments)
            {
                departmentComboBox.Items.Add(department.Name);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (departmentComboBox.Text == "")
            {
                CenteredMessageBox.Show("Please type a select a Department to edit.", "Error", MessageBoxButtons.OK);
                return;
            }
            if (newNameTextBox.Text == "")
            {
                CenteredMessageBox.Show("Please type a new Department name.", "Error", MessageBoxButtons.OK);
                return;
            }
            Department = departmentComboBox.Text;
            if (
                CenteredMessageBox.Show("Are you sure you want to " + Department + " to " + newNameTextBox.Text + "?",
                    "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            newName = newNameTextBox.Text;
            DialogResult = DialogResult.Yes;
            Close();
        }
    }
}