// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.Binding.Bindings;

namespace MvvmCross.Binding.BindingContext
{
    public class MvxFluentBindingDescriptionSet<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TOwningTarget, TSource>
            : MvxApplicable, IDisposable
                where TOwningTarget : class, IMvxBindingContextOwner
    {
        private readonly List<IMvxApplicable> _applicables = [];
        private readonly TOwningTarget _bindingContextOwner;
        private readonly string _clearBindingKey;

        public MvxFluentBindingDescriptionSet(TOwningTarget bindingContextOwner)
        {
            _bindingContextOwner = bindingContextOwner;
        }
        public MvxFluentBindingDescriptionSet(TOwningTarget bindingContextOwner, string clearBindingKey) : this(bindingContextOwner)
        {
            _clearBindingKey = clearBindingKey;
        }
        public MvxFluentBindingDescription<TOwningTarget, TSource> Bind()
        {
            var toReturn = new MvxFluentBindingDescription<TOwningTarget, TSource>(
                _bindingContextOwner, _bindingContextOwner);
            _applicables.Add(toReturn);
            return toReturn;
        }

        public MvxFluentBindingDescription<TChildTarget, TSource> Bind<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TChildTarget>(TChildTarget childTarget)
                where TChildTarget : class
        {
            var toReturn = new MvxFluentBindingDescription<TChildTarget, TSource>(_bindingContextOwner, childTarget);
            _applicables.Add(toReturn);
            return toReturn;
        }

        public MvxFluentBindingDescription<TChildTarget, TSource> Bind<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TChildTarget>(
                TChildTarget childTarget,
                string bindingDescription)
                    where TChildTarget : class
        {
            var toReturn = Bind(childTarget);
            toReturn.FullyDescribed(bindingDescription);
            return toReturn;
        }

        public MvxFluentBindingDescription<TChildTarget, TSource> Bind<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TChildTarget>(
                TChildTarget childTarget, MvxBindingDescription bindingDescription)
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
                    MvxBindingLog.Instance?.LogWarning(
                        "Fluent binding description must implement {InterfaceName} in order to add {Description}",
                        nameof(IMvxBaseFluentBindingDescription),
                        nameof(IMvxBaseFluentBindingDescription.ClearBindingKey));
                }

                applicable.Apply();
            }

            base.Apply();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (string.IsNullOrEmpty(_clearBindingKey))
                    Apply();
                else
                    ApplyWithClearBindingKey(_clearBindingKey);
            }
        }
    }
}
