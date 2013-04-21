using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using LCW.Framework.Common.Genernation.DataBases.Entities;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Globalization;

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

        public override string OpenTable(string database, string table)
        {
            string message = string.Empty;
            using (SqlConnection connection = new SqlConnection(this.DbConnectionStringBuilder.ConnectionString))
            {
                Server server = new Server(new ServerConnection(connection));
                server.ConnectionContext.Connect();
                Table temp = server.Databases[database].Tables[table];
                //初始化Scripter 
                Scripter a = new Scripter(server);
                a.Options.Add(ScriptOption.DriAllConstraints);
                a.Options.Add(ScriptOption.DriAllKeys);
                a.Options.Add(ScriptOption.Default);
                a.Options.Add(ScriptOption.ContinueScriptingOnError);
                a.Options.Add(ScriptOption.ConvertUserDefinedDataTypesToBaseType);
                a.Options.Add(ScriptOption.IncludeIfNotExists);
                UrnCollection collection = new UrnCollection();
                collection.Add(temp.Urn);
                var sqls = a.Script(collection);
                foreach (var s in sqls)
                {
                    message += s;
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
            DataTable columns = new DataTable();
            columns.Locale = CultureInfo.CurrentCulture;
            using (SqlConnection connection = new SqlConnection(table.DbConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();                
                command.CommandText = string.Format(CultureInfo.CurrentCulture, "SELECT TOP 1 * FROM {0}", table.Name);
                using(IDataReader reader = command.ExecuteReader(CommandBehavior.KeyInfo))
                {
                    columns = reader.GetSchemaTable();
                    reader.Close();
                }
                #region
                //DataTable columns = connection.GetSchema(SqlClientMetaDataCollectionNames.Columns, new string[] { null, null, table.Name, null });
                //if (columns != null && columns.Rows.Count > 0)
                //{
                //    list = new List<ColumnEntity>();
                //    DataView dv = columns.DefaultView;
                //    dv.Sort = "ORDINAL_POSITION asc";
                //    foreach (DataRowView view in dv)
                //    {
                //        string name = string.Format("{0}", view["COLUMN_NAME"]);
                //        string description = string.Format("{0}({1})", view["COLUMN_NAME"], view["DATA_TYPE"]);
                //        ColumnEntity column = new ColumnEntity(table.DbConnectionStringBuilder,name);
                //        column.Description = description;
                //        column.DataType = DbType.SqlParse(view["DATA_TYPE"].ToString());
                //        column.Table = table;
                //        list.Add(column);
                //    }
                //}
                #endregion
            }
            if (columns != null)
            {
                list = new List<ColumnEntity>();
                foreach (DataRow row in columns.Rows)
                {
                    ColumnEntity column = new ColumnEntity(table.DbConnectionStringBuilder, row["ColumnName"].ToString());
                    column.Description = string.Format("{0}({1})", row["ColumnName"].ToString(), row["DataTypeName"].ToString());
                    column.AllowDBNull = bool.Parse(row["AllowDBNull"].ToString());
                    column.DataType = (Type)row["DataType"];
                    column.IsIdentity = (bool)row["IsIdentity"];
                    column.IsPrimaryKey = (bool)row["IsKey"];
                    column.IsReadOnly = (bool)row["IsReadOnly"];
                    column.IsUnique = (bool)row["IsUnique"];
                    column.Table = table;
                    list.Add(column);
                }
                CheckForeignKeys(table, list);
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

        private void CheckForeignKeys(TableEntity table,IList<ColumnEntity> Columns)
        {
            #region
            //string[] restrictions = new string[4] { null, null, table.Name, null };
            //DataTable foreignKeys = null;
            //DataTable tablekeys = null;
            //string text1 = string.Format(CultureInfo.CurrentCulture, "SELECT * FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE where table_name = '{1}'", table.Name);
            //using (SqlConnection connection = new SqlConnection(table.DbConnectionStringBuilder.ConnectionString))
            //{
            //    connection.Open();
            //    foreignKeys = connection.GetSchema("ForeignKeys", restrictions);

            //    SqlCommand command1 = new SqlCommand(text1,connection);
            //    using (IDataReader reader = command1.ExecuteReader())
            //    {
            //        tablekeys=reader.GetSchemaTable();
            //    }
            //}
                                  
            //if (foreignKeys != null && Columns != null)
            //{
            //    foreach (DataRow row in foreignKeys.Rows)
            //    {
            //        foreach (ColumnEntity c in Columns)
            //        {
            //            if (row[""].ToString().Equals(c.Name))
            //            {
            //                c.IsForeignKey = true;
            //                break;
            //            }
            //        }
            //    }
            //}
            #endregion
            StringBuilder text = new StringBuilder();
            text.Append("SELECT ");
            text.Append("Fk_Table = object_name(b.fkeyid) ,");
            text.Append("FK_Column = (SELECT name FROM syscolumns WHERE colid = b.fkey AND id = b.fkeyid) ,");
            text.Append("PK_Table   = object_name(b.rkeyid) ,");
            text.Append("PK_Column   = (SELECT name FROM syscolumns WHERE colid = b.rkey AND id = b.rkeyid) ,");
            text.Append("Cascade_Update   = ObjectProperty(a.id,'CnstIsUpdateCascade') ,");//级联更新
            text.Append("Cascade_Delete   = ObjectProperty(a.id,'CnstIsDeleteCascade') ");//级联删除
            text.Append("FROM sysobjects a ");
            text.Append("JOIN sysforeignkeys b ON a.id = b.constid ");
            text.Append("JOIN sysobjects c ON a.parent_obj = c.id ");
            text.Append("WHERE a.xtype = 'F' AND c.xtype = 'U' and object_name(b.fkeyid)='{0}'");
            DataTable list = new DataTable();
            string[] str = { "Fk_Table", "FK_Column", "PK_Table", "PK_Column", "Cascade_Update", "Cascade_Delete" };
            foreach (var s in str)
            {
                list.Columns.Add(new DataColumn(s));
            }
            using (SqlConnection connection = new SqlConnection(table.DbConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = string.Format(CultureInfo.CurrentCulture,text.ToString(), table.Name);
                using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        DataRow row = list.NewRow();
                        row[0] = reader[0].ToString();
                        row[1] = reader[1].ToString();
                        row[2] = reader[2].ToString();
                        row[3] = reader[3].ToString();
                        list.Rows.Add(row);
                    }
                }
            }
            if (list != null)
            {
                foreach (DataRow row in list.Rows)
                {
                    foreach (ColumnEntity temp in Columns)
                    {
                        if (temp.Name.Equals(row["FK_Column"].ToString()))
                        {
                            temp.IsForeignKey = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}
