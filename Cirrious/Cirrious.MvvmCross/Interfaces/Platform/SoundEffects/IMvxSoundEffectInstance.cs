using System;

namespace Cirrious.MvvmCross.Interfaces.Platform.SoundEffects
{
    public interface IMvxSoundEffectInstance
        : IDisposable
    {
        void Play();
        void Stop();
    }
}