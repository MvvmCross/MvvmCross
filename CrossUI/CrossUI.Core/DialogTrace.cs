#region Copyright

// <copyright file="DialogTrace.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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