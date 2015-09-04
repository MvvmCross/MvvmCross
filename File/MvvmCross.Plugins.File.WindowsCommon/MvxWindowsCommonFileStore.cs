// MvxWindowsStoreBlockingFileStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.WindowsCommon.Platform;

namespace MvvmCross.Plugins.File.WindowsCommon
{
    // note that we use the full WindowsStore name here deliberately to avoid 'Store' naming confusion
    public class MvxWindowsCommonFileStore : MvxFileStoreBase
    {
        public override Stream OpenRead(string path)
        {
            try
            {
                var storageFile = StorageFileFromRelativePath(path);
                var streamWithContentType = storageFile.OpenReadAsync().Await();
                return streamWithContentType.AsStreamForRead();
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file load {0} : {1}", path, exception.ToLongString());
                return null;
            }
        }

        public override Stream OpenWrite(string path)
        {
            try
            {
                StorageFile storageFile;

                if (Exists(path))
                    storageFile = StorageFileFromRelativePath(path);
                else
                    storageFile = CreateStorageFileFromRelativePathAsync(path).GetAwaiter().GetResult();

                var streamWithContentType = storageFile.OpenAsync(FileAccessMode.ReadWrite).Await();
                return streamWithContentType.AsStream();
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file save {0} : {1}", path, exception.ToLongString());
                throw;
            }
        }

        public override bool TryMove(string from, string to, bool deleteExistingTo)
        {
            try
            {
                StorageFile fromFile;

                try
                {
                    fromFile = StorageFileFromRelativePath(from);
                }
                catch (FileNotFoundException)
                {
                    return false;
                }

                if (deleteExistingTo)
                {
                    if (!SafeDeleteFile(to))
                    {
                        return false;
                    }
                }

                var fullToPath = ToFullPath(to);
                var toDirectory = Path.GetDirectoryName(fullToPath);
                var toFileName = Path.GetFileName(fullToPath);
                var toStorageFolder = StorageFolder.GetFolderFromPathAsync(toDirectory).Await();
                fromFile.MoveAsync(toStorageFolder, toFileName).Await();
                return true;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Exception during file move from {0} to {1} - {2}", from, to, exception.ToLongString());
                return false;
            }
        }

        public override bool Exists(string path)
        {
            try
            {
                // this implementation is based on code from https://github.com/MvvmCross/MvvmCross/issues/521
                path = ToFullPath(path);
                var fileName = Path.GetFileName(path);
                var directoryPath = Path.GetDirectoryName(path);
                if (!FolderExists(directoryPath))
                    return false;

                var directory = StorageFolder.GetFolderFromPathAsync(directoryPath).Await();
                directory.GetFileAsync(fileName).Await();
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                MvxTrace.Trace("Exception seen in Exists - seen for path: {0} - {1}", path, ex.ToLongString());
                throw;
            }
        }

        public override bool FolderExists(string folderPath)
        {
            // contributed by @AlexMortola via Stackoverflow creative commons
            // http://stackoverflow.com/questions/19890756/mvvmcross-notimplementedexception-calling-ensurefolderexists-method-of-imvxfile
            try
            {
                folderPath = ToFullPath(folderPath);
                folderPath = folderPath.TrimEnd('\\');

                var thisFolder = StorageFolder.GetFolderFromPathAsync(folderPath).Await();
                return true;
            }
            catch (System.UnauthorizedAccessException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                MvxTrace.Trace("Exception in FolderExists - folderPath: {0} - {1}", folderPath, ex.ToLongString());
                throw;
            }
        }

        public override void EnsureFolderExists(string folderPath)
        {
          if (FolderExists(folderPath))
            return;

          var rootFolder = ToFullPath(string.Empty);
          var storageFolder = StorageFolder.GetFolderFromPathAsync(rootFolder).Await();
          CreateFolderAsync(storageFolder, folderPath).GetAwaiter().GetResult();
        }

        private static async Task<StorageFolder> CreateFolderAsync(StorageFolder rootFolder, string folderPath)
        {
          if (string.IsNullOrEmpty(folderPath))
            return rootFolder;
          var currentFolder = await CreateFolderAsync(rootFolder, Path.GetDirectoryName(folderPath)).ConfigureAwait(false);
          return await currentFolder.CreateFolderAsync(Path.GetFileName(folderPath), CreationCollisionOption.OpenIfExists).AsTask().ConfigureAwait(false);
        }

        public override IEnumerable<string> GetFilesIn(string folderPath)
        {
            var folder = StorageFolder.GetFolderFromPathAsync(ToFullPath(folderPath)).Await();
            var files = folder.GetFilesAsync().Await();
            return files.Select(x => x.Name);
        }

        public override IEnumerable<string> GetFoldersIn(string folderPath)
        {
            var folder = StorageFolder.GetFolderFromPathAsync(ToFullPath(folderPath)).Await();
            var files = folder.GetFoldersAsync().Await();
            return files.Select(x => x.Name);
        }

        public override void DeleteFile(string path)
        {
            SafeDeleteFile(path);
        }

        public override void DeleteFolder(string folderPath, bool recursive)
        {
            // contributed by @AlexMortola via Stackoverflow creative commons
            // http://stackoverflow.com/questions/19890756/mvvmcross-notimplementedexception-calling-ensurefolderexists-method-of-imvxfile
            try
            {
                var directory = ToFullPath(folderPath);
                var storageFolder = StorageFolder.GetFolderFromPathAsync(directory).Await();
                storageFolder.DeleteAsync().Await();
            }
            catch (FileNotFoundException)
            {
                //Folder doesn't exist. Nothing to do
            }
            catch (Exception ex)
            {
                MvxTrace.Trace("Exception in DeleteFolder - folderPath: {0} - {1}", folderPath, ex.ToLongString());
                throw ex;
            }
        }

        protected override void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            // from https://github.com/MvvmCross/MvvmCross/issues/500 we delete any existing file
            // before writing the new one
            SafeDeleteFile(path);

            try
            {
                var storageFile = CreateStorageFileFromRelativePathAsync(path).GetAwaiter().GetResult();
                using (var streamWithContentType = storageFile.OpenAsync(FileAccessMode.ReadWrite).Await())
                {
                    using (var stream = streamWithContentType.AsStreamForWrite())
                    {
                        streamAction(stream);
                    }
                }
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file save {0} : {1}", path, exception.ToLongString());
                throw;
            }
        }

        protected override async Task WriteFileCommonAsync(string path, Func<Stream, Task> streamAction)
        {
            // from https://github.com/MvvmCross/MvvmCross/issues/500 we delete any existing file
            // before writing the new one
            await SafeDeleteFileAsync(path);

            try
            {
                var storageFile = await CreateStorageFileFromRelativePathAsync(path).ConfigureAwait(false);
                using (var streamWithContentType =
                        await storageFile.OpenAsync(FileAccessMode.ReadWrite).AsTask().ConfigureAwait(false))
                {
                    using (var stream = streamWithContentType.AsStreamForWrite())
                    {
                        await streamAction(stream).ConfigureAwait(false);
                    }
                }
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
                var storageFile = StorageFileFromRelativePath(path);
                using (var streamWithContentType = storageFile.OpenReadAsync().Await())
                {
                    using (var stream = streamWithContentType.AsStreamForRead())
                    {
                        return streamAction(stream);
                    }
                }
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file load {0} : {1}", path, exception.ToLongString());
                return false;
            }
        }

        protected override async Task<bool> TryReadFileCommonAsync(string path, Func<Stream, Task<bool>> streamAction)
        {
            try
            {
                var storageFile = await StorageFileFromRelativePathAsync(path).ConfigureAwait(false);
                using (var streamWithContentType = await storageFile.OpenReadAsync().AsTask().ConfigureAwait(false))
                {
                    using (var stream = streamWithContentType.AsStreamForRead())
                    {
                        return await streamAction(stream).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file load {0} : {1}", path, exception.ToLongString());
                return false;
            }
        }

        private static bool SafeDeleteFile(string path)
        {
            try
            {
                var toFile = StorageFileFromRelativePath(path);
                toFile.DeleteAsync().Await();
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

        private static async Task<bool> SafeDeleteFileAsync(string path)
        {
            try
            {
                var toFile = await StorageFileFromRelativePathAsync(path).ConfigureAwait(false);
                await toFile.DeleteAsync().AsTask().ConfigureAwait(false);
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

        private static StorageFile StorageFileFromRelativePath(string path)
        {
            var fullPath = ToFullPath(path);
            var storageFile = StorageFile.GetFileFromPathAsync(fullPath).Await();
            return storageFile;
        }

        private static async Task<StorageFile> StorageFileFromRelativePathAsync(string path)
        {
            var fullPath = ToFullPath(path);
            var storageFile = await StorageFile.GetFileFromPathAsync(fullPath).AsTask().ConfigureAwait(false);
            return storageFile;
        }

        private static async Task<StorageFile> CreateStorageFileFromRelativePathAsync(string path)
        {
            var fullPath = ToFullPath(path);
            var directory = Path.GetDirectoryName(fullPath);
            var fileName = Path.GetFileName(fullPath);
            var storageFolder = await StorageFolder.GetFolderFromPathAsync(directory).AsTask().ConfigureAwait(false);
            var storageFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting).AsTask().ConfigureAwait(false);
            return storageFile;
        }

        public override string NativePath(string path)
        {
            return ToFullPath(path);
        }

        private static string ToFullPath(string path)
        {
            var localFolderPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            return System.IO.Path.Combine(localFolderPath, path);
        }
    }
}