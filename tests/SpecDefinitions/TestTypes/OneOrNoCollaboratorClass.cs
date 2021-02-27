namespace MakeItEasy.Specs.TestTypes
{
    public class OneOrNoCollaboratorClass
    {
        public OneOrNoCollaboratorClass()
        {
        }

        public OneOrNoCollaboratorClass(ICanCollaborate collaborator)
            => this.Collaborator = collaborator;

        public ICanCollaborate? Collaborator { get; }
    }
}