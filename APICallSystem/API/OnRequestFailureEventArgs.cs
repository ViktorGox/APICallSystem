namespace APICallSystem.API
{
    public class OnRequestFailureEventArgs : EventArgs
    {
        public required string errorData;
        public required HttpResponseMessage response;
    }
}
