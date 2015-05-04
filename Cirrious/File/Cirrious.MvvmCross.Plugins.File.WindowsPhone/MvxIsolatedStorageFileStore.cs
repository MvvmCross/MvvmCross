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
using System.Threading.Tasks;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Plugins.File.WindowsPhone
{
    public class MvxIsolatedStorageFileStore
        : IMvxFileStore, IMvxFileStoreAsync
    {
        #region IMvxFileStoreAsync

        public async Task<TryResult<string>> TryReadTextFileAsync(string path)
        {
            var content = "";
            var operationSucceeded = await TryReadFileCommonAsync(path, async stream =>
            {
                using (var reader = new StreamReader(stream))
                {
                    content = await reader.ReadToEndAsync().ConfigureAwait(false);
                    return true;
                };
            }).ConfigureAwait(false);
            return TryResult.Create(operationSucceeded, content);
        }

        public async Task<TryResult<byte[]>> TryReadBinaryFileAsync(string path)
        {
            byte[] content = null;
            var operationSucceeded = await TryReadFileCommonAsync(path, async stream =>
            {
                using (var ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms).ConfigureAwait(false);
                    content = ms.ToArray();
                    return true;
                }
            }).ConfigureAwait(false);
            return TryResult.Create(operationSucceeded, content);
        }

        public async Task<bool> TryReadBinaryFileAsync(string path, Func<Stream, Task<bool>> readMethod)
        {
            return await TryReadFileCommonAsync(path, async stream =>
            {
                // TODO: check this works - ms is redundant
                using (var ms = new MemoryStream())
                {
                    return await readMethod(stream).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        public async Task WriteFileAsync(string path, string contents)
        {
            await WriteFileCommonAsync(path, async stream =>
            {
                using (var writer = new StreamWriter(stream))
                {
                    await writer.WriteAsync(contents).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        public async Task WriteFileAsync(string path, byte[] contents)
        {
            await WriteFileCommonAsync(path, async stream =>
            {
                using (var ms = new MemoryStream(contents))
                {
                    await ms.CopyToAsync(stream).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        public async Task WriteFileAsync(string path, IEnumerable<byte> contents)
        {
            await WriteFileAsync(path, contents.ToArray()).ConfigureAwait(false);
        }

        public async Task WriteFileAsync(string path, Func<Stream, Task> writeMethod)
        {
            await WriteFileCommonAsync(path, async stream =>
            {
                await writeMethod(stream).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        #endregion

        #region IMvxFileStore

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
            {
                DeleteFolderRecursive(folderPath);
            }
            else
            {
                DeleteFolderNonRecursive(folderPath);
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

        #endregion

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

        private async Task WriteFileCommonAsync(string path, Func<Stream, Task> streamAction)
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

        private async Task<bool> TryReadFileCommonAsync(string path, Func<Stream, Task<bool>> streamAction)
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