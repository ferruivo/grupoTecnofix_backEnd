using AutoMapper;
using GrupoTecnofix_Api.Dtos.Produto;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class ProdutoKitItenProfile : Profile
    {
        public ProdutoKitItenProfile()
        {
            CreateMap<ProdutoKitCreateUpdateDto, ProdutoKitIten>();
            CreateMap<ProdutoKitIten, ProdutoKitCreateUpdateDto>();
        }
    }
}
