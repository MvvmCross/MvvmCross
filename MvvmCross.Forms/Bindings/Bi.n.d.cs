// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.Base;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings;
using MvvmCross.Exceptions;
using Xamarin.Forms;

namespace MvvmCross.Forms.Bindings
{
    // ReSharper disable once InconsistentNaming
    public static class Bi
    {
        static Bi()
        {
            MvxDesignTimeChecker.Check();
        }

        // ReSharper disable once InconsistentNaming
        public static readonly BindableProperty ndProperty = 
            BindableProperty.CreateAttached("nd",
                                            typeof(string),
                                            typeof(Bi),
                                            null,
                                            BindingMode.OneWay,
                                            null,
                                            CallBackWhenndIsChanged);

        public static string Getnd(BindableObject obj)
        {
            return obj.GetValue(ndProperty) as string;
        }

        public static void Setnd(BindableObject obj, string value)
        {
            obj.SetValue(ndProperty, value);
        }

        private static IMvxBindingCreator _bindingCreator;

        private static IMvxBindingCreator BindingCreator
        {
            get
            {
                _bindingCreator = _bindingCreator ?? ResolveBindingCreator();
                return _bindingCreator;
            }
        }

        private static IMvxBindingCreator ResolveBindingCreator()
        {
            if (MvxDesignTimeChecker.IsDesignTime)
                return null;

            if (!Mvx.IoCProvider.TryResolve(out IMvxBindingCreator toReturn))
            {
                throw new MvxException("Unable to resolve the binding creator - have you initialized Xamarin Forms Binding");
            }

            return toReturn;
        }

        private static void CallBackWhenndIsChanged(BindableObject sender, object oldValue, object newValue)
        {
            var bindingCreator = BindingCreator;

            bindingCreator?.CreateBindings(sender, oldValue, newValue, ParseBindingDescriptions);
        }

        private static IEnumerable<MvxBindingDescription> ParseBindingDescriptions(string bindingText)
        {
            if (MvxSingleton<IMvxBindingSingletonCache>.Instance == null)
                return null;

            return MvxSingleton<IMvxBindingSingletonCache>.Instance.BindingDescriptionParser.Parse(bindingText);
        }
    }
}
