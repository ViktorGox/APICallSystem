using APICallSystem.API.EventArguments;
using APICallSystem.DataAdaptation;

namespace APICallSystem.API
{
    public class APICall<T>
    {
        private RequestType _requestType;
        private string _url;
        public event Action<OnRequestSuccessEventArgs<T>>? OnSuccess;
        public event Action<OnRequestFailureEventArgs>? OnFailure;
        private IHttpReqResponseAdapter _responseAdapter;
        private IHttpReqBodyAdapter? _bodyAdapter;

        public APICall(RequestType type, string url, IHttpReqResponseAdapter responseAdapter, IHttpReqBodyAdapter? bodyAdapter = null,
            Action<OnRequestSuccessEventArgs<T>>? OnSuccess = null, Action<OnRequestFailureEventArgs>? OnFailure = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
            _requestType = type;
            _url = url;
            if (OnSuccess != null) this.OnSuccess += OnSuccess;
            if (OnFailure != null) this.OnFailure += OnFailure;
            _responseAdapter = responseAdapter;
            _bodyAdapter = bodyAdapter;
        }

        public async Task Execute()
        {
            if (_requestType is RequestType.Get) await Get(_url);
            //else if (_requestType is RequestType.Post) await Get(_url);
            else throw new InvalidOperationException("Unsupported enum.");
        }

        private async Task Get(string url)
        {
            using HttpClient client = new();

            HttpResponseMessage response = await client.GetAsync(url);

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
