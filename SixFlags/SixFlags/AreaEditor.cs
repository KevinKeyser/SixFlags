using System;
using System.Windows.Forms;

namespace SixFlags
{
    public partial class AreaEditor : Form
    {
        public string Area;
        public string newName;

        public AreaEditor()
        {
            InitializeComponent();
            foreach (Area area in SixFlagsTracker.Areas)
            {
                areaComboBox.Items.Add(area.Name);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (areaComboBox.Text == "")
            {
                CenteredMessageBox.Show("Please type a select a Area to edit.", "Error", MessageBoxButtons.OK);
                return;
            }
            if (newNameTextBox.Text == "")
            {
                CenteredMessageBox.Show("Please type a new Area name.", "Error", MessageBoxButtons.OK);
                return;
            }
            Area = areaComboBox.Text;
            if (
                CenteredMessageBox.Show("Are you sure you want to " + Area + " to " + newNameTextBox.Text + "?",
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