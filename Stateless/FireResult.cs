using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stateless
{
    /// <summary>
    /// FireResult
    /// </summary>
    public class FireResult
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
        /// FireResult
        /// </summary>
        /// <param name="isTriggerAuthorized"></param>
        /// <param name="response"></param>
        public FireResult(bool isTriggerAuthorized, object response = null)
        {
            IsTriggerAuthorized = isTriggerAuthorized;
            Response = response;
        }
    }
}
