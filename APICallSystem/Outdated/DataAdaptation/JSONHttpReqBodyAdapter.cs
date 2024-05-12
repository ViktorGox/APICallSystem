using Newtonsoft.Json;

namespace APICallSystem.Outdated.DataAdaptation
{
    internal class JSONHttpReqBodyAdapter : IHttpReqBodyAdapter
    {
        public string Convert<T>(T t)
        {
            return JsonConvert.SerializeObject(t);
        }
    }
}
