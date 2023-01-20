namespace BLL.Errors
{
    public class CreateIdentityUserException : Exception
    {
        public CreateIdentityUserException() : base()
        {
        }

        public CreateIdentityUserException(string message) : base(message)
        {
        }
    }
}
