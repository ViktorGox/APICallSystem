using APICallSystem.APIRequestBuilder.Query;

namespace APICallSystem.APIRequestBuilder
{
    /// <summary>
    /// Explain possibilities, allows separation of data... why list inside dictionary inside dictionary?
    /// </summary>
    internal class RequestBuilder
    {
        // TODO: private after testing.
        public readonly Dictionary<RequestQueryKeyPair, Dictionary<string, List<string>>> queryPairs = [];

        /// <summary>
        /// Appends value to existing parameter and value pair. Creates new pair if one does not exist.
        /// </summary>
        /// <param name="parameter">Model class variable/property name that will be used as key in the query.</param>
        /// <param name="value">Value that will be assigned to the key.</param>
        /// <param name="key">The key that will be assigned to the couple of values. Will be set to random Guid if empty or null</param>
        /// <param name="setting">The way the values will be compared.</param>
        /// <returns></returns>
        public RequestBuilder QueryAppend(string parameter, string value, ref string key, RequestCompareSetting setting = RequestCompareSetting.None)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(parameter, nameof(parameter));
            ArgumentNullException.ThrowIfNull(value);

            RequestQueryKeyPair newKeyPair = new(parameter, setting);

            if (!queryPairs.ContainsKey(newKeyPair))
            {
                queryPairs[newKeyPair] = [];
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                key = Guid.NewGuid().ToString();
            }

            if (!queryPairs[newKeyPair].ContainsKey(key))
            {
                queryPairs[newKeyPair].Add(key, []);
            }

            queryPairs[newKeyPair][key].Add(value);

            return this;
        }

        /// <summary>
        /// Creates new parameter and value pair, does not append value to existing pair. 
        /// Key used is a randomly generated Guid which is assigned to the <paramref name="key"/>.
        /// </summary>
        /// <param name="parameter">Model class variable/property name that will be used as key in the query.</param>
        /// <param name="value">Value that will be assigned to the key.</param>
        /// <param name="key"></param>
        /// <param name="setting">The way the values will be compared.</param>
        /// <returns></returns>
        public RequestBuilder QueryAdd(string parameter, string value, ref string key, RequestCompareSetting setting = RequestCompareSetting.None)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(parameter, nameof(parameter));
            ArgumentNullException.ThrowIfNull(value);

            RequestQueryKeyPair newKeyPair = new(parameter, setting);

            if (!queryPairs.ContainsKey(newKeyPair))
            {
                queryPairs[newKeyPair] = [];
            }

            if (queryPairs[newKeyPair].ContainsKey(key)) 
            {
                key = Guid.NewGuid().ToString();
            }

            queryPairs[newKeyPair][key] = [];
            queryPairs[newKeyPair][key].Add(value);

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

        public RequestBuilder QueryRemove(string parameterName, string value, ref string key, RequestCompareSetting setting = RequestCompareSetting.None)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(parameterName, nameof(parameterName));
            RequestQueryKeyPair newKeyPair = new(parameterName, setting);

            if (string.IsNullOrWhiteSpace(key)) return this;
            if (!queryPairs.ContainsKey(newKeyPair)) return this;
            if (queryPairs[newKeyPair][key] == null) return this;

            queryPairs[newKeyPair][key].Remove(value);
            if(queryPairs[newKeyPair][key].Count == 0)
            {
                queryPairs[newKeyPair].Remove(key);
            } 

            return this;
        }

        public RequestBuilder QueryRemovePair(string parameter)
        {
            return this;
        }

        public void Print()
        {
            foreach (var outerPair in queryPairs)
            {
                Console.WriteLine($"Key: {outerPair.Key.ParameterName}, {outerPair.Key.Setting}");

                foreach (var innerPair in outerPair.Value)
                {
                    Console.WriteLine($"  Inner Key: {innerPair.Key}");

                    foreach (var value in innerPair.Value)
                    {
                        Console.WriteLine($"    Value: {value}");
                    }
                }
            }
        }
    }
}
