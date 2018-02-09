﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Base;
using MvvmCross.Logging;
using MvvmCross.Test;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.UnitTest.Mocks.Dispatchers
{
    public class NavigationMockDispatcher 
        : IMvxMainThreadDispatcher, IMvxViewDispatcher
    {
        public readonly List<MvxViewModelRequest> Requests = new List<MvxViewModelRequest>();
        public readonly List<MvxPresentationHint> Hints = new List<MvxPresentationHint>();

        public virtual bool RequestMainThreadAction(Action action, 
                                                    bool maskExceptions = true)
        {
            action();
            return true;
        }

        public virtual bool ShowViewModel(MvxViewModelRequest request)
        {
            var debugString = $"ShowViewModel: '{request.ViewModelType.Name}' ";
            if (request.ParameterValues != null)
                debugString += $"with parameters: {string.Join(",", request.ParameterValues.Select(pair => $"{{ {pair.Key}={pair.Value} }}"))}";
            else
                debugString += "without parameters";
            MvxTestLog.Instance.Log(MvxLogLevel.Debug, () => debugString);

            Requests.Add(request);
            return true;
        }

        public virtual bool ChangePresentation(MvxPresentationHint hint)
        {
            Hints.Add(hint);
            return true;
        }

    }
}
