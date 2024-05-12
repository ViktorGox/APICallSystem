namespace APICallSystem.Outdated.DataAdaptation
{
    public interface IHttpReqBodyAdapter
    {
        string Convert<T>(T t);
    }
}
