using CustomConsole;

namespace APICallSystem.API
{
    public class APICall
    {
        private RequestType _requestType;
        private string _url;
        public event EventHandler<OnRequestResponse>? OnAnswerReceived;
        public event EventHandler<OnRequestResponse>? OnSuccess;
        public event EventHandler<OnRequestResponse>? OnFailure;
        public class OnRequestResponse : EventArgs
        {
            public required HttpResponseMessage response;
        }

        public APICall(RequestType type, string url, EventHandler<OnRequestResponse>? OnAnswerReceived = null,
            EventHandler<OnRequestResponse>? OnSuccess = null, EventHandler<OnRequestResponse>? OnFailure = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
            _requestType = type;
            _url = url;
            if (OnAnswerReceived != null) this.OnAnswerReceived += OnAnswerReceived;
            if (OnSuccess != null) this.OnSuccess += OnSuccess;
            if (OnFailure != null) this.OnFailure += OnFailure;
        }

        public async Task<string> Execute() => _requestType switch
        {
            { } when _requestType == RequestType.Get => await Get(_url),
            //{ } when _requestType == RequestType.Post => Get(_url),
            _ => throw new ArgumentOutOfRangeException(nameof(_requestType), $"Unsupported request type: {_requestType}")
        };

        private async Task<string> Get(string url)
        {
            using HttpClient client = new();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                OnAnswerReceived?.Invoke(typeof(APICall), new OnRequestResponse { response = response });

                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    OnSuccess?.Invoke(typeof(APICall), new OnRequestResponse { response = response });
                    CConsole.WriteSuccess(responseContent);
                }
                else
                {
                    OnFailure?.Invoke(typeof(APICall), new OnRequestResponse { response = response });
                    CConsole.WriteError(responseContent);
                }
                return responseContent;
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
