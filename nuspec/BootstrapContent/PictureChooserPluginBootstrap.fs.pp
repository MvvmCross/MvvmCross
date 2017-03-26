namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type PictureChooserPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.PictureChooser.PluginLoader>()