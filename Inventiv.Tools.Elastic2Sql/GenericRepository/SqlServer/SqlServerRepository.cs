using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Inventiv.Tools.Elastic2Sql.GenericRepository.SqlServer
{
	public class SqlServerRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly SqlServerContext context;
		private readonly DbSet<T> dbSet;

		public SqlServerRepository()
		{
			context = new SqlServerContext();
			dbSet = context.Set<T>();
		}

		public IQueryable<T> GetAll()
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public T GetById(int id)
		{
			throw new NotImplementedException();
		}

		public T Get(Expression<Func<T, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public T Add(T entity)
		{
			entity = dbSet.Add(entity);

			context.SaveChanges();

			return entity;
		}

		public void Update(T entity)
		{
			dbSet.Attach(entity);
			context.Entry(entity).State = EntityState.Modified;

			context.SaveChanges();
		}

		public void Delete(T entity)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{
			throw new NotImplementedException();
		}
	}
}
