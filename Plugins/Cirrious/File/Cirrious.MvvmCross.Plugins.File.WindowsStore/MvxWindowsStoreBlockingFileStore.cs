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
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.WindowsStore.Platform;
using Windows.Storage;

namespace Cirrious.MvvmCross.Plugins.File.WindowsStore
{
    // note that we use the full WindowsStore name here deliberately to avoid 'Store' naming confusion
    public class MvxWindowsStoreBlockingFileStore : IMvxFileStore
    {
        public Stream OpenRead(string path)
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

        public Stream OpenWrite(string path)
        {
            try
            {
                StorageFile storageFile;
                
                if (Exists(path))
                    storageFile = StorageFileFromRelativePath(path);
                else
                    storageFile = CreateStorageFileFromRelativePath(path);

                var streamWithContentType = storageFile.OpenAsync(FileAccessMode.ReadWrite).Await();
                return streamWithContentType.AsStream();
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file save {0} : {1}", path, exception.ToLongString());
                throw;
            }
        }

        public bool TryReadTextFile(string path, out string contents)
        {
            string result = null;
            var toReturn = TryReadFileCommon(path, (stream) =>
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    return true;
                });
            contents = result;
            return toReturn;
        }

        public bool TryReadBinaryFile(string path, out byte[] contents)
        {
            Byte[] result = null;
            var toReturn = TryReadFileCommon(path, (stream) =>
                {
                    using (var binaryReader = new BinaryReader(stream))
                    {
                        var memoryBuffer = new byte[stream.Length];
                        if (binaryReader.Read(memoryBuffer, 0, memoryBuffer.Length) != memoryBuffer.Length)
                            return false;

                        result = memoryBuffer;
                        return true;
                    }
                });
            contents = result;
            return toReturn;
        }

        public bool TryReadBinaryFile(string path, Func<System.IO.Stream, bool> readMethod)
        {
            var toReturn = TryReadFileCommon(path, readMethod);
            return toReturn;
        }

        public void WriteFile(string path, string contents)
        {
            WriteFileCommon(path, (stream) =>
                {
                    using (var sw = new StreamWriter(stream))
                    {
                        sw.Write(contents);
                        sw.Flush();
                    }
                });
        }

        public void WriteFile(string path, IEnumerable<byte> contents)
        {
            WriteFileCommon(path, (stream) =>
                {
                    using (var binaryWriter = new BinaryWriter(stream))
                    {
                        binaryWriter.Write(contents.ToArray());
                        binaryWriter.Flush();
                    }
                });
        }

        public void WriteFile(string path, Action<System.IO.Stream> writeMethod)
        {
            WriteFileCommon(path, writeMethod);
        }

        public bool TryMove(string from, string to, bool deleteExistingTo)
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

        public bool Exists(string path)
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

        public bool FolderExists(string folderPath)
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

        public string PathCombine(string items0, string items1)
        {
            return Path.Combine(items0, items1);
        }

        public void EnsureFolderExists(string folderPath)
        {
            // contributed by @AlexMortola via Stackoverflow creative commons
            // http://stackoverflow.com/questions/19890756/mvvmcross-notimplementedexception-calling-ensurefolderexists-method-of-imvxfile
            if (FolderExists(folderPath))
                return;

            // note that this does not work recursively
            if (folderPath.Contains("\\") || folderPath.Contains("/"))
                Mvx.Warning("WindowsStore EnsureFolderExists implementation can't yet cope with nested paths");

            var rootFolder = ToFullPath(string.Empty);
            var storageFolder = StorageFolder.GetFolderFromPathAsync(rootFolder).Await();
            storageFolder.CreateFolderAsync(folderPath).Await();
        }

        public IEnumerable<string> GetFilesIn(string folderPath)
        {
            var folder = StorageFolder.GetFolderFromPathAsync(ToFullPath(folderPath)).Await();
            var files = folder.GetFilesAsync().Await();
            return files.Select(x => x.Name);
        }

        public IEnumerable<string> GetFoldersIn(string folderPath)
        {
            var folder = StorageFolder.GetFolderFromPathAsync(ToFullPath(folderPath)).Await();
            var files = folder.GetFoldersAsync().Await();
            return files.Select(x => x.Name);
        }

        public void DeleteFile(string path)
        {
            SafeDeleteFile(path);
        }

        public void DeleteFolder(string folderPath, bool recursive)
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

        private void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            // from https://github.com/MvvmCross/MvvmCross/issues/500 we delete any existing file
            // before writing the new one
            SafeDeleteFile(path);

            try
            {
                var storageFile = CreateStorageFileFromRelativePath(path);
                var streamWithContentType = storageFile.OpenAsync(FileAccessMode.ReadWrite).Await();
                using (var stream = streamWithContentType.AsStreamForWrite())
                {
                    streamAction(stream);
                }
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file save {0} : {1}", path, exception.ToLongString());
                throw;
            }
        }

        private bool TryReadFileCommon(string path, Func<Stream, bool> streamAction)
        {
            try
            {
                var storageFile = StorageFileFromRelativePath(path);
                var streamWithContentType = storageFile.OpenReadAsync().Await();
                using (var stream = streamWithContentType.AsStreamForRead())
                {
                    return streamAction(stream);
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

        private static StorageFile StorageFileFromRelativePath(string path)
        {
            var fullPath = ToFullPath(path);
            var storageFile = StorageFile.GetFileFromPathAsync(fullPath).Await();
            return storageFile;
        }

        private static StorageFile CreateStorageFileFromRelativePath(string path)
        {
            var fullPath = ToFullPath(path);
            var directory = Path.GetDirectoryName(fullPath);
            var fileName = Path.GetFileName(fullPath);
            var storageFolder = StorageFolder.GetFolderFromPathAsync(directory).Await();
            var storageFile = storageFolder.CreateFileAsync(fileName).Await();
            return storageFile;
        }

        public string NativePath(string path)
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
