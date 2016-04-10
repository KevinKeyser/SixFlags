using System.Windows.Forms;

namespace SixFlags
{
    public static class CenteredMessageBox
    {
        public static DialogResult Show(string message, string title, MessageBoxButtons buttons)
        {
            var form = new CustomMessageBox(message, title, buttons);
            form.ShowDialog();
            return form.DialogResult;
        }
    }
}