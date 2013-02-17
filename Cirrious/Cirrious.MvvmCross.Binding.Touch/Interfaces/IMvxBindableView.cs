// IMvxBindableView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Touch.Interfaces
{
    public interface IMvxBindableView
		: IDataContext
		, IMvxServiceConsumer
    {
		Action CallOnNextDataContextChange { get; set; }
		IList<IMvxUpdateableBinding> Bindings {get;set;}
    }
}