using Data.Entities;
using Service.Dto;

namespace Service.Interfaces
{
    public interface IExtratoClienteService : IServiceGeneric<ExtratoCliente>
    {
        bool ProcessaPdf(ExtratoPdfDto dto);
    }
}
