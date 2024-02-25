using KineMartAPI.ModelEntities;

namespace KineMartAPI.Repositories
{
    public interface ISupplierRepository : IRepositoryBase<Supplier>
    {
        Task<IEnumerable<Supplier>> FindSuppliersAsync();
    }
}
