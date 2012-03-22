#region Copyright
// <copyright file="MvxBindingTrace.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding
{
    public static class MvxBindingTrace
    {
		public static MvxTraceLevel TraceBindingLevel = MvxTraceLevel.Warning;
		
        public const string Tag = "MvxBind";

        public static void Trace(MvxTraceLevel level, string message, params object[] args)
        {
			if (level < TraceBindingLevel)
				return;
			
            MvxTrace.TaggedTrace(level, Tag, message, args);
        }
    }
}