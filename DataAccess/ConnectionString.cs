using System;

namespace LCW.Framework.Common.DataAccess
{
    public class ConnectionString
    {
        private string connectionString;
        private string userIdTokens;
        private string passwordTokens;

        public ConnectionString(string constr)
        {
            this.connectionString = constr;
        }

        public override string ToString()
        {
            return connectionString.ToString();
        }
    }
}
