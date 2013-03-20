using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LCW.Framework.Common.Ioc
{
    internal class Container:IContainer
    {
        private readonly IDictionary<Type, Type> types = new Dictionary<Type, Type>();
        private readonly IDictionary<Type, object> typeInstances = new Dictionary<Type, object>();

        public Container()
        {
            
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public void Register<TContract, TImplementation>() where TImplementation : TContract
        {
            types[typeof(TContract)] = typeof(TImplementation);
        }

        public void Register<TContract, TImplementation>(TImplementation instance)where TImplementation : TContract
        {
            typeInstances[typeof(TContract)] = instance;
        }

        private object Resolve(Type contract)
        {
            if (typeInstances.ContainsKey(contract))
            {
                return typeInstances[contract];
            }
            else
            {
                Type implementation = types[contract];
                ConstructorInfo constructor = implementation.GetConstructors()[0];
                ParameterInfo[] constructorParameters = constructor.GetParameters();
                if (constructorParameters.Length == 0)
                {
                    return Activator.CreateInstance(implementation);
                }
                List<object> parameters = new List<object>(constructorParameters.Length);
                foreach (ParameterInfo parameterInfo in constructorParameters)
                {
                    parameters.Add(Resolve(parameterInfo.ParameterType));
                }
                return constructor.Invoke(parameters.ToArray());
            }
        }
    }
}
