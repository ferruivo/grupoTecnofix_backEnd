using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[Table("TOKENS_ATUALIZACAO")]
[Index("IdUsuario", "DataExpiracao", Name = "TOKENS_ATUALIZACAO__IDX_USUARIO")]
public partial class TokensAtualizacao
{
    [Key]
    [Column("ID_TOKEN_ATUALIZACAO")]
    public long IdTokenAtualizacao { get; set; }

    [Column("ID_USUARIO")]
    public int IdUsuario { get; set; }

    [Column("TOKEN_HASH")]
    [StringLength(255)]
    [Unicode(false)]
    public string TokenHash { get; set; } = null!;

    [Column("DATA_EXPIRACAO")]
    public DateTime DataExpiracao { get; set; }

    [Column("DATA_REVOGACAO")]
    public DateTime? DataRevogacao { get; set; }

    [Column("IP_CRIACAO")]
    [StringLength(45)]
    [Unicode(false)]
    public string? IpCriacao { get; set; }

    [Column("USER_AGENT_CRIACAO")]
    [StringLength(200)]
    [Unicode(false)]
    public string? UserAgentCriacao { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; }

    //[ForeignKey("IdUsuario")]
    //[InverseProperty("TokensAtualizacaos")]
    //public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
