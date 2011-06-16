using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Lucid.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            try
            {
                Application.Run(new ClientForm());
            }
            catch(Exception ex)
            {
                Inv.Common.Exceptions.ExceptionDialog(ex.Message + 
                    Environment.NewLine + "Please restart the program.");
            }
        }
    }
}