using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService _UsersService;

    public UsersController(IUsersService UsersService)
    {
        _UsersService = UsersService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Users>>> GetAll()
    {
        try
        {
            return Ok(await _UsersService.GetAll());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpGet("{id:long:min(1)}")]
    public async Task<ActionResult<IEnumerable<Users>>> GetById([FromRoute] long id)
    {
        try
        {
            return Ok(await _UsersService.GetById(id));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult<IEnumerable<Users>>> Delete([FromRoute] long id)
    {
        try
        {
            return Ok(await _UsersService.DeleteRequest(id));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<Users>>> Post([Required] Users User)
    {
        try
        {
            var UserResponse = await _UsersService.CreateRequest(User);
            return Ok(UserResponse);
        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    [HttpPost("ValidateUser")]
    public async Task<ActionResult<ServiceResponse<Users>>> ValidateUser([Required] Users User)
    {
        try
        {
            var UserResponse = await _UsersService.ValidateUser(User);
            return Ok(UserResponse);
        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    //[HttpPut("{id:long:min(1)}")]
    //public async Task<IActionResult> Put([FromRoute] long id, [FromBody] Users User)
    //{
    //    ServiceResponse<Users> response;
    //    if (id != User.Id)
    //    {
    //        return StatusCode(StatusCodes.Status400BadRequest, "Bad Request");
    //    }
    //    try
    //    {
    //        response = await _UsersService.UpdateRequest(User);
    //        return Ok(response);
    //    }
    //    catch (Exception e)
    //    {
    //        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
    //    }
    //}
}

