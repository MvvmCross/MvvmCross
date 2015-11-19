// MvxBindingContextOwnerExtensions.Statics.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.ExpressionParse;
using Cirrious.MvvmCross.Binding.Parse.Binding.Lang;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public static partial class MvxBindingContextOwnerExtensions
    {
        public static IMvxLanguageBindingParser LanguageParser => MvxBindingSingletonCache.Instance.LanguageParser;

        public static IMvxPropertyExpressionParser PropertyExpressionParser => MvxBindingSingletonCache.Instance.PropertyExpressionParser;

        public static IMvxValueConverterLookup ValueConverterLookup => MvxBindingSingletonCache.Instance.ValueConverterLookup;

        public static IMvxBindingNameLookup DefaultBindingNameLookup => MvxBindingSingletonCache.Instance.DefaultBindingNameLookup;

        public static IMvxBinder Binder => MvxBindingSingletonCache.Instance.Binder;
    }
}