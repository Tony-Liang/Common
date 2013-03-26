using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LCW.Framework.Common.Ioc
{
    internal class Container : IContainer
    {
        internal readonly Multimap<Type, Component> typeInstances = new Multimap<Type, Component>();

        private ComponentFactory componentfactory;

        public Container(ComponentFactory componentfactory)
        {
            this.componentfactory = componentfactory;
            this.componentfactory.Container = this;
        }

        #region IContainer 成员
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public T Resolve<T>(string name)
        {
			return (T)Resolve(typeof(T),name);
        }
        #endregion

        private object Resolve(Component component)
        {
			Executed MemberInfo = (temp) =>
			{
				componentfactory.MemberInfo(temp,component.BindingTarget.Property);
			};
			Executed Methods = (temp) =>
			{
				componentfactory.Methods(temp,component.BindingTarget.Method);
			};
			component.LiftTimeScope.ComponentExecuted += MemberInfo;
			component.LiftTimeScope.ComponentExecuted += Methods;
            object obj=component.LiftTimeScope.CreateInstance(()=>{
                return componentfactory.CreateInstance(component);
            });
			component.LiftTimeScope.ComponentExecuted -= MemberInfo;
			component.LiftTimeScope.ComponentExecuted -= Methods;
            return obj;
        }

        public object Resolve(Type type)
        {
            return Resolve(typeInstances[type].First());
        }

		#region IResolve 成员


		public object Resolve(Type type,string name)
		{
			return Resolve(typeInstances[type].FirstOrDefault((p) =>
			{
				if(p.Alias.Equals(name))
					return true;
				return false;
			}));
		}

		#endregion

		#region IResolve 成员


		public K Resolve<V,K>() where K: V
		{
			return (K)Resolve(typeof(V),typeof(K));
		}

		public object Resolve(Type V,Type K)
		{
			return Resolve(typeInstances[V].Where((p) =>
			{
				if(p.BindingTarget.Type.Equals(K))
					return true;
				return false;
			}).First());
		}

		#endregion
	}
}
