using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace App.Framework.Mvc.UI.Sorting
{
	public static class SortExtensions
	{
		public static IQueryable<T> OrderBy<T>(this IQueryable<T> dtSource, SortOptions sort)
		{
			return dtSource.OrderBy(sort.SortBy, sort.SortOrder);
		}

		public static IQueryable<T> OrderBy<T>(this IQueryable<T> dtSource, string propertyName, SortDirection direction)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				return dtSource;
			}

			var type = typeof(T);
			var prop = type.GetProperty(propertyName);

			if (prop == null)
			{
				throw new InvalidOperationException(string.Format("Could not find a property called '{0}' on type {1}", propertyName, type));
			}

			var paramEx = Expression.Parameter(type, "e");
			var propAccessEx = Expression.MakeMemberAccess(paramEx, prop);
			var orderByEx = Expression.Lambda(propAccessEx, paramEx);

			const string orderBy = "OrderBy";
			const string orderByDesc = "OrderByDescending";

			string methodToInvoke = direction == SortDirection.Ascending ? orderBy : orderByDesc;

			var orderByCall = Expression.Call(typeof(Queryable),
				methodToInvoke,
				new[] { type, prop.PropertyType },
				dtSource.Expression,
				Expression.Quote(orderByEx));

			return dtSource.Provider.CreateQuery<T>(orderByCall);
		}


		public static IQueryable<object> OrderBy(this IQueryable<object> dtSource, SortOptions sort)
		{
			return dtSource.OrderBy(sort.SortBy, sort.SortOrder);
		}

		public static IQueryable<object> OrderBy(this IQueryable<object> dtSource, string propertyName, SortDirection direction)
		{
			var instance = dtSource.Take(1).SingleOrDefault();

			if (instance == null) return dtSource;

			var type = instance.GetType();

			var prop = type.GetProperty(propertyName);

			var paramEx = Expression.Parameter(typeof(object), "e");

			var propAccessEx = Expression.Constant(prop, typeof(PropertyInfo));

			var getValueEx = Expression.Call(propAccessEx, "GetValue", null, paramEx, Expression.Constant(new object[] { }));

			var sortEx = Expression.Lambda<Func<object, object>>(getValueEx, paramEx);

			var orderByEx = Expression.Call(typeof(Queryable),
																		direction == SortDirection.Ascending ? "OrderBy" : "OrderByDescending",
																			new[] { typeof(object), typeof(object) },
																				dtSource.Expression,
																					Expression.Quote(sortEx));

			return dtSource.Provider.CreateQuery<object>(orderByEx);
		}


		public static IQueryable<DataRow> OrderBy(this IQueryable<DataRow> dtSource, SortOptions sort)
		{
			return dtSource.OrderBy(sort.SortBy, sort.SortOrder);
		}

		public static IQueryable<DataRow> OrderBy(this IQueryable<DataRow> dtSource, string propertyName, SortDirection direction)
		{
			var dtRowEx = Expression.Parameter(typeof(DataRow), "e");

			var strType = typeof(string);

			var fldMethod = typeof(DataRowExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
													.SingleOrDefault(e => e.Name.Equals("Field")
														&& e.GetParameters().Count() == 2
															&& e.GetParameters()[1].ParameterType == strType)
																.MakeGenericMethod(typeof(object));

			var sortEx = Expression.Lambda<Func<DataRow, object>>(Expression.Call(null, fldMethod, dtRowEx,
																															Expression.Constant(propertyName, strType)),
																																dtRowEx);

			var orderByEx = Expression.Call(typeof(Queryable),
																		direction == SortDirection.Ascending ? "OrderBy" : "OrderByDescending",
																			new[] { typeof(DataRow), typeof(object) },
																				dtSource.Expression,
																					Expression.Quote(sortEx));

			return dtSource.Provider.CreateQuery<DataRow>(orderByEx);
		}
	}
}
