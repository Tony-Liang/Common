using System;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;

namespace LCW.Framework.Common.DataAccess.Mysql
{
    public class MysqlDatabase : Database
    {
        public MysqlDatabase(string connectionString):
            base(connectionString,MySqlClientFactory.Instance)
        {
            
        }

        private static MySqlCommand CheckIfSqlCommand(DbCommand command)
        {
            MySqlCommand sqlCommand = command as MySqlCommand;
            if (sqlCommand == null) throw new ArgumentException("ExceptionCommandNotMySqlCommand", "command");
            return sqlCommand;
        }

        private static MySqlCommand CreateMySqlCommandByCommandType(CommandType commandType, string commandText)
        {
            return new MySqlCommand(commandText)
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

        private IAsyncResult DoBeginExecuteNonQuery(MySqlCommand command, bool disposeCommand, AsyncCallback callback, object state)
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
    }
}
