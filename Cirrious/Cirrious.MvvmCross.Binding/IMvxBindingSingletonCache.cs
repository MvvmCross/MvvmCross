// IMvxBindingSingletonCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Combiners;
using Cirrious.MvvmCross.Binding.ExpressionParse;
using Cirrious.MvvmCross.Binding.Parse.Binding.Lang;

namespace Cirrious.MvvmCross.Binding
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
    }
}