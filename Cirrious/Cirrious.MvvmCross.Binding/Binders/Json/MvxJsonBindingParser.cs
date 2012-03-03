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
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Newtonsoft.Json;

namespace Cirrious.MvvmCross.Binding.Binders.Json
{
    public class MvxJsonBindingParser
    {
        public bool TryParseBindingSpecification(string text, out MvxJsonBindingSpecification requestedBindings)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                requestedBindings = new MvxJsonBindingSpecification();
                return false;
            }

            try
            {
                requestedBindings = JsonConvert.DeserializeObject<MvxJsonBindingSpecification>(text);
            }
            catch (ThreadAbortException)
            {
                throw;
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