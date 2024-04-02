using APICallSystem.Query;

namespace APICallSystem.BackEnd
{
    internal interface IEntity<T> where T : class
    {
        string MainUrl { set; }
        string EndPoint { get; }
        void Get(Guid id, Func<T?, Task> executable);
        void Get(IQuery? query = null);
        void Post(T t);
    }
}
