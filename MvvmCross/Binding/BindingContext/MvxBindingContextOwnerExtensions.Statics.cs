// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.Binders;
using MvvmCross.Binding.ExpressionParse;
using MvvmCross.Binding.Parse.Binding.Lang;

namespace MvvmCross.Binding.BindingContext
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
