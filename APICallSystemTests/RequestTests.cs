using APICallSystem.APIRequestBuilder;
using APICallSystem.APIRequestBuilder.Query;

namespace APICallSystemTests
{
    internal class RequestTests
    {
        private string[] parameters = ["p0", "p1", "p2", "p3", "p4", "p5"];
        private string[] values = ["v0", "v1", "v2", "v3", "v4", "v5"];

        [Test]
        public void RequestWithSingleInnerElement_GetQueryAsString()
        {
            RequestBuilder requestBuilder = new();
            string key = "";
            requestBuilder.QueryAdd(parameters[0], values[0], ref key);
            Request request = requestBuilder.GetRequest();

            Assert.That(request.GetQueryAsString(), Is.EqualTo(parameters[0] + "=" + values[0]));
        }

        [Test]
        public void RequestWithVariousEntries_GetQueryAsString()
        {
            RequestBuilder requestBuilder = new();
            string key = "";

            requestBuilder.QueryAdd(parameters[0], values[0], ref key)
                .QueryAppend(parameters[0], values[1], ref key)
                .QueryAdd(parameters[1], values[2], ref key);

            requestBuilder.QueryAdd(parameters[3], values[3], ref key, RequestCompareSetting.IncludesAny)
                .QueryAppend(parameters[1], values[1], ref key);

            Request request = requestBuilder.GetRequest();

            Assert.That(request.GetQueryAsString(), Is.EqualTo(parameters[0] + "=" + values[0] + "," + values[1] +
                "&" + parameters[1] + "=" + values[2] + "&" + parameters[1] + "=" + values[1] +
                "&" + parameters[3] + "=" + values[3] + ";" + RequestCompareSetting.IncludesAny));
        }

        [Test]
        public void AddValueTwice_GetQueryAsString()
        {
            RequestBuilder requestBuilder = new();
            string key = string.Empty;

            requestBuilder.QueryAdd(parameters[0], values[0], ref key, RequestCompareSetting.Equals)
                .QueryAdd(parameters[0], values[1], ref key, RequestCompareSetting.Equals);

            Request request = requestBuilder.GetRequest();

            Assert.That(request.GetQueryAsString(), Is.EqualTo(parameters[0] + "=" + values[0] + ";" + RequestCompareSetting.Equals + "&" +
                parameters[0] + "=" + values[1] + ";" + RequestCompareSetting.Equals));
        }

        [Test]
        public void RequestWithNoInnerParts_GetQueryAsString()
        {
            RequestBuilder requestBuilder = new RequestBuilder();
            Request request = requestBuilder.GetRequest();

            Assert.That(request.GetQueryAsString(), Is.EqualTo(""));
        }
    }
}
