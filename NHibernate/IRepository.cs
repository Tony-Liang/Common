using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using NHibernate;

namespace LCW.Framework.Common.NHibernate
{
    public interface IRepository
    {
        ISession NHSsession { get; }
        /// <summary>
        /// Loads a proxy object with nothing but the primary key set.  
        /// Other properties will be pulled from the DB the first time they are accessed.
        /// Generally only use when you know you will NOT be wanting the other properties though.
        /// </summary>
        T Load<T>(object primaryKey);
        T Get<T>(object primaryKey);
        T Get<T>(Expression<Func<T, bool>> predicate);
        IQueryable<T> Find<T>();
        IQueryable<T> Find<T>(Expression<Func<T, bool>> predicate);
        T Add<T>(T entity);
        T Remove<T>(T entity);
    }
}
