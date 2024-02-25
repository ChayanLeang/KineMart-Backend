namespace KineMartAPI.Exceptions
{
    public class ExceptionBase : Exception
    {
        public ExceptionBase(string field) : base(field == "Gender" ? "Gender should be Male or Female" :
                                                  field.Contains("PhoneNumber") ? $"{field} should be has 9 digits and"
                                                  + " start from 0" : field == "Name" ? "Name must be not start "
                                                  + "from number" : field == "StartDate" || field == "EndDate" ? 
                                                  $"{field} should be " + "yyyy-MM-dd" : field == "InvalidDate" ? 
                                                  "StartDate must be smaller than EndDate" : field == "Cost" || 
                                                  field == "Price" || field == "Qty" ? $"{field} must be greater " 
                                                  + $"than 0" : $"{field} wasn't enough in stock")
        { }
    }
}
