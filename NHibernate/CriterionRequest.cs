using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

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

        public System.Linq.Expressions.Expression<Func<ICriterion>>[] Criterias
        {
            get;
            set;
        }

        public System.Linq.Expressions.Expression<Func<Order>>[] Orders
        {
            get;
            set;
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
