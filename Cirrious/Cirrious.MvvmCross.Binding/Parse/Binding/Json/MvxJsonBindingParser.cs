// MvxJsonBindingParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#warning This file needs to go into a new Assembly
#if false
using System;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Plugins.Json;

namespace Cirrious.MvvmCross.Binding.Parse.Binding.Json
{
    public class MvxJsonBindingParser
        : IMvxBindingParser
        , IMvxConsumer
    {
        public bool TryParseBindingDescription(string text, out MvxSerializableBindingDescription requestedDescription)
        {
            if (string.IsNullOrEmpty(text))
            {
                requestedDescription = new MvxSerializableBindingDescription();
                return false;
            }

            try
            {
                var converter = this.GetService<IMvxJsonConverter>();
                requestedDescription = converter.DeserializeObject<MvxSerializableBindingDescription>(text);
            }
            catch (Exception exception)
            {
                requestedDescription = null;
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Problem parsing Json tag for databinding {0}", exception.ToLongString());
                return false;
            }

            return true;
        }

        public bool TryParseBindingSpecification(string text, out MvxSerializableBindingSpecification requestedBindings)
        {
            if (string.IsNullOrEmpty(text))
            {
                requestedBindings = new MvxSerializableBindingSpecification();
                return false;
            }

            try
            {
                var converter = this.GetService<IMvxJsonConverter>();
                requestedBindings = converter.DeserializeObject<MvxSerializableBindingSpecification>(text);
            }
            catch (Exception exception)
            {
                requestedBindings = null;
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Problem parsing Json tag for databinding {0}", exception.ToLongString());
                return false;
            }
            return true;
        }
    }
}
#endif