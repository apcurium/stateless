using System;

namespace Stateless
{
    /// <summary>
    /// TriggerNotValidEventArgs
    /// </summary>
    /// <typeparam name="TTrigger"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public class TriggerNotValidEventArgs<TTrigger, TState> : EventArgs
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
        /// Message
        /// </summary>
        public String Message { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public TriggerNotValidEventArgs(TTrigger trigger, TState currentState, string message = null)
        {
            Trigger = trigger;
            CurrentState = currentState;
            Message = message;
        }
    }
}
