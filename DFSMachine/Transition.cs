namespace DFSMachine
{
    public class Transition
    {
        public string StartState { get; set; }
        public char Symbol { get; set; }
        public string EndState { get; set; }

        public Transition(string startState, char symbol, string endState)
        {
            StartState = startState;
            Symbol = symbol;
            EndState = endState;
        }

        public override string ToString()
        {
            return $"({StartState}, {Symbol}) -> {EndState}";
        }
    }
}