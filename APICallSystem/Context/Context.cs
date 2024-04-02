namespace APICallSystem.BackEnd
{
    internal class Context(string baseUrl)
    {
        private readonly string _baseUrl = baseUrl;
        private readonly List<object> _entities = [];

        public bool AddEntity<T>(IEntity<T> entity) where T : class
        {
            if (_entities.OfType<IEntity<T>>().Any()) return false;
            _entities.Add(entity);
            entity.MainUrl = _baseUrl;
            return true;
        }

        public IEntity<T>? Get<T>() where T : class
        {
            return _entities.OfType<IEntity<T>>().FirstOrDefault();
        }
    }
}
