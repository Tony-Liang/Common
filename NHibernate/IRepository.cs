using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using System.Collections;
using NHibernate.Linq;

namespace LCW.Framework.Common.NHibernate
{
    public interface IRepository
    {
        T Get<T>(object id);
        T Load<T>(object id);
        void Insert(object obj);
        void Update(object obj);
        void InsertOrUpdate(object obj);
        void Delete(object obj);
        void DeleteByQuery(string hql);
        void DeleteByQuery(string hql, string name, object value);
        void DeleteByQuery(string hql, string[] names, object[] values);
        void DeleteByQuery(string hql, object[] values);
        int Count<T>() where T : class;
        int Count<T>(ICriterion expression) where T : class;
        object Scalar(string hql);
        object Scalar(string hql, params object[] values);
        object Scalar(string hql, string name, object value);
        object Scalar(string hql, string[] names, object[] values);
        object Scalar(string hql, Action<IQuery> action);
        object ScalarBySQL(string sql);
        object ScalarBySQL(string sql, params object[] values);
        object ScalarBySQL(string sql, string name, object value);
        object ScalarBySQL(string sql, string[] names, object[] values);
        object ScalarBySQL(string sql, Action<ISQLQuery> action);
        IList<T> Query<T>() where T : class;
        IList<T> Query<T>(Action<ICriteria> action) where T : class;
        IList<T> Query<T>(ICriterion expression) where T : class;
        IList<T> Query<T>(ICriterion expression, Action<ICriteria> action) where T : class;
        IList<T> Query<T>(string hql);
        IList<T> Query<T>(string hql, params object[] values);
        IList<T> Query<T>(string hql, string name, object value);
        IList<T> Query<T>(string hql, string[] names, object[] values);
        IList<T> Query<T>(string hql, Action<IQuery> action);
        IList<T> Query<T>(int pageIndex, int pageSize, out int recordCount) where T : class;
        IList<T> Query<T>(ICriterion expression, int pageIndex, int pageSize, out int recordCount) where T : class;
        IList<T> Query<T>(ICriterion expression, Order[] order, int pageIndex, int pageSize, out int recordCount) where T : class;
        IList<T> Query<T>(string hql, object[] values, int pageIndex, int pageSize);
        IList<T> Query<T>(string hql, string name, object value, int pageIndex, int pageSize);
        IList<T> Query<T>(string hql, string[] names, object[] values, int pageIndex, int pageSize);
        IList<T> Query<T>(string hql, int pageIndex, int pageSize);
        IList QueryBySQL(string sql);
        IList QueryBySQL(string sql, params object[] values);
        IList QueryBySQL(string sql, string name, object[] value);
        IList QueryBySQL(string sql, string[] names, object[] values);
        IList QueryBySQL(string sql, Action<ISQLQuery> action);
        IList QueryBySQL(string sql, object[] values, int pageIndex, int pageSize);
        IList QueryBySQL(string sql, string name, object value, int pageIndex, int pageSize);
        IList QueryBySQL(string sql, string[] names, object[] values, int pageIndex, int pageSize);
        IList QueryBySQL(string sql, int pageIndex, int pageSize);
    }

    public interface IRepository<T> where T : class
    {
        T Get(object id);
        T Load(object id);
        void Insert(T obj);
        void Update(T obj);
        void InsertOrUpdate(T obj);

        void Delete(T obj);        
        void DeleteByQuery(string hql);
        void DeleteByQuery(string hql, string name, object value);
        void DeleteByQuery(string hql, string[] names, object[] values);
        void DeleteByQuery(string hql, object[] values);

        int Count();
        int Count(ICriterion expression);

        T Scalar(string hql);
        T Scalar(string hql, params object[] values);
        T Scalar(string hql, string name, object value);
        T Scalar(string hql, string[] names, object[] values);
        T Scalar(string hql, Action<IQuery> action);

        T ScalarBySQL(string sql);
        T ScalarBySQL(string sql, params object[] values);
        T ScalarBySQL(string sql, string name, object value);
        T ScalarBySQL(string sql, string[] names, object[] values);
        T ScalarBySQL(string sql, Action<ISQLQuery> action);

        IList<T> Query();
        IList<T> Query(Action<ICriteria> action);
        IList<T> Query(ICriterion expression);
        IList<T> Query(ICriterion expression, Action<ICriteria> action);

        IList<T> Query(string hql);
        IList<T> Query(string hql, params object[] values);
        IList<T> Query(string hql, string name, object value);
        IList<T> Query(string hql, string[] names, object[] values);
        IList<T> Query(string hql, Action<IQuery> action);

        IList<T> Query(int pageIndex, int pageSize, out int recordCount);
        IList<T> Query(ICriterion expression, int pageIndex, int pageSize, out int recordCount);
        IList<T> Query(ICriterion expression, Order[] order, int pageIndex, int pageSize, out int recordCount);

        IList<T> Query(string hql, object[] values, int pageIndex, int pageSize);
        IList<T> Query(string hql, string name, object value, int pageIndex, int pageSize);
        IList<T> Query(string hql, string[] names, object[] values, int pageIndex, int pageSize);
        IList<T> Query(string hql, int pageIndex, int pageSize);

        IList QueryBySQL(string sql);
        IList QueryBySQL(string sql, params object[] values);
        IList QueryBySQL(string sql, string name, object[] value);
        IList QueryBySQL(string sql, string[] names, object[] values);
        IList QueryBySQL(string sql, Action<ISQLQuery> action);

        IList QueryBySQL(string sql, object[] values, int pageIndex, int pageSize);
        IList QueryBySQL(string sql, string name, object value, int pageIndex, int pageSize);
        IList QueryBySQL(string sql, string[] names, object[] values, int pageIndex, int pageSize);
        IList QueryBySQL(string sql, int pageIndex, int pageSize);

        IQueryable<T> Find();
        IEnumerable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        IEnumerable<T> Query(CriterionRequest<T> criterion);
    }

    public abstract class RepositoryBase : IRepository
    {
        protected ISession session;

        protected ISession Session
        {
            get
            {
                return session;
            }
        }

        public T Get<T>(object id)
        {
            return session.Get<T>(id);
        }

        public T Load<T>(object id)
        {
            return session.Load<T>(id);
        }

        public void Insert(object obj)
        {
            session.Save(obj);
            session.Flush();
        }

        public void Update(object obj)
        {
            session.Update(obj);
            session.Flush();
        }

        public void InsertOrUpdate(object obj)
        {
            session.SaveOrUpdate(obj);
            session.Flush();
        }

        #region Delete

        /// <summary>
        /// 根据实体对象删除
        /// </summary>
        /// <param name="obj">实体对象</param>
        public void Delete(object obj)
        {
            session.Delete(obj);
            session.Flush();
        }

        /// <summary>
        /// 根据hql语句删除
        /// <example>
        /// hql="from 类名 where 属性名=值"
        /// </example>
        /// </summary>
        /// <param name="hql">hql语句</param>
        public void DeleteByQuery(string hql)
        {
            session.Delete(hql);
            session.Flush();
        }

        /// <summary>
        /// 根据Query进行删除
        /// <example>
        /// hql="delete from 类名 where 属性名=:参数名";
        /// </example>
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public void DeleteByQuery(string hql, string name, object value)
        {
            DeleteByQuery(hql, new string[] { name }, new object[] { value });
        }

        /// <summary>
        /// 根据Query进行删除
        /// <example>
        /// hql="delete from 类名 where 属性名=:参数名";
        /// </example>
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="names">参数名称数组</param>
        /// <param name="values">参数值数组</param>
        public void DeleteByQuery(string hql, string[] names, object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            query.ExecuteUpdate();
            session.Flush();
        }

        /// <summary>
        /// 根据Query进行删除
        /// <example>
        /// hql="delete from 类名 where 属性名=? and 属性名=？";
        /// </example>
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="values">参数值数组</param>
        public void DeleteByQuery(string hql, object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            query.ExecuteUpdate();
            session.Flush();
        }

        #endregion

        #region Count

        public int Count<T>() where T : class
        {
            ICriteria criteria = session.CreateCriteria<T>();
            return Convert.ToInt32(criteria.SetProjection(Projections.RowCount()).UniqueResult());
        }

        public int Count<T>(ICriterion expression) where T : class
        {
            ICriteria criteria = session.CreateCriteria<T>();
            if (expression != null)
            {
                criteria.Add(expression);
            }
            return Convert.ToInt32(criteria.SetProjection(Projections.RowCount()).UniqueResult());
        }

        #endregion

        #region Scalar

        public object Scalar(string hql)
        {
            return session.CreateQuery(hql).UniqueResult();
        }

        public object Scalar(string hql, params object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return query.UniqueResult();
        }

        public object Scalar(string hql, string name, object value)
        {
            return Scalar(hql, new string[] { name }, new object[] { value });
        }

        public object Scalar(string hql, string[] names, object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return query.UniqueResult();
        }

        public object Scalar(string hql, Action<IQuery> action)
        {
            IQuery query = session.CreateQuery(hql);
            action(query);
            return query.UniqueResult();
        }

        #endregion

        #region ScalarBySQL

        public object ScalarBySQL(string sql)
        {
            return session.CreateSQLQuery(sql).UniqueResult();
        }

        public object ScalarBySQL(string sql, params object[] values)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return query.UniqueResult();
        }

        public object ScalarBySQL(string sql, string name, object value)
        {
            return ScalarBySQL(sql, new string[] { name }, new object[] { value });
        }

        public object ScalarBySQL(string sql, string[] names, object[] values)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return query.UniqueResult();
        }

        public object ScalarBySQL(string sql, Action<ISQLQuery> action)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            action(query);
            return query.UniqueResult();
        }

        #endregion

        #region Query

        public IList<T> Query<T>() where T : class
        {
            return session.CreateCriteria<T>().List<T>();
        }

        public IList<T> Query<T>(Action<ICriteria> action) where T : class
        {
            ICriteria criteria = session.CreateCriteria<T>();
            action(criteria);
            return criteria.List<T>();
        }

        public IList<T> Query<T>(ICriterion expression) where T : class
        {
            return Query<T>(expression, null);
        }

        public IList<T> Query<T>(ICriterion expression, Action<ICriteria> action) where T : class
        {
            ICriteria criteria = session.CreateCriteria<T>();
            if (expression != null)
            {
                criteria.Add(expression);
            }
            if (action != null)
            {
                action(criteria);
            }
            return criteria.List<T>();
        }

        public IList<T> Query<T>(string hql)
        {
            return session.CreateQuery(hql).List<T>();
        }

        public IList<T> Query<T>(string hql, params object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return query.List<T>();
        }

        public IList<T> Query<T>(string hql, string name, object value)
        {
            return Query<T>(hql, new string[] { name }, new object[] { value });
        }

        public IList<T> Query<T>(string hql, string[] names, object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return query.List<T>();
        }

        public IList<T> Query<T>(string hql, Action<IQuery> action)
        {
            IQuery query = session.CreateQuery(hql);
            action(query);
            return query.List<T>();
        }

        #endregion

        #region Page Query

        public IList<T> Query<T>(int pageIndex, int pageSize, out int recordCount) where T : class
        {
            return Query<T>(null, null, pageIndex, pageSize, out recordCount);
        }

        public IList<T> Query<T>(ICriterion expression, int pageIndex, int pageSize, out int recordCount) where T : class
        {
            return Query<T>(expression, null, pageIndex, pageSize, out recordCount);
        }

        public IList<T> Query<T>(ICriterion expression, Order[] order,
            int pageIndex, int pageSize, out int recordCount) where T : class
        {
            IList<T> list = new List<T>();
            recordCount = 0;
            ICriteria query = session.CreateCriteria<T>();
            if (expression != null)
            {
                query.Add(expression);
            }
            ICriteria queryPage = CriteriaTransformer.Clone(query);
            //获取记录总数
            recordCount = Convert.ToInt32(query.SetProjection(Projections.RowCount()).UniqueResult());

            //设置排序
            if (order != null)
            {
                foreach (Order o in order)
                {
                    queryPage.AddOrder(o);
                }
            }
            queryPage.SetFirstResult((pageIndex - 1) * pageSize);
            queryPage.SetMaxResults(pageSize);
            list = queryPage.List<T>();

            return list;
        }

        public IList<T> Query<T>(string hql, object[] values, int pageIndex, int pageSize)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List<T>();
        }

        public IList<T> Query<T>(string hql, string name, object value, int pageIndex, int pageSize)
        {
            return Query<T>(hql, new string[] { name }, new object[] { value }, pageIndex, pageSize);
        }

        public IList<T> Query<T>(string hql, string[] names, object[] values, int pageIndex, int pageSize)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List<T>();
        }

        public IList<T> Query<T>(string hql, int pageIndex, int pageSize)
        {
            IQuery query = session.CreateQuery(hql);
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List<T>();
        }

        #endregion

        #region SQL Query

        public IList QueryBySQL(string sql)
        {
            return session.CreateSQLQuery(sql).List();
        }

        public IList QueryBySQL(string sql, params object[] values)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return query.List();
        }

        public IList QueryBySQL(string sql, string name, object[] value)
        {
            return QueryBySQL(sql, new string[] { name }, new object[] { value });
        }

        public IList QueryBySQL(string sql, string[] names, object[] values)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return query.List();
        }

        public IList QueryBySQL(string sql, Action<ISQLQuery> action)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            action(query);
            return query.List();
        }

        #endregion

        #region Page SQL Query

        public IList QueryBySQL(string sql, object[] values, int pageIndex, int pageSize)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List();
        }

        public IList QueryBySQL(string sql, string name, object value, int pageIndex, int pageSize)
        {
            return QueryBySQL(sql, new string[] { name }, new object[] { value }, pageIndex, pageSize);
        }

        public IList QueryBySQL(string sql, string[] names, object[] values, int pageIndex, int pageSize)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List();
        }

        public IList QueryBySQL(string sql, int pageIndex, int pageSize)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List();
        }

        #endregion
    }

    public abstract class RepositoryBase<T> : IRepository<T> where T:class
    {
        protected ISession session;

        protected ISession Session
        {
            get
            {
                return session;
            }
        }

        public T Get(object id)
        {
            return session.Get<T>(id);
        }

        public T Load(object id)
        {
            return session.Load<T>(id);
        }

        public void Insert(T obj)
        {
            session.Save(obj);
            session.Flush();
        }

        public void Update(T obj)
        {
            session.Update(obj);
            session.Flush();
        }

        public void InsertOrUpdate(T obj)
        {
            session.SaveOrUpdate(obj);
            session.Flush();
        }

        #region Delete

        /// <summary>
        /// 根据实体对象删除
        /// </summary>
        /// <param name="obj">实体对象</param>
        public void Delete(T obj)
        {
            session.Delete(obj);
            session.Flush();
        }

        /// <summary>
        /// 根据hql语句删除
        /// <example>
        /// hql="from 类名 where 属性名=值"
        /// </example>
        /// </summary>
        /// <param name="hql">hql语句</param>
        public void DeleteByQuery(string hql)
        {
            session.Delete(hql);
            session.Flush();
        }

        /// <summary>
        /// 根据Query进行删除
        /// <example>
        /// hql="delete from 类名 where 属性名=:参数名";
        /// </example>
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public void DeleteByQuery(string hql, string name, object value)
        {
            DeleteByQuery(hql, new string[] { name }, new object[] { value });
        }

        /// <summary>
        /// 根据Query进行删除
        /// <example>
        /// hql="delete from 类名 where 属性名=:参数名";
        /// </example>
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="names">参数名称数组</param>
        /// <param name="values">参数值数组</param>
        public void DeleteByQuery(string hql, string[] names, object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            query.ExecuteUpdate();
            session.Flush();
        }

        /// <summary>
        /// 根据Query进行删除
        /// <example>
        /// hql="delete from 类名 where 属性名=? and 属性名=？";
        /// </example>
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="values">参数值数组</param>
        public void DeleteByQuery(string hql, object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            query.ExecuteUpdate();
            session.Flush();
        }

        #endregion

        #region Count

        public int Count()
        {
            ICriteria criteria = session.CreateCriteria<T>();
            return Convert.ToInt32(criteria.SetProjection(Projections.RowCount()).UniqueResult());
        }

        public int Count(ICriterion expression)
        {
            ICriteria criteria = session.CreateCriteria<T>();
            if (expression != null)
            {
                criteria.Add(expression);
            }
            return Convert.ToInt32(criteria.SetProjection(Projections.RowCount()).UniqueResult());
        }

        #endregion

        #region Scalar

        public T Scalar(string hql)
        {
            return (T)session.CreateQuery(hql).UniqueResult();
        }

        public T Scalar(string hql, params object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return (T)query.UniqueResult();
        }

        public T Scalar(string hql, string name, object value)
        {
            return Scalar(hql, new string[] { name }, new object[] { value });
        }

        public T Scalar(string hql, string[] names, object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return (T)query.UniqueResult();
        }

        public T Scalar(string hql, Action<IQuery> action)
        {
            IQuery query = session.CreateQuery(hql);
            action(query);
            return (T)query.UniqueResult();
        }

        #endregion

        #region ScalarBySQL

        public T ScalarBySQL(string sql)
        {
            return (T)session.CreateSQLQuery(sql).UniqueResult();
        }

        public T ScalarBySQL(string sql, params object[] values)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return (T)query.UniqueResult();
        }

        public T ScalarBySQL(string sql, string name, object value)
        {
            return ScalarBySQL(sql, new string[] { name }, new object[] { value });
        }

        public T ScalarBySQL(string sql, string[] names, object[] values)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return (T)query.UniqueResult();
        }

        public T ScalarBySQL(string sql, Action<ISQLQuery> action)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            action(query);
            return (T)query.UniqueResult();
        }

        #endregion

        #region Query

        public IList<T> Query()
        {
            return session.CreateCriteria<T>().List<T>();
        }

        public IList<T> Query(Action<ICriteria> action)
        {
            ICriteria criteria = session.CreateCriteria<T>();
            action(criteria);
            return criteria.List<T>();
        }

        public IList<T> Query(ICriterion expression)
        {
            return Query(expression, null);
        }

        public IList<T> Query(ICriterion expression, Action<ICriteria> action)
        {
            ICriteria criteria = session.CreateCriteria<T>();
            if (expression != null)
            {
                criteria.Add(expression);
            }
            if (action != null)
            {
                action(criteria);
            }
            return criteria.List<T>();
        }

        public IList<T> Query(string hql)
        {
            return session.CreateQuery(hql).List<T>();
        }

        public IList<T> Query(string hql, params object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return query.List<T>();
        }

        public IList<T> Query(string hql, string name, object value)
        {
            return Query(hql, new string[] { name }, new object[] { value });
        }

        public IList<T> Query(string hql, string[] names, object[] values)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return query.List<T>();
        }

        public IList<T> Query(string hql, Action<IQuery> action)
        {
            IQuery query = session.CreateQuery(hql);
            action(query);
            return query.List<T>();
        }

        #endregion

        #region Page Query

        public IList<T> Query(int pageIndex, int pageSize, out int recordCount)
        {
            return Query(null, null, pageIndex, pageSize, out recordCount);
        }

        public IList<T> Query(ICriterion expression, int pageIndex, int pageSize, out int recordCount)
        {
            return Query(expression, null, pageIndex, pageSize, out recordCount);
        }

        public IList<T> Query(ICriterion expression, Order[] order,
            int pageIndex, int pageSize, out int recordCount)
        {
            IList<T> list = new List<T>();
            recordCount = 0;
            ICriteria query = session.CreateCriteria<T>();
            if (expression != null)
            {
                query.Add(expression);
            }
            ICriteria queryPage = CriteriaTransformer.Clone(query);
            //获取记录总数
            recordCount = Convert.ToInt32(query.SetProjection(Projections.RowCount()).UniqueResult());

            //设置排序
            if (order != null)
            {
                foreach (Order o in order)
                {
                    queryPage.AddOrder(o);
                }
            }
            queryPage.SetFirstResult((pageIndex - 1) * pageSize);
            queryPage.SetMaxResults(pageSize);
            list = queryPage.List<T>();

            return list;
        }

        public IList<T> Query(string hql, object[] values, int pageIndex, int pageSize)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List<T>();
        }

        public IList<T> Query(string hql, string name, object value, int pageIndex, int pageSize)
        {
            return Query(hql, new string[] { name }, new object[] { value }, pageIndex, pageSize);
        }

        public IList<T> Query(string hql, string[] names, object[] values, int pageIndex, int pageSize)
        {
            IQuery query = session.CreateQuery(hql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List<T>();
        }

        public IList<T> Query(string hql, int pageIndex, int pageSize)
        {
            IQuery query = session.CreateQuery(hql);
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List<T>();
        }

        #endregion

        #region SQL Query

        public IList QueryBySQL(string sql)
        {
            return session.CreateSQLQuery(sql).List();
        }

        public IList QueryBySQL(string sql, params object[] values)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return query.List();
        }

        public IList QueryBySQL(string sql, string name, object[] value)
        {
            return QueryBySQL(sql, new string[] { name }, new object[] { value });
        }

        public IList QueryBySQL(string sql, string[] names, object[] values)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return query.List();
        }

        public IList QueryBySQL(string sql, Action<ISQLQuery> action)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            action(query);
            return query.List();
        }

        #endregion

        #region Page SQL Query

        public IList QueryBySQL(string sql, object[] values, int pageIndex, int pageSize)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < values.Length; i++)
            {
                query.SetParameter(i, values[i]);
            }
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List();
        }

        public IList QueryBySQL(string sql, string name, object value, int pageIndex, int pageSize)
        {
            return QueryBySQL(sql, new string[] { name }, new object[] { value }, pageIndex, pageSize);
        }

        public IList QueryBySQL(string sql, string[] names, object[] values, int pageIndex, int pageSize)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            for (int i = 0; i < names.Length; i++)
            {
                query.SetParameter(names[i], values[i]);
            }
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List();
        }

        public IList QueryBySQL(string sql, int pageIndex, int pageSize)
        {
            ISQLQuery query = session.CreateSQLQuery(sql);
            return query.SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List();
        }

        #endregion


        public IQueryable<T> Find()
        {
            return session.Query<T>();
        }

        public IEnumerable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return session.Query<T>().Where(predicate).ToList<T>();
        }

        public IEnumerable<T> Query(CriterionRequest<T> criterion)
        {
            ICriteria query = session.CreateCriteria<T>();
            if (criterion.subCriteria != null)
            {
                foreach (var c in criterion.subCriteria.AllKeys)
                {
                    query.CreateCriteria(c, criterion.subCriteria[c]);
                }
            }

            if (criterion.Expressions != null)
            {
                foreach (var exp in criterion.Expressions)
                {
                    if(exp!=null)
                        query.Add(exp.Compile().Invoke());
                }
            }
            ICriteria queryPage = CriteriaTransformer.Clone(query);
            //获取记录总数
            criterion.Totals = Convert.ToInt32(query.SetProjection(Projections.RowCount()).UniqueResult());

            //设置排序
            if (criterion.Orders != null)
            {
                foreach (var o in criterion.Orders)
                {
                    if(o!=null)
                        queryPage.AddOrder(o.Compile().Invoke());
                }
            }
            queryPage.SetFirstResult((criterion.CurrentPage - 1) * criterion.PageSize);
            queryPage.SetMaxResults(criterion.PageSize);
            criterion.DataList = queryPage.List<T>();

            return criterion.DataList;
        } 
    }
}
