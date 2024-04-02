using Newtonsoft.Json;

namespace APICallSystem.DataAdaptation
{
    internal class JSONHttpReqResponseAdapter : IHttpReqResponseAdapter
    {
        public T? Convert<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
