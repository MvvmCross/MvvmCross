// MvxBindingLog.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Runtime.CompilerServices;
using MvvmCross.Logging;

[assembly: InternalsVisibleTo("MvvmCross.UnitTest")]

namespace MvvmCross.Test
{
    internal static class MvxTestLog
    {
        internal static IMvxLog Instance { get; } = Mvx.Resolve<IMvxLogProvider>().GetLogFor("MvxTest");
    }
}
