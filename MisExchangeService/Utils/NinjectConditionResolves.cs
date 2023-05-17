using Ninject.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Utils
{
    static class NinjectConditionResolves
    {
        public static bool IsParentRequestTypeMatch<T>(IRequest request)
        {
            return request.ParentRequest != null && request.ParentRequest.Service.Name.Contains(typeof(T).Name);
            //return request.ParentRequest != null && request.ParentRequest.Target.Member.DeclaringType.Name.Contains(typeof(T).Name);        
        }
        public static bool IsCurrentTargeTypeMatch<T>(IRequest request)
        {
            return request.Target != null && request.Target.Member.ReflectedType.Name.Contains(typeof(T).Name); 
        }
        /// <summary>
        /// Recursive condition method for When method to find out if any ancestor mathes the type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsAncestorRequestTypeMatch<T>(IRequest request)
        {
            if (request.ParentRequest == null)
                return false;
            if (request.ParentRequest != null && request.ParentRequest.Service.Name.Contains(typeof(T).Name))
                return true;
            return IsAncestorRequestTypeMatch<T>(request.ParentRequest);
        }
        /// <summary>
        /// Conditional method for WhenAnyAncestorMatches
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsAnyAncestorContextTypeMatch<T>(IContext context)
        {
            return context.Plan.Type.Name.Contains(typeof(T).Name);
        }
    }
}
