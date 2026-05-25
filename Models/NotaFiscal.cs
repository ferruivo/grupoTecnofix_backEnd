using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class NotaFiscal
{
    public long IdNotaFiscal { get; set; }

    public long NumeroNota { get; set; }

    public int Serie { get; set; }

    public string Modelo { get; set; } = null!;

    public string? ChaveAcesso { get; set; }

    public string TipoMovimento { get; set; } = null!;

    public int Finalidade { get; set; }

    public string TipoOperacao { get; set; } = null!;

    public DateOnly DataEmissao { get; set; }

    public DateTime? DataSaidaEntrada { get; set; }

    public int? IdDestinatario { get; set; }

    public string? TipoDestinatario { get; set; }

    public int? IdPagamento { get; set; }

    public int IdNaturezaOperacao { get; set; }

    public int? IdTransportadora { get; set; }

    public string Status { get; set; } = null!;

    public string? ProtocoloAutorizacao { get; set; }

    public DateTime? DataAutorizacao { get; set; }

    public string? IdNfe { get; set; }

    public string? TipoFrete { get; set; }

    public string? Antt { get; set; }

    public string? Placa { get; set; }

    public string? UfPlaca { get; set; }

    public string? Especie { get; set; }

    public string? QtdEspecie { get; set; }

    public decimal PesoLiquido { get; set; }

    public decimal PesoBruto { get; set; }

    public decimal BaseIcms { get; set; }

    public decimal ValorIcms { get; set; }

    public decimal BaseIcmsSt { get; set; }

    public decimal ValorIcmsSt { get; set; }

    public decimal ValorProdutos { get; set; }

    public decimal ValorFrete { get; set; }

    public decimal ValorSeguro { get; set; }

    public decimal ValorDesconto { get; set; }

    public decimal ValorDespesasAcessorias { get; set; }

    public decimal ValorIpi { get; set; }

    public decimal ValorPis { get; set; }

    public decimal ValorCofins { get; set; }

    public decimal ValorImportacao { get; set; }

    public decimal BaseCbs { get; set; }

    public decimal ValorCbs { get; set; }

    public decimal BaseIbsUf { get; set; }

    public decimal ValorIbsUf { get; set; }

    public decimal BaseIbsMunicipio { get; set; }

    public decimal ValorIbsMunicipio { get; set; }

    public decimal ValorIs { get; set; }

    public decimal ValorNota { get; set; }

    public string? Observacao { get; set; }

    public string? ObservacaoAdicional { get; set; }

    public string UsuarioEmissao { get; set; } = null!;

    public DateTime? DataCancelamento { get; set; }

    public string? UsuarioCancelamento { get; set; }

    public string? MotivoCancelamento { get; set; }

    public DateTime DataCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public virtual ICollection<NotaFiscalEvento> NotaFiscalEventos { get; set; } = new List<NotaFiscalEvento>();

    public virtual ICollection<NotaFiscalItem> NotaFiscalItems { get; set; } = new List<NotaFiscalItem>();
}
