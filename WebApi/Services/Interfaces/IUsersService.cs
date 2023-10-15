namespace WebApi.Services.Interfaces
{
    public interface IUsersService
    {
        Task<ServiceResponse<Users>> CreateRequest(Users User);
        //Task<ServiceResponse<Users>> UpdateRequest(Users User);
        Task<ServiceResponse<Users>> DeleteRequest(long UserId);
        Task<ServiceResponse<List<Users>>> GetAll();
        Task<ServiceResponse<Users>> GetById(long UserId);
    }
}
