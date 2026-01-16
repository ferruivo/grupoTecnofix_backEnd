using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[Table("CLIENTES")]
[Index("Nome", Name = "CLIENTES__IDX")]
public partial class Cliente
{
    [Key]
    [Column("ID_CLIENTE")]
    public int IdCliente { get; set; }

    [Column("ID_TIPODOCUMENTO")]
    public int IdTipodocumento { get; set; }

    [Column("CPF")]
    [StringLength(11)]
    [Unicode(false)]
    public string? Cpf { get; set; }

    [Column("CNPJ")]
    [StringLength(14)]
    [Unicode(false)]
    public string? Cnpj { get; set; }

    [Column("INSCRICAO_ESTADUAL")]
    [StringLength(50)]
    [Unicode(false)]
    public string? InscricaoEstadual { get; set; }

    [Column("NOME")]
    [StringLength(150)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Column("FANTASIA")]
    [StringLength(150)]
    [Unicode(false)]
    public string Fantasia { get; set; } = null!;

    [Column("CONTATO")]
    [StringLength(50)]
    [Unicode(false)]
    public string Contato { get; set; } = null!;

    [Column("EMAIL")]
    [StringLength(150)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("CEP")]
    [StringLength(8)]
    [Unicode(false)]
    public string Cep { get; set; } = null!;

    [Column("ENDERECO")]
    [StringLength(150)]
    [Unicode(false)]
    public string Endereco { get; set; } = null!;

    [Column("BAIRRO")]
    [StringLength(50)]
    [Unicode(false)]
    public string Bairro { get; set; } = null!;

    [Column("NUMERO")]
    public int Numero { get; set; }

    [Column("COMPLEMENTO")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Complemento { get; set; }

    [Column("ID_MUNICIPIO")]
    public int IdMunicipio { get; set; }

    [Column("CEP_COBRANCA")]
    [StringLength(8)]
    [Unicode(false)]
    public string? CepCobranca { get; set; }

    [Column("ENDERECO_COBRANCA")]
    [StringLength(150)]
    [Unicode(false)]
    public string? EnderecoCobranca { get; set; }

    [Column("BAIRRO_COBRANCA")]
    [StringLength(50)]
    [Unicode(false)]
    public string? BairroCobranca { get; set; }

    [Column("NUMERO_COBRANCA")]
    public int? NumeroCobranca { get; set; }

    [Column("COMPLEMENTO_COBRANCA")]
    [StringLength(50)]
    [Unicode(false)]
    public string? ComplementoCobranca { get; set; }

    [Column("ID_MUNICIPIO_COBRANCA")]
    public int? IdMunicipioCobranca { get; set; }

    [Column("ID_VENDEDORINTERNO")]
    public int IdVendedorinterno { get; set; }

    [Column("ID_VENDEDOREXTERNO")]
    public int IdVendedorexterno { get; set; }

    [Column("ID_TRANSPORTADORA")]
    public int IdTransportadora { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; }

    [Column("ID_USUARIO_CADASTRO")]
    public int? IdUsuarioCadastro { get; set; }

    [Column("DATA_ALTERACAO")]
    public DateTime? DataAlteracao { get; set; }

    [Column("ID_USUARIO_ALTERACAO")]
    public int? IdUsuarioAlteracao { get; set; }

    [Column("SUFRAMA")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Suframa { get; set; }

    [Column("IPI_BC")]
    public bool IpiBc { get; set; }   

    [Column("OBSERVACAO")]
    [Unicode(false)]
    public string? Observacao { get; set; }  

    [Column("OBSERVACAO_ORDEM_EXPEDICAO")]
    [Unicode(false)]
    public string? ObservacaoOrdemExpedicao { get; set; } 

    [Column("OBSERVACAO_NOTA_FISCAL")]
    [Unicode(false)]
    public string? ObservacaoNotaFiscal { get; set; } 

    [Column("ID_ORIGEM")]
    public int IdOrigem { get; set; }


}
