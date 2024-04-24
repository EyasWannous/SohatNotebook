using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SohatNotebook.Configuration.Messages;
using SohatNotebook.DataService.IConfiguration;
using SohatNotebook.Entities.DbSet;
using SohatNotebook.Entities.DTOs.Errors;
using SohatNotebook.Entities.DTOs.Generic;
using SohatNotebook.Entities.DTOs.Incoming.Profile;
using SohatNotebook.Entities.DTOs.Outcoming.Profile;

namespace SohatNotebook.Api.Controllers.v1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProfileController(IUnitOfWork unitOfWork,
    UserManager<IdentityUser> userManager, IMapper mapper) : BaseController(unitOfWork, userManager, mapper)
{
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);
        if (loggedinUser is null)
        {
            var badResult = new Result<ProfileDTO>
            {
                Error = PopulateError
                (
                    400,
                    ErrorMessages.Profile.UserNotFound,
                    ErrorMessages.Generic.TypeBadRequest
                ),
            };

            return BadRequest(badResult);
        }

        var identityId = new Guid(loggedinUser.Id);

        var profile = await _unitOfWork.UsersRepository.GetUserByIdentityIdAsync(identityId);
        if (profile is null)
        {
            var badResult = new Result<ProfileDTO>
            {
                Error = PopulateError
                (
                    400,
                    ErrorMessages.Profile.UserNotFound,
                    ErrorMessages.Generic.TypeBadRequest
                ),
            };

            return BadRequest(badResult);
        }

        var mappedProfile = _mapper.Map<ProfileDTO>(profile);

        var result = new Result<ProfileDTO>
        {
            Content = mappedProfile
        };

        return Ok(result);
    }


    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateProfileDTO updateProfileDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var loggedinUser = await _userManager.GetUserAsync(HttpContext.User);
        if (loggedinUser is null)
        {
            var badResult = new Result<ProfileDTO>
            {
                Error = PopulateError
                (
                    400,
                    ErrorMessages.Profile.UserNotFound,
                    ErrorMessages.Generic.TypeBadRequest
                ),
            };

            return BadRequest(badResult);
        }

        var identityId = new Guid(loggedinUser.Id);

        var profile = await _unitOfWork.UsersRepository.GetUserByIdentityIdAsync(identityId);
        if (profile is null)
        {
            var badResult = new Result<ProfileDTO>
            {
                Error = PopulateError
                (
                    400,
                    ErrorMessages.Profile.UserNotFound,
                    ErrorMessages.Generic.TypeBadRequest
                ),
            };

            return BadRequest(badResult);
        }

        profile.FirstName = updateProfileDTO.FirstName;
        profile.LastName = updateProfileDTO.LastName;
        profile.Address = updateProfileDTO.Address;
        profile.Country = updateProfileDTO.Country;
        profile.Sex = updateProfileDTO.Sex;
        profile.MobileNumber = updateProfileDTO.MobileNumber;
        profile.Phone = updateProfileDTO.Phone;
        profile.UpdateDate = DateTime.UtcNow;

        var didProfileUpdated = await _unitOfWork.UsersRepository.UpdateUserProfileAsync(profile);
        if (!didProfileUpdated)
        {
            var badResult = new Result<ProfileDTO>
            {
                Error = PopulateError
                (
                    500,
                    ErrorMessages.Generic.SometingWentWrong,
                    ErrorMessages.Generic.UnableToProcess
                ),
            };

            return BadRequest(badResult);
        }

        await _unitOfWork.CompleteAsync();

        var mappedProfile = _mapper.Map<ProfileDTO>(profile);

        var result = new Result<ProfileDTO>
        {
            Content = mappedProfile
        };

        return Ok(result);
    }
}