// MvxBaseFileStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

// ReSharper disable all

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Plugins.File
{
    public class MvxIoFileStoreBase
        : MvxFileStoreBase
    {
        #region IMvxFileStore Members

        public MvxIoFileStoreBase(bool appendDefaultPath, string basePath)
        {
            BasePath = basePath;
            AppendDefaultPath = appendDefaultPath;
        }

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

        public override bool TryMove(string from, string to, bool overwrite)
        {
            try
            {
                var fullFrom = FullPath(from);
                var fullTo = FullPath(to);

                if (!System.IO.File.Exists(fullFrom))
                {
                    MvxTrace.Error("Error during file move {0} : {1}. File does not exist!", from, to);
                    return false;
                }

                if (System.IO.File.Exists(fullTo))
                {
                    if (overwrite)
                    {
                        System.IO.File.Delete(fullTo);
                    }
                    else
                    {
                        return false;
                    }
                }

                System.IO.File.Move(fullFrom, fullTo);
                return true;
            }
            catch (Exception exception)
            {
                MvxTrace.Error("Error during file move {0} : {1} : {2}", from, to, exception.ToLongString());
                return false;
            }
        }

        public override bool TryCopy(string from, string to, bool overwrite)
        {
            try
            {
                var fullFrom = FullPath(from);
                var fullTo = FullPath(to);

                if (!System.IO.File.Exists(fullFrom))
                {
                    MvxTrace.Error("Error during file copy {0} : {1}. File does not exist!", from, to);
                    return false;
                }

                System.IO.File.Copy(fullFrom, fullTo, overwrite);
                return true;
            }
            catch (Exception exception)
            {
                MvxTrace.Error("Error during file copy {0} : {1} : {2}", from, to, exception.ToLongString());
                return false;
            }
        }

        public override string NativePath(string path)
        {
            return FullPath(path);
        }

        public override long GetSize(string path)
        {
            return new FileInfo(path).Length;
        }

        public override DateTime GetLastWriteTimeUtc(string path)
        {
            return System.IO.File.GetLastWriteTimeUtc(path);
        }

        #endregion IMvxFileStore Members

        protected string BasePath { get; }

        protected bool AppendDefaultPath { get; }

        protected override void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            var fullPath = FullPath(path);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            using (var fileStream = System.IO.File.OpenWrite(fullPath))
            {
                streamAction?.Invoke(fileStream);
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

        private string FullPath(string path)
        {
            if (!AppendDefaultPath) return path;

            return AppendPath(path);
        }

        protected virtual string AppendPath(string path)
            => Path.Combine(BasePath, path);
    }
}

// ReSharper restore all