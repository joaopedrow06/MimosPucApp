namespace WebApi.Services.Interfaces
{
    public interface IAppointmentsService
    {
        Task<ServiceResponse<Appointments>> CreateRequest(Appointments Appointment);
        Task<ServiceResponse<Appointments>> UpdateRequest(Appointments Appointment);
        Task<ServiceResponse<Appointments>> DeleteRequest(long AppointmentId);
        Task<ServiceResponse<List<Appointments>>> GetAll();
        Task<ServiceResponse<List<Appointments>>> GetAllFutures();
        Task<ServiceResponse<Appointments>> GetById(long AppointmentId);
        Task<ServiceResponse<List<TransactionHistories>>> GetAppointmentsVetHistories();
        Task<ServiceResponse<List<TransactionHistories>>> GetAppointmentsPetShopHistories();
        Task<ServiceResponse<List<TransactionHistories>>> GetHistoryByClientId(long clientId);
        Task<ServiceResponse<List<TransactionHistories>>> UpdateAppointments();
        Task<ServiceResponse<Appointments>> AppointmentCanceled(Appointments Appointment);
    }
}
