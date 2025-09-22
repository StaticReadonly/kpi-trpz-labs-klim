﻿using BookingClinic.Data.AppContext;
using Microsoft.EntityFrameworkCore;

namespace BookingClinic.Data.Repositories.Abstraction
{
    public class RepositoryBase<T, TKey> : IRepository<T, TKey> where T : class
    {
        protected readonly ApplicationContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void AddEntity(T entity) => _dbSet.Add(entity);

        public void AddRange(IEnumerable<T> entities) => _dbSet.AddRange(entities);

        public void DeleteEntity(T entity) => _dbSet.Remove(entity);

        public IEnumerable<T> GetAll() => _dbSet.ToList();

        public T? GetById(TKey key) => _dbSet.Find(key);

        public Task SaveChangesAsync() => _context.SaveChangesAsync();

        public void UpdateEntity(T entity) => _dbSet.Update(entity);
    }
}
