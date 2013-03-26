using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LCW.Framework.Common.Ioc.LiftTimeScope;
using LCW.Framework.Common.Ioc;

namespace LCW.Framework.Common.Ioc
{
    internal class Component
    {
        private Type target;
        public Type Target
        {
            get
            {
                return this.target;
            }
        }

        private ILiftTimeScope liftTimeScope;
        public ILiftTimeScope LiftTimeScope
        {
            get
            {
                return this.liftTimeScope;
            }
        }

        private IBindingTarget bingingTarget;
        public IBindingTarget BindingTarget
        {
            get
            {
                return this.bingingTarget;
            }
        }

        private string name;
        public string Alias
        {
            get
            {
                return this.name;
            }
        }

		private List<Component> dependent=new List<Component>();
		public List<Component> Dependent
		{
			get
			{
				return dependent;
			}
		}

		internal Component( Type type,
							ILiftTimeScope liftTimeScope,
							string name,
							IBindingTarget bingingTarget
							)
        {
            this.target = type;
			this.name = name;
            this.bingingTarget = bingingTarget;
			this.liftTimeScope = liftTimeScope;
        }      
    }

    public delegate void Executed(object sender);
}


internal class ComponentFacade: IComponent,IDisposable
{
	private Type target;

	private ILiftTimeScope liftTimeScope;

	private Type source;

	private string name;

	private List<Type> injects = new List<Type>();
	private List<InjectProperty> propertys = new List<InjectProperty>();
	private List<InjectMethod> methods = new List<InjectMethod>();

	#region IComponent 成员

	public IComponent Implement<T>()
	{
		source = typeof(T);
		return this;
	}

	public IComponent Name(string name)
	{
		this.name = name;
		return this;
	}

	public IComponent To<T>()
	{
		injects.Add(typeof(T));
		return this;
	}

	#endregion
	public ComponentFacade(Type TContract)
	{
		source=target = TContract;
		name = Guid.NewGuid().ToString();
	}

	public ComponentFacade(Type TContract,Type TImplementation):this(TContract)
	{
		source = TImplementation;
	}

	public Component CreateComponent(Multimap<Type,Component> dependents)
	{
		if(liftTimeScope == null)  
			liftTimeScope=new TransientScope();

		InjectProperty[] propertyarray = propertys.ToArray();
		InjectMethod[] methodsarray = methods.ToArray();

		var component = new Component(target,liftTimeScope,name,new BindingTarget(source,propertyarray,methodsarray));
		foreach(Type type in injects)
		{
			dependents.Add(type,component);
		}
		return component;
	}

	#region IComponent 成员


	public IComponent To(params Type[] types)
	{
		foreach(var t in types)
		{
			if(!injects.Contains(t))
				injects.Add(t);
		}
		return this;
	}

	#endregion

	#region IComponent 成员


	public void Singleton()
	{
		liftTimeScope = new SingletonScope();
	}

	#endregion

	#region IComponent 成员


	public IComponent Property(params InjectProperty[] property)
	{
		if(property != null)
		{
			foreach(var p in property)
			{
				propertys.Add(p);
			}
		}
		return this;
	}

	public IComponent Method(params InjectMethod[] method)
	{
		if(method != null)
		{
			foreach(var m in method)
			{
				methods.Add(m);
			}
		}
		return this;
	}

	#endregion

	#region IDisposable 成员

	public void Dispose()
	{
		GC.SuppressFinalize(this);
	}

	#endregion
}
