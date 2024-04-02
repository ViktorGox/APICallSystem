namespace APICallSystem.BackEnd
{
    internal class Context
    {
        private string _baseUrl;
        private List<object> _entities = [];

        public Context(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public bool AddEntity<T>(IEntity<T> entity) where T : class
        {
            if (_entities.OfType<IEntity<T>>().Count() > 0) return false;
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
