using AutoMapper;
using GrupoTecnofix_Api.Dtos.Condições_Pagamento;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class CondicaoPagamentoProfile : Profile
    {
        public CondicaoPagamentoProfile()
        {
            CreateMap<CondicaoPagamentoDto, Condicoespagamento>();
            CreateMap<Condicoespagamento, CondicaoPagamentoDto>();
        }

    }
}
