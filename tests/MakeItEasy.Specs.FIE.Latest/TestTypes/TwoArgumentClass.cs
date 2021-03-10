namespace MakeItEasy.Specs.TestTypes
{
    public class TwoArgumentClass
    {
        public TwoArgumentClass(int argument1, string argument2)
        {
            this.Argument1 = argument1;
            this.Argument2 = argument2;
        }

        public int Argument1 { get; }

        public string Argument2 { get; }
    }
}