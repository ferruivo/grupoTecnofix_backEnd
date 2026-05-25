using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class NotaFiscalEvento
{
    public long IdNotaFiscalEvento { get; set; }

    public long IdNotaFiscal { get; set; }

    public string TipoEvento { get; set; } = null!;

    public string? Protocolo { get; set; }

    public DateTime DataEvento { get; set; }

    public string? Usuario { get; set; }

    public string? Justificativa { get; set; }

    public string? XmlRetorno { get; set; }

    public virtual NotaFiscal IdNotaFiscalNavigation { get; set; } = null!;
}
