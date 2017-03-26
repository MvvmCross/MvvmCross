namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type BookmarksPluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.Bookmarks.PluginLoader, MvvmCross.Plugins.Bookmarks.iOS.Plugin>()