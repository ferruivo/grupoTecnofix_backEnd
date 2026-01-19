using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NomeCompleto { get; set; } = null!;

    public string? NomeExibicao { get; set; }

    public string Email { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string SenhaHash { get; set; } = null!;

    public bool Ativo { get; set; }

    public DateTime DataCadastro { get; set; }

    public int? IdUsuarioCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

   
}
