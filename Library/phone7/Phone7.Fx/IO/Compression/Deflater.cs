using System.Diagnostics;

namespace Phone7.Fx.IO.Compression
{
    internal class Deflater
    {
        private readonly FastEncoder _encoder;

        public Deflater(bool doGZip)
        {
            _encoder = new FastEncoder(doGZip);
        }

        public void SetInput(byte[] input, int startIndex, int count)
        {
            _encoder.SetInput(input, startIndex, count);
        }

        public int GetDeflateOutput(byte[] output)
        {
            Debug.Assert(output != null, "Can't pass in a null output buffer!");
            return _encoder.GetCompressedOutput(output);
        }

        public bool NeedsInput()
        {
            return _encoder.NeedsInput();
        }

        public int Finish(byte[] output)
        {
            Debug.Assert(output != null, "Can't pass in a null output buffer!");
            return _encoder.Finish(output);
        }
    }
}