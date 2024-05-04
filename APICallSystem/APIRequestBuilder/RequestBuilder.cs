using APICallSystem.APIRequestBuilder.Query;
using CustomConsole;

namespace APICallSystem.APIRequestBuilder
{
    internal class RequestBuilder
    {
        // TODO: private after testing.
        public readonly Dictionary<RequestQueryKeyPair, Dictionary<string, string>> queryPairs = [];
        private int currentInnerKeyIndex = 0;

        /// <summary>
        /// Creates new parameter and value pair, does not append value to existing pair.
        /// </summary>
        /// <param name="parameterName">Name of </param>
        /// <param name="value"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public RequestBuilder QueryAdd(string parameterName, string value, ref string innerKey, RequestCompareSetting setting = RequestCompareSetting.None)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(parameterName, nameof(parameterName));
            ArgumentNullException.ThrowIfNull(value);

            RequestQueryKeyPair newKeyPair = new(parameterName, setting);

            if (!queryPairs.ContainsKey(newKeyPair))
            {
                queryPairs[newKeyPair] = [];
            }

            if (string.IsNullOrWhiteSpace(innerKey))
            {
                innerKey = currentInnerKeyIndex.ToString();
                currentInnerKeyIndex++;
            }

            if (!queryPairs[newKeyPair].ContainsKey(innerKey))
            {
                queryPairs[newKeyPair].Add(innerKey, value);
            }
            else
            {
                queryPairs[newKeyPair][innerKey] += ", " + value;
            }

            return this;
        }

        /// <summary>
        /// Allows changing value of the innerKey without breaking the chain.
        /// </summary>
        /// <param name="innerKey">The key reference.</param>
        /// <param name="newValue">New value that will be assigned to it.</param>
        /// <returns></returns>
        public RequestBuilder InnerKeyChange(ref string innerKey, string newValue)
        {
            innerKey = newValue;
            return this;
        }

        /// <summary>
        /// Appends value to existing parameter and value pair. Creates new pair if one does not exist.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        //public RequestBuilder QueryAppend(string parameterName, object value, RequestCompareSetting setting = RequestCompareSetting.Equals) { }

        //public RequestBuilder QueryRemovePair(string parameterName, string value) { }

        //public RequestBuilder QueryRemove(string parameterName) { }
        //public RequestBuilder DummyQueryRemove(string parameterName, ref int value) 
        //{
        //    value += 2;
        //    return this;
        //}
        public void Print()
        {
            foreach (var outerPair in queryPairs)
            {
                CConsole.WriteLine($"Key: {outerPair.Key.ParameterName}, {outerPair.Key.Setting}");

                foreach (var innerPair in outerPair.Value)
                {
                    CConsole.WriteLine($"Inner Key: {innerPair.Key}, Value: {innerPair.Value}");
                }
                CConsole.WriteLine("");
            }
        }
    }
}
