namespace SimProgramming.Controller.Exceptions
{
    public class DocumentValidationException : Exception
    {
        public DocumentValidationException()
        {
        }

        public DocumentValidationException(string message)
            : base(message)
        {
        }

        public DocumentValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
