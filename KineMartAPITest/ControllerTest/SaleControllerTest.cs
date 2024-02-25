using KineMartAPI;
using KineMartAPI.Controllers;
using KineMartAPI.ModelDtos;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KineMartAPITest.ControllerTest
{
    public class SaleControllerTest
    {
        private MartDbContext martDbContext;
        private IRepositoryWrapper repositoryWrapper;
        private ISaleService saleService;
        private IProductPropertyService productPropertyService;
        private IProductExportService productExportService;
        private IProductImportService productImportService;
        private IImportService importService;
        private IExportService exportService;
        private SaleController saleController;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            productImportService = new ProductImportService(repositoryWrapper);
            productExportService = new ProductExportService(repositoryWrapper);
            importService = new ImportService(repositoryWrapper, productImportService);
            exportService = new ExportService(repositoryWrapper, productExportService);
            productPropertyService = new ProductPropertyService(repositoryWrapper, importService, 
                                                                           productImportService);
            saleService = new SaleService(exportService, productExportService, productPropertyService);
            saleController = new SaleController(saleService);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfSale(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test,Order(1)]
        public async Task TestSellWithNoException()
        {
            var saleDto= new SaleDto()
            {
                UserId="afafafafaa",
                Products = new List<ProductSaleDto>
                {
                    new ProductSaleDto()
                    {
                        ProductProId=1,
                        Qty=5
                    }
                }
            };
            var actionResult = await saleController.SellAsync(saleDto);
            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }

        [Test,Order(2)]
        public async Task TestSellWithException()
        {
            var actionResult = await saleController.SellAsync(null!);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }
    }
}
