using System;
using System.Data;

namespace LCW.Framework.Common.DataAccess
{
    public class RefCountingDataReader : DataReaderWrapper
    {
        private readonly DatabaseConnectionWrapper connectionWrapper;

        public RefCountingDataReader(DatabaseConnectionWrapper connection, IDataReader innerReader)
            : base(innerReader)
        {           
            connectionWrapper = connection;
            connectionWrapper.AddRef();
        }

        public override void Close()
        {
            if (!IsClosed)
            {
                base.Close();
                connectionWrapper.Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!IsClosed)
                {
                    base.Dispose(true);
                    connectionWrapper.Dispose();
                }
            }
        }
    }
}
