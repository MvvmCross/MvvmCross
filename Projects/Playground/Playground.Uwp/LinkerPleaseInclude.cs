using System;
using System.Collections.Specialized;
using System.Windows.Input;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Uwp
{
    // This class is never actually executed, but when Xamarin linking is enabled it does how to ensure types and properties
    // are preserved in the deployed app
    [Preserve(AllMembers = true)]
    public class LinkerPleaseInclude
    {
        public void Include(MvvmCross.Plugin.Json.Plugin plugin)
        {
            plugin.Load();
        }

        public void Include(MvvmCross.Plugin.Messenger.Plugin plugin)
        {
            plugin.Load();
        }
    }
}
