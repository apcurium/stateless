using System;

namespace Stateless
{
    /// <summary>
    /// TriggerResultEventArgs
    /// </summary>
    public class TriggerResultEventArgs : EventArgs
    {
        /// <summary>
        /// FireCounter
        /// </summary>
        public Int64 FireCounter { get; }

        /// <summary>
        /// Result
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public TriggerResultEventArgs(Int64 fireCounter, object result = null)
        {
            FireCounter = fireCounter;
            Result = result;
        }
    }
}
