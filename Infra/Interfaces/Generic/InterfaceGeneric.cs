using System.Collections.Generic;

namespace Data.Interfaces.Generic
{
    public interface InterfaceGeneric<T> where T : class
    {
        void Add(T Entity);
        void Update(T Entity);
        void Remove(T Entity);
        T GetById(int Id);
        List<T> GetAll();
    }
}
