using Microsoft.EntityFrameworkCore;
using Models.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Services
{
    public class AppointmentsService : IAppointmentsService
    {
        private readonly AppDbContext _context;
        public AppointmentsService(AppDbContext context) {  _context = context; }

        public async Task<ServiceResponse<Appointments>> CreateRequest(Appointments Appointment)
        {
            var ServiceResponse = new ServiceResponse<Appointments>();

            var valid = isValid(Appointment);
            if(valid)
            {
                try
                {
                    var ExistsAppointment = Exists(Appointment);
                    if (ExistsAppointment)
                    {
                        ServiceResponse.Success = false;
                        ServiceResponse.Message = "Já existe um apontamente em aberto para esse cliente e este tipo de serviço.";
                        return ServiceResponse;
                    }
                    var newClientPet = new Appointments
                    {
                        ClientPetId = Appointment.ClientPetId,
                        AppointmentName = Appointment.AppointmentName,
                        Date = Appointment.Date,
                        AppointmentIsComplete = false,
                    };
                    _context.Appointments.Add(newClientPet);
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
                if(Appointment is null)
                {
                    ServiceResponse.Message = "Por favor, preencha todos os dados do Appointmente e tente novamente.";
                }
                else
                {
                    if(Appointment.AppointmentNameEnumString == "")
                    {
                        ServiceResponse.Message = "Por favor, informe um tipo de apontamento.";
                    }
                    if (Appointment.Date != DateTime.Now)
                    {
                        ServiceResponse.Message = "Por favor, informe uma data válida.";
                    }
                    if (Appointment.ClientPetId == 0)
                    {
                        ServiceResponse.Message = "Por favor, informe um cliente válido.";
                    }
                }
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<Appointments>> DeleteRequest(long AppointmentId)
        {
            var ServiceResponse = new ServiceResponse<Appointments>();
            try
            {
                Appointments? Appointment = await _context.Appointments!.FirstOrDefaultAsync(z => z.Id == AppointmentId);
                if(Appointment is not null)
                {
                    _context.Remove(Appointment);
                    await _context.SaveChangesAsync();
                    ServiceResponse.Message = "Appointmente removido com sucesso.";
                }
                else
                {
                    ServiceResponse.Success = false;
                    ServiceResponse.Message = "Appointmente informado não é um Appointmente válido, por favor tente novamente.";
                }
            }
            catch(Exception ex) 
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<List<Appointments>>> GetAll()
        {
            var ServiceResponse = new ServiceResponse<List<Appointments>>();
            try
            {
                var appointments = await _context.Appointments!.ToListAsync();
                foreach(var q in appointments)
                {
                    q.ClientPet = _context.ClientsPets.Where(z => z.Id == q.ClientPetId).FirstOrDefault();
                    q.ClientPet.Client = _context.Clients.Where(z => z.Id == q.ClientPet.ClientId).FirstOrDefault();
                    q.ClientPet.Pet = _context.Pets.Where(z => z.Id == q.ClientPet.PetId).FirstOrDefault();
                }

                ServiceResponse.Data = appointments;
                ServiceResponse.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<List<Appointments>>> GetAllFutures()
        {
            var ServiceResponse = new ServiceResponse<List<Appointments>>();
            try
            {
                await UpdateAppointments();
                var appointments = await _context.Appointments!.ToListAsync();
                foreach (var q in appointments)
                {
                    q.ClientPet = _context.ClientsPets.Where(z => z.Id == q.ClientPetId).FirstOrDefault();
                    q.ClientPet.Client = _context.Clients.Where(z => z.Id == q.ClientPet.ClientId).FirstOrDefault();
                    q.ClientPet.Pet = _context.Pets.Where(z => z.Id == q.ClientPet.PetId).FirstOrDefault();
                }
                ServiceResponse.Data = appointments
                .Where(z => z.Date >= DateTime.Now).ToList();
                ServiceResponse.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<Appointments>> GetById(long AppointmentId)
        {
            var ServiceResponse = new ServiceResponse<Appointments>();
            try
            {
                ServiceResponse.Data =  await _context.Appointments!.FirstOrDefaultAsync(q => q.Id == AppointmentId);
                ServiceResponse.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<Appointments>> UpdateRequest(Appointments Appointment)
        {
            var ServiceResponse = new ServiceResponse<Appointments>();

            var valid = isValid(Appointment);
            if (valid)
            {
                try
                {
                    var context = await _context.Appointments!.FirstOrDefaultAsync(q => q.Id.Equals(Appointment.Id));
                    if(context is not null)
                    {
                        context.AppointmentName = Appointment.AppointmentName;
                        context.Date = Appointment.Date;
                    }
                    else
                    {
                        ServiceResponse.Success = false;
                        ServiceResponse.Message = "Erro ao atualizar o registro, tente novamente.";
                        return ServiceResponse;
                    }
                    //if(Appointment.AppointmentIsComplete)
                    //{
                    //    var q = await _context.TransactionHistories!.Where(q => q.AppointmentId == Appointment.Id).FirstOrDefaultAsync();
                    //    if(q is null)
                    //    {
                    //        var Transaction = new TransactionHistories()
                    //        {
                    //            TransactionDate = Appointment.Date,
                    //            AppointmentId = Appointment.Id,
                    //            WasCanceled = Appointment.WasCanceled,
                    //        };
                    //        _context.TransactionHistories!.Add(Transaction);
                    //        await _context.SaveChangesAsync();
                    //    }
                    //}
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
                if (Appointment is null)
                {
                    ServiceResponse.Message = "Por favor, preencha todos os dados do Appointmente e tente novamente.";
                }
                else
                {
                    if (Appointment.AppointmentNameEnumString == "")
                    {
                        ServiceResponse.Message = "Por favor, informe um tipo de apontamento.";
                    }
                    if (Appointment.Date != DateTime.Now)
                    {
                        ServiceResponse.Message = "Por favor, informe uma data válida.";
                    }
                    if (Appointment.ClientPetId == 0)
                    {
                        ServiceResponse.Message = "Por favor, informe um cliente válido.";
                    }
                }
            }
            return ServiceResponse;
        }
        public async Task<ServiceResponse<List<TransactionHistories>>> GetAppointmentsPetShopHistories()
        {
            var Response = new ServiceResponse<List<TransactionHistories>>();
            try
            {
                await UpdateAppointments();
                var Transactions = await _context.TransactionHistories!.ToListAsync();
                foreach(var i in Transactions)
                {
                    i.Appointment = _context.Appointments.Where(z => z.Id == i.AppointmentId).FirstOrDefault();
                    i.Appointment.ClientPet = _context.ClientsPets.Where(z => z.Id == i.Appointment.ClientPetId).FirstOrDefault();
                    i.Appointment.ClientPet.Client = _context.Clients.Where(z => z.Id == i.Appointment.ClientPet.ClientId).FirstOrDefault();
                    i.Appointment.ClientPet.Pet = _context.Pets.Where(z => z.Id == i.Appointment.ClientPet.PetId).FirstOrDefault();
                }
                Response.Data = Transactions;
                Response.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                Response.Success = false;
                Response.Message = ex.Message;  
            }
            return Response;
        }
        public async Task<ServiceResponse<List<TransactionHistories>>> GetHistoryByClientId(long clientId)
        {
            var Response = new ServiceResponse<List<TransactionHistories>>();
            try
            {
                var ClientPet = await _context.ClientsPets.Where(z => z.ClientId == clientId).ToListAsync();
                if(ClientPet is not null)
                {
                    var Appointments = await _context.Appointments.Where(z => ClientPet.Select(z => z.Id).Contains(z.ClientPetId)).Select(x => x.Id).ToListAsync();
                    if(Appointments is not null)
                    {
                        var Transactions = await _context.TransactionHistories!.Where(q => Appointments.Contains(q.AppointmentId)).ToListAsync();
                        foreach (var i in Transactions)
                        {
                            i.Appointment = _context.Appointments.Where(z => z.Id == i.AppointmentId).FirstOrDefault();
                            i.Appointment.ClientPet = _context.ClientsPets.Where(z => z.Id == i.Appointment.ClientPetId).FirstOrDefault();
                            i.Appointment.ClientPet.Client = _context.Clients.Where(z => z.Id == i.Appointment.ClientPet.ClientId).FirstOrDefault();
                            i.Appointment.ClientPet.Pet = _context.Pets.Where(z => z.Id == i.Appointment.ClientPet.PetId).FirstOrDefault();
                        }
                        Response.Data = Transactions;
                        Response.Message = "Sucesso.";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Success = false;
                Response.Message = ex.Message;
            }
            return Response;
        }

        public async Task<ServiceResponse<List<TransactionHistories>>> GetAppointmentsVetHistories()
        {
            var Response = new ServiceResponse<List<TransactionHistories>>();
            try
            {
                await UpdateAppointments();
                var Transactions = await _context.TransactionHistories!.ToListAsync();
                foreach (var i in Transactions)
                {
                    i.Appointment = _context.Appointments.Where(z => z.Id == i.AppointmentId).FirstOrDefault();
                    i.Appointment.ClientPet = _context.ClientsPets.Where(z => z.Id == i.Appointment.ClientPetId).FirstOrDefault();
                    i.Appointment.ClientPet.Client = _context.Clients.Where(z => z.Id == i.Appointment.ClientPet.ClientId).FirstOrDefault();
                    i.Appointment.ClientPet.Pet = _context.Pets.Where(z => z.Id == i.Appointment.ClientPet.PetId).FirstOrDefault();
                }
                Response.Data = Transactions;
                Response.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                Response.Success = false;
                Response.Message = ex.Message;
            }
            return Response;
        }
        public async Task<ServiceResponse<List<TransactionHistories>>> UpdateAppointments()
        {
            var Response = new ServiceResponse<List<TransactionHistories>>();
            try
            {
                var appointments =  await _context.Appointments!.Where(z => !z.AppointmentIsComplete && z.Date < DateTime.Now).ToListAsync();
                foreach(var q in appointments)
                {
                    q.AppointmentIsComplete = true;
                    var History = new TransactionHistories()
                    {
                        AppointmentId = q.Id,
                        TransactionDate = q.Date,
                        WasCanceled = false,
                    };
                    _context.TransactionHistories.Add(History);
                }
                await _context.SaveChangesAsync();
                Response.Message = "Sucesso.";
            }
            catch (Exception ex)
            {
                Response.Success = false;
                Response.Message = ex.Message;
            }
            return Response;
        }
        public async Task<ServiceResponse<Appointments>> AppointmentCanceled(long AppointmentId)
        {
            var ServiceResponse = new ServiceResponse<Appointments>();

            try
            {
                var AppointmentDetails = await _context.Appointments.Where(q => q.Id == AppointmentId).FirstOrDefaultAsync();
                if(AppointmentDetails is not null)
                {
                    var q = await _context.TransactionHistories!.Where(q => q.AppointmentId == AppointmentId).FirstOrDefaultAsync();
                    if (q is null)
                    {
                        var Transaction = new TransactionHistories()
                        {
                            TransactionDate = AppointmentDetails.Date,
                            AppointmentId = AppointmentId,
                            WasCanceled = true,
                        };
                        _context.TransactionHistories!.Add(Transaction);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        q.WasCanceled = true;
                    }
                    AppointmentDetails.AppointmentIsComplete = true;
                    await _context.SaveChangesAsync();
                    ServiceResponse.Data = AppointmentDetails;
                    ServiceResponse.Message = "Registro atualizado com Sucesso";
                }
                else
                {
                    ServiceResponse.Success = false;
                    ServiceResponse.Message = "Apontamento não encontrado";
                }
            }
            catch (Exception ex)
            {
                ServiceResponse.Success = false;
                ServiceResponse.Message = ex.Message;
                throw;
            }

            return ServiceResponse;
        }
        private bool isValid(Appointments Appointment)
        {
            if (Appointment == null)
            {
                return false;
            }
            if (Appointment.AppointmentNameEnumString ==  "" || Appointment.Date <= DateTime.Now || Appointment.ClientPetId == 0)
            {
                return false;
            }
            return true;
        }
        private bool Exists(Appointments appointment)
        {
            var NotCompletedAppointments = _context.Appointments.Where(z => !z.AppointmentIsComplete).ToList();
            if(NotCompletedAppointments.Any())
            {
                var exists = NotCompletedAppointments.Any(z => z.ClientPetId == appointment.ClientPetId && z.AppointmentName == appointment.AppointmentName);
                return exists;
            }
            return false;
        }
    }
}
