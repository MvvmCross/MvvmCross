namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type PictureChooserPluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.PictureChooser.PluginLoader, MvvmCross.Plugins.PictureChooser.iOS.Plugin>()