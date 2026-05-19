using System;

namespace GrupoTecnofix_Api.Models;

public partial class LoteInspecao
{
    public int IdLoteInspecao { get; set; }
    public int IdLote { get; set; }
    public int IdEspecificacao { get; set; }

    public string? Minimo { get; set; }
    public string? Maximo { get; set; }
    public string? Aproximado { get; set; }
    public string? Observacao { get; set; }

    public DateTime DataCadastro { get; set; }
}
