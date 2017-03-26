namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type WebBrowserPluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.WebBrowser.PluginLoader, MvvmCross.Plugins.WebBrowser.iOS.Plugin>()