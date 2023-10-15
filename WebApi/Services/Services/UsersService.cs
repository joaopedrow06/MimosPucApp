using Microsoft.EntityFrameworkCore;
using Models.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Services
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext _context;
        public UsersService(AppDbContext context) {  _context = context; }
        public async Task<ServiceResponse<Users>> CreateRequest(Users User)
        {
            var ServiceResponse = new ServiceResponse<Users>();

            try
            {

                var newUser = _context.Users.Add(User);
                await _context.SaveChangesAsync();
                await _context.SaveChangesAsync();
                ServiceResponse.Message = "Registro Criado com Sucesso";
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
                throw;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<Users>> DeleteRequest(long UserId)
        {
            var ServiceResponse = new ServiceResponse<Users>();
            try
            {
                Users? User = await _context.Users!.FirstOrDefaultAsync(z => z.Id == UserId);
                if(User is not null)
                {
                    _context.Remove(User);
                    await _context.SaveChangesAsync();
                    ServiceResponse.Message = "Usere removido com sucesso.";
                }
                else
                {
                    ServiceResponse.Success = false;
                    ServiceResponse.Message = "Usere informado não é um Usere válido, por favor tente novamente.";
                }
            }
            catch(Exception ex) 
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<List<Users>>> GetAll()
        {
            var ServiceResponse = new ServiceResponse<List<Users>>();
            try
            {
                ServiceResponse.Data = await _context.Users!.ToListAsync();
                ServiceResponse.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<Users>> GetById(long UserId)
        {
            var ServiceResponse = new ServiceResponse<Users>();
            try
            {
                ServiceResponse.Data =  await _context.Users!.FirstOrDefaultAsync(q => q.Id == UserId);
                ServiceResponse.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        //public async Task<ServiceResponse<Users>> UpdateRequest(Users User)
        //{
        //    var ServiceResponse = new ServiceResponse<Users>();

        //    var valid = isValid(User);
        //    if (valid)
        //    {
        //        try
        //        {
        //            var context = await _context.Users!.FirstOrDefaultAsync(q => q.Id.Equals(User.Id));
        //            if(context is not null)
        //            {
        //                var AlreadyExists = Exists(User);
        //                if (AlreadyExists)
        //                {
        //                    ServiceResponse.Success = false;
        //                    ServiceResponse.Message = "Um User com esse nome já existe para esse Tutor.";
        //                    return ServiceResponse;
        //                }
        //                context.Name = User.Name;
        //            }
        //            else
        //            {
        //                ServiceResponse.Success = false;
        //                ServiceResponse.Message = "Erro ao atualizar o registro, tente novamente.";
        //                return ServiceResponse;
        //            }
        //            await _context.SaveChangesAsync();
        //            ServiceResponse.Message = "Registro atualizado com Sucesso";
        //        }
        //        catch (Exception ex)
        //        {
        //            ServiceResponse.Success = false;
        //            ServiceResponse.Message = ex.Message;
        //            throw;
        //        }
        //    }
        //    else
        //    {
        //        ServiceResponse.Success = false;
        //        if (User is null)
        //        {
        //            ServiceResponse.Message = "Por favor, preencha todos os dados do Usere e tente novamente.";
        //        }
        //        else
        //        {
        //            if (User.Name == "")
        //            {
        //                ServiceResponse.Message = "Por favor, informe um nome.";
        //            }
        //        }
        //    }
        //    return ServiceResponse;
        //}
        //private bool isValid(Users User)
        //{
        //    if (User == null)
        //    {
        //        return false;
        //    }
        //    if (User.Name ==  "")
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        //private bool Exists(Users User)
        //{
        //    var Exists = _context.ClientsUsers.Any(q => (q.UserId == User.Id && q.ClientId == User.ClientId && q.User.Name == User.Name)
        //    || (q.ClientId == User.ClientId && q.User.Name == User.Name));
        //    return Exists;
        //}

    }
}
