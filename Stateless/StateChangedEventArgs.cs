using System;

namespace Stateless
{
    /// <summary>
    /// StateChangedEventArgs
    /// </summary>
    /// <typeparam name="TSTate"></typeparam>
    public class StateChangedEventArgs<TSTate> : EventArgs
    {
        /// <summary>
        /// State
        /// </summary>
        public TSTate State { get; set; }

        /// <summary>
        /// StateChangedEventArgs
        /// </summary>
        /// <param name="state"></param>
        public StateChangedEventArgs(TSTate state)
        {
            State = state;
        }
    }
}
