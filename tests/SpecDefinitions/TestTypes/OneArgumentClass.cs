namespace MakeItEasy.Specs.TestTypes
{
    public class OneArgumentClass
    {
        public OneArgumentClass(int argument)
            => this.Argument = argument;

        public int Argument { get; }
    }
}