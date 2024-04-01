namespace APICallSystem.BackEnd
{
    internal class Context
    {
        private List<object> _entities = [];

        public bool AddEntity<T>(IEntity<T> entity) where T : class
        {
            if (_entities.OfType<IEntity<T>>().Count() > 0) return false;
            _entities.Add(entity);
            return true;
        }

        public IEntity<T>? Get<T>() where T : class
        {
            return _entities.OfType<IEntity<T>>().FirstOrDefault();
        }
    }
}
