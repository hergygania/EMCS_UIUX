using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using App.Framework.Mvc.UI.Sorting;

namespace App.Framework.Mvc.UI
{
	public interface IPaginationQuery<T> where T : class
	{
		IQueryable<T> OrderBy(IQueryable<T> query, SortOptions sort);
		IList<ItemDescription> GetDescriptions(IQueryable<T> query);
		object ToJsonRawObject(IEnumerable<T> query);
	}

	public class DefaultPaginationQuery<T> : IPaginationQuery<T>
		where T : class
	{
		public virtual IQueryable<T> OrderBy(IQueryable<T> query, SortOptions sort)
		{
			return query.OrderBy(sort);
		}

		public virtual object ToJsonRawObject(IEnumerable<T> query)
		{
			return query;
		}

		protected virtual PropertyDescriptorCollection GetDescriptor(IQueryable<T> query)
		{
			return TypeDescriptor.GetProperties(typeof(T));
		}

		public IList<ItemDescription> GetDescriptions(IQueryable<T> query)
		{
			var retVal = new List<ItemDescription>();

			var descriptor = GetDescriptor(query);

			if (descriptor != null)
			{
				foreach (PropertyDescriptor prop in descriptor)
				{
					retVal.Add(new ItemDescription { Name = prop.Name, DisplayName = prop.DisplayName, Type = prop.PropertyType });
				}
			}

			return retVal;
		}
	}

	public class DataRowPaginationQuery : DefaultPaginationQuery<DataRow>
	{
		public override object ToJsonRawObject(IEnumerable<DataRow> query)
		{
			return query.ToJsonObject(); 
		}

		public override IQueryable<DataRow> OrderBy(IQueryable<DataRow> query, SortOptions sort)
		{
			return query.OrderBy(sort);
		}

		protected override PropertyDescriptorCollection GetDescriptor(IQueryable<DataRow> query)
		{
			var item = query.Take(1).SingleOrDefault();
			if (item == null) return null;
			return ((ITypedList)item.Table.AsDataView()).GetItemProperties(null);
		}
	}

	public class ObjectPaginationQuery : DefaultPaginationQuery<object>
	{
		public override IQueryable<object> OrderBy(IQueryable<object> query, SortOptions sort)
		{
			return query.OrderBy(sort);
		}

		protected override PropertyDescriptorCollection GetDescriptor(IQueryable<object> query)
		{
			var item = query.Take(1).SingleOrDefault();
			if (item == null) return null;
			return TypeDescriptor.GetProperties(item.GetType());
		}
	}
}
