using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MedicalApp.DA.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> GetAll();
        public TEntity? GetById(int id);
        public void Add(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);

    }
}
