#region Copyright
// <copyright file="MvxTouchFileStoreService.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.IO;
using Cirrious.MvvmCross.Platform;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Platform
{
    public class MvxTouchFileStoreService : MvxBaseFileStoreService
    {
		public const string ResScheme = "res:";
		
        protected override string FullPath(string path)
        {
			if (path.StartsWith(ResScheme))
				return path.Substring(ResScheme.Length);
			
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), path);
        }
    }
}


// TODO - credits needed!