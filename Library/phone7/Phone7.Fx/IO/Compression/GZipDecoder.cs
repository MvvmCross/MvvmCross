using System.Diagnostics;

namespace Phone7.Fx.IO.Compression
{
    internal class GZipDecoder
    {
        const int FileText = 1;
        const int CRCFlag = 2;
        const int ExtraFieldsFlag = 4;
        const int FileNameFlag = 8;
        const int CommentFlag = 16;

        private InputBuffer input;
        GZIPHeaderState gzipHeaderSubstate;
        GZIPHeaderState gzipFooterSubstate;

        int gzip_header_flag;
        int gzip_header_xlen;
        uint gzipCrc32;
        uint gzipOutputStreamSize;
        int loopCounter;

        public GZipDecoder(InputBuffer input)
        {
            this.input = input;
            Reset();
        }

        public void Reset()
        {
            gzipHeaderSubstate = GZIPHeaderState.ReadingID1;
            gzipFooterSubstate = GZIPHeaderState.ReadingCRC;
            gzipCrc32 = 0;
            gzipOutputStreamSize = 0;
        }

        public uint Crc32
        {
            get
            {
                return gzipCrc32;
            }
        }

        public uint StreamSize
        {
            get
            {
                return gzipOutputStreamSize;
            }
        }


        public bool ReadGzipHeader()
        {

            while (true)
            {
                int bits;
                switch (gzipHeaderSubstate)
                {
                    case GZIPHeaderState.ReadingID1:
                        bits = input.GetBits(8);
                        if (bits < 0)
                        {
                            return false;
                        }

                        if (bits != 0x1F)
                        {
                            throw new InvalidDataException("SR.CorruptedGZipHeader");
                        }
                        gzipHeaderSubstate = GZIPHeaderState.ReadingID2;
                        goto case GZIPHeaderState.ReadingID2;

                    case GZIPHeaderState.ReadingID2:
                        bits = input.GetBits(8);
                        if (bits < 0)
                        {
                            return false;
                        }

                        if (bits != 0x8b)
                        {
                            throw new InvalidDataException("SR.CorruptedGZipHeader");
                        }

                        gzipHeaderSubstate = GZIPHeaderState.ReadingCM;
                        goto case GZIPHeaderState.ReadingCM;

                    case GZIPHeaderState.ReadingCM:
                        bits = input.GetBits(8);
                        if (bits < 0)
                        {
                            return false;
                        }

                        if (bits != 0x8)
                        {         // compression mode must be 8 (deflate)
                            throw new InvalidDataException("SR.UnknownCompressionMode");
                        }

                        gzipHeaderSubstate = GZIPHeaderState.ReadingFLG; ;
                        goto case GZIPHeaderState.ReadingFLG;

                    case GZIPHeaderState.ReadingFLG:
                        bits = input.GetBits(8);
                        if (bits < 0)
                        {
                            return false;
                        }

                        gzip_header_flag = bits;
                        gzipHeaderSubstate = GZIPHeaderState.ReadingMMTime;
                        loopCounter = 0; // 4 MMTIME bytes 
                        goto case GZIPHeaderState.ReadingMMTime;

                    case GZIPHeaderState.ReadingMMTime:
                        bits = 0;
                        while (loopCounter < 4)
                        {
                            bits = input.GetBits(8);
                            if (bits < 0)
                            {
                                return false;
                            }

                            loopCounter++;
                        }

                        gzipHeaderSubstate = GZIPHeaderState.ReadingXFL;
                        loopCounter = 0;
                        goto case GZIPHeaderState.ReadingXFL;

                    case GZIPHeaderState.ReadingXFL:      // ignore XFL 
                        bits = input.GetBits(8);
                        if (bits < 0)
                        {
                            return false;
                        }

                        gzipHeaderSubstate = GZIPHeaderState.ReadingOS;
                        goto case GZIPHeaderState.ReadingOS;

                    case GZIPHeaderState.ReadingOS:      // ignore OS 
                        bits = input.GetBits(8);
                        if (bits < 0)
                        {
                            return false;
                        }

                        gzipHeaderSubstate = GZIPHeaderState.ReadingXLen1;
                        goto case GZIPHeaderState.ReadingXLen1;

                    case GZIPHeaderState.ReadingXLen1:
                        if ((gzip_header_flag & ExtraFieldsFlag) == 0)
                        {
                            goto case GZIPHeaderState.ReadingFileName;
                        }

                        bits = input.GetBits(8);
                        if (bits < 0)
                        {
                            return false;
                        }

                        gzip_header_xlen = bits;
                        gzipHeaderSubstate = GZIPHeaderState.ReadingXLen2;
                        goto case GZIPHeaderState.ReadingXLen2;

                    case GZIPHeaderState.ReadingXLen2:
                        bits = input.GetBits(8);
                        if (bits < 0)
                        {
                            return false;
                        }

                        gzip_header_xlen |= (bits << 8);
                        gzipHeaderSubstate = GZIPHeaderState.ReadingXLenData;
                        loopCounter = 0; // 0 bytes of XLEN data read so far
                        goto case GZIPHeaderState.ReadingXLenData;

                    case GZIPHeaderState.ReadingXLenData:
                        bits = 0;
                        while (loopCounter < gzip_header_xlen)
                        {
                            bits = input.GetBits(8);
                            if (bits < 0)
                            {
                                return false;
                            }

                            loopCounter++;
                        }
                        gzipHeaderSubstate = GZIPHeaderState.ReadingFileName;
                        loopCounter = 0;
                        goto case GZIPHeaderState.ReadingFileName;

                    case GZIPHeaderState.ReadingFileName:
                        if ((gzip_header_flag & FileNameFlag) == 0)
                        {
                            gzipHeaderSubstate = GZIPHeaderState.ReadingComment;
                            goto case GZIPHeaderState.ReadingComment;
                        }

                        do
                        {
                            bits = input.GetBits(8);
                            if (bits < 0)
                            {
                                return false;
                            }

                            if (bits == 0)
                            {   // see '\0' in the file name string 
                                break;
                            }
                        } while (true);

                        gzipHeaderSubstate = GZIPHeaderState.ReadingComment;
                        goto case GZIPHeaderState.ReadingComment;

                    case GZIPHeaderState.ReadingComment:
                        if ((gzip_header_flag & CommentFlag) == 0)
                        {
                            gzipHeaderSubstate = GZIPHeaderState.ReadingCRC16Part1;
                            goto case GZIPHeaderState.ReadingCRC16Part1;
                        }

                        do
                        {
                            bits = input.GetBits(8);
                            if (bits < 0)
                            {
                                return false;
                            }

                            if (bits == 0)
                            {   // see '\0' in the file name string 
                                break;
                            }
                        } while (true);

                        gzipHeaderSubstate = GZIPHeaderState.ReadingCRC16Part1;
                        goto case GZIPHeaderState.ReadingCRC16Part1;

                    case GZIPHeaderState.ReadingCRC16Part1:
                        if ((gzip_header_flag & CRCFlag) == 0)
                        {
                            gzipHeaderSubstate = GZIPHeaderState.Done;
                            goto case GZIPHeaderState.Done;
                        }

                        bits = input.GetBits(8);     // ignore crc
                        if (bits < 0)
                        {
                            return false;
                        }

                        gzipHeaderSubstate = GZIPHeaderState.ReadingCRC16Part2;
                        goto case GZIPHeaderState.ReadingCRC16Part2;

                    case GZIPHeaderState.ReadingCRC16Part2:
                        bits = input.GetBits(8);     // ignore crc
                        if (bits < 0)
                        {
                            return false;
                        }

                        gzipHeaderSubstate = GZIPHeaderState.Done;
                        goto case GZIPHeaderState.Done;

                    case GZIPHeaderState.Done:
                        return true;
                    default:
                        Debug.Assert(false, "We should not reach unknown state!");
                        throw new InvalidDataException("SR.UnknownState");
                }
            }
        }

        public bool ReadGzipFooter()
        {
            input.SkipToByteBoundary();
            if (gzipFooterSubstate == GZIPHeaderState.ReadingCRC)
            {
                while (loopCounter < 4)
                {
                    int bits = input.GetBits(8);
                    if (bits < 0)
                    {
                        return false;
                    }

                    gzipCrc32 |= ((uint)bits << (8 * loopCounter));
                    loopCounter++;
                }
                gzipFooterSubstate = GZIPHeaderState.ReadingFileSize;
                loopCounter = 0;

            }

            if (gzipFooterSubstate == GZIPHeaderState.ReadingFileSize)
            {
                if (loopCounter == 0)
                    gzipOutputStreamSize = 0;

                while (loopCounter < 4)
                {
                    int bits = input.GetBits(8);
                    if (bits < 0)
                    {
                        return false;
                    }

                    gzipOutputStreamSize |= ((uint)bits << (8 * loopCounter));
                    loopCounter++;
                }
            }

            return true;
        }
    }
}