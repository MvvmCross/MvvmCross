namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type VisibilityPluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.Visibility.PluginLoader, MvvmCross.Plugins.Visibility.iOS.Plugin>()