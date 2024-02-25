namespace KineMartAPI.Repositories
{
    public interface IRepositoryWrapper
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        IProductPropertyRepository ProductPropertyRepository { get; }
        IImportRepository ImportRepository { get; }
        IExportRepository ExportRepository { get; }
        IProductImportRepository ProductImportRepository { get; }
        IProductExportRepository ProductExportRepository { get; }
        ILogRepository LogRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
    }
}
