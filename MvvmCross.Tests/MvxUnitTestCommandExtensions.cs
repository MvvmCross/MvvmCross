using System;
using MvvmCross.Commands;

namespace MvvmCross.Tests
{
    public static class MvxUnitTestCommandExtensions
    {
        public static void ListenForRaiseCanExecuteChanged(this IMvxCommand command)
        {
            var helper = GetCommandHelper();
            helper.WillCallRaisePropertyChangedFor(command);
        }

        public static bool RaisedCanExecuteChanged(this IMvxCommand command)
        {
            var helper = GetCommandHelper();
            return helper.HasCalledRaisePropertyChangedFor(command);
        }

        private static MvxUnitTestCommandHelper GetCommandHelper()
        {
            if (Mvx.IoCProvider.TryResolve<IMvxCommandHelper>(out IMvxCommandHelper helper))
            {
                if (helper != null && helper is MvxUnitTestCommandHelper)
                {
                    return (MvxUnitTestCommandHelper)helper;
                }
            }

            helper = new MvxUnitTestCommandHelper();
            Mvx.IoCProvider.RegisterSingleton<IMvxCommandHelper>(helper);
            return (MvxUnitTestCommandHelper)helper;
        }
    }
}
