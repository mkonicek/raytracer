using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inv.Common
{
    public class Exceptions
    {
        public static void ExceptionDialog(string text)
        {
            MessageBox.Show(text, AppDomain.CurrentDomain.FriendlyName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
