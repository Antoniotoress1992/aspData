using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace CRM.Data.Entities
{
	public static class LinqExtensions {
		// url: http://www.christian-etter.de/?p=522
		//
		/// <summary>Generic method used for sorting according to the pseudo SQL syntax emmitted by ObjectDataSource.</summary>
		public static IQueryable<T> orderBySQLSyntax<T>(this IQueryable<T> source, string sPropertyParameter) {
			string[] asParams = sPropertyParameter.Split(new char[] { ' ' }, 2);
			bool descending = (asParams.Length == 2 && String.Equals(asParams[1], "DESC"));
			return (IQueryable<T>)orderByExtension(source, asParams[0], descending);
		}

		/// <summary>Used for sorting in ascending/descending order according to the property provided.</summary>
		public static IQueryable orderByExtension(this IQueryable source, string propertyName, bool descending) {
			var x = Expression.Parameter(source.ElementType, "x");
			var selector = Expression.Lambda(Expression.PropertyOrField(x, propertyName), x);
			MethodCallExpression mce = Expression.Call(typeof(Queryable), descending ? "OrderByDescending" : "OrderBy",
			    new Type[] { source.ElementType, selector.Body.Type }, source.Expression, selector);
			return source.Provider.CreateQuery(mce);
		}

		public static IEnumerable<T> orderBy<T>(this IEnumerable<T> collection, string columnName) {
			ParameterExpression param = Expression.Parameter(typeof(T), "y");  // y  
			Expression property = Expression.Property(param, columnName);    // y.ColumnName  
			Func<T, object> lambda = Expression.Lambda<Func<T, object>>(    // y => y.ColumnName  
			    Expression.Convert(property, typeof(object)),
			    param)
			  .Compile();
			Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> expression = (c, f) => c.OrderBy(f);
			IEnumerable<T> sortedlist = expression(collection, lambda);
			return sortedlist;
		}
		
		/// <summary>
		/// sortExpression = column name + " asc " or " desc "
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="sortExpression"></param>
		/// <returns></returns>
		public static IEnumerable<T> sort<T>(this IEnumerable<T> source, string sortExpression) {
			string[] sortParts = sortExpression.Split(' ');
			var param = Expression.Parameter(typeof(T), string.Empty);
			try {
				var property = Expression.Property(param, sortParts[0]);
				var sortLambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);

				if (sortParts.Length > 1 && sortParts[1].Equals("desc", StringComparison.OrdinalIgnoreCase)) {
					return source.AsQueryable<T>().OrderByDescending<T, object>(sortLambda);
				}
				return source.AsQueryable<T>().OrderBy<T, object>(sortLambda);
			}
			catch (ArgumentException) {
				return source;
			}
		}
		
		/// <summary>
		/// Sorts both flat collection or collection with nested property.
		/// http://jarrettmeyer.com/post/9039239535/server-side-sorting-with-dynamic-linq
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="propertyPath"></param>
		/// <param name="sortDirection"></param>
		/// <returns></returns>
		public static IQueryable orderByNested<T>(this IQueryable<T> collection, string propertyPath, SortDirection sortDirection) {

			if (string.IsNullOrEmpty(propertyPath)) {
				return collection;
			}

			Type collectionType = typeof(T);

			ParameterExpression parameterExpression = Expression.Parameter(collectionType, "p");

			Expression seedExpression = parameterExpression;

			Expression aggregateExpression = propertyPath.Split('.').Aggregate(seedExpression, Expression.Property);

			MemberExpression memberExpression = aggregateExpression as MemberExpression;

			if (memberExpression == null) {

				throw new NullReferenceException(string.Format("Unable to cast Member Expression for given path: {0}.", propertyPath));
			}

			LambdaExpression orderByExp = Expression.Lambda(memberExpression, parameterExpression);

			const string orderBy = "OrderBy";

			const string orderByDesc = "OrderByDescending";

			Type childPropertyType = ((PropertyInfo)(memberExpression.Member)).PropertyType;

			string methodToInvoke = sortDirection == SortDirection.Ascending ? orderBy : orderByDesc;

			var orderByCall = Expression.Call(typeof(Queryable), methodToInvoke, new[] { collectionType, childPropertyType }, collection.Expression, Expression.Quote(orderByExp));

			return collection.Provider.CreateQuery(orderByCall);
		}


	}
}
