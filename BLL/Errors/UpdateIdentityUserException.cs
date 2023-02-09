namespace BLL.Errors
{
    public class UpdateIdentityUserException : Exception
    {
        public UpdateIdentityUserException()
        {
        }

        public UpdateIdentityUserException(string message) : base(message)
        {
        }
    }
}
