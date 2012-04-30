namespace Phone7.Fx.Security.Cryptography
{
    internal class WorkingSet
    {
        internal BLOCK8BYTE IP = new BLOCK8BYTE();
        internal BLOCK8BYTE[] Ln = new BLOCK8BYTE[17];
        internal BLOCK8BYTE[] Rn = new BLOCK8BYTE[17];
        internal BLOCK8BYTE RnExpand = new BLOCK8BYTE();
        internal BLOCK8BYTE XorBlock = new BLOCK8BYTE();
        internal BLOCK8BYTE SBoxValues = new BLOCK8BYTE();
        internal BLOCK8BYTE f = new BLOCK8BYTE();
        internal BLOCK8BYTE X = new BLOCK8BYTE();

        internal BLOCK8BYTE DataBlockIn = new BLOCK8BYTE();
        internal BLOCK8BYTE DataBlockOut = new BLOCK8BYTE();
        internal BLOCK8BYTE DecryptXorBlock = new BLOCK8BYTE();

        internal WorkingSet()
        {
            // Build the arrays
            for (int i1 = 0; i1 < 17; i1++)
            {
                Ln[i1] = new BLOCK8BYTE();
                Rn[i1] = new BLOCK8BYTE();
            }
        }

        internal void Scrub()
        {
            // Scrub data
            IP.Reset();
            for (int i1 = 0; i1 < 17; i1++)
            {
                Ln[i1].Reset();
                Rn[i1].Reset();
            }
            RnExpand.Reset();
            XorBlock.Reset();
            SBoxValues.Reset();
            f.Reset();
            X.Reset();
            DataBlockIn.Reset();
            DataBlockOut.Reset();
            DecryptXorBlock.Reset();
        }
    }
}