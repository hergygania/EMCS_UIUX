using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace App.Data
{
    public class Repository<E> : IRepository<E>
        where E : class
    {
        private DbContext _dbContext { get; set; }
        private DbSet<E> _dbSet { get; set; }

        public Repository(DbContext dbContext)
        {
            if (dbContext == null) throw new NullReferenceException("dbContext");
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<E>();
        }

        #region Implementation of IRepository<T>

        //public IQueryable<E> GetAll()
        //{
        //	return _dbSet;
        //}
        public virtual IQueryable<E> GetAll(string includeProperties = "")
        {
            IQueryable<E> query = _dbSet;

            foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;
        }
        public virtual IQueryable<E> Table
        {
            get
            {
                return this._dbSet;
            }
        }

        public virtual IQueryable<E> TableNoTracking
        {
            get
            {
                return this._dbSet.AsNoTracking();
            }
        }

        public E GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public E GetByCode(string code)
        {
            return _dbSet.Find(code);
        }

        public virtual int Add(E entity)
        {
            _dbSet.Add(entity);
            var retValue = this.SaveChanges();
            return retValue;
        }
        public async virtual Task<int> AddAsync(E entity)
        {
            _dbSet.Add(entity);
            var retValue = await this.SaveChangesAsync();
            return retValue;
        }

        public virtual int Update(E entity)
        {
            var entry = _dbContext.Entry(entity);
            _dbSet.Attach(entity);
            entry.State = EntityState.Modified;
            var retValue = this.SaveChanges();
            return retValue;
        }

        public virtual int UpdateBatch(List<E> entity)
        {
            foreach (var e in entity)
            {
                var entry = _dbContext.Entry(e);
                _dbSet.Attach(e);
                entry.State = EntityState.Modified;
            }

            var retValue = this.SaveChanges();
            return retValue;
        }

        public async virtual Task<int> UpdateAsync(E entity)
        {
            var entry = _dbContext.Entry(entity);
            _dbSet.Attach(entity);
            entry.State = EntityState.Modified;
            var retValue = await this.SaveChangesAsync();
            return retValue;
        }

        public virtual int Delete(E entity)
        {
            var entry = _dbContext.Entry(entity);
            _dbSet.Attach(entity);
            entry.State = EntityState.Deleted;
            var retValue = this.SaveChanges();
            return retValue;
        }
        public async virtual Task<int> DeleteAsync(E entity)
        {
            var entry = _dbContext.Entry(entity);
            _dbSet.Attach(entity);
            entry.State = EntityState.Deleted;
            var retValue = await this.SaveChangesAsync();
            return retValue;
        }

        public virtual int Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return 0;
            Delete(entity);
            var retValue = this.SaveChanges();
            return retValue;
        }
        public virtual int Delete(string code)
        {
            var entity = GetByCode(code);
            if (entity == null) return 0;
            Delete(entity);
            var retValue = this.SaveChanges();
            return retValue;
        }

        public virtual Int32 CRUD(string dml, E item, Boolean immediately = true)
        {
            int retValue = 0;
            if (dml == "I")
            {
                retValue = this.Add(item);
            }
            else if (dml == "U")
            {
                retValue = this.Update(item);
            }
            else if (dml == "D")
            {
                retValue = this.Delete(item);
            }
            return retValue;
        }
        public async virtual Task<int> CrudAsync(string dml, E item)
        {
            int retValue = 0;
            if (dml == "I")
            {
                retValue = await this.AddAsync(item);
            }
            else if (dml == "U")
            {
                retValue = await this.UpdateAsync(item);
            }
            else if (dml == "D")
            {
                retValue = await this.DeleteAsync(item);
            }
            return retValue;
        }

        public virtual int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        public async virtual Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        #endregion

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
                disposed = true;
            }
        }
    }
}
