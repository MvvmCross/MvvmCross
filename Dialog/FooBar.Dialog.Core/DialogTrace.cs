using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Foobar.Dialog.Core
{
    public class DialogTrace
    {
        public static Action<string> WriteLineImpl { get; set; }

        public static void WriteLine(string format, params object[] args)
        {
            var message = string.Format(format, args);
            if (WriteLineImpl == null)
            {
                Debug.WriteLine(message);
            }
            else
            {
                WriteLineImpl(message);
            }
        }
    }
}
