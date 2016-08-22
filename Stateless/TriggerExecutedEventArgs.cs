using System;

namespace Stateless
{
    /// <summary>
    /// TriggerExecutedEventArgs
    /// </summary>
    /// <typeparam name="TTrigger"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public class TriggerExecutedEventArgs<TTrigger, TState> : EventArgs
    {
        /// <summary>
        /// Trigger
        /// </summary>
        public TTrigger Trigger { get; }

        /// <summary>
        /// CurrentState
        /// </summary>
        public TState CurrentState { get; }

        /// <summary>
        /// FireCounter
        /// </summary>
        public Int64 FireCounter { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public TriggerExecutedEventArgs(TTrigger trigger, TState currentState, Int64 fireCounter)
        {
            Trigger = trigger;
            CurrentState = currentState;
            FireCounter = fireCounter;
        }
    }
}
