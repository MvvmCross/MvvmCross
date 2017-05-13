// MvxIosFileStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Plugins.File.iOS
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