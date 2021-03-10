namespace MakeItEasy.Specs.TestTypes
{
    public class OneCollaboratorOneArgumentClass
    {
        public OneCollaboratorOneArgumentClass(int argument, ICanCollaborate collaborator)
        {
            this.Argument = argument;
            this.Collaborator = collaborator;
        }

        public int Argument { get; }

        public ICanCollaborate Collaborator { get; }

        public int GetIntFromCollaborator() => this.Collaborator.IntMethod();
    }
}