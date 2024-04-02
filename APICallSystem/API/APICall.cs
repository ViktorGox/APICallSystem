using CustomConsole;
using System.Text;

namespace APICallSystem.API
{
    public static class APICall
    {
        public static event EventHandler<OnAnswerReceivedEventArgs>? OnAnswerReceived;
        public class OnAnswerReceivedEventArgs : EventArgs
        {
            public required HttpResponseMessage response;
        }
        public static async Task<string> Get(string url)
        {
            using HttpClient client = new();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                OnAnswerReceived?.Invoke(typeof(APICall), new OnAnswerReceivedEventArgs { response = response});

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    CConsole.WriteLine("Response: " + responseBody);
                    return responseBody;
                }
                else
                {
                    CConsole.WriteLine("Request failed with status code " + response.StatusCode);
                }
            }
            catch (HttpRequestException e)
            {
                CConsole.WriteLine("Request failed: " + e.Message);
            }
            return "";
        }



        public static async Task Method()
        {
            using HttpClient client = new();
            try
            {
                // Create a JSON string for the request body
                string jsonBody = "{\"label\": \"value\"}";

                // Create the HttpContent object with JSON content
                HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Send the POST request with the specified body
                HttpResponseMessage response = await client.PostAsync("https://localhost:7225/api/Module", content);
                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    CConsole.WriteLine("Response: " + responseBody);
                }
                else
                {
                    CConsole.WriteLine("Request failed with status code " + response.StatusCode);
                }
            }
            catch (HttpRequestException e)
            {
                CConsole.WriteLine("Request failed: " + e.Message);
            }
        }
    }
}
