// MvxBaseBindingDescriptionParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Converters;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Parse.Binding
{
    public class MvxBindingDescriptionParser
        : IMvxBindingDescriptionParser
        , IMvxServiceConsumer
    {
        protected IMvxBindingParser CreateParser ()
		{
			return this.GetService<IMvxBindingParser>();
		}

		protected IMvxValueConverter FindConverter(string converterName)
		{
			return this.GetService<IMvxValueConverterProvider>().Find(converterName);
		}

        #region IMvxBindingDescriptionParser Members

        public IEnumerable<MvxBindingDescription> Parse(string text)
        {
            MvxSerializableBindingSpecification specification;
            var parser = CreateParser();
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
            var parser = CreateParser();
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
                    SourcePropertyPath = description.Path,
                    Converter = FindConverter(description.Converter),
                    ConverterParameter = description.ConverterParameter,
                    Mode = description.Mode,
                    FallbackValue = description.FallbackValue
                };
        }

        #endregion
    }
}