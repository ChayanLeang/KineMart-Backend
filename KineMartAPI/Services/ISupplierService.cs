using KineMartAPI.ModelEntities;

namespace KineMartAPI.Services
{
    public interface ISupplierService
    {
        Task AddSupplierAsync(IEnumerable<Supplier> suppliers);
        Task<IEnumerable<Supplier>> GetSuppliersAsync(string order, string search ,int pageNumber, int pageSize);
        Task UpdateSupplierAsync(int id, Supplier supplier);
        Task<int> GetNumberOfSuppliersAsync();
    }
}
