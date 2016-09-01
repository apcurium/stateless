using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stateless
{
    /// <summary>
    /// FireResponse
    /// </summary>
    public class FireResponse
    {
        /// <summary>
        /// IsTriggerAuthorized
        /// </summary>
        public bool IsTriggerAuthorized { get; }

        /// <summary>
        /// Response
        /// </summary>
        public object Response { get; }

        /// <summary>
        /// FireResponse
        /// </summary>
        /// <param name="isTriggerAuthorized"></param>
        /// <param name="response"></param>
        public FireResponse(bool isTriggerAuthorized, object response = null)
        {
            IsTriggerAuthorized = isTriggerAuthorized;
            Response = response;
        }
    }
}
