using System;
using System.Data.Common;
using System.Threading;


namespace LCW.Framework.Common.DataAccess
{
    public class DatabaseConnectionWrapper:IDisposable
    {
        private int refCount;
        private DbConnection dbConnection;

        public DbConnection DbConnection
        {
            get;
            private set;
        }

        public DatabaseConnectionWrapper(DbConnection dbConnection)
        {
            dbConnection = dbConnection;
            refCount = 1;
        }

        public bool IsDispose
        {
            get
            {
               return refCount == 0;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                int count=Interlocked.Decrement(ref refCount);
                if(count==0)
                {
                    dbConnection.Dispose();
                    dbConnection=null;
                    GC.SuppressFinalize(this);
                }
            }
        }

        public DatabaseConnectionWrapper AddRef()
        {
            Interlocked.Increment(ref refCount);
            return this;
        }
    }
}
