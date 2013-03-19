// MvxBindingContextOwnerExtensions.CachedStatics.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;
using Cirrious.MvvmCross.Binding.Interfaces.ExpressionParse;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public static partial class MvxBindingContextOwnerExtensions
    {
        public static IMvxLanguageBindingParser LanguageParser
        {
            get { return MvxBindingStatics.LanguageParser; }
        }

        public static IMvxPropertyExpressionParser PropertyExpressionParser
        {
            get { return MvxBindingStatics.PropertyExpressionParser; }
        }

        public static IMvxValueConverterLookup ValueConverterLookup
        {
            get { return MvxBindingStatics.ValueConverterLookup; }
        }

        public static IMvxBindingNameLookup DefaultBindingNameLookup
        {
            get { return MvxBindingStatics.DefaultBindingNameLookup; }
        }

        public static IMvxBinder Binder
        {
            get { return MvxBindingStatics.Binder; }
        }
    }
}