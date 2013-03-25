using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LCW.Framework.Common.Ioc
{
    public abstract class ComponentFactory
    {
        private IContainer container;
        public IContainer Container
        {
            get
            {
                return container;
            }
            internal set
            {
                container = value;
            }
        }
        public virtual object CreateInstance(Component component)
        {
            return null;
        }

        public virtual void MemberInfo(object obj)
        {

        }

        public virtual void Methods(object obj)
        {
        }
    }

    public class ILFactory : ComponentFactory
    {

    }

    public class ReferenceFactory : ComponentFactory
    {
        public override object CreateInstance(Component component)
        {
            object obj = null;
            Type target = component.BindingTarget.Type;
            ConstructorInfo construct=target.GetConstructors().First();
            try
            {
                if (construct.GetParameters().Count() > 0)
                {
                    object[] argments = construct.GetParameters().Select((p) => Container.Resolve(p.ParameterType)).ToArray();
                    obj= construct.Invoke(argments);
                }
                else
                {
                    obj= Activator.CreateInstance(component.BindingTarget.Type);
                }
            }
            catch (Exception ex)
            {
                Ensure.ArgumentNotNull(ex.Message, "");
            }
            return obj;
        }

        public override void MemberInfo(object obj)
        {
            base.MemberInfo(obj);
        }

        public override void Methods(object obj)
        {
            base.Methods(obj);
        }
    }
}
