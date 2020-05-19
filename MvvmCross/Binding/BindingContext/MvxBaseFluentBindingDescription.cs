// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MvvmCross.Base;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.Bindings;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Combiners;
using MvvmCross.Converters;
using MvvmCross.Exceptions;

namespace MvvmCross.Binding.BindingContext
{
    public class MvxBaseFluentBindingDescription<TTarget>
        : MvxApplicableTo<TTarget>, IMvxBaseFluentBindingDescription
        where TTarget : class
    {
        private readonly TTarget _target;
        private readonly IMvxBindingContextOwner _bindingContextOwner;

        private readonly MvxBindingDescription _bindingDescription = new MvxBindingDescription();
        private readonly MvxSourceStepDescription _sourceStepDescription = new MvxSourceStepDescription();
        private ISourceSpec _sourceSpec;

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
                return new MvxPathSourceStepDescription()
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
                var parser = Mvx.IoCProvider.Resolve<IMvxBindingDescriptionParser>();
                var parsedDescription = parser.ParseSingle(_freeText);

                if (inputs.Converter == null
                    && inputs.FallbackValue == null)
                {
                    return parsedDescription.Source;
                }

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
                {
                    return _sourceStepDescription;
                }

                return SourceSpecHelpers.WrapInsideSingleCombiner(inputs, _sourceStepDescription);
            }
        }

        public class CombinerSourceSpec
            : ISourceSpec
        {
            private readonly bool _useParser;
            private readonly string[] _properties;
            private readonly IMvxValueCombiner _combiner;

            public CombinerSourceSpec(IMvxValueCombiner combiner, string[] properties, bool useParser)
            {
                _combiner = combiner;
                _useParser = useParser;
                _properties = properties;
            }

            public MvxSourceStepDescription CreateSourceStep(MvxSourceStepDescription inputs)
            {
                var parser = Mvx.IoCProvider.Resolve<IMvxBindingDescriptionParser>();
                var innerSteps = _useParser ?
                    _properties.Select(p => parser.ParseSingle(p).Source) :
                    _properties.Select(p => new MvxPathSourceStepDescription { SourcePropertyPath = p });

                return new MvxCombinerSourceStepDescription
                {
                    Combiner = _combiner,
                    Converter = inputs.Converter,
                    ConverterParameter = inputs.ConverterParameter,
                    FallbackValue = inputs.FallbackValue,
                    InnerSteps = innerSteps.ToList()
                };
            }
        }

        public static class SourceSpecHelpers
        {
            public static MvxSourceStepDescription WrapInsideSingleCombiner(MvxSourceStepDescription inputs,
                                                                        MvxSourceStepDescription sourceStepDescription)
            {
                return new MvxCombinerSourceStepDescription()
                {
                    Combiner = new MvxSingleValueCombiner(),
                    Converter = inputs.Converter,
                    ConverterParameter = inputs.ConverterParameter,
                    FallbackValue = inputs.FallbackValue,
                    InnerSteps = new List<MvxSourceStepDescription>()
                            {
                                sourceStepDescription
                            }
                };
            }
        }

        protected object ClearBindingKey { get; set; }

        object IMvxBaseFluentBindingDescription.ClearBindingKey
        {
            get => ClearBindingKey;
            set => ClearBindingKey = value;
        }

        protected MvxBindingDescription BindingDescription => _bindingDescription;

        protected MvxSourceStepDescription SourceStepDescription => _sourceStepDescription;

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

        protected void SetCombiner(IMvxValueCombiner combiner, string[] properties, bool useParser)
        {
            if (_sourceSpec != null)
                throw new MvxException("You cannot set the source path of a Fluent binding more than once");

            _sourceSpec = new CombinerSourceSpec(combiner, properties, useParser);
        }

        protected void SourceOverwrite(MvxBindingDescription bindingDescription)
        {
            if (_sourceSpec != null)
                throw new MvxException("You cannot set the source path of a Fluent binding more than once");

            _bindingDescription.Mode = bindingDescription.Mode;
            _bindingDescription.TargetName = bindingDescription.TargetName;

            _sourceSpec = new FullySourceSpec(bindingDescription.Source);
        }

        protected void FullOverwrite(MvxBindingDescription bindingDescription)
        {
            if (_sourceSpec != null)
                throw new MvxException("You cannot set the source path of a Fluent binding more than once");

            _sourceSpec = new FullySourceSpec(bindingDescription.Source);
        }

        public MvxBaseFluentBindingDescription(IMvxBindingContextOwner bindingContextOwner, TTarget target)
        {
            _bindingContextOwner = bindingContextOwner;
            _target = target;
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
            {
                source = new MvxPathSourceStepDescription()
                {
                    Converter = _sourceStepDescription.Converter,
                    ConverterParameter = _sourceStepDescription.ConverterParameter,
                    FallbackValue = _sourceStepDescription.FallbackValue
                };
            }
            else
            {
                source = _sourceSpec.CreateSourceStep(_sourceStepDescription);
            }

            var toReturn = new MvxBindingDescription()
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
    }
}
