namespace MakeItEasy.Specs.TestTypes
{
    public class OneCollaboratorClass
    {
        public OneCollaboratorClass(ICanCollaborate collaborator)
            => this.Collaborator = collaborator;

        public ICanCollaborate Collaborator { get; }

        public int GetIntFromCollaborator() => this.Collaborator.IntMethod();
    }
}