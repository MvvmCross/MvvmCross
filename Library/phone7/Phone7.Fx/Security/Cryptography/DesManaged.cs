using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Phone7.Fx.Security.Cryptography
{
    public class DesManaged : ICryptoTransform
    {
        private readonly byte[] _rgbKey;
        private byte[] rgbIV;
        private readonly bool _shouldEncrypt;

        public const int KEY_BYTE_LENGTH = 8;
        public const int BITS_PER_BYTE = 8;

        public DesManaged(byte[] rgbKey, byte[] rgbIV, bool shouldEncrypt)
        {
            this._rgbKey = rgbKey;
            this.rgbIV = rgbIV;
            _shouldEncrypt = shouldEncrypt;
        }

        #region ICryptoTransform implem

        public bool CanReuseTransform
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanTransformMultipleBlocks
        {
            get { throw new System.NotImplementedException(); }
        }

        public int InputBlockSize
        {
            get { throw new System.NotImplementedException(); }
        }

        public int OutputBlockSize
        {
            get { throw new System.NotImplementedException(); }
        }

        public virtual int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            throw new System.NotImplementedException();
        }

        public virtual byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }
            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException("inputOffset");
            }
            if ((inputCount < 0) || (inputCount > inputBuffer.Length))
            {
                throw new ArgumentException("Argument InvalidValue");
            }
            if ((inputBuffer.Length - inputCount) < inputOffset)
            {
                throw new ArgumentException("Argument InvalidOffLenght");
            }


            byte[] bufferOut = null;

            // Create the output buffer
            CreateBufferOut(inputBuffer.Length, ref bufferOut, _shouldEncrypt);

            // Expand the keys into Kn
            KeySet[] kn = new[] {
				ExpandKey(_rgbKey, 0)
			};

            // Apply DES keys
            DESAlgorithm(inputBuffer, ref bufferOut, kn, _shouldEncrypt);

            // If decrypting...
            if (!_shouldEncrypt)
                RemovePadding(ref bufferOut);

            return bufferOut;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Static Operations

        internal static bool IsStrongDESKey(byte[] key)
        {
            // Compare by large integer
            UInt64 uiKey = BitConverter.ToUInt64(key, 0);

            // Find weak keys...
            if (
                (uiKey == 0x0000000000000000) ||
                (uiKey == 0x00000000FFFFFFFF) ||
                (uiKey == 0xE0E0E0E0F1F1F1F1) ||
                (uiKey == 0x1F1F1F1F0E0E0E0E)
            )
                return false;

            // Find semi-weak keys...
            if (
                (uiKey == 0x011F011F010E010E) ||
                (uiKey == 0x1F011F010E010E01) ||
                (uiKey == 0x01E001E001F101F1) ||
                (uiKey == 0xE001E001F101F101) ||
                (uiKey == 0x01FE01FE01FE01FE) ||
                (uiKey == 0xFE01FE01FE01FE01) ||
                (uiKey == 0x1FE01FE00EF10EF1) ||
                (uiKey == 0xE01FE01FF10EF10E) ||
                (uiKey == 0x1FFE1FFE0EFE0EFE) ||
                (uiKey == 0xFE1FFE1FFE0EFE0E) ||
                (uiKey == 0xE0FEE0FEF1FEF1FE) ||
                (uiKey == 0xFEE0FEE0FEF1FEF1)
            )
                return false;

            return true;
        }

        internal static bool IsValidDESKey(byte[] key)
        {
            // Shortcuts
            if (key == null)
                return false;
            if (key.Length != KEY_BYTE_LENGTH)
                return false;
            if (!IsStrongDESKey(key))
                return false;

            // Make sure end bits have odd parity
            for (int byteOffset = 0; byteOffset < KEY_BYTE_LENGTH; byteOffset++)
            {
                // Add bits for this byte
                int iTotalBits = 0;
                byte Mask = 1;
                for (int bitOffset = 0; bitOffset < BITS_PER_BYTE; bitOffset++)
                {
                    if ((key[byteOffset] & Mask) != 0)
                        iTotalBits++;
                    Mask <<= 1;
                }

                // If the total bits is not odd...
                if ((iTotalBits % 2) != 1)
                    return false;
            }
            return true;
        }

        internal static void IncKey(byte[] key, int inc)
        {
            Debug.Assert(key.Length == KEY_BYTE_LENGTH);

            // shortcuts
            if (inc == 0)
                return;

            // Add the increment				
            int iCarry = inc;
            for (int byteOffset = 0; byteOffset < KEY_BYTE_LENGTH; byteOffset++)
            {
                int iTemp = key[byteOffset] + iCarry;
                iCarry = iTemp >> 8;
                key[byteOffset] = Convert.ToByte(iTemp & 0xFF);
                if (iCarry == 0)
                    break;
            }
        }

        internal static void ModifyKeyParity(byte[] key)
        {
            Debug.Assert(key.Length == KEY_BYTE_LENGTH);

            // Make sure end bits have odd parity
            for (int byteOffset = 0; byteOffset < KEY_BYTE_LENGTH; byteOffset++)
            {
                // Add bits for this byte
                int iTotalBits = 0;
                byte Mask = 1;
                for (int bitOffset = 0; bitOffset < BITS_PER_BYTE; bitOffset++)
                {
                    if ((key[byteOffset] & Mask) != 0)
                        iTotalBits++;
                    Mask <<= 1;
                }

                // If the total bits is not odd...
                if ((iTotalBits % 2) != 1)
                {
                    // Flip the first bit to retain odd parity
                    key[byteOffset] ^= 0x01;
                }
            }
        }

        protected KeySet ExpandKey(byte[] key, int offset)
        {
            //
            // Expand an 8 byte DES key into a set of permuted keys
            //

            // Declare return variable
            KeySet ftmp = new KeySet();

            // Declaration of local variables
            int arrayOffset, permOffset, byteOffset, bitOffset;
            bool bBit;

            // Put key into an 8-bit block
            BLOCK8BYTE k = new BLOCK8BYTE();
            k.Set(key, offset);

            // Permutate Kp with PC1
            BLOCK8BYTE kp = new BLOCK8BYTE();
            for (arrayOffset = 0; arrayOffset < DESTables.PC1.Length; arrayOffset++)
            {
                // Get permute offset
                permOffset = DESTables.PC1[arrayOffset];
                permOffset--;

                // Get and set bit
                kp.SetBit(
                    BitAddressToByteOffset(arrayOffset, 7),
                    BitAddressToBitOffset(arrayOffset, 7),
                    k.GetBit(
                        BitAddressToByteOffset(permOffset, 8),
                        BitAddressToBitOffset(permOffset, 8)
                    )
                );

            }

            // Create 17 blocks of C and D from Kp
            BLOCK8BYTE[] kpCn = new BLOCK8BYTE[17];
            BLOCK8BYTE[] kpDn = new BLOCK8BYTE[17];
            for (arrayOffset = 0; arrayOffset < 17; arrayOffset++)
            {
                kpCn[arrayOffset] = new BLOCK8BYTE();
                kpDn[arrayOffset] = new BLOCK8BYTE();
            }
            for (arrayOffset = 0; arrayOffset < 32; arrayOffset++)
            {
                // Set bit in KpCn
                byteOffset = BitAddressToByteOffset(arrayOffset, 8);
                bitOffset = BitAddressToBitOffset(arrayOffset, 8);
                bBit = kp.GetBit(byteOffset, bitOffset);
                kpCn[0].SetBit(byteOffset, bitOffset, bBit);

                // Set bit in KpDn
                bBit = kp.GetBit(byteOffset + 4, bitOffset);
                kpDn[0].SetBit(byteOffset, bitOffset, bBit);
            }
            for (arrayOffset = 1; arrayOffset < 17; arrayOffset++)
            {
                // Shift left wrapped
                kpCn[arrayOffset].ShiftLeftWrapped(kpCn[arrayOffset - 1], DESTables.Shifts[arrayOffset - 1]);
                kpDn[arrayOffset].ShiftLeftWrapped(kpDn[arrayOffset - 1], DESTables.Shifts[arrayOffset - 1]);
            }

            // Create 17 keys Kn
            for (arrayOffset = 0; arrayOffset < 17; arrayOffset++)
            {
                // Loop through the bits
                int tableOffset;
                for (tableOffset = 0; tableOffset < 48; tableOffset++)
                {
                    // Get address if bit
                    permOffset = DESTables.PC2[tableOffset];
                    permOffset--;

                    // Convert to byte and bit offsets
                    byteOffset = BitAddressToByteOffset(permOffset, 7);
                    bitOffset = BitAddressToBitOffset(permOffset, 7);

                    // Get bit
                    if (byteOffset < 4)
                        bBit = kpCn[arrayOffset].GetBit(byteOffset, bitOffset);
                    else
                        bBit = kpDn[arrayOffset].GetBit(byteOffset - 4, bitOffset);

                    // Set bit
                    byteOffset = BitAddressToByteOffset(tableOffset, 6);
                    bitOffset = BitAddressToBitOffset(tableOffset, 6);
                    ftmp.GetAt(arrayOffset).SetBit(byteOffset, bitOffset, bBit);
                }
            }
            return ftmp;
        }

        protected void CreateBufferOut(int bufferInLength, ref byte[] bufferOut, bool encrypt)
        {
            //
            // Create a buffer for the output, which may be trimmed later
            //

            // If encrypting...
            int outputLength;
            if (encrypt)
            {
                if ((bufferInLength % KEY_BYTE_LENGTH) != 0)
                    outputLength = ((bufferInLength / KEY_BYTE_LENGTH) + 1) * KEY_BYTE_LENGTH;
                else
                    outputLength = bufferInLength + KEY_BYTE_LENGTH;
            }
            else
            {
                if (bufferInLength < 8)
                    throw new Exception("DES cypher-text must be at least 8 bytes.");
                if ((bufferInLength % 8) != 0)
                    throw new Exception("DES cypher-text must be a factor of 8 bytes in length.");
                outputLength = bufferInLength;
            }

            // Create buffer
            if ((bufferOut == null) || (bufferOut.Length != outputLength))
                bufferOut = new byte[outputLength];
            else
                Array.Clear(bufferOut, 0, bufferOut.Length);
        }

        protected static void RemovePadding(ref byte[] bufferOut)
        {
            //
            // Remove the padding after decrypting
            //

            // Get the padding...
            byte padding = bufferOut[bufferOut.Length - 1];
            if ((padding == 0) || (padding > 8))
                throw new Exception("Invalid padding on DES data.");

            // Confirm padding
            bool bPaddingOk = true;
            for (int byteOffset = 1; byteOffset < padding; byteOffset++)
            {
                if (bufferOut[bufferOut.Length - 1 - byteOffset] != padding)
                {
                    bPaddingOk = false;
                    break;
                }
            }
            if (bPaddingOk)
            {
                // Chop off the padding
                Array.Resize(ref bufferOut, bufferOut.Length - padding);
            }
            else
                throw new Exception("Invalid padding on DES data.");

        }

        protected void DESAlgorithm(byte[] bufferIn, ref byte[] bufferOut, KeySet[] keySets, bool encrypt)
        {
            //
            // Apply the DES algorithm to each block
            //

            // Declare a workset set of variables
            WorkingSet workingSet = new WorkingSet();

            // encode/decode blocks
            int iBufferPos = 0;
            while (true)
            {
                // Check buffer position
                if (encrypt)
                {
                    // If end of buffer...
                    if (iBufferPos >= bufferOut.Length)
                        break;
                    // Calulate remaining bytes
                    int iRemainder = (bufferIn.Length - iBufferPos);
                    if (iRemainder >= 8)
                        workingSet.DataBlockIn.Set(bufferIn, iBufferPos);
                    else
                    {
                        // Copy part-block
                        workingSet.DataBlockIn.Reset();
                        if (iRemainder > 0)
                            Array.Copy(bufferIn, iBufferPos, workingSet.DataBlockIn._data, 0, iRemainder);

                        // Get the padding byte
                        byte padding = Convert.ToByte(KEY_BYTE_LENGTH - iRemainder);

                        // Add padding to block
                        for (int byteOffset = iRemainder; byteOffset < KEY_BYTE_LENGTH; byteOffset++)
                            workingSet.DataBlockIn._data[byteOffset] = padding;
                    }
                }
                else
                {
                    // If end of buffer...
                    if (iBufferPos >= bufferIn.Length)
                        break;

                    // Get the next block
                    workingSet.DataBlockIn.Set(bufferIn, iBufferPos);
                }

                // if encrypting and not the first block...
                if ((encrypt) && (iBufferPos > 0))
                {
                    // Apply succession => XOR M with previous block
                    workingSet.DataBlockIn.Xor(workingSet.DataBlockOut, workingSet.DataBlockIn);
                }

                // Apply the algorithm
                workingSet.DataBlockOut.Set(workingSet.DataBlockIn);
                LowLevelDesAlgorithm(workingSet, keySets, encrypt);

                // If decrypting...
                if (!encrypt)
                {
                    // Retain the succession
                    if (iBufferPos > 0)
                        workingSet.DataBlockOut.Xor(workingSet.DecryptXorBlock, workingSet.DataBlockOut);

                    // Retain the last block
                    workingSet.DecryptXorBlock.Set(workingSet.DataBlockIn);
                }

                // Update buffer out
                Array.Copy(workingSet.DataBlockOut._data, 0, bufferOut, iBufferPos, 8);

                // Move on
                iBufferPos += 8;
            }

            // Scrub the working set
            workingSet.Scrub();
        }

        private void LowLevelDesAlgorithm(WorkingSet workingSet, KeySet[] keySets, bool bEncrypt)
        {
            //
            // Apply 1 or 3 keys to a block of data
            //

            // Loop through keys
            for (int iKeySetOffset = 0; iKeySetOffset < keySets.Length; iKeySetOffset++)
            {
                // Permute with byteIP
                workingSet.IP.Reset();
                int tableOffset;
                int iPermOffset;
                for (tableOffset = 0; tableOffset < DESTables.Ip.Length; tableOffset++)
                {
                    // Get perm offset
                    iPermOffset = DESTables.Ip[tableOffset];
                    iPermOffset--;

                    // Get and set bit
                    workingSet.IP.SetBit(
                        BitAddressToByteOffset(tableOffset, 8),
                        BitAddressToBitOffset(tableOffset, 8),
                        workingSet.DataBlockOut.GetBit(
                            BitAddressToByteOffset(iPermOffset, 8),
                            BitAddressToBitOffset(iPermOffset, 8)
                        )
                    );
                }

                // Create Ln[0] and Rn[0]
                workingSet.Ln[0].Reset();
                workingSet.Rn[0].Reset();
                int iArrayOffset;
                for (iArrayOffset = 0; iArrayOffset < 32; iArrayOffset++)
                {
                    int byteOffset = BitAddressToByteOffset(iArrayOffset, 8);
                    int bitOffset = BitAddressToBitOffset(iArrayOffset, 8);
                    workingSet.Ln[0].SetBit(byteOffset, bitOffset, workingSet.IP.GetBit(byteOffset, bitOffset));
                    workingSet.Rn[0].SetBit(byteOffset, bitOffset, workingSet.IP.GetBit(byteOffset + 4, bitOffset));
                }

                // Loop through 17 interations
                for (int iBlockOffset = 1; iBlockOffset < 17; iBlockOffset++)
                {
                    // Get the array offset
                    int iKeyOffset;
                    if (bEncrypt != (iKeySetOffset == 1))
                        iKeyOffset = iBlockOffset;
                    else
                        iKeyOffset = 17 - iBlockOffset;

                    // Set Ln[N] = Rn[N-1]
                    workingSet.Ln[iBlockOffset].Set(workingSet.Rn[iBlockOffset - 1]);

                    // Set Rn[N] = Ln[0] + f(R[N-1],K[N])
                    for (tableOffset = 0; tableOffset < DESTables.E.Length; tableOffset++)
                    {
                        // Get perm offset
                        iPermOffset = DESTables.E[tableOffset];
                        iPermOffset--;

                        // Get and set bit
                        workingSet.RnExpand.SetBit(
                            BitAddressToByteOffset(tableOffset, 6),
                            BitAddressToBitOffset(tableOffset, 6),
                            workingSet.Rn[iBlockOffset - 1].GetBit(
                                BitAddressToByteOffset(iPermOffset, 8),
                                BitAddressToBitOffset(iPermOffset, 8)
                            )
                        );

                    }

                    // XOR expanded block with K-block
                    if (bEncrypt != (iKeySetOffset == 1))
                        workingSet.XorBlock.Xor(workingSet.RnExpand, keySets[iKeySetOffset].GetAt(iKeyOffset));
                    else
                        workingSet.XorBlock.Xor(workingSet.RnExpand, keySets[keySets.Length - 1 - iKeySetOffset].GetAt(iKeyOffset));

                    // Set S-Box values
                    workingSet.SBoxValues.Reset();
                    for (tableOffset = 0; tableOffset < 8; tableOffset++)
                    {
                        // Calculate m and n
                        int m = ((workingSet.XorBlock.GetBit(tableOffset, 7) ? 1 : 0) << 1) | (workingSet.XorBlock.GetBit(tableOffset, 2) ? 1 : 0);
                        int n = (workingSet.XorBlock._data[tableOffset] >> 3) & 0x0F;

                        // Get s-box value
                        iPermOffset = DESTables.SBox[(tableOffset * 4) + m, n];
                        workingSet.SBoxValues._data[tableOffset] = (byte)(iPermOffset << 4);
                    }

                    // Permute with P -> f
                    workingSet.f.Reset();
                    for (tableOffset = 0; tableOffset < DESTables.P.Length; tableOffset++)
                    {
                        // Get perm offset
                        iPermOffset = DESTables.P[tableOffset];
                        iPermOffset--;

                        // Get and set bit
                        workingSet.f.SetBit(
                            BitAddressToByteOffset(tableOffset, 4),
                            BitAddressToBitOffset(tableOffset, 4),
                            workingSet.SBoxValues.GetBit(
                                BitAddressToByteOffset(iPermOffset, 4),
                                BitAddressToBitOffset(iPermOffset, 4)
                            )
                        );
                    }

                    // Rn[N] = Ln[N-1] ^ f
                    workingSet.Rn[iBlockOffset].Reset();
                    for (tableOffset = 0; tableOffset < 8; tableOffset++)
                    {
                        // Get Ln[N-1] -> A
                        byte a = workingSet.Ln[iBlockOffset - 1]._data[(tableOffset >> 1)];
                        if ((tableOffset % 2) == 0)
                            a >>= 4;
                        else
                            a &= 0x0F;

                        // Get f -> B
                        byte b = Convert.ToByte(workingSet.f._data[tableOffset] >> 4);

                        // Update Rn[N]
                        if ((tableOffset % 2) == 0)
                            workingSet.Rn[iBlockOffset]._data[tableOffset >> 1] |= Convert.ToByte((a ^ b) << 4);
                        else
                            workingSet.Rn[iBlockOffset]._data[tableOffset >> 1] |= Convert.ToByte(a ^ b);
                    }
                }

                // X = R16 L16
                workingSet.X.Reset();
                for (tableOffset = 0; tableOffset < 4; tableOffset++)
                {
                    workingSet.X._data[tableOffset] = workingSet.Rn[16]._data[tableOffset];
                    workingSet.X._data[tableOffset + 4] = workingSet.Ln[16]._data[tableOffset];
                }

                // C = X perm IP
                workingSet.DataBlockOut.Reset();
                for (tableOffset = 0; tableOffset < DESTables.Rfp.Length; tableOffset++)
                {
                    // Get perm offset
                    iPermOffset = DESTables.Rfp[tableOffset];
                    iPermOffset--;

                    // Get and set bit
                    workingSet.DataBlockOut.SetBit(
                        BitAddressToByteOffset(tableOffset, 8),
                        BitAddressToBitOffset(tableOffset, 8),
                        workingSet.X.GetBit(
                            BitAddressToByteOffset(iPermOffset, 8),
                            BitAddressToBitOffset(iPermOffset, 8)
                        )
                    );
                }
            }
        }

        #endregion Static Operations

        private static int BitAddressToByteOffset(int tableAddress, int tableWidth)
        {
            int ftmp = tableAddress / tableWidth;
            return ftmp;
        }

        private static int BitAddressToBitOffset(int tableAddress, int tableWidth)
        {
            int ftmp = BITS_PER_BYTE - 1 - (tableAddress % tableWidth);
            return ftmp;
        }
    }
}