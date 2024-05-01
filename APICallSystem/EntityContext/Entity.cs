using APICallSystem.API.APICalls;
using APICallSystem.API.EventArguments;
using APICallSystem.DataAdaptation;

namespace APICallSystem.EntityContext
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
            GetAPICall<T> getApiCall = new(_baseUrl + _endPoint + "/" + id, _responseAdapter, onSuccess, onFailure);
            Prepare(getApiCall, onError);
        }

        public void Post(T body, Action<OnRequestSuccessEventArgs<T>>? onSuccess = null, Action<OnRequestFailureEventArgs>? onFailure = null, Action<OnReqExecutionFailureEventArgs>? onError = null) 
        {
            PostAPICall<T> postApiCall = new(_baseUrl + _endPoint, _responseAdapter, _bodyAdapter, body, onSuccess, onFailure);
            Prepare(postApiCall, onError);
        }

        private void Prepare(IAPICall call, Action<OnReqExecutionFailureEventArgs>? onError = null)
        {
            Thread thread = new(() => Entity<T>.ExecuteCall(call, onError));
            thread.Start();
        }

        private static void ExecuteCall(IAPICall call, Action<OnReqExecutionFailureEventArgs>? onError = null)
        {
            try
            {
                call.Execute();
            }
            catch (Exception ex)
            {
                if (onError == null) return;
                onError(new OnReqExecutionFailureEventArgs() { reason = ex });
            }
        }
    }
}
