// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Binding.ExpressionParse;
using MvvmCross.Binding.Parse.Binding.Lang;

namespace MvvmCross.Binding
{
    public interface IMvxBindingSingletonCache
    {
        IMvxAutoValueConverters AutoValueConverters { get; }
        IMvxBindingDescriptionParser BindingDescriptionParser { get; }
        IMvxLanguageBindingParser LanguageParser { get; }
        IMvxPropertyExpressionParser PropertyExpressionParser { get; }
        IMvxValueConverterLookup ValueConverterLookup { get; }
        IMvxBindingNameLookup DefaultBindingNameLookup { get; }
        IMvxBinder Binder { get; }
        IMvxSourceBindingFactory SourceBindingFactory { get; }
        IMvxTargetBindingFactory TargetBindingFactory { get; }
        IMvxSourceStepFactory SourceStepFactory { get; }
        IMvxValueCombinerLookup ValueCombinerLookup { get; }
        IMvxMainThreadAsyncDispatcher MainThreadDispatcher { get; }
    }
}
