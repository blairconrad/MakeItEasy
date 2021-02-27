namespace MakeItEasy
{
    using System;

    /// <summary>
    /// An exception that is thrown when there was an error creating an object.
    /// </summary>
    public class CreationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreationException"/> class.
        /// </summary>
        public CreationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CreationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CreationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
