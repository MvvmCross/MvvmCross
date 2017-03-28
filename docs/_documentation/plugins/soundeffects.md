---
layout: documentation
title: SoundEffects
category: Plugins
---
The `SoundEffects` plugin is only supported on WindowsPhone currently.

It uses the ResourceLoader plugin and allows small SoundEffect files to be played via:
```c# 
public interface IMvxSoundEffectLoader
  : IMvxResourceObjectLoader<IMvxSoundEffect>
  {
  }

public interface IMvxSoundEffect
  : IMvxResourceObject, IDisposable
  {
    IMvxSoundEffectInstance CreateInstance();
  }

public interface IMvxSoundEffectInstance
  : IDisposable
  {
    void Play();
    void Stop();
  }
```
Current advice (August 2013): this plugin isn't really useful outside of WindowsPhone today - hopefully some project will come along soon that needs this implemented on more platforms.