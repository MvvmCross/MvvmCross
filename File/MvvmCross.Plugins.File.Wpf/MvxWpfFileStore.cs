// MvxWpfFileStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;

namespace MvvmCross.Plugins.File.Wpf
{
    public class MvxWpfFileStore : MvxIoFileStoreBase
    {
        private readonly string _rootFolder;

        public MvxWpfFileStore(string rootFolder)
        {
            _rootFolder = rootFolder;
        }

        protected override string FullPath(string path)
        {
            return Path.Combine(_rootFolder, path);
        }
    }
}

// TODO - credits needed!