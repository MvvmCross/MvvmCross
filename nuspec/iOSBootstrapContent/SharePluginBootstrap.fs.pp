namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type SharePluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.Share.PluginLoader, MvvmCross.Plugins.Share.iOS.Plugin>()