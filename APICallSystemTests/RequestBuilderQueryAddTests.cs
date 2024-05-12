using APICallSystem.APIRequestBuilder;
using APICallSystem.APIRequestBuilder.Query;

namespace APICallSystemTests
{
    public class RequestBuilderQueryAddTests
    {
        private readonly string[] parameters = ["p0", "p1", "p2", "p3", "p4", "p5"];
        private readonly string[] values = ["v0", "v1", "v2", "v3", "v4", "v5"];

        [Test]
        public void EmptyRequest_AddValue_CreatesANewValueWithTheGivenData()
        {
            RequestBuilder requestBuilder = new();
            string key = string.Empty;
            requestBuilder.QueryAdd(parameters[0], values[0], ref key, RequestCompareSetting.Equals);

            Request request = requestBuilder.GetRequest();

            Assert.That(request.GetQueryAsString(), Is.EqualTo(parameters[0] + "=" + values[0] + ";" + RequestCompareSetting.Equals));
        }

        [Test]
        public void EmptyRequest_AppendValue_CreatesANewValueWithTheGivenData()
        {
            RequestBuilder requestBuilder = new();
            string key = string.Empty;
            requestBuilder.QueryAppend(parameters[0], values[0], ref key, RequestCompareSetting.Equals);

            Request request = requestBuilder.GetRequest();

            Assert.That(request.GetQueryAsString(), Is.EqualTo(parameters[0] + "=" + values[0] + ";" + RequestCompareSetting.Equals));
        }

        [Test]
        public void EmptyRequest_AddValueAndAppendOnSamePair_CreatesANewValueAndSecondIsAddedToTheSameInnerKey()
        {
            RequestBuilder requestBuilder = new();
            string key = string.Empty;

            requestBuilder.QueryAdd(parameters[0], values[0], ref key, RequestCompareSetting.Equals)
                .QueryAppend(parameters[0], values[1], ref key, RequestCompareSetting.Equals);

            Request request = requestBuilder.GetRequest();

            Assert.That(request.GetQueryAsString(), Is.EqualTo(parameters[0] + "=" + values[0] + "," + values[1] + ";" + RequestCompareSetting.Equals));
        }

        [Test]
        public void EmptyRequest_AppendValueTwice_AddsBothValuesToTheSameInnerKey()
        {
            RequestBuilder requestBuilder = new();
            string key = string.Empty;

            requestBuilder.QueryAppend(parameters[0], values[0], ref key, RequestCompareSetting.Equals)
                .QueryAppend(parameters[0], values[1], ref key, RequestCompareSetting.Equals);

            Request request = requestBuilder.GetRequest();

            Assert.That(request.GetQueryAsString(), Is.EqualTo(parameters[0] + "=" + values[0] + "," + values[1] + ";" + RequestCompareSetting.Equals));
        }

        [Test]
        public void EmptyRequest_AddValueTwice_AddsBothValuesInTwoSeparateKeys()
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
        public void EmptyRequest_AppendingValueWithNewSetting_CreatesANewInnerKey()
        {
            RequestBuilder requestBuilder = new();
            string key = string.Empty;

            requestBuilder.QueryAdd(parameters[0], values[0], ref key, RequestCompareSetting.Equals).
                QueryAppend(parameters[0], values[1], ref key, RequestCompareSetting.NotEquals);

            Request request = requestBuilder.GetRequest();

            Assert.That(request.GetQueryAsString(), Is.EqualTo(parameters[0] + "=" + values[0] + ";" + RequestCompareSetting.Equals + "&" +
                parameters[0] + "=" + values[1] + ";" + RequestCompareSetting.NotEquals));
        }

        [Test]
        public void EmptyRequest_AppendingValueWithNewParameter_CreatesANewInnerKey()
        {
            RequestBuilder requestBuilder = new();
            string key = string.Empty;

            requestBuilder.QueryAdd(parameters[0], values[0], ref key, RequestCompareSetting.Equals).
                QueryAppend(parameters[1], values[1], ref key, RequestCompareSetting.Equals);

            Request request = requestBuilder.GetRequest();

            Assert.That(request.GetQueryAsString(), Is.EqualTo(parameters[0] + "=" + values[0] + ";" + RequestCompareSetting.Equals + "&" +
                parameters[1] + "=" + values[1] + ";" + RequestCompareSetting.Equals));
        }

        [Test]
        public void EmptyRequest_AppendingValueBySettingOldKey_AppendsTheValueToTheOldLocation()
        {
            RequestBuilder requestBuilder = new();
            string key = string.Empty;
            string copy = string.Empty;

            requestBuilder.QueryAdd(parameters[0], values[0], ref key, RequestCompareSetting.Equals)
                .KeySave(ref key, ref copy)
                .QueryAdd(parameters[1], values[1], ref key, RequestCompareSetting.Equals)
                .QueryAppend(parameters[0], values[1], ref copy, RequestCompareSetting.Equals);

            requestBuilder.Print();

            Request request = requestBuilder.GetRequest();

            Assert.That(request.GetQueryAsString(), Is.EqualTo(parameters[0] + "=" + values[0] + "," + values[1] + ";" + RequestCompareSetting.Equals + "&" +
                parameters[1] + "=" + values[1] + ";" + RequestCompareSetting.Equals));
        }
    }
}