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
    public partial class DepartmentAdd : Form
    {
        public string Department;

        public DepartmentAdd()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            foreach (Department department in SixFlagsTracker.Departments)
            {
                if (department.Name == Department)
                {
                    CenteredMessageBox.Show("Department already created.", "Error", MessageBoxButtons.OK);
                    return;
                }
            }
            if (departmentTextBox.Text != "")
            {
                if (CenteredMessageBox.Show(string.Format($"Are you sure you want to add {departmentTextBox.Text}?"),
                    "Deletion Dialog", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Department = departmentTextBox.Text;
                    DialogResult = DialogResult.Yes;
                    Close();
                }
            }
            else
            {
                CenteredMessageBox.Show("Please type a department to add.", "Error", MessageBoxButtons.OK);
            }
        }
    }
}
