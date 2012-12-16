#region Copyright
// <copyright file="MvxWpfFileStoreService.cs" company="Cirrious">
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

namespace Cirrious.MvvmCross.Plugins.File.Wpf
{
    public class MvxWpfFileStoreService : MvxBaseFileStoreService
    {
        protected override string FullPath(string path)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);
        }
    }
}


// TODO - credits needed!