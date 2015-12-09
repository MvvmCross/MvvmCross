// MvxBaseFluentBindingDescription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.BindingContext
{
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

    public class MvxBaseFluentBindingDescription<TTarget>
        : MvxApplicableTo<TTarget>
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
                this._knownSourcePath = knownSourcePath;
            }

            public MvxSourceStepDescription CreateSourceStep(MvxSourceStepDescription inputs)
            {
                return new MvxPathSourceStepDescription()
                {
                    Converter = inputs.Converter,
                    ConverterParameter = inputs.ConverterParameter,
                    FallbackValue = inputs.FallbackValue,
                    SourcePropertyPath = this._knownSourcePath
                };
            }
        }

        public class FreeTextSourceSpec
            : ISourceSpec
        {
            private readonly string _freeText;

            public FreeTextSourceSpec(string freeText)
            {
                this._freeText = freeText;
            }

            public MvxSourceStepDescription CreateSourceStep(MvxSourceStepDescription inputs)
            {
                var parser = Mvx.Resolve<IMvxBindingDescriptionParser>();
                var parsedDescription = parser.ParseSingle(this._freeText);

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
                this._sourceStepDescription = sourceStepDescription;
            }

            public MvxSourceStepDescription CreateSourceStep(MvxSourceStepDescription inputs)
            {
                if (inputs.Converter == null || inputs.FallbackValue == null)
                {
                    return this._sourceStepDescription;
                }

                return SourceSpecHelpers.WrapInsideSingleCombiner(inputs, this._sourceStepDescription);
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

        protected MvxBindingDescription BindingDescription => this._bindingDescription;

        protected MvxSourceStepDescription SourceStepDescription => this._sourceStepDescription;

        protected void SetFreeTextPropertyPath(string sourcePropertyPath)
        {
            if (this._sourceSpec != null)
                throw new MvxException("You cannot set the source path of a Fluent binding more than once");

            this._sourceSpec = new FreeTextSourceSpec(sourcePropertyPath);
        }

        protected void SetKnownTextPropertyPath(string sourcePropertyPath)
        {
            if (this._sourceSpec != null)
                throw new MvxException("You cannot set the source path of a Fluent binding more than once");

            this._sourceSpec = new KnownPathSourceSpec(sourcePropertyPath);
        }

        [Obsolete("Please use SourceOverwrite instead")]
        protected void Overwrite(MvxBindingDescription bindingDescription)
        {
            this.SourceOverwrite(bindingDescription);
        }

        protected void SourceOverwrite(MvxBindingDescription bindingDescription)
        {
            if (this._sourceSpec != null)
                throw new MvxException("You cannot set the source path of a Fluent binding more than once");

            this._bindingDescription.Mode = bindingDescription.Mode;
            this._bindingDescription.TargetName = bindingDescription.TargetName;

            this._sourceSpec = new FullySourceSpec(bindingDescription.Source);
        }

        protected void FullOverwrite(MvxBindingDescription bindingDescription)
        {
            if (this._sourceSpec != null)
                throw new MvxException("You cannot set the source path of a Fluent binding more than once");

            this._sourceSpec = new FullySourceSpec(bindingDescription.Source);
        }

        public MvxBaseFluentBindingDescription(IMvxBindingContextOwner bindingContextOwner, TTarget target)
        {
            this._bindingContextOwner = bindingContextOwner;
            this._target = target;
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
            this.EnsureTargetNameSet();

            MvxSourceStepDescription source;
            if (this._sourceSpec == null)
            {
                source = new MvxPathSourceStepDescription()
                {
                    Converter = this._sourceStepDescription.Converter,
                    ConverterParameter = this._sourceStepDescription.ConverterParameter,
                    FallbackValue = this._sourceStepDescription.FallbackValue
                };
            }
            else
            {
                source = this._sourceSpec.CreateSourceStep(this._sourceStepDescription);
            }

            var toReturn = new MvxBindingDescription()
            {
                Mode = this.BindingDescription.Mode,
                TargetName = this.BindingDescription.TargetName,
                Source = source
            };

            return toReturn;
        }

        public override void Apply()
        {
            var bindingDescription = this.CreateBindingDescription();
            this._bindingContextOwner.AddBinding(this._target, bindingDescription, this.ClearBindingKey);
            base.Apply();
        }

        public override void ApplyTo(TTarget what)
        {
            var bindingDescription = this.CreateBindingDescription();
            this._bindingContextOwner.AddBinding(what, bindingDescription, this.ClearBindingKey);
            base.ApplyTo(what);
        }

        protected void EnsureTargetNameSet()
        {
            if (!string.IsNullOrEmpty(this.BindingDescription.TargetName))
                return;

            this.BindingDescription.TargetName =
                MvxBindingSingletonCache.Instance.DefaultBindingNameLookup.DefaultFor(typeof(TTarget));
        }
    }
}