using GrupoTecnofix_Api.Dtos.Municipios;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrupoTecnofix_Api.Dtos.Empresa
{
    public class EmpresaDto
    {
       
        public int IdEmpresa { get; set; }
        public string RazaoSocial { get; set; } = null!;
        public string NomeFantasia { get; set; } = null!;
        public string Cnpj { get; set; } = null!;
        public string? InscricaoEstadual { get; set; }
        public string Endereco { get; set; } = null!;
        public string Bairro { get; set; } = null!;
        public string Cep { get; set; } = null!;
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public int IdMunicipio { get; set; }
        public MunicipioDto Municipio { get; set; }
        public string? Telefone { get; set; }
        public string Regime { get; set; } = null!;
        public decimal? AliquotaRecIcms { get; set; }
    }
}
