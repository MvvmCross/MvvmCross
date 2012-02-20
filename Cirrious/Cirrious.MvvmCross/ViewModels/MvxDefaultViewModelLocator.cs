using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Interfaces.ViewModels;

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
                parameters.TryGetValue(parameter.Name, out parameterValue);
                invokeWith.Add(parameterValue);
            }

            model = Activator.CreateInstance(viewModelType, invokeWith.ToArray()) as IMvxViewModel;
            return (model != null);
        }
    }
}