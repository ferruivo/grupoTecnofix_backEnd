using System.Threading.Tasks;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IPdfService
    {
        Task<string> GeneratePdfBase64Async<T>(T model);
    }
}
