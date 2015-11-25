// MvxBaseFileStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

// ReSharper disable all

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MvvmCross.Plugins.File
{
    public abstract class MvxIoFileStoreBase
        : MvxFileStoreBase
    {
        #region IMvxFileStore Members

        public override Stream OpenRead(string path)
        {
            var fullPath = FullPath(path);
            if (!System.IO.File.Exists(fullPath))
            {
                return null;
            }

            return System.IO.File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        public override Stream OpenWrite(string path)
        {
            var fullPath = FullPath(path);

            if (!System.IO.File.Exists(fullPath))
            {
                return System.IO.File.Create(fullPath);
            }

            return System.IO.File.OpenWrite(fullPath);
        }

        public override bool Exists(string path)
        {
            var fullPath = FullPath(path);
            return System.IO.File.Exists(fullPath);
        }

        public override bool FolderExists(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            return Directory.Exists(fullPath);
        }

        public override void EnsureFolderExists(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
        }

        public override IEnumerable<string> GetFilesIn(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            return Directory.GetFiles(fullPath);
        }

        public override IEnumerable<string> GetFoldersIn(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            return Directory.GetDirectories(fullPath);
        }

        public override void DeleteFile(string filePath)
        {
            var fullPath = FullPath(filePath);
            System.IO.File.Delete(fullPath);
        }

        public override void DeleteFolder(string folderPath, bool recursive)
        {
            var fullPath = FullPath(folderPath);
            Directory.Delete(fullPath, recursive);
        }

        public override bool TryMove(string from, string to, bool deleteExistingTo)
        {
            try
            {
                var fullFrom = FullPath(from);
                var fullTo = FullPath(to);

                if (!System.IO.File.Exists(fullFrom))
                    return false;

                if (System.IO.File.Exists(fullTo))
                {
                    if (deleteExistingTo)
                        System.IO.File.Delete(fullTo);
                    else
                        return false;
                }

                System.IO.File.Move(fullFrom, fullTo);
                return true;
            }
            //catch (ThreadAbortException)
            //{
            //    throw;
            //}
            catch (Exception exception)
            {
                MvxTrace.Error("Error during file move {0} : {1} : {2}", from, to, exception.ToLongString());
                return false;
            }
        }

        public override string NativePath(string path)
        {
            return FullPath(path);
        }

        #endregion IMvxFileStore Members

        protected abstract string FullPath(string path);

        protected override void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            var fullPath = FullPath(path);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            using (var fileStream = System.IO.File.OpenWrite(fullPath))
            {
                streamAction(fileStream);
            }
        }

        protected override bool TryReadFileCommon(string path, Func<Stream, bool> streamAction)
        {
            var fullPath = FullPath(path);
            if (!System.IO.File.Exists(fullPath))
            {
                return false;
            }

            using (var fileStream = System.IO.File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return streamAction(fileStream);
            }
        }

        protected override async Task WriteFileCommonAsync(string path, Func<Stream, Task> streamAction)
        {
            var fullPath = FullPath(path);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            using (var fileStream = System.IO.File.OpenWrite(fullPath))
            {
                await streamAction(fileStream).ConfigureAwait(false);
                return;
            }
        }

        protected override async Task<bool> TryReadFileCommonAsync(string path, Func<Stream, Task<bool>> streamAction)
        {
            var fullPath = FullPath(path);
            if (!System.IO.File.Exists(fullPath))
            {
                return false;
            }

            using (var fileStream = System.IO.File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return await streamAction(fileStream).ConfigureAwait(false);
            }
        }
    }
}

// ReSharper restore all