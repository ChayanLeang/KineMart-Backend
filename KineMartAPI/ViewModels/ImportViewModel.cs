namespace KineMartAPI.ViewModels
{
    public class ImportViewModel
    {
        public ImportViewModel(int id, string name, string date, List<ImportRecordViewModel> list)
        {
            ImportId = id;
            Date = date;
            UserName = name;
            ImportRecords = list;
        }
        public int ImportId { get; set; }
        public string UserName { get; set; } = null!;
        public string Date { get; set; } = null!;
        public List<ImportRecordViewModel> ImportRecords { get; set; } = null!;
    }
}
