using System;
using System.Data;

namespace LCW.Framework.Common.DataAccess
{
    public abstract class DataReaderWrapper : MarshalByRefObject,IDataReader
    {
        private readonly IDataReader innerReader;

        protected DataReaderWrapper(IDataReader innerReader)
        {
            this.innerReader = innerReader;
        }

        public IDataReader InnerReader { get { return innerReader; } }

        public virtual void Close()
        {
            if (!innerReader.IsClosed)
            {
                innerReader.Close();
            }
        }

        public virtual int Depth
        {
            get { return innerReader.Depth; }
        }

        public virtual DataTable GetSchemaTable()
        {
            return innerReader.GetSchemaTable();
        }

        public virtual bool IsClosed
        {
            get { return innerReader.IsClosed; }
        }

        public virtual bool NextResult()
        {
            return innerReader.NextResult();
        }

        public virtual bool Read()
        {
            return innerReader.Read();
        }

        public virtual int RecordsAffected
        {
            get { return innerReader.RecordsAffected; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!innerReader.IsClosed)
                {
                    innerReader.Dispose();
                }
            }
        }

        public virtual int FieldCount
        {
            get { return innerReader.FieldCount; }
        }

        public virtual bool GetBoolean(int i)
        {
            return innerReader.GetBoolean(i);
        }
        public virtual bool GetBoolean(string name)
        {
            return GetBoolean(GetOrdinal(name));
        }

        public virtual byte GetByte(int i)
        {
            return innerReader.GetByte(i);
        }
        public virtual byte GetByte(string name)
        {
            return GetByte(GetOrdinal(name));
        }

        public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return innerReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }
        public virtual long GetBytes(string name, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return GetBytes(GetOrdinal(name), fieldOffset,buffer,bufferoffset,length);
        }

        public virtual char GetChar(int i)
        {
            return innerReader.GetChar(i);
        }
        public virtual char GetChar(string name)
        {
            return GetChar(GetOrdinal(name));
        }

        public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return innerReader.GetChars(i,fieldoffset,buffer,bufferoffset,length);
        }
        public virtual long GetChars(string name, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return GetChars(GetOrdinal(name), fieldoffset, buffer, bufferoffset, length);
        }

        public virtual IDataReader GetData(int i)
        {
            return innerReader.GetData(i);
        }
        public virtual IDataReader GetData(string name)
        {
            return GetData(GetOrdinal(name));
        }

        public virtual string GetDataTypeName(int i)
        {
            return innerReader.GetDataTypeName(i);
        }
        public virtual string GetDataTypeName(string name)
        {
            return GetDataTypeName(GetOrdinal(name));
        }

        public virtual DateTime GetDateTime(int i)
        {
            return innerReader.GetDateTime(i);
        }
        public virtual DateTime GetDateTime(string name)
        {
            return GetDateTime(GetOrdinal(name));
        }

        public virtual decimal GetDecimal(int i)
        {
            return innerReader.GetDecimal(i);
        }
        public virtual decimal GetDecimal(string name)
        {
            return GetDecimal(GetOrdinal(name));
        }

        public virtual double GetDouble(int i)
        {
            return innerReader.GetDouble(i);
        }
        public virtual double GetDouble(string name)
        {
            return GetDouble(GetOrdinal(name));
        }

        public virtual Type GetFieldType(int i)
        {
            return innerReader.GetFieldType(i);
        }
        public virtual Type GetFieldType(string name)
        {
            return GetFieldType(GetOrdinal(name));
        }

        public virtual float GetFloat(int i)
        {
            return innerReader.GetFloat(i);
        }
        public virtual float GetFloat(string name)
        {
            return GetFloat(GetOrdinal(name));
        }

        public virtual Guid GetGuid(int i)
        {
            return innerReader.GetGuid(i);
        }
        public virtual Guid GetGuid(string name)
        {
            return GetGuid(GetOrdinal(name));
        }

        public virtual short GetInt16(int i)
        {
            return innerReader.GetInt16(i);
        }
        public virtual short GetInt16(string name)
        {
            return GetInt16(GetOrdinal(name));
        }

        public virtual int GetInt32(int i)
        {
            return innerReader.GetInt32(i);
        }
        public virtual int GetInt32(string name)
        {
            return GetInt32(GetOrdinal(name));
        }

        public virtual long GetInt64(int i)
        {
            return innerReader.GetInt64(i);
        }
        public virtual long GetInt64(string name)
        {
            return GetInt64(GetOrdinal(name));
        }

        public virtual string GetName(int i)
        {
            return innerReader.GetName(i);
        }      

        public virtual int GetOrdinal(string name)
        {
            return innerReader.GetOrdinal(name);
        }

        public virtual string GetString(int i)
        {
            return innerReader.GetString(i);
        }
        public virtual string GetString(string name)
        {
            return GetString(GetOrdinal(name));
        }

        public virtual object GetValue(int i)
        {
            return innerReader.GetValue(i);
        }
        public virtual object GetValue(string name)
        {
            return GetValue(GetOrdinal(name));
        }

        public virtual int GetValues(object[] values)
        {
            return innerReader.GetValues(values);
        }

        public virtual bool IsDBNull(int i)
        {
            return innerReader.IsDBNull(i);
        }
        public virtual bool IsDBNull(string name)
        {
            return IsDBNull(GetOrdinal(name));
        }

        public virtual object this[string name]
        {
            get
            {
                return innerReader[name];
            }
        }

        public virtual object this[int i]
        {
            get { return innerReader[i]; }
        }
    }
}
