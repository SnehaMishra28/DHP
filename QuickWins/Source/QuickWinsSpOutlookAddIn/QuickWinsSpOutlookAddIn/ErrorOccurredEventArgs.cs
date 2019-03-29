using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickWinsSpOutlookAddIn
{
    public class ErrorOccurredEventArgs : EventArgs
    {
        public Exception Exception { get; }

        public ErrorOccurredEventArgs(string message)
        {
            Exception = new Exception(message);
        }

        public ErrorOccurredEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
