using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace chatApp.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entityObj);
        void Update(T entityObj);

        IEnumerable<T> GellAll();

        List<T> GetByExpression(Func<T, bool> expression);

        IQueryable<T> GetQueryable(Func<T, bool> expression);

        void Delete(int id);

        CustomResponse Save();
        
    }
}
