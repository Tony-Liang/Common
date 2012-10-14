using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace LCW.Framework.Common.DataAccess.Sql
{
    public class SqlDatabase : Database
    {
        public SqlDatabase(string connectionString)
            : base(connectionString, SqlClientFactory.Instance)
        {

        }

        private static SqlCommand CheckIfSqlCommand(DbCommand command)
        {
            SqlCommand sqlCommand = command as SqlCommand;
            if (sqlCommand == null) throw new ArgumentException("ExceptionCommandNotSqlCommand", "command");
            return sqlCommand;
        }

        private static SqlCommand CreateSqlCommandByCommandType(CommandType commandType, string commandText)
        {
            return new SqlCommand(commandText)
            {
                CommandType = commandType
            };
        }

        public override bool SupportsAsync
        {
            get
            {
                return true;
            }
        }

        private IAsyncResult DoBeginExecuteNonQuery(SqlCommand command, bool disposeCommand, AsyncCallback callback, object state)
        {
            bool closeConnection = command.Transaction == null;

            try
            {
                return WrappedAsyncOperation.BeginAsyncOperation(
                    callback,
                    cb => command.BeginExecuteNonQuery(cb, state),
                    ar => new DaabAsyncResult(ar, command, disposeCommand, closeConnection, DateTime.Now));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override IAsyncResult BeginExecuteNonQuery(CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CreateSqlCommandByCommandType(commandType, commandText);
            DbConnection connection = this.GetNewConnection();
            try
            {
                PrepareCommand(sqlCommand, connection);
                return DoBeginExecuteNonQuery(sqlCommand, true, callback, state);
            }
            catch(Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }

        public override IAsyncResult BeginExecuteNonQuery(DbCommand command, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);
            DbConnection connection = this.GetNewConnection();
            try
            {
                PrepareCommand(sqlCommand, connection);
                return DoBeginExecuteNonQuery(sqlCommand, false, callback, state);
            }
            catch(Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }

        public override IAsyncResult BeginExecuteNonQuery(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);
            PrepareCommand(sqlCommand, transaction);
            return DoBeginExecuteNonQuery(sqlCommand, false, callback, state);
        }

        public override IAsyncResult BeginExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CreateSqlCommandByCommandType(commandType, commandText);
            PrepareCommand(sqlCommand, transaction);
            return DoBeginExecuteNonQuery(sqlCommand, true, callback, state);
        }

        public override IAsyncResult BeginExecuteNonQuery(DbTransaction transaction, string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(GetStoredProcCommand(storedProcedureName, parameterValues));
            PrepareCommand(sqlCommand, transaction);
            return DoBeginExecuteNonQuery(sqlCommand, true, callback, state);
        }

        public override IAsyncResult BeginExecuteNonQuery(string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(GetStoredProcCommand(storedProcedureName, parameterValues));
            DbConnection connection = this.GetNewConnection();
            try
            {
                PrepareCommand(sqlCommand, connection);
                return DoBeginExecuteNonQuery(sqlCommand, true, callback, state);
            }
            catch(Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }

        public override int EndExecuteNonQuery(IAsyncResult asyncResult)
        {
            DaabAsyncResult daabAsyncResult = (DaabAsyncResult)asyncResult;
            SqlCommand command = (SqlCommand)daabAsyncResult.Command;
            try
            {
                int affected = command.EndExecuteNonQuery(daabAsyncResult.InnerAsyncResult);               
                return affected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CleanupConnectionFromAsyncOperation(daabAsyncResult);
            }
        }

        public override IAsyncResult BeginExecuteReader(CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CreateSqlCommandByCommandType(commandType, commandText);
            DbConnection connection = this.GetNewConnection();
            try
            {
                PrepareCommand(sqlCommand, connection);
                return DoBeginExecuteReader(sqlCommand, true, callback, state);
            }
            catch(Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }

        public override IAsyncResult BeginExecuteReader(DbCommand command, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);
            DbConnection connection = this.GetNewConnection();
            try
            {
                PrepareCommand(sqlCommand, connection);
                return DoBeginExecuteReader(sqlCommand, false, callback, state);
            }
            catch(Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }

        public override IAsyncResult BeginExecuteReader(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);
            PrepareCommand(sqlCommand, transaction);
            return DoBeginExecuteReader(sqlCommand, false, callback, state);
        }

        public override IAsyncResult BeginExecuteReader(DbTransaction transaction, CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            SqlCommand sqlCommand = CreateSqlCommandByCommandType(commandType, commandText);
            PrepareCommand(sqlCommand, transaction);
            return DoBeginExecuteReader(sqlCommand, true, callback, state);
        }

        public override IAsyncResult BeginExecuteReader(DbTransaction transaction, string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(GetStoredProcCommand(storedProcedureName, parameterValues));
            PrepareCommand(sqlCommand, transaction);
            return DoBeginExecuteReader(sqlCommand, true, callback, state);
        }

        public override IAsyncResult BeginExecuteReader(string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(GetStoredProcCommand(storedProcedureName, parameterValues));
            DbConnection connection = this.GetNewConnection();
            try
            {
                PrepareCommand(sqlCommand, connection);
                return DoBeginExecuteReader(sqlCommand, true, callback, state);
            }
            catch(Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }

        private IAsyncResult DoBeginExecuteReader(SqlCommand command, bool disposeCommand, AsyncCallback callback, object state)
        {
            CommandBehavior commandBehavior =
                command.Transaction == null ? CommandBehavior.CloseConnection : CommandBehavior.Default;

            try
            {
                return WrappedAsyncOperation.BeginAsyncOperation(
                    callback,
                    cb => command.BeginExecuteReader(cb, state, commandBehavior),
                    ar => new DaabAsyncResult(ar, command, disposeCommand, false, DateTime.Now));
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        public override IDataReader EndExecuteReader(IAsyncResult asyncResult)
        {
            DaabAsyncResult daabAsyncResult = (DaabAsyncResult)asyncResult;
            SqlCommand command = (SqlCommand)daabAsyncResult.Command;
            try
            {
                IDataReader reader = command.EndExecuteReader(daabAsyncResult.InnerAsyncResult);
                //instrumentationProvider.FireCommandExecutedEvent(daabAsyncResult.StartTime);

                return reader;
            }
            catch (Exception ex)
            {
                //instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
                if (command.Transaction == null)
                {
                    // for a reader, the standard cleanup will not close the connection, so it needs to be closed
                    // in the catch block if necessary
                    command.Connection.Close();
                }
                throw ex;
            }
            finally
            {
                CleanupConnectionFromAsyncOperation(daabAsyncResult);
            }
        }

        public override IAsyncResult BeginExecuteScalar(CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            return BeginExecuteReader(commandType, commandText, callback, state);
        }

        public override IAsyncResult BeginExecuteScalar(DbCommand command, AsyncCallback callback, object state)
        {
            return BeginExecuteReader(command, callback, state);
        }

        public override IAsyncResult BeginExecuteScalar(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state)
        {
            return BeginExecuteReader(command, transaction, callback, state);
        }

        public override IAsyncResult BeginExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            return BeginExecuteReader(transaction, commandType, commandText, callback, state);
        }

        public override IAsyncResult BeginExecuteScalar(DbTransaction transaction, string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            return BeginExecuteReader(transaction, storedProcedureName, callback, state, parameterValues);
        }

        public override IAsyncResult BeginExecuteScalar(string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            return BeginExecuteReader(storedProcedureName, callback, state, parameterValues);
        }

        public override object EndExecuteScalar(IAsyncResult asyncResult)
        {
            using (IDataReader reader = EndExecuteReader(asyncResult))
            {
                if (!reader.Read() || reader.FieldCount == 0)
                {
                    return null;
                }
                return reader.GetValue(0);
            }
        }

        private static void CleanupConnectionFromAsyncOperation(DaabAsyncResult daabAsyncResult)
        {
            if (daabAsyncResult.DisposeCommand)
            {
                if (daabAsyncResult.Command != null)
                {
                    daabAsyncResult.Command.Dispose();
                }
            }
            if (daabAsyncResult.CloseConnection)
            {
                if (daabAsyncResult.Connection != null)
                {
                    daabAsyncResult.Connection.Close();
                }
            }
        }
    }
}
