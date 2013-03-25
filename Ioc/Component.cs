using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LCW.Framework.Common.Ioc.LiftTimeScope;

namespace LCW.Framework.Common.Ioc
{
    public class Component : IComponent
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

        internal Component(Type type)
        {
            this.target = type;
            this.name = Guid.NewGuid().ToString();
            this.bingingTarget = new BindingTarget(type);
            this.liftTimeScope = new TransientScope();
        }

        public IComponent Implement<T>()
        {
            Type t = typeof(T);
            this.bingingTarget = new BindingTarget(t);
            return this;
        }

        public IComponent Name(string name)
        {
            this.name = name;
            return this;
        }
    }

    public delegate void Executed(object sender);
}
