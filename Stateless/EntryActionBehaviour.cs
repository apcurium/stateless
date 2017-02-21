using System;
using System.Threading.Tasks;

namespace Stateless
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal class EntryActionBehavior
        {
            readonly string _actionDescription;
            readonly Func<Transition, object[], Task<object>> _func;

            public EntryActionBehavior(Func<Transition, object[], Task<object>> func, string actionDescription)
            {
                _func = func;
                _actionDescription = Enforce.ArgumentNotNull(actionDescription, nameof(actionDescription));
            }

            internal string ActionDescription { get { return _actionDescription; } }
            internal Func<Transition, object[], Task<object>> Func { get { return _func; } }
        }
    }
}
