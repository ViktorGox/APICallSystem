namespace APICallSystem.Outdated.API.EventArguments
{
    internal class OnReqExecutionFailureEventArgs : EventArgs
    {
        public required Exception reason;
    }
}
