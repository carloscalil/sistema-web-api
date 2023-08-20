using System;
using System.Collections.Generic;

namespace Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> get();

        T getById(int id);
        void add(T entity);

        int update(int id,T entity);

        int delete(int id);
    }
}
