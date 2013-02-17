// MvxDynamicImageHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Plugins.DownloadCache
{
	public interface IMvxImageHelper<T> 
		: IDisposable
		where T : class
	{
		string DefaultImagePath { get;set; }
		
		string ErrorImagePath { get;set; }

		string ImageUrl { get;set; }

		event EventHandler<MvxValueEventArgs<T>> ImageChanged;
	}
    
}