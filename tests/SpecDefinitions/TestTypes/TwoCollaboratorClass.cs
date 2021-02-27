namespace MakeItEasy.Specs.TestTypes
{
    public class TwoCollaboratorClass
    {
        public TwoCollaboratorClass(ICanCollaborate collaborator1, ICanCollaborateToo collaborator2)
        {
            this.Collaborator1 = collaborator1;
            this.Collaborator2 = collaborator2;
        }

        public ICanCollaborate Collaborator1 { get; }

        public ICanCollaborateToo Collaborator2 { get; }

        public int GetIntFromCollaborator1() => this.Collaborator1.IntMethod();

        public string GetStringFromCollaborator2() => this.Collaborator2.StringMethod();
    }
}