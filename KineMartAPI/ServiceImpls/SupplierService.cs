using KineMartAPI.Exceptions;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace KineMartAPI.ServiceImpls
{
    public class SupplierService : ISupplierService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public SupplierService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task AddSupplierAsync(IEnumerable<Supplier> suppliers)
        {
            foreach(var sr in suppliers)
            {
                await CheckAsync(sr);
            }
            await _repositoryWrapper.SupplierRepository.SaveManyAsync(suppliers);
        }

        public async Task<int> GetNumberOfSuppliersAsync()
        {
            return await _repositoryWrapper.SupplierRepository.CountAsync();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersAsync(string order,string search,int pageNumber,
                                                                                                int pageSize)
        {
            var suppliers = await _repositoryWrapper.SupplierRepository.FindSuppliersAsync();

            if (!order.IsNullOrEmpty())
            {
                switch (order)
                {
                    case "id":
                        suppliers = suppliers.OrderBy(sr => sr.SupplierId);
                        break;

                    case "name":
                        suppliers = suppliers.OrderBy(sr => sr.CompanyName);
                        break;
                }
            }

            if (!search.IsNullOrEmpty())
            {
                suppliers = suppliers.Where(sr => sr.CompanyName.Contains(search, StringComparison
                                                                       .CurrentCultureIgnoreCase));
            }

            return PaginatedList<Supplier>.Create(suppliers, pageNumber, pageSize);
        }

        public async Task UpdateSupplierAsync(int id, Supplier supplier)
        {
            var currentSupplier = await GetSupplierByIdAsync(id);
            currentSupplier.CompanyName = supplier.CompanyName;
            currentSupplier.Owner = supplier.Owner;
            currentSupplier.PhoneNumber = supplier.PhoneNumber;
            currentSupplier.Address = supplier.Address;
            currentSupplier.IsActive = supplier.IsActive;
            await CheckAsync(currentSupplier);
            await _repositoryWrapper.SupplierRepository.SaveAsync(currentSupplier,true);
        }

        private async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            var supplierExist = await _repositoryWrapper.SupplierRepository.FindByIdAsync(id);
            if (supplierExist == null)
            {
                throw new NotFoundException(id.ToString(),"Supplier");
            }
            return supplierExist;
        }

        private async Task CheckAsync(Supplier supplier)
        {
            //var regEx = new GeneratedRegexAttribute("^0\\d{8}$");
            if (!Regex.IsMatch(supplier.PhoneNumber, "^0\\d{8}$"))
            {
                throw new ExceptionBase($"PhoneNumber ({supplier.PhoneNumber})");
            }

            var companyNameExist = await _repositoryWrapper.SupplierRepository.FindByConditionAsync(sr =>sr.CompanyName
                                         .Trim().ToLower().Equals(supplier.CompanyName.Trim().ToLower()) && 
                                         sr.SupplierId!=supplier.SupplierId);   
            if (companyNameExist != null)
            {
                throw new UniqueException($"CompanyName ({companyNameExist.CompanyName})");
            }

            var phoneNumberExist = await _repositoryWrapper.SupplierRepository.FindByConditionAsync(sr =>sr.PhoneNumber
                                         .Trim().ToLower().Equals(supplier.PhoneNumber.Trim().ToLower()) && 
                                         sr.SupplierId != supplier.SupplierId);

            if (phoneNumberExist!=null)
            {
                throw new UniqueException($"PhoneNumber ({phoneNumberExist.PhoneNumber})");
            }
        }
    }
}
