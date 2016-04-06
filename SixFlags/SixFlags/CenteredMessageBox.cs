using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SixFlags
{
    public static class CenteredMessageBox
    {
        public static DialogResult Show(string message, string title, MessageBoxButtons buttons)
        {
            CustomMessageBox form = new CustomMessageBox(message, title, buttons);
            form.ShowDialog();
            return form.DialogResult;
        }
    }
}
