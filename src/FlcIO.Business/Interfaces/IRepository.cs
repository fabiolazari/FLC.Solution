using FlcIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FlcIO.Business.Interfaces
{
	public interface IRepository<TEntity> : IDisposable where TEntity : Entity
	{
		Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate);
		Task<TEntity> GetById(Guid id);
		Task<List<TEntity>> GetAll();
		Task Add(TEntity entity);
		Task<int> SaveChanges();
	}
}
