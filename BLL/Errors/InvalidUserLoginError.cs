namespace BLL.Errors
{
    public class InvalidUserLoginError : Exception
    {
        public InvalidUserLoginError()
        {
        }

        public InvalidUserLoginError(string message) : base(message)
        {
        }
    }
}
