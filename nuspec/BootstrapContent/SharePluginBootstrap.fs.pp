namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type SharePluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.Share.PluginLoader>()