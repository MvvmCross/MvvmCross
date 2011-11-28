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

using System.Collections.Generic;

namespace Cirrious.MvvmCross.Views
{
    public class MvxShowViewModelRequest
    {
        public MvxShowViewModelRequest()
        {            
        }

        public MvxShowViewModelRequest(MxvViewModelAction viewModelAction, IDictionary<string, string> parameterValues)
        {
            ViewModelAction = viewModelAction;
            ParameterValues = parameterValues;
        }

        public MxvViewModelAction ViewModelAction { get; set; }
        public IDictionary<string, string> ParameterValues { get; set; }
    }

    public class MvxShowViewModelRequest<TViewModel> : MvxShowViewModelRequest
    {
        public MvxShowViewModelRequest(MxvViewModelAction<TViewModel> viewModelAction, IDictionary<string, string> parameterValues)
            : base(viewModelAction, parameterValues)
        {
        }
    }
}