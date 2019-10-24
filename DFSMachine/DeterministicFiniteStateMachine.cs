using System;
using System.Collections.Generic;
using System.Linq;

namespace DFSMachine
{
    public class DeterministicFiniteStateMachine
    {
        private readonly List<string> Q;
        private readonly List<char> Sigma;
        private readonly List<Transition> Delta = new List<Transition>();
        private string Q0;
        private readonly List<string> F = new List<string>();

        public DeterministicFiniteStateMachine(
            List<string> q,
            List<char> sigma,
            IEnumerable<Transition> delta,
            string q0,
            IEnumerable<string> f)
        {
            if (delta == null) throw new ArgumentNullException(nameof(delta));
            if (q0 == null) throw new ArgumentNullException(nameof(q0));
            if (f == null) throw new ArgumentNullException(nameof(f));
            Q = q ?? throw new ArgumentNullException(nameof(q));
            Sigma = sigma ?? throw new ArgumentNullException(nameof(sigma));
            AddTransition(delta);
            AddInitialState(q0);
            AddFinalStates(f);
        }

        public void Accepts(string input)
        {
            var currentState = Q0;
            foreach (var transition in input.Select(symbol =>
                Delta.Find(t => t.StartState == currentState && t.Symbol == symbol)))
            {
                if (transition is null)
                {
                    return;
                }

                currentState = transition.EndState;
            }

            Console.WriteLine(F.Contains(currentState)
                ? "Accepted the input value"
                : $"Stopped in state {currentState} which is not a final state.");
        }

        private void AddTransition(IEnumerable<Transition> transitions)
        {
            foreach (var transition in transitions.Where(ValidTransition))
            {
                Delta.Add(transition);
            }
        }

        private bool ValidTransition(Transition transition)
        {
            return Q.Contains(transition.StartState) &&
                   Q.Contains(transition.EndState) &&
                   Sigma.Contains(transition.Symbol) &&
                   !TransitionAlreadyDefined(transition);
        }

        private bool TransitionAlreadyDefined(Transition transition)
        {
            return Delta.Any(t => t.StartState == transition.StartState &&
                                  t.Symbol == transition.Symbol);
        }

        private void AddInitialState(string q0)
        {
            if (Q.Contains(q0))
            {
                Q0 = q0;
            }
        }

        private void AddFinalStates(IEnumerable<string> finalStates)
        {
            foreach (var finalState in finalStates.Where(
                finalState => Q.Contains(finalState)))
            {
                F.Add(finalState);
            }
        }
    }
}