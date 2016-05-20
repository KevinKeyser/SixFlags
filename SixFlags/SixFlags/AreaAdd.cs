using System;
using System.Windows.Forms;

namespace SixFlags
{
    public partial class AreaAdd : Form
    {
        public string Area;

        public AreaAdd()
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
            foreach (var area in SixFlagsTracker.Areas)
            {
                if (area.Name == Area)
                {
                    CenteredMessageBox.Show("Area already created.", "Error", MessageBoxButtons.OK);
                    return;
                }
            }
            if (areaTextBox.Text != "")
            {
                if (CenteredMessageBox.Show(string.Format($"Are you sure you want to add {areaTextBox.Text}?"),
                    "Deletion Dialog", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Area = areaTextBox.Text;
                    DialogResult = DialogResult.Yes;
                    Close();
                }
            }
            else
            {
                CenteredMessageBox.Show("Please type a Area to add.", "Error", MessageBoxButtons.OK);
            }
        }
    }
}