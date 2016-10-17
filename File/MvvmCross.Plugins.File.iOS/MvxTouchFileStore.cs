// MvxIosFileStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;

namespace MvvmCross.Plugins.File.iOS
{
    [Preserve(AllMembers = true)]
    public class MvxIosFileStore : MvxIoFileStoreBase
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