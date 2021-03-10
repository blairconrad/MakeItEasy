namespace MakeItEasy.Specs.TestTypes
{
    using System;
    using FakeItEasy;

    public class CannotBeDummiedDummyFactory : DummyFactory<ICannotBeDummied>
    {
        protected override ICannotBeDummied Create()
        {
            throw new InvalidOperationException();
        }
    }
}