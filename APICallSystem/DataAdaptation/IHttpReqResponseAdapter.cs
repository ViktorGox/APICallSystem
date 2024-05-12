namespace APICallSystem.DataAdaptation
{
    public interface IHttpReqResponseAdapter
    {
        T? Convert<T>(string data);
    }
}
