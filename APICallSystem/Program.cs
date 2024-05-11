using APICallSystem.API.EventArguments;
using APICallSystem.APIRequestBuilder;
using APICallSystem.APIRequestBuilder.Query;
using CustomConsole;
using System.Text;

namespace MyApp
{
    internal class Program
    {
        static void Main()
        {
            RequestBuilder builder = new();

            string innerKey = "CustomKey";
            builder.QueryAppend("label", "MyValue", ref innerKey, RequestCompareSetting.IncludesAny)
                .KeyChange(ref innerKey, "SecondCustomKey")
                .QueryAppend("label", "ValueOne1", ref innerKey, RequestCompareSetting.IncludesAnyEndCS)
                .QueryAppend("label", "ValueOne2", ref innerKey, RequestCompareSetting.IncludesAnyEndCS)
                .QueryAppend("label", "ValueOne3", ref innerKey, RequestCompareSetting.IncludesAnyEndCS)
                .QueryAdd("description", "ValueOne4", ref innerKey, RequestCompareSetting.Equals)
                .QueryAdd("description", "ValueOne5", ref innerKey, RequestCompareSetting.Equals)
                .QueryAdd("description", "ValueOne6", ref innerKey, RequestCompareSetting.NotEquals);

            CConsole.WriteLine(builder.GetRequest().GetQueryAsString());

            builder.Print();
            innerKey = "CustomKey";
            builder.QueryRemoveFromKey("label", "MyValue", ref innerKey, RequestCompareSetting.IncludesAny);
            innerKey = "SecondCustomKey";
            builder.QueryRemoveKey("label", ref innerKey, RequestCompareSetting.IncludesAnyEndCS);
            builder.QueryRemovePair("label", RequestCompareSetting.IncludesAnyEndCS);
            CConsole.WriteLine();
            builder.Print();
            CConsole.WriteLine(builder.GetRequest().GetQueryAsString());

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