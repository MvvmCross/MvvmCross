// MvxBindingDescriptionParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;
using Cirrious.MvvmCross.Binding.Combiners;
using Cirrious.MvvmCross.Binding.Parse.Binding.Lang;
using Cirrious.MvvmCross.Binding.Parse.Binding.Tibet;
using System.Collections.Generic;
using System.Linq;

namespace Cirrious.MvvmCross.Binding.Parse.Binding
{
    public class MvxBindingDescriptionParser
        : IMvxBindingDescriptionParser
    {
        private IMvxBindingParser _bindingParser;
        private IMvxValueConverterLookup _valueConverterLookup;

        protected IMvxBindingParser BindingParser
        {
            get
            {
                _bindingParser = _bindingParser ?? Mvx.Resolve<IMvxBindingParser>();
                return _bindingParser;
            }
        }

        private IMvxLanguageBindingParser _languageBindingParser;

        protected IMvxLanguageBindingParser LanguageBindingParser
        {
            get
            {
                _languageBindingParser = _languageBindingParser ?? Mvx.Resolve<IMvxLanguageBindingParser>();
                return _languageBindingParser;
            }
        }

        protected IMvxValueConverterLookup ValueConverterLookup
        {
            get
            {
                _valueConverterLookup = _valueConverterLookup ?? Mvx.Resolve<IMvxValueConverterLookup>();
                return _valueConverterLookup;
            }
        }

        protected IMvxValueConverter FindConverter(string converterName)
        {
            if (converterName == null)
                return null;

            var toReturn = ValueConverterLookup.Find(converterName);
            if (toReturn == null)
                MvxBindingTrace.Trace("Could not find named converter for {0}", converterName);

            return toReturn;
        }

        protected IMvxValueCombiner FindCombiner(string combiner)
        {
            return MvxBindingSingletonCache.Instance.ValueCombinerLookup.Find(combiner);
        }

        public IEnumerable<MvxBindingDescription> Parse(string text)
        {
            var parser = BindingParser;
            return Parse(text, parser);
        }

        public IEnumerable<MvxBindingDescription> LanguageParse(string text)
        {
            var parser = LanguageBindingParser;
            return Parse(text, parser);
        }

        public IEnumerable<MvxBindingDescription> Parse(string text, IMvxBindingParser parser)
        {
            MvxSerializableBindingSpecification specification;
            if (!parser.TryParseBindingSpecification(text, out specification))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Failed to parse binding specification starting with {0}",
                                      text == null ? "" : (text.Length > 20 ? text.Substring(0, 20) : text));
                return null;
            }

            if (specification == null)
                return null;

            return from item in specification
                   select SerializableBindingToBinding(item.Key, item.Value);
        }

        public MvxBindingDescription ParseSingle(string text)
        {
            MvxSerializableBindingDescription description;
            var parser = BindingParser;
            if (!parser.TryParseBindingDescription(text, out description))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Failed to parse binding description starting with {0}",
                                      text == null ? "" : (text.Length > 20 ? text.Substring(0, 20) : text));
                return null;
            }

            if (description == null)
                return null;

            return SerializableBindingToBinding(null, description);
        }

        public MvxBindingDescription SerializableBindingToBinding(string targetName,
                                                                  MvxSerializableBindingDescription description)
        {
            return new MvxBindingDescription
            {
                TargetName = targetName,
                Source = SourceStepDescriptionFrom(description),
                Mode = description.Mode,
            };
        }

        private MvxSourceStepDescription SourceStepDescriptionFrom(MvxSerializableBindingDescription description)
        {
            if (description.Path != null)
            {
                return new MvxPathSourceStepDescription()
                {
                    SourcePropertyPath = description.Path,
                    Converter = FindConverter(description.Converter),
                    ConverterParameter = description.ConverterParameter,
                    FallbackValue = description.FallbackValue
                };
            }

            if (description.Literal != null)
            {
                var literal = description.Literal;
                if (literal == MvxTibetBindingParser.LiteralNull)
                    literal = null;

                return new MvxLiteralSourceStepDescription()
                {
                    Literal = literal,
                    Converter = FindConverter(description.Converter),
                    ConverterParameter = description.ConverterParameter,
                    FallbackValue = description.FallbackValue
                };
            }

            if (description.Function != null)
            {
                // first look for a combiner with the name
                var combiner = FindCombiner(description.Function);
                if (combiner != null)
                {
                    return new MvxCombinerSourceStepDescription()
                    {
                        Combiner = combiner,
                        InnerSteps = description.Sources == null
                            ? new List<MvxSourceStepDescription>() :
                            description.Sources.Select(s => SourceStepDescriptionFrom(s)).ToList(),
                        Converter = FindConverter(description.Converter),
                        ConverterParameter = description.ConverterParameter,
                        FallbackValue = description.FallbackValue
                    };
                }
                else
                {
                    // no combiner, then drop back to looking for a converter
                    var converter = FindConverter(description.Function);
                    if (converter == null)
                    {
                        MvxBindingTrace.Error("Failed to find combiner or converter for {0}", description.Function);
                    }

                    if (description.Sources == null || description.Sources.Count == 0)
                    {
                        MvxBindingTrace.Error("Value Converter {0} supplied with no source", description.Function);
                        return new MvxLiteralSourceStepDescription()
                        {
                            Literal = null,
                        };
                    }
                    else if (description.Sources.Count > 2)
                    {
                        MvxBindingTrace.Error("Value Converter {0} supplied with too many parameters - {1}", description.Function, description.Sources.Count);
                        return new MvxLiteralSourceStepDescription()
                        {
                            Literal = null,
                        };
                    }
                    else
                    {
                        return new MvxCombinerSourceStepDescription()
                        {
                            Combiner = new MvxValueConverterValueCombiner(converter),
                            InnerSteps = description.Sources.Select(source => SourceStepDescriptionFrom(source)).ToList(),
                            Converter = FindConverter(description.Converter),
                            ConverterParameter = description.ConverterParameter,
                            FallbackValue = description.FallbackValue
                        };
                    }
                }
            }

            // this probably suggests that the path is the entire source object
            return new MvxPathSourceStepDescription()
            {
                SourcePropertyPath = null,
                Converter = FindConverter(description.Converter),
                ConverterParameter = description.ConverterParameter,
                FallbackValue = description.FallbackValue
            };
        }
    }
}