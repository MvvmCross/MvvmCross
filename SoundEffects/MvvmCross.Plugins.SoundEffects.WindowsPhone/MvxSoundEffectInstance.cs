// MvxSoundEffectInstance.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Microsoft.Xna.Framework.Audio;

namespace MvvmCross.Plugins.SoundEffects.WindowsPhone
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