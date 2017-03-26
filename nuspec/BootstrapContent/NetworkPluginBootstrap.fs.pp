namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type NetworkPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.Network.PluginLoader>()