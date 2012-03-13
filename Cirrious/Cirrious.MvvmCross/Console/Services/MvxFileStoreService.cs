#region Copyright
// <copyright file="MvxFileStoreService.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cirrious.MvvmCross.Platform;

#endregion

namespace Cirrious.MvvmCross.Console.Services
{
    public class MvxFileStoreService : MvxBaseFileStoreService
    {
        protected override string FullPath(string path)
        {
            return Path.Combine(Environment.CurrentDirectory, path);
        }
    }
}