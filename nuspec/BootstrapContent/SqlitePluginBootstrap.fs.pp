namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type SqlitePluginBootstrap =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.Sqlite.PluginLoader>()