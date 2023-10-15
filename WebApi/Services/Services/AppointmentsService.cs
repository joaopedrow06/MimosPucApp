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
                    var newClientPet = new Appointments
                    {
                        ClientPetId = Appointment.ClientPetId,
                        AppointmentName = Appointment.AppointmentName,
                        Date = Appointment.Date,
                        AppointmentIsComplete = Appointment.AppointmentIsComplete,
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
                ServiceResponse.Data = await _context.Appointments!.Include(q => q.ClientPet.Client)
                .Include(q => q.ClientPet.Pet).ToListAsync();
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
                ServiceResponse.Data = await _context.Appointments!.Include(q => q.ClientPet.Client)
                .Include(q => q.ClientPet.Pet)
                .Where(z => z.Date >= DateTime.Now)
                .ToListAsync();
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
                Response.Data = await _context.TransactionHistories!.Include(z => z.Appointment).Include(q => q.Appointment.ClientPet.Client).Include(q => q.Appointment.ClientPet.Pet).ToListAsync();
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
                Response.Data = await _context.TransactionHistories!.Include(z => z.Appointment).Include(q => q.Appointment.ClientPet.Client).Include(q => q.Appointment.ClientPet.Pet)
                .Where(q => q.Appointment.ClientPet.ClientId == clientId).ToListAsync();
                Response.Message = "Sucesso.";
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
                Response.Data = await _context.TransactionHistories!.Include(z => z.Appointment).Include(q => q.Appointment.ClientPet.Client).Include(q => q.Appointment.ClientPet.Pet).Where(q => q.Appointment.AppointmentNameEnumString == Models.Enums.AppointmentNames.VET.ToString()).ToListAsync();
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
                var appointments =  await _context.Appointments!.Where(z => !z.AppointmentIsComplete).ToListAsync();
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
        public async Task<ServiceResponse<Appointments>> AppointmentCanceled(Appointments Appointment)
        {
            var ServiceResponse = new ServiceResponse<Appointments>();

            var valid = isValid(Appointment);
            if (valid)
            {
                try
                {
                    if (Appointment.WasCanceled)
                    {
                        var q = await _context.TransactionHistories!.Where(q => q.AppointmentId == Appointment.Id).FirstOrDefaultAsync();
                        if (q is null)
                        {
                            var Transaction = new TransactionHistories()
                            {
                                TransactionDate = Appointment.Date,
                                AppointmentId = Appointment.Id,
                                WasCanceled = Appointment.WasCanceled,
                            };
                            _context.TransactionHistories!.Add(Transaction);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            q.WasCanceled = Appointment.WasCanceled;
                        }
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
    }
}
