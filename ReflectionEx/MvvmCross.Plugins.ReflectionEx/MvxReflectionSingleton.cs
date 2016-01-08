// MvxReflectionSingleton.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Core;

namespace MvvmCross.Plugins.ReflectionEx
{
    public class MvxReflectionSingleton
        : MvxSingleton
    {
        public static readonly MvxReflectionSingleton Instance = new MvxReflectionSingleton();

        private MvxReflectionSingleton()
        {
        }

        private IMvxReflectionEx _reflectionEx;

        public IMvxReflectionEx ReflectionEx
        {
            get
            {
                _reflectionEx = _reflectionEx ?? Mvx.Resolve<IMvxReflectionEx>();
                return _reflectionEx;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            _reflectionEx = null;
        }
    }
}