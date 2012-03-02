#region Copyright
// <copyright file="MvxSoundEffect.cs" company="Cirrious">
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