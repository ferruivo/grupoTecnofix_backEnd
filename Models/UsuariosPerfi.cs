using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class UsuariosPerfi
{
    public int IdUsuario { get; set; }

    public int IdPerfil { get; set; }

    public DateTime DataCadastro { get; set; }

    public int? IdUsuarioCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    public virtual Perfi IdPerfilNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
