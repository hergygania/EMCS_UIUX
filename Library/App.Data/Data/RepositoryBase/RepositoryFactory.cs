using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace App.Data
{
	public interface IRepositoryFactory : IDisposable
	{
		DbContext DbContext { get; }

		IRepository<E> CreateRepository<E>() where E : class;
	}

	public class RepositoryFactory : IRepositoryFactory
	{
		#region Fields
		private bool m_disposed = false;
		private DbContext m_dbContext = null;
		#endregion

		#region Ctor And Destructor
		public RepositoryFactory(DbContext dbContext)
		{
			m_dbContext = dbContext;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!m_disposed) {
				if (disposing) {
				}
				m_dbContext.Dispose();
				m_disposed = true;
			}
		}

		~RepositoryFactory()
		{
			Dispose(false);
		}
		#endregion

		#region Asset
		public DbContext DbContext
		{
			get
			{
				return m_dbContext;
			}
		}

		public IRepository<E> CreateRepository<E>() where E : class
		{
			if (RegisterRepositoryType != null) {
				var type = RegisterRepositoryType.MakeGenericType(typeof(E));
				return Activator.CreateInstance(type, m_dbContext) as IRepository<E>;
			}

			return new Repository<E>(m_dbContext);
		}
		#endregion


		public static Type RegisterRepositoryType
		{
			get;
			set;
		}
	}
}
