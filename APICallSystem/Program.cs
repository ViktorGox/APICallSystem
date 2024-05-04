using APICallSystem.API.EventArguments;
using APICallSystem.APIRequestBuilder;
using APICallSystem.APIRequestBuilder.Query;
using CustomConsole;

namespace MyApp
{
    internal class Program
    {
        static void Main()
        {
            RequestBuilder builder = new RequestBuilder();

            string innerKey = "CustomKey";
            builder.QueryAdd("name", "MyValue", ref innerKey, RequestCompareSetting.Equals)
                .InnerKeyChange(ref innerKey, "")
                .QueryAppend("name", "ValueOne1", ref innerKey, RequestCompareSetting.Equals)
                .QueryAppend("name", "ValueOne2", ref innerKey, RequestCompareSetting.Equals)
                .QueryAppend("name", "ValueOne3", ref innerKey, RequestCompareSetting.Equals)
                .QueryAdd("name", "ValueOne4", ref innerKey, RequestCompareSetting.Equals)
                .QueryAdd("name1", "ValueOne5", ref innerKey, RequestCompareSetting.Equals)
                .QueryAdd("name1", "ValueOne6", ref innerKey, RequestCompareSetting.NotEquals);

            builder.Print();
            innerKey = "CustomKey";
            builder.QueryRemove("name", "MyValue", ref innerKey, RequestCompareSetting.Equals);
            CConsole.WriteLine();
            builder.Print();

            //JSONHttpReqResponseAdapter jsonResponseAdapter = new();
            //JSONHttpReqBodyAdapter jsonBodyAdapter = new();

            //Context context = new(UrlFactory.Create(true, "localhost", 7225, "api"), jsonResponseAdapter, jsonBodyAdapter);

            //context.AddEntity(typeof(Message), "Module");
            //context.AddEntity(typeof(User), "Test");

            //User dummyUser = new("112", "Obvious name", "@av");

            //context.Get<User>()?.Get(new Guid("62b89e37-0a89-4233-5db9-08dc4dcaf70c"), OnSuccess, OnFailure, OnError);
            //context.Get<User>()?.Post(dummyUser, OnSuccess, OnFailure);
        }

        public static void OnSuccess<T>(OnRequestSuccessEventArgs<T> onRequestSuccessEventArgs)
        {
            CConsole.WriteLine(onRequestSuccessEventArgs.entity + " Success");
        }

        public static void OnFailure(OnRequestFailureEventArgs onRequestFailureEventArgs)
        {
            CConsole.WriteLine(onRequestFailureEventArgs.errorData + " Failure");
        }

        public static void OnError(OnReqExecutionFailureEventArgs onReqExecutionFailureEventArgs)
        {
            throw onReqExecutionFailureEventArgs.reason;
        }
    }
}