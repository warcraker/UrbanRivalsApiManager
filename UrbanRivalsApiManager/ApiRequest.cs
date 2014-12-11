using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UrbanRivalsApiManager
{
    /// <summary>
    /// Represent a queue of ApiCall items. It can be used to send multiple different ApiCall items on the same HTTP Request
    /// </summary>
    public class ApiRequest
    {
        private Queue<ApiCall> CallsQueue;
        internal int ApiCallsCount { get { return CallsQueue.Count; } }

        /// <summary>
        /// Creates an empty ApiRequest.
        /// </summary>
        public ApiRequest() 
        {
            CallsQueue = new Queue<ApiCall>();
        }
        /// <summary>
        /// Enqueue an ApiCall to the end of the queue. Multiple ApiCall items with the same Call parameter are not allowed.
        /// </summary>
        /// <param name="apiCall"></param>
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
        /// Returns the string resulting from encoding on JSON the queue.
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
