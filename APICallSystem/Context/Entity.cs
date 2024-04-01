using APICallSystem.BackEnd;
using APICallSystem.Query;
using Newtonsoft.Json;

namespace APICallSystem.Context
{
    internal class Entity<T> : IEntity<T> where T : class
    {
        private string _endPoint;

        public string EndPoint
        {
            get { return _endPoint; }
        }

        public Entity(string endPoint)
        {
            _endPoint = endPoint;
        }


        public T Get(Guid id)
        {
            string data = "{\r\n    \"Id\": 2,\r\n    \"Title\": \"a\",\r\n    \"Body\": \"sdfsd\"\r\n}";

            // Deserialize JSON to object of type T
            T result = JsonConvert.DeserializeObject<T>(data);

            return result;
        }

        public ICollection<T> Get(IQuery? query = null)
        {
            return [];
        }

        public void Post(T t)
        {
            throw new NotImplementedException();
        }
    }
}
