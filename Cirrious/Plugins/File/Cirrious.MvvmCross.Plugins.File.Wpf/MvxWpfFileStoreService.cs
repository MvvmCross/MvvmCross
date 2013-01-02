﻿// MvxWpfFileStoreService.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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