// IMvxBindingSingletonCache.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Core;

namespace MvvmCross.Binding
{
    using Binders;
    using BindingContext;
    using Bindings.Source.Construction;
    using Bindings.SourceSteps;
    using Bindings.Target.Construction;
    using Combiners;
    using ExpressionParse;
    using Parse.Binding.Lang;

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
        IMvxMainThreadDispatcher MainThreadDispatcher { get; }
    }
}