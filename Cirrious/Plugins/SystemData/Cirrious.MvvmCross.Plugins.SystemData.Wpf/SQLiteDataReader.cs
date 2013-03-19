using System.Data;

namespace Cirrious.MvvmCross.Plugins.SystemData.Wpf
{
    public class SQLiteDataReader : Cirrious.MvvmCross.Plugins.SystemData.IDataReader
    {
        private System.Data.IDataReader reader;

        public SQLiteDataReader(System.Data.IDataReader reader)
        {
            this.reader = reader;
        }

        public bool IsClosed
        {
            get { return reader.IsClosed; }
        }

        public int RecordsAffected
        {
            get { return reader.RecordsAffected; }
        }

        public void Close()
        {
            reader.Close();
        }

        public bool Read()
        {
           return reader.Read();
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        public int FieldCount
        {
            get {return reader.FieldCount; }
        }

        public object this[int i]
        {
            get { return reader[i]; }
        }

        public object this[string name]
        {
            get { return reader[name]; }
        }

        public bool GetBoolean(int i)
        {
            throw new System.NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new System.NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new System.NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new System.NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new System.NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new System.NotImplementedException();
        }

        public System.DateTime GetDateTime(int i)
        {
            return reader.GetDateTime(i);
        }

        public decimal GetDecimal(int i)
        {
            throw new System.NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new System.NotImplementedException();
        }

        public System.Type GetFieldType(int i)
        {
            throw new System.NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new System.NotImplementedException();
        }

        public System.Guid GetGuid(int i)
        {
            return reader.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            throw new System.NotImplementedException();
        }

        public int GetInt32(int i)
        {
            return reader.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            throw new System.NotImplementedException();
        }

        public string GetName(int i)
        {
            return reader.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            return reader.GetOrdinal(name);
        }

        public string GetString(int i)
        {
            return reader.GetString(i);
        }

        public object GetValue(int i)
        {
            return reader.GetValue(i);
        }

        public int GetValues(object[] values)
        {
            throw new System.NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            return reader.IsDBNull(i);
        }
    }
}
