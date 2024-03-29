﻿using FlcIO.Business.Interfaces;
using FlcIO.Business.Models;
using FlcIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FlcIO.Data.Repository
{
	public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
	{
		protected readonly MessengerContext Db;
		protected readonly DbSet<TEntity> DbSet;

		protected Repository(MessengerContext db)
		{
			Db = db;
			DbSet = db.Set<TEntity>();
		}

		public async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
		{
			return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
		}

		public virtual async Task<TEntity> GetById(Guid id)
		{
			return await DbSet.FindAsync(id);
		}

		public virtual async Task<List<TEntity>> GetAll()
		{
			return await DbSet.ToListAsync();
		}

		public virtual async Task Add(TEntity entity)
		{
			Db.Entry(entity).State = EntityState.Detached;
			EntityEntry<TEntity> _entityEntry = DbSet.Add(entity);
			await SaveChanges();
		}

		public async Task<int> SaveChanges()
		{
			return await Db.SaveChangesAsync();
		}

		public void Dispose()
		{
			Db?.Dispose();
		}
	}
}
