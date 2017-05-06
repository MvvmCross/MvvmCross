namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type SqlitePluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.Sqlite.PluginLoader, MvvmCross.Plugins.Sqlite.iOS.Plugin>())