using System;
using System.Diagnostics;

namespace Phone7.Fx.Security.Cryptography
{
    public class BLOCK8BYTE
    {
        public const int BYTE_LENGTH = 8;
        internal byte[] _data = new byte[BYTE_LENGTH];

        public void Reset()
        {
            // Reset bytes
            Array.Clear(_data, 0, BYTE_LENGTH);
        }

        public void Set(BLOCK8BYTE source)
        {
            // Copy source data to this
            this.Set(source._data, 0);
        }

        public void Set(byte[] buffer, int offset)
        {
            // Set contents by copying array
            Array.Copy(buffer, offset, _data, 0, BYTE_LENGTH);
        }

        public void Xor(BLOCK8BYTE a, BLOCK8BYTE b)
        {
            // Set byte to A ^ B
            for (int offset = 0; offset < BYTE_LENGTH; offset++)
                _data[offset] = Convert.ToByte(a._data[offset] ^ b._data[offset]);
        }

        public void SetBit(int byteOffset, int bitOffset, bool flag)
        {
            // Compose mask
            byte mask = Convert.ToByte(1 << bitOffset);
            if (((_data[byteOffset] & mask) == mask) != flag)
                _data[byteOffset] ^= mask;
        }

        public bool GetBit(int byteOffset, int bitOffset)
        {
            // call sibling function
            return ((this._data[byteOffset] >> bitOffset) & 0x01) == 0x01;
        }

        public void ShiftLeftWrapped(BLOCK8BYTE s, int bitShift)
        {
            // this shift is only applied to the first 32 bits, and parity bit is ignored

            // Declaration of local variables
            int byteOffset;
            bool bit;

            // Copy byte and shift regardless
            for (byteOffset = 0; byteOffset < 4; byteOffset++)
                _data[byteOffset] = Convert.ToByte((s._data[byteOffset] << bitShift) & 0xFF);

            // if shifting by 1...
            if (bitShift == 1)
            {
                // repair bits on right of BYTE
                for (byteOffset = 0; byteOffset < 3; byteOffset++)
                {
                    // get repairing bit offsets
                    bit = s.GetBit(byteOffset + 1, 7);
                    this.SetBit(byteOffset, 1, bit);
                }
                // wrap around the final bit
                this.SetBit(3, 1, s.GetBit(0, 7));
            }
            else if (bitShift == 2)
            {
                // repair bits on right of BYTE
                for (byteOffset = 0; byteOffset < 3; byteOffset++)
                {
                    // get repairing bit offsets
                    bit = s.GetBit(byteOffset + 1, 7);
                    this.SetBit(byteOffset, 2, bit);
                    bit = s.GetBit(byteOffset + 1, 6);
                    this.SetBit(byteOffset, 1, bit);
                }
                // wrap around the final bit
                this.SetBit(3, 2, s.GetBit(0, 7));
                this.SetBit(3, 1, s.GetBit(0, 6));
            }
            else
                Debug.Assert(false);
        }
    }
}