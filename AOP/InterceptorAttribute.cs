using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace LCW.Framework.Common.AOP
{
    // class 继承一下ContextBoundObject
    [AttributeUsage(AttributeTargets.Class,AllowMultiple=false)]
    public sealed class InterceptorAttribute : ContextAttribute, IContributeObjectSink
    {
        public InterceptorAttribute()
            : base("InterceptorAttribute")
        {
        }
        public IMessageSink GetObjectSink(MarshalByRefObject obj, System.Runtime.Remoting.Messaging.IMessageSink nextSink)
        {
            throw new NotImplementedException();
        }
    }

    [AttributeUsage(AttributeTargets.Method,AllowMultiple=false)]
    public sealed class InterceptorMethodAttribute : Attribute
    {
        public InterceptorMethodAttribute()
        {
        }
    }

    public sealed class MyInterceptor:IMessageSink
    {
        private IMessageSink nextSink; //保存下一个接收器

        public MyInterceptor(IMessageSink nextSink)
        {
            this.nextSink = nextSink;
        }

        public IMessageCtrl  AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
 	        throw new NotImplementedException();
        }

        public IMessageSink  NextSink
        {
	        get { return nextSink; }
        }

        public IMessage  SyncProcessMessage(IMessage msg)
        {
 	        IMessage retMsg = null;

            IMethodCallMessage call = msg as IMethodCallMessage;
            
            if (call == null ||  (Attribute.GetCustomAttribute(call.MethodBase, typeof(InterceptorMethodAttribute))) == null)
                retMsg = nextSink.SyncProcessMessage(msg);
            else
            {
                Console.WriteLine("start...");
                retMsg = nextSink.SyncProcessMessage(msg);
                Console.WriteLine("end...");
            }
            return retMsg;
        }
    }
}
