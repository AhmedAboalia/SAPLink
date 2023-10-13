using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SAPLink.EF.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        T GetById(string id);
        Task<T> GetByIdAsync(string id);
        Task<T> GetByIdAsync(int id);

        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> GetAll(string[] includes = null);

        T Find(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        T Add(T entity);
        Task<T> AddAsync(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        void Save();
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        int Count();
        int Count(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync();
        //Task<int> CountAsync(Expression<Func<T, bool>> criteria);
    }
}