namespace Phone7.Fx.IO.Compression
{
    internal enum GZIPHeaderState
    {
        // GZIP header 
        ReadingID1,
        ReadingID2,
        ReadingCM,
        ReadingFLG,
        ReadingMMTime, // iterates 4 times
        ReadingXFL,
        ReadingOS,
        ReadingXLen1,
        ReadingXLen2,
        ReadingXLenData,
        ReadingFileName,
        ReadingComment,
        ReadingCRC16Part1,
        ReadingCRC16Part2,
        Done, // done reading GZIP header

        // GZIP footer
        ReadingCRC, // iterates 4 times 
        ReadingFileSize // iterates 4 times 
    }
}