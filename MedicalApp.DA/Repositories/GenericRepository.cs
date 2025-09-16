using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalApp.DA.Interfaces;
using MedicalApp.DA.Models;

namespace MedicalApp.DA.Repositories
{
    public class GenericRepository<TEntity> :IGenericRepository<TEntity> where TEntity : class 
    {
        private readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }
        public TEntity? GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }
        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }
        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }
    }
}
