using APICallSystem.API;
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

            var entity = context.Get<User>();
            Console.WriteLine(entity?.ToString());

            context.Get<User>()?.Get(new Guid("62b89e37-0a89-4233-5db9-08dc4dcaf70c"), OnSuccess, onFailure: OnFailure);

            Thread.Sleep(3000);
        }

        public async static Task OnSuccess<T>(T? t)
        {
            await CConsole.WriteLineAsync(t + " Success");
        }

        public static void OnFailure(OnRequestSuccessEventArgs response)
        {
            //await CConsole.WriteLineAsync("Failure");
        }
    }
}