namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class ConsumoloteDto
    {
        public int IdConsumolote { get; set; }

        public int Idpedidovenda { get; set; }

        public int Item { get; set; }

        public int? Idproduto { get; set; }

        public int Idlote { get; set; }

        public decimal Quantidade { get; set; }

        public DateOnly Data { get; set; }

        public string Usuario { get; set; } = null!;

        public long? Notafiscal { get; set; }
    }
}
