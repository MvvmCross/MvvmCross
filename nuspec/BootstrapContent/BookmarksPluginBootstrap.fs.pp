namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type BookmarksPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.Bookmarks.PluginLoader>()