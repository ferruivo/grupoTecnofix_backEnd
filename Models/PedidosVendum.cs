using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class PedidosVendum
{
    public int IdPedidoVenda { get; set; }

    public int IdCliente { get; set; }

    public int IdVendedor { get; set; }

    public int IdCondicaoPagamento { get; set; }

    public int? IdTransportadora { get; set; }

    public DateTime DataPedido { get; set; }

    public string TipoFrete { get; set; } = null!;

    public decimal ValorFrete { get; set; }

    public decimal TotalProdutos { get; set; }

    public decimal TotalIpi { get; set; }

    public decimal TotalIcms { get; set; }

    public decimal TotalPedido { get; set; }

    public string? Observacoes { get; set; }

    public int IdUsuarioCadastro { get; set; }

    public DateTime DataCadastro { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    public DateTime? DataAlteracao { get; set; }
}
