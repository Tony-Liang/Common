using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc
{
    public class ContaineraBuilder : IocContainer
    {
        private readonly IList<Action<Multimap<Type, Component>>> actions = new List<Action<Multimap<Type, Component>>>();

        public IContainer Build()
        {
            var build = new Container(new ReferenceFactory());
            Building(build.typeInstances);
            return build;
        }

        private void Building(Multimap<Type, Component> list)
        {
            foreach (var action in actions)
            {
                action(list);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IComponent Register<T>()
        {
            var component=new Component(typeof(T));
            actions.Add((p) =>
            {
                p.Add(typeof(T),component);
            });
            return component;
        }

        public IComponent Register<TContract, TImplementation>() where TImplementation : TContract
        {
            var component=this.Register<TContract>();
            component.Implement<TImplementation>();
            return component;
        }
    }
}
