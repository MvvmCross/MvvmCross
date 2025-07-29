// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using MvvmCross.Base;

namespace MvvmCross.ViewModels
{
#nullable enable
    public static class MvxViewModelExtensions
    {
        public static void CallBundleMethods<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TViewModel>(
            this TViewModel viewModel, string methodName, IMvxBundle? bundle)
                where TViewModel : IMvxViewModel
        {
            ArgumentNullException.ThrowIfNull(viewModel);

            var methods = viewModel.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .Where(m => m.Name == methodName && !m.IsAbstract);

            foreach (var methodInfo in methods)
            {
                viewModel.CallBundleMethod(methodInfo, bundle);
            }
        }

        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The parameter type is determined at runtime and may not have the required annotations")]
        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Callers should guarantee the dynamically accessed members are preserved")]
        public static void CallBundleMethod<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TViewModel>(
            this TViewModel viewModel, MethodInfo methodInfo, IMvxBundle? bundle)
                where TViewModel : IMvxViewModel
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(methodInfo);

            var parameters = methodInfo.GetParameters().ToArray();

            // Make sure we have a bundle that matches function parameters
            if (bundle == null && parameters.Length > 0)
                return;

            if (bundle != null && parameters.Length == 1)
            {
                if (parameters[0].ParameterType == typeof(IMvxBundle))
                {
                    // this method is the 'normal' interface method
                    methodInfo.Invoke(viewModel, new object[] { bundle });
                    return;
                }

                if (MvxSingletonCache.Instance?.Parser?.TypeSupported(parameters[0].ParameterType) == false)
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

        public static IMvxBundle SaveStateBundle<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TViewModel>(this TViewModel viewModel)
            where TViewModel : IMvxViewModel
        {
            ArgumentNullException.ThrowIfNull(viewModel);

            var toReturn = new MvxBundle();
            var methods =
                viewModel
                    .GetType()
                    .GetMethods()
                    .Where(m => m.Name == "SaveState" && m.ReturnType != typeof(void) && !m.GetParameters().Any());

            foreach (var methodInfo in methods)
            {
                // use methods like `public T SaveState()`
                var stateObject = methodInfo.Invoke(viewModel, []);
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
#nullable restore
}
