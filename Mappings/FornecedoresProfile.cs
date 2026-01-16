using AutoMapper;
using GrupoTecnofix_Api.Dtos.Condições_Pagamento;
using GrupoTecnofix_Api.Dtos.Fornecedor;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class FornecedoresProfile : Profile
    {
        public FornecedoresProfile()
        {
            CreateMap<FornecedorDto, Fornecedore>();
            CreateMap<Fornecedore, FornecedorDto>();

            CreateMap<FornecedorCreateUpdateDto, Fornecedore>();
            CreateMap<Fornecedore, FornecedorCreateUpdateDto>();
        }
    }
}
