namespace Phone7.Fx.Security.Cryptography
{
    //Copyright (c) Microsoft Corporation.  All rights reserved.
    using System;
    using System.Security.Cryptography;

    // **************************************************************
    // * Raw implementation of the MD5 hash algorithm
    // * from RFC 1321.
    // *
    // * Written By: Reid Borsuk and Jenny Zheng
    // * Copyright (c) Microsoft Corporation.  All rights reserved.
    // **************************************************************

    public class MD5Managed : HashAlgorithm
    {
        private byte[] _data;
        private ABCDStruct _abcd;
        private Int64 _totalLength;
        private int _dataSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="MD5Managed"/> class.
        /// </summary>
        public MD5Managed()
        {
            base.HashSizeValue = 0x80;
            this.Initialize();
        }

        /// <summary>
        /// Initializes an implementation of the <see cref="T:System.Security.Cryptography.HashAlgorithm"/> class.
        /// </summary>
        public override sealed void Initialize()
        {
            _data = new byte[64];
            _dataSize = 0;
            _totalLength = 0;
            _abcd = new ABCDStruct {A = 0x67452301, B = 0xefcdab89, C = 0x98badcfe, D = 0x10325476};
            //Intitial values as defined in RFC 1321
        }

        /// <summary>
        /// When overridden in a derived class, routes data written to the object into the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            int startIndex = ibStart;
            int totalArrayLength = _dataSize + cbSize;
            if (totalArrayLength >= 64)
            {
                Array.Copy(array, startIndex, _data, _dataSize, 64 - _dataSize);
                // Process message of 64 bytes (512 bits)
                MD5Core.GetHashBlock(_data, ref _abcd, 0);
                startIndex += 64 - _dataSize;
                totalArrayLength -= 64;
                while (totalArrayLength >= 64)
                {
                    Array.Copy(array, startIndex, _data, 0, 64);
                    MD5Core.GetHashBlock(array, ref _abcd, startIndex);
                    totalArrayLength -= 64;
                    startIndex += 64;
                }
                _dataSize = totalArrayLength;
                Array.Copy(array, startIndex, _data, 0, totalArrayLength);
            }
            else
            {
                Array.Copy(array, startIndex, _data, _dataSize, cbSize);
                _dataSize = totalArrayLength;
            }
            _totalLength += cbSize;
        }

        /// <summary>
        /// When overridden in a derived class, finalizes the hash computation after the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        protected override byte[] HashFinal()
        {
            base.HashValue = MD5Core.GetHashFinalBlock(_data, 0, _dataSize, _abcd, _totalLength * 8);
            return base.HashValue;
        }
    }
}