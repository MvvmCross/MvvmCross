// DialogTrace.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Diagnostics;

namespace CrossUI.Core
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