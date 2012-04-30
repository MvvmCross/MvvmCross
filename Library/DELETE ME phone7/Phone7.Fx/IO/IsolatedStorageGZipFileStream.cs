using System;
using System.IO;
using System.IO.IsolatedStorage;
using Phone7.Fx.IO.Compression;

namespace Phone7.Fx.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class IsolatedStorageGZipFileStream : GZipStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageGZipFileStream"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="access">The access.</param>
        /// <param name="isf">The isf.</param>
        /// <param name="compressionMode">The compression mode.</param>
        public IsolatedStorageGZipFileStream(string path, FileMode mode, FileAccess access, IsolatedStorageFile isf, CompressionMode compressionMode)
            : base(new IsolatedStorageFileStream(path, mode, access, isf), compressionMode)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageGZipFileStream"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="access">The access.</param>
        /// <param name="share">The share.</param>
        /// <param name="isf">The isf.</param>
        /// <param name="compressionMode">The compression mode.</param>
        public IsolatedStorageGZipFileStream(string path, FileMode mode, FileAccess access, FileShare share,
                                         IsolatedStorageFile isf, CompressionMode compressionMode)
            : base(new IsolatedStorageFileStream(path, mode, access, share, isf), compressionMode)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageGZipFileStream"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="isf">The isf.</param>
        /// <param name="compressionMode">The compression mode.</param>
        public IsolatedStorageGZipFileStream(string path, FileMode mode, IsolatedStorageFile isf, CompressionMode compressionMode)
            : base(new IsolatedStorageFileStream(path, mode, isf), compressionMode)
        {
            
        }
    }
}