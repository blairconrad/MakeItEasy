namespace MakeItEasy.Specs
{
    using Xbehave;
    using Xbehave.Sdk;

    public static class StringExtensions
    {
        /// <summary>
        /// Creates a step builder that can be used to refer to a type.
        /// Useful when we want to establish a condition without actually performing an action.
        /// </summary>
        /// <typeparam name="T">The type to look at.</typeparam>
        /// <param name="text">A description of the type's relevant quality.</param>
        /// <returns>A step builder.</returns>
        public static IStepBuilder See<T>(this string text)
        {
            return text.x(() => { });
        }
    }
}
