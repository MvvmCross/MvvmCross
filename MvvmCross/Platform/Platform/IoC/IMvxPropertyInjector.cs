// IMvxPropertyInjector.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.IoC
{
    public interface IMvxPropertyInjector
    {
        void Inject(object target, IMvxPropertyInjectorOptions options = null);
    }
}