using APICallSystem.API.EventArguments;
using APICallSystem.DataAdaptation;

namespace APICallSystem.API.APICalls
{
    public class PostAPICall<T> : IAPICall where T : class
    {
        private readonly string _url;
        private readonly T _body;
        private readonly IHttpReqResponseAdapter _responseAdapter;
        private readonly IHttpReqBodyAdapter _bodyAdapter;
        private event Action<OnRequestSuccessEventArgs<T>>? OnSuccess;
        private event Action<OnRequestFailureEventArgs>? OnFailure;

        public PostAPICall(string url, IHttpReqResponseAdapter responseAdapter, IHttpReqBodyAdapter bodyAdapter, T body,
            Action<OnRequestSuccessEventArgs<T>>? onSuccess = null, Action<OnRequestFailureEventArgs>? onFailure = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
            _url = url;
            _body = body;
            _responseAdapter = responseAdapter;
            _bodyAdapter = bodyAdapter;
            if (onSuccess != null) OnSuccess += onSuccess;
            if (onFailure != null) OnFailure += onFailure;
        }

        public void Execute()
        {
            ExecuteAsync().Wait();
        }

        private async Task ExecuteAsync()
        {
            using HttpClient client = new();

            StringContent content = new(_bodyAdapter.Convert(_body), System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(_url, content);
            await Console.Out.WriteLineAsync(_url);
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
