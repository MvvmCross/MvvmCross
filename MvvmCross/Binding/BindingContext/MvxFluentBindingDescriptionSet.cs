// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.Base;
using MvvmCross.Binding.Bindings;

namespace MvvmCross.Binding.BindingContext
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
                                                                                     MvxBindingDescription bindingDescription)
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

        public void ApplyWithClearBindingKey(object clearBindingKey)
        {
            foreach (var applicable in _applicables)
            {
                if (applicable is IMvxBaseFluentBindingDescription fluentBindingDescription)
                {
                    fluentBindingDescription.ClearBindingKey = clearBindingKey;
                }
                else
                {
                    MvxBindingLog.Warning("Fluent binding description must implement {0} in order to add {1}",
                        nameof(IMvxBaseFluentBindingDescription),
                        nameof(IMvxBaseFluentBindingDescription.ClearBindingKey));
                }

                applicable.Apply();
            }

            base.Apply();
        }
    }
}
