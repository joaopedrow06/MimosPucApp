using Microsoft.EntityFrameworkCore;
using Models.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Services
{
    public class PetsService : IPetsService
    {
        private readonly AppDbContext _context;
        public PetsService(AppDbContext context) {  _context = context; }
        public async Task<ServiceResponse<List<Pets>>> GetPetsByClientId(long clientId)
        {
            var serviceResponse = new ServiceResponse<List<Pets>>();
            try
            {
                var response = await _context.ClientsPets.Include(z => z.Pet).Where(q => q.ClientId == clientId).ToListAsync();
                if(response is not null)
                {
                    serviceResponse.Data = response.Select(q => q.Pet).ToList();
                    serviceResponse.Message = "Sucesso";
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Erro ao buscar as informações dos Pets vinculados à esse usuário.";
                }
            }
            catch (Exception ex)
            {

                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                throw;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<Pets>> CreateRequest(Pets Pet)
        {
            var ServiceResponse = new ServiceResponse<Pets>();

            var valid = isValid(Pet);
            if(valid)
            {
                try
                {
                    var AlreadyExists = Exists(Pet);
                    if(AlreadyExists)
                    {
                        ServiceResponse.Success = false;
                        ServiceResponse.Message = "Um Pet com esse nome já existe para esse Tutor.";
                        return ServiceResponse;
                    }
                    else
                    {
                        var newPet = _context.Pets.Add(Pet);
                        await _context.SaveChangesAsync();
                        if (newPet != null && Pet.ClientId != 0)
                        {
                            var clientPet = new ClientsPets()
                            {
                                ClientId = Pet.ClientId,
                                PetId = newPet.Entity.Id,
                            };
                            _context.ClientsPets.Add(clientPet);
                        }
                        await _context.SaveChangesAsync();
                        ServiceResponse.Message = "Registro Criado com Sucesso";
                    }
                }
                catch (Exception ex)
                {
                    ServiceResponse.Success = false;
                    ServiceResponse.Message = ex.Message;
                    throw;
                }
            }
            else
            {
                ServiceResponse.Success = false;
                if(Pet is null)
                {
                    ServiceResponse.Message = "Por favor, preencha todos os dados do Pete e tente novamente.";
                }
                else
                {
                    if(Pet.Name == "")
                    {
                        ServiceResponse.Message = "Por favor, informe um nome.";
                    }
                }
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<Pets>> DeleteRequest(long PetId)
        {
            var ServiceResponse = new ServiceResponse<Pets>();
            try
            {
                Pets? Pet = await _context.Pets!.FirstOrDefaultAsync(z => z.Id == PetId);
                if(Pet is not null)
                {
                    _context.Remove(Pet);
                    await _context.SaveChangesAsync();
                    ServiceResponse.Message = "Pete removido com sucesso.";
                }
                else
                {
                    ServiceResponse.Success = false;
                    ServiceResponse.Message = "Pete informado não é um Pete válido, por favor tente novamente.";
                }
            }
            catch(Exception ex) 
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<List<Pets>>> GetAll()
        {
            var ServiceResponse = new ServiceResponse<List<Pets>>();
            try
            {
                ServiceResponse.Data = await _context.Pets!.ToListAsync();
                ServiceResponse.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<Pets>> GetById(long PetId)
        {
            var ServiceResponse = new ServiceResponse<Pets>();
            try
            {
                ServiceResponse.Data =  await _context.Pets!.FirstOrDefaultAsync(q => q.Id == PetId);
                ServiceResponse.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<Pets>> UpdateRequest(Pets Pet)
        {
            var ServiceResponse = new ServiceResponse<Pets>();

            var valid = isValid(Pet);
            if (valid)
            {
                try
                {
                    var context = await _context.Pets!.FirstOrDefaultAsync(q => q.Id.Equals(Pet.Id));
                    if(context is not null)
                    {
                        var AlreadyExists = Exists(Pet);
                        if (AlreadyExists)
                        {
                            ServiceResponse.Success = false;
                            ServiceResponse.Message = "Um Pet com esse nome já existe para esse Tutor.";
                            return ServiceResponse;
                        }
                        context.Name = Pet.Name;
                    }
                    else
                    {
                        ServiceResponse.Success = false;
                        ServiceResponse.Message = "Erro ao atualizar o registro, tente novamente.";
                        return ServiceResponse;
                    }
                    await _context.SaveChangesAsync();
                    ServiceResponse.Message = "Registro atualizado com Sucesso";
                }
                catch (Exception ex)
                {
                    ServiceResponse.Success = false;
                    ServiceResponse.Message = ex.Message;
                    throw;
                }
            }
            else
            {
                ServiceResponse.Success = false;
                if (Pet is null)
                {
                    ServiceResponse.Message = "Por favor, preencha todos os dados do Pete e tente novamente.";
                }
                else
                {
                    if (Pet.Name == "")
                    {
                        ServiceResponse.Message = "Por favor, informe um nome.";
                    }
                }
            }
            return ServiceResponse;
        }
        private bool isValid(Pets Pet)
        {
            if (Pet == null)
            {
                return false;
            }
            if (Pet.Name ==  "")
            {
                return false;
            }
            return true;
        }
        private bool Exists(Pets Pet)
        {
            var Exists = _context.ClientsPets.Any(q => (q.PetId == Pet.Id && q.ClientId == Pet.ClientId && q.Pet.Name == Pet.Name)
            || (q.ClientId == Pet.ClientId && q.Pet.Name == Pet.Name));
            return Exists;
        }

    }
}
