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
            if (_xnaSoundEffectInstance.State == SoundState.Playing)
                _xnaSoundEffectInstance.Stop();
            _xnaSoundEffectInstance.Dispose();
        }

        #endregion
    }
}