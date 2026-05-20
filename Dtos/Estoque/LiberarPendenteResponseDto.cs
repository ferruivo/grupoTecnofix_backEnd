namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class LiberarPendenteResponseDto
    {
        public int IdPedidoVenda { get; set; }
        public int Item { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Liberado { get; set; }
        public string Status { get; set; } = "L";
        public List<LiberarPendenteAlocacaoResponseDto> Alocacoes { get; set; } = new();
    }

    public class LiberarPendenteAlocacaoResponseDto
    {
        public int IdLote { get; set; }
        public decimal Quantidade { get; set; }
        public string NumeroLote { get; set; } = string.Empty;
    }
}
