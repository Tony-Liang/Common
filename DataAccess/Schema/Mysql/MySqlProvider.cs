﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;

namespace LCW.Framework.Common.DataAccess.Schema.Mysql
{
    public class MySqlProvider:SchemaProviderBase
    {
        internal MySqlProvider()
        {

        }

        public override IList<DataBaseSchema> GetDataBases(System.Data.Common.DbConnectionStringBuilder connectionstr)
        {
            IList<DataBaseSchema> list = null;
            using (MySqlConnection connection = new MySqlConnection(connectionstr.ConnectionString))
            {
                connection.Open();
                DataTable databases = connection.GetSchema(SqlClientMetaDataCollectionNames.Databases, connectionstr.DataBase());
                if (databases != null && databases.Rows.Count > 0)
                {
                    list = new List<DataBaseSchema>();
                    foreach (DataRow database in databases.Rows)
                    {
                        string name = (string)database["database_name"];
                        DataBaseSchema db = new DataBaseSchema(name, name);
                        list.Add(db);
                    }
                }
            }
            return list;
        }

        public override IList<TableSchema> GetTables(System.Data.Common.DbConnectionStringBuilder connectionstr)
        {
            IList<TableSchema> list = null;
            using (MySqlConnection connection = new MySqlConnection(connectionstr.ConnectionString))
            {
                connection.Open();
                DataTable tables = connection.GetSchema(SqlClientMetaDataCollectionNames.Tables, new string[] { null, null, null, "BASE TABLE" });
                if (tables != null && tables.Rows.Count > 0)
                {
                    list = new List<TableSchema>();
                    foreach (DataRow table in tables.Rows)
                    {
                        string name = string.Format("{0}", table["TABLE_NAME"]);
                        string des = string.Format("{0}.{1}", table["TABLE_SCHEMA"], table["TABLE_NAME"]);
                        TableSchema t = new TableSchema(name, des);
                        list.Add(t);
                    }
                }
            }
            return list;
        }

        public override IList<ViewSchema> GetViews(System.Data.Common.DbConnectionStringBuilder connectionstr)
        {
            IList<ViewSchema> list = null;
            using (MySqlConnection connection = new MySqlConnection(connectionstr.ConnectionString))
            {
                connection.Open();
                DataTable views = connection.GetSchema(SqlClientMetaDataCollectionNames.Views, new string[] { null, null, null,null});
                if (views != null && views.Rows.Count > 0)
                {
                    list = new List<ViewSchema>();
                    foreach (DataRow table in views.Rows)
                    {
                        string name = string.Format("{0}", table["TABLE_NAME"]);
                        string des = string.Format("{0}.{1}", table["TABLE_SCHEMA"], table["TABLE_NAME"]);
                        ViewSchema view = new ViewSchema(name,des);
                        list.Add(view);
                    }
                }
            }
            return list;
        }

        public override IList<ProceduresSchema> GetProcedures(System.Data.Common.DbConnectionStringBuilder connectionstr)
        {
            IList<ProceduresSchema> list = null;
            using (MySqlConnection connection = new MySqlConnection(connectionstr.ConnectionString))
            {
                connection.Open();
                DataTable procedures = connection.GetSchema(SqlClientMetaDataCollectionNames.Procedures, new string[] { null, null, null, "PROCEDURE" });
                if (procedures != null && procedures.Rows.Count > 0)
                {
                    list = new List<ProceduresSchema>();
                    foreach (DataRow procedure in procedures.Rows)
                    {
                        string name = string.Format("{0}",procedure["SPECIFIC_NAME"]);
                        string des = string.Format("{0}.{1}", procedure["ROUTINE_SCHEMA"], procedure["SPECIFIC_NAME"]);
                        ProceduresSchema proc = new ProceduresSchema(name,des);
                        list.Add(proc);
                    }
                }
            }
            return list;
        }

        public override IList<TriggersSchema> GetTriggers(System.Data.Common.DbConnectionStringBuilder connectionstr)
        {
            IList<TriggersSchema> list = null;
            using (MySqlConnection connection = new MySqlConnection(connectionstr.ConnectionString))
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SHOW TRIGGERS";//"select * FROM SysObjects where xtype='TR'";
                cmd.CommandType = CommandType.Text;
                MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader != null && reader.HasRows)
                {
                    list = new List<TriggersSchema>();
                    while (reader.Read())
                    {
                        string name = reader["Trigger"].ToString();
                        list.Add(new TriggersSchema(name, name));
                    }
                }  
            }
            return list;
        }

        public override IList<ColumnSchema> GetColumns(System.Data.Common.DbConnectionStringBuilder connectionstr, string tablename)
        {
            IList<ColumnSchema> list = null;
            using (MySqlConnection connection = new MySqlConnection(connectionstr.ConnectionString))
            {
                connection.Open();
                DataTable columns = connection.GetSchema(SqlClientMetaDataCollectionNames.Columns, new string[] { null, null, tablename, null });
                if (columns != null && columns.Rows.Count > 0)
                {
                    list = new List<ColumnSchema>();
                    DataView dv = columns.DefaultView;
                    dv.Sort = "ORDINAL_POSITION asc";
                    foreach (DataRowView table in dv)
                    {
                        string name = string.Format("{0}({1})", table["COLUMN_NAME"], table["DATA_TYPE"]);
                        ColumnSchema column = new ColumnSchema(name);
                        list.Add(column);
                    }
                }
            }
            return list;
        }

        public override bool CheckConnection(System.Data.Common.DbConnectionStringBuilder connectionstr)
        {
            bool flag = false;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionstr.ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        flag = true;
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        public override string OpenProcedure(System.Data.Common.DbConnectionStringBuilder connectionstr,string procedure)
        {
            string message = string.Empty;
            using (MySqlConnection connection = new MySqlConnection(connectionstr.ConnectionString))
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "show create procedure "+procedure;
                cmd.CommandType = CommandType.Text;
                MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader != null && reader.HasRows)
                {
                    if (reader.Read())
                    {
                        message = reader["Create Procedure"].ToString();
                    }
                }
            }
            return message;
        }

        public override string OpenTriggers(System.Data.Common.DbConnectionStringBuilder connectionstr,string triggers)
        {
            string message = string.Empty;
            using (MySqlConnection connection = new MySqlConnection(connectionstr.ConnectionString))
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SHOW CREATE TRIGGER " + triggers;
                cmd.CommandType = CommandType.Text;
                MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader != null && reader.HasRows)
                {
                    if (reader.Read())
                    {
                        message = reader["SQL Original Statement"].ToString();
                    }
                }
            }
            return message;
        }

        public override string OpenView(System.Data.Common.DbConnectionStringBuilder connectionstr,string view)
        {
            string message = string.Empty;
            using (MySqlConnection connection = new MySqlConnection(connectionstr.ConnectionString))
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SHOW CREATE View " + view;
                cmd.CommandType = CommandType.Text;
                MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader != null && reader.HasRows)
                {
                    if (reader.Read())
                    {
                        message = reader["Create View"].ToString();
                    }
                }
            }
            return message;
        }      
    }
}
