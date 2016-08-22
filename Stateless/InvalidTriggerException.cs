using System;

namespace Stateless
{
    /// <summary>
    /// 
    /// </summary>
    public class InvalidTriggerException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public InvalidTriggerException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InvalidTriggerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidTriggerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
