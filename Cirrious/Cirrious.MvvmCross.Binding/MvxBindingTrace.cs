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

using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding
{
    public static class MvxBindingTrace
    {
        public const string Tag = "MvxBind";
        public static MvxBindingTraceLevel Level = MvxBindingTraceLevel.Warning;

        public static void Trace(MvxBindingTraceLevel level, string message, params object[] args)
        {
            if (level >= Level)
                MvxTrace.TaggedTrace(Tag, message, args);
        }
    }
}