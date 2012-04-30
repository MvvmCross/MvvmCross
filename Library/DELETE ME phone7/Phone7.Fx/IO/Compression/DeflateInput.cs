using System.Diagnostics;

namespace Phone7.Fx.IO.Compression
{
    internal class DeflateInput
    {
        internal byte[] Buffer { get; set; }

        internal int Count { get; set; }

        internal int StartIndex { get; set; }

        internal void ConsumeBytes(int n)
        {
            Debug.Assert(n <= Count, "Should use more bytes than what we have in the buffer");
            StartIndex += n;
            Count -= n;
            Debug.Assert(StartIndex + Count <= Buffer.Length, "Input buffer is in invalid state!");
        }
    }
}