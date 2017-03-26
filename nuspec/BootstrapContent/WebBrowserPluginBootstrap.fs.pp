namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type WebBrowserPluginBootstrap() =
     inherit MvxPluginBootstrapAction<MvvmCross.Plugins.WebBrowser.PluginLoader>()