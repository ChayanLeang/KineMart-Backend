using System.ComponentModel.DataAnnotations;

namespace KineMartAPI
{
    public class DateFilter
    {
        [Required(ErrorMessage ="StartDate must not be null or blank")]
        public string StartDate { get; set; } = null!;

        [Required(ErrorMessage = "EndDate must not be null or blank")]
        public string EndDate { get; set; } = null!;

        public DateFilter(string startDate, string endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
