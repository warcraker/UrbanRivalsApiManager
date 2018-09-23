using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UrbanRivalsApiManager
{
    /// <summary>
    /// Represent a queue of ApiCall items.
    /// </summary>
    public class ApiRequest
    {
        private Queue<ApiCall> CallsQueue;
        internal int ApiCallsCount { get { return CallsQueue.Count; } }

        /// <summary>
        /// Creates an new empty <see cref="ApiRequest"/>.
        /// </summary>
        public ApiRequest() 
        {
            CallsQueue = new Queue<ApiCall>();
        }
        /// <summary>
        /// Creates a new <see cref="ApiRequest"/> with one <see cref="ApiCall"/> enqueued.
        /// </summary>
        /// <param name="apiCall">Call to be enqueued.</param>
        /// <exception cref="ArgumentNullException"><paramref name="apiCall"/> is <code>null</code></exception>
        public ApiRequest(ApiCall apiCall)
            : this()
        {
            if (apiCall == null)
                throw new ArgumentNullException("apiCall");

            this.EnqueueApiCall(apiCall);
        }
        /// <summary>
        /// Enqueue an <see cref="ApiCall"/>. Multiple <see cref="ApiCall"/> items with the same <see cref="ApiCall.Call"/> are not allowed.
        /// </summary>
        /// <param name="apiCall"></param>
        /// <exception cref="ArgumentNullException"><paramref name="apiCall"/> is <code>null</code>.</exception>
        /// <exception cref="ArgumentException"><paramref name="apiCall"/> type of call is already included.</exception>
        public void EnqueueApiCall(ApiCall apiCall)
        {
            if (apiCall == null)
                throw new ArgumentNullException("apiCall");
            if (CallsQueue.Count(item => item.Call == apiCall.Call) > 0)
                throw new ArgumentException("An ApiCall with this Call: {0} already exists", "apiCall");

            CallsQueue.Enqueue(apiCall);
        }
        /// <summary>
        /// Removes all elements from the queue.
        /// </summary>
        public void ClearQueue()
        {
            CallsQueue.Clear();
        }

        /// <summary>
        /// Returns the request encoded in JSON.
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            var backup = new Queue<ApiCall>();
            var builder = new StringBuilder();

            builder.Append('[');
            while (CallsQueue.Count != 0)
            {
                var item = CallsQueue.Dequeue();
                backup.Enqueue(item);

                builder.Append(item.ToJson());
                builder.Append(',');
            }
            if (builder[builder.Length - 1] == ',')
                builder.Remove(builder.Length - 1, 1);
            builder.Append(']');

            CallsQueue = backup;
            return builder.ToString();
        }
    }
}
