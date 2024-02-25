using AutoMapper;
using KineMartAPI.ModelDtos;
using KineMartAPI.ModelEntities;

namespace KineMartAPI
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<CategoryDto, Category>();
            CreateMap<ProductDto, Product>();
            CreateMap<SupplierDto, Supplier>();
        }
    }
}
