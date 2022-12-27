namespace DLL.Errors
{
    public class DbEntityNotFoundException : Exception
    {
        public DbEntityNotFoundException()
        {
        }

        public DbEntityNotFoundException(string message) : base(message)
        {
        }
    }
}
