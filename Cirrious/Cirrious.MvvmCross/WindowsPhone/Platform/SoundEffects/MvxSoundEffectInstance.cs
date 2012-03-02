#region Copyright
// <copyright file="MvxSoundEffectInstance.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Interfaces.Platform.SoundEffects;
using Microsoft.Xna.Framework.Audio;

namespace Cirrious.MvvmCross.WindowsPhone.Platform.SoundEffects
{
    public sealed class MvxSoundEffectInstance : IMvxSoundEffectInstance
    {
        private readonly SoundEffectInstance _xnaSoundEffectInstance;

        public MvxSoundEffectInstance(SoundEffectInstance xnaSoundEffect)
        {
            _xnaSoundEffectInstance = xnaSoundEffect;
        }

        ~MvxSoundEffectInstance()
        {
            Dispose(false);
        }

        #region Implementation of IMvxSoundEffect

        public void Play()
        {
            _xnaSoundEffectInstance.Play();
        }

        public void Stop()
        {
            _xnaSoundEffectInstance.Stop();
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_xnaSoundEffectInstance.State == SoundState.Playing)
                    _xnaSoundEffectInstance.Stop();
                _xnaSoundEffectInstance.Dispose();
            }
        }

        #endregion
    }
}