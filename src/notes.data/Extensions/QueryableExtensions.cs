using notes.data.Exceptions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace notes.data.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortProperty,
            ListSortDirection sortOrder)
        {
            var type = typeof(T);
            var property = type.GetProperty(sortProperty,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new DalException(ErrorCodes.SortPropertyNotFound);
            }

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var typeArguments = new Type[] { type, property.PropertyType };
            var methodName = sortOrder == ListSortDirection.Ascending ? "OrderBy" : "OrderByDescending";
            var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, source.Expression,
                Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            ParameterExpression param = expr1.Parameters[0];
            if (ReferenceEquals(param, expr2.Parameters[0]))
            {
                // simple version
                return Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(expr1.Body, expr2.Body), param);
            }

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    expr1.Body,
                    Expression.Invoke(expr2, param)), param);
        }

        public static Expression<Func<T, bool>> OrAlso<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            ParameterExpression param = expr1.Parameters[0];
            if (ReferenceEquals(param, expr2.Parameters[0]))
            {
                return Expression.Lambda<Func<T, bool>>(
                    Expression.Or(expr1.Body, expr2.Body), param);
            }

            return Expression.Lambda<Func<T, bool>>(
                Expression.Or(
                    expr1.Body,
                    Expression.Invoke(expr2, param)), param);
        }

        public static Expression<Func<T, bool>> CombineAnd<T>(Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            if (first == null)
                return second;
            if (second == null)
                return first;
            return first.AndAlso(second);
        }

        public static Expression<Func<T, bool>> CombineOr<T>(Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            if (first == null)
                return second;
            if (second == null)
                return first;
            return first.OrAlso(second);
        }
    }
}