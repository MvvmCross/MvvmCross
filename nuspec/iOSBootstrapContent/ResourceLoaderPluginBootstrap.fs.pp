namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type ResourceLoaderPluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.ResourceLoader.PluginLoader, MvvmCross.Plugins.ResourceLoader.iOS.Plugin>()