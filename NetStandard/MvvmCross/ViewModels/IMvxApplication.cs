// IMvxApplication.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Plugins;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxApplication : IMvxViewModelLocatorCollection
    {
        void LoadPlugins(IMvxPluginManager pluginManager);

        void Initialize();
    }
}