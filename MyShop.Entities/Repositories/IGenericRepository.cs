using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.Repositories
{
	public interface IGenericRepository<T> where T : class
	{
		IEnumerable<T> GetAll(Expression<Func<T,bool>>? perdicate = null , string? IncludeWord = null);
		T GetById(Expression<Func<T, bool>>? perdicate = null, string? IncludeWord = null);

		void Add(T entity);

		void Remove(T entity);

		void RemoveRange(IEnumerable<T> entities);
	}
}
