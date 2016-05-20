using System;
using System.Windows.Forms;

namespace SixFlags
{
    public partial class AreaRemoval : Form
    {
        public string Area;

        public AreaRemoval()
        {
            InitializeComponent();
            foreach (var depart in SixFlagsTracker.Areas)
            {
                areaComboBox.Items.Add(depart.Name);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (areaComboBox.Text != "")
            {
                if (CenteredMessageBox.Show(
                    string.Format($"Are you sure you want to delete {areaComboBox.Text}?"),
                    "Deletion Dialog", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Area = areaComboBox.Text;
                    DialogResult = DialogResult.Yes;
                    Close();
                }
            }
            else
            {
                CenteredMessageBox.Show("Please select a Area to delete.", "Error", MessageBoxButtons.OK);
            }
        }
    }
}