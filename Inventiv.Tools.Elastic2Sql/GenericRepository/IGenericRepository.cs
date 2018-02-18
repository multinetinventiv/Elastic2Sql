using System;
using System.Linq;
using System.Linq.Expressions;

namespace Inventiv.Tools.Elastic2Sql.GenericRepository
{
	public interface IGenericRepository<T> where T :class
	{
		IQueryable<T> GetAll();
		IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
		T GetById(int id);
		T Get(Expression<Func<T, bool>> predicate);

		T Add(T entity);
		void Update(T entity);
		void Delete(T entity);
		void Delete(int id);
	}
}
