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
using System.Linq;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using System.Threading.Tasks;

namespace Cirrious.MvvmCross.Plugins.File
{
    public abstract class MvxFileStore
		: IMvxFileStore, IMvxFileStoreAsync
    {

        #region IMvxFileStore Members
        
        public Stream OpenRead(string path)
        {
            var fullPath = FullPath(path);
            if (!System.IO.File.Exists(fullPath))
            {
                return null;
            }

            return System.IO.File.OpenRead(fullPath);
        }

        public Stream OpenWrite(string path)
        {
            var fullPath = FullPath(path);

            if (!System.IO.File.Exists(fullPath))
            {
                return System.IO.File.Create(fullPath);
            }

            return System.IO.File.OpenWrite(fullPath);
        }

        public bool Exists(string path)
        {
            var fullPath = FullPath(path);
            return System.IO.File.Exists(fullPath);
        }

        public bool FolderExists(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            return Directory.Exists(fullPath);
        }

        public string PathCombine(string items0, string items1)
        {
            return Path.Combine(items0, items1);
        }

        public void EnsureFolderExists(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
        }

        public IEnumerable<string> GetFilesIn(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            return Directory.GetFiles(fullPath);
        }

        public IEnumerable<string> GetFoldersIn(string folderPath)
        {
            var fullPath = FullPath(folderPath);
            return Directory.GetDirectories(fullPath);
        }

        public void DeleteFile(string filePath)
        {
            var fullPath = FullPath(filePath);
            System.IO.File.Delete(fullPath);
        }

        public void DeleteFolder(string folderPath, bool recursive)
        {
            var fullPath = FullPath(folderPath);
            Directory.Delete(fullPath, recursive);
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
                        if (binaryReader.Read(memoryBuffer, 0,
                                              memoryBuffer.Length) !=
                            memoryBuffer.Length)
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
            return TryReadFileCommon(path, readMethod);
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

        public string NativePath(string path)
        {
            return FullPath(path);
        }

        #endregion

		#region IMvxFileStore Async
		public async Task<TryResult<string>> TryReadTextFileAsync (string path)
		{
			string content = "";
			bool operationSucceeded = await TryReadFileCommonAsync (path, async stream => {
				using (var reader = new StreamReader(stream)) {
					content = await reader.ReadToEndAsync().ConfigureAwait(false);
					return true;
				};
			}).ConfigureAwait (false);
			return TryResult.Create (operationSucceeded, content);
		}

		public async Task<TryResult<byte[]>> TryReadBinaryFileAsync (string path)
		{
			byte[] content = null;
			bool operationSucceeded =  await TryReadFileCommonAsync (path, async stream => { 
				using (var ms = new MemoryStream ()) {
					await stream.CopyToAsync (ms).ConfigureAwait (false);
					content = ms.ToArray ();
					return true;
				}
			}).ConfigureAwait (false);
			return TryResult.Create (operationSucceeded, content);
		}

		public async Task<bool> TryReadBinaryFileAsync (string path, Func<Stream, Task<bool>> readMethod)
		{
			return await TryReadFileCommonAsync (path, async stream => { 
				using (var ms = new MemoryStream ()) {
					return await readMethod(stream).ConfigureAwait(false);
				}
			}).ConfigureAwait (false);
		}

		public async Task WriteFileAsync (string path, string contents)
		{
			await WriteFileCommonAsync (path, async stream => {
				using (var writer = new StreamWriter(stream)) {
					await writer.WriteAsync(contents).ConfigureAwait(false);
				}
			}).ConfigureAwait (false);
		}

		public async Task WriteFileAsync (string path, byte[] contents)
		{
			await WriteFileCommonAsync (path, async stream => {
				using (MemoryStream ms = new MemoryStream(contents)) {
					await ms.CopyToAsync(stream).ConfigureAwait(false);
				}
			}).ConfigureAwait (false);
		}

		public async Task WriteFileAsync (string path, IEnumerable<byte> contents)
		{
			await WriteFileAsync (path, contents.ToArray ()).ConfigureAwait (false);
		}

		public async Task WriteFileAsync (string path, Func<Stream, Task> writeMethod)
		{
			await WriteFileCommonAsync (path, async stream => {
				await writeMethod(stream).ConfigureAwait(false);
			}).ConfigureAwait (false);
		}
		#endregion

        protected abstract string FullPath(string path);

        private void WriteFileCommon(string path, Action<Stream> streamAction)
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

        private bool TryReadFileCommon(string path, Func<Stream, bool> streamAction)
        {
            var fullPath = FullPath(path);
            if (!System.IO.File.Exists(fullPath))
            {
                return false;
            }

            using (var fileStream = System.IO.File.OpenRead(fullPath))
            {
                return streamAction(fileStream);
            }
        }

		private async Task WriteFileCommonAsync(string path, Func<Stream, Task> streamAction)
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

		private async Task<bool> TryReadFileCommonAsync(string path, Func<Stream, Task<bool>> streamAction)
		{
			var fullPath = FullPath(path);
			if (!System.IO.File.Exists(fullPath))
			{
				return false;
			}

			using (var fileStream = System.IO.File.OpenRead(fullPath))
			{
				return await streamAction(fileStream).ConfigureAwait(false);
			}
		}
    }
}

// ReSharper restore all