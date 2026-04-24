namespace SimProgramming.Controller.Exceptions
{
    public class PdfGenerationException : Exception
    {
        public PdfGenerationException()
        {
        }

        public PdfGenerationException(string message)
            : base(message)
        {
        }

        public PdfGenerationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
