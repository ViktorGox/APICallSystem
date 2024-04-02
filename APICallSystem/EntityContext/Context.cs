using APICallSystem.DataAdaptation;
using System.Reflection;

namespace APICallSystem.EntityContext
{
    internal class Context(string baseUrl, IHttpReqResponseAdapter responseAdapter, IHttpReqBodyAdapter bodyAdapter)
    {
        internal protected readonly string _baseUrl = baseUrl;
        private readonly List<object> _entities = [];
        private readonly IHttpReqResponseAdapter _responseAdapter = responseAdapter;
        private readonly IHttpReqBodyAdapter _bodyAdapter = bodyAdapter;

        public void AddEntity(Type t, string resourcePathUrl)
        {
            Type entityType = typeof(Entity<>).MakeGenericType(t);
            ConstructorInfo? constructor = entityType.GetConstructor([typeof(string), typeof(string), typeof(IHttpReqResponseAdapter), typeof(IHttpReqBodyAdapter)]);

            if (constructor is null) throw new InvalidOperationException("Constructor failed to be found! Could not create entity class instance.");

            object entityInstance = constructor.Invoke([resourcePathUrl, _baseUrl, _responseAdapter, _bodyAdapter]);
            _entities.Add(entityInstance);
        }

        public Entity<T>? Get<T>() where T : class
        {
            return _entities.OfType<Entity<T>>().FirstOrDefault();
        }
    }
}
