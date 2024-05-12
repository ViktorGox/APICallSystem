namespace APICallSystem.DataAdaptation
{
    public interface IHttpReqBodyAdapter
    {
        string Convert<T>(T t);
    }
}
