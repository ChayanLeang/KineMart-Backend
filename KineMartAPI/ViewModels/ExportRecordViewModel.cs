namespace KineMartAPI.ViewModels
{
    public class ExportRecordViewModel
    {
        public ExportRecordViewModel(string proName,double price,int qty)
        {
            ProductName = proName;
            Price = price;
            Qty = qty;
        }
        public string ProductName { get; set; } = null!;
        public double Price { get; set; }
        public int Qty { get; set; }
    }
}
