using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Lote
{
    public int IdLote { get; set; }

    public int PedidoCompra { get; set; }

    public int ItemPedidoCompra { get; set; }

    public int NfCompra { get; set; }

    public int IdProduto { get; set; }

    public decimal Quantidade { get; set; }

    public decimal? QtdUtilizada { get; set; }

    public string Qpecas { get; set; } = null!;

    public string? ObsQpecas { get; set; }

    public string Aspecto { get; set; } = null!;

    public string? ObsAspecto { get; set; }

    public string Embalagem { get; set; } = null!;

    public string? ObsEmbalagem { get; set; }

    public string Status { get; set; } = null!;

    public DateOnly DataEntrada { get; set; }

    public string? Obs { get; set; }

    public string? Certificado { get; set; }
}
