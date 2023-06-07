using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data
{
	public interface IRepository<T> : IDisposable
	 where T : class
	{
		int Add(T entity);
		Int32 CRUD(string dml, T item, Boolean immediately = true);
		Task<int> CrudAsync(string dml, T item);

		int Delete(int id);
		int Delete(string code);
		int Delete(T entity);
		//IQueryable<T> GetAll();
		IQueryable<T> GetAll(string includeProperties = "");
		IQueryable<T> Table { get; }
		IQueryable<T> TableNoTracking { get; }

		T GetByCode(string code);
		T GetById(int id);
		int SaveChanges();
		int Update(T entity);
		int UpdateBatch(List<T> entity);
		//void Dispose();
	}
}
