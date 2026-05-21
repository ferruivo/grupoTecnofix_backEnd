namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class AlocacaoMaterialListDto
    {
        public int IdAlocacaoMaterial { get; set; }

        public int IdLote { get; set; }

        public int Etq { get; set; }

        public int IdPrateleira { get; set; }
        public string Prateleira { get; set; } = string.Empty;

        public int Qtd { get; set; }

        public int Reservado { get; set; }

        public DateTime? DataAlocacao { get; set; }
    }
}
