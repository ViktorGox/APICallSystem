using APICallSystem.API;
using APICallSystem.BackEnd;
using APICallSystem.Query;
using Newtonsoft.Json;

namespace APICallSystem.Context
{
    internal class Entity<T> : IEntity<T> where T : class
    {
        private readonly string _endPoint;
        private string? _mainUrl;

        public string EndPoint
        {
            get { return _endPoint; }
        }

        public string MainUrl
        {
            set
            {
                if (_mainUrl is not null && _mainUrl != value)
                {
                    throw new InvalidOperationException("Invalid attempt to reassign the main url. Possibly reusing by accident.");
                }
                _mainUrl = value;
            }
        }

        public Entity(string endPoint)
        {
            _endPoint = endPoint;
        }

        public void Get(Guid id, Func<T, Task> executable)
        {
            Task.Run(async () =>
            {
                string response = await APICall.Get(_mainUrl + _endPoint + "/" + id);
                T? result = JsonConvert.DeserializeObject<T>(response);
                await executable(result);
            });
        }

        public void Get(IQuery? query = null)
        {
            throw new NotImplementedException();
        }

        public void Post(T t)
        {
            throw new NotImplementedException();
        }
    }
}
