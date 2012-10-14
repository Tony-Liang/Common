using System;
using System.Data.Common;
using System.Data;
using System.Globalization;

namespace LCW.Framework.Common.DataAccess
{
    public abstract class Database
    {
        private readonly ConnectionString connectionstring;
        private readonly DbProviderFactory dbProviderFactory;

        protected Database(string connectionString, DbProviderFactory dbProviderFactory)
        {
            this.connectionstring = new ConnectionString(connectionString);
            this.dbProviderFactory = dbProviderFactory;
        }

        public string ConnectionString
        {
            get
            {
                return connectionstring.ToString();
            }
        }

        public DbProviderFactory DbProviderFactory
        {
            get
            {
                return dbProviderFactory;
            }
        }

        public virtual DbConnection CreateConnection()
        {
            DbConnection connection=dbProviderFactory.CreateConnection();
            connection.ConnectionString = ConnectionString;
            return connection;
        }

        internal DbConnection GetNewConnection()
        {
            DbConnection connection = null;
            try
            {
                connection = CreateConnection();
                connection.Open();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return connection;
        }

        protected DatabaseConnectionWrapper GetOpenConnection()
        {
            DatabaseConnectionWrapper con = TransactionScopeConnections.GetConnection(this);
            return con ?? GetWrappedConnection();
        }

        protected virtual DatabaseConnectionWrapper GetWrappedConnection()
        {
            return new DatabaseConnectionWrapper(GetNewConnection());
        }



        public virtual int ExecuteNonQuery(DbCommand command)
        {
            using (var wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.DbConnection);
                return DoExecuteNonQuery(command);
            }
        }

        public virtual int ExecuteNonQuerybySqlStringCommand(string query)
        {
            using (DbCommand command = GetSqlStringCommand(query))
            {
                return ExecuteNonQuery(command);
            }
        }

        public virtual int ExecuteNonQuerybyStoredProcCommand(string storedProcedureName)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName))
            {
                return ExecuteNonQuery(command);
            }
        }

        public virtual IDataReader ExecuteReader(DbCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            using (DatabaseConnectionWrapper wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.DbConnection);
                IDataReader reader = DoExecuteReader(command, CommandBehavior.Default);
                return CreateWrapperReader(wrapper, reader);
            }
        }

        public virtual IDataReader ExecuteReaderbySqlStringCommand(string query)
        {
            using (var command = GetSqlStringCommand(query))
            {
                return ExecuteReader(command);
            }
        }

        public virtual IDataReader ExecuteReaderbyStoredProcCommand(string storedProcedureName)
        {
            using (var command = GetStoredProcCommand(storedProcedureName))
            {
                return ExecuteReader(command);
            }
        }

        public virtual object ExecuteScalar(DbCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");

            using (var wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.DbConnection);
                return DoExecuteScalar(command);
            }
        }

        public virtual object ExecuteScalarbySqlStringCommand(string query)
        {
            using (var command = GetSqlStringCommand(query))
            {
                return ExecuteScalar(command);
            }
        }

        public virtual object ExecuteScalarbyStoredProcCommand(string storedProcedureName)
        {
            using (var command = GetStoredProcCommand(storedProcedureName))
            {
                return ExecuteScalar(command);
            }
        }

        protected int DoExecuteNonQuery(DbCommand command)
        {
            if (command == null) throw new ArgumentNullException("Command");
            try
            {
                DateTime startTime = DateTime.Now;
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected IDataReader DoExecuteReader(DbCommand command, CommandBehavior cmdBehavior)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                IDataReader reader = command.ExecuteReader(cmdBehavior);
                return reader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected object DoExecuteScalar(IDbCommand command)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                object returnValue = command.ExecuteScalar();
                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public virtual DbCommand GetSqlStringCommand(string query)
        {
            if (string.IsNullOrEmpty(query)) throw new ArgumentException("query");

            return CreateCommand(query,CommandType.Text);
        }

        public virtual DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            if (string.IsNullOrEmpty(storedProcedureName)) throw new ArgumentException("storedProcedureName");

            return CreateCommand(storedProcedureName,CommandType.StoredProcedure);
        }

        internal DbCommand CreateCommand(string commandText, CommandType commandType)
        {
            DbCommand command=dbProviderFactory.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            return command;
        }

        protected static void PrepareCommand(DbCommand command, DbConnection connection)
        {
            if (command == null) throw new ArgumentNullException("Command");
            if (connection == null) throw new ArgumentNullException("Connection");

            command.Connection = connection;
        }

        protected static void PrepareCommand(DbCommand command,DbTransaction transaction)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (transaction == null) throw new ArgumentNullException("transaction");

            PrepareCommand(command, transaction.Connection);
            command.Transaction = transaction;
        }

        protected virtual IDataReader CreateWrapperReader(DatabaseConnectionWrapper wrapper, IDataReader innerReader)
        {
            return new RefCountingDataReader(wrapper, innerReader);
        }


        protected DbParameter CreateParameter(string name)
        {
            DbParameter param = dbProviderFactory.CreateParameter();
            param.ParameterName = BuildParameterName(name);
            return param;
        }

        protected DbParameter CreateParameter(string name,
                                              DbType dbType,
                                              int size,
                                              ParameterDirection direction,
                                              bool nullable,
                                              byte precision,
                                              byte scale,
                                              string sourceColumn,
                                              DataRowVersion sourceVersion,
                                              object value)
        {
            DbParameter param = CreateParameter(name);
            ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return param;
        }

        public virtual string BuildParameterName(string name)
        {
            return name;
        }

        protected virtual void ConfigureParameter(DbParameter param,
                                                  string name,
                                                  DbType dbType,
                                                  int size,
                                                  ParameterDirection direction,
                                                  bool nullable,
                                                  byte precision,
                                                  byte scale,
                                                  string sourceColumn,
                                                  DataRowVersion sourceVersion,
                                                  object value)
        {
            param.DbType = dbType;
            param.Size = size;
            param.Value = value ?? DBNull.Value;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
        }

        public void AddInParameter(DbCommand command,string name,DbType dbType)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
        }

        public void AddInParameter(DbCommand command,string name,DbType dbType,object value)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        public void AddInParameter(DbCommand command,string name,DbType dbType,string sourceColumn,DataRowVersion sourceVersion)
        {
            AddParameter(command, name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, null);
        }

        public void AddOutParameter(DbCommand command,string name,DbType dbType,int size)
        {
            AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        public virtual void AddParameter(DbCommand command,
                                         string name,
                                         DbType dbType,
                                         int size,
                                         ParameterDirection direction,
                                         bool nullable,
                                         byte precision,
                                         byte scale,
                                         string sourceColumn,
                                         DataRowVersion sourceVersion,
                                         object value)
        {
            if (command == null) throw new ArgumentNullException("command");

            DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }

        public void AddParameter(DbCommand command,
                                 string name,
                                 DbType dbType,
                                 ParameterDirection direction,
                                 string sourceColumn,
                                 DataRowVersion sourceVersion,
                                 object value)
        {
            AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
        }

        public virtual void AssignParameters(DbCommand command, object[] parameterValues)
        {
            //parameterCache.SetParameters(command, this);

            if (SameNumberOfParametersAndValues(command, parameterValues) == false)
            {
                throw new InvalidOperationException("");
            }

            AssignParameterValues(command, parameterValues);
        }
        protected void AssignParameterValues(DbCommand command,object[] values)
        {
            int parameterIndexShift = 0; //UserParametersStartIndex();
            for (int i = 0; i < values.Length; i++)
            {
                IDataParameter parameter = command.Parameters[i + parameterIndexShift];

                SetParameterValue(command, parameter.ParameterName, values[i]);
            }
        }

        public virtual void SetParameterValue(DbCommand command,string parameterName,object value)
        {
            if (command == null) throw new ArgumentNullException("command");

            command.Parameters[BuildParameterName(parameterName)].Value = value ?? DBNull.Value;
        }

        protected virtual bool SameNumberOfParametersAndValues(DbCommand command,object[] values)
        {
            int numberOfParametersToStoredProcedure = command.Parameters.Count;
            int numberOfValuesProvidedForStoredProcedure = values.Length;
            return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
        }


        public virtual DbCommand GetStoredProcCommand(string storedProcedureName,params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(storedProcedureName)) throw new ArgumentException("ExceptionNullOrEmptyString", "storedProcedureName");

            DbCommand command = GetStoredProcCommand(storedProcedureName);

            AssignParameters(command, parameterValues);

            return command;
        }
        #region Async  methods
        public virtual bool SupportsAsync
        {
            get { return false; }
        }

        public virtual IAsyncResult BeginExecuteNonQuery(DbCommand command, AsyncCallback callback, object state)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteNonQuery(DbCommand command, DbTransaction transaction, AsyncCallback callback,object state)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteNonQuery(string storedProcedureName, AsyncCallback callback, object state,params object[] parameterValues)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteNonQuery(DbTransaction transaction,
            string storedProcedureName,
            AsyncCallback callback,
            object state,
            params object[] parameterValues)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteNonQuery(CommandType commandType,
            string commandText,
            AsyncCallback callback,
            object state)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteNonQuery(DbTransaction transaction, 
            CommandType commandType,
            string commandText,
            AsyncCallback callback, object state)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual int EndExecuteNonQuery(IAsyncResult asyncResult)
        {
            AsyncNotSupported();
            return 0;
        }

        public virtual IAsyncResult BeginExecuteReader(DbCommand command, AsyncCallback callback, object state)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteReader(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteReader(string storedProcedureName, 
            AsyncCallback callback, 
            object state,
            params object[] parameterValues)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteReader(DbTransaction transaction, 
            string storedProcedureName, 
            AsyncCallback callback,
            object state, params object[] parameterValues)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteReader(CommandType commandType, 
            string commandText, 
            AsyncCallback callback,
            object state)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteReader(DbTransaction transaction, 
            CommandType commandType,
            string commandText,
            AsyncCallback callback, object state)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IDataReader EndExecuteReader(IAsyncResult asyncResult)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteScalar(DbCommand command, AsyncCallback callback, object state)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteScalar(DbCommand command,
            DbTransaction transaction,
            AsyncCallback callback,
            object state)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteScalar(string storedProcedureName, 
            AsyncCallback callback, 
            object state,
            params object[] parameterValues)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteScalar(DbTransaction transaction, 
            string storedProcedureName, 
            AsyncCallback callback,
            object state, params object[] parameterValues)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteScalar(CommandType commandType, 
            string commandText, 
            AsyncCallback callback,
            object state)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual IAsyncResult BeginExecuteScalar(DbTransaction transaction, 
            CommandType commandType, 
            string commandText,
            AsyncCallback callback, object state)
        {
            AsyncNotSupported();
            return null;
        }

        public virtual object EndExecuteScalar(IAsyncResult asyncResult)
        {
            AsyncNotSupported();
            return null;
        }

        private void AsyncNotSupported()
        {
            throw new InvalidOperationException(
                string.Format(
                CultureInfo.CurrentCulture,
                "AsyncOperationsNotSupported",
                GetType().Name));
        }
        #endregion
    }
}
