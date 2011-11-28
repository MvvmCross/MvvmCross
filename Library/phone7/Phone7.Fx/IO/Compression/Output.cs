#region

using System;
using System.Diagnostics;

#endregion

namespace Phone7.Fx.IO.Compression
{
    internal class Output
    {
        private static readonly byte[] DistLookup;
        private uint _bitBuf; // store uncomplete bits
        private int _bitCount; // number of bits in bitBuffer
        private byte[] _outputBuf; // output buffer 
        private int _outputPos; // output position

        //static private byte[] lengthLookup;

        static Output()
        {
            //lengthLookup = new byte[512]; 
            DistLookup = new byte[512];

            GenerateSlotTables();
        }

        internal int BytesWritten
        {
            get { return _outputPos; }
        }

        internal int FreeBytes
        {
            get { return _outputBuf.Length - _outputPos; }
        }

        // Generate the global slot tables which allow us to convert a distance 
        // (0..32K) to a distance slot (0..29) 
        //
        // Distance table 
        //   Extra           Extra               Extra
        // Code Bits Dist  Code Bits   Dist     Code Bits Distance
        // ---- ---- ----  ---- ----  ------    ---- ---- --------
        //   0   0    1     10   4     33-48    20    9   1025-1536 
        //   1   0    2     11   4     49-64    21    9   1537-2048
        //   2   0    3     12   5     65-96    22   10   2049-3072 
        //   3   0    4     13   5     97-128   23   10   3073-4096 
        //   4   1   5,6    14   6    129-192   24   11   4097-6144
        //   5   1   7,8    15   6    193-256   25   11   6145-8192 
        //   6   2   9-12   16   7    257-384   26   12  8193-12288
        //   7   2  13-16   17   7    385-512   27   12 12289-16384
        //   8   3  17-24   18   8    513-768   28   13 16385-24576
        //   9   3  25-32   19   8   769-1024   29   13 24577-32768 

        internal static void GenerateSlotTables()
        {
            // Initialize the mapping length (0..255) -> length code (0..28) 
            //int length = 0;
            //for (code = 0; code < FastEncoderStatics.NumLengthBaseCodes-1; code++) { 
            //    for (int n = 0; n < (1 << FastEncoderStatics.ExtraLengthBits[code]); n++)
            //        lengthLookup[length++] = (byte) code;
            //}
            //lengthLookup[length-1] = (byte) code; 

            // Initialize the mapping dist (0..32K) -> dist code (0..29) 
            int dist = 0;
            int code;
            for (code = 0; code < 16; code++)
            {
                for (int n = 0; n < (1 << FastEncoderStatics.ExtraDistanceBits[code]); n++)
                    DistLookup[dist++] = (byte) code;
            }

            dist >>= 7; // from now on, all distances are divided by 128

            for (; code < FastEncoderStatics.NumDistBaseCodes; code++)
            {
                for (int n = 0; n < (1 << (FastEncoderStatics.ExtraDistanceBits[code] - 7)); n++)
                    DistLookup[256 + dist++] = (byte) code;
            }
        }

        // set the output buffer we will be using 
        internal void UpdateBuffer(byte[] output)
        {
            _outputBuf = output;
            _outputPos = 0;
        }

        internal bool SafeToWriteTo()
        {
            // can we safely continue writing to output buffer
            return _outputBuf.Length - _outputPos > 16;
        }

        // Output the block type and tree structure for our hard-coded trees.
        // Contains following data: 
        //  "final" block flag 1 bit
        //  BLOCKTYPE_DYNAMIC 2 bits
        //  FastEncoderLiteralTreeLength
        //  FastEncoderDistanceTreeLength 
        //
        internal void WritePreamble()
        {
            Debug.Assert(_bitCount == 0, "bitCount must be zero before writing tree bit!");
            Debug.Assert(FreeBytes >= FastEncoderStatics.FastEncoderTreeStructureData.Length,
                         "Not enough space in output buffer!");
            Array.Copy(FastEncoderStatics.FastEncoderTreeStructureData, 0, _outputBuf, _outputPos,
                       FastEncoderStatics.FastEncoderTreeStructureData.Length);
            _outputPos += FastEncoderStatics.FastEncoderTreeStructureData.Length;

            const uint FastEncoderPostTreeBitBuf = 0x0022;
            const int FastEncoderPostTreeBitCount = 9;
            _bitCount = FastEncoderPostTreeBitCount;
            _bitBuf = FastEncoderPostTreeBitBuf;
        }

        internal void WriteMatch(int matchLen, int matchPos)
        {
            Debug.Assert(matchLen >= FastEncoderWindow.MinMatch && matchLen <= FastEncoderWindow.MaxMatch,
                         "Illegal currentMatch length!");


            // Get the code information for a match code 
            uint codeInfo =
                FastEncoderStatics.FastEncoderLiteralCodeInfo[
                    (FastEncoderStatics.NumChars + 1 - FastEncoderWindow.MinMatch) + matchLen];
            int codeLen = (int) codeInfo & 31;
            Debug.Assert(codeLen != 0, "Invalid Match Length!");
            if (codeLen <= 16)
            {
                WriteBits(codeLen, codeInfo >> 5);
            }
            else
            {
                WriteBits(16, (codeInfo >> 5) & 65535);
                WriteBits(codeLen - 16, codeInfo >> (5 + 16));
            }

            // Get the code information for a distance code 
            codeInfo = FastEncoderStatics.FastEncoderDistanceCodeInfo[GetSlot(matchPos)];
            WriteBits((int) (codeInfo & 15), codeInfo >> 8);
            int extraBits = (int) (codeInfo >> 4) & 15;
            if (extraBits != 0)
            {
                WriteBits(extraBits, (uint) matchPos & FastEncoderStatics.BitMask[extraBits]);
            }
        }

        // write gzip footer 
        internal void WriteGzipFooter(uint gzipCrc32, uint inputStreamSize)
        {
            Debug.Assert(FreeBytes >= 8, "No enough space in output buffer!");
            _outputBuf[_outputPos++] = (byte) (gzipCrc32 & 255);
            _outputBuf[_outputPos++] = (byte) ((gzipCrc32 >> 8) & 255);
            _outputBuf[_outputPos++] = (byte) ((gzipCrc32 >> 16) & 255);
            _outputBuf[_outputPos++] = (byte) ((gzipCrc32 >> 24) & 255);

            _outputBuf[_outputPos++] = (byte) (inputStreamSize & 255);
            _outputBuf[_outputPos++] = (byte) ((inputStreamSize >> 8) & 255);
            _outputBuf[_outputPos++] = (byte) ((inputStreamSize >> 16) & 255);
            _outputBuf[_outputPos++] = (byte) ((inputStreamSize >> 24) & 255);
        }

        // write gzip header
        internal void WriteGzipHeader(int compression_level)
        {
            // only need 11 bytes 
            Debug.Assert(FreeBytes >= 16, "No enough space in output buffer!");
            Debug.Assert(_outputPos == 0, "GZIP header must be at the begining of output!");
            _outputBuf[_outputPos++] = 0x1F; // ID1 
            _outputBuf[_outputPos++] = 0x8B; // ID2
            _outputBuf[_outputPos++] = 8; // CM = deflate 
            _outputBuf[_outputPos++] = 0; // FLG, no text, no crc, no extra, no name, no comment

            _outputBuf[_outputPos++] = 0; // MTIME (Modification Time) - no time available
            _outputBuf[_outputPos++] = 0;
            _outputBuf[_outputPos++] = 0;
            _outputBuf[_outputPos++] = 0;

            // XFL
            // 2 = compressor used max compression, slowest algorithm 
            // 4 = compressor used fastest algorithm
            if (compression_level == 10)
                _outputBuf[_outputPos++] = 2;
            else
                _outputBuf[_outputPos++] = 4;

            _outputBuf[_outputPos++] = 0; // OS: 0 = FAT filesystem (MS-DOS, OS/2, NT/Win32) 
        }

        internal void WriteChar(byte b)
        {
            uint code = FastEncoderStatics.FastEncoderLiteralCodeInfo[b];
            WriteBits((int) code & 31, code >> 5);
        }

        internal void WriteBits(int n, uint bits)
        {
            Debug.Assert(n <= 16, "length must be larger than 16!");
            _bitBuf |= bits << _bitCount;
            _bitCount += n;
            if (_bitCount >= 16)
            {
                Debug.Assert(_outputBuf.Length - _outputPos >= 2, "No enough space in output buffer!");
                _outputBuf[_outputPos++] = unchecked((byte) _bitBuf);
                _outputBuf[_outputPos++] = unchecked((byte) (_bitBuf >> 8));
                _bitCount -= 16;
                _bitBuf >>= 16;
            }
        }

        // Return the position slot (0...29) of a match offset (0...32767)
        internal int GetSlot(int pos)
        {
            return DistLookup[((pos) < 256) ? (pos) : (256 + ((pos) >> 7))];
        }

        // write the bits left in the output as bytes 
        internal void FlushBits()
        {
            // flush bits from bit buffer to output buffer 
            while (_bitCount >= 8)
            {
                _outputBuf[_outputPos++] = unchecked((byte) _bitBuf);
                _bitCount -= 8;
                _bitBuf >>= 8;
            }

            if (_bitCount > 0)
            {
                _outputBuf[_outputPos++] = unchecked((byte) _bitBuf);
                _bitCount = 0;
            }
        }
    }
}