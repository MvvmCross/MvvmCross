// IMvxBindingSingletonCache.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Binding.ExpressionParse;
using MvvmCross.Binding.Parse.Binding.Lang;
using MvvmCross.Platform.Core;

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
        IMvxMainThreadDispatcher MainThreadDispatcher { get; }
    }
}