// MvxSoundEffect.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Microsoft.Xna.Framework.Audio;

namespace MvvmCross.Plugins.SoundEffects.WindowsPhone
{
    public sealed class MvxSoundEffect : IMvxSoundEffect
    {
        private readonly SoundEffect _xnaSoundEffect;

        public MvxSoundEffect(SoundEffect xnaSoundEffect)
        {
            _xnaSoundEffect = xnaSoundEffect;
        }

        ~MvxSoundEffect()
        {
            Dispose(false);
        }

        #region IMvxSoundEffect Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _xnaSoundEffect.Dispose();
            }
        }

        public IMvxSoundEffectInstance CreateInstance()
        {
            return new MvxSoundEffectInstance(_xnaSoundEffect.CreateInstance());
            throw new NotImplementedException();
        }

        #endregion
    }
}