using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SohatNotebook.Configuration.Messages;
using SohatNotebook.DataService.IConfiguration;
using SohatNotebook.Entities.DbSet;
using SohatNotebook.Entities.DTOs.Generic;
using SohatNotebook.Entities.DTOs.Incoming;
using SohatNotebook.Entities.DTOs.Outcoming.Profile;

namespace SohatNotebook.Api.Controllers.v1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IMapper mapper)
    : BaseController(unitOfWork, userManager, mapper)
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var users = await _unitOfWork.UsersRepository.GetAllAysnc();

        var result = new PageResult<User>
        {
            Content = users.ToList(),
            ResultCount = users.Count(),
        };

        return Ok(result);
    }


    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _unitOfWork.UsersRepository.GetByIdAysnc(id);
        if (user is null)
        {
            var notFoundResult = new Result<ProfileDTO>
            {
                Error = PopulateError
                (
                    404,
                    ErrorMessages.Users.UserNotFound,
                    ErrorMessages.Generic.ObjectNotFound
                ),
            };

            return NotFound(notFoundResult);
        }

        var mappedUser = _mapper.Map<ProfileDTO>(user);

        var result = new Result<ProfileDTO>
        {
            Content = mappedUser
        };

        return Ok(result);
    }


    [HttpPost]
    public async Task<IActionResult> Add([FromBody] UserDTO userDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _mapper.Map<User>(userDTO);

        // var user = new User
        // {
        //     FirstName = userDTO.FirstName,
        //     LastName = userDTO.LastName,
        //     Email = userDTO.Email,
        //     Phone = userDTO.Phone,
        //     DateOfBirth = Convert.ToDateTime(userDTO.DateOfBirth),
        //     Country = userDTO.Country,
        // };

        await _unitOfWork.UsersRepository.AddAysnc(user);

        await _unitOfWork.CompleteAsync();

        var result = new Result<UserDTO>
        {
            Content = userDTO,
        };

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, result);
    }
}
