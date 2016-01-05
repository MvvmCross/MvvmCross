// MvxFluentBindingDescriptionSet.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.BindingContext
{
    using System.Collections.Generic;

    using MvvmCross.Binding.Bindings;
    using MvvmCross.Platform.Core;

    public class MvxFluentBindingDescriptionSet<TOwningTarget, TSource>
        : MvxApplicable
        where TOwningTarget : class, IMvxBindingContextOwner
    {
        private readonly List<IMvxApplicable> _applicables = new List<IMvxApplicable>();
        private readonly TOwningTarget _bindingContextOwner;

        public MvxFluentBindingDescriptionSet(TOwningTarget bindingContextOwner)
        {
            this._bindingContextOwner = bindingContextOwner;
        }

        public MvxFluentBindingDescription<TOwningTarget, TSource> Bind()
        {
            var toReturn = new MvxFluentBindingDescription<TOwningTarget, TSource>(this._bindingContextOwner,
                                                                                   this._bindingContextOwner);
            this._applicables.Add(toReturn);
            return toReturn;
        }

        public MvxFluentBindingDescription<TChildTarget, TSource> Bind<TChildTarget>(TChildTarget childTarget)
            where TChildTarget : class
        {
            var toReturn = new MvxFluentBindingDescription<TChildTarget, TSource>(this._bindingContextOwner, childTarget);
            this._applicables.Add(toReturn);
            return toReturn;
        }

        public MvxFluentBindingDescription<TChildTarget, TSource> Bind<TChildTarget>(TChildTarget childTarget,
                                                                                     string bindingDescription)
            where TChildTarget : class
        {
            var toReturn = this.Bind(childTarget);
            toReturn.FullyDescribed(bindingDescription);
            return toReturn;
        }

        public MvxFluentBindingDescription<TChildTarget, TSource> Bind<TChildTarget>(TChildTarget childTarget,
                                                                                     MvxBindingDescription
                                                                                         bindingDescription)
            where TChildTarget : class
        {
            var toReturn = this.Bind(childTarget);
            toReturn.FullyDescribed(bindingDescription);
            return toReturn;
        }

        public override void Apply()
        {
            foreach (var applicable in this._applicables)
                applicable.Apply();
            base.Apply();
        }
    }
}