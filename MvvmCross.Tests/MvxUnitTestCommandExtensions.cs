using System;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Commands;
using MvvmCross.Hosting;

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
            var existing = MvxHost.Current?.Services.GetService<IMvxCommandHelper>();
            if (existing is MvxUnitTestCommandHelper unitTestHelper)
                return unitTestHelper;

            // No helper registered yet — return a standalone instance (not registered to DI).
            // Tests that need the helper registered should do so via ServiceCollection before BuildServices().
            return new MvxUnitTestCommandHelper();
        }
    }
}
