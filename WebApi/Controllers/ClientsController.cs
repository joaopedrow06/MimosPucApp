using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;

    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Clients>>> GetAll()
    {
        try
        {
            return Ok(await _clientsService.GetAll());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpGet("/api/[controller]/GetClientsPetsGroupedByClientId")]
    public async Task<ActionResult<IEnumerable<ClientsPets>>> GetAllClientsPets()
    {
        try
        {
            return Ok(await _clientsService.GetAllClientsPetsGroupByClient());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpGet("{id:long:min(1)}")]
    public async Task<ActionResult<IEnumerable<Clients>>> GetById([FromRoute] long id)
    {
        try
        {
            return Ok(await _clientsService.GetById(id));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("/api/[controller]/GetInfosByClientId/{id:long:min(1)}")]
    public async Task<ActionResult<IEnumerable<ClientsPets>>> GetInfosByClientId([FromRoute] long id)
    {
        try
        {
            return Ok(await _clientsService.GetClientsDataByClientId(id));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpGet("/api/[controller]/GetClientPetByClientAndPetId/{clientId:long:min(1)}/{petId:long:min(1)}")]
    public async Task<ActionResult<IEnumerable<ClientsPets>>> GetClientPetByClientAndPetId([FromRoute] long clientId, [FromRoute] long petId)
    {
        try
        {
            return Ok(await _clientsService.GetClientPetByClientAndPetId(clientId,petId));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult<IEnumerable<Clients>>> Delete([FromRoute] long id)
    {
        try
        {
            return Ok(await _clientsService.DeleteRequest(id));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<Clients>>> Post([Required] Clients client)
    {
        try
        {
            var clientResponse = await _clientsService.CreateRequest(client);
            return Ok(clientResponse);
        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost("/api/[controller]/createRelationshipClientPet")]
    public async Task<ActionResult<ServiceResponse<Clients>>> CreateRelationshipClientPet([Required] ClientsPets clientPet)
    {
        if (clientPet.ClientId == 0 || clientPet.PetId == 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Cliente ou Pet invalidos.");
        }
        try
        {
            var clientResponse = await _clientsService.CreateRelationshiopClientPet(clientPet.ClientId, clientPet.PetId);
            return Ok(clientResponse);
        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpPut("{id:long:min(1)}")]
    public async Task<IActionResult> Put([FromRoute] long id, [FromBody] Clients client)
    {
        ServiceResponse<Clients> response;
        if (id != client.Id)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Bad Request");
        }
        try
        {
            response = await _clientsService.UpdateRequest(client);
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}

