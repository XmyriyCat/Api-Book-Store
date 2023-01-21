namespace BLL.Errors
{
    public class CreateIdentityUserException : Exception
    {
        public CreateIdentityUserException()
        {
        }

        public CreateIdentityUserException(string message) : base(message)
        {
        }
    }
}
