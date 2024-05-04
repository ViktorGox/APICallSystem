namespace APICallSystem.APIRequestBuilder.Query
{
    /// <summary>
    /// Represents key for the dictionary used in <see cref="RequestBuilder"/> for the query data. 
    /// Instances with the same parameter name and setting will return true when compared.
    /// </summary>
    /// <param name="setting">The setting associated with the variable. Defaults to <see cref="RequestCompareSetting.None"/>.</param>
    /// <param name="parameter">Name of the variable/property of the represented model class.</param>
    internal sealed class RequestQueryKeyPair(string parameter, RequestCompareSetting setting = RequestCompareSetting.None)
    {
        public string ParameterName { get; private set; } = parameter;
        public RequestCompareSetting Setting { get; private set; } = setting;

        public override int GetHashCode()
        {
            return HashCode.Combine(ParameterName, Setting);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            RequestQueryKeyPair other = (RequestQueryKeyPair)obj;
            return ParameterName == other.ParameterName && Setting == other.Setting;
        }
    }
}
