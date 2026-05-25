namespace GrupoTecnofix_Api.Dtos.Cfop
{
    public class CfopDto
    {
        public int IdCfop { get; set; }
        public string Cfop { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string TipoOperacao { get; set; } = string.Empty;
        public string? Venda { get; set; }
        public string? Icms { get; set; }
        public string? Obs { get; set; }
        public string? Devolucao { get; set; }
        public string? Interestadual { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }

    public class CfopLookupDto
    {
        public int Id { get; set; }
        public string Cfop { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Label => $"{Cfop} - {Descricao}";
    }

    public class CfopCreateUpdateDto
    {
        public string Cfop { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string TipoOperacao { get; set; } = string.Empty;
        public string? Venda { get; set; }
        public string? Icms { get; set; }
        public string? Obs { get; set; }
        public string? Devolucao { get; set; }
        public string? Interestadual { get; set; }
    }
}
