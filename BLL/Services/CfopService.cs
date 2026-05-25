using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.Cfop;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class CfopService : ICfopService
    {
        private readonly ICfopRepository _repository;

        public CfopService(ICfopRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CfopDto>> GetAllAsync(CancellationToken ct)
        {
            var list = await _repository.GetAllAsync(ct);
            return list.Select(ToDto).ToList();
        }

        public async Task<List<CfopLookupDto>> GetListAsync(string? search, CancellationToken ct)
        {
            var list = await _repository.GetListAsync(search, ct);

            return list.Select(x => new CfopLookupDto
            {
                Id = x.IdCfop,
                Cfop = x.Cfop1,
                Descricao = x.Descricao
            }).ToList();
        }

        public async Task<CfopDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            return entity == null ? null : ToDto(entity);
        }

        public async Task<CfopDto> CreateAsync(CfopCreateUpdateDto dto, CancellationToken ct)
        {
            Validate(dto);

            var cfop = dto.Cfop.Trim();

            if (await _repository.ExistsCfopAsync(cfop, null, ct))
                throw new InvalidOperationException("Já existe um CFOP cadastrado com esse código.");

            var entity = new Cfop
            {
                Cfop1 = cfop,
                Descricao = dto.Descricao.Trim(),
                TipoOperacao = dto.TipoOperacao.Trim().ToUpper(),
                Venda = NormalizeSN(dto.Venda),
                Icms = NormalizeSN(dto.Icms),
                Obs = string.IsNullOrWhiteSpace(dto.Obs) ? null : dto.Obs.Trim(),
                Devolucao = NormalizeSN(dto.Devolucao),
                Interestadual = NormalizeSN(dto.Interestadual),
                DataCadastro = DateTime.Now
            };

            await _repository.AddAsync(entity, ct);
            await _repository.SaveChangesAsync(ct);

            return ToDto(entity);
        }

        public async Task<CfopDto?> UpdateAsync(int id, CfopCreateUpdateDto dto, CancellationToken ct)
        {
            Validate(dto);

            var entity = await _repository.GetByIdAsync(id, ct);

            if (entity == null)
                return null;

            var cfop = dto.Cfop.Trim();

            if (await _repository.ExistsCfopAsync(cfop, id, ct))
                throw new InvalidOperationException("Já existe um CFOP cadastrado com esse código.");

            entity.Cfop1 = cfop;
            entity.Descricao = dto.Descricao.Trim();
            entity.TipoOperacao = dto.TipoOperacao.Trim().ToUpper();
            entity.Venda = NormalizeSN(dto.Venda);
            entity.Icms = NormalizeSN(dto.Icms);
            entity.Obs = string.IsNullOrWhiteSpace(dto.Obs) ? null : dto.Obs.Trim();
            entity.Devolucao = NormalizeSN(dto.Devolucao);
            entity.Interestadual = NormalizeSN(dto.Interestadual);
            entity.DataAlteracao = DateTime.Now;

            _repository.Update(entity);
            await _repository.SaveChangesAsync(ct);

            return ToDto(entity);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            _repository.Delete(entity);
            await _repository.SaveChangesAsync(ct);

            return true;
        }

        private static CfopDto ToDto(Cfop entity)
        {
            return new CfopDto
            {
                IdCfop = entity.IdCfop,
                Cfop = entity.Cfop1,
                Descricao = entity.Descricao,
                TipoOperacao = entity.TipoOperacao,
                Venda = entity.Venda,
                Icms = entity.Icms,
                Obs = entity.Obs,
                Devolucao = entity.Devolucao,
                Interestadual = entity.Interestadual,
                DataCadastro = entity.DataCadastro,
                DataAlteracao = entity.DataAlteracao
            };
        }

        private static void Validate(CfopCreateUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Cfop))
                throw new ArgumentException("CFOP é obrigatório.");

            if (dto.Cfop.Trim().Length != 4)
                throw new ArgumentException("CFOP deve conter exatamente 4 caracteres.");

            if (!dto.Cfop.All(char.IsDigit))
                throw new ArgumentException("CFOP deve conter somente números.");

            if (string.IsNullOrWhiteSpace(dto.Descricao))
                throw new ArgumentException("Descrição é obrigatória.");

            if (dto.Descricao.Trim().Length > 100)
                throw new ArgumentException("Descrição deve conter no máximo 100 caracteres.");

            if (string.IsNullOrWhiteSpace(dto.TipoOperacao))
                throw new ArgumentException("Tipo da operação é obrigatório.");

            var tipoOperacao = dto.TipoOperacao.Trim().ToUpper();

            if (tipoOperacao != "E" && tipoOperacao != "S")
                throw new ArgumentException("Tipo da operação deve ser E para Entrada ou S para Saída.");

            if (!string.IsNullOrWhiteSpace(dto.Obs) && dto.Obs.Trim().Length > 150)
                throw new ArgumentException("Observação deve conter no máximo 150 caracteres.");
        }

        private static string? NormalizeSN(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            value = value.Trim().ToUpper();

            if (value != "S" && value != "N")
                throw new ArgumentException("Campos Sim/Não devem receber S ou N.");

            return value;
        }
    }
}