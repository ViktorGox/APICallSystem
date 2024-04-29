using APICallSystem.API.EventArguments;
using APICallSystem.DataAdaptation;

namespace APICallSystem.API.APICalls
{
    internal class GetAPICall<T> where T : class
    {
        private string _url;
        private IHttpReqResponseAdapter _responseAdapter;
        private event Action<OnRequestSuccessEventArgs<T>>? OnSuccess;
        private event Action<OnRequestFailureEventArgs>? OnFailure;

        public GetAPICall(string url, IHttpReqResponseAdapter responseAdapter, Action<OnRequestSuccessEventArgs<T>>? onSuccess = null, Action<OnRequestFailureEventArgs>? onFailure = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
            _url = url;
            _responseAdapter = responseAdapter;
            if (onSuccess != null) OnSuccess += onSuccess;
            if (onFailure != null) OnFailure += onFailure;
        }

        public async Task Execute()
        {
            using HttpClient client = new();

            HttpResponseMessage response = await client.GetAsync(_url);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                T? entity = _responseAdapter.Convert<T>(responseContent);
                OnSuccess?.Invoke(new OnRequestSuccessEventArgs<T> { response = response, entity = entity });
            }
            else
            {
                OnFailure?.Invoke(new OnRequestFailureEventArgs { response = response, errorData = responseContent });
            }
        }
    }
}
