// MvxDefaultViewModelLocator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Application
{
    public class MvxDefaultViewModelLocator
        : MvxBaseViewModelLocator
    {
        public string InitMethodName { get; set; }
        public string ReloadStateMethodName { get; set; }
        public string StartMethodName { get; set; }

        public MvxDefaultViewModelLocator()
        {
            InitMethodName = "Init";
            ReloadStateMethodName = "ReloadState";
            StartMethodName = "Start";
        }

        public override bool TryLoad(Type viewModelType,
                                     IMvxBundle parameterValueLookup,
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

            // Initialise the ViewModel...
            try
            {
                viewModel.Init(parameterValueLookup);
                TryCallCustomInit(viewModel, parameterValueLookup);
                viewModel.LoadState(savedState);
                TryCallCustomReloadState(viewModel, savedState);
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

        protected virtual void TryCallCustomInit(IMvxViewModel viewModel, IMvxBundle parameterValueLookup)
        {
            var init = viewModel
                .GetType()
                .GetMethods()
                .FirstOrDefault(x => x.Name == InitMethodName);
            if (init == null)
                return;

            var parameters = init.GetParameters().ToList();
            if (parameters.Count() == 1
                && parameters[0].ParameterType == typeof (IMvxBundle))
            {
                // skip - this happens normally
                return;
            }

            if (parameters.Count() == 1
                && !parameters[0].ParameterType.IsValueType
                && parameters[0].ParameterType != typeof (string))
            {
                var parameter = parameterValueLookup.Read(parameters[0].ParameterType);
                init.Invoke(viewModel, new[] {parameter});
                return;
            }

            var invokeWith = CreateArgumentList(viewModel.GetType(), parameterValueLookup, parameters)
                .ToArray();
            init.Invoke(viewModel, invokeWith);
        }

        protected virtual void TryCallCustomReloadState(IMvxViewModel viewModel, IMvxBundle savedState)
        {
            var reload = viewModel
                .GetType()
                .GetMethods()
                .FirstOrDefault(x => x.Name == ReloadStateMethodName);
            if (reload == null)
                return;

            if (savedState == null)
                return;

            var parameters = reload.GetParameters().ToArray();
            if (parameters.Count() == 1
                && parameters[0].ParameterType == typeof (IMvxBundle))
            {
                // skip - this happens normally
            }
            else if (parameters.Count() == 1
                     && !parameters[0].ParameterType.IsValueType
                     && parameters[0].ParameterType != typeof (string))
            {
                var value = savedState.Read(parameters[0].ParameterType);
                reload.Invoke(viewModel, new[] {value});
            }
            else
            {
                var invokeWith = CreateArgumentList(viewModel.GetType(), savedState, parameters)
                    .ToArray();
                reload.Invoke(viewModel, invokeWith);
            }
        }
    }
}