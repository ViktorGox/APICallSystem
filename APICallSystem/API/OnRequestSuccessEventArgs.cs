namespace APICallSystem.API
{
    public class OnRequestSuccessEventArgs<T> : EventArgs
    {
        public required T entity;
        public required HttpResponseMessage response;
    }
}
