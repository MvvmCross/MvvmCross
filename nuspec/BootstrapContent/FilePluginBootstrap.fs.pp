namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type FilePluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.File.PluginLoader>()