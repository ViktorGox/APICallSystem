using APICallSystem.Query;

namespace APICallSystem.BackEnd
{
    internal interface IEntity<T> where T : class
    {
        string EndPoint { get; }
        T Get(Guid id);
        ICollection<T> Get(IQuery? query = null);
        void Post(T t);
    }
}
