using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessERP.Repositories
{
    interface IRepository<TEntity> where TEntity:class
    {
        List<TEntity> GetAll();
        TEntity GetById(int id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);

    }
}