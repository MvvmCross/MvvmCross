---
layout: documentation
title: SoundEffects
category: Plugins
---
The `SoundEffects` plugin is only supported on WindowsPhone currently.

It uses the ResourceLoader plugin and allows small SoundEffect files to be played via:
[block:code]
{
  "codes": [
    {
      "code": "public interface IMvxSoundEffectLoader\n  : IMvxResourceObjectLoader<IMvxSoundEffect>\n  {\n  }\n\npublic interface IMvxSoundEffect\n  : IMvxResourceObject, IDisposable\n  {\n    IMvxSoundEffectInstance CreateInstance();\n  }\n\npublic interface IMvxSoundEffectInstance\n  : IDisposable\n  {\n    void Play();\n    void Stop();\n  }",
      "language": "csharp"
    }
  ]
}
[/block]
Current advice (August 2013): this plugin isn't really useful outside of WindowsPhone today - hopefully some project will come along soon that needs this implemented on more platforms.