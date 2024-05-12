using APICallSystem.Outdated.API.APICalls;
using APICallSystem.Outdated.API.EventArguments;
using APICallSystem.Outdated.DataAdaptation;

namespace APICallSystem.Outdated.EntityContext
{
    /// <summary>
    /// Represents entity present in the back end, allows for calling available end points.
    /// </summary>
    internal class Entity<T>(string endPoint, string baseUrl, IHttpReqResponseAdapter responseAdapter, IHttpReqBodyAdapter bodyAdapter) where T : class
    {
        private readonly string _endPoint = endPoint;
        private readonly string _baseUrl = baseUrl;
        private readonly IHttpReqResponseAdapter _responseAdapter = responseAdapter;
        private readonly IHttpReqBodyAdapter _bodyAdapter = bodyAdapter;

        public void Get(Guid id, Action<OnRequestSuccessEventArgs<T>>? onSuccess = null, Action<OnRequestFailureEventArgs>? onFailure = null, Action<OnReqExecutionFailureEventArgs>? onError = null)
        {
            ExecuteCall(RequestType.Get, _baseUrl + _endPoint + "/" + id, onSuccess, onFailure, onError);
        }

        public void Post(T body, Action<OnRequestSuccessEventArgs<T>>? onSuccess = null, Action<OnRequestFailureEventArgs>? onFailure = null, Action<OnReqExecutionFailureEventArgs>? onError = null)
        {
            ExecuteCall(RequestType.Post, _baseUrl + _endPoint, onSuccess, onFailure, onError, body);
        }

        public void Delete(Guid id, Action<OnRequestSuccessEventArgs<T>>? onSuccess = null, Action<OnRequestFailureEventArgs>? onFailure = null, Action<OnReqExecutionFailureEventArgs>? onError = null)
        {
            ExecuteCall(RequestType.Delete, _baseUrl + _endPoint + "/" + id, onSuccess, onFailure, onError);
        }

        public void Put(Guid id, T body, Action<OnRequestSuccessEventArgs<T>>? onSuccess = null, Action<OnRequestFailureEventArgs>? onFailure = null, Action<OnReqExecutionFailureEventArgs>? onError = null)
        {
            ExecuteCall(RequestType.Put, _baseUrl + _endPoint + "/" + id, onSuccess, onFailure, onError, body);
        }

        public void Patch(Guid id, T body, Action<OnRequestSuccessEventArgs<T>>? onSuccess = null, Action<OnRequestFailureEventArgs>? onFailure = null, Action<OnReqExecutionFailureEventArgs>? onError = null)
        {
            ExecuteCall(RequestType.Patch, _baseUrl + _endPoint + "/" + id, onSuccess, onFailure, onError, body);
        }
        private void ExecuteCall(RequestType type, string url, Action<OnRequestSuccessEventArgs<T>>? onSuccess = null, Action<OnRequestFailureEventArgs>? onFailure = null, Action<OnReqExecutionFailureEventArgs>? onError = null, T? body = null)
        {
            try
            {
                Thread thread = new(() => APICall<T>.Execute(type, url, _responseAdapter, _bodyAdapter, body, onSuccess, onFailure));
                thread.Start();
            }
            catch (Exception ex)
            {
                if (onError == null) return;
                onError(new OnReqExecutionFailureEventArgs() { reason = ex });
            }
        }
    }
}
