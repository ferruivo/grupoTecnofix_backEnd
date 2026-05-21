namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class EtiquetaProdutoDto
    {
        public string Empresa { get; set; } = "";
        public string ProdutoCodigo { get; set; } = "";
        public string ProdutoDescricao { get; set; } = "";
        public decimal QuantidadeTotal { get; set; }
        public int Fator { get; set; }
        public string Lote { get; set; } = "";
    }
}
