using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Interface.Repository
{
    public interface IDataRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();

        int Insert(TEntity entity);
        int Update( TEntity entity);
        int Delete(object id );
    }
}
 