// MvxSoundEffectObjectLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.IO;
using Cirrious.MvvmCross.Plugins.ResourceLoader;
using Microsoft.Xna.Framework.Audio;

namespace Cirrious.MvvmCross.Plugins.SoundEffects.WindowsPhone
{
    public class MvxSoundEffectObjectLoader
        : MvxResourceObjectLoader<IMvxSoundEffect>
          , IMvxSoundEffectLoader
    {
        protected override IMvxSoundEffect Load(Stream stream)
        {
            var xnaSoundEffect = SoundEffect.FromStream(stream);
            return new MvxSoundEffect(xnaSoundEffect);
        }
    }
}