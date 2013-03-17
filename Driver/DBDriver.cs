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
using System.Collections.ObjectModel;
namespace LCW.Framework.Common.Driver
{
    public sealed class DBDriverProvice
    {
        private List<DBDriver> drivers = new List<DBDriver>();
        private LanguagesCollection languages = new LanguagesCollection();
        public IList<DBDriver> Drivers
        {
            get
            {
                return drivers.AsReadOnly();
            }
        }

        public LanguagesCollection Languages
        {
            get
            {
                return languages;
            }
        }

        private DBDriverProvice()
        {
            init_driver();
            init_dbtype();
            init_languages();
        }

        private void init_driver()
        {
            drivers.Clear();
            var xml = new XmlDocument();
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
        private void init_dbtype()
        {
            var xml = new XmlDocument();
            using (var stream = ResourcesHelper.ReadResources<DBDriverProvice>("Setting.DbTargets.xml"))
            {
                xml.Load(stream);
            }
            var root = xml.SelectSingleNode("/DbTargets");
            if (root.HasChildNodes)
            {

            }
        }
        private void init_languages()
        {
            try
            {
                var xml = new XmlDocument();
                using (var stream = ResourcesHelper.ReadResources<DBDriverProvice>("Setting.Languages.xml"))
                {
                    xml.Load(stream);
                }
                var root = xml.SelectSingleNode("/Languages");
                if (root.HasChildNodes)
                {
                    foreach (XmlNode item in root.ChildNodes)
                    {
                        string key = item.Attributes["From"].Value;
                        string to = item.Attributes["To"].Value;
                        LanguagesWrapper w=languages.Add(key, to);
                        if (item.HasChildNodes)
                        {
                            foreach (XmlNode child in item)
                            {
                                w.languages.Add(new LanguagesType(child.Attributes["From"].Value, child.Attributes["To"].Value));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static DBDriverProvice GetInstance
        {
            get
            {
                return ProviceWrapper.provice;
            }
        }

        #region Nested
        class ProviceWrapper
        {
            static ProviceWrapper()
            {

            }
            internal static readonly DBDriverProvice provice = new DBDriverProvice();
        }
        #endregion
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

    [Serializable]
    public class LanguagesCollection
    {
        private List<LanguagesWrapper> collection;
        public LanguagesCollection()
        {
            collection=new List<LanguagesWrapper>();
        }

        public LanguagesWrapper Add(string from, string to)
        {
            LanguagesWrapper w = new LanguagesWrapper(from, to);
            collection.Add(w);
            return w;
        }

        public LanguagesWrapper ContainsKey(string from,string to)
        {
            foreach (LanguagesWrapper w in collection)
            {
                if (w.Form == from && w.To == to)
                    return w;
            }
            return null;
        }
    }
    [Serializable]
    public class LanguagesWrapper
    {
        internal LanguagesWrapper(string form, string to)
        {
            this.form = form;
            this.to = to;
            this.languages = new List<LanguagesType>();
        }
        
        private string form;
        public string Form
        {
            get
            {
                return this.form;
            }
        }

        private string to;
        public string To
        {
            get
            {
                return to;
            }
        }

        internal IList<LanguagesType> languages;
        public IList<LanguagesType> Languages
        {
            get
            {
                return new ReadOnlyCollection<LanguagesType>(languages);
            }
        }
    }
    [Serializable]
    public class LanguagesType
    {
        internal LanguagesType(string form,string to)
        {
            this.form=form;
            this.to=to;
        }
        private string form;
        public string Form
        {
            get
            {
                return this.form;
            }
        }

        private string to;
        public Type To
        {
            get
            {
                if(!string.IsNullOrEmpty(to))
                {
                    try
                    {
                        return Type.GetType(to,false);
                    }
                    catch(TypeLoadException ex)
                    {

                    }
                }
                return null;
            }
        }
    }
}
