namespace Web_Api.Tests.Errors
{
    public class DbContextIsNotFoundException : Exception
    {
        public DbContextIsNotFoundException()
        {
        }

        public DbContextIsNotFoundException(string message) : base(message)
        {
        }
    }
}
