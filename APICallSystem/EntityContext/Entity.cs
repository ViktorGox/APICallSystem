using APICallSystem.API;
using APICallSystem.DataAdaptation;
using Newtonsoft.Json;

namespace APICallSystem.EntityContext
{
    internal class Entity<T>(string endPoint, string baseUrl, IHttpReqResponseAdapter responseAdapter, IHttpReqBodyAdapter bodyAdapter)
    {
        private readonly string _endPoint = endPoint;
        private readonly string _baseUrl = baseUrl;
        private readonly IHttpReqResponseAdapter _responseAdapter = responseAdapter;
        private readonly IHttpReqBodyAdapter _bodyAdapter = bodyAdapter;

        public string EndPoint
        {
            get { return _endPoint; }
        }

        public void Get(Guid id, Action<OnRequestSuccessEventArgs<T>>? onSuccess = null, Action<OnRequestFailureEventArgs<T>>? onFailure = null)
        {
            Task.Run(async () =>
            {
                APICall<T> apiCall = new(RequestType.Get, _baseUrl + _endPoint + "/" + id, _responseAdapter, OnSuccess: onSuccess, OnFailure: onFailure);
                await apiCall.Execute();
            });
        }
    }
}
