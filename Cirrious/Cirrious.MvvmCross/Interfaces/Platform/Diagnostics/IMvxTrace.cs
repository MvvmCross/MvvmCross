// IMvxTrace.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Interfaces.Platform.Diagnostics
{
    public interface IMvxTrace
    {
        void Trace(MvxTraceLevel level, string tag, string message);
        void Trace(MvxTraceLevel level, string tag, string message, params object[] args);
    }
}