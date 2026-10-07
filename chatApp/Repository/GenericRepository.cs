using System.Collections.Generic;
using System;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;
using chatApp.Data;
using chatApp.Models;
using System.Linq.Expressions;

namespace chatApp.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class // we will inserting table name here for use in controllers

    {
        private chatAppContext _context = null;
        private readonly DbSet<T> table = null;

        public GenericRepository(chatAppContext context)
        {
            this._context = context;
            //this._context = new chatAppContext();
            table = _context.Set<T>();
        }

        public GenericRepository()
        {
        }

        public void Add(T Obj)
        {
            table.Add(Obj);
        }
        public void update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }
        public void delete(int id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }
        public CustomResponse Save()
        {
            CustomResponse response = new CustomResponse();
            try
            {
                var check = _context.SaveChanges();
                response.Success = check > 0;
            }
            catch (Exception exception)
            {
                response.Success = false;
                response.Exception = exception;
            }
            return response;
        }

        //public void Add(T entityObj)
        //{
        //    //throw new NotImplementedException();
        //}

        public void Update(T entityObj)
        {
            //throw new NotImplementedException();
        }

        //public IEnumerable<T> GellAll()
        //{
        //    throw new NotImplementedException();
        //}

        public void Delete(int id)
        {
           // throw new NotImplementedException();
        }

        IEnumerable<T> IGenericRepository<T>.GellAll()
        {
            return (IEnumerable<T>)table.ToList();
        }

        public List<T> GetByExpression(Func<T, bool> expression)
        {
            return _context.Set<T>().Where(expression).ToList();
        }

        public IQueryable<T> GetQueryable(Func<T, bool> expression)
        {
            IQueryable<T> queryable = _context.Set<T>().Where(expression).AsQueryable();
            return queryable;
        }


    }

        //public IEnumerable<T> GellAll()
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<T> GellAll()
        //{
        //    //throw new NotImplementedException();
        //}

        //IEnumerable<T> IGenericRepository<T>.GellAll()
        //{
        //    throw new NotImplementedException();
        //}

        //void IGenericRepository<T>.SaveChanges()
        //{
        //    throw new NotImplementedException();
        //}

        //        public void Add(T entityObj)
        //        {
        //            throw new NotImplementedException();
        //        }

        //        public void Update(T entityObj)
        //        {
        //            throw new NotImplementedException();
        //        }

        //        public IEnumerable<T> GellAll()
        //        {
        //            throw new NotImplementedException();
        //        }

        //        public void Delete(int id)
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }
    }

public class CustomResponse
{
    public bool Success { get; set; }
    public Exception Exception { get; set; }
}