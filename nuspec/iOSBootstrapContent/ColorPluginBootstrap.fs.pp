namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type ColorPluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.Color.PluginLoader, MvvmCross.Plugins.Color.iOS.Plugin>()