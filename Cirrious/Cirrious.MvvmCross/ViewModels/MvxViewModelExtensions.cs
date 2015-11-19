// MvxViewModelExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
{
    public static class MvxViewModelExtensions
    {
        public static void CallBundleMethods(this IMvxViewModel viewModel, string methodName, IMvxBundle bundle)
        {
            var methods = viewModel
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .Where(m => m.Name == methodName)
                .Where(m => !m.IsAbstract)
                .ToList();

            foreach (var methodInfo in methods)
            {
                viewModel.CallBundleMethod(methodInfo, bundle);
            }
        }

        public static void CallBundleMethod(this IMvxViewModel viewModel, MethodInfo methodInfo, IMvxBundle bundle)
        {
            var parameters = methodInfo.GetParameters().ToArray();
            if (parameters.Count() == 1
                && parameters[0].ParameterType == typeof(IMvxBundle))
            {
                // this method is the 'normal' interface method
                methodInfo.Invoke(viewModel, new object[] { bundle });
                return;
            }

            if (parameters.Count() == 1
                && !MvxSingletonCache.Instance.Parser.TypeSupported(parameters[0].ParameterType))
            {
                // call method using typed object
                var value = bundle.Read(parameters[0].ParameterType);
                methodInfo.Invoke(viewModel, new[] { value });
                return;
            }

            // call method using named method arguments
            var invokeWith = bundle.CreateArgumentList(parameters, viewModel.GetType().Name)
                                   .ToArray();
            methodInfo.Invoke(viewModel, invokeWith);
        }

        public static IMvxBundle SaveStateBundle(this IMvxViewModel viewModel)
        {
            var toReturn = new MvxBundle();
            var methods = viewModel.GetType()
                                   .GetMethods()
                                   .Where(m => m.Name == "SaveState")
                                   .Where(m => m.ReturnType != typeof(void))
                                   .Where(m => !m.GetParameters().Any());

            foreach (var methodInfo in methods)
            {
                // use methods like `public T SaveState()`
                var stateObject = methodInfo.Invoke(viewModel, new object[0]);
                if (stateObject != null)
                {
                    toReturn.Write(stateObject);
                }
            }

            // call the general `public void SaveState(bundle)` method too
            viewModel.SaveState(toReturn);

            return toReturn;
        }
    }
}