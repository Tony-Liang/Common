using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LCW.Framework.Common.Serialization;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml;
using DotNet.Framework.Common.Xml;
using LCW.Framework.Common.Resources;
namespace LCW.Framework.Common.Driver
{
    public class DBDriverProvice
    {
        private List<DBDriver> drivers = new List<DBDriver>();
        public IList<DBDriver> Drivers
        {
            get
            {
                return drivers.AsReadOnly();
            }
        }

        private static DBDriverProvice dbprovice;
        private DBDriverProvice()
        {
            drivers.Clear();
            var xml=new XmlDocument();
            using (var stream = ResourcesHelper.ReadResources<DBDriverProvice>("Setting.driver.xml"))
            {
                xml.Load(stream);
            }
            var root = xml.SelectSingleNode("/Drivers");
            if (root.HasChildNodes)
            {
                foreach (XmlNode item in root.ChildNodes)
                {
                    if (item != null && item.HasChildNodes)
                    {
                        DBDriver driver = new DBDriver();
                        foreach (XmlNode children in item.ChildNodes)
                        {
                            if (children.Name == "ConnectionString")
                            {
                                driver.ConnectionString = children.InnerText;
                            }
                            else if (children.Name == "Description")
                            {
                                driver.Description = children.InnerText;
                            }
                        }
                        drivers.Add(driver);
                    }
                }
            }
        }
        public static DBDriverProvice GetInstance()
        {
            if (dbprovice == null)
            {
                dbprovice = new DBDriverProvice();
            }
            return dbprovice;
        }
    }

    [Serializable]
    public class DBDriver
    {
        internal DBDriver()
        {

        }
        internal DBDriver(string description,string connectionstring)
        {
            this.Description = description;
            this.ConnectionString = connectionstring;
        }
        public string Description { get;  internal set; }
        public string ConnectionString { get;  internal set; }
    }
}
