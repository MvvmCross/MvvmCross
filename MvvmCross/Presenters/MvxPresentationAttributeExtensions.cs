// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters
{
#nullable enable
    public static class MvxPresentationAttributeExtensions
    {
        public static bool HasBasePresentationAttribute(this Type candidateType)
        {
            var attributes = candidateType.GetCustomAttributes(typeof(MvxBasePresentationAttribute), true);
            return attributes.Length > 0;
        }

        public static IEnumerable<MvxBasePresentationAttribute> GetBasePresentationAttributes(this Type fromViewType)
        {
            var attributes = fromViewType.GetCustomAttributes(typeof(MvxBasePresentationAttribute), true);

            if (attributes.Length == 0)
                throw new InvalidOperationException($"Type does not have {nameof(MvxBasePresentationAttribute)} attribute!");

            return attributes.Cast<MvxBasePresentationAttribute>();
        }

        public static MvxBasePresentationAttribute GetBasePresentationAttribute(this Type fromViewType)
        {
            return fromViewType.GetBasePresentationAttributes().FirstOrDefault();
        }

        public static Type? GetViewModelType(this Type viewType)
        {
            if (!viewType.HasBasePresentationAttribute())
                return null;

            return viewType.GetBasePresentationAttributes()
                .Select(x => x.ViewModelType)
                .FirstOrDefault();
        }

        public static void Register<TMvxPresentationAttribute>(
            this IDictionary<Type, MvxPresentationAttributeAction> attributeTypesToActionsDictionary,
            Func<Type, TMvxPresentationAttribute, MvxViewModelRequest, Task<bool>> showAction,
            Func<IMvxViewModel, TMvxPresentationAttribute, Task<bool>> closeAction)
                where TMvxPresentationAttribute : class, IMvxPresentationAttribute
        {
            attributeTypesToActionsDictionary.Add(
                typeof(TMvxPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => showAction(view, (attribute as TMvxPresentationAttribute)!, request),
                    CloseAction = (viewModel, attribute) => closeAction(viewModel, (attribute as TMvxPresentationAttribute)!)
                });
        }
    }
#nullable restore
}
