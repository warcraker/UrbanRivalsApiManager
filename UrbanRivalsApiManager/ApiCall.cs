using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace UrbanRivalsApiManager
{
    /// <summary>
    /// Defines the essential elements for an ApiCall. To use a specific ApiCall use <seealso cref="ApiCallList"/>
    /// </summary>
    public abstract class ApiCall
    {
        /// <summary>
        /// Parameters with a <code>null</code> value will use this reference instead.
        /// </summary>
        private static readonly object EmptyValue = new object();

        /// <summary>
        /// Parameters of the ApiCall and their values.
        /// </summary>
        private Dictionary<string, object> Parameters;
        /// <summary>
        /// Names of the compulsory parameters. Those that cannot be <see cref="EmptyValue"/>.
        /// </summary>
        private List<string> CompulsoryParameters;

        /// <summary>
        /// <code>True</code> if the ApiCall requires Public access to the API. <code>False</code> otherwise.
        /// </summary>
        public bool RequiresPublicAccess { get; protected set; }
        /// <summary>
        /// <code>True</code> if the ApiCall requires User access to the API. <code>False</code> otherwise.
        /// </summary>
        public bool RequiresUserAccess { get; protected set; }
        /// <summary>
        /// <code>True</code> if the ApiCall requires Action access to the API. <code>False</code> otherwise.
        /// </summary>
        public bool RequiresActionAccess { get; protected set; }

        /// <summary>
        /// <code>True</code> if the ApiCall returns Items. <code>False</code> otherwise.
        /// </summary>
        public bool ReturnsItems { get; protected set; }
        /// <summary>
        /// <code>True</code> if the ApiCall returns Context. <code>False</code> otherwise.
        /// </summary>
        public bool ReturnsContext { get; protected set; }
        /// <summary>
        /// Gets or Sets the ItemsFilter used in the ApiCall.
        /// </summary>
        public List<string> ItemsFilter { get; set; }
        /// <summary>
        /// Gets or Sets the ContextFilter used in the ApiCall.
        /// </summary>
        public List<string> ContextFilter { get; set; }

        /// <summary>
        /// Gets the call used. For a list of all valid calls check <seealso cref="http://www.urban-rivals.com/api/developer/"/>.
        /// </summary>
        public string Call { get; private set; }
        /// <summary>
        /// A list of all the parameters that the ApiCall accepts. This list will be empty for parameterless calls.
        /// </summary>
        public List<string> ParametersList { get { return Parameters.Keys.ToList(); } }
        /// <summary>
        /// A list of all the parameters that can't be left empty. This list will be empty for calls without compulsory parameters.
        /// </summary>
        public List<string> CompulsoryParametersList { get { return CompulsoryParameters.ToList(); } }

        /// <summary>
        /// Tries to set the parameter to the specified value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter. Can't be null or whitespace.</param>
        /// <param name="value">Value of the parameter. Must be in the subset T ∈ (null, bool, int, string, List&lt;T&gt;). Can't be null if is a compulsory parameter.</param>
        /// <returns><code>True</code> if the parameter name is valid and the value is set. <code>False</code> otherwise.</returns>
        public bool TrySetParamenterValue(string parameterName, object value)
        {
            if (String.IsNullOrWhiteSpace(parameterName))
                return false;

            if (!Parameters.Keys.Contains(parameterName))
                return false;

            if (value == null)
            {
                if (CompulsoryParameters.Contains(parameterName))
                    return false;

                value = EmptyValue;
            }

            try
            {
                Parameters[parameterName] = value;
            }
            catch
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Sets the parameter to the specified value. Doesn't check that the value is correct.
        /// </summary>
        /// <param name="parameterName">Name of the parameter. Can't be null or whitespace.</param>
        /// <param name="value">Value of the parameter. Must be in the subset T ∈ (null, bool, int, string, List&lt;T&gt;). Can't be null if is a compulsory parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parameterName"/> is null, empty or whitespace</exception>
        /// <exception cref="ArgumentException">The parameter doesn't exist, or is a compulsory parameter and the value is null.</exception>
        public void SetParamenterValue(string parameterName, object value)
        {
            if (TrySetParamenterValue(parameterName, value))
                return;

            if (String.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException("parameterName");

            if (!Parameters.Keys.Contains(parameterName))
                throw new ArgumentException("The parameter doesn't exist", "parameter");

            if (CompulsoryParameters.Contains(parameterName) && value == null)
                throw new ArgumentException(parameterName + " is a compulsory parameter and cannot be null", "parameter");

            // Sanity check. This should never happen. 
            throw new Exception("SetParamenterValue() has failed for unknown reasons.");
        }
        /// <summary>
        /// Tries to get the value of the specified parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter. Can't be null or whitespace.</param>
        /// <param name="value">Value of the parameter.</param>
        /// <returns><code>True</code> if the name is valid. <code>False</code> otherwise.</returns>
        public bool TryGetParameterValue(string parameterName, out object value)
        {
            value = null;

            if (String.IsNullOrWhiteSpace(parameterName))
                return false;

            if (!Parameters.Keys.Contains(parameterName))
                return false;

            if (Parameters[parameterName] == EmptyValue)
                return true;

            value = Parameters[parameterName];
            return true;
        }
        /// <summary>
        /// Gets the value of the specified parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter. Can't be null or whitespace.</param>
        /// <param name="value">Value of the parameter.</param>
        /// <returns><code>True</code> if the name is valid. <code>False</code> otherwise.</returns>
        /// <exception cref="ArgumentNullException">parameterName is null, empty or whitespace</exception>
        /// <exception cref="ArgumentException">The parameter doesn't exist.</exception>
        public object GetParameterValue(string parameterName)
        {
            object res;
            if (TryGetParameterValue(parameterName, out res))
                return res;
                
            if (String.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException("parameterName");

            if (!Parameters.Keys.Contains(parameterName))
                throw new ArgumentException("The parameter doesn't exist", "parameter");

            // Sanity check. This should never happen.
            throw new Exception("GetParameterValue() has failed for unknown reasons.");
        }

        /// <summary>
        /// Returns the <see cref="ApiCall"/> encoded in JSON.
        /// </summary>
        /// <remarks>The server requires the JSON request to be encoded as an array. This method returns a single element.</remarks>
        public string ToJson()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"call\":");
            EncodeString(Call, builder);
            builder.Append(',');

            builder.Append("\"params\":{"); 
            if (Parameters.Count != 0)
            {
                foreach (KeyValuePair<string, object> item in Parameters)
                {
                    if (item.Value == EmptyValue)
                        continue;

                    EncodeString(item.Key, builder);
                    builder.Append(':');
                    EncodeItem(item.Value, builder);
                    builder.Append(',');
                }
                if (builder[builder.Length - 1] == ',')
                    builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("},");

            builder.Append("\"itemsFilter\":");
            if (ItemsFilter != null && ItemsFilter.Count > 0)
                EncodeItem(ItemsFilter, builder);
            else
                builder.Append("[]");
            builder.Append(',');

            builder.Append("\"contextFilter\":");
            if (ContextFilter != null && ContextFilter.Count > 0)
                EncodeItem(ContextFilter, builder);
            else
                builder.Append("[]");
            builder.Append('}');

            return builder.ToString();
        }
        private static void EncodeString(string item, StringBuilder builder)
        {
            builder.Append('"');
            builder.Append(item);
            builder.Append('"');
        }
        private static void EncodeItem(object item, StringBuilder builder)
        {
            builder.Append(JsonConvert.SerializeObject(item));
        }

        /// <summary>
        /// Initializes basic data of the object.
        /// </summary>
        /// <remarks>In order to create inherited versions, call the <code>protected</code> constructor and methods inside your constructor.</remarks>
        private ApiCall() 
        {
            Parameters = new Dictionary<string, object>();
            CompulsoryParameters = new List<string>();
        }
        /// <summary>
        /// Creates a new ApiCall with the call indicated.
        /// </summary>
        /// <param name="call">Name of the call.</param>
        /// <remarks>This is the intended way to create inherited versions.</remarks>
        protected ApiCall(string call)
            : this()
        {
            if (String.IsNullOrWhiteSpace(call))
                throw new ArgumentNullException("call");

            Call = call;
        }

        /// <summary>
        /// Adds a parameter to the list of the parameters that the ApiCall is enforced to use. It also adds the parameter to the normal parameter list.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <remarks>This is intended to be used only on the constructor of a inherited version.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="parameterName"/> is null, empty or whitespace</exception>
        /// <exception cref="ArgumentException">The parameter already exists on the compulsory parameters list</exception>
        protected void AddCompulsoryParameter(string parameterName)
        {
            if (String.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException("parameterName");
            if (CompulsoryParameters.Contains(parameterName))
                throw new ArgumentException("The parameter already exists on the compulsory parameters list", "parameter");

            CompulsoryParameters.Add(parameterName);
            AddParameter(parameterName);
        }
        /// <summary>
        /// Adds a parameter to the list of the parameters accepted by the ApiCall. If the parameter is compulsory, call AddCompulsoryParameter() instead.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <remarks>This is intended to be used only on the constructor of a inherited version.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="parameterName"/> is null, empty or whitespace</exception>
        /// <exception cref="ArgumentException">The parameter already exists on the parameters list</exception>
        protected void AddParameter(string parameterName)
        {
            if (Parameters.ContainsKey(parameterName))
                throw new ArgumentException("The parameter already exists on the parameters list", "parameter");

            Parameters.Add(parameterName, EmptyValue);
        }
    }
}
