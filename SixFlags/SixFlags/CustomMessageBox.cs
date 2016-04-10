using System;
using System.Drawing;
using System.Windows.Forms;

namespace SixFlags
{
    public partial class CustomMessageBox : Form
    {
        public CustomMessageBox(string message, string title, MessageBoxButtons buttons)
        {
            InitializeComponent();
            Text = title;
            messageLabel.Text = message;
            if (buttons == MessageBoxButtons.OK)
            {
                yesButton.Text = "OK";
                yesButton.Location = new Point(ClientSize.Width/2 - yesButton.Width/2, yesButton.Location.Y);
                noButton.Visible = false;
            }
            else if (buttons == MessageBoxButtons.OKCancel)
            {
                yesButton.Text = "OK";
                noButton.Text = "Cancel";
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            DialogResult = (DialogResult) Enum.Parse(typeof (DialogResult), ((Button) sender).Text);
            Close();
        }
    }
}