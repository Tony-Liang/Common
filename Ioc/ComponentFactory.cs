using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LCW.Framework.Common.Ioc
{
    internal abstract class ComponentFactory
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

		public virtual void MemberInfo(object obj,InjectProperty[] InjectPropertys)
        {

        }

		public virtual void Methods(object obj,InjectMethod[] InjectMethods)
        {
        }
    }

	internal class ILFactory: ComponentFactory
    {

    }

	internal class ReferenceFactory: ComponentFactory
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
					object[] argments = construct.GetParameters().Select((p) =>
					{
						var com=component.Dependent.Find(g=>g.Target==p.ParameterType);
						if(com != null)
							return Container.Resolve(p.ParameterType,com.Alias);
						else
							return Container.Resolve(p.ParameterType);
					}).ToArray();
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

		public override void MemberInfo(object obj,InjectProperty[] InjectPropertys)
        {
			if(InjectPropertys != null && InjectPropertys.Count() > 0)
			{
				foreach(var property in InjectPropertys)
				{
					PropertyInfo propertyInfo = obj.GetType().GetProperty(property.Name,BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance);
					if(propertyInfo != null && propertyInfo.CanWrite)
					{
						object tempobj = null;
						if(property.Value == null)
						{
							if(property.Source != null)
								tempobj = Container.Resolve(property.Target,property.Source);
							else if(property.Target != null)
								tempobj = Container.Resolve(property.Target);
							else
								tempobj = Container.Resolve(propertyInfo.PropertyType);
						}
						else
						{
							tempobj = property.Value;
						}
						propertyInfo.SetValue(obj,tempobj,null);
					}
				}
			}
        }

		public override void Methods(object obj,InjectMethod[] InjectMethods)
        {
			if(InjectMethods != null && InjectMethods.Count() > 0)
			{
				foreach(var m in InjectMethods)
				{
					MethodInfo methodInfo = obj.GetType().GetMethod(m.Name,BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
					methodInfo.Invoke(obj,m.Argments);
				}
			}
        }
    }

	public abstract class AbstractInject
	{
		private string name;
		public virtual string Name
		{
			get
			{
				return name;
			}
		}

		public AbstractInject(string name)
		{
			this.name = name;
		}
	}

	public class InjectProperty: AbstractInject
	{
		private Type target;
		private Type source;
		private object value;
		public Type Target
		{
			get
			{
				return target;
			}
		}
		public Type Source
		{
			get
			{
				return source;
			}
		}
		public object Value
		{
			get
			{
				return value;
			}
		}

		public InjectProperty(string name,Type target):this(name)
		{
			this.target = target;
		}

		public InjectProperty(string name,Type TContract,Type TImplementation)
			: this(name)
		{
			target = TContract;
			source = TImplementation;
		}

		public InjectProperty(string name,object value):this(name)			
		{
			this.value = value;
		}

		public InjectProperty(string name)
			: base(name)
		{

		}
	}

	public class InjectMethod: AbstractInject
	{
		public InjectMethod(string name)
			: base(name)
		{

		}

		public InjectMethod(string name,params object[] argments)
			: base(name)
		{
			this.argments = argments;
		}

		private object[] argments;
		public object[] Argments
		{
			get
			{
				return argments;
			}
		}
	}
}
