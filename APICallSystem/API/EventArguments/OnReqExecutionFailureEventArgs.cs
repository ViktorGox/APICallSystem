namespace APICallSystem.API.EventArguments
{
    internal class OnReqExecutionFailureEventArgs : EventArgs
    {
        public required Exception reason;
    }
}
