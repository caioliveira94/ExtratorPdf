using Data.Interfaces.Generic;
using Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infra.Repository.Generic
{
    public class RepositoryGeneric<T> : InterfaceGeneric<T>, IDisposable where T : class
    {
        private readonly DbContextOptionsBuilder<ApplicationContext> _optionsBuilder;
        public RepositoryGeneric()
        {
            _optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        }

        public void Add(T Entity)
        {
            using (var db = new ApplicationContext(_optionsBuilder.Options))
            {
                db.Set<T>().Add(Entity);
                db.SaveChanges();
            }
        }

        public List<T> GetAll()
        {
            using (var db = new ApplicationContext(_optionsBuilder.Options))
            {
                return db.Set<T>().AsNoTracking().ToList();
            }
        }

        public T GetById(int Id)
        {
            using (var db = new ApplicationContext(_optionsBuilder.Options))
            {
                return db.Set<T>().Find(Id);
            }
        }

        public void Remove(T Entity)
        {
            using (var db = new ApplicationContext(_optionsBuilder.Options))
            {
                db.Set<T>().Remove(Entity);
                db.SaveChanges();
            }
        }

        public void Update(T Entity)
        {
            using (var db = new ApplicationContext(_optionsBuilder.Options))
            {
                db.Set<T>().Update(Entity);
                db.SaveChanges();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDispose)
        {
            if (!isDispose) return;
        }
        ~RepositoryGeneric()
        {
            Dispose(false);
        }
    }
}
