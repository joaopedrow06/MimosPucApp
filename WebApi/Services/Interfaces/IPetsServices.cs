namespace WebApi.Services.Interfaces
{
    public interface IPetsService
    {
        Task<ServiceResponse<Pets>> CreateRequest(Pets Pet);
        Task<ServiceResponse<Pets>> UpdateRequest(Pets Pet);
        Task<ServiceResponse<Pets>> DeleteRequest(long PetId);
        Task<ServiceResponse<List<Pets>>> GetAll();
        Task<ServiceResponse<Pets>> GetById(long PetId);
        Task<ServiceResponse<List<Pets>>> GetPetsByClientId(long clientId);
    }
}
