namespace APICallSystem.Outdated.API.EventArguments
{
    public class OnRequestFailureEventArgs : EventArgs
    {
        public required string errorData;
        public required HttpResponseMessage response;
    }
}
