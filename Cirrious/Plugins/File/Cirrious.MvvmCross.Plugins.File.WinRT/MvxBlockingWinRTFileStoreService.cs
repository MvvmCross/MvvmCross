#warning Copyright needed

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Windows.Storage;

namespace Cirrious.MvvmCross.Plugins.File.WinRT
{
    public class MvxBlockingWinRTFileStoreService : IMvxSimpleFileStoreService
    {
        #region IMvxSimpleFileStoreService Members

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
            var fromFile = StorageFileFromRelativePath(from);
            var toFile = StorageFileFromRelativePath(to);

            fromFile.CopyAndReplaceAsync(toFile).Await();

#warning What should TryMove return? Consider renaming to a void?
            return true;
        }

        public bool Exists(string path)
        {
            try
            {
                var storageFile = StorageFileFromRelativePath(path);
                return storageFile != null;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        public string PathCombine(string items0, string items1)
        {
            return Path.Combine(items0, items1);
        }

        public void EnsureFolderExists(string folderPath)
        {
#warning TODO - not sure how to do this in WinRT?
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFilesIn(string folderPath)
        {
            var folder = StorageFolder.GetFolderFromPathAsync(ToFullPath(folderPath)).Await();
            var files = folder.GetFilesAsync().Await();
            return files.Select(x => x.Name);
        }

        public void DeleteFile(string path)
        {
            var file = StorageFileFromRelativePath(path);
            file.DeleteAsync().Await();
        }

        #endregion

        private static void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            try
            {
                var storageFile = CreateStorageFileFromRelativePath(path);
                var streamWithContentType = storageFile.OpenAsync(FileAccessMode.ReadWrite).Await();
                var stream = streamWithContentType.AsStreamForWrite();
                streamAction(stream);
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file save {0} : {1}", path, exception.ToLongString());
                throw;
            }
        }

        private static bool TryReadFileCommon(string path, Func<Stream, bool> streamAction)
        {
            try
            {
                var storageFile = StorageFileFromRelativePath(path);
                var streamWithContentType = storageFile.OpenReadAsync().Await();
                var stream = streamWithContentType.AsStreamForRead();
                return streamAction(stream);
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error during file load {0} : {1}", path, exception.ToLongString());
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

        private static string ToFullPath(string path)
        {
            var localFolderPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            return System.IO.Path.Combine(localFolderPath, path);
        }
    }
}
