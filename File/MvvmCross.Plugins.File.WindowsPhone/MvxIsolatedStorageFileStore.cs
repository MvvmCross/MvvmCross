// MvxIsolatedStorageFileStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MvvmCross.Plugins.File.WindowsPhone
{
    public class MvxIsolatedStorageFileStore
        : MvxFileStoreBase
    {
        #region IMvxFileStore

        public override Stream OpenRead(string path)
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isf.FileExists(path))
                        return null;

                    return isf.OpenFile(path, FileMode.Open);
                }
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file open {0} : {1}", path, exception.ToLongString());
                return null;
            }
        }

        public override Stream OpenWrite(string path)
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    return new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, isf);
                }
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file save {0} : {1}", path, exception.ToLongString());
                return null;
            }
        }

        public override bool Exists(string path)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return isf.FileExists(path);
            }
        }

        public override bool FolderExists(string folderPath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return isf.DirectoryExists(folderPath);
            }
        }

        public override void EnsureFolderExists(string folderPath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.DirectoryExists(folderPath))
                    return;
                isf.CreateDirectory(folderPath);
            }
        }

        public override IEnumerable<string> GetFilesIn(string folderPath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                folderPath = folderPath.TrimEnd('/');
                var path = folderPath + "/*";
                return isf.GetFileNames(path).Select(x => folderPath + "/" + x).ToArray();
            }
        }

        public override IEnumerable<string> GetFoldersIn(string folderPath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                folderPath = folderPath.TrimEnd('/');
                var path = folderPath + "/*";
                return isf.GetDirectoryNames(path).Select(x => folderPath + "/" + x).ToArray();
            }
        }

        public override void DeleteFile(string path)
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    isf.DeleteFile(path);
                }
            }
            catch (IsolatedStorageException)
            {
                // if the exception was thrown because the file was missing, then we can ignore this
                if (!Exists(path))
                    return;

                throw;
            }
        }

        public override void DeleteFolder(string folderPath, bool recursive)
        {
            if (recursive)
            {
                DeleteFolderRecursive(folderPath);
            }
            else
            {
                DeleteFolderNonRecursive(folderPath);
            }
        }

        public override bool TryMove(string from, string to, bool overwrite)
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isf.FileExists(from))
                    {
                        MvxTrace.Error("Error during file move {0} : {1}. File does not exist!", from, to);
                        return false;
                    }

                    if (isf.FileExists(to))
                    {
                        if (overwrite)
                        {
                            isf.DeleteFile(to);
                        }
                        else
                        {
                            return false;
                        }
                    }

                    isf.MoveFile(from, to);
                    return true;
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error masked during file move {0} : {1} : {2}", from, to, exception.ToLongString());
                return false;
            }
        }

        public override bool TryCopy(string from, string to, bool overwrite)
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isf.FileExists(from))
                    {
                        MvxTrace.Error("Error during file copy {0} : {1}. File does not exist!", from, to);
                        return false;
                    }

                    isf.CopyFile(from, to, overwrite);
                    return true;
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error masked during file copy {0} : {1} : {2}", from, to, exception.ToLongString());
                return false;
            }
        }

        public override string NativePath(string path)
        {
            return path;
        }

        #endregion IMvxFileStore

        private void DeleteFolderRecursive(string folderPath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isf.DirectoryExists(folderPath))
                {
                    return;
                }

                // use KeyValuePair - Tuple is not available in WP 7
                var folderStack = new Stack<KeyValuePair<string, bool>>();
                folderStack.Push(new KeyValuePair<string, bool>(folderPath, false));
                while (folderStack.Count > 0)
                {
                    var stackItem = folderStack.Pop();
                    var folder = stackItem.Key;
                    if (stackItem.Value)
                    {
                        // delete folder as all subfolders are deleted already - avoids unnecessary GetFileNames/GetDirectoryNames
                        isf.DeleteDirectory(folder);
                        continue;
                    }
                    foreach (var file in isf.GetFileNames(Path.Combine(folder, "*")))
                    {
                        isf.DeleteFile(Path.Combine(folder, file));
                    }
                    var subFolders = isf.GetDirectoryNames(Path.Combine(folder, "*"));
                    if (subFolders.Length > 0)
                    {
                        folderStack.Push(new KeyValuePair<string, bool>(folder, true)); // mark folder for later deletion
                        foreach (var subFolder in subFolders)
                        {
                            folderStack.Push(new KeyValuePair<string, bool>(Path.Combine(folder, subFolder), false));
                        }
                    }
                    else
                    {
                        isf.DeleteDirectory(folder);
                    }
                }
            }
        }

        private void DeleteFolderNonRecursive(string folderPath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                isf.DeleteDirectory(folderPath);
            }
        }

        protected override void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var fileStream = new IsolatedStorageFileStream(path, FileMode.Create, isf))
                        streamAction(fileStream);
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file save {0} : {1}", path, exception.ToLongString());
                throw;
            }
        }

        protected override bool TryReadFileCommon(string path, Func<Stream, bool> streamAction)
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isf.FileExists(path))
                    {
                        return false;
                    }

                    using (var fileStream = isf.OpenFile(path, FileMode.Open))
                    {
                        return streamAction(fileStream);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file load {0} : {1}", path, exception.ToLongString());
                return false;
            }
        }

        protected override async Task WriteFileCommonAsync(string path, Func<Stream, Task> streamAction)
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var fileStream = new IsolatedStorageFileStream(path, FileMode.Create, isf))
                        await streamAction(fileStream).ConfigureAwait(false);
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file save {0} : {1}", path, exception.ToLongString());
                throw;
            }
        }

        protected override async Task<bool> TryReadFileCommonAsync(string path, Func<Stream, Task<bool>> streamAction)
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isf.FileExists(path))
                    {
                        return false;
                    }

                    using (var fileStream = isf.OpenFile(path, FileMode.Open))
                    {
                        return await streamAction(fileStream).ConfigureAwait(false);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file load {0} : {1}", path, exception.ToLongString());
                return false;
            }
        }
    }
}