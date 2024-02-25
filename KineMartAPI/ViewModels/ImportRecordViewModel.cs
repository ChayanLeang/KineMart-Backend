namespace KineMartAPI.ViewModels
{
    public class ImportRecordViewModel
    {
        public ImportRecordViewModel(string name,string date, List<SubRecordViewModel> list)
        {
            CompanyName = name;
            Date = date;
            Products = list;
        }
        public string CompanyName { get; set; } = null!;
        public string Date { get; set; } = null!;
        public List<SubRecordViewModel> Products { get; set; } = null!;
    }
}
