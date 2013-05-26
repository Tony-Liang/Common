using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Specialized;

namespace LCW.Framework.Common.NHibernate
{
    public class CriterionRequest
    {
        public object DataList
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public int CurrentPage
        {
            get;
            set;
        }

        public int Totals
        {
            get;
            set;
        }

        private IList<System.Linq.Expressions.Expression<Func<ICriterion>>> expressions = new List<System.Linq.Expressions.Expression<Func<ICriterion>>>();

        public System.Linq.Expressions.Expression<Func<ICriterion>>[] Expressions
        {
            get
            {
                return expressions.ToArray();
            }
        }

        private IList<System.Linq.Expressions.Expression<Func<Order>>> orders = new List<System.Linq.Expressions.Expression<Func<Order>>>();
        public System.Linq.Expressions.Expression<Func<Order>>[] Orders
        {
            get
            {
                return orders.ToArray();
            }
        }

        internal NameValueCollection subCriteria = new NameValueCollection();

        public CriterionRequest CreateCriteria(string associationPath, string alias)
        {
            if (!string.IsNullOrEmpty(associationPath))
                subCriteria.Add(associationPath, alias);
            return this;
        }

        public CriterionRequest Add(System.Linq.Expressions.Expression<Func<ICriterion>> expression)
        {
            expressions.Add(expression);
            return this;
        }

        public CriterionRequest AddOrder(System.Linq.Expressions.Expression<Func<Order>> order)
        {
            orders.Add(order);
            return this;
        }
    }

    public class CriterionRequest<T> : CriterionRequest
    {
        public new IEnumerable<T> DataList
        {
            get;
            set;
        }
    }
}
