using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class PedidosCompraIten
{
    public int IdPedidoCompraItem { get; set; }

    public int IdPedidoCompra { get; set; }

    public int IdProduto { get; set; }

    public decimal Quantidade { get; set; }

    public decimal PrecoUnitario { get; set; }

    public decimal AliquotaIpi { get; set; }

    public decimal ValorIpi { get; set; }

    public decimal AliquotaIcms { get; set; }

    public decimal ValorIcms { get; set; }

    public decimal TotalItem { get; set; }

    public int IdUsuarioCadastro { get; set; }

    public DateTime DataCadastro { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    public DateTime? DataAlteracao { get; set; }
}
