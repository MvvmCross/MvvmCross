using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxDefaultViewModelLocator
        : IMvxViewModelLocator
    {
        public bool TryLoad(Type viewModelType, IDictionary<string, string> parameters, out IMvxViewModel model)
        {
            model = null;
            var constructor = viewModelType
                .GetConstructors()
                .FirstOrDefault(c => c.GetParameters().All(p=> p.ParameterType == typeof(string)));

            if (constructor == null)
                return false;

            var invokeWith = new List<object>();
            foreach (var parameter in constructor.GetParameters())
            {
                string parameterValue = null;
                if (parameters == null ||
                    !parameters.TryGetValue(parameter.Name, out parameterValue))
                {
                    MvxTrace.Trace("Missing parameter in call to {0} - missing parameter {1} - asssuming null", viewModelType,
                                   parameter.Name);
                }
                invokeWith.Add(parameterValue);
            }

            model = Activator.CreateInstance(viewModelType, invokeWith.ToArray()) as IMvxViewModel;
            return (model != null);
        }
    }
}