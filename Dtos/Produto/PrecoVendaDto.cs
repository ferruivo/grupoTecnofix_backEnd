using GrupoTecnofix_Api.Dtos.Cliente;

namespace GrupoTecnofix_Api.Dtos.Produto
{
    public class PrecoVendaDto
    {
        public int IdPrecovenda { get; set; }
        public int IdProduto { get; set; }
        public int IdCliente { get; set; }
        public decimal Preco { get; set; }
        public DateTime? Vigencia { get; set; }
        public string? Revisao { get; set; }
        public DateTime? Datarevisao { get; set; }
        public string? Usurevisao { get; set; }
        public decimal? Precoantigo { get; set; }
        public string? Obs { get; set; }
        public ProdutoListDto? Produto { get; set; }
    }
}
