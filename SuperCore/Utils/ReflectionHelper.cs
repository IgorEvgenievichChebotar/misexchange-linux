using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ru.novolabs.SuperCore
{
    /// <summary>
    /// Class used to get reflected information without "magic-string" information using Strong-typed arguments with ExpressionTrees magic
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Get property or field name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns>Strong-type name of property or field</returns>
        public static string GetPropertyName<T>(Expression<Func<T>> property)
        {
            if (property == null)
                throw new ArgumentNullException();

            MemberExpression member = property.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException("Argument is not property or field");
            return member.Member.Name;        
        }
        /// <summary>
        /// Get property or field name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string GetFieldName<T>(Expression<Func<T>> field)
        {
            return GetPropertyName<T>(field);
        }
        /// <summary>
        /// Get method name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodCall"></param>
        /// <returns></returns>
        public static string GetMethodName<T>(Expression<Func<T>> methodCall)
        {
            if (methodCall == null)
                throw new ArgumentNullException();

            MethodCallExpression member = methodCall.Body as MethodCallExpression;
            if (member == null)
                throw new ArgumentException("Argument is not calling method");
            return member.Method.Name;    
        }
        public static string GetMethodName(Expression<Action> methodCall)
        {
            if (methodCall == null)
                throw new ArgumentNullException();

            MethodCallExpression member = methodCall.Body as MethodCallExpression;
            if (member == null)
                throw new ArgumentException("Argument is not calling method");
            return member.Method.Name;
        }
        public static string GetLocalVariableName<T>(Expression<Func<T>> expression)
        {
            try
            {
                return GetPropertyName(expression);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Argument is not a local variable or function parameter");            
            }        
        }
    }
}
