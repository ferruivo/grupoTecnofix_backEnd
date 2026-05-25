using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class NotaFiscalItemTributo
{
    public long IdNotaFiscalItemTributo { get; set; }

    public long IdNotaFiscalItem { get; set; }

    public string TipoTributo { get; set; } = null!;

    public string? Cst { get; set; }

    public string? Csosn { get; set; }

    public string? CclassTrib { get; set; }

    public string? CodigoBeneficioFiscal { get; set; }

    public decimal BaseCalculo { get; set; }

    public decimal Aliquota { get; set; }

    public decimal Valor { get; set; }

    public decimal? PercentualReducao { get; set; }

    public decimal? ValorCreditoPresumido { get; set; }

    public string? Observacao { get; set; }

    public virtual NotaFiscalItem IdNotaFiscalItemNavigation { get; set; } = null!;
}
