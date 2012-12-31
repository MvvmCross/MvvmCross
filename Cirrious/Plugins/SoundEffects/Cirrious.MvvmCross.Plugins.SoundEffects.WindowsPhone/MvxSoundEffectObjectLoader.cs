#region Copyright

// <copyright file="MvxSoundEffectObjectLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.IO;
using Cirrious.MvvmCross.Plugins.ResourceLoader;
using Microsoft.Xna.Framework.Audio;

namespace Cirrious.MvvmCross.Plugins.SoundEffects.WindowsPhone
{
    public class MvxSoundEffectObjectLoader
        : MvxBaseResourceObjectLoader<IMvxSoundEffect>
          , IMvxSoundEffectLoader
    {
        protected override IMvxSoundEffect Load(Stream stream)
        {
            var xnaSoundEffect = SoundEffect.FromStream(stream);
            return new MvxSoundEffect(xnaSoundEffect);
        }
    }
}