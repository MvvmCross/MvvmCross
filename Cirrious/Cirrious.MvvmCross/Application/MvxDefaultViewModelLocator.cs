// MvxDefaultViewModelLocator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Application
{
    public class MvxDefaultViewModelLocator
        : IMvxViewModelLocator
    {
        public string CustomInitMethodName { get; set; }
        public string CustomReloadStateMethodName { get; set; }

        public MvxDefaultViewModelLocator()
        {
            CustomInitMethodName = "Init";
            CustomReloadStateMethodName = "ReloadState";
        }

        public virtual bool TryLoad(Type viewModelType,
                                     IMvxBundle parameterValues,
                                     IMvxBundle savedState,
                                     out IMvxViewModel viewModel)
        {
            viewModel = null;

            try
            {
                viewModel = (IMvxViewModel) Mvx.IocConstruct(viewModelType);
            }
            catch (Exception exception)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "Problem creating viewModel of type {0} - problem {1}",
                               viewModelType.Name, exception.ToLongString());
                return false;
            }

            try
            {
                viewModel.Init(parameterValues);
                CallCustomInitMethods(viewModel, parameterValues);
                viewModel.ReloadState(savedState);
                CallReloadStateMethods(viewModel, savedState);
                viewModel.Start();
            }
            catch (Exception exception)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "Problem initialising viewModel of type {0} - problem {1}",
                               viewModelType.Name, exception.ToLongString());
                return false;
            }

            return true;
        }

        protected virtual void CallCustomInitMethods(IMvxViewModel viewModel, IMvxBundle parameterValues)
        {
            CallCustomMethods(viewModel, CustomInitMethodName, parameterValues);
        }

        protected virtual void CallReloadStateMethods(IMvxViewModel viewModel, IMvxBundle savedState)
        {
            CallCustomMethods(viewModel, CustomReloadStateMethodName, savedState);
        }

        protected virtual void CallCustomMethods(IMvxViewModel viewModel, string methodName, IMvxBundle savedState)
        {
            var reloadMethods = viewModel
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.Name == methodName)
                .Where(m => !m.IsAbstract)
                .ToList();

            foreach (var methodInfo in reloadMethods)
            {
                CallCustomMethod(viewModel, savedState, methodInfo);
            }
        }

        protected virtual void CallCustomMethod(IMvxViewModel viewModel, IMvxBundle savedState, MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters().ToArray();
            if (parameters.Count() == 1
                && parameters[0].ParameterType == typeof (IMvxBundle))
            {
                // this method is the 'normal' interface method
                // - we'll call it conventionally outside of this mechanism
                // - so return
                return;
            }
            
            if (parameters.Count() == 1
                     && !parameters[0].ParameterType.IsValueType
                     && parameters[0].ParameterType != typeof (string))
            {
                // call method using typed object
                var value = savedState.Read(parameters[0].ParameterType);
                methodInfo.Invoke(viewModel, new[] {value});
                return;
            }

            // call method using named method arguments
            var invokeWith = savedState.CreateArgumentList(viewModel.GetType(), parameters)
                .ToArray();
            methodInfo.Invoke(viewModel, invokeWith);
        }
    }
}