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
            return (T)Resolve(typeInstances[typeof(T)].First());
        }

        public T Resolve<T>(string name)
        {
            return (T)Resolve(typeInstances[typeof(T)].FirstOrDefault((p) =>
            {
                if (p.Alias.Equals(name))
                    return true;
                return false;
            }));
        }
        #endregion

        private object Resolve(Component component)
        {
            component.LiftTimeScope.ComponentExecuted += componentfactory.MemberInfo;
            component.LiftTimeScope.ComponentExecuted += componentfactory.Methods;
            object obj=component.LiftTimeScope.CreateInstance(()=>{
                return componentfactory.CreateInstance(component);
            });
            component.LiftTimeScope.ComponentExecuted -= componentfactory.MemberInfo;
            component.LiftTimeScope.ComponentExecuted -= componentfactory.Methods;
            return obj;
        }


        public object Resolve(Type type)
        {
            return Resolve(typeInstances[type].First());
        }
    }
}
