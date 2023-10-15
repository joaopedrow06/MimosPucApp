namespace WebApi.Services.Interfaces
{
    public interface IClientsService
    {
        Task<ServiceResponse<Clients>> CreateRequest(Clients client);
        Task<ServiceResponse<Clients>> UpdateRequest(Clients client);
        Task<ServiceResponse<Clients>> DeleteRequest(long clientId);
        Task<ServiceResponse<List<Clients>>> GetAll();
        Task<ServiceResponse<List<ClientsPets>>> GetAllClientsPetsGroupByClient();
        Task<ServiceResponse<Clients>> GetById(long clientId);
        Task<ServiceResponse<ClientsPets>> GetClientsDataByClientId(long clientId);
        Task<ServiceResponse<ClientsPets>>CreateRelationshiopClientPet(long  clientId, long petId);
        Task<ServiceResponse<ClientsPets>> GetClientPetByClientAndPetId(long clientId, long petId);
    }
}
