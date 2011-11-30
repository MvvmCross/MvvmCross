#region Copyright

// <copyright file="MvxShowViewModelRequest.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ViewModel;

namespace Cirrious.MvvmCross.Views
{
    public class MvxShowViewModelRequest
    {
        public MvxShowViewModelRequest()
        {
        }

        public MvxShowViewModelRequest(Type viewModelType, IDictionary<string, string> parameterValues)
        {
            ViewModelType = viewModelType;
            ParameterValues = parameterValues;
        }

        public Type ViewModelType { get; set; }
        public IDictionary<string, string> ParameterValues { get; set; }

        public static MvxShowViewModelRequest GetDefaultRequest(Type viewModelType)
        {
            return new MvxShowViewModelRequest(viewModelType, null);
        }
    }

    public class MvxShowViewModelRequest<TViewModel> : MvxShowViewModelRequest where TViewModel : IMvxViewModel
    {
        public MvxShowViewModelRequest(IDictionary<string, string> parameterValues)
            : base(typeof (TViewModel), parameterValues)
        {
        }

        public static MvxShowViewModelRequest GetDefaultRequest()
        {
            return GetDefaultRequest(typeof (TViewModel));
        }
    }
}