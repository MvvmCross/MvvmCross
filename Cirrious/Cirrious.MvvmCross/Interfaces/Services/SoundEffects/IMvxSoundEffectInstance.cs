using System;

namespace Cirrious.MvvmCross.Interfaces.Services.SoundEffects
{
    public interface IMvxSoundEffectInstance
        : IDisposable
    {
        void Play();
        void Stop();
    }
}