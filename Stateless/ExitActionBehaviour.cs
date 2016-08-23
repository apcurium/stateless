using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stateless
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal class ExitActionBehavior
        {
            readonly string _actionDescription;
            readonly Func<Transition, object> _func;

            public ExitActionBehavior(Func<Transition, object> func, string actionDescription)
            {
                _func = func;
                _actionDescription = Enforce.ArgumentNotNull(actionDescription, nameof(actionDescription));
            }

            internal string ActionDescription { get { return _actionDescription; } }
            internal Func<Transition, object> Func { get { return _func; } }
        }
    }
}
