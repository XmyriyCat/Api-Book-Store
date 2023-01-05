namespace BLL.Errors
{
    public class UserLoginIsNotFound : Exception
    {
        public UserLoginIsNotFound()
        {
        }

        public UserLoginIsNotFound(string message) : base(message)
        {
        }
    }
}
