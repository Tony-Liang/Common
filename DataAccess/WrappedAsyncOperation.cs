using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.DataAccess
{
    public static class WrappedAsyncOperation
    {
        
        public static IAsyncResult BeginAsyncOperation<TWrappingAsyncResult>(
            AsyncCallback callback,
            Func<AsyncCallback, IAsyncResult> beginOperation,
            Func<IAsyncResult, TWrappingAsyncResult> wrappingResultCreator)
            where TWrappingAsyncResult : class, IAsyncResult
        {
            
            var padlock = new object();
            TWrappingAsyncResult result = null;
            AsyncCallback wrapperCallback = null;
            if (callback != null)
            {
                wrapperCallback = ar =>
                {
                    if (!ar.CompletedSynchronously)
                    {
                        lock (padlock) { }
                        callback(result);
                    }
                };
            }

            lock (padlock)
            {
                IAsyncResult innerAsyncResult = beginOperation(wrapperCallback);
                result = wrappingResultCreator(innerAsyncResult);
            }

            if (result.CompletedSynchronously && callback != null)
            {
                callback(result);
            }
            return result;
        }
    }
}
