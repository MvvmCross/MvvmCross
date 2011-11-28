namespace Phone7.Fx.Security.Cryptography
{
    /// <summary>
    /// Declaration of KEY_SET class
    /// </summary>
    public class KeySet
    {
        public const int KEY_COUNT = 17;
        internal BLOCK8BYTE[] _array;

        internal KeySet()
        {
            // Create array
            _array = new BLOCK8BYTE[KEY_COUNT];
            for (int i1 = 0; i1 < KEY_COUNT; i1++)
                _array[i1] = new BLOCK8BYTE();
        }

        public BLOCK8BYTE GetAt(int iArrayOffset)
        {
            return _array[iArrayOffset];
        }
    }
}