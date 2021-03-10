namespace MakeItEasy.Specs.TestTypes
{
    public class TwoOfTheSameCollaboratorsClass
    {
        public TwoOfTheSameCollaboratorsClass(ICanCollaborate collaborator1, ICanCollaborate collaborator2)
        {
            this.Collaborator1 = collaborator1;
            this.Collaborator2 = collaborator2;
        }

        public ICanCollaborate Collaborator1 { get; }

        public ICanCollaborate Collaborator2 { get; }

        public int GetIntFromCollaborator1() => this.Collaborator1.IntMethod();

        public int GetIntFromCollaborator2() => this.Collaborator2.IntMethod();
    }
}