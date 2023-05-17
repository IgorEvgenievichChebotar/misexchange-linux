using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Reflection;

namespace ru.novolabs.Common
{
    /// <summary>
    /// Generate DbQuery for EF Entity with all relatted entities by Eager loading(Include methods)
    /// </summary>
    public class DBQueryGenerator
    {
        /// <summary>
        /// Identifies that type is not custom == in System. namespaces
        /// </summary>
        private const string SystemNmspc = "System";
        /// <summary>
        /// Identifies that type is a collection type(not IEnumerable, because duck-typeing)
        /// </summary>
        private const string CollTypeIdentifier = "GetEnumerator";
        /// <summary>
        /// List of strings for chain of Include methods
        /// </summary>
        private List<string> dBQueryStrs = new List<string>();
        /// <summary>
        /// Build in one-step(Fill list and build chain) DbQuery (not tested)
        /// </summary>
        /// <typeparam name="T">Type of Entity</typeparam>
        /// <param name="type">Must be typeof(T)</param>
        /// <param name="Set">DbSet from DbContext</param>
        /// <returns>DbQuery with all relatted entities</returns>
        public DbQuery<T> BuildQueryByAllProps<T> (Type type, DbSet<T> Set) where T:class
        {
            if (type != typeof(T))
                throw new ArgumentException("Argument of parameter 'type' must be typeof(T)");
            if (dBQueryStrs.Count > 0)
            {
                dBQueryStrs = new List<string>();
            
            }
            var custProps = from prop in type.GetProperties()
                            where prop.PropertyType.IsPublic && !prop.PropertyType.IsEnum && !prop.PropertyType.IsPrimitive
                            where !prop.PropertyType.Namespace.StartsWith(SystemNmspc) || prop.PropertyType.GetMethod(CollTypeIdentifier) != null
                                                                                        && prop.PropertyType.IsGenericType
                                                                                        && prop.PropertyType.GetGenericArguments().Where(t => !t.Namespace.StartsWith(SystemNmspc)).Count() > 0
                            select prop;
            if (custProps.Count() == 0)
            {
                return Set;
            
            }
            foreach (var p in custProps)
            {
                BuildDbQueryStr(p, String.Empty);            
            }
            DbQuery<T> result = Set;
            foreach (string str in dBQueryStrs)
            {
                result = result.Include(str); 
            }
            return result;

        }
        /// <summary>
        /// Build in two-steps DbQuery(Fill and return list)
        /// </summary>
        /// <param name="type">Type of Entity, for which you want to build DbQuery</param>
        /// <returns>List of string for chain of Include methods</returns>
        public List<string> PreBuildQueryList(Type type)
        {
            if (dBQueryStrs.Count > 0)
            {
                dBQueryStrs = new List<string>();            
            }
            var custProps = from prop in type.GetProperties()
                            where prop.PropertyType.IsPublic && !prop.PropertyType.IsEnum && !prop.PropertyType.IsPrimitive
                            where !prop.PropertyType.Namespace.StartsWith(SystemNmspc) || prop.PropertyType.GetMethod(CollTypeIdentifier) != null
                                                                                        && prop.PropertyType.IsGenericType 
                                                                                        && prop.PropertyType.GetGenericArguments().Where(t=>!t.Namespace.StartsWith(SystemNmspc)).Count() > 0
                            select prop;
            foreach (var p in custProps)
            {
                BuildDbQueryStr(p, String.Empty);
            }
            return dBQueryStrs;      
        }
        /// <summary>
        /// Build in two-steps DbQuery(build chain from List of strings for Include methods)
        /// </summary>
        /// <typeparam name="T">Type of Entity</typeparam>
        /// <param name="propList">List of strings from Include methods</param>
        /// <param name="Set">DbSet from DbContext</param>
        /// <returns>DbQuery with all related entities</returns>
        public static DbQuery<T> BuildQueryByAllProps<T>(List<string> propList, DbSet<T> Set) where T: class
        {
            if (Set == null || propList == null)
            {
                throw new ArgumentNullException("Some of Arguments are null");            
            }
            DbQuery<T> result = Set;
            foreach (string str in propList)
            {
                result = result.Include(str);
            }
            return result;
        
        }

        /// <summary>
        /// Recursive build property string by Method or recursive descent 
        /// </summary>
        /// <param name="prop">Current proprty in chain-building</param>
        /// <param name="queryStr">current state of string</param>
        private void BuildDbQueryStr(PropertyInfo prop, string queryStr)
        {

            var custProps = from propIn in prop.PropertyType.GetProperties()
                            where propIn.PropertyType.IsPublic && !propIn.PropertyType.IsEnum && !propIn.PropertyType.IsPrimitive
                            where !propIn.PropertyType.Namespace.StartsWith(SystemNmspc) || propIn.PropertyType.GetMethod(CollTypeIdentifier) != null
                                                                                         && propIn.PropertyType.IsGenericType
                                                                                         && propIn.PropertyType.GetGenericArguments().Where(t => !t.Namespace.StartsWith(SystemNmspc)).Count() > 0
                            select propIn;
            queryStr =String.IsNullOrEmpty(queryStr)? prop.Name : 
                (prop.DeclaringType.GetMethod(CollTypeIdentifier) != null && prop.DeclaringType.IsGenericType)? queryStr : queryStr + "." + prop.Name;
            if (custProps.Count() > 0)
            {
                foreach(var p in custProps)
                {
                    BuildDbQueryStr(p,queryStr);                   
                }
            }
            else 
            {
                dBQueryStrs.Add(queryStr);                            
            }        
        
        }
    }
}
