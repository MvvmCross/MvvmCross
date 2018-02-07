// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform;

namespace MvvmCross.Plugin.File.Platform.iOS
{
    [Preserve(AllMembers = true)]
    public class MvxIosFileStore : MvxIoFileStoreBase
    {
        public MvxIosFileStore(bool appendDefaultPath, string basePath)
            : base(appendDefaultPath, basePath)
        {
        }

        public const string ResScheme = "res:";

        protected override string AppendPath(string path)
        {
            if (path.StartsWith(ResScheme))
                return path.Substring(ResScheme.Length);
            
            return base.AppendPath(path);
        }
    }
}

// TODO - credits needed!
