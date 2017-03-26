namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type NetworkPluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.Network.PluginLoader, MvvmCross.Plugins.Network.iOS.Plugin>()