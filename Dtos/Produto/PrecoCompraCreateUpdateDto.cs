namespace GrupoTecnofix_Api.Dtos.Produto
{
    public class PrecoCompraCreateUpdateDto
    {
        public int IdProduto { get; set; }

        public int IdFornecedor { get; set; }

        public decimal Preco { get; set; }

        public DateTime? Vigencia { get; set; }

        public decimal? Precoantigo { get; set; }

        public string? Obs { get; set; }
    }
}
