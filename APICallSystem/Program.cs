using APICallSystem.API;
using APICallSystem.API.EventArguments;
using APICallSystem.BackEnd;
using APICallSystem.DataAdaptation;
using APICallSystem.EntityContext;
using CustomConsole;

namespace MyApp
{
    internal class Program
    {
        static void Main()
        {
            JSONHttpReqResponseAdapter jsonResponseAdapter = new();
            JSONHttpReqBodyAdapter jsonBodyAdapter = new();

            Context context = new(UrlFactory.Create(true, "localhost", 7225, "api"), jsonResponseAdapter, jsonBodyAdapter);

            context.AddEntity(typeof(Message), "Module"); 
            context.AddEntity(typeof(User), "Test");

            User dummyUser = new User("112", "Obvious name", "@av");

            //context.Get<User>()?.Get(new Guid("62b89e37-0a89-4233-5db9-08dc4dcaf70c"), OnSuccess, OnFailure);
            context.Get<User>()?.Post(dummyUser, OnSuccess, OnFailure);
            Thread.Sleep(16000);
        }

        public static void OnSuccess<T>(OnRequestSuccessEventArgs<T> onRequestSuccessEventArgs)
        {
            CConsole.WriteLine(onRequestSuccessEventArgs.entity + " Success");
        }

        public static void OnFailure(OnRequestFailureEventArgs onRequestFailureEventArgs)
        {
            CConsole.WriteLine(onRequestFailureEventArgs.errorData + " Failure");
        }
    }
}