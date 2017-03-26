namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type FilePluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.File.PluginLoader, MvvmCross.Plugins.File.iOS.Plugin>()