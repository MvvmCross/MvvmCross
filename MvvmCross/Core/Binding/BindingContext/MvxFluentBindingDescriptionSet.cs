// MvxFluentBindingDescriptionSet.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Binding.Bindings;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public class MvxFluentBindingDescriptionSet<TOwningTarget, TSource>
        : MvxApplicable
        where TOwningTarget : class, IMvxBindingContextOwner
    {
        private readonly List<IMvxApplicable> _applicables = new List<IMvxApplicable>();
        private readonly TOwningTarget _bindingContextOwner;

        public MvxFluentBindingDescriptionSet(TOwningTarget bindingContextOwner)
        {
            _bindingContextOwner = bindingContextOwner;
        }

        public MvxFluentBindingDescription<TOwningTarget, TSource> Bind()
        {
            var toReturn = new MvxFluentBindingDescription<TOwningTarget, TSource>(_bindingContextOwner,
                                                                                   _bindingContextOwner);
            _applicables.Add(toReturn);
            return toReturn;
        }

        public MvxFluentBindingDescription<TChildTarget, TSource> Bind<TChildTarget>(TChildTarget childTarget)
            where TChildTarget : class
        {
            var toReturn = new MvxFluentBindingDescription<TChildTarget, TSource>(_bindingContextOwner, childTarget);
            _applicables.Add(toReturn);
            return toReturn;
        }

        public MvxFluentBindingDescription<TChildTarget, TSource> Bind<TChildTarget>(TChildTarget childTarget,
                                                                                     string bindingDescription)
            where TChildTarget : class
        {
            var toReturn = Bind(childTarget);
            toReturn.FullyDescribed(bindingDescription);
            return toReturn;
        }

        public MvxFluentBindingDescription<TChildTarget, TSource> Bind<TChildTarget>(TChildTarget childTarget,
                                                                                     MvxBindingDescription
                                                                                         bindingDescription)
            where TChildTarget : class
        {
            var toReturn = Bind(childTarget);
            toReturn.FullyDescribed(bindingDescription);
            return toReturn;
        }

        public override void Apply()
        {
            foreach (var applicable in _applicables)
                applicable.Apply();
            base.Apply();
        }
    }
}