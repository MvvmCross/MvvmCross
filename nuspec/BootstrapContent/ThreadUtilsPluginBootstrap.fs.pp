namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type ThreadUtilsPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.ThreadUtils.PluginLoader>()