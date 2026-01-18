namespace GrupoTecnofix_Api.Dtos.Produto
{
    public class ProdutoDto
    {
        public int IdProduto { get; set; }
        public string Codigo { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public string Ncm { get; set; } = null!;
        public string Unidade { get; set; } = null!;
        public string CstIpi { get; set; } = null!;
        public string CstIcms { get; set; } = null!;
        public int? Minimo { get; set; }
        public string? Obs { get; set; }
        public string? ObsNf { get; set; }
        public string? ObsEntrada { get; set; }
    }
}
