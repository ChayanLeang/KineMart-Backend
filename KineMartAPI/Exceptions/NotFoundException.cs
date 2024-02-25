namespace KineMartAPI.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string an,string field) : base(field=="RefreshToken" ? 
                                                           $"{field} with token = {an} didn't find" :
                                                           $"{field} with id = {an} didn't find") { }
    }
}
