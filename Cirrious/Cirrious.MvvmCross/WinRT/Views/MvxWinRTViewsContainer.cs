#region Copyright
// <copyright file="MvxWinRTViewsContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
using System;
using System.Linq;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WinRT.Platform;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WinRT.Views
{
	public class MvxWinRTViewsContainer 
        : MvxViewsContainer
    {
	    private readonly Frame _rootFrame;

	    public MvxWinRTViewsContainer(Frame frame)
		{
			_rootFrame = frame;
		}

	    public override IMvxViewDispatcher Dispatcher
	    {
	        get { return new MvxWinRTViewDispatcher(_rootFrame); }
	    }
	}
}