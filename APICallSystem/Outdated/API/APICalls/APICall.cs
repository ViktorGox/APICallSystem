using APICallSystem.Outdated.API.EventArguments;
using APICallSystem.Outdated.DataAdaptation;

namespace APICallSystem.Outdated.API.APICalls
{
    internal static class APICall<T> where T : class
    {
        private static readonly int TIMEOUT_S = 10;

        public static void Execute(RequestType type, string url, IHttpReqResponseAdapter responseAdapter, IHttpReqBodyAdapter? bodyAdapter = null, T? body = null, Action<OnRequestSuccessEventArgs<T>>? onSuccess = null, Action<OnRequestFailureEventArgs>? onFailure = null)
        {
            using HttpClient client = new();
            client.Timeout = TimeSpan.FromSeconds(TIMEOUT_S);

            HttpResponseMessage? response = null;

            if (CanHaveBody(type))
            {
                if (bodyAdapter == null || body == null) throw new ArgumentException("BodyAdapter or provided body is null.");
                StringContent content = new(bodyAdapter.Convert(body), System.Text.Encoding.UTF8, "application/json");
                response = HandleRequest(type, client, url, content);
            }
            else
            {
                response = HandleRequest(type, client, url);
            }

            string responseContent = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                T? entity = responseAdapter.Convert<T>(responseContent);
                onSuccess?.Invoke(new OnRequestSuccessEventArgs<T>
                {
                    response = response,
                    entity = entity
                });
            }
            else
            {
                onFailure?.Invoke(new OnRequestFailureEventArgs { response = response, errorData = responseContent });
            }
        }

        private static bool CanHaveBody(RequestType type) => type switch
        {
            { } when type == RequestType.Get => false,
            { } when type == RequestType.Delete => false,
            { } when type == RequestType.Post => true,
            { } when type == RequestType.Put => true,
            { } when type == RequestType.Patch => true,
            _ => throw new NotSupportedException($"Unsupported request type: {type}")
        };

        private static HttpResponseMessage HandleRequest(RequestType type, HttpClient client, string url, StringContent? content = null) => type switch
        {
            { } when type == RequestType.Get => client.GetAsync(url).Result,
            { } when type == RequestType.Delete => client.DeleteAsync(url).Result,
            { } when type == RequestType.Post => client.PostAsync(url, content).Result,
            { } when type == RequestType.Put => client.PutAsync(url, content).Result,
            { } when type == RequestType.Patch => client.PatchAsync(url, content).Result,
            _ => throw new NotSupportedException($"Unsupported request type: {type}")
        };
    }
}
