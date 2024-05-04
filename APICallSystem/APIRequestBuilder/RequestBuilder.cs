using APICallSystem.APIRequestBuilder.Query;

namespace APICallSystem.APIRequestBuilder
{
    // Explain possibilities, allows separation of data... why list inside dictionary inside dictionary?
    /// <summary>
    /// Creates a custom HTTP request based on provided data.
    /// <br><strong>Queries:</strong></br>
    /// <br>To add new parameter-value pair use <see cref="QueryAdd(string, string, ref string, RequestCompareSetting)"/> or <see cref="QueryAppend
    /// (string, string, ref string, RequestCompareSetting)"/>.</br>
    /// <br>When a new value is added, it is assigned to a <see cref="RequestQueryKeyPair"/> as well as another key, which will mentioned later 
    /// as just key. This is done to allow for grouping of values. Example is shown later in the documentation.</br>
    /// <br>To generate multiple pairs with the same parameter name, a different key must be used. 
    /// This will result in .../api?name=1&amp;name=2&amp;name=3.</br>
    /// <br>It is possible to add multiple values separated by a comma by using the <see cref="QueryAppend(string, string, ref string, 
    /// RequestCompareSetting)"/> method. 
    /// The result will be .../api?name=1,2&amp;name=3,4</br>
    /// <br>It is also possible to remove parts with <see cref="QueryRemoveFromKey(string, string, ref string, RequestCompareSetting)"/>, <see 
    /// cref="QueryRemoveKey(string, ref string, RequestCompareSetting)"/>, <see cref="QueryRemovePair(string, RequestCompareSetting)"/>.</br>
    /// <br>The setting is mainly for the personal API, to not include it inside a query, use <see cref="RequestCompareSetting.None"/>. It is used to choose 
    /// the way the values will be compared. The parameters are saved in combination with the setting as unique key. That allows for choosing multiple 
    /// settings per one parameter. </br>
    /// <br>For example: </br>
    /// <br>Parameter: FirstName, Setting: Includes, Values: J, o, and in the same example another pair, Parameter: FirstName, Setting: NotIncludes,  
    /// Values: b, k will result in a query which will look for names that include either J OR o, AND where it doesn't include b OR k.</br>
    /// <br>Note the OR and AND. Values inside the same group will be compared with OR, values in different groups will be compared with AND. 
    /// The query will look something like this: .../api?FirstName=J,o;Includes&amp;FirstName=b,k;NotIncludes.</br>
    /// <br><strong>Body:</strong></br>
    /// <br>...</br>
    /// <br><strong>Headers:</strong></br>
    /// <br>...</br>
    /// <br><strong>Other modifiers:</strong></br>
    /// <br>...</br>
    /// <br><strong>Convert to an API call:</strong></br>
    /// <br>...</br>
    /// </summary>
    internal class RequestBuilder
    {
        /// <summary>
        /// Holds all data related to queries. Queries might be separated 
        /// </summary>
        private readonly Dictionary<RequestQueryKeyPair, Dictionary<string, List<string>>> queryPairs = [];

        /// <summary>
        /// Appends the value to an existing key-value pair within a parameter-setting pair. Creates new pairs if ones do not exist.
        /// </summary>
        /// <param name="parameter">The name of a variable or property from a model class, which will be used as key in the query.</param>
        /// <param name="value">Value that will be assigned to the key.</param>
        /// <param name="key">The key that will be, or already is, associated to the group of values. Will be set to random Guid if empty or null.</param>
        /// <param name="setting">The way the values will be compared. Only works for personal API. Defaults to <see cref="RequestCompareSetting.None"/>.</param>
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
        /// Creates a new key-value pair within a parameter-setting pair. Will create the parameter-setting pair, if one does not exist.
        /// The <paramref name="key"/> will equal the new key from the key-value pair.
        /// </summary>
        /// <param name="parameter">The name of a variable or property from a model class, which will be used as key in the query.</param>
        /// <param name="value">Value that will be assigned to the key.</param>
        /// <param name="key">Value of this variable will equal the key that was used to create the new key-value pair.</param>
        /// <param name="setting">The way the values will be compared. Only works for personal API. Defaults to <see cref="RequestCompareSetting.None"/>.</param>
        public RequestBuilder QueryAdd(string parameter, string value, ref string key, RequestCompareSetting setting = RequestCompareSetting.None)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(parameter, nameof(parameter));
            ArgumentNullException.ThrowIfNull(value);

            RequestQueryKeyPair newKeyPair = new(parameter, setting);

            if (!queryPairs.ContainsKey(newKeyPair))
            {
                queryPairs[newKeyPair] = [];
            }

            key = Guid.NewGuid().ToString();

            queryPairs[newKeyPair][key] = [];
            queryPairs[newKeyPair][key].Add(value);

            return this;
        }

        /// <summary>
        /// Allows changing value of the key without breaking the chain.
        /// </summary>
        /// <param name="key">The key reference.</param>
        /// <param name="newValue">New value that will be assigned to it.</param>
        public RequestBuilder InnerKeyChange(ref string key, string newValue)
        {
            key = newValue;
            return this;
        }

        /// <summary>
        /// Removes specified value from an existing key-value pair within a parameter-setting pair. Requires the correct key from the time of assigning, 
        /// can be found inside the ref value of the <see cref="QueryAdd(string, string, ref string, RequestCompareSetting)"/> 
        /// or <see cref="QueryAppend(string, string, ref string, RequestCompareSetting)"/>. Deletes the parent pair if parent is left empty.
        /// </summary>
        /// <param name="parameter">The name of a variable or property from a model class, which was given when creating the parameter-setting pair.</param>
        /// <param name="value">The value to be removed.</param>
        /// <param name="key">The associated key.</param>
        /// <param name="setting">The setting which was given when creating the parameter-setting pair.</param>
        public RequestBuilder QueryRemoveFromKey(string parameter, string value, ref string key, RequestCompareSetting setting = RequestCompareSetting.None)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(parameter, nameof(parameter));
            RequestQueryKeyPair newKeyPair = new(parameter, setting);

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

        /// <summary>
        /// Removes all values from an existing key-value pair including itself within a parameter-setting pair. 
        /// Requires the correct key from the time of assigning, can be found inside the ref value of the 
        /// <see cref="QueryAdd(string, string, ref string, RequestCompareSetting)"/> 
        /// or <see cref="QueryAppend(string, string, ref string, RequestCompareSetting)"/>.
        /// </summary>
        /// <param name="parameter">The name of a variable or property from a model class, which was given when creating the parameter-setting pair.</param>
        /// <param name="key">The associated key.</param>
        /// <param name="setting">The setting which was given when creating the parameter-setting pair.</param>
        public RequestBuilder QueryRemoveKey(string parameter, ref string key, RequestCompareSetting setting = RequestCompareSetting.None)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(parameter, nameof(parameter));
            RequestQueryKeyPair newKeyPair = new(parameter, setting);

            if (string.IsNullOrWhiteSpace(key)) return this;
            if (!queryPairs.ContainsKey(newKeyPair)) return this;
            if (queryPairs[newKeyPair][key] == null) return this;

            queryPairs[newKeyPair].Remove(key);

            return this;
        }


        /// <summary>
        /// Removes full parameter-setting pair and all data within.
        /// </summary>
        /// <param name="parameter">The name of a variable or property from a model class, which was given when creating the parameter-setting pair.</param>
        /// <param name="setting">The setting which was given when creating the parameter-setting pair.</param>
        public RequestBuilder QueryRemovePair(string parameter, RequestCompareSetting setting = RequestCompareSetting.None)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(parameter, nameof(parameter));
            RequestQueryKeyPair newKeyPair = new(parameter, setting);

            if (!queryPairs.ContainsKey(newKeyPair)) return this;
            
            queryPairs.Remove(newKeyPair);

            return this;
        }

        /// <summary>
        /// Dummy method used to print the contents. Should be deleted. 
        /// </summary>
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
