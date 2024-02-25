namespace KineMartAPI.Exceptions
{
    public class UniqueException : Exception
    {
        public UniqueException(string field) : base($"{field} was already exist") { }
    }
}
