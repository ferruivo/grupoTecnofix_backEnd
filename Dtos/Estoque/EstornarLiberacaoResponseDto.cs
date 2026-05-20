namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class EstornarLiberacaoResponseDto
    {
        public int IdPedidoVenda { get; set; }
        public int Item { get; set; }
        public decimal Liberado { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
