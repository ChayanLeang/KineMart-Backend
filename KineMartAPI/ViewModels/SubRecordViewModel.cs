namespace KineMartAPI.ViewModels
{
    public class SubRecordViewModel
    {
        public SubRecordViewModel(string name,double cost,double price,int qty,int remain)
        {
            ProductName = name;
            Cost = cost;
            Price = price;
            Qty = qty;
            Remain = remain;
        }
        public string ProductName { get; set; } = null!;
        public double Cost { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }
        public int Remain { get; set; }
    }
}
