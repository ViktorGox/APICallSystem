using APICallSystem.APIRequestBuilder.Query;
using System.Text;

namespace APICallSystem.APIRequestBuilder
{
    public class Request
    {
        /// <summary>
        /// Holds all data related to queries. Queries might be separated 
        /// </summary>
        public Dictionary<RequestQueryKeyPair, Dictionary<string, List<string>>> QueryPairs { get; private set; } = [];

        public string GetQueryAsString()
        {
            StringBuilder sb = new();
            for (int k = 0; k < QueryPairs.Count; k++)
            {
                var parameterSettingPair = QueryPairs.ElementAt(k);
                var parameterSettingPairCount = parameterSettingPair.Value.Count;
                for (int j = 0; j < parameterSettingPairCount; j++)
                {
                    var innerPair = parameterSettingPair.Value.ElementAt(j);
                    var innerPairCount = innerPair.Value.Count;

                    sb.Append(parameterSettingPair.Key.ParameterName).Append('=');
                    for (int i = 0; i < innerPairCount; i++)
                    {
                        sb.Append(innerPair.Value[i]);
                        if (i < innerPairCount - 1) sb.Append(',');
                    }
                    if (parameterSettingPair.Key.Setting != RequestCompareSetting.None) sb.Append(';').Append(parameterSettingPair.Key.Setting.ToString());

                    if (k != QueryPairs.Count - 1 || j != parameterSettingPairCount - 1) sb.Append('&');
                }
            }
            return sb.ToString();
        }
    }
}
