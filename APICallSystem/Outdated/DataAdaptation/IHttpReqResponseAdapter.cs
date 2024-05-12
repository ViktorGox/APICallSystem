namespace APICallSystem.Outdated.DataAdaptation
{
    public interface IHttpReqResponseAdapter
    {
        T? Convert<T>(string data);
    }
}
