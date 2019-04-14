using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyGenericUnitOfWork.Base
{
    public static class DynamicQueryExpressions<T> where T : class
    {
        public static Expression<Func<T, bool>> GetExpresionTrees(string propertyName, object propertyValue)
        {

            // Get prooerty access expression
            var property = PropertyAccessorCache<T>.Get(propertyName);
            if (property == null) return null;

            // Property express without cache
            //var type = typeof(T);
            //var parameter = Expression.Parameter(type, "p");
            //var property = Expression.Property(parameter, propertyName);

            // Try converting value to correct type
            object value;
            try
            {
                value = Convert.ChangeType(propertyValue, property.ReturnType);
            }
            catch (SystemException ex) when (
                ex is InvalidCastException ||
                ex is FormatException ||
                ex is OverflowException ||
                ex is ArgumentNullException)
            {
                return null;
            }

            // Construct expression tree
            var eqe = Expression.Equal(
                property.Body,
                Expression.Constant(value, property.ReturnType));
            return Expression.Lambda<Func<T, bool>>(eqe, property.Parameters[0]);
        }

    }

    public static class PropertyAccessorCache<T> where T : class
    {
        private static readonly IDictionary<string, LambdaExpression> _cache;

        static PropertyAccessorCache()
        {
            var storage = new Dictionary<string, LambdaExpression>();

            var t = typeof(T);
            var parameter = Expression.Parameter(t, "p");
            foreach (var property in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var lambdaExpression = Expression.Lambda(propertyAccess, parameter);
                storage[property.Name] = lambdaExpression;
            }

            _cache = storage;
        }

        public static LambdaExpression Get(string propertyName)
        {
            LambdaExpression result;
            return _cache.TryGetValue(propertyName, out result) ? result : null;
        }
    }


}
