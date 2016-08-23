﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stateless
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal class EntryActionBehavior
        {
            readonly string _actionDescription;
            readonly Func<Transition, object[], object> _func;

            public EntryActionBehavior(Func<Transition, object[], object> func, string actionDescription)
            {
                _func = func;
                _actionDescription = Enforce.ArgumentNotNull(actionDescription, nameof(actionDescription));
            }

            internal string ActionDescription { get { return _actionDescription; } }
            internal Func<Transition, object[], object> Func { get { return _func; } }
        }
    }
}
