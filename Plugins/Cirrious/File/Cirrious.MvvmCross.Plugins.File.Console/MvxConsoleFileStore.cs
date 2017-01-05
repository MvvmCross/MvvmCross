// MvxAndroidFileStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using System.IO;
using Cirrious.CrossCore;

#endregion

namespace Cirrious.MvvmCross.Plugins.File.Console
{
    public class MvxConsoleFileStore
        : MvxFileStore
          
    {
        protected override string FullPath(string path)
        {
            return Path.Combine(System.Environment.CurrentDirectory, path);
        }
    }
}