using Microsoft.EntityFrameworkCore;
using Models.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Services
{
    public class ClientsService : IClientsService
    {
        private readonly AppDbContext _context;
        public ClientsService(AppDbContext context) { _context = context; }

        public async Task<ServiceResponse<ClientsPets>> CreateRelationshiopClientPet(long clientId, long petId)
        {
            var serviceResponse = new ServiceResponse<ClientsPets>();
            try
            {
                var ClientPet = new ClientsPets()
                {
                    ClientId = clientId,
                    PetId = petId
                };
                _context.ClientsPets!.Add(ClientPet);
                await _context.SaveChangesAsync();
                serviceResponse.Message = "Registro Criado com Sucesso";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                throw;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Clients>> CreateRequest(Clients client)
        {
            var ServiceResponse = new ServiceResponse<Clients>();

            var valid = isValid(client);
            if (valid)
            {
                try
                {
                    var existsClient = Exists(client);
                    if (existsClient)
                    {
                        ServiceResponse.Success = false;
                        ServiceResponse.Message = "Um cliente com esses mesmos dados já existe.";
                        return ServiceResponse;
                    }
                    _context.Clients.Add(client);
                    await _context.SaveChangesAsync();
                    ServiceResponse.Message = "Registro Criado com Sucesso";
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
                if (client is null)
                {
                    ServiceResponse.Message = "Por favor, preencha todos os dados do cliente e tente novamente.";
                }
                else
                {
                    if (client.Name == "")
                    {
                        ServiceResponse.Message = "Por favor, informe um nome.";
                    }
                    if (client.Email == "")
                    {
                        ServiceResponse.Message = "Por favor, informe um Email.";
                    }
                    if (client.CellPhone == "")
                    {
                        ServiceResponse.Message = "Por favor, informe um número de telefone.";
                    }
                }
            }
            return ServiceResponse;
        }

        public async Task<ServiceResponse<Clients>> DeleteRequest(long clientId)
        {
            var ServiceResponse = new ServiceResponse<Clients>();
            try
            {
                Clients? client = await _context.Clients!.FirstOrDefaultAsync(z => z.Id == clientId);
                if (client is not null)
                {
                    _context.Remove(client);
                    await _context.SaveChangesAsync();
                    ServiceResponse.Message = "Cliente removido com sucesso.";
                }
                else
                {
                    ServiceResponse.Success = false;
                    ServiceResponse.Message = "Cliente informado não é um cliente válido, por favor tente novamente.";
                }
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<List<Clients>>> GetAll()
        {
            var ServiceResponse = new ServiceResponse<List<Clients>>();
            try
            {
                ServiceResponse.Data = await _context.Clients!.ToListAsync();
                ServiceResponse.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }

        public async Task<ServiceResponse<Clients>> GetById(long clientId)
        {
            var ServiceResponse = new ServiceResponse<Clients>();
            try
            {
                ServiceResponse.Data = await _context.Clients!.FirstOrDefaultAsync(q => q.Id == clientId);
                ServiceResponse.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<Clients>> UpdateRequest(Clients client)
        {
            var ServiceResponse = new ServiceResponse<Clients>();

            var valid = isValid(client);
            if (valid)
            {
                try
                {
                    var context = await _context.Clients!.FirstOrDefaultAsync(q => q.Id.Equals(client.Id));
                    if (context is not null)
                    {
                        context.Name = client.Name;
                        context.Email = client.Email;
                        context.CellPhone = client.CellPhone;
                        context.CEP = client.CEP;
                        context.HouseNumber = client.HouseNumber;
                        context.Adress = client.Adress;
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
                if (client is null)
                {
                    ServiceResponse.Message = "Por favor, preencha todos os dados do cliente e tente novamente.";
                }
                else
                {
                    if (client.Name == "")
                    {
                        ServiceResponse.Message = "Por favor, informe um nome.";
                    }
                    if (client.Email == "")
                    {
                        ServiceResponse.Message = "Por favor, informe um Email.";
                    }
                    if (client.CellPhone == "")
                    {
                        ServiceResponse.Message = "Por favor, informe um número de telefone.";
                    }
                }
            }
            return ServiceResponse;
        }

        public async Task<ServiceResponse<ClientsPets>> GetClientPetByClientAndPetId(long clientId, long petId)
        {
            var ServiceResponse = new ServiceResponse<ClientsPets>();
            try
            {
                var ClientPet = new ClientsPets();
                ClientPet = await _context.ClientsPets.Where(q => q.ClientId == clientId && q.PetId == petId).FirstOrDefaultAsync();
                if (ClientPet is not null)
                {
                    ClientPet.Pet = await _context.Pets.FirstOrDefaultAsync(q => q.Id == petId);
                    ClientPet.Client = await _context.Clients.FirstOrDefaultAsync(q => q.Id == clientId);
                }

                ServiceResponse.Data = ClientPet;
                ServiceResponse.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<List<ClientsPets>>> GetAllClientsPetsGroupByClient()
        {
            var ServiceResponse = new ServiceResponse<List<ClientsPets>>();
            try
            {

                var data = await _context!.Clients.ToListAsync();

                var ClientsPetsGroupedByClientId = new List<ClientsPets>();

                if (data is not null)
                {
                    foreach (var client in data.DistinctBy(q => q.Id))
                    {
                        var GroupPets = _context!.ClientsPets!.Where(z => z.ClientId == client.Id).Select(z => z.PetId).ToList();

                        var Pets = await _context.Pets.Where(q => GroupPets.Contains(q.Id)).ToListAsync();

                        var ClientPet = new ClientsPets
                        {
                            Client = client,
                            Pets = Pets
                        };
                        ClientsPetsGroupedByClientId.Add(ClientPet);
                    }
                }
                ServiceResponse.Data = ClientsPetsGroupedByClientId;

                ServiceResponse.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<ClientsPets>> GetClientsDataByClientId(long clientId)
        {
            var response = new ServiceResponse<ClientsPets>();
            var data = new ClientsPets();
            try
            {
                var client = await _context.Clients!.Where(q => q.Id == clientId).FirstOrDefaultAsync();
                if (client is not null)
                {
                    data.Client = client;
                    try
                    {
                        var GroupPets = _context!.ClientsPets!.Where(z => z.ClientId == client.Id).Select(z => z.PetId).ToList();

                        var Pets = await _context.Pets.Where(q => GroupPets.Contains(q.Id)).ToListAsync();

                        if (Pets is not null)
                        {
                            data.Pets = Pets;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Success = false;
                        response.Message = ex.Message;
                    }
                }
                response.Data = data;
                response.Message = "Sucesso";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        private bool isValid(Clients client)
        {
            if (client == null)
            {
                return false;
            }
            if (client.Name == "" || client.Email == "" || client.CellPhone == ""
            || client.CEP == "" || client.Adress == "" || client.HouseNumber == "")
            {
                return false;
            }
            return true;
        }
        private bool Exists(Clients client)
        {
            var Clients = _context.Clients.ToList();
            if(Clients.Any())
            {
                var existsClient = Clients.Where(q => q.Name == client.Name && q.CellPhone == client.CellPhone
                && q.Email == client.Email && q.CEP == client.CEP && q.Adress == client.Adress && q.HouseNumber == client.HouseNumber)
                .ToList();
                return existsClient.Any();
            }
            return false;
        }
    }
}
