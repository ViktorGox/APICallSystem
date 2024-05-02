using Newtonsoft.Json;

namespace APICallSystem.DataAdaptation.JSON
{
    internal class JSONHttpReqBodyAdapter : IHttpReqBodyAdapter
    {
        public string Convert<T>(T t)
        {
            return JsonConvert.SerializeObject(t);
        }
    }
}
