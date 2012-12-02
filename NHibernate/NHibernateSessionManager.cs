using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using System.Web;
using NHibernate.Context;
using System.IO;
using NHibernate.Cfg.MappingSchema;

namespace LCW.Framework.Common.NHibernate
{
    public class NHibernateSessionManager
    {
        public static ISessionFactory SessionFactory=null;

        //static NHibernateSessionManager()
        //{
        //    try
        //    {
        //        Configuration cfg = new Configuration();
        //        if (SessionFactory != null)
        //            throw new Exception("trying to init SessionFactory twice!");
        //        SessionFactory = cfg.Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Config/Mysql.config"))
        //                            .AddMapping()
        //                            .BuildSessionFactory();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Error.WriteLine(ex);
        //        throw new Exception("NHibernate initialization failed", ex);
        //    }
        //}

        public static void init(HbmMapping mapping)
        {
            try
            {
                Configuration cfg = new Configuration();
                if (SessionFactory != null)
                    throw new Exception("trying to init SessionFactory twice!");
                cfg.Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config/Mysql.config")).AddMapping(mapping);
                SessionFactory = cfg.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                throw new Exception("NHibernate initialization failed", ex);
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
    //NHibernateSessionManager.SessionFactory.GetCurrentSession()

    public class NHibernateSessionPerRequestModule : IHttpModule
    {
        private HttpApplication app;
        public void Dispose()
        {
            app.BeginRequest -= context_BeginRequest;
            app.BeginRequest -= context_EndRequest;
        }

        public void Init(HttpApplication context)
        {
            app = context;
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            ISession session = NHibernateSessionManager.OpenSession();
            session.BeginTransaction();
            CurrentSessionContext.Bind(session);
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            ISession session = CurrentSessionContext.Unbind(NHibernateSessionManager.SessionFactory);
            if (session != null)
            {
                try
                {
                    session.Transaction.Commit();
                }
                catch (Exception)
                {
                    session.Transaction.Rollback();
                }
                finally
                {
                    session.Close();
                }
            }
        }
    }
}
