// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Uap;

namespace MvvmCross.Plugin.File.Platforms.Uap
{
    // note that we use the full WindowsStore name here deliberately to avoid 'Store' naming confusion
    public class MvxWindowsFileStore : MvxFileStoreBase
    {
        public override async ValueTask<Stream> OpenRead(string path)
        {
            try
            {
                var storageFile = await StorageFileFromRelativePath(path).ConfigureAwait(false);
                var streamWithContentType = await storageFile.OpenReadAsync();
                return streamWithContentType.AsStreamForRead();
            }
            catch (Exception exception)
            {
                MvxPluginLog.Instance.Trace("Error during file load {0} : {1}", path, exception.ToLongString());
                return null;
            }
        }

        public override async ValueTask<Stream> OpenWrite(string path)
        {
            try
            {
                StorageFile storageFile;

                storageFile = await Exists(path) ? await StorageFileFromRelativePath(path).ConfigureAwait(false) : await CreateStorageFileFromRelativePath(path).ConfigureAwait(false);

                var streamWithContentType = await storageFile.OpenAsync(FileAccessMode.ReadWrite);
                return streamWithContentType.AsStream();
            }
            catch (Exception exception)
            {
                MvxPluginLog.Instance.Trace("Error during file save {0} : {1}", path, exception.ToLongString());
                throw;
            }
        }

        public override async ValueTask<bool> TryMove(string from, string to, bool overwrite)
        {
            try
            {
                var fromFile = await StorageFileFromRelativePath(from);

                if (overwrite)
                {
                    if (! await SafeDeleteFile(to).ConfigureAwait(false))
                    {
                        return false;
                    }
                }

                var fullToPath = ToFullPath(to);
                var toDirectory = Path.GetDirectoryName(fullToPath);
                var toFileName = Path.GetFileName(fullToPath);
                var toStorageFolder = await StorageFolder.GetFolderFromPathAsync(toDirectory);
                await fromFile.MoveAsync(toStorageFolder, toFileName);
                return true;
            }
            catch (Exception exception)
            {
                MvxPluginLog.Instance.Trace("Exception during file move from {0} to {1} - {2}", from, to, exception.ToLongString());
                return false;
            }
        }

        public override async ValueTask<bool> TryCopy(string @from, string to, bool overwrite)
        {
            try
            {
                var fromFile = await StorageFileFromRelativePath(from);

                var fullToPath = ToFullPath(to);
                var toDirectory = Path.GetDirectoryName(fullToPath);
                var toFileName = Path.GetFileName(fullToPath);

                if (overwrite)
                {
                    var toFile = await StorageFileFromRelativePath(to).ConfigureAwait(false);
                    await fromFile.CopyAndReplaceAsync(toFile);
                }
                else
                {
                    var toStorageFolder = await StorageFolder.GetFolderFromPathAsync(toDirectory);
                    await fromFile.CopyAsync(toStorageFolder, toFileName);
                }
                return true;
            }
            catch (Exception exception)
            {
                MvxPluginLog.Instance.Trace("Exception during file copy from {0} to {1} - {2}", from, to, exception.ToLongString());
                return false;
            }
        }

        public override async ValueTask<bool> Exists(string path)
        {
            try
            {
                // this implementation is based on code from https://github.com/MvvmCross/MvvmCross/issues/521
                path = ToFullPath(path);
                var fileName = Path.GetFileName(path);
                var directoryPath = Path.GetDirectoryName(path);
                if (!await FolderExists(directoryPath))
                    return false;

                var directory = await StorageFolder.GetFolderFromPathAsync(directoryPath);
                await directory.GetFileAsync(fileName);
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                MvxPluginLog.Instance.Trace("Exception seen in Exists - seen for path: {0} - {1}", path, ex.ToLongString());
                throw;
            }
        }

        public override async ValueTask<bool> FolderExists(string folderPath)
        {
            // contributed by @AlexMortola via Stackoverflow creative commons
            // http://stackoverflow.com/questions/19890756/mvvmcross-notimplementedexception-calling-ensurefolderexists-method-of-imvxfile
            try
            {
                folderPath = ToFullPath(folderPath);
                folderPath = folderPath.TrimEnd('\\');

                var thisFolder = await StorageFolder.GetFolderFromPathAsync(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                MvxPluginLog.Instance.Trace("Exception in FolderExists - folderPath: {0} - {1}", folderPath, ex.ToLongString());
                throw;
            }
        }

        public override async ValueTask EnsureFolderExists(string folderPath)
        {
            if (await FolderExists(folderPath))
                return;

            var rootFolder = ToFullPath(string.Empty);
            var storageFolder = await StorageFolder.GetFolderFromPathAsync(rootFolder);
            await CreateFolderAsync(storageFolder, folderPath).ConfigureAwait(false);
        }

        private static async ValueTask<StorageFolder> CreateFolderAsync(StorageFolder rootFolder, string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
                return rootFolder;
            var currentFolder = await CreateFolderAsync(rootFolder, Path.GetDirectoryName(folderPath)).ConfigureAwait(false);

            //folder name may be empty if original path was ended by a separator like My/Custom/Path/ instead of My/Custom/Path
            var folderName = Path.GetFileName(folderPath);
            if (string.IsNullOrEmpty(folderName))
                return currentFolder;
            else
                return await currentFolder.CreateFolderAsync(Path.GetFileName(folderPath), CreationCollisionOption.OpenIfExists);
        }

        public override async ValueTask<IEnumerable<string>> GetFilesIn(string folderPath)
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(ToFullPath(folderPath));
            var files = await folder.GetFilesAsync();
            return files.Select(x => x.Path);
        }

        public override async ValueTask<IEnumerable<string>> GetFoldersIn(string folderPath)
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(ToFullPath(folderPath));
            var files = await folder.GetFoldersAsync();
            return files.Select(x => x.Path);
        }

        public override void DeleteFile(string path)
        {
            SafeDeleteFile(path);
        }

        public override async ValueTask DeleteFolder(string folderPath, bool recursive)
        {
            // contributed by @AlexMortola via Stackoverflow creative commons
            // http://stackoverflow.com/questions/19890756/mvvmcross-notimplementedexception-calling-ensurefolderexists-method-of-imvxfile
            try
            {
                var directory = ToFullPath(folderPath);
                var storageFolder = await StorageFolder.GetFolderFromPathAsync(directory);
                await storageFolder.DeleteAsync();
            }
            catch (FileNotFoundException)
            {
                //Folder doesn't exist. Nothing to do
            }
            catch (Exception ex)
            {
                MvxPluginLog.Instance.Trace("Exception in DeleteFolder - folderPath: {0} - {1}", folderPath, ex.ToLongString());
                throw;
            }
        }

        protected override async ValueTask WriteFileCommon(string path, Action<Stream> streamAction)
        {
            // from https://github.com/MvvmCross/MvvmCross/issues/500 we delete any existing file
            // before writing the new one
            SafeDeleteFile(path);

            try
            {
                var storageFile = await CreateStorageFileFromRelativePath(path).ConfigureAwait(false);
                using (var streamWithContentType = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (var stream = streamWithContentType.AsStreamForWrite())
                    {
                        streamAction(stream);
                    }
                }
            }
            catch (Exception exception)
            {
                MvxPluginLog.Instance.Trace("Error during file save {0} : {1}", path, exception.ToLongString());
                throw;
            }
        }

        protected override async ValueTask WriteFileCommonAsync(string path, Func<Stream, ValueTask> streamAction)
        {
            // from https://github.com/MvvmCross/MvvmCross/issues/500 we delete any existing file
            // before writing the new one
            await SafeDeleteFileAsync(path);

            try
            {
                var storageFile = await CreateStorageFileFromRelativePath(path).ConfigureAwait(false);
                using (var streamWithContentType =
                        await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (var stream = streamWithContentType.AsStreamForWrite())
                    {
                        await streamAction(stream).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception exception)
            {
                MvxPluginLog.Instance.Trace("Error during file save {0} : {1}", path, exception.ToLongString());
                throw;
            }
        }

        protected override async ValueTask<bool> TryReadFileCommon(string path, Func<Stream, bool> streamAction)
        {
            try
            {
                var storageFile = await StorageFileFromRelativePath(path);
                using (var streamWithContentType = await storageFile.OpenReadAsync())
                {
                    using (var stream = streamWithContentType.AsStreamForRead())
                    {
                        return streamAction(stream);
                    }
                }
            }
            catch (Exception exception)
            {
                MvxPluginLog.Instance.Trace("Error during file load {0} : {1}", path, exception.ToLongString());
                return false;
            }
        }

        protected override async ValueTask<bool> TryReadFileCommonAsync(string path, Func<Stream, ValueTask<bool>> streamAction)
        {
            try
            {
                var storageFile = await StorageFileFromRelativePath(path).ConfigureAwait(false);
                using (var streamWithContentType = await storageFile.OpenReadAsync())
                {
                    using (var stream = streamWithContentType.AsStreamForRead())
                    {
                        return await streamAction(stream).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception exception)
            {
                MvxPluginLog.Instance.Trace("Error during file load {0} : {1}", path, exception.ToLongString());
                return false;
            }
        }

        private static async ValueTask<bool> SafeDeleteFile(string path)
        {
            try
            {
                var toFile = await StorageFileFromRelativePath(path).ConfigureAwait(false);
                await toFile.DeleteAsync();
                return true;
            }
            catch (FileNotFoundException)
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async ValueTask<bool> SafeDeleteFileAsync(string path)
        {
            try
            {
                var toFile = await StorageFileFromRelativePath(path).ConfigureAwait(false);
                await toFile.DeleteAsync();
                return true;
            }
            catch (FileNotFoundException)
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async ValueTask<StorageFile> StorageFileFromRelativePath(string path)
        {
            var fullPath = ToFullPath(path);
            var storageFile = await StorageFile.GetFileFromPathAsync(fullPath);
            return storageFile;
        }

        private static async ValueTask<StorageFile> CreateStorageFileFromRelativePath(string path)
        {
            var fullPath = ToFullPath(path);
            var directory = Path.GetDirectoryName(fullPath);
            var fileName = Path.GetFileName(fullPath);
            var storageFolder = await StorageFolder.GetFolderFromPathAsync(directory);
            var storageFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            return storageFile;
        }

        public override string NativePath(string path)
        {
            return ToFullPath(path);
        }

        private static string ToFullPath(string path)
        {
            var localFolderPath = ApplicationData.Current.LocalFolder.Path;
            return Path.Combine(localFolderPath, path);
        }

        public override async ValueTask<long> GetSize(string path)
        {
            var storageFile = await StorageFileFromRelativePath(path).ConfigureAwait(false);
            var result = await storageFile.GetBasicPropertiesAsync();
            return (long)result.Size;
        }

        public override async ValueTask<DateTime> GetLastWriteTimeUtc(string path)
        {
            var storageFile = await StorageFileFromRelativePath(path).ConfigureAwait(false);
            var result = await storageFile.GetBasicPropertiesAsync();
            return result.DateModified.UtcDateTime;
        }
    }
}
