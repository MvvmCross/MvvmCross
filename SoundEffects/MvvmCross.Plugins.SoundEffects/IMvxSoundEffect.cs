// IMvxSoundEffect.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Plugins.ResourceLoader;

namespace MvvmCross.Plugins.SoundEffects
{
    public interface IMvxSoundEffect
        : IMvxResourceObject, IDisposable
    {
        IMvxSoundEffectInstance CreateInstance();
    }
}