using System;
using System.Threading;
using Cirrious.MvvmCross.ExtensionMethods;

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
                requestedBindings = Newtonsoft.Json.JsonConvert.DeserializeObject<MvxJsonBindingSpecification>(text);
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                requestedBindings = null;
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Error,"Problem parsing Json tag for databinding " + exception.ToLongString());
                return false;
            }
            return true;
        }
    }
}