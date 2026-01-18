using AutoMapper;
using GrupoTecnofix_Api.Dtos.Fornecedor;
using GrupoTecnofix_Api.Dtos.Produto;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class ProdutosProfile : Profile
    {
        public ProdutosProfile()
        {
            CreateMap<ProdutoDto, Produto>();
            CreateMap<Produto, ProdutoDto>();

            CreateMap<ProdutoCreateUpdate, Produto>();
            CreateMap<Produto, ProdutoCreateUpdate>();
        }
    }
}
