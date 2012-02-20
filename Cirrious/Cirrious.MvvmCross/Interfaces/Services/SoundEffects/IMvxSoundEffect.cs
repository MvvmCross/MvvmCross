using System;
using Cirrious.MvvmCross.Interfaces.Localization;

namespace Cirrious.MvvmCross.Interfaces.Services.SoundEffects
{
    public interface IMvxSoundEffect
        : IMvxResourceObject, IDisposable
    {
        IMvxSoundEffectInstance CreateInstance();
    }
}