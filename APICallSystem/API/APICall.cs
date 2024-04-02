using APICallSystem.DataAdaptation;
using CustomConsole;
using System.Linq.Expressions;

namespace APICallSystem.API
{
    public class APICall<T>
    {
        private RequestType _requestType;
        private string _url;
        public event Action<OnRequestSuccessEventArgs<T>>? OnSuccess;
        public event Action<OnRequestFailureEventArgs<T>>? OnFailure;
        private IHttpReqResponseAdapter _responseAdapter;
        private IHttpReqBodyAdapter? _bodyAdapter;

        public APICall(RequestType type, string url, IHttpReqResponseAdapter responseAdapter, IHttpReqBodyAdapter? bodyAdapter = null,
            Action<OnRequestSuccessEventArgs<T>>? OnSuccess = null, Action<OnRequestFailureEventArgs<T>>? OnFailure = null)
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

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    T? entity = _responseAdapter.Convert<T>(responseContent);
                    OnSuccess?.Invoke(new OnRequestSuccessEventArgs<T> { response = response, entity = entity });
                    CConsole.WriteSuccess(responseContent);
                }
                else
                {
                    OnFailure?.Invoke(new OnRequestFailureEventArgs<T> { response = response, errorData = responseContent });
                    CConsole.WriteError(responseContent);
                }
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }



        //public static async Task Method()
        //{
        //    using HttpClient client = new();
        //    try
        //    {
        //        // Create a JSON string for the request body
        //        string jsonBody = "{\"label\": \"value\"}";

        //        // Create the HttpContent object with JSON content
        //        HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //        // Send the POST request with the specified body
        //        HttpResponseMessage response = await client.PostAsync("https://localhost:7225/api/Module", content);
        //        // Check if the response is successful
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string responseBody = await response.Content.ReadAsStringAsync();
        //            CConsole.WriteLine("Response: " + responseBody);
        //        }
        //        else
        //        {
        //            CConsole.WriteLine("Request failed with status code " + response.StatusCode);
        //        }
        //    }
        //    catch (HttpRequestException e)
        //    {
        //        CConsole.WriteLine("Request failed: " + e.Message);
        //    }
        //}
    }
}
