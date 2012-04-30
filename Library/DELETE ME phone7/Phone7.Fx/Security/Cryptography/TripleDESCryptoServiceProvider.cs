using System;
using System.Security.Cryptography;

namespace Phone7.Fx.Security.Cryptography
{
    public class TripleDESCryptoServiceProvider : SymmetricAlgorithm 
    {
        public const int KEY_BYTE_LENGTH = 8;
        public const int BITS_PER_BYTE = 8;

        public TripleDESCryptoServiceProvider()
        {

        }

        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new TripleDesManaged(rgbKey, rgbIV, false);
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new TripleDesManaged(rgbKey, rgbIV, true);
        }

        public override void GenerateIV()
        {
            base.IVValue = new byte[BITS_PER_BYTE];
            RandomNumberGenerator rnd = new RNGCryptoServiceProvider();
            rnd.GetBytes(base.IVValue);
        }

        public override void GenerateKey()
        {
            RandomNumberGenerator rnd = new RNGCryptoServiceProvider();
            byte[] Ftmp = new byte[KEY_BYTE_LENGTH * 3];
            // Fill with random data
            rnd.GetBytes(Ftmp);

            // Make the key good
            Ftmp = MakeGoodTripleDesKey(Ftmp);
            // Call sibling function
            this.KeyValue = Ftmp;
        }

        public byte[] MakeGoodTripleDesKey(byte[] keyIn)
		{
			// Declare return variable
			byte[] ftmp = new byte[KEY_BYTE_LENGTH * 3];

			// Declaration of local variables
			byte[] subKey = new byte[KEY_BYTE_LENGTH];

			// Loop through key modifications
			int iInc = 0;
			while (true)
			{
				// Start with the key
				Array.Copy(keyIn, ftmp, KEY_BYTE_LENGTH * 3);

				// Make sure each part of the key is valid
				int iKey = 0;
				for (iKey = 0; iKey < 3; iKey++)
				{
					// Get the sub-key
					Array.Copy(ftmp, iKey * KEY_BYTE_LENGTH, subKey, 0, KEY_BYTE_LENGTH);

					// Increment sub-key
					DesManaged.IncKey(subKey, iInc * (iKey + 1));

					// Make the parity valid
                    DesManaged.ModifyKeyParity(subKey);

					// Return to the Ftmp
					Array.Copy(subKey, 0, ftmp, iKey * KEY_BYTE_LENGTH, KEY_BYTE_LENGTH);
				}

				// Check the key
                if (TripleDesManaged.IsValidTripleDESKey(ftmp))
					break;

				// Move on
				iInc++;

			} // while-loop

			// Return variable
			return ftmp;
		}

        public override byte[] Key
        {
            get
            {
                if (KeyValue == null)
                    GenerateKey();
                return KeyValue;
            }
            set
            {
                if (!TripleDesManaged.IsValidTripleDESKey(value))
                    throw new Exception("Invalid DES key.");
                KeyValue = value;
            }
        }
    }
}