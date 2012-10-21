#region Copyright
// <copyright file="MvxJsonBindingParser.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Threading;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Plugins.Json;

namespace Cirrious.MvvmCross.Binding.Binders.Json
{
    public class MvxJsonBindingParser
        : IMvxServiceConsumer<IMvxJsonConverter>
    {
        public bool TryParseBindingDescription(string text, out MvxJsonBindingDescription requestedDescription)
        {
            if (string.IsNullOrEmpty(text))
            {
                requestedDescription = new MvxJsonBindingDescription();
                return false;
            }

            try
            {
                var converter = this.GetService<IMvxJsonConverter>();
                requestedDescription = converter.DeserializeObject<MvxJsonBindingDescription>(text);
            }
            catch (Exception exception)
            {
                requestedDescription = null;
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Problem parsing Json tag for databinding " + exception.ToLongString());
                return false;
            }

            return true;
        }

        public bool TryParseBindingSpecification(string text, out MvxJsonBindingSpecification requestedBindings)
        {
            if (string.IsNullOrEmpty(text))
            {
                requestedBindings = new MvxJsonBindingSpecification();
                return false;
            }

            try
            {
                var converter = this.GetService<IMvxJsonConverter>();
                requestedBindings = converter.DeserializeObject<MvxJsonBindingSpecification>(text);
            }
            catch (Exception exception)
            {
                requestedBindings = null;
                MvxBindingTrace.Trace(MvxTraceLevel.Error,"Problem parsing Json tag for databinding " + exception.ToLongString());
                return false;
            }
            return true;
        }
    }
}