// Bi.n.d.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;
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
        public static readonly BindableProperty ndProperty = BindableProperty.CreateAttached("nd",
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
            IMvxBindingCreator toReturn;
            if (!Mvx.TryResolve<IMvxBindingCreator>(out toReturn))
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