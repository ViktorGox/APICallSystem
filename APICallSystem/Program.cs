using APICallSystem.API;
using APICallSystem.BackEnd;
using APICallSystem.Context;
using CustomConsole;

namespace MyApp
{
    internal class Program
    {
        static void Main()
        {
            Context context = new Context(UrlFactory.Create(true, "localhost", 7225, "api"));

            context.AddEntity(new Entity<Message>("Module")); 
            context.AddEntity(new Entity<User>("Test"));

            context.Get<User>()?.Get(new Guid("62b89e37-0a89-4233-5db9-08dc4dcaf70c"), Act);

            Thread.Sleep(3000);
        }

        public async static Task Act<T>(T t)
        {
            await CConsole.WriteLineAsync(t + " ye");
        }
    }
}