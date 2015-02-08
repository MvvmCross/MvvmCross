// MvxIsolatedStorageFileStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Plugins.File.WindowsPhone
{
    public class MvxIsolatedStorageFileStore
        : IMvxFileStore
    {
        public Stream OpenRead(string path)
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

        public Stream OpenWrite(string path)
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

        public bool Exists(string path)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return isf.FileExists(path);
            }
        }

        public bool FolderExists(string folderPath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return isf.DirectoryExists(folderPath);
            }
        }

        public string PathCombine(string items0, string items1)
        {
            return Path.Combine(items0, items1);
        }

        public void EnsureFolderExists(string folderPath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.DirectoryExists(folderPath))
                    return;
                isf.CreateDirectory(folderPath);
            }
        }

        public IEnumerable<string> GetFilesIn(string folderPath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                folderPath = folderPath.TrimEnd('/');
                var path = folderPath + "/*";
                return isf.GetFileNames(path).Select(x => folderPath + "/" + x).ToArray();
            }
        }


        public IEnumerable<string> GetFoldersIn(string folderPath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                folderPath = folderPath.TrimEnd('/');
                var path = folderPath + "/*";
                return isf.GetDirectoryNames(path).Select(x => folderPath + "/" + x).ToArray();
            }
        }

        public void DeleteFile(string path)
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

        public void DeleteFolder(string folderPath, bool recursive)
        {
            if (recursive)
                throw new NotImplementedException("WindowsPhone does not support recursive Directory Deletion");

            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                isf.DeleteDirectory(folderPath);
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

        public bool TryReadBinaryFile(string path, out Byte[] contents)
        {
            Byte[] result = null;
            var toReturn = TryReadFileCommon(path, (stream) =>
                {
                    using (var binaryReader = new BinaryReader(stream))
                    {
                        var memoryBuffer = new byte[stream.Length];
                        if (binaryReader.Read(memoryBuffer, 0, memoryBuffer.Length) != memoryBuffer.Length)
                            return false; // TODO - do more here?

                        result = memoryBuffer;
                        return true;
                    }
                });
            contents = result;
            return toReturn;
        }

        public bool TryReadBinaryFile(string path, Func<Stream, bool> readMethod)
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

        public void WriteFile(string path, IEnumerable<Byte> contents)
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

        public void WriteFile(string path, Action<Stream> writeMethod)
        {
            WriteFileCommon(path, writeMethod);
        }

        public bool TryMove(string from, string to, bool deleteExistingTo)
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isf.FileExists(from))
                        return false;

                    if (isf.FileExists(to))
                    {
                        if (deleteExistingTo)
                            isf.DeleteFile(to);
                        else
                            return false;
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

        public string NativePath(string path)
        {
            return path;
        }

        private static void WriteFileCommon(string path, Action<Stream> streamAction)
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

        private static bool TryReadFileCommon(string path, Func<Stream, bool> streamAction)
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
    }
}