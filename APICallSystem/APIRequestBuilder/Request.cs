using APICallSystem.APIRequestBuilder.Query;
using APICallSystem.Outdated.DataAdaptation;
using System.Text;

namespace APICallSystem.APIRequestBuilder
{
    public class Request
    {
        /// <summary>
        /// Holds all data related to queries.
        /// </summary>
        public Dictionary<RequestQueryKeyPair, Dictionary<string, List<string>>> QueryPairs { get; private set; } = [];
        public object? body { get; set; }

        public string GetQueryAsString()
        {
            StringBuilder sb = new();
            for (int k = 0; k < QueryPairs.Count; k++)
            {
                KeyValuePair<RequestQueryKeyPair, Dictionary<string, List<string>>> parameterSettingPair = QueryPairs.ElementAt(k);
                int parameterSettingPairCount = parameterSettingPair.Value.Count;

                for (int j = 0; j < parameterSettingPairCount; j++)
                {
                    KeyValuePair<string, List<string>> innerPair = parameterSettingPair.Value.ElementAt(j);
                    int innerPairCount = innerPair.Value.Count;

                    sb.Append(parameterSettingPair.Key.ParameterName).Append('=');

                    for (int i = 0; i < innerPairCount; i++)
                    {
                        sb.Append(innerPair.Value[i]);
                        if (i < innerPairCount - 1) sb.Append(',');
                    }
                    if (parameterSettingPair.Key.Setting != RequestCompareSetting.None)
                    {
                        sb.Append(';').Append(parameterSettingPair.Key.Setting.ToString());
                    }
                    if (k != QueryPairs.Count - 1 || j != parameterSettingPairCount - 1) sb.Append('&');
                }
            }
            return sb.ToString();
        }

        public string GetBodyAsString(IHttpReqBodyAdapter bodyAdapter)
        {
            ArgumentNullException.ThrowIfNull(bodyAdapter);
            if (body == null) return "";
            return bodyAdapter.Convert(body);
        }
    }
}
