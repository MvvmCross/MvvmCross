// MvxBaseFluentBindingDescription.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.Bindings;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Combiners;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Binding.BindingContext
{
    public class MvxBaseFluentBindingDescription<TTarget>
        : MvxApplicableTo<TTarget>
        where TTarget : class
    {
        private readonly IMvxBindingContextOwner _bindingContextOwner;

        private readonly TTarget _target;
        private ISourceSpec _sourceSpec;

        public MvxBaseFluentBindingDescription(IMvxBindingContextOwner bindingContextOwner, TTarget target)
        {
            _bindingContextOwner = bindingContextOwner;
            _target = target;
        }

        protected object ClearBindingKey { get; set; }

        protected MvxBindingDescription BindingDescription { get; } = new MvxBindingDescription();

        protected MvxSourceStepDescription SourceStepDescription { get; } = new MvxSourceStepDescription();

        protected void SetFreeTextPropertyPath(string sourcePropertyPath)
        {
            if (_sourceSpec != null)
                throw new MvxException("You cannot set the source path of a Fluent binding more than once");

            _sourceSpec = new FreeTextSourceSpec(sourcePropertyPath);
        }

        protected void SetKnownTextPropertyPath(string sourcePropertyPath)
        {
            if (_sourceSpec != null)
                throw new MvxException("You cannot set the source path of a Fluent binding more than once");

            _sourceSpec = new KnownPathSourceSpec(sourcePropertyPath);
        }

        [Obsolete("Please use SourceOverwrite instead")]
        protected void Overwrite(MvxBindingDescription bindingDescription)
        {
            SourceOverwrite(bindingDescription);
        }

        protected void SourceOverwrite(MvxBindingDescription bindingDescription)
        {
            if (_sourceSpec != null)
                throw new MvxException("You cannot set the source path of a Fluent binding more than once");

            BindingDescription.Mode = bindingDescription.Mode;
            BindingDescription.TargetName = bindingDescription.TargetName;

            _sourceSpec = new FullySourceSpec(bindingDescription.Source);
        }

        protected void FullOverwrite(MvxBindingDescription bindingDescription)
        {
            if (_sourceSpec != null)
                throw new MvxException("You cannot set the source path of a Fluent binding more than once");

            _sourceSpec = new FullySourceSpec(bindingDescription.Source);
        }

        protected static string TargetPropertyName(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var parser = MvxBindingSingletonCache.Instance.PropertyExpressionParser;
            var targetPropertyName = parser.Parse(targetPropertyPath).Print();
            return targetPropertyName;
        }

        protected static string SourcePropertyPath<TSource>(Expression<Func<TSource, object>> sourceProperty)
        {
            var parser = MvxBindingSingletonCache.Instance.PropertyExpressionParser;
            var sourcePropertyPath = parser.Parse(sourceProperty).Print();
            return sourcePropertyPath;
        }

        protected static IMvxValueConverter ValueConverterFromName(string converterName)
        {
            var converter = MvxBindingSingletonCache.Instance.ValueConverterLookup.Find(converterName);
            return converter;
        }

        protected MvxBindingDescription CreateBindingDescription()
        {
            EnsureTargetNameSet();

            MvxSourceStepDescription source;
            if (_sourceSpec == null)
                source = new MvxPathSourceStepDescription
                {
                    Converter = SourceStepDescription.Converter,
                    ConverterParameter = SourceStepDescription.ConverterParameter,
                    FallbackValue = SourceStepDescription.FallbackValue
                };
            else
                source = _sourceSpec.CreateSourceStep(SourceStepDescription);

            var toReturn = new MvxBindingDescription
            {
                Mode = BindingDescription.Mode,
                TargetName = BindingDescription.TargetName,
                Source = source
            };

            return toReturn;
        }

        public override void Apply()
        {
            var bindingDescription = CreateBindingDescription();
            _bindingContextOwner.AddBinding(_target, bindingDescription, ClearBindingKey);
            base.Apply();
        }

        public override void ApplyTo(TTarget what)
        {
            var bindingDescription = CreateBindingDescription();
            _bindingContextOwner.AddBinding(what, bindingDescription, ClearBindingKey);
            base.ApplyTo(what);
        }

        protected void EnsureTargetNameSet()
        {
            if (!string.IsNullOrEmpty(BindingDescription.TargetName))
                return;

            BindingDescription.TargetName =
                MvxBindingSingletonCache.Instance.DefaultBindingNameLookup.DefaultFor(typeof(TTarget));
        }

        public interface ISourceSpec
        {
            MvxSourceStepDescription CreateSourceStep(MvxSourceStepDescription inputs);
        }

        public class KnownPathSourceSpec
            : ISourceSpec
        {
            private readonly string _knownSourcePath;

            public KnownPathSourceSpec(string knownSourcePath)
            {
                _knownSourcePath = knownSourcePath;
            }

            public MvxSourceStepDescription CreateSourceStep(MvxSourceStepDescription inputs)
            {
                return new MvxPathSourceStepDescription
                {
                    Converter = inputs.Converter,
                    ConverterParameter = inputs.ConverterParameter,
                    FallbackValue = inputs.FallbackValue,
                    SourcePropertyPath = _knownSourcePath
                };
            }
        }

        public class FreeTextSourceSpec
            : ISourceSpec
        {
            private readonly string _freeText;

            public FreeTextSourceSpec(string freeText)
            {
                _freeText = freeText;
            }

            public MvxSourceStepDescription CreateSourceStep(MvxSourceStepDescription inputs)
            {
                var parser = Mvx.Resolve<IMvxBindingDescriptionParser>();
                var parsedDescription = parser.ParseSingle(_freeText);

                if (inputs.Converter == null
                    && inputs.FallbackValue == null)
                    return parsedDescription.Source;

                if (parsedDescription.Source.Converter == null
                    && parsedDescription.Source.FallbackValue == null)
                {
                    var parsedStep = parsedDescription.Source;
                    parsedStep.Converter = inputs.Converter;
                    parsedStep.ConverterParameter = inputs.ConverterParameter;
                    parsedStep.FallbackValue = inputs.FallbackValue;
                    return parsedStep;
                }

                return SourceSpecHelpers.WrapInsideSingleCombiner(inputs, parsedDescription.Source);
            }
        }

        public class FullySourceSpec
            : ISourceSpec
        {
            private readonly MvxSourceStepDescription _sourceStepDescription;

            public FullySourceSpec(MvxSourceStepDescription sourceStepDescription)
            {
                _sourceStepDescription = sourceStepDescription;
            }

            public MvxSourceStepDescription CreateSourceStep(MvxSourceStepDescription inputs)
            {
                if (inputs.Converter == null || inputs.FallbackValue == null)
                    return _sourceStepDescription;

                return SourceSpecHelpers.WrapInsideSingleCombiner(inputs, _sourceStepDescription);
            }
        }

        public static class SourceSpecHelpers
        {
            public static MvxSourceStepDescription WrapInsideSingleCombiner(MvxSourceStepDescription inputs,
                MvxSourceStepDescription sourceStepDescription)
            {
                return new MvxCombinerSourceStepDescription
                {
                    Combiner = new MvxSingleValueCombiner(),
                    Converter = inputs.Converter,
                    ConverterParameter = inputs.ConverterParameter,
                    FallbackValue = inputs.FallbackValue,
                    InnerSteps = new List<MvxSourceStepDescription>
                    {
                        sourceStepDescription
                    }
                };
            }
        }
    }
}