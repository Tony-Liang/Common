using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc
{
    public class ContaineraBuilder : IocContainer
    {
        private readonly IList<Action<Multimap<Type, Component>>> actions = new List<Action<Multimap<Type, Component>>>();
		
		private readonly Multimap<Type,Component> dependent = new Multimap<Type,Component>();
        
		public IContainer Build()
        {
            var build = new Container(new ReferenceFactory());
            Building(build.typeInstances);
            return build;
        }

        private void Building(Multimap<Type, Component> list)
        {
			list.Clear();
            foreach (var action in actions)
            {
                action(list);
            }
			foreach(var f in dependent.Keys)
			{
				foreach(var l in list[f])
				{
					l.Dependent.AddRange(dependent[f]);
				}
			}
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IComponent Register<T>()
        {
			var component = new ComponentFacade(typeof(T));
            actions.Add((p) =>
            {
				p.Add(typeof(T),component.CreateComponent(dependent));
            });
            return component;
        }

        public IComponent Register<TContract, TImplementation>() where TImplementation : TContract
        {
			var component = new ComponentFacade(typeof(TContract),typeof(TImplementation));
			actions.Add((p) =>
			{
				p.Add(typeof(TContract),component.CreateComponent(dependent));
			});
            return component;
        }
    }
}
