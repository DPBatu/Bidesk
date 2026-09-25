using Bidesk.Application.DTOs.Endpoint;

namespace Bidesk.Application.Abstraction.Services
{
    public interface IEndpointService
    {
        public List<EndpointDto> GetAPIEndpoints(Type type);
    }
}
