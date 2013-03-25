using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LCW.Framework.Common.Ioc
{
    internal class ComponentInterpreter : IComponentInterpreter
    {
        private Container container;
        public ComponentInterpreter(Container container)
        {
            this.container = container;
        }
        #region IComponentInterpreter 成员
        public object Execute(Component component)
        {
            object temp = null;
            try
            {
                //ConstructorInfo construct = component.target.GetConstructors().FirstOrDefault();
                //if (construct.GetParameters().Length > 0)
                //{
                //    var argment = construct.GetParameters().Select(parameter => Get(parameter.ParameterType, component.target)).ToArray();
                //    //if (argment!=null && argment.Count() > 0)
                //    temp = construct.Invoke(argment);
                //}
                //else
                //    temp = Activator.CreateInstance(component.target);
            }
            catch (Exception ex)
            {
            }
            return temp;
        }
        private object Get(Type type, Type target)
        {
            //if (container.typeInstances.ContainsKey(type))
            //{
            //    //Component temp = null;
            //    //var list = container.typeInstances[type].Where(component =>
            //    //{
            //    //    if (component.injects.Contains(target))
            //    //    {
            //    //        return true;
            //    //    }
            //    //    return false;
            //    //});
            //    //if (list != null && list.Count() > 0)
            //    //{
            //    //    temp = list.First();
            //    //}
            //    //else
            //    //{
            //    //    temp = container.typeInstances[type].First();
            //    //}
            //    //return Execute(temp);
            //}
            return null;
        }
        #endregion
    }
}
