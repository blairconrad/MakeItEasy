namespace MakeItEasy.Specs.TestTypes
{
    public class TwoOfTheSameArgumentsClass
    {
        public TwoOfTheSameArgumentsClass(string argument1, string argument2)
        {
            this.Argument1 = argument1;
            this.Argument2 = argument2;
        }

        public string Argument1 { get; }

        public string Argument2 { get; }
    }
}