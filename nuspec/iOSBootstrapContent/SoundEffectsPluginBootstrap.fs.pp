namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type SoundEffectsPluginBootstrap() = 
    inherit MvxLoaderPluginBootstrapAction<MvvmCross.Plugins.SoundEffects.PluginLoader, MvvmCross.Plugins.SoundEffects.iOS.Plugin>()