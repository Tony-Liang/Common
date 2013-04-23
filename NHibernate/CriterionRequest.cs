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

        public IEnumerable<CriterionAssociations> Associations
        {
            get;
            set;
        }
    }

    public class CriterionAssociations
    {
        public CriterionAssociations(string associationPath, string alias)
        {
            this.associationPath = associationPath;
            this.alias = alias;
        }

        public CriterionAssociations(string associationPath)
            : this(associationPath, associationPath)
        {
        }

        private string associationPath;
        public string AssociationPath
        {
            get
            {
                return associationPath;
            }
        }

        private string alias;
        public string Alias
        {
            get
            {
                return alias;
            }
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
}
