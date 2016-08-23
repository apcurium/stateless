﻿using System.Collections.Generic;
using System.Linq;

namespace Stateless
{
    public partial class StateMachine<TState, TTrigger>
    {
        /// <summary>
        /// A string representation of the state machine in the DOT graph language.
        /// </summary>
        /// <returns>A description of all simple source states, triggers and destination states.</returns>
        public string ToDotGraph()
        {
            List<string> lines = new List<string>();
            List<string> unknownDestinations = new List<string>();

            foreach (var stateCfg in _stateConfiguration) 
            {
                TState source = stateCfg.Key;
                foreach (var behaviours in stateCfg.Value.TriggerBehaviours) 
                {
                    foreach (TriggerBehaviour behaviour in behaviours.Value) 
                    {
                        string destination;

                        if (behaviour is TransitioningTriggerBehaviour)
                        {
                            destination = ((TransitioningTriggerBehaviour)behaviour).Destination.ToString ();
                        }
                        else if (behaviour is IgnoredTriggerBehaviour)
                        {
                            continue; 
                        }
                        else 
                        {
                            destination = "unknownDestination_" + unknownDestinations.Count;
                            unknownDestinations.Add(destination);
                        }

                        string line = (behaviour.Guard.TryGetMethodInfo().DeclaringType.Namespace.Equals("Stateless")) ?
                            string.Format(" {0} -> {1} [label=\"{2}\"];", source, destination, behaviour.Trigger) :
                            string.Format(" {0} -> {1} [label=\"{2} [{3}]\"];", source, destination, behaviour.Trigger, behaviour.GuardDescription);

                        lines.Add(line);
                    }
                }
            }

            if (unknownDestinations.Any())
            {
                string label = string.Format(" {{ node [label=\"?\"] {0} }};", string.Join(" ", unknownDestinations));
                lines.Insert(0, label);
            }

            if (_stateConfiguration.Any(s => s.Value.EntryAction != null || s.Value.ExitAction != null))
            {
                lines.Add("node [shape=box];");

                foreach (var stateCfg in _stateConfiguration)
                {
                    TState source = stateCfg.Key;

                    lines.Add(string.Format(" {0} -> \"{1}\" [label=\"On Entry\" style=dotted];", source, stateCfg.Value.EntryAction.ActionDescription));
                    
                    lines.Add(string.Format(" {0} -> \"{1}\" [label=\"On Exit\" style=dotted];", source, stateCfg.Value.ExitAction.ActionDescription));
                    
                }
            }

            return "digraph {" + System.Environment.NewLine +
                     string.Join(System.Environment.NewLine, lines) + System.Environment.NewLine +
                   "}";
        }
    }
}
