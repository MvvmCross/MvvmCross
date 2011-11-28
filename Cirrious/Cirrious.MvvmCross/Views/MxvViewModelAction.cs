#region Copyright
// <copyright file="MxvViewModelAction.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;

namespace Cirrious.MvvmCross.Views
{
    public class MxvViewModelAction
    {
        public MxvViewModelAction()
        {            
        }

        public MxvViewModelAction(Type viewModelType, string actionName)
        {
            ViewModelType = viewModelType;
            ActionName = actionName;
        }

        public string ActionName { get; set; }
        public Type ViewModelType { get; set; }
        public string Key { get { return string.Format("{0}:{1}", ViewModelType.FullName, ActionName); } }
    }

    public class MxvViewModelAction<TViewModel> : MxvViewModelAction
    {
        public MxvViewModelAction(string actionName)
            : base(typeof(TViewModel), actionName)
        {
        }
    }
}