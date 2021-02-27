namespace MakeItEasy.Specs.TestTypes
{
    public class OneCollaboratorOrOneArgumentClass
    {
        public OneCollaboratorOrOneArgumentClass(ICanCollaborate collaborator)
        {
            this.Collaborator = collaborator;
        }

        public OneCollaboratorOrOneArgumentClass(int argument)
        {
            this.Argument = argument;
        }

        public int Argument { get; }

        public ICanCollaborate? Collaborator { get; }
    }
}