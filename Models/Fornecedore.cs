using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[Table("FORNECEDORES")]
[Index("IdMunicipio", Name = "IX_FORNECEDORES_ID_MUNICIPIO")]
[Index("IdPagamento", Name = "IX_FORNECEDORES_ID_PAGAMENTO")]
[Index("IdTransportadora", Name = "IX_FORNECEDORES_ID_TRANSPORTADORA")]
public partial class Fornecedore
{
    [Key]
    [Column("ID_FORNECEDOR")]
    public int IdFornecedor { get; set; }

    [Column("RAZAOSOCIAL")]
    [StringLength(150)]
    [Unicode(false)]
    public string? Razaosocial { get; set; }

    [Column("FANTASIA")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Fantasia { get; set; }

    [Column("CPFCNPJ")]
    [StringLength(14)]
    [Unicode(false)]
    public string Cpfcnpj { get; set; } = null!;

    [Column("IE")]
    [StringLength(20)]
    [Unicode(false)]
    public string Ie { get; set; } = null!;

    [Column("CONTATO")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Contato { get; set; }

    [Column("TELEFONE")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Telefone { get; set; }

    [Column("EMAIL")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("CEP")]
    [StringLength(8)]
    [Unicode(false)]
    public string Cep { get; set; } = null!;

    [Column("ENDERECO")]
    [StringLength(100)]
    [Unicode(false)]
    public string Endereco { get; set; } = null!;

    [Column("NUMERO")]
    [StringLength(10)]
    [Unicode(false)]
    public string Numero { get; set; } = null!;

    [Column("BAIRRO")]
    [StringLength(100)]
    [Unicode(false)]
    public string Bairro { get; set; } = null!;

    [Column("COMPLEMENTO")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Complemento { get; set; }

    [Column("ID_MUNICIPIO")]
    public int IdMunicipio { get; set; }

    [Column("ID_PAGAMENTO")]
    public int? IdPagamento { get; set; }

    [Column("ID_TRANSPORTADORA")]
    public int? IdTransportadora { get; set; }

    [Column("IPI")]
    [StringLength(1)]
    [Unicode(false)]
    public string Ipi { get; set; } = null!;

    [Column("ICMS")]
    [StringLength(1)]
    [Unicode(false)]
    public string Icms { get; set; } = null!;

    [Column("FRETE")]
    [StringLength(3)]
    [Unicode(false)]
    public string? Frete { get; set; }

    [Column("OBS")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Obs { get; set; }

    [Column("USUARIO_CADASTRO")]
    [StringLength(45)]
    [Unicode(false)]
    public string? UsuarioCadastro { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime? DataCadastro { get; set; }

    [Column("USUARIO_ALTERACAO")]
    [StringLength(45)]
    [Unicode(false)]
    public string? UsuarioAlteracao { get; set; }

    [Column("DATA_ALTERACAO")]
    public DateTime? DataAlteracao { get; set; }
}
