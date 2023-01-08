namespace BLL.Errors
{
    public class JwtKeyIsNotFound : Exception
    {
        public JwtKeyIsNotFound()
        {
        }

        public JwtKeyIsNotFound(string message) : base(message)
        {
        }
    }
}
