using System;
using Cirrious.MvvmCross.Interfaces.Localization;

namespace Cirrious.MvvmCross.Interfaces.Platform.SoundEffects
{
    public interface IMvxSoundEffect
        : IMvxResourceObject, IDisposable
    {
        IMvxSoundEffectInstance CreateInstance();
    }
}