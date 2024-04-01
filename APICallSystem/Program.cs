using APICallSystem.BackEnd;
using APICallSystem.Context;

namespace MyApp
{
    internal class Program
    {
        static void Main()
        {
            Context context = new Context();

            context.AddEntity(new Entity<Message>(""));
            context.AddEntity(new Entity<User>(""));

            IEntity<User>? messageEntity = context.Get<User>();
            Console.WriteLine(messageEntity);
            context.Get<User>().Get();
        }
    }
}