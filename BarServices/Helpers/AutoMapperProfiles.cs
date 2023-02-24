using AutoMapper;
using BarServices.DTOs;
using BarServices.Models;

namespace BarServices.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<UserCreationDTO, User>().ReverseMap();
            CreateMap<UserUpdateDTO, User>();
            
            CreateMap<ElaborationCreationDTO, Elaboration>();
            CreateMap<Elaboration, ElaborationDTO>();
            CreateMap<Elaboration, ElaborationDTOWithProducts>()
                .ForMember(dto => dto.Products, e => e.MapFrom(MapElaborationDTOProducts));

            CreateMap<ProductCreationDTO, Product>();
            CreateMap<ProductUpdateDTO, Product>();
            CreateMap<Product, ProductDTO>();
            
            CreateMap<TableCreationDTO, Table>()
                .ForMember(t => t.Bar, dto => dto.MapFrom(field => new Elaboration { Id = field.BarId }))
                .ForMember(t => t.Kitchen, dto => dto.MapFrom(field => new Elaboration { Id = field.KitchenId }));
            CreateMap<TableUpdateDTO, Table>();
            CreateMap<Table, TableDTO>()
                .ForMember(dto => dto.Products, t => t.MapFrom(MapTableDTOProducts))
                .ForMember(dto => dto.Bar, t => t.MapFrom(MapTableDTOBar))
                .ForMember(dto => dto.Kitchen, t => t.MapFrom(MapTableDTOKitchen));
        }

        private List<ProductDTO> MapElaborationDTOProducts(Elaboration elaboration, ElaborationDTOWithProducts elaborationDTOWithProducts)
        {
            var products = new List<ProductDTO>();
            if (elaboration.Products is null) { return products; }

            foreach (var product in elaboration.Products)
            {
                products.Add(new ProductDTO()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Category = product.Category,
                    Picture = product.Picture,
                    Status = product.Status
                });
            }
            return products;
        }

        private List<ProductDTO> MapTableDTOProducts(Table table, TableDTO tableDTO)
        {
            var products = new List<ProductDTO>();
            if (table.Products is null) { return products; }
            foreach (var product in table.Products)
            {
                products.Add(new ProductDTO()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Category = product.Category,
                    Picture = product.Picture,
                    Status = product.Status
                });
            }
            return products;
        }

        private ElaborationDTO MapTableDTOBar(Table table, TableDTO tableDTO)
        {
            return new ElaborationDTO() { Id = table.Bar.Id, Name = table.Bar.Name, };
        }

        private ElaborationDTO MapTableDTOKitchen(Table table, TableDTO tableDTO)
        {
            return new ElaborationDTO() { Id = table.Kitchen.Id, Name = table.Kitchen.Name, };
        }

    }
}
