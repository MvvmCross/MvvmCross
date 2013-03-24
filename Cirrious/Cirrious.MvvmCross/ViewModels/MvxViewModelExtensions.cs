// MvxViewModelExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Linq;
using System.Reflection;
using Cirrious.MvvmCross.Platform;

namespace Cirrious.MvvmCross.ViewModels
{
    public static class MvxViewModelExtensions
    {
        public static void CallBundleMethods(this IMvxViewModel viewModel, string methodName, IMvxBundle bundle)
        {
            var methods = viewModel
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
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
                && parameters[0].ParameterType == typeof (IMvxBundle))
            {
                // this method is the 'normal' interface method
                methodInfo.Invoke(viewModel, new object[] {bundle});
                return;
            }

            if (parameters.Count() == 1
                && !MvxStringToTypeParser.TypeSupported(parameters[0].ParameterType))
            {
                // call method using typed object
                var value = bundle.Read(parameters[0].ParameterType);
                methodInfo.Invoke(viewModel, new[] {value});
                return;
            }

            // call method using named method arguments
            var invokeWith = bundle.CreateArgumentList(parameters, viewModel.GetType().Name)
                                   .ToArray();
            methodInfo.Invoke(viewModel, invokeWith);
        }
    }
}