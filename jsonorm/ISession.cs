using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace jsonorm
{
    public interface ISession
    {

        #region ISession Implementation

        T GetElementById<T>(Func<T, bool> query, string entityname) where T : new();

        List<T> GetElementList<T>(string entityname);

        List<T> GetElementList<T>(Func<T, bool> query, string entityname);

        int Count();

        IList<T> FindPage<T>(int pageStartRow, int pageSize);

        IList<T> FindSortedPage<T>(int pageStartRow, int pageSize, string sortBy, bool descending);

        void Save<T>(T entity, string entityname);

        void AddRange<T>(string resource, string entityname) where T : new();

        void Update<T>(T entity, Func<T, bool> query, string entityname);

        void SaveOrUpdate<T>(T entity, Func<T, bool> query, string entityname) where T : new();

        void Delete<T>(T entity, string entityname);

        #endregion
    }
}
