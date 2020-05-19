﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Reflection;
using MvvmCross.Base;

namespace MvvmCross.ViewModels
{
    public static class MvxViewModelExtensions
    {
        public static void CallBundleMethods(this IMvxViewModel viewModel, string methodName, IMvxBundle bundle)
        {
            var methods = viewModel
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .Where(m => m.Name == methodName && !m.IsAbstract);

            foreach (var methodInfo in methods)
            {
                viewModel.CallBundleMethod(methodInfo, bundle);
            }
        }

        public static void CallBundleMethod(this IMvxViewModel viewModel, MethodInfo methodInfo, IMvxBundle bundle)
        {
            var parameters = methodInfo.GetParameters().ToArray();

            //Make sure we have a bundle that matches function parameters
            if (bundle == null && parameters.Length > 0)
                return;
            
            if (parameters.Length == 1)
            {
                if (parameters[0].ParameterType == typeof(IMvxBundle))
                {
                    // this method is the 'normal' interface method
                    methodInfo.Invoke(viewModel, new object[] { bundle });
                    return;
                }

                if (!MvxSingletonCache.Instance.Parser.TypeSupported(parameters[0].ParameterType))
                {
                    // call method using typed object
                    var value = bundle.Read(parameters[0].ParameterType);
                    methodInfo.Invoke(viewModel, new[] { value });
                    return;
                }
            }

            // call method using named method arguments. If bundle is null, the null-check makes sure that Init still is called.
            var invokeWith = bundle?.CreateArgumentList(parameters, viewModel.GetType().Name)
                                   .ToArray();
            methodInfo.Invoke(viewModel, invokeWith);
        }

        public static IMvxBundle SaveStateBundle(this IMvxViewModel viewModel)
        {
            var toReturn = new MvxBundle();
            var methods = viewModel.GetType()
                                   .GetMethods()
                                   .Where(m => m.Name == "SaveState" && m.ReturnType != typeof(void) && !m.GetParameters().Any());

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
