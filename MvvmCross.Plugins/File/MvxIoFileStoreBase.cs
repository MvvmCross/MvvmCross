// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

namespace MvvmCross.Plugin.File
{
    public class MvxIoFileStoreBase : MvxFileStoreBase
    {
        #region IMvxFileStore Members

        public MvxIoFileStoreBase(bool appendDefaultPath, string basePath)
        {
            BasePath = basePath;
            AppendDefaultPath = appendDefaultPath;
        }

        public override ValueTask<Stream> OpenRead(string path)
        {
            var fullPath = FullPath(path);
            if (!System.IO.File.Exists(fullPath))
            {
                return new ValueTask<Stream>((Stream)null);
            }

            return new ValueTask<Stream>(System.IO.File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }

        public override ValueTask<Stream> OpenWrite(string path)
        {
            var fullPath = FullPath(path);

            if (!System.IO.File.Exists(fullPath))
            {
                return new ValueTask<Stream>(System.IO.File.Create(fullPath));
            }

            return new ValueTask<Stream>(System.IO.File.OpenWrite(fullPath));
        }

        public override ValueTask<bool> Exists(string path)
        {
            var fullPath = FullPath(path);
            return new ValueTask<bool>(System.IO.File.Exists(fullPath));
        }

        public override ValueTask<bool> FolderExists(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            return new ValueTask<bool>(Directory.Exists(fullPath));
        }

        public override ValueTask EnsureFolderExists(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            return new ValueTask();
        }

        public override ValueTask<IEnumerable<string>> GetFilesIn(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            return new ValueTask<IEnumerable<string>>(Directory.GetFiles(fullPath));
        }

        public override ValueTask<IEnumerable<string>> GetFoldersIn(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            return new ValueTask<IEnumerable<string>>(Directory.GetDirectories(fullPath));
        }

        public override ValueTask DeleteFile(string filePath)
        {
            var fullPath = FullPath(filePath);
            System.IO.File.Delete(fullPath);

            return new ValueTask();
        }

        public override ValueTask DeleteFolder(string folderPath, bool recursive)
        {
            var fullPath = FullPath(folderPath);
            Directory.Delete(fullPath, recursive);

            return new ValueTask();
        }

        public override ValueTask<bool> TryMove(string from, string to, bool overwrite)
        {
            try
            {
                var fullFrom = FullPath(from);
                var fullTo = FullPath(to);

                if (!System.IO.File.Exists(fullFrom))
                {
                    MvxPluginLog.Instance.Error("Error during file move {0} : {1}. File does not exist!", from, to);
                    return new ValueTask<bool>(false);
                }

                if (System.IO.File.Exists(fullTo))
                {
                    if (overwrite)
                    {
                        System.IO.File.Delete(fullTo);
                    }
                    else
                    {
                        return new ValueTask<bool>(false);
                    }
                }

                System.IO.File.Move(fullFrom, fullTo);
                return new ValueTask<bool>(true);
            }
            catch (Exception exception)
            {
                MvxPluginLog.Instance.Error("Error during file move {0} : {1} : {2}", from, to, exception.ToLongString());
                return new ValueTask<bool>(false);
            }
        }

        public override ValueTask<bool> TryCopy(string from, string to, bool overwrite)
        {
            try
            {
                var fullFrom = FullPath(from);
                var fullTo = FullPath(to);

                if (!System.IO.File.Exists(fullFrom))
                {
                    MvxPluginLog.Instance.Error("Error during file copy {0} : {1}. File does not exist!", from, to);
                    return new ValueTask<bool>(false);
                }

                System.IO.File.Copy(fullFrom, fullTo, overwrite);
                return new ValueTask<bool>(true);
            }
            catch (Exception exception)
            {
                MvxPluginLog.Instance.Error("Error during file copy {0} : {1} : {2}", from, to, exception.ToLongString());
                return new ValueTask<bool>(false);
            }
        }

        public override string NativePath(string path)
        {
            return FullPath(path);
        }

        public override ValueTask<long> GetSize(string path)
        {
            var length = new FileInfo(path).Length;

            return new ValueTask<long>(length);
        }

        public override ValueTask<DateTime> GetLastWriteTimeUtc(string path)
        {
            var timeUtc = System.IO.File.GetLastWriteTimeUtc(path);

            return new ValueTask<DateTime>(timeUtc);
        }

        #endregion IMvxFileStore Members

        protected string BasePath { get; }

        protected bool AppendDefaultPath { get; }

        protected override ValueTask WriteFileCommon(string path, Action<Stream> streamAction)
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

            return new ValueTask();
        }

        protected override ValueTask<bool> TryReadFileCommon(string path, Func<Stream, bool> streamAction)
        {
            var fullPath = FullPath(path);
            if (!System.IO.File.Exists(fullPath))
            {
                return new ValueTask<bool>(false);
            }

            using (var fileStream = System.IO.File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return new ValueTask<bool>(streamAction(fileStream));
            }
        }

        protected override async ValueTask WriteFileCommonAsync(string path, Func<Stream, ValueTask> streamAction)
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

        protected override async ValueTask<bool> TryReadFileCommonAsync(string path, Func<Stream, ValueTask<bool>> streamAction)
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
