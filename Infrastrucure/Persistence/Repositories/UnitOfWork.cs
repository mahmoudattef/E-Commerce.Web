using DomainLayer.Contracts;
using DomainLayer.Models;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork(StoreDbContext _dbContext) : IUnitOFWork
    {
        private readonly Dictionary<string, object> _respositories = [];
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var typeName= typeof(TEntity).Name;
            //if(_respositories.ContainsKey(typeName))
            //    return (IGenericRepository<TEntity,TKey>)_respositories[typeName];       

            if (_respositories.TryGetValue(typeName,out object? value ))
                return (IGenericRepository<TEntity, TKey>) value ;
            else
            {
                var Repo=new GenericRepository<TEntity,TKey>(_dbContext);
                _respositories.Add(typeName, Repo);
                return Repo;
            }
        }

        public async Task<int> SaveChangeAsync() => await _dbContext.SaveChangesAsync(); 
    }
}
