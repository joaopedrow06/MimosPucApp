using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentsService _AppointmentsService;

    public AppointmentsController(IAppointmentsService AppointmentsService)
    {
        _AppointmentsService = AppointmentsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointments>>> GetAll()
    {
        try
        {
            return Ok(await _AppointmentsService.GetAll());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpGet("/api/[controller]/GetAllFutures")]
    public async Task<ActionResult<IEnumerable<Appointments>>> GetAllFutures()
    {
        try
        {
            return Ok(await _AppointmentsService.GetAllFutures());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpGet("/api/[controller]/GetHistoryPetShop")]
    public async Task<ActionResult<IEnumerable<TransactionHistories>>> GetHistoryPetshop()
    {
        try
        {
            return Ok(await _AppointmentsService.GetAppointmentsPetShopHistories());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpGet("/api/[controller]/GetHistoryByClientId/{id:long:min(1)}")]
    public async Task<ActionResult<IEnumerable<TransactionHistories>>> GetHistoryByClientId(long id)
    {
        try
        {
            return Ok(await _AppointmentsService.GetHistoryByClientId(id));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpGet("/api/[controller]/GetHistoryVet")]
    public async Task<ActionResult<IEnumerable<TransactionHistories>>> GetHistoryVet()
    {
        try
        {
            return Ok(await _AppointmentsService.GetAppointmentsVetHistories());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpGet("{id:long:min(1)}")]
    public async Task<ActionResult<IEnumerable<Appointments>>> GetById([FromRoute] long id)
    {
        try
        {
            return Ok(await _AppointmentsService.GetById(id));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult<IEnumerable<Appointments>>> Delete([FromRoute] long id)
    {
        try
        {
            return Ok(await _AppointmentsService.DeleteRequest(id));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<Appointments>>> Post([Required] Appointments Appointment)
    {
        try
        {
            var AppointmentResponse = await _AppointmentsService.CreateRequest(Appointment);
            return Ok(AppointmentResponse);
        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpPut("{id:long:min(1)}")]
    public async Task<IActionResult> Put([FromRoute] long id, [FromBody] Appointments Appointment)
    {
        ServiceResponse<Appointments> response;
        if (id != Appointment.Id)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Bad Request");
        }
        try
        {
            response = await _AppointmentsService.UpdateRequest(Appointment);
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpGet("UpdateStates")]
    public async Task<ActionResult<IEnumerable<Appointments>>> UpdateStates()
    {
        try
        {
            return Ok(await _AppointmentsService.UpdateAppointments());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpGet("AppointmentCanceled/{id:long:min(1)}")]
    public async Task<ActionResult<IEnumerable<Appointments>>> UpdateByAppointmentId([FromRoute] long id)
    {
        try
        {
            return Ok(await _AppointmentsService.AppointmentCanceled(id));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}

