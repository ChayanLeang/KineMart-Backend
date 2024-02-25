namespace KineMartAPI.ViewModels
{
    public class ExportViewModel
    {
        public ExportViewModel(int id,string name,string date, IEnumerable<ExportRecordViewModel> records)
        {
            ExportId = id;
            UserName = name;
            Date = date;
            ExportRecords = records;
        }
        public int ExportId { get; set; }
        public string UserName { get; set; } = null!;
        public string Date { get; set; } = null!;
        public IEnumerable<ExportRecordViewModel> ExportRecords { get; set; } = null!;
        
    }
}
