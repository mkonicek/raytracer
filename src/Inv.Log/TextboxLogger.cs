using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inv.Log
{
    public class TextBoxLogger : ILogger
    {
        private TextBox _textBox;

        public TextBoxLogger(TextBox logTextBox)
        {
            if (logTextBox == null)
                throw new InvalidOperationException();
            _textBox = logTextBox;
        }

        delegate void WriteToTextBox(string message);

        void write(string message)
        {
            _textBox.Text +=
                    "[" + DateTime.Now.ToLongTimeString() + "] " + message + Environment.NewLine;
            // scroll to end
            _textBox.SelectionStart = _textBox.Text.Length;
            _textBox.ScrollToCaret();
        }

        #region ILogger Members

        public void WriteMessage(string message)
        {
            WriteToTextBox w = write;
            if (_textBox.InvokeRequired)
            {
                _textBox.Invoke(w, message);
            }
            else
            {
                w.Invoke(message);
            }
            Application.DoEvents();
        }

        #endregion
    }
}
