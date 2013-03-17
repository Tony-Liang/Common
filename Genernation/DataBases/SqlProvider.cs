using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using LCW.Framework.Common.Genernation.DataBases.Entities;
using System.Data.SqlClient;
using System.Data;

namespace LCW.Framework.Common.Genernation.DataBases
{
    public class SqlProvider:ProviderBuilder
    {
        public SqlProvider(DbConnectionStringBuilder connectionstringbuilder):base(connectionstringbuilder)
        {

        }

        public override bool CheckConnection()
        {
            bool flag = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.DbConnectionStringBuilder.ConnectionString))
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
       
        public override string OpenProcedure(string procedure)
        {
            string message = string.Empty;
            using (SqlConnection connection = new SqlConnection(this.DbConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = string.Format("select text from syscomments where id=object_id('{0}')", procedure);//"select * FROM SysObjects where xtype='TR'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader != null && reader.HasRows)
                {
                    if (reader.Read())
                    {
                        message = reader["text"].ToString();
                    }
                }
            }
            return message;
        }

        public override string OpenTriggers(string triggers)
        {
            string message = string.Empty;
            using (SqlConnection connection = new SqlConnection(this.DbConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = string.Format("select text from syscomments where id=object_id('{0}')", triggers); //"exec sp_helptext " + triggers;//"select * FROM SysObjects where xtype='TR'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader != null && reader.HasRows)
                {
                    if (reader.Read())
                    {
                        message = reader["text"].ToString();
                    }
                }
            }
            return message;
        }

        public override string OpenView(string view)
        {
            string message = string.Empty;
            using (SqlConnection connection = new SqlConnection(this.DbConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = string.Format("SELECT text FROM syscomments,sysobjects WHERE sysobjects.xtype='V' and syscomments.id=sysobjects.id and  sysobjects.name='{0}'", view);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader != null && reader.HasRows)
                {
                    if (reader.Read())
                    {
                        message = reader["Text"].ToString();
                    }
                }
            }
            return message;
        }

        public override IList<DataBaseEntity> GetDataBases(ServiceSite site)
        {
            IList<DataBaseEntity> list = null;
            using (SqlConnection connection = new SqlConnection(site.DbConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                DataTable databases = connection.GetSchema(SqlClientMetaDataCollectionNames.Databases, new string[] { null });
                if (databases != null && databases.Rows.Count > 0)
                {
                    list = new List<DataBaseEntity>();
                    foreach (DataRow database in databases.Rows)
                    {
                        string name = (string)database["database_name"];
                        SqlConnectionStringBuilder temp = new SqlConnectionStringBuilder(site.DbConnectionStringBuilder.ConnectionString);
                        temp.InitialCatalog = name;
                        DataBaseEntity db = new DataBaseEntity(temp,name);
                        db.Service = site;
                        list.Add(db);
                    }
                }
            }
            return list;
        }

        public override IList<TableEntity> GetTables(DataBaseEntity database)
        {
            IList<TableEntity> list = null;
            using (SqlConnection connection = new SqlConnection(database.DbConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                DataTable tables = connection.GetSchema(SqlClientMetaDataCollectionNames.Tables, new string[] { null, null, null, "BASE TABLE" });
                if (tables != null && tables.Rows.Count > 0)
                {
                    list = new List<TableEntity>();
                    foreach (DataRow table in tables.Rows)
                    {
                        string name = string.Format("{0}", table["TABLE_NAME"]);
                        string des = string.Format("{0}.{1}", table["TABLE_SCHEMA"], table["TABLE_NAME"]);
                        TableEntity t = new TableEntity(database.DbConnectionStringBuilder,name, des);
                        t.DataBase = database;
                        list.Add(t);
                    }
                }
            }
            return list;
        }

        public override IList<ColumnEntity> GetColumns(TableEntity table)
        {
            IList<ColumnEntity> list = null;
            using (SqlConnection connection = new SqlConnection(table.DbConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                DataTable columns = connection.GetSchema(SqlClientMetaDataCollectionNames.Columns, new string[] { null, null, table.Name, null });
                if (columns != null && columns.Rows.Count > 0)
                {
                    list = new List<ColumnEntity>();
                    DataView dv = columns.DefaultView;
                    dv.Sort = "ORDINAL_POSITION asc";
                    foreach (DataRowView view in dv)
                    {
                        string name = string.Format("{0}", view["COLUMN_NAME"]);
                        string description = string.Format("{0}({1})", view["COLUMN_NAME"], view["DATA_TYPE"]);
                        ColumnEntity column = new ColumnEntity(table.DbConnectionStringBuilder,name);
                        column.Description = description;
                        column.DataType = DbType.SqlParse(view["DATA_TYPE"].ToString());
                        column.Table = table;
                        list.Add(column);
                    }
                }
            }
            return list;
        }

        public override IList<ViewEntity> GetViews(DataBaseEntity database)
        {
            IList<ViewEntity> list = null;
            using (SqlConnection connection = new SqlConnection(database.DbConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                DataTable views = connection.GetSchema(SqlClientMetaDataCollectionNames.Tables, new string[] { null, null, null, "View" });
                if (views != null && views.Rows.Count > 0)
                {
                    list = new List<ViewEntity>();
                    foreach (DataRow table in views.Rows)
                    {
                        string name = string.Format("{0}", table["TABLE_NAME"]);
                        string des = string.Format("{0}.{1}", table["TABLE_SCHEMA"], table["TABLE_NAME"]);
                        ViewEntity view = new ViewEntity(database.DbConnectionStringBuilder,name, des);
                        view.DataBase = database;
                        list.Add(view);
                    }
                }
            }
            return list;
        }

        public override IList<ProcedureEntity> GetProcedures(DataBaseEntity database)
        {
            IList<ProcedureEntity> list = null;
            using (SqlConnection connection = new SqlConnection(database.DbConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                DataTable procedures = connection.GetSchema(SqlClientMetaDataCollectionNames.Procedures, new string[] { null, null, null, "PROCEDURE" });
                if (procedures != null && procedures.Rows.Count > 0)
                {
                    list = new List<ProcedureEntity>();
                    foreach (DataRow procedure in procedures.Rows)
                    {
                        string name = string.Format("{0}", procedure["SPECIFIC_NAME"]);
                        string des = string.Format("{0}.{1}", procedure["SPECIFIC_SCHEMA"], procedure["SPECIFIC_NAME"]);
                        ProcedureEntity proc = new ProcedureEntity(database.DbConnectionStringBuilder,name, des);
                        proc.DataBase = database;
                        list.Add(proc);
                    }
                }
            }
            return list;
        }

        public override IList<TriggersEntity> GetTriggers(DataBaseEntity database)
        {
            IList<TriggersEntity> list = null;
            using (SqlConnection connection = new SqlConnection(database.DbConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from sys.triggers";//"select * FROM SysObjects where xtype='TR'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader != null && reader.HasRows)
                {
                    list = new List<TriggersEntity>();
                    while (reader.Read())
                    {
                        string name = reader["name"].ToString();
                        TriggersEntity tr = new TriggersEntity(database.DbConnectionStringBuilder, name, name);
                        tr.DataBase = database;
                        list.Add(tr);
                    }
                }
            }
            return list;
        }
    }
}
