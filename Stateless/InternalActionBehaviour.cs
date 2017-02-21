using System;
using System.Threading.Tasks;

namespace Stateless
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal class InternalActionBehavior
        {
            readonly string _actionDescription;
            readonly TTrigger _trigger;
            readonly Func<Transition, object[], Task<object>> _func;

            public InternalActionBehavior(Func<Transition, object[], Task<object>> func, TTrigger trigger, string actionDescription)
            {
                _func = func;
                _trigger = trigger;
                _actionDescription = Enforce.ArgumentNotNull(actionDescription, nameof(actionDescription));
            }

            internal string ActionDescription { get { return _actionDescription; } }
            internal TTrigger Trigger { get { return _trigger; } }
            internal Func<Transition, object[], Task<object>> Func { get { return _func; } }
        }
    }
}
