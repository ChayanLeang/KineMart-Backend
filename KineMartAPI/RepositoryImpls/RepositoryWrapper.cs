using KineMartAPI.Repositories;

namespace KineMartAPI.RepositoryImpls
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly MartDbContext _martDbContext;
        private ICategoryRepository? _categoryRepository;
        private IProductRepository? _productRepository;
        private ISupplierRepository? _supplierRepository;
        private IProductPropertyRepository? _productPropertyRepository;
        private IImportRepository? _importRepository;
        private IExportRepository? _exportRepository;
        private IProductImportRepository? _productImportRepository;
        private IProductExportRepository? _productExportRepository;
        private ILogRepository? _logRepository;
        private IRefreshTokenRepository? _refreshTokenRepository;
        public RepositoryWrapper(MartDbContext martDbContext)
        {
            _martDbContext = martDbContext;
        }
        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository ??= new CategoryRepository(_martDbContext);
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new ProductRepository(_martDbContext);
            }
        }

        public ISupplierRepository SupplierRepository
        {
            get
            {
                return _supplierRepository ??= new SupplierRepository(_martDbContext);
            }
        }

        public IProductPropertyRepository ProductPropertyRepository
        {
            get
            {
                return _productPropertyRepository ??= new ProductPropertyRepository(_martDbContext);
            }
        }

        public IImportRepository ImportRepository
        {
            get
            {
                return _importRepository ??= new ImportRepository(_martDbContext);
            }
        }

        public IExportRepository ExportRepository
        {
            get
            {
                return _exportRepository ??= new ExportRepository(_martDbContext);
            }
        }

        public IProductImportRepository ProductImportRepository
        {
            get
            {
                return _productImportRepository ??= new ProductImportRepository(_martDbContext);
            }
        }

        public IProductExportRepository ProductExportRepository
        {
            get
            {
                return _productExportRepository ??= new ProductExportRepository(_martDbContext);
            }
        }

        public ILogRepository LogRepository
        {
            get
            {
                return _logRepository ??= new LogRepository(_martDbContext);
            }
        }

        public IRefreshTokenRepository RefreshTokenRepository
        {
            get
            {
                return _refreshTokenRepository ??= new RefreshTokenRepository(_martDbContext);
            }
        }
    }
}
