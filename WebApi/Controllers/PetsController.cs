using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PetsController : ControllerBase
{
    private readonly IPetsService _PetsService;

    public PetsController(IPetsService PetsService)
    {
        _PetsService = PetsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pets>>> GetAll()
    {
        try
        {
            return Ok(await _PetsService.GetAll());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpGet("{id:long:min(1)}")]
    public async Task<ActionResult<IEnumerable<Pets>>> GetById([FromRoute] long id)
    {
        try
        {
            return Ok(await _PetsService.GetById(id));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult<IEnumerable<Pets>>> Delete([FromRoute] long id)
    {
        try
        {
            return Ok(await _PetsService.DeleteRequest(id));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<Pets>>> Post([Required] Pets Pet)
    {
        try
        {
            var PetResponse = await _PetsService.CreateRequest(Pet);
            return Ok(PetResponse);
        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpPut("{id:long:min(1)}")]
    public async Task<IActionResult> Put([FromRoute] long id, [FromBody] Pets Pet)
    {
        ServiceResponse<Pets> response;
        if (id != Pet.Id)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Bad Request");
        }
        try
        {
            response = await _PetsService.UpdateRequest(Pet);
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpGet("GetPetsByClientId/{id:long:min(1)}")]
    public async Task<ActionResult<IEnumerable<Pets>>> GetByClientid([FromRoute] long id)
    {
        try
        {
            return Ok(await _PetsService.GetPetsByClientId(id));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}

