namespace MakeItEasy
{
    using MakeItEasy.Syntax;

    /// <summary>
    /// Primary entry point into MakeItEasy functionality. Provides the ability to create "systems under test".
    /// </summary>
    public static class Make
    {
        /// <summary>
        /// Begin creating a system under test by specifying the type to create.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <returns>A newly-created object of type <c>T</c>. Will not be a Fake.</returns>
        /// <seealso cref="An{T}()">
        /// If desired, <c>An&lt;T&gt;</c> may be used to create objects of types whose names begin with a vowel sound.
        /// </seealso>
        public static Maker<T> A<T>() => new Maker<T>();

        /// <summary>
        /// Begin creating a system under test by specifying the type to create.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <returns>A newly-created object of type <c>T</c>. Will not be a Fake.</returns>
        /// <seealso cref="A{T}()">
        /// If desired, <c>A&lt;T&gt;</c> may be used to create objects of types whose names begin with a consonant sound.
        /// </seealso>
        public static Maker<T> An<T>() => A<T>();
    }
}