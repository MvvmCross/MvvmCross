namespace $rootnamespace$.Bootstrap

open MvvmCross.Platform.Plugins

type SoundEffectsPluginBootstrap() =
    inherit MvxPluginBootstrapAction<MvvmCross.Plugins.SoundEffects.PluginLoader>()