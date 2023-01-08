namespace BLL.Errors
{
    public class WrongUserPasswordError : Exception
    {
        public WrongUserPasswordError()
        {
        }

        public WrongUserPasswordError(string message) : base(message)
        {
        }
    }
}
