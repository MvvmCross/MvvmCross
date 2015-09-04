// MvxFileStoreBase.cs
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvvmCross.Plugins.File
{
    public abstract class MvxFileStoreBase
		: IMvxFileStore, IMvxFileStoreAsync
    {
        const int BufferSize = 1024;

        #region IMvxFileStore Members

        public abstract Stream OpenRead(string path);

        public abstract Stream OpenWrite(string path);

        public abstract bool Exists(string path);

        public abstract bool FolderExists(string folderPath);

        public string PathCombine(string items0, string items1)
        {
            return Path.Combine(items0, items1);
        }

        public abstract void EnsureFolderExists(string folderPath);

        public abstract IEnumerable<string> GetFilesIn(string folderPath);

        public abstract IEnumerable<string> GetFoldersIn(string folderPath);

        public abstract void DeleteFile(string filePath);

        public abstract void DeleteFolder(string folderPath, bool recursive);

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

        public abstract bool TryMove(string from, string to, bool deleteExistingTo);

        public abstract string NativePath(string path);

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

        public async Task<TryResult<string>> TryReadTextFileAsync(string path, CancellationToken cancellationToken)
        {
            var contentStringBuilder = new StringBuilder();
            var operationSucceeded = await TryReadFileCommonAsync(path, async stream =>
            {
                using (var reader = new StreamReader(stream))
                {
                    var buffer = new char[BufferSize];

                    while (reader.Peek() > 0)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            return false;

                        var charsRead = await reader.ReadAsync(buffer, 0, BufferSize);

                        contentStringBuilder.Append(buffer, 0, charsRead);
                    }
                    return true;
                };
            }).ConfigureAwait(false);

            return TryResult.Create(operationSucceeded, operationSucceeded ? contentStringBuilder.ToString() : string.Empty);
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

        public async Task<TryResult<byte[]>> TryReadBinaryFileAsync(string path, CancellationToken cancellationToken)
        {
            byte[] content = null;
            var operationSucceeded = await TryReadFileCommonAsync(path, async stream =>
            {
                using (var ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms, BufferSize, cancellationToken).ConfigureAwait(false);

                    if (cancellationToken.IsCancellationRequested)
                        return false;

                    content = ms.ToArray();
                    return true;
                }
            }).ConfigureAwait(false);
            return TryResult.Create(operationSucceeded, content);
        }

		public async Task<bool> TryReadBinaryFileAsync (string path, Func<Stream, Task<bool>> readMethod)
		{
			return await TryReadFileCommonAsync (path, 
                async stream => await readMethod(stream).ConfigureAwait(false)).ConfigureAwait (false);
		}

        public async Task<bool> TryReadBinaryFileAsync(string path, 
            Func<Stream, CancellationToken, Task<bool>> readMethod, CancellationToken cancellationToken)
        {
            return await TryReadFileCommonAsync(
                path, async stream => await readMethod(stream, cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

		public async Task WriteFileAsync (string path, string contents)
		{
			await WriteFileCommonAsync (path, async stream => {
				using (var writer = new StreamWriter(stream)) {
					await writer.WriteAsync(contents).ConfigureAwait(false);
				}
			}).ConfigureAwait (false);
		}

        public async Task WriteFileAsync(string path, string contents, CancellationToken cancellationToken)
        {
            var contentsCharArray = contents.ToCharArray();
            await WriteFileCommonAsync(path, async stream =>
            {
                using (var writer = new StreamWriter(stream))
                {
                    var startIndex = 0;
                    while (startIndex < contentsCharArray.Length)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            return;

                        var charsToRead = Math.Min(BufferSize, contentsCharArray.Length - startIndex);
                        await writer.WriteAsync(contentsCharArray, 0, charsToRead).ConfigureAwait(false);
                        startIndex += BufferSize;
                    }
                }
            }).ConfigureAwait(false);
        }

		public async Task WriteFileAsync (string path, byte[] contents)
		{
			await WriteFileCommonAsync (path, async stream => {
				using (MemoryStream ms = new MemoryStream(contents)) {
					await ms.CopyToAsync(stream).ConfigureAwait(false);
				}
			}).ConfigureAwait (false);
		}

        public async Task WriteFileAsync(string path, byte[] contents, CancellationToken cancellationToken)
        {
            await WriteFileCommonAsync(path, async stream =>
            {
                using (var ms = new MemoryStream(contents))
                {
                    await ms.CopyToAsync(stream, BufferSize, cancellationToken).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

		public async Task WriteFileAsync (string path, IEnumerable<byte> contents)
		{
			await WriteFileAsync (path, contents.ToArray ()).ConfigureAwait (false);
		}

        public async Task WriteFileAsync(string path, IEnumerable<byte> contents, CancellationToken cancellationToken)
        {
            await WriteFileAsync(path, contents.ToArray(), cancellationToken).ConfigureAwait(false);
        }

		public async Task WriteFileAsync (string path, Func<Stream, Task> writeMethod)
		{
			await WriteFileCommonAsync (path, async stream => {
				await writeMethod(stream).ConfigureAwait(false);
			}).ConfigureAwait (false);
		}

        public async Task WriteFileAsync(string path, Func<Stream, CancellationToken, Task> writeMethod,
            CancellationToken cancellationToken)
        {
            await WriteFileCommonAsync(path, async stream =>
            {
                await writeMethod(stream, cancellationToken).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
		#endregion

        protected abstract void WriteFileCommon(string path, Action<Stream> streamAction);

        protected abstract bool TryReadFileCommon(string path, Func<Stream, bool> streamAction);

        protected abstract Task WriteFileCommonAsync(string path, Func<Stream, Task> streamAction);

        protected abstract Task<bool> TryReadFileCommonAsync(string path, Func<Stream, Task<bool>> streamAction);
    }
}

// ReSharper restore all
