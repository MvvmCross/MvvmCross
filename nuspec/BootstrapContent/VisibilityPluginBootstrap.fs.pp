namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type VisibilityPluginBootstrap() =
     inherit MvxPluginBootstrapAction<MvvmCross.Plugins.Visibility.PluginLoader>()