using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.TipoDocumento;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class ClientesRepository : IClientesRepository
    {
        private readonly AppDbContext _db;

        public ClientesRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<ClienteListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            var query = _db.Clientes.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(c =>
                    c.Fantasia.Contains(s) ||
                    c.Contato.Contains(s)||
                    c.Nome.Contains(s) ||
                    c.Cpf.Contains(s) ||
                    c.Cnpj.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(c => c.Fantasia)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ClienteListDto
                {
                    IdCliente = c.IdCliente,
                    Fantasia = c.Fantasia,
                    Nome = c.Nome,
                    Contato = c.Contato,
                    Cnpj = c.Cnpj,
                    Cpf = c.Cpf,
                    
                    Municipio = (
                        from m in _db.Municipios
                        where m.IdMunicipio == c.IdMunicipio
                        select new MunicipioDto
                        {
                            Nome = m.Nome,
                            UF = m.Uf
                        }
                    ).First()
                })
                .ToListAsync(ct);

            return new PagedResult<ClienteListDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public Task<Cliente?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id, ct);

        public Task<ClienteDto?> GetClienteDtoByIdAsync(int id, CancellationToken ct)
        {
            return _db.Clientes
                .AsNoTracking()
                .Where(c => c.IdCliente == id)
                .Select(c => new ClienteDto
                {
                    IdCliente = c.IdCliente,
                    IdTipodocumento = c.IdTipodocumento,
                    Cpf = c.Cpf,
                    Cnpj = c.Cnpj,
                    InscricaoEstadual = c.InscricaoEstadual,

                    Nome = c.Nome,
                    Fantasia = c.Fantasia,
                    Contato = c.Contato,
                    Email = c.Email,

                    Cep = c.Cep,
                    Endereco = c.Endereco,
                    Bairro = c.Bairro,
                    Numero = c.Numero,
                    Complemento = c.Complemento,
                    IdMunicipio = c.IdMunicipio,

                    CepCobranca = c.CepCobranca,
                    EnderecoCobranca = c.EnderecoCobranca,
                    BairroCobranca = c.BairroCobranca,
                    NumeroCobranca = c.NumeroCobranca,
                    ComplementoCobranca = c.ComplementoCobranca,
                    IdMunicipioCobranca = c.IdMunicipioCobranca,

                    IdVendedorinterno = c.IdVendedorinterno,
                    IdVendedorexterno = c.IdVendedorexterno,
                    IdTransportadora = c.IdTransportadora,

                    // =========================
                    // Subquery: Municipio
                    // =========================
                    Municipio = _db.Municipios
                        .Where(m => m.IdMunicipio == c.IdMunicipio)
                        .Select(m => new MunicipioDto
                        {
                            IdMunicipio = m.IdMunicipio,
                            Nome = m.Nome,
                            UF = m.Uf
                        })
                        .FirstOrDefault(),

                    // =========================
                    // Subquery: Tipo Documento (obrigatório)
                    // =========================
                    TipoDocumento = _db.Tipodocumentos
                        .Where(t => t.IdTipodocumento == c.IdTipodocumento)
                        .Select(t => new TipoDocumentoDto
                        {
                            IdTipoDocumento = t.IdTipodocumento,
                            Descricao = t.Descricao
                        })
                        .First(),

                    // =========================
                    // Subquery: Vendedor Interno + Usuario
                    // =========================
                    VendedorInterno = _db.Vendedores
                        .Where(v => v.IdVendedor == c.IdVendedorinterno)
                        .Select(v => new VendedorDto
                        {
                            IdVendedor = v.IdVendedor,
                            IdUsuario = v.IdUsuario,
                            Interno = v.Interno,
                            Externo = v.Externo,
                            Observacao = v.Observacao ?? "",

                            Usuario = _db.Usuarios
                                .Where(u => u.IdUsuario == v.IdUsuario)
                                .Select(u => new UsuarioDto
                                {
                                    IdUsuario = u.IdUsuario,
                                    NomeCompleto = u.NomeCompleto,
                                    NomeExibicao = u.NomeExibicao,
                                    Login = u.Login,
                                    Email = u.Email,
                                    Ativo = u.Ativo
                                })
                                .FirstOrDefault()!
                        })
                        .FirstOrDefault(),

                    // =========================
                    // Subquery: Vendedor Externo + Usuario
                    // =========================
                    VendedorExterno = _db.Vendedores
                        .Where(v => v.IdVendedor == c.IdVendedorexterno)
                        .Select(v => new VendedorDto
                        {
                            IdVendedor = v.IdVendedor,
                            IdUsuario = v.IdUsuario,
                            Interno = v.Interno,
                            Externo = v.Externo,
                            Observacao = v.Observacao ?? "",

                            Usuario = _db.Usuarios
                                .Where(u => u.IdUsuario == v.IdUsuario)
                                .Select(u => new UsuarioDto
                                {
                                    IdUsuario = u.IdUsuario,
                                    NomeCompleto = u.NomeCompleto,
                                    NomeExibicao = u.NomeExibicao,
                                    Login = u.Login,
                                    Email = u.Email,
                                    Ativo = u.Ativo
                                })
                                .FirstOrDefault()!
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync(ct);
        }

        public Task AddAsync(Cliente entity, CancellationToken ct)
        {
            return _db.Clientes.AddAsync(entity).AsTask();
        }

        public Task SaveAsync(CancellationToken ct)
        {
            try
            {
                return _db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }
    }
}
