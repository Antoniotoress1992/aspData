using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IgniteUI.SamplesBrowser.Models.Repositories
{
    interface IRepository<T>
    {
        IEnumerable<T> Get();
        T Get(Predicate<T> condition);
        bool Update(T item, Predicate<T> condition);
        T Add(T item);
        int Delete(Predicate<T> condition);
    }
}
