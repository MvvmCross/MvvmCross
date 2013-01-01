// MvxFileStoreService.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#if false

#warning removed as its not really useful currently

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

#endif