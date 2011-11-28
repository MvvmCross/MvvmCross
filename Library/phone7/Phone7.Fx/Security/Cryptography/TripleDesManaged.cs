using System;

namespace Phone7.Fx.Security.Cryptography
{
    public class TripleDesManaged : DesManaged
    {
        private readonly byte[] rgbKey;
        private byte[] rgbIV;
        private readonly bool _shouldEncrypt;

        public TripleDesManaged(byte[] rgbKey, byte[] rgbIV, bool shouldEncrypt)
            : base(rgbKey, rgbIV, shouldEncrypt)
        {
            this.rgbKey = rgbKey;
            this.rgbIV = rgbIV;
            _shouldEncrypt = shouldEncrypt;
        }

      

        public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
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
				ExpandKey(rgbKey, 0),
				ExpandKey(rgbKey, 8),
				ExpandKey(rgbKey, 16)
			};

            // Apply DES keys
            DESAlgorithm(inputBuffer, ref bufferOut, kn, _shouldEncrypt);

            // If decrypting...
            if (!_shouldEncrypt)
                RemovePadding(ref bufferOut);

            return bufferOut;
        }


        internal static bool IsValidTripleDESKey(byte[] key)
        {

            // Shortcuts
            if (key == null)
                return false;
            if (key.Length != (3 * KEY_BYTE_LENGTH))
                return false;

            // Check each part of the key
            byte[] subKey = new byte[KEY_BYTE_LENGTH];
            for (int keyLoop = 0; keyLoop < 3; keyLoop++)
            {

                // Get sub-key
                Array.Copy(key, keyLoop * 8, subKey, 0, KEY_BYTE_LENGTH);

                // Check this DES key
                if (!IsValidDESKey(subKey))
                    return false;

            }

            // Keys must not be equal
            bool bAEqualsB = true;
            bool bAEqualsC = true;
            bool bBEqualsC = true;
            for (int byteOffset = 0; byteOffset < KEY_BYTE_LENGTH; byteOffset++)
            {
                if (key[byteOffset] != key[byteOffset + KEY_BYTE_LENGTH])
                    bAEqualsB = false;
                if (key[byteOffset] != key[byteOffset + KEY_BYTE_LENGTH + KEY_BYTE_LENGTH])
                    bAEqualsC = false;
                if (key[byteOffset + KEY_BYTE_LENGTH] != key[byteOffset + KEY_BYTE_LENGTH + KEY_BYTE_LENGTH])
                    bBEqualsC = false;
            }
            if ((bAEqualsB) || (bAEqualsC) || (bBEqualsC))
                return false;

            // Return success
            return true;

        }
    }
}